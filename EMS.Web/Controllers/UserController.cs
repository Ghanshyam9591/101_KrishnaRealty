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
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Item()
        {
            return View();
        }

        public ActionResult GetRoleGrid()
        {
            User bll = new User();
            return new JsonNetResult(bll.GetRoleGrid());

        }
        public ActionResult GetItem(int id)
        {
            User bll = new User();
            return new JsonNetResult(bll.GetItem(id));
        }
        public ActionResult Save(UserModel model)
        {
            //UserModel modelObj = Helper.Deserialize<UserModel>(model);
            User bll = new User();
            return new JsonNetResult(bll.Save(model));
        }
    }
}