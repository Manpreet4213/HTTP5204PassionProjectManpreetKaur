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
//to use viewmodels, we need this Otherwise viewmodels will not be working.
using System.Diagnostics;
using System.IO;
namespace PassionProject.Controllers
{
    public class MakeupProductController : Controller
    {
        private PassionProjectContext db = new PassionProjectContext();
        //to use database
        // GET: MakeupProduct
        public ActionResult List()
        {
            //query to get the list of all the makeupproducts in the system.
            List<MakeupProduct> makeupproducts = db.MakeupProducts.SqlQuery("Select * from MakeupProducts").ToList();
            return View(makeupproducts);

        }

        // GET: details of the makeupproduct with id =id
        public ActionResult Show(int? id)
        {
            //if the id is null, then return this message of the bad request.
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            MakeupProduct MakeupProduct = db.MakeupProducts.SqlQuery("select * from makeupproducts where makeupproductid=@MakeupProductID", new SqlParameter("@MakeupProductID", id)).FirstOrDefault();
            //if id exists, but the makeupproduct does not exist, then return this message.
            if (MakeupProduct == null)
            {
                return HttpNotFound();
            }

            //need information about the list of stores associated with that makeupproduct
            string query = "select * from stores inner join StoreMakeupProducts on Stores.StoreID = StoreMakeupProducts.Store_StoreID where MakeupProduct_MakeupProductID = @id";
            SqlParameter param = new SqlParameter("@id", id);
            List<Store> StoreMakeupProducts = db.Stores.SqlQuery(query, param).ToList();

            //using viewmodel here to get the list of the stores
            ShowMakeupProduct viewmodel = new ShowMakeupProduct();
            viewmodel.makeupProduct = MakeupProduct;
            viewmodel.stores = StoreMakeupProducts;

            
            return View(viewmodel);
        }

        //THE [HttpPost] Means that method will be activated after the post from the following method.
        //URL: /MakeupProduct/Add
        [HttpPost]
        public ActionResult Add(string MakeupProductName, int BrandID, int MakeupProductPrice, DateTime ProductionDate, DateTime ExpiryDate, string MakeupProductNotes)
        {
            //the above are taken from the addmakeupproduct from and should alwasys match.
            //otherwise the following method will not work.

            
            //Debug.WriteLine("Want to create a makeupproduct with name " + MakeupProductName + " and notes " + MakeupProductNotes) ;
            //the debug writeline is for testing, we don't need all the parameters.Some of them are fine.
            
            //query to add a new makeupproduct into the database.
            string query = "insert into makeupproducts (MakeupProductName, BrandID, ProductionDate, ExpiryDate, Price, Notes) values (@MakeupProductName, @BrandID, @ProductionDate, @ExpiryDate, @MakeupProductPrice, @MakeupProductNotes)";
            SqlParameter[] sqlparams = new SqlParameter[6]; //0,1,2,3,4,5 pieces of information to add
            //here each piece is represented as the key-value pairs.
            sqlparams[0] = new SqlParameter("@MakeupProductName", MakeupProductName);
            sqlparams[1] = new SqlParameter("@ProductionDate", ProductionDate);
            sqlparams[2] = new SqlParameter("@ExpiryDate", ExpiryDate);
            sqlparams[3] = new SqlParameter("@MakeupProductPrice", MakeupProductPrice);
            sqlparams[4] = new SqlParameter("@MakeupProductNotes", MakeupProductNotes);
            sqlparams[5] = new SqlParameter("@BrandID", BrandID);
            
            //db.Pets.SqlCommand to run a select statement
            db.Database.ExecuteSqlCommand(query, sqlparams);


            //runing the list method to return to a list of pets so as to look our newly added makeupproduct.
            return RedirectToAction("List");
        }


        public ActionResult New()
        {
            //this is get the information that we want to provide the user in order
            //for the addition of the makeupproduct into the system.We need brandnames in the 
            //dropdown list.

            List<Brand> brands = db.Brands.SqlQuery("select * from brands").ToList();

            return View(brands);
        }
        public ActionResult Update(int id)
        {
            //to get information for a makeupproduct that we want to update.
            MakeupProduct selectedmakeupproduct = db.MakeupProducts.SqlQuery("select * from makeupproducts where makeupproductid = @id", new SqlParameter("@id", id)).FirstOrDefault();
            List<Brand> Brands = db.Brands.SqlQuery("select * from brands").ToList();

            UpdateMakeupProduct UpdateMakeupProductViewModel = new UpdateMakeupProduct();
            UpdateMakeupProductViewModel.MakeupProduct = selectedmakeupproduct;
            UpdateMakeupProductViewModel.Brand = Brands;
            //using viewmodel to get the list of all the brands in order to perform updation of the makeupproduct.

            return View(UpdateMakeupProductViewModel);

            //Debug.WriteLine("Want to update a makeupproduct with name " + MakeupProductName + " and notes " + MakeupProductNotes) ;

        }

