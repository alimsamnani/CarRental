using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRentalApp.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Add(string categoryName)
        {
           
            if (categoryName != "" && categoryName != null)
            {
                Models.Category category = new Models.Category();
                category.CategoryName = categoryName;
                CarController c = new CarController();
                c.addCarCategory(category);
                //ViewBag.JavaScriptFunction = string.Format("openModal();");

            }
            return View();
        }


        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult Category()
        {
            CarController c = new CarController();
            Models.Category category = new Models.Category();
            DataTable dt = c.getCategory(category);
            ViewBag.htmlString = "";
            foreach (DataRow dr in dt.Rows)
            {
                ViewBag.htmlString += @"<tr><th> " + dr["categoryName"].ToString() + " </th><td><div class='input-submit'><a class='btn btn-secondary' href='Update/categoryId=" + dr["categoryId"].ToString() + "'>Edit</a></div></td><td><a class='btn btn-danger' href='#'>Delete</a></tr> ";
            }

            return View();
        }
    }
}