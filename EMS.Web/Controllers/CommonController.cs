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
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult GetPropertytypes()
        {
            Common2 bll = new Common2();
            return new JsonNetResult(bll.GetPropertyTypes());
        }

        public ActionResult GetEnquiryTypes()
        {
            Common2 bll = new Common2();
            return new JsonNetResult(bll.GetEnquiryTypes());
        }
        public ActionResult GetLovCategories()
        {
            Common2 bll = new Common2();
            return new JsonNetResult(bll.GetLovCategory());
        }
        public ActionResult GetActionTypes()
        {
            Common2 bll = new Common2();
            return new JsonNetResult(bll.GetActionTypes());
        }
        public ActionResult GetEnquirySources()
        {
            Common2 bll = new Common2();
            return new JsonNetResult(bll.GetEnquirySouces());
        }
        public ActionResult GetLocations()
        {
            Common2 bll = new Common2();
            return new JsonNetResult(bll.GetLocations());
        }
        public ActionResult GetMenus()
        {
            Common2 bll = new Common2();
            return new JsonNetResult(bll.GetMenus());
        }
        public ActionResult GetRoles()
        {
            Common2 bll = new Common2();
            return new JsonNetResult(bll.GetRoles());
        }
        public ActionResult GetUsers()
        {
            Common2 bll = new Common2();
            return new JsonNetResult(bll.GetUsers());
        }
        
        public ActionResult GetCustomers(string q)
        {
            Common2 bll = new Common2();
            return new JsonNetResult(bll.GetCustomers(q));
        }
        public ActionResult GetOwners(string q)
        {
            Common2 bll = new Common2();
            return new JsonNetResult(bll.GetOwners(q));
        }
        public ActionResult GetRenters(string q)
        {
            Common2 bll = new Common2();
            return new JsonNetResult(bll.GetRenters(q));
        }
        public ActionResult GetBuildings()
        {
            Common2 bll = new Common2();
            return new JsonNetResult(bll.GetBuildings());
        }
    }
}
