using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Users
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string phone { get; set; }
        public string emailId { get; set; }
        public string address { get; set; }
        public DateTime dateAdded { get; set; }
        public string addedBy { get; set; }
        public string modifiedBy { get; set; }
        public DateTime modifiedOn { get; set; }
    }
}