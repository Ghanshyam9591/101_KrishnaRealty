using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using System.Configuration;
using System.Globalization;
using System.Web;

namespace EMS.Common
{
    public static class Helper
    {

        public const string CONNECTION_STRING_NAME = "DatabaseConnection";
        static JsonSerializer GetSerializer()
        {
            JsonSerializerSettings settings;
            settings = new JsonSerializerSettings();
            settings.Formatting = Newtonsoft.Json.Formatting.None;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            settings.DateFormatString = "yyyy-MM-ddTHH:mm:00zzz";
            return JsonSerializer.Create(settings);
        }
        public static T Deserialize<T>(string value)
        {
            StringReader txtreader = new StringReader(value);
            JsonTextReader reader = new JsonTextReader(txtreader);
            return GetSerializer().Deserialize<T>(reader);
        }
        public static string Serialize(object data)
        {
            StringWriter txtwriter = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(txtwriter);
            GetSerializer().Serialize(writer, data);
            return txtwriter.GetStringBuilder().ToString();
        }
        public static void WriteLog(string message)
        {
            string ErrorLogDir = ConfigurationManager.AppSettings["ErrorLogFile"].ToString();
            if (!Directory.Exists(ErrorLogDir))
                Directory.CreateDirectory(ErrorLogDir);

            ErrorLogDir += "\\EMS_Error_Log_" + ".txt";

            using (StreamWriter sw = new StreamWriter(ErrorLogDir, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
        public static string ToDateFormat(this string cString)
        {
            DateTime output;
            DateTime checkedDate;
            if (DateTime.TryParse(cString, out checkedDate))
            {
                string[] formats = { "M/d/yyyy", "dd-MMM-yyyy", "dd/M/yyyy", "d/M/yyyy", "M-d-yyyy", "d-M-yyyy", "d-MMM-yy", "d-MMMM-yyyy", "yyyyMMdd", "yyyy-MM-dd", "yyyy/MM/dd", "yyyyMMMdd", "yyyy/MMM/dd", "yyyy-MMM-dd", "dd/MM/yyyy", "MM/dd/yyyy", "MM/dd/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss" };

                DateTime.TryParseExact(checkedDate.Date.ToString("yyyy-MM-dd"), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out output);
                return output.ToString("yyyy-MM-dd");

            }
            else
                return "";
        }
        public static int GetUserID()
        {
            UserDetailsModel UDM = (UserDetailsModel)HttpContext.Current.Session["UserDetails"];
            return UDM.employee_code;
        }
        
    }
}
