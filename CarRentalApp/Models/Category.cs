using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Category
    {
        
        public string categoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime dateAdded { get; set; }
        public string addedBy { get; set; }
        public string modifiedBy { get; set; }
        public DateTime modifiedOn { get; set; }
      

    }
}