using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PassionProject.Models
{
    public class Store
    {
        /*
            A store is a place from where people can buy makeupproducts.
            One store can have many makeupProducts.And, one makeupProduct can be in many stores.
            Some things that describe a Store 
                - Name
                - Address
                - Owner
            
           
            A Store must reference
                - A list of MakeupProducts
                
        */
        [Key]
        public int StoreID { get; set; }
        public string  StoreName { get; set; }
        public string StoreAddress { get; set; }

        public string StoreOwner { get; set; }

        //Representing the Many in (Many Stores to Many MakeupProducts)
        public ICollection<MakeupProduct> MakeupProducts { get; set; }

    }
}
