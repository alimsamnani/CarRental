using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Booking
    {
        
        public string location { get; set; }
        public string pickUpDate { get; set; } // category of car Sedan/SUV/etc
        public string returnDate { get; set; }
        public string carId { get; set; }
        public string categoryId { get; set; }
    }
}