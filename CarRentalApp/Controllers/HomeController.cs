using CarRentalApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRentalApp.Controllers
{
    public class HomeController : Controller
    {
        CarMaster ds = new CarMaster();
        public ActionResult Index()
        {
            var categoryList = ds.category_master.ToList();
            ViewBag.CategoryList = new SelectList(categoryList, "categoryId", "categoryName");
            CarController c = new CarController();
            Models.Car car = new Models.Car();
            DataTable dt = c.Gettop8(car);
            foreach (category_master category in categoryList)
            {
               
                ViewBag.menuString += @"<a href='#' data-filter='."+category.categoryName+"'>"+category.categoryName+"</a>";
            }

            foreach (DataRow dr in dt.Rows)
            {
                string path = VirtualPathUtility.ToAbsolute("~/Image/" + dr["ImageName"].ToString());
                ViewBag.carString += @"<div class='col-lg-4 col-md-6 "+ dr["category"].ToString() + "'><div class='single-popular-car'><div class='p-car-thumbnails'><a class='car-hover' href='"+path+"'><img src='"+path+"' alt='JSOFT'></a></div><div class='p-car-content'><h3><a href='#'> "+ dr["carName"].ToString() + "</a><span class='price'><i class='fa fa-tag'></i> $"+dr["rentalCharge"].ToString()+"/day</span></h3><h5>"+dr["category"].ToString()+ "</h5><div class='p-car-feature'><a href='#'>" + dr["carModel"].ToString() + "</a><a href='#'>" + dr["carType"].ToString() + "</a></div></div></div></div>";
            }

            return View();
        }

       

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Confirmation()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Login(string username,string password)
        {
            if(username=="admin" && password == "admin")
            {
                Session["username"] = "admin";
                return RedirectToAction("Cars", "AddCar");
            }else
            {
                ViewBag.Alert = "Failed";
            }

            return View();
        }

    }
}