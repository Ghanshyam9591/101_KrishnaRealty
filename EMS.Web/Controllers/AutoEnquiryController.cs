using EMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.Web.Controllers
{
    [CustomAuthorize]
    public class AutoEnquiryController : Controller
    {
        // GET: AutoEnquiry
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAutoEnquiryGrid()
        {
            AutoEnquiry bll = new AutoEnquiry();
            return new JsonNetResult(bll.GetAutoEnquiryGrid());

        }
    }
}