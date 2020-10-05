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
    public class MenuRoleMapController : Controller
    {
        // GET: MenuRoleMap
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetGridDetail()
        {
            MenuRoleMap bll = new MenuRoleMap();
            return new JsonNetResult(bll.GetMenusGrid());

        }

        public ActionResult Save(List<MenuModel> model, int role_id)
        {
            MenuRoleMap bll = new MenuRoleMap();
            return new JsonNetResult(bll.Save(model,role_id));
        }
        public ActionResult GetAssignedMenuToRole(int RoleID)
        {
            MenuRoleMap bll = new MenuRoleMap();
            return new JsonNetResult(bll.GetAssignedMenuToRole(RoleID));

        }
    }
}