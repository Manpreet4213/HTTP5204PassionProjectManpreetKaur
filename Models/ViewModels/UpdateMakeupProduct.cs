using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject.Models.ViewModels
{
    public class UpdateMakeupProduct
    {
        //when we need to update a makeupproduct
        //we need the makeupproduct info as well as a list of brands

        public MakeupProduct MakeupProduct{ get; set; }
        public List<Brand> Brand { get; set; }
    }
}