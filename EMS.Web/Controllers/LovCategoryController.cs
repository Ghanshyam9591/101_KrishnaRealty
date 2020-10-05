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
    public class LovCategoryController : Controller
    {
        // GET: LovCategory
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
            LovCategory bll = new LovCategory();
            return new JsonNetResult(bll.GetItem(id));
        }

        public ActionResult GetLoveCategoryGrid()
        {
            LovCategory bll = new LovCategory();
            return new JsonNetResult(bll.GetLovCategoryGrid());

        }
        public ActionResult Save(LovCategoryModel model)
        {
            //UserModel modelObj = Helper.Deserialize<UserModel>(model);
            LovCategory bll = new LovCategory();
            return new JsonNetResult(bll.Save(model));
        }
    }
}