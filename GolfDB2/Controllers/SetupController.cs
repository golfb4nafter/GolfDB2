using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GolfDB2.Models;

namespace GolfDB2.Controllers
{
    public class SetupController : Controller
    {
        // GET: Setup
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("../Account/Login");

            return View(new SetupModel());
        }
    }
}