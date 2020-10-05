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
    public class LovController : Controller
    {
        // GET: Lov
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Item()
        {
            return View();
        }
        public ActionResult GetItem(int id)
        {
            Lov bll = new Lov();
            return new JsonNetResult(bll.GetItem(id));
        }

        public ActionResult GetLovGrid()
        {
            Lov bll = new Lov();
            return new JsonNetResult(bll.GetLovGrid());

        }
        public ActionResult Save(LovModel model)
        {
            //UserModel modelObj = Helper.Deserialize<UserModel>(model);
            Lov bll = new Lov();
            return new JsonNetResult(bll.Save(model));
        }
    }
}