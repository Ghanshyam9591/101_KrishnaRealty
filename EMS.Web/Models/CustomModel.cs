using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EMS.Web.Models
{
    public class CustomModel
    {
    }
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "UserID")]
        [EmailAddress]
        public string UserID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}