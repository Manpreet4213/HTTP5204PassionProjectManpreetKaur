using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class ShowMakeupProduct
    {
        //for information about an individual makeupproduct
        public virtual MakeupProduct makeupProduct { get; set; }

        //information about multiple stores
        //used to show list of the stores on the pages we require
        
        public List<Store> stores { get; set; }
    }
}