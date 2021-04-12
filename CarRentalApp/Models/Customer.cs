using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Customer
    {
        public string customerId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailId { get; set; }
        public string phone { get; set; }
        public string carId { get; set; }

        public string bookingId { get; set; }        
        public string rentTotal { get; set; }
        public string feesTaxes { get; set; }
        public string addedBy { get; set; }
    }
}