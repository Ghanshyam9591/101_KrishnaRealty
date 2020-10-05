using EMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS.Web.Controllers
{
    [CustomAuthorize]
    public class CallHistoryController : Controller
    {
        // GET: CallHistory
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetGridDetail(int enquiry_no)
        {
            CallHistory bll = new CallHistory();
            return new JsonNetResult(bll.GetGridDetail(enquiry_no));
        }
    }
}