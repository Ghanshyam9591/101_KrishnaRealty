using EMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.Common;

namespace EMS.Web.Controllers
{
    [CustomAuthorize]
    public class EnquiryDataEntryController : Controller
    {
        // GET: EnquiryDataEntry
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Item()
        {
            return View();
        }
        public ActionResult ListData()
        {
            EnquiryDataEntry bll = new EnquiryDataEntry();
            return new JsonNetResult(bll.LIST_DATA());

        }
        public ActionResult GetItem(int id)
        {
            EnquiryDataEntry bll = new EnquiryDataEntry();
            return new JsonNetResult(bll.GetItem(id));
        }
        public ActionResult Save(EnquiryDataModel model)
        {
            //UserModel modelObj = Helper.Deserialize<UserModel>(model);
            EnquiryDataEntry bll = new EnquiryDataEntry();
            return new JsonNetResult(bll.Save(model));
        }
    }
}