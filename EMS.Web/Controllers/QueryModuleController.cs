using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.Common;
using EMS.Web.Models;

namespace EMS.Web.Controllers
{
    [CustomAuthorize]
    public class QueryModuleController : Controller
    {
        // GET: QueryModul
        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetGridDetail(int cust_id, int location_id, int enquiry_source_id, string enquiry_from,string enquiry_to, int action_type_id, string query_type,int enquiry_type_id)
        {
            QueryModule bll = new QueryModule();
            return new JsonNetResult(bll.GetGridDetail(cust_id, location_id, enquiry_source_id, enquiry_from,enquiry_to, action_type_id, query_type, enquiry_type_id));
        }
        public ActionResult AddComment(Int64 Enquiry_No, Int64 Action_Type_ID, string Comment)
        {
            QueryModule bll = new QueryModule();
            return new JsonNetResult(bll.Save(Enquiry_No, Action_Type_ID, Comment));

        }
    }
}