using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EmailUtility.Class;
using Npgsql;

namespace EmailUtility
{
    public class Helper
    {
        static string strcon = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        public static void CheckEnquirySource(string EnquiryFrom, DateTime EmailReceivedDate, string html_Emailbody, string str_email_body)
        {
            DataTable dt_config = Helper.executeSelect("select * from lms_tbl_email_body_parsing where is_active=true", null);

            if (EnquiryFrom.ToLower().Contains("ypleads@sulekhanotifications.com"))
            {
                if (str_email_body.Contains("!Sulekha Campaign Topup"))
                {
                    StringReader reader = new StringReader(str_email_body);
                    Sulekha.GetSulekhEnquiry(reader, DateTime.Now, EmailReceivedDate, dt_config);
                }
            }
            else if (EnquiryFrom.ToLower().Contains("no-reply@99acres.com"))
            {
                _99acres.GetEnquiry99Acrs(html_Emailbody, EmailReceivedDate, dt_config);
            }
            else if (EnquiryFrom.ToLower().Contains("noreply@housing-mailer.com"))
            {
                Housing.GetHousingEnquiry(html_Emailbody, DateTime.Now, EmailReceivedDate, dt_config);
            }
            else if (EnquiryFrom.ToLower().Contains("prop.ex@magicbricks.com"))
            {

            }
        }
        public static string get_enquiry_type(string strbody)
        {
            StringReader reader = new StringReader(strbody);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if ((line.Contains("₹") && line.ToLower().Contains("l")) || (line.ToLower().Contains("rs") && line.ToLower().Contains("lac")))
                {
                    return "Sale";
                }
                else if (line.Contains("₹") && line.ToLower().Contains("k"))
                {
                    return "Rent";
                }
            }
            return "";
        }
        public static string ExecuteProcedure(string procedure_name, NpgsqlParameter[] paramenters)
        {
            DataTable dt = new DataTable();
            string Response = string.Empty;
            object obj = new object();
            using (NpgsqlConnection conn = new NpgsqlConnection(strcon))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(procedure_name, conn))
                {
                    try
                    {
                        conn.Open();
                        if (paramenters != null)
                            cmd.Parameters.AddRange(paramenters);
                        // cmd.CommandType = CommandType.StoredProcedure;
                        obj = cmd.ExecuteScalar();

                        if (obj != null)
                            return obj.ToString();
                        else
                            return "";
                    }
                    catch (Exception exx)
                    {
                        Helper.WriteLog("ExecuteProcedure error :" + exx.Message);
                        return "";
                    }
                }
            }
        }

        public static DataTable executeSelect(string selectQuery, NpgsqlParameter[] paramenters)
        {
            DataTable dt = new DataTable();
            string Response = string.Empty;
            using (NpgsqlConnection conn = new NpgsqlConnection(strcon))
            {
                try
                {
                    conn.Open();
                    using (NpgsqlCommand cmdSelect = new NpgsqlCommand(selectQuery))
                    {
                        cmdSelect.Connection = conn;
                        if (paramenters != null)
                            cmdSelect.Parameters.AddRange(paramenters);
                        using (NpgsqlDataAdapter SqDA = new NpgsqlDataAdapter(cmdSelect))
                        {
                            SqDA.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Helper.WriteLog("ERROR IN Save fun : " + ex.Message);
                }
                return dt;
            }
        }
        public static void InsertOwnerDetails(OwnerModel OWM)
        {
            bool isSuccess = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString").ToString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "KRMInsertOwnerDetails";
                        cmd.Parameters.AddWithValue("@Name", OWM.Name);
                        cmd.Parameters.AddWithValue("@Email", OWM.Email);
                        cmd.Parameters.AddWithValue("@Mobile1", OWM.Mobile1);
                        cmd.Parameters.AddWithValue("@Mobile2", OWM.Mobile2);
                        cmd.Parameters.AddWithValue("@FlatType", OWM.FlatType);
                        cmd.Parameters.AddWithValue("@Title", OWM.Title);
                        cmd.Parameters.AddWithValue("@Location", OWM.Location);
                        cmd.Parameters.AddWithValue("@SiteID", OWM.SiteID);
                        cmd.Parameters.AddWithValue("@Source", OWM.Source);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("Unable to insert record into Enquiry table " + ex.ToString());
                Console.WriteLine("Unable to Insert Record into database. Error is " + ex.ToString());
            }
        }
        public static bool InsertInquery(EnquiryModl enqModl)
        {
            bool isSuccess = false;

            NpgsqlParameter[] Insert_Parameters = {
                                             new NpgsqlParameter("pname",string.IsNullOrWhiteSpace(enqModl.Name)?"":enqModl.Name),
                                             new NpgsqlParameter("pmobile1",enqModl.phone),
                                             new NpgsqlParameter("pemail_id",enqModl.Email),
                                             new NpgsqlParameter("penquiry_source",enqModl.EnqSoure),
                                             new NpgsqlParameter("padditional_info",string.IsNullOrWhiteSpace(enqModl.additional_Info)?"":enqModl.additional_Info),
                                             new NpgsqlParameter("pemail_body",string.IsNullOrWhiteSpace(enqModl.Email_body)?"":enqModl.Email_body),
                                             new NpgsqlParameter("plocation_id",enqModl.location_id),
                                             new NpgsqlParameter("pproperty_type_id",enqModl.property_type_id),
                                             new NpgsqlParameter("penquiry_type_id",enqModl.enquiry_type_id),
                                             new NpgsqlParameter("penquiry_source_id",enqModl.enquiry_source_id),
                                             new NpgsqlParameter("pcost_upto",enqModl.cost_upto)
            };
            string Response = Helper.ExecuteProcedure("select ems_fun_insert_auto_enquiry(:pname,:pmobile1,:pemail_id,:penquiry_source,:padditional_info,:pemail_body,:plocation_id,:pproperty_type_id,:penquiry_type_id,:penquiry_source_id,:pcost_upto)", Insert_Parameters);



            //try
            //{
            //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString").ToString()))
            //    {
            //        using (SqlCommand cmd = new SqlCommand())
            //        {
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.CommandText = "KRMInsertEnquiry";
            //            //cmd.Parameters.AddWithValue("@Project", enqModl.Project);
            //            //cmd.Parameters.AddWithValue("@Title", enqModl.Title);
            //            cmd.Parameters.AddWithValue("@Name", enqModl.Name);
            //            cmd.Parameters.AddWithValue("@Phone", enqModl.phone);
            //            cmd.Parameters.AddWithValue("@Email", enqModl.Email);
            //            cmd.Parameters.AddWithValue("@EnquiryDate", enqModl.EnquiryDate);
            //            cmd.Parameters.AddWithValue("@EnqSoure", enqModl.EnqSoure);

            //            cmd.Connection = con;
            //            con.Open();
            //            cmd.ExecuteNonQuery();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    WriteLog("Unable to insert record into Enquiry table " + ex.ToString());
            //    Console.WriteLine("Unable to Insert Record into database. Error is " + ex.ToString());
            //}

            return isSuccess;
        }
        public static void WriteLog(string message)
        {
            string ErrorLogDir = ConfigurationManager.AppSettings["ErrorLogFile"];
            if (!Directory.Exists(ErrorLogDir))
                Directory.CreateDirectory(ErrorLogDir);

            ErrorLogDir += "\\EmailUtilityErrorLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";

            using (StreamWriter sw = new StreamWriter(ErrorLogDir, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }

        public static string Get_emaid_from_string(string text)
        {
            const string MatchEmailPattern =
           @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
           + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
             + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
           + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
            Regex rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            // Find matches.
            MatchCollection matches = rx.Matches(text);
            // Report the number of matches found.
            int noOfMatches = matches.Count;

            // Report on each match.
            foreach (Match match in matches)
            {
                return match.Value.ToString();
            }
            return "";
        }
        public static string Get_mobile_from_string(string text)
        {
            const string MatchPhonePattern = @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}";

            Regex rx = new Regex(MatchPhonePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Find matches.
            MatchCollection matches = rx.Matches(text);

            // Report the number of matches found.
            int noOfMatches = matches.Count;


            //Do something with the matches

            foreach (Match match in matches)
            {
                //Do something with the matches
                return match.Value.ToString(); ;

            }
            return "";
        }
    }
}
