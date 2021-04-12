using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Rental
    {
        public string Id { get; set; }
        public string carId { get; set; }
        public string customerId { get; set; }
        public string status { get; set; }
        public DateTime dateAdded { get; set; }
        public string addedBy { get; set; }
        public string modifiedBy { get; set; }
        public DateTime modifiedOn { get; set; }
    }
}