        [HttpPost]
        public ActionResult Update(int id, string MakeupProductName, int MakeupProductPrice, DateTime MakeupProductProductionDate, DateTime MakeupProductExpiryDate, int BrandID, string MakeupProductNotes, HttpPostedFileBase MakeupProductPic)
        {
            //assume there is no picture

            int haspic = 0;
            string makeupProductPicextension = "";
            //check to get some more information.
            if (MakeupProductPic != null)
            {
                Debug.WriteLine("Something identified...");
                //checking  file size  greater than 0 (bytes)
                if (MakeupProductPic.ContentLength > 0)
                {
                    Debug.WriteLine("Successfully Identified Image");
                    
                    var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                    var extension = Path.GetExtension(MakeupProductPic.FileName).Substring(1);
                    //if extension exists then trying to find the image.
                    if (valtypes.Contains(extension))
                    {
                        try
                        {
                            //file name is the id of the image
                            string fn = id + "." + extension;

                            //get a direct file path 
                            string path = Path.Combine(Server.MapPath("~/Content/MakeupProducts/"), fn);

                            //save the file
                            MakeupProductPic.SaveAs(path);
                            //if these are all successful then we can set these fields
                            haspic = 1;
                            makeupProductPicextension = extension;

                        }
                        //if not sucessful then use following the catch method to show the error.
                        catch (Exception ex)
                        {
                            Debug.WriteLine("MakeupProduct Image was not saved successfully.");
                            Debug.WriteLine("Exception:" + ex);
                        }



                    }
                }
            }

            //Debug.WriteLine("I am trying to edit a makeupproduct's name to "+MakeupProductName+" and change the Notes to "+MakeupProductNotes);
            //query to update the makeupProduct
            string query = "update makeupproducts set MakeupProductName=@MakeupProductName, BrandID=@BrandID, ProductionDate=@MakeupProductProductionDate, ExpiryDate=@MakeupProductExpiryDate, Price=@MakeupProductPrice, Notes=@MakeupProductNotes, HasPic=@haspic, PicExtension=@makeupProductPicextension where MakeupProductID=@id";
            SqlParameter[] sqlparams = new SqlParameter[9];
            //0 to 8 pieces of the information.
            sqlparams[0] = new SqlParameter("@MakeupProductName", MakeupProductName);
            sqlparams[1] = new SqlParameter("@MakeupProductProductionDate", MakeupProductProductionDate);
            sqlparams[2] = new SqlParameter("@MakeupProductExpiryDate", MakeupProductExpiryDate);
            sqlparams[3] = new SqlParameter("@BrandID", BrandID);
            sqlparams[4] = new SqlParameter("@MakeupProductPrice", MakeupProductPrice);
            sqlparams[5] = new SqlParameter("@MakeupProductNotes",MakeupProductNotes);
            sqlparams[6] = new SqlParameter("@id", id);
            sqlparams[7] = new SqlParameter("@HasPic", haspic);
            sqlparams[8] = new SqlParameter("@makeupProductPicextension", makeupProductPicextension);
            
            db.Database.ExecuteSqlCommand(query, sqlparams);

            
            return RedirectToAction("List");
        }


        //for the confirmation of the delete.
        public ActionResult DeleteConfirm(int id)
        {
            string query = "select * from makeupproducts where makeupproductid = @id";
            SqlParameter param = new SqlParameter("@id", id);
            MakeupProduct selectedmakeupproduct = db.MakeupProducts.SqlQuery(query, param).FirstOrDefault();

            return View(selectedmakeupproduct);
        }
        [HttpPost]
        //for deletion after the user confirmation.
        public ActionResult Delete(int id)
        {
            string query = "delete from makeupproducts where makeupproductid = @id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);

            return RedirectToAction("List");
        }

        //Debug.WriteLine("I Want to delete a makeupproduct with id " + id") ;


        //I tried a lot to complete image part.But, due to that access denied problem
        //and the shortage of time i was not able to figure out that.
        //Also, the side panel part of the makeupproduct show page is also not working properly.
        //I checked the query again and again.

    }
}