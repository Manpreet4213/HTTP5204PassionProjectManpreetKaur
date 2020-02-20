using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PassionProject.Data
{
    public class PassionProjectContext : DbContext
    {
        public PassionProjectContext() : base("name=PassionProjectContext")
        {
        }

       //for describing the database models.
        public System.Data.Entity.DbSet<PassionProject.Models.MakeupProduct> MakeupProducts { get; set; }

        public System.Data.Entity.DbSet<PassionProject.Models.Brand> Brands { get; set; }
        public System.Data.Entity.DbSet<PassionProject.Models.Store> Stores { get; set; }
       
    }
}