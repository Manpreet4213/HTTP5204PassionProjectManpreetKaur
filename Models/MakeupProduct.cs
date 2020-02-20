using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject.Models
{
    public class MakeupProduct
    {
        /*
         One makeup product can be in many stores and one brand can have many makeup products 
         associated with it.
         The things that describe a makeup product are 
                - Name
                - Productiondate
                - Expirydate
                - Price
                - Special Notes


        One makeupproduct  must reference a Brand and a list of stores.
     */
        [Key]
        public int MakeupProductID { get; set; }
        public string MakeupProductName { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Price { get; set; }
        //established as the price of the product (5% HST tax included)
        //price is in cents i.e. (12664cents) = $126.64
        //currency is CANADIAN (cad)
        public string Notes { get; set; }
        // public int HasPic { get; set; }
        // public string PicExtension { get; set; }

        public int HasPic { get; set; }
        public string PicExtension { get; set; }




        //Representing the Many in (One Brand to Many MakeupProducts)        
        public int BrandID { get; set; }
        [ForeignKey("BrandID")]
        public virtual Brand Brand { get; set; }

  

        //Representing the "Many" in (Many MakeupProducts to Many Stores)
        public ICollection<Store> Stores { get; set; }
    }
}