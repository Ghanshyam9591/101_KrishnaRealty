using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Common;
using Newtonsoft.Json;

namespace EMS.Web.Models
{
    public class UserManager
    {

        public static UserDetailsModel User
        {
            get
            {
                return (UserDetailsModel)HttpContext.Current.Session["UserDetails"];
            }
        }

    }
}