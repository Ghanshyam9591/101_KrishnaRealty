using EMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.Web.Controllers
{
    [CustomAuthorize]
    public class FollowUpController : Controller
    {
        // GET: FollowUp
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public Action Index(int enquiry_no)
        {
            return null;
        }
    }
}