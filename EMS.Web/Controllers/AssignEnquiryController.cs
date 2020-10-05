using EMS.Common;
using EMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.Web.Controllers
{
    [CustomAuthorize]
    public class AssignEnquiryController : Controller
    {
        // GET: AssignEnquiry
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Save(EnquiryDataModel model)
        {
            AssignEnquiry bll = new AssignEnquiry();
            return new JsonNetResult(bll.Save(model));
        }
        public ActionResult ListData()
        {
            AssignEnquiry bll = new AssignEnquiry();
            return new JsonNetResult(bll.LIST_DATA());

        }
    }
}