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
using System.Diagnostics;
// i have taken help from the Christine Bittle's class example.
//here, we are not declaring for the viewmodel as none of the brands page need any viewmodel.

namespace PassionProject.Controllers
{
    public class BrandController : Controller
    {
        private PassionProjectContext db = new PassionProjectContext();
        //the above is used to use database.If we don't declare it.Our,
        //queries will not be working.
        // GET: Brands
        public ActionResult Index()
        {
            return View();
        }

        
        //  For List Page
        public ActionResult List()
        {
            //To  get the list of all the brands in the system, we use following query to show
            //every brand in the system.
            
            List<Brand> mybrands = db.Brands.SqlQuery("Select * from brands").ToList();
            
            return View(mybrands);
            //then in return, returning the view of all the brands in the system on the list page.
        }
        public ActionResult Add()
        {
            //Because we don't need any information to display on the add page like for the user
            //when we create makeupproduct we need brands dropdown.But, here we need nothing to provide
            //to the user.
            return View();
        }
        [HttpPost]
        public ActionResult Add(string BrandName)
        {
            //using insert query for the brands, for only name as id will automatically 
            //given to the new brand by the database system.
            string query = "insert into brands (Name) values (@BrandName)";
            var parameter = new SqlParameter("@BrandName", BrandName);
            //here, @BrandName is coming from the add brand page form that is the value
            //for the new brandname that the user is giving.

            //Debug.WriteLine("I am trying to add a brand's name to "+Name");
            db.Database.ExecuteSqlCommand(query, parameter);
            //this is required for the insert, update and delete queries in the system.
            //we have not used this for the list page.
            return RedirectToAction("List");
            //after adding redirecting the page back to the list brands page.
        }
        public ActionResult Show(int id)
        {
            //query to show a brand with brandid = @id
            string query = "select * from brands where brandid = @id";
            var parameter = new SqlParameter("@id", id);
            Brand selectedbrands = db.Brands.SqlQuery(query, parameter).FirstOrDefault();
            //then, return the brand with brandid = @id
            //although, we are saying here selectedbarnds, but we have one brand with one id.
            //i.e.every brand have their unique brand id.
            
            //Debug.WriteLine("I Want to show a brand with id" + id") ;
            return View(selectedbrands);
        }

        public ActionResult Update(int id)
        {
            //Debug.WriteLine("I Want to show a makeupproduct with id" + id") ;

            //query to select a brand with brandid = @id
            string query = "select * from brands where brandid = @id";
            var parameter = new SqlParameter("@id", id);
            Brand selectedbrand = db.Brands.SqlQuery(query, parameter).FirstOrDefault();
            //here, we have not written selected brands as will be updating only brand
            //by clicking the update button corresponding to that brand.
            return View(selectedbrand);
        }
        [HttpPost]
        public ActionResult Update(int id, string BrandName)
        {
            //Debug.WriteLine("I Want to update a makeupproduct with id" + id") ;

            //query to update a brand with brandid = @id
            string query = "update brands set name = @BrandName where brandid = @id";
            //we, have only two pieces of the information working in the whole process of the updating of a brand.
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@BrandName", BrandName);
            sqlparams[1] = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }
        public ActionResult DeleteConfirm(int id)
        {
            //query to select a brand from the brands with BrandID = @id
            //this is just for showing the delete confirm page for a brand rather then actually deletinga brand.
            //this will ask the user for permission to delete that brand.
            string query = "select * from brands where BrandID=@id";
            SqlParameter param = new SqlParameter("@id", id);
            Brand selectedbrands = db.Brands.SqlQuery(query, param).FirstOrDefault();
            return View(selectedbrands);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //here, will use a delete query to actually delete the brand from the system after 
            //permission or having the confirmation from the user.
            string query = "delete from brands where brandid=@id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);


            //Debug.WriteLine("I Want to delete a brand with id" + id") ;


            //for referential integrity, we will set the brandid of all of the makeupproducts as null
            //whose brandid was same as the brandid being deleted from the system.
            string refquery = "update makeupproducts set BrandID = '' where BrandID=@id";
            db.Database.ExecuteSqlCommand(refquery, param); //same param brand id that was deleted from the system.

            return RedirectToAction("List");
        }

    }
}