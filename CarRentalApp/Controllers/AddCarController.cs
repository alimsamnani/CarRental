using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using CarRentalApp.Controllers;
using CarRentalApp.Models;

namespace CarRentalApp.Controllers
{
    public class AddCarController : Controller
    {
        // GET: AddCar
        CarMaster ds = new CarMaster();
        public ActionResult Add()
        {
            var categoryList = ds.category_master.ToList();
            ViewBag.CategoryList = new SelectList(categoryList, "categoryId", "categoryName");
            return View();
        }

        [HttpPost]
        public ActionResult saveData(string CarName, string categoryId, string carMake, string carModel, string CarDoors, string numOfSeats, string hasAC, string carType, string rentalCharge, string mileageAllowed, string bufferDays, HttpPostedFileBase[] files)
        {
            if (CarName != "" && CarName != null)
            {
                Models.Car car = new Models.Car();
                car.carName = CarName;
                car.categoryId = Convert.ToInt32(categoryId);
                car.carMake = carMake;
                car.carModel = carModel;
                car.carDoors = Convert.ToInt32(CarDoors);
                car.carSeats = Convert.ToInt32(numOfSeats);
                car.carAC = Convert.ToBoolean(hasAC);
                car.carType = carType;
                car.rentalCharge = Convert.ToInt32(rentalCharge);
                car.mileageAllowed = mileageAllowed;
                car.BufferDays = Convert.ToInt32(bufferDays);
                CarController c = new CarController();
                string x = c.addCar(car);
                int i = 0;
                List<string> images = new List<string>();
                foreach (HttpPostedFileBase file in files)
                {

                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        string strpath = Path.GetExtension(file.FileName);
                        string fileName = x + "_" + i.ToString() + strpath;
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Image/") + fileName);
                        //Save file to server folder  
                        file.SaveAs(ServerSavePath);
                        images.Add(fileName);

                    }
                    i++;
                }
                c.addCarImages(images, x);

                return RedirectToAction("Cars", "AddCar");

            }
            return View();
        }
        public ActionResult Update(string carId, string CarName, string categoryId, string carMake, string carModel, string CarDoors, string numOfSeats, string hasAC, string carType, string rentalCharge, string mileageAllowed, string bufferDays)
        {

            CarController c = new CarController();
            Models.Car car = new Models.Car();
            car.carId = carId;
            DataTable dt = c.GetCarDetails(car);
            ViewBag.carDetails = dt;
            var categoryList = ds.category_master.ToList();
            ViewBag.CategoryList = new SelectList(categoryList, "categoryId", "categoryName", dt.Rows[0]["categoryid"]);
            Dictionary<String, String> AC = new Dictionary<String, String>();
            AC.Add("true", "Yes");
            AC.Add("false", "No");
            var hasACList = AC;
            ViewBag.ACList = new SelectList(hasACList, "Key", "Value", dt.Rows[0]["carAC"]);

            Dictionary<String, String> carTypeList = new Dictionary<String, String>();
            carTypeList.Add("Automatic", "Automatic");
            carTypeList.Add("Manual", "Manual");
            ViewBag.carType = new SelectList(carTypeList, "Key", "Value", dt.Rows[0]["carType"]);
            if (CarName != "" && CarName != null)
            {
                car = new Car();
                car.carId = carId;
                car.carName = CarName;
                car.categoryId = Convert.ToInt32(categoryId);
                car.carMake = carMake;
                car.carModel = carModel;
                car.carDoors = Convert.ToInt32(CarDoors);
                car.carSeats = Convert.ToInt32(numOfSeats);
                car.carAC = Convert.ToBoolean(hasAC);
                car.carType = carType;
                car.rentalCharge = Convert.ToInt32(rentalCharge);
                car.mileageAllowed = mileageAllowed;
                car.BufferDays = Convert.ToInt32(bufferDays);
                c.editCar(car);

            }
            return View();
        }

        public ActionResult Cars()
        {
            CarController c = new CarController();
            Models.Car car = new Models.Car();
            DataTable dt = c.GetCarDetails(car);
            ViewBag.htmlString = "";
            foreach (DataRow dr in dt.Rows)
            {
                ViewBag.htmlString += @"<tr><th> " + dr["carName"].ToString() + " </th><td>" + dr["category"].ToString() + "</td><td> " + dr["carMake"].ToString() + " </td><td> " + dr["carModel"].ToString() + " </td><td> " + dr["carDoors"].ToString() + " </td><td> " + dr["carSeats"].ToString() + " </td><td> " + dr["carAC"].ToString() + " </td><td> " + dr["carType"].ToString() + " </td><td> " + dr["rentalCharge"].ToString() + " </td><td> " + dr["mileageAllowed"].ToString() + " </td><td> " + dr["bufferDays"].ToString() + " </td><td><a href='Images?carId=" + dr["carId"].ToString() + "'>View</a></td><td><div class='input-submit'><a class='btn btn-secondary' href='Update?carId=" + dr["carId"].ToString() + "'>Edit</a></div></td><td><a class='btn btn-danger' href='#'>Delete</a></td></tr> ";
            }

            return View();
        }

        public ActionResult Images(string carId)
        {
            CarController c = new CarController();
            Models.Car car = new Models.Car();
            DataTable dt = c.GetCarImage(carId);
            ViewBag.htmlString = "";
            foreach (DataRow dr in dt.Rows)
            {
                string path = VirtualPathUtility.ToAbsolute("~/Image/" + dr["ImageName"].ToString());
                ViewBag.htmlString += @" <div class='col-lg-4 col-md-6 con suv mpv'><div class='single-popular-car'><div class='p-car-thumbnails'><a class='car-hover' href='" + path + "'><img src='" + path + "' alt='JSOFT'></a></div><div class='p-car-content' style='text-align:center'><a class='btn btn-danger' href='#'>Delete</a></div></div></div> ";
            }

            return View();
        }

        public ActionResult Vehicle(string location, string startDate, string endDate, string categoryId)
        {
            CarController c = new CarController();
            Models.Booking book = new Models.Booking();
            book.location = location;
            book.categoryId = categoryId;
            string[] sdate = startDate.Split(' ');
            book.pickUpDate = sdate[1];
            string[] edate = endDate.Split(' ');
            book.returnDate = edate[1];
            DataTable dt = c.getAvailableCars(book);

            foreach (DataRow dr in dt.Rows)
            {
                book.carId = dr["carId"].ToString();
                Session["book"] = book;
                string path = VirtualPathUtility.ToAbsolute("~/Image/" + dr["ImageName"].ToString());
                ViewBag.carString += @"<div class='col-lg-6 col-md-6'>
                        <div class='single-car-wrap'><div class='car-list-thumb car-thumb-2' style='background-image: url(" + path + ");'></div><div class='car-list-info without-bar'><h2><a href='#'>" + dr["carName"] + "</a></h2><h5>" + dr["rentalCharge"] + "$ Rent /per a day</h5><ul class='car-info-list'><li>" + dr["carType"] + "</li><li>" + dr["carSeats"] + " Seater</li><li>" + dr["carType"].ToString() + "</li></ul><a href='Book?carid=" + dr["carId"].ToString() + "' class='rent-btn'>Book It</a></div></div></div>";
            }
            return View();
        }

        public ActionResult book()
        {
            Models.Booking book = (Models.Booking)Session["book"];
            CarController c = new CarController();
            Models.Car car = new Models.Car();
            car.carId = book.carId;
            DataTable dt = c.GetCarDetails(car);
            int rent = 0;
            foreach (DataRow dr in dt.Rows)
            {
                rent = Convert.ToInt32(dr["rentalCharge"]);
            }
                
            double days = (Convert.ToDateTime(book.returnDate) - Convert.ToDateTime(book.pickUpDate)).TotalDays;
            double total = rent * days;
            foreach (DataRow dr in dt.Rows)
            {
                ViewBag.htmlString = "<tr><td>Car Requested</td><td>" + dr["carName"] + "</td></tr><tr><td>Pick-Up Location</td><td>" + book.location + "</td></tr><tr><td>Pick-Up Date</td><td>" + book.pickUpDate + "</td></tr><tr><td>Return Date</td><td>" + book.returnDate + "</td></tr><tr><td>Base Rate</td><td>CAD " + dr["rentalCharge"] + "</td></tr></tr><tr><td>Num Of Days</td><td>" + days + "</td></tr><tr><td>Total</td><td>CAD" + total + "</td></tr>";
            }
            return View();
        }

        public ActionResult confirmBooking(string fname, string lname, string phone, string email)
        {
            Models.Booking book = (Models.Booking)Session["book"];
            Customer customer = new Customer();
            customer.firstName = fname;
            customer.lastName = lname;
            customer.emailId = email;
            customer.phone = phone;
            customer.carId = book.carId;
            customer.rentTotal = "";
            customer.feesTaxes = "";
            customer.addedBy = "";
            CarController c = new CarController();
            c.addBooking(book, customer);
            return RedirectToAction("Confirmation", "Home");
            return View();
        }
    }
}