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
    public  class Helper
    {
        static string strcon = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        public static void CheckEnquirySource(string EnquiryFrom, DateTime EmailReceivedDate, string html_Emailbody, string str_email_body)
        {
            if (EnquiryFrom.ToLower().Contains("ypleads@sulekhanotifications.com"))
            {
                StringReader reader = new StringReader(str_email_body);
                Sulekha.GetSulekhEnquiry(reader, DateTime.Now, EmailReceivedDate);
            }
            else if (EnquiryFrom.ToLower().Contains("no-reply@99acres.com"))
            {
                _99acres.GetEnquiry99Acrs(str_email_body, EmailReceivedDate);
            }
            else if (EnquiryFrom.ToLower().Contains("noreply@housing-mailer.com"))
            {
                StringReader reader = new StringReader(str_email_body);
                Housing.GetHousingEnquiry(reader, DateTime.Now, EmailReceivedDate);
            }
            else if (EnquiryFrom.ToLower().Contains("lead@magicbriks.com"))
            {

            }
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
                                             new NpgsqlParameter("pname",enqModl.Name),
                                             new NpgsqlParameter("pmobile1",enqModl.phone),
                                             new NpgsqlParameter("pemail_id",enqModl.Email),
                                             new NpgsqlParameter("penquiry_source",enqModl.EnqSoure),
                                             new NpgsqlParameter("padditional_info",enqModl.additional_Info)
            };
            string Response = Helper.ExecuteProcedure("select ems_fun_insert_auto_enquiry(:pname,:pmobile1,:pemail_id,:penquiry_source,:padditional_info)", Insert_Parameters);



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
        private static void WriteLog(string message)
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
    }
}
