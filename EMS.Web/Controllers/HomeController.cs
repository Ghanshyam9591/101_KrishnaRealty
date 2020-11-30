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
        public ActionResult Get_Dashboard_Detail()
        {
            Home bll = new Home();
            return new JsonNetResult(bll.GetDashboardBoxDetail());
        }
        public ActionResult Get_Dashboard_Location_Wise()
        {
            Home bll = new Home();
            return new JsonNetResult(bll.GetDashboardLocationWise_Detail());
        }
        public ActionResult Get_Rental_Dashboard()
        {
            Home bll = new Home();
            return new JsonNetResult(bll.Get_Rental_Dashboard_Detail());
        }
    }
}
