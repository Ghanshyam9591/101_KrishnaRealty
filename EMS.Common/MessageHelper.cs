using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Common
{
    public class MessageHelper
    {
        public static string InvalidCredentials
        {
            get
            {
                return "Invalid User & Password.";
            }
        }
        public static string Success
        {
            get
            {
                return "Data Save successfully !";
            }
        }
        public static string Update
        {
            get
            {
                return "Data updated successfully !";
            }
        }
        public static string Fail
        {
            get
            {
                return "Unable to save data !";
            }
        }
        public static string DuplicateRecord
        {
            get
            {
                return "Record Already Present !";
            }
        }
    }
}