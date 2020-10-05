using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EMS.Web.Models
{

    public class CustomAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            string returnURL = filterContext.HttpContext.Request.RawUrl;
            var user = UserManager.User;
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                        {
                            { "action", "Index" },
                            { "controller", "Login" }
                        });
            }
            else
            {
                if (!user.FullAccess)
                {
                    if ((returnURL.ToLower().Contains("/menu") || returnURL.ToLower().Contains("/user") || returnURL.ToLower().Contains("/menucreation") || returnURL.ToLower().Contains("/role") || returnURL.ToLower().Contains("/menurolemap")))
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                        {
                            { "action", "Index" },
                            { "controller", "AccessDenied" }
                        });
                    }
                }

            }
        }
    }
}
