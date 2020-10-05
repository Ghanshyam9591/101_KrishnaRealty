using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Common
{
    public class EmailActivity
    {
        public static void SendEmail(List<RecordException> ExceptionModel, string AlertCategory)
        {
            

            string PrimaryMailServer = ConfigurationManager.AppSettings["PrimaryMailServer"];

            string SENDER_EMAIL = ConfigurationManager.AppSettings["SenderEmail"];
            string SENDER_PASS = ConfigurationManager.AppSettings["SenderPass"];
            int PORT = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            bool IsSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSsl"]);
            string[] AleartEmailIds = ConfigurationManager.AppSettings["AleartEmailIds"].Split(',');
            string SenderEmailId = "";
            int serverFlag = 0;
            SenderEmailId = ConfigurationManager.AppSettings["SenderEmailId"];

            SmtpClient smtpClient;

            retry:
            if (serverFlag == 2)
                return;

            smtpClient = new SmtpClient(PrimaryMailServer, PORT);
            try
            {

                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                MailAddress mailAddress = new MailAddress(SENDER_EMAIL);
                message.Sender = mailAddress;
                message.From = mailAddress;

                switch (AlertCategory)
                {
                    case "FILE ERROR":
                        {
                            message.Body = GetEmailBody(ExceptionModel);
                            message.Subject = "EMS UTILITY - ERROR IN FILE [ K ]";
                            break;
                        }
                    case "INSERT ERROR":
                        {
                            message.Body = GetEmailBody(ExceptionModel);
                            message.Subject = "EMS UTILITY - ERROR IN FILE [ k]";
                            break;
                        }
                }
                if (AleartEmailIds != null)
                {
                    for (int i = 0; i < AleartEmailIds.Length; i++)
                    {
                        message.To.Add(AleartEmailIds[i]);
                    }
                }

                smtpClient.Send(message);
                smtpClient.Dispose();
            }
            catch (SmtpException se)
            {
                serverFlag++;
                Console.WriteLine("Error sending email " + se.ToString());
                Helper.WriteLog("SMTP ERROR : " + se.Message);
                goto retry;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email " + ex.ToString());
                Helper.WriteLog("Sending Mail Failed : "+ex.Message);
            }
        }

        public static string GetEmailBody(List<RecordException> ExceptionModel)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang='en' xmlns='http://www.w3.org/1999/xhtml'>");
            sb.AppendLine("<head><title></title></head>");
            sb.AppendLine("<body>");
            sb.AppendLine("Dear Sir/Madam");
            sb.AppendLine("</br></br>");
            sb.AppendLine("The Utility failed to upload the data.</br>Please find the Error Details</br>");
            sb.AppendLine("<table border='1'>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<th style='background-color:darkgray'><b>Sr.No.</b></th>");
            sb.AppendLine("<th style='background-color:darkgray'><b>Activity Number</b></th>");
            sb.AppendLine("<th style='background-color:darkgray'><b>Activity Name</b></th>");
            sb.AppendLine("<th style='background-color:darkgray'><b>Error Message</b></th>");
            sb.AppendLine("</tr>");
            int srno = 1;
            foreach (RecordException recException in ExceptionModel)
            {
                sb.AppendLine("<tr>");
                sb.AppendLine("<td>" + srno + "</td>");
                
                sb.AppendLine("<td>" + recException.ERR_MSG + "</td>");
                sb.AppendLine("</tr>");
                srno++;
            }

            sb.AppendLine("</table>");
            sb.AppendLine("</br></br></br>");
            sb.AppendLine("Thanks & Regards");
            sb.AppendLine("</br>");
            sb.AppendLine("This is a system generated mail.");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}
