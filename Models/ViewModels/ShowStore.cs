using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class ShowStore
    {

        //one individual Store
        public virtual Store store { get; set; }
        //a list for every makeupproduct they own 
        public List<MakeupProduct> makeupProducts { get; set; }

        //a SEPARATE list for representing the ADD of an store to a makeupproduct,
        //for showing a dropdownlist of all makeupproducts, with cta "Add MakeupProduct" on Show Store etc.
        public List<MakeupProduct> all_makeupProducts { get; set; }

    }
}