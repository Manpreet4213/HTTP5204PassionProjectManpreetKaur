using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PassionProject.Models
{
    public class Brand
    {

        [Key]
        public int BrandID { get; set; }
        public string Name { get; set; }

        //Representing the "Many" in (One brand to many makeupproducts)
        public ICollection<MakeupProduct> MakeupProducts { get; set; }
    }
}