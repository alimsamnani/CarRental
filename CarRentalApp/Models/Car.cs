using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Car
    {
        
        public string carId { get; set; }
        public int categoryId { get; set; } // category of car Sedan/SUV/etc
        public string carName { get; set; }
        public string carModel { get; set; }
        public string carMake { get; set; }
        public int carDoors { get; set; }
        public int carSeats { get; set; }
        public bool carAC { get; set; }
        public string carType { get; set; } //Automatic or manual
        public int rentalCharge { get; set; }
        public string mileageAllowed { get; set; }
        public bool carStatus { get; set; }
        public DateTime dateAdded { get; set; }
        public string addedBy { get; set; }
        public string modifiedBy { get; set; }
        public DateTime modifiedOn { get; set; }
        public int BufferDays { get; set; }
    }
}