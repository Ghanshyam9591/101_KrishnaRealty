using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.Web.Models;

namespace EMS.Web.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
     
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
