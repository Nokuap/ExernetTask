﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exernet.Filters;
using Exernet.Models;

namespace Exernet.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var result = db.Tasks.OrderByDescending(obj=>obj.UploadDate);
            return View(result);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        [Authorize(Roles="Administrator")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }



    }
}