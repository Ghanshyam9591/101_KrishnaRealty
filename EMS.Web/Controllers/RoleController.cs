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
    public class RoleController : Controller
    {
        // GET: Role
       
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
            Role bll = new Role();
            return new JsonNetResult(bll.GetItem(id));
        }

        public ActionResult GetRoleGrid()
        {
            Role bll = new Role();
            return new JsonNetResult(bll.GetRoleGrid());

        }
        public ActionResult Save(RoleModel model)
        {
            //UserModel modelObj = Helper.Deserialize<UserModel>(model);
            Role bll = new Role();
            return new JsonNetResult(bll.Save(model));
        }
    }
}