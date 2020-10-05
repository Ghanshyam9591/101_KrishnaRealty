using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EMS.Web.Models;
using EMS.Common;

namespace EMS.Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(LoginViewModel model, string returnUrl)
        {
            
            Login bll = new Login();
            UserDetailsModel objmodel = bll.GetUserDetail(model.UserID,model.Password);

            if (objmodel != null)
            {
                Session["UserDetails"] = objmodel;
                Common2 cm2 = new Common2();
                List<MenusModel> menus = cm2.GetMenus();
                Session["MenuList"] = menus;

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return Redirect(Request.Url.AbsoluteUri + returnUrl);
                }
                //if (!string.IsNullOrWhiteSpace(objmodel.employee_code.ToString()))
                //{
                //    return RedirectToAction("Index", "Home");

                //}
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.errorMessage = MessageHelper.InvalidCredentials;
            }
            return View();

        }
        public ActionResult Logout()
        {
            Session["UserDetails"] = null;
            return RedirectToAction("Index");
        }
    }
}