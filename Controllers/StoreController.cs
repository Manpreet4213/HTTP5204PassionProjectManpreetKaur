using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PassionProject.Data;
using PassionProject.Models;
using PassionProject.Models.ViewModels;
//to use viewmodels we need this.
using System.Diagnostics;

namespace PassionProject.Controllers
{
    public class StoreController : Controller
    {
        private PassionProjectContext db = new PassionProjectContext();

        // GET: Store/List
        public ActionResult List()
        {
            //query to return the list of the makeupproducts in the system.
            List<Store> stores = db.Stores.SqlQuery("Select * from stores").ToList();
            return View(stores);

        }

        public ActionResult New()
        {

            return View();
        }


        [HttpPost]
        public ActionResult Add(string Name, string Address, string Owner)
        {
            //for add using query
            string query = "insert into stores (StoreName, StoreAddress, StoreOwner) values (@Name, @Address, @Owner)";

            SqlParameter[] sqlparams = new SqlParameter[3];
            //3 pieces of the information.
            sqlparams[0] = new SqlParameter("@Name", Name);
            sqlparams[1] = new SqlParameter("@Address", Address);
            sqlparams[2] = new SqlParameter("@Owner", Owner);
            
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");

            //Debug.WriteLine("Want to create a store with name " + StoreName + " and address " + storeAddress " + and owner " + StoreOwner") ;
        }


        public ActionResult Show(int id)
        {

            //Debug.WriteLine("I Want to show a Store with id " + id") ;


            //to show data for an individual store
            //query to select a store from the list of the stores.
            string main_query = "select * from stores where StoreID = @id";
            var pk_parameter = new SqlParameter("@id", id);
            Store Store = db.Stores.SqlQuery(main_query, pk_parameter).FirstOrDefault();

            //find data about all makeupproducts that store has (through id)
           
            string aside_query = "select * from MakeupProducts inner join StoreMakeupProducts on MakeupProducts.MakeupProductID = StoreMakeupProducts.MakeupProduct_MakeupProductID where StoreMakeupProducts.Store_StoreID=@id";
            var fk_parameter = new SqlParameter("@id", id);
            List<MakeupProduct> AvailableMakeupProducts = db.MakeupProducts.SqlQuery(aside_query, fk_parameter).ToList();

            string all_makeupProducts_query = "select * from MakeupProducts";
            List<MakeupProduct> AllMakeupProducts = db.MakeupProducts.SqlQuery(all_makeupProducts_query).ToList();

            //using the viewmodel to get all the makeupproducts list available on that store.
            ShowStore viewmodel = new ShowStore();
            viewmodel.store = Store;
            viewmodel.makeupProducts = AvailableMakeupProducts;
            viewmodel.all_makeupProducts = AllMakeupProducts;

            return View(viewmodel);
        }


        // to insert into the bridging table StoreMakeupProducts
        //for adding a makeupProduct to  a store.
        [HttpPost]
        public ActionResult AttachMakeupProduct(int id, int MakeupProductID)
        {
            Debug.WriteLine("store id is"+ id + " and makeupproductid is " + MakeupProductID);

            //first, check if that makeupproduct is already available  on that store
            string check_query = "select * from MakeupProducts inner join StoreMakeupProducts on StoreMakeupProducts.MakeupProduct_MakeupProductID = MakeupProducts.MakeupProductID where MakeupProduct_MakeupProductID=@MakeupProductID and Store_StoreID=@id";
            SqlParameter[] check_params = new SqlParameter[2];
            check_params[0] = new SqlParameter("@id", id);
            check_params[1] = new SqlParameter("@MakeupProductID", MakeupProductID);
            List<MakeupProduct> makeupProducts = db.MakeupProducts.SqlQuery(check_query, check_params).ToList();
            //only executed if the makeupproducts count is less than or equal to 0.
            if (makeupProducts.Count <= 0)
            {


                //first id above is the storeid, then the makeupproductid
                string query = "insert into StoreMakeupProducts (MakeupProduct_MakeupProductID, Store_StoreID) values (@MakeupProductID, @id)";
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@id", id);
                sqlparams[1] = new SqlParameter("@MakeupProductID", MakeupProductID);


                db.Database.ExecuteSqlCommand(query, sqlparams);
            }

            return RedirectToAction("Show/" + id);

        }


        //URL: /Store/DetachMakeupProduct/id?MakeupProductID=pid
        [HttpGet]
        public ActionResult DetachMakeupProduct(int id, int MakeupProductID)
        {
            //query for deleting from the bridging table
            Debug.WriteLine("store id is" + id + " and makeupproductid is " + MakeupProductID);

            string query = "delete from StoreMakeupProducts where MakeupProduct_MakeupProductID=@MakeupProductID and Store_StoreID=@id";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@MakeupProductID", MakeupProductID);
            sqlparams[1] = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("Show/" + id);
        }

        public ActionResult Update(int id)
        {
            //to select the store that we want to update.
            string query = "select * from Stores where StoreID = @id";
            var parameter = new SqlParameter("@id", id);
            Store store = db.Stores.SqlQuery(query, parameter).FirstOrDefault();

            return View(store);
        }


        [HttpPost]
        public ActionResult Update(int id, string StoreName, string StoreAddress, string storeOwner)
        {

            //Debug.WriteLine(" I Want to update a store with name " + StoreName + " and address " + StoreAddress) ;

            //query to update.
            string query = "update stores set StoreName=@StoreName, StoreAddress=@StoreAddress, StoreOwner=@StoreOwner where StoreID = @id";

            SqlParameter[] sqlparams = new SqlParameter[4];

            sqlparams[0] = new SqlParameter("@StoreName", StoreName);
            sqlparams[1] = new SqlParameter("@StoreAddress", StoreAddress);
            sqlparams[2] = new SqlParameter("@StoreOwner", storeOwner);
  
            sqlparams[3] = new SqlParameter("@id", id);


            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }

        //for the confirmation of the delete.
        public ActionResult DeleteConfirm(int id)
        {
            string query = "select * from Stores where StoreID=@id";
            SqlParameter param = new SqlParameter("@id", id);
            Store store = db.Stores.SqlQuery(query, param).FirstOrDefault();
            return View(store);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //query for actual deletion after confirmation.
            string query = "delete from Stores where StoreID=@id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);



            return RedirectToAction("List");
            //Debug.WriteLine("I Want to delete a Store with id " + id") ;
        }
    }
}