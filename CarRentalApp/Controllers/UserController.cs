﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRentalApp.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}