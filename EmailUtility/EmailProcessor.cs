using Ionic.Zip;
using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net.Mail;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using EmailUtility;

namespace EmailUtility
{
    public enum EmailStatus
    {
        InvalidAttachmentFormat,
        NoAttachments,
        RecipientNotRegistered,
        RecipientNotRegisteredAndTimestamped,
        RecipientRegistered,
        RegisteredANDTimestamped,
        Timestamped,
        UnableToReadOrTimestamp
    }

    public class EmailProcessor
    {
        public void ProcessEmails()
        {
            string PrimaryMailServer = ConfigurationManager.AppSettings["PrimaryMailServer"];
            string SecondryMailServer = ConfigurationManager.AppSettings["SecondryMailServer"];
            string MailPort = ConfigurationManager.AppSettings["MailServerPort"];
            string CorporateEmail = ConfigurationManager.AppSettings["CorporateEmailId"];
            string CorporatPass = ConfigurationManager.AppSettings["CorporateEmailPass"];
            bool IsSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["MailServerUseSSL"]);
            string MailServerPort = ConfigurationManager.AppSettings["MailServerPort"];
            string MailNotReadToRecipients = ConfigurationManager.AppSettings["MailNotReadToRecipients"];

            string SulekhaEmailFromID = ConfigurationManager.AppSettings["SulekhaEmaiilFrom"];

            string[] sulekhaFormat = ConfigurationManager.AppSettings["SulekhaEmailFormat"].Split(',');

            int TotalEmailCount = 0;

            Message EmailMessage = null;
            DTSEmail DTSemail = new DTSEmail();
            DataTable dtExisting = getDataTable("getEmailUtilityEmailAudit", null);
            DataRow[] aRow;
            Pop3Client PopClient = new Pop3Client();
            List<string> mailCcIds = new List<string>();
            List<string> messageUids = new List<string>();
            int currentMessageId = 0;

            try
            {
                currentMessageId = 0;
                WriteLog("Email Utility started");
                PopClient.Connect(PrimaryMailServer, Convert.ToInt16(MailPort), IsSSL);
                PopClient.Authenticate(CorporateEmail, CorporatPass, AuthenticationMethod.UsernameAndPassword);
                TotalEmailCount = PopClient.GetMessageCount();
                messageUids = PopClient.GetMessageUids();
            }
            catch (Exception ex)
            {
                try
                {
                    WriteLog("Unable to connect to Mail server " + PrimaryMailServer + " Error " + ex.ToString());
                    PopClient.Connect(SecondryMailServer, Convert.ToInt16(MailPort), false);
                    PopClient.Authenticate(CorporateEmail, CorporatPass, AuthenticationMethod.UsernameAndPassword);
                    TotalEmailCount = PopClient.GetMessageCount();
                    messageUids = PopClient.GetMessageUids();
                }
                catch (Exception ex2)
                {
                    WriteLog("Email Utility trying to connect to mail server " + PrimaryMailServer + " and " + SecondryMailServer + " Error " + ex2.ToString());
                    SendEmail(MailNotReadToRecipients.Split(','), null, "Unable to connect to Mail server" + PrimaryMailServer + " and " + SecondryMailServer, "Dear Team, </br> </br> DTS Email Utility was unable to Connect to mailserver " + PrimaryMailServer + " and " + SecondryMailServer + ". </br> Error </br> " + ex2.ToString(), null, null);
                    Console.WriteLine("Unable to connect to Mail server " + SecondryMailServer + " Error is " + ex2.ToString());
                    return;
                }
            }


            for (int i = 0; i < messageUids.Count; i++)
            {
                try
                {
                    DTSemail = new DTSEmail();
                    mailCcIds = new List<string>();
                    currentMessageId = i + 1;

                    aRow = dtExisting.Select("MessageUid = '" + messageUids[i] + "'");// today 1 feb comment 2020
                    // aRow = dtExisting.Select("MessageUid = '" + messageUids[i] + "' and EmailReceivedDate ='" + getEmailDate(EmailMessage).ToString("dd-MMM-yyyy HH:mm:ss") + "'");
                    if (aRow.Length < 1)
                    {
                        EmailMessage = PopClient.GetMessage(currentMessageId);
                        PreserveEmail(EmailMessage, messageUids[i]);
                        //Console.WriteLine(EmailMessage.Headers.From.Address.ToString());
                        //if (EmailMessage.Headers.From.Address.ToString().Contains("kreality00@gmail.com"))
                        //    continue;
                    }
                    else continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to read email at index " + currentMessageId.ToString() + " " + ex.ToString());
                    WriteLog("Unable to read email at index " + currentMessageId.ToString() + " " + ex.ToString());
                    SendEmail(MailNotReadToRecipients.Split(','), null, "Unable to read email at index " + currentMessageId.ToString(), "Dear Team, </br> </br> DTS Email Utility was unable to read email at index " + currentMessageId.ToString() + ". </br> Error </br> " + ex.ToString(), null, null);
                    continue;
                }

                DTSemail.EmailReceivedDateTime = getEmailDate(EmailMessage);
                //if (DTSemail.EmailReceivedDateTime.Date.CompareTo(DateTime.Now.Date) != 0)
                //    continue;

                DTSemail.EmailReceivedFrom = EmailMessage.Headers.From.Address.ToString();

                StringBuilder sb_html_text = new StringBuilder();

                OpenPop.Mime.MessagePart plainHtml = EmailMessage.FindFirstHtmlVersion();
                HtmlDocument docc = new HtmlDocument();

                byte[] htmlbody = plainHtml.Body;
                string emailbody = System.Text.Encoding.UTF8.GetString(htmlbody);
                sb_html_text.AppendLine("<!DOCTYPE html><html><head><title>Page Title</title></head><body>");
                sb_html_text.AppendLine(emailbody);
                sb_html_text.AppendLine("</body></html> ");

                StringBuilder sb_plain_text = new StringBuilder();
                OpenPop.Mime.MessagePart plainText = EmailMessage.FindFirstPlainTextVersion();
                if (plainText != null)
                {
                    // We found some plaintext!
                    sb_plain_text.Append(plainText.GetBodyAsText());
                }


                Helper.CheckEnquirySource(DTSemail.EmailReceivedFrom, sb_html_text.ToString(), sb_plain_text.ToString());
                // OwnerModel ownmoderl = Helper.GetOwnerDetailsFrom99Acrs(docc.DocumentNode.InnerText.ToString());
                //    if (ownmoderl != null)
                //    {
                //        // Helper.InsertOwnerDetails(ownmoderl);
                //    }
                //    HtmlDocument htmlDoc = new HtmlDocument();
                //    htmlDoc.LoadHtml(builder.ToString());
                //    var inputs = htmlDoc.DocumentNode.Descendants("table");

                //    HtmlDocument doc = new HtmlDocument();
                //    doc.LoadHtml(builder.ToString());
                //    foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
                //    {
                //        Console.WriteLine("Found: " + table.Id);
                //        foreach (HtmlNode row in table.SelectNodes("tr"))
                //        {
                //            Console.WriteLine("row");
                //            //foreach (HtmlNode cell in row.SelectNodes("th|td"))
                //            //{

                //            Console.WriteLine("cell: " + row.InnerText);


                //            HtmlNodeCollection cells = row.SelectNodes("td");
                //            if (cells != null)
                //            {
                //                for (int k = 0; k < cells.Count; ++k)
                //                {
                //                    if (k == 0)
                //                    {
                //                        Console.WriteLine("Colnm :" + cells[k].InnerText + "<br>");
                //                    }
                //                    else
                //                    {
                //                        Console.WriteLine("Other attributes are:  :" + cells[k].InnerText + "<br>");

                //                    }
                //                }
                //                //}
                //            }
                //        }



                //        foreach (HtmlNode row in htmlDoc.DocumentNode.Descendants("table"))
                //        {
                //            HtmlNodeCollection cells = row.SelectNodes("td");
                //            if (cells != null)
                //            {
                //                for (int k = 0; k < cells.Count; ++k)
                //                {
                //                    if (k == 0)
                //                    {
                //                        Console.WriteLine("Colnm :" + cells[k].InnerText + "<br>");
                //                    }
                //                    else
                //                    {
                //                        Console.WriteLine("Other attributes are:  :" + cells[k].InnerText + "<br>");

                //                    }
                //                }
                //            }
                //        }




                //        //HtmlDocument doc = new HtmlDocument();
                //        //doc.LoadHtml(builder.ToString());
                //        //foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//html/body/div[2]/div[2]/table/tbody/tr/td/center/div[2]/table/tbody/tr[5]/td/table"))
                //        //{
                //        //    Console.WriteLine("Found: " + table.Id);
                //        //    foreach (HtmlNode row in table.SelectNodes("tr"))
                //        //    {
                //        //        Console.WriteLine("row");
                //        //        foreach (HtmlNode cell in row.SelectNodes("th|td"))
                //        //        {
                //        //            Console.WriteLine("cell: " + cell.InnerText);
                //        //        }
                //        //    }
                //        //}

                //        //var query = from table in doc.DocumentNode.SelectNodes("//table").Cast<HtmlNode>()
                //        //            from row in table.SelectNodes("tr").Cast<HtmlNode>()
                //        //            from cell in row.SelectNodes("th|td").Cast<HtmlNode>()
                //        //            select new { Table = table.Id, CellText = cell.InnerText };
                //        /////html/body/div[2]/div[2]/table/tbody/tr/td/center/div[2]/table/tbody/tr[5]/td/table/tbody/tr[1]/td[3]

                //        //foreach (var cell in query)
                //        //{
                //        //    Console.WriteLine("{0}: {1}", cell.Table, cell.CellText);
                //        //}

                //        // var testt = from table in doc.DocumentNode.SelectNodes("//html/body/div[2]/div[2]/table/tbody/tr/td/center/div[2]/table/tbody/tr[5]/td/table").Cast<HtmlNode>();

                //    }


                //    //if (plainHtml != null)
                //    //{
                //    //    builder.Append(plainHtml.GetBodyAsText());
                //    //}




                //    //foreach (HtmlNode table in docc.DocumentNode.SelectNodes("//*[@id=':9n']/div[1]/div[3]/blockquote/div[1]/table/tbody/tr/td/table/tbody/tr[1]/td/table/tbody/tr[6]/td[1]/table/tbody/tr[1]/td[2]/table/tbody/tr/td/table/tbody/tr[3]/td[2]/table[1]"))
                //    //{
                //    //    Console.WriteLine("Found: " + table.Id);
                //    //    foreach (HtmlNode row in table.SelectNodes("tr"))
                //    //    {
                //    //        Console.WriteLine("row");
                //    //        foreach (HtmlNode cell in row.SelectNodes("th|td"))
                //    //        {
                //    //            Console.WriteLine("cell: " + cell.InnerText);
                //    //        }
                //    //    }
                //    //}

                //    ////*[@id=":9n"]/div[1]/div[3]/blockquote/div[1]/table/tbody/tr/td/table/tbody/tr[1]/td/table/tbody/tr[6]/td[1]/table/tbody/tr[1]/td[2]/table/tbody/tr/td/table/tbody/tr[3]/td[2]/table[1]

                //    bool IsSkiped = false;
                //    if (IsSkiped)
                //    {
                //        OpenPop.Mime.MessagePart plainText = EmailMessage.FindFirstPlainTextVersion();
                //        if (plainText != null)
                //        {
                //            // We found some plaintext!
                //            builder.Append(plainText.GetBodyAsText());
                //        }
                //        EnquiryModl EnqModel = new EnquiryModl();
                //        using (StringReader reader = new StringReader(builder.ToString()))
                //        {
                //            //------------------Enquiry Reading from Sulekha-------------
                //            //if (DTSemail.EmailReceivedFrom.Contains(SulekhaEmailFromID))
                //            Helper.GetSulekhEnquiry(reader, DTSemail.EmailReceivedDateTime);


                //            //---------------------------

                //        }
                //    }
            }
            WriteLog("Email Utility completed");
        }

        private DateTime getEmailDate(Message message)
        {
            string mailHeaderIdentifier = ConfigurationManager.AppSettings.Get("MailHeaderIdentifier").ToString();
            DateTime mailDateTime = new DateTime(0001, 01, 1);

            for (int i = 0; i < message.Headers.Received.Count; i++)
            {
                if (!string.IsNullOrEmpty(mailHeaderIdentifier))
                {
                    string[] hIdentifiers = mailHeaderIdentifier.Split(',');

                    for (int j = 0; j < hIdentifiers.Length; j++)
                    {
                        if (message.Headers.Received[i].Raw.ToString().ToLower().Trim().Contains(hIdentifiers[j].Trim().ToLower()))
                        {
                            if (mailDateTime != null)
                            {
                                if (message.Headers.Received[i].Date.ToLocalTime().CompareTo(mailDateTime) >= 0)
                                    mailDateTime = message.Headers.Received[i].Date.ToLocalTime();
                            }
                            else
                                mailDateTime = message.Headers.Received[i].Date.ToLocalTime();
                        }
                    }
                }
            }
            if (mailDateTime.CompareTo(new DateTime(0001, 01, 1)) == 0)
                mailDateTime = message.Headers.DateSent.ToLocalTime();

            return mailDateTime;
        }

        private string CreateDirectory(string tempDir)
        {
            try
            {
                string temp = tempDir + "\\" + Guid.NewGuid().ToString();
                Directory.CreateDirectory(temp);
                tempDir = temp;

            }
            catch (Exception ex)
            {
                WriteLog("Unable to create directory " + Guid.NewGuid().ToString() + " Error " + ex.ToString());
                Console.Write("Unable to create directory " + Guid.NewGuid().ToString() + " Error " + ex.ToString());
            }
            return tempDir;
        }



        private DataTable getDataTable(string procedureName, SqlParameter sqlparameter)
        {
            DataTable dt = new DataTable();
            string connString = ConfigurationManager.AppSettings["ConnectionString"];
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(procedureName);

            if (sqlparameter != null)
                cmd.Parameters.Add(sqlparameter);


            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;

            try
            {
                conn.Open();
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                WriteLog("Error " + ex.ToString());
            }
            finally { conn.Close(); }

            return dt;
        }

        private string GenerateTimestamp(DTSEmail dtsemail)
        {
            string nextTimestamp = "";
            try
            {
                DataTable dtTSSetting = getDataTable("getTimeStampSetting", null);

                if (dtTSSetting.Rows.Count > 0)
                {
                    string suffix = dtTSSetting.Rows[0]["Suffix"].ToString();
                    string prefix = dtTSSetting.Rows[0]["Prefix"].ToString();
                    string stampId = dtTSSetting.Rows[0]["LastStampId"].ToString();
                    string dateFormat = dtTSSetting.Rows[0]["DateFormat"].ToString();

                    stampId = (Convert.ToInt32(stampId) + 1).ToString("000000");
                    dtsemail.StampId = stampId;
                    //UTEMAIL02-09-16 13:11:38_011082 
                    nextTimestamp = prefix + dtsemail.EmailReceivedDateTime.ToString(dateFormat) + suffix + stampId;
                }
            }
            catch (Exception ex)
            {
                WriteLog("Unable to generate timestamp. Error is " + ex.ToString());
                Console.WriteLine("Unable to generate timestamp. Error is " + ex.ToString());
            }
            return nextTimestamp;
        }

        private void UpdateStampId(string stampId)
        {
            string connString = ConfigurationManager.AppSettings["ConnectionString"];
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("UpdateTimeStampSetting", conn);
            try
            {
                cmd.Parameters.Add("@StampId", stampId);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                WriteLog("Unable to Update stampId. Error is " + ex.ToString());
                Console.WriteLine("Unable to Update stampId. Error is " + ex.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        private void PrepareEmails(DTSEmail dtsEmails)
        {
            List<string> toRecipients = new List<string>();
            List<string> ccRecipients = new List<string>();
            string templateDir = ConfigurationManager.AppSettings["EmailTemplateDir"];
            string defaultcc = ConfigurationManager.AppSettings["DefaultCCRecipients"];
            string defaultNotRegcc = ConfigurationManager.AppSettings["DefaultNotRegistredCCRecipients"];
            string subject = "";
            string emailBody = "";
            string[] tempCC;

            switch (dtsEmails.Status)
            {
                case EmailStatus.InvalidAttachmentFormat:
                    {
                        toRecipients.Add(dtsEmails.EmailReceivedFrom);

                        if (dtsEmails.ClientEmailIds != null)
                            for (int i = 0; i < dtsEmails.ClientEmailIds.Count; i++)
                            {
                                toRecipients.Add(dtsEmails.ClientEmailIds[i]);
                            }

                        if (dtsEmails.RMEmailId != null)
                            ccRecipients.Add(dtsEmails.RMEmailId);

                        tempCC = defaultcc.Split(',');
                        for (int a = 0; a < tempCC.Length; a++)
                        {
                            ccRecipients.Add(tempCC[a]);
                        }

                        if (dtsEmails.EmailReceiveFromCC != null)
                            for (int k = 0; k < dtsEmails.EmailReceiveFromCC.Count; k++)
                            {
                                ccRecipients.Add(dtsEmails.EmailReceiveFromCC[k]);
                            }

                        if (dtsEmails.CCEmailIds != null)
                            for (int i = 0; i < dtsEmails.CCEmailIds.Count; i++)
                            {
                                ccRecipients.Add(dtsEmails.CCEmailIds[i]);
                            }


                        if (!String.IsNullOrEmpty(dtsEmails.InvestorName))
                            subject = "Invalid format / No attachment-E-mail received from " + dtsEmails.InvestorName + " by KR";
                        else
                            subject = "Invalid format / No attachment-E-mail received from " + dtsEmails.EmailReceivedFrom + " by KR";


                        if (File.Exists(templateDir + "\\" + "Email_reply_exception.html"))
                        {
                            using (StreamReader reader = new StreamReader(templateDir + "\\" + "Email_reply_exception.html"))
                            {
                                emailBody = reader.ReadToEnd();
                            }

                            if (dtsEmails.OriginalEmail != null)
                            {
                                MessagePart originalBody = dtsEmails.OriginalEmail.FindFirstHtmlVersion();

                                if (originalBody != null)
                                    emailBody = emailBody.Replace("{EmailBody}", originalBody.GetBodyAsText());
                                else
                                {
                                    originalBody = dtsEmails.OriginalEmail.FindFirstPlainTextVersion();

                                    if (originalBody != null)
                                        emailBody = emailBody.Replace("{EmailBody}", originalBody.GetBodyAsText());
                                }
                            }
                        }
                        else
                            Console.WriteLine("Email template " + templateDir + "\\Email_reply_exception.html does not exists.");

                        List<string> aTemp = new List<string>();
                        for (int n = 0; n < dtsEmails.EmailAttachment.Count; n++)
                        {
                            aTemp.Add(dtsEmails.EmailAttachment[n].FileName.Replace(",", "").Replace(",,", "").Replace("&", "").Replace("!", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("/", "").Replace("+", "").Replace("-", "").Replace("=", "").Replace("~", "").Replace(";", "").Replace("\"", "").Replace("\'", "").Replace("\t", ""));
                        }

                        SendEmail(toRecipients.ToArray(), ccRecipients.ToArray(), subject, emailBody, aTemp.ToArray(), dtsEmails.AttachmentDownloadDirectory);
                    }
                    break;
                case EmailStatus.NoAttachments:
                    {
                        toRecipients.Add(dtsEmails.EmailReceivedFrom);

                        if (dtsEmails.ClientEmailIds != null)
                            for (int i = 0; i < dtsEmails.ClientEmailIds.Count; i++)
                            {
                                toRecipients.Add(dtsEmails.ClientEmailIds[i]);
                            }

                        if (dtsEmails.RMEmailId != null)
                            ccRecipients.Add(dtsEmails.RMEmailId);

                        tempCC = defaultcc.Split(',');
                        for (int a = 0; a < tempCC.Length; a++)
                        {
                            ccRecipients.Add(tempCC[a]);
                        }

                        if (dtsEmails.EmailReceiveFromCC != null)
                            for (int k = 0; k < dtsEmails.EmailReceiveFromCC.Count; k++)
                            {
                                ccRecipients.Add(dtsEmails.EmailReceiveFromCC[k]);
                            }


                        if (dtsEmails.CCEmailIds != null)
                            for (int i = 0; i < dtsEmails.CCEmailIds.Count; i++)
                            {
                                ccRecipients.Add(dtsEmails.CCEmailIds[i]);
                            }


                        if (!String.IsNullOrEmpty(dtsEmails.InvestorName))
                            subject = "No attachment / Invalid format-E-mail received from " + dtsEmails.InvestorName + " by KR";
                        else
                            subject = "No attachment / Invalid format-E-mail received from " + dtsEmails.EmailReceivedFrom + " by KR";


                        if (File.Exists(templateDir + "\\" + "Email_reply_exception.html"))
                        {
                            using (StreamReader reader = new StreamReader(templateDir + "\\" + "Email_reply_exception.html"))
                            {
                                emailBody = reader.ReadToEnd();
                            }

                            MessagePart originalBody = dtsEmails.OriginalEmail.FindFirstHtmlVersion();

                            if (originalBody != null)
                                emailBody = emailBody.Replace("{EmailBody}", originalBody.GetBodyAsText());
                            else
                            {
                                originalBody = dtsEmails.OriginalEmail.FindFirstPlainTextVersion();

                                if (originalBody != null)
                                    emailBody = emailBody.Replace("{EmailBody}", originalBody.GetBodyAsText());
                            }
                        }
                        else
                            Console.WriteLine("Email template " + templateDir + "\\Email_reply_exception.html does not exists.");

                        SendEmail(toRecipients.ToArray(), ccRecipients.ToArray(), subject, emailBody, null);
                    }
                    break;
                case EmailStatus.RecipientNotRegistered:
                    {
                        //toRecipients.Add(dtsEmails.EmailReceivedFrom);
                        //if (dtsEmails.EmailReceiveFromCC != null)
                        //    for (int k = 0; k < dtsEmails.EmailReceiveFromCC.Count; k++)
                        //    {
                        //        ccRecipients.Add(dtsEmails.EmailReceiveFromCC[k]);
                        //    }

                        tempCC = defaultNotRegcc.Split(',');
                        for (int a = 0; a < tempCC.Length; a++)
                        {
                            toRecipients.Add(tempCC[a]);
                        }

                        subject = "E-mail received from " + dtsEmails.EmailReceivedFrom + " by KR";


                        if (File.Exists(templateDir + "\\" + "Email_reply.html"))
                        {
                            using (StreamReader reader = new StreamReader(templateDir + "\\" + "Email_reply.html"))
                            {
                                emailBody = reader.ReadToEnd();
                            }

                            emailBody = emailBody.Replace("{EmailTimeDate}", dtsEmails.EmailReceivedDateTime.ToString("dd-MMM-yyyy HH:mm:ss"));
                            emailBody = emailBody.Replace("{atMobileNo}", dtsEmails.RMMobileNumber);
                            emailBody = emailBody.Replace("{attachment}", dtsEmails.AttachmentNames.Count.ToString());
                            emailBody = emailBody.Replace("{page}", dtsEmails.PageCount.ToString());
                        }
                        else
                            Console.WriteLine("Email template " + templateDir + "\\Email_reply.html does not exists.");

                        SendEmail(toRecipients.ToArray(), null, subject, emailBody, dtsEmails.AttachmentNames.ToArray(), dtsEmails.TimestampDirectory);
                    }
                    break;
                case EmailStatus.RegisteredANDTimestamped:
                    {
                        toRecipients.Add(dtsEmails.EmailReceivedFrom);

                        if (dtsEmails.ClientEmailIds != null)
                            for (int i = 0; i < dtsEmails.ClientEmailIds.Count; i++)
                            {
                                toRecipients.Add(dtsEmails.ClientEmailIds[i]);
                            }

                        if (dtsEmails.RMEmailId != null)
                            ccRecipients.Add(dtsEmails.RMEmailId);

                        tempCC = defaultcc.Split(',');
                        for (int a = 0; a < tempCC.Length; a++)
                        {
                            ccRecipients.Add(tempCC[a]);
                        }

                        if (dtsEmails.EmailReceiveFromCC != null)
                            for (int k = 0; k < dtsEmails.EmailReceiveFromCC.Count; k++)
                            {
                                ccRecipients.Add(dtsEmails.EmailReceiveFromCC[k]);
                            }

                        if (dtsEmails.CCEmailIds != null)
                            for (int i = 0; i < dtsEmails.CCEmailIds.Count; i++)
                            {
                                ccRecipients.Add(dtsEmails.CCEmailIds[i]);
                            }

                        if (!String.IsNullOrEmpty(dtsEmails.InvestorName))
                            subject = "E-mail received from " + dtsEmails.InvestorName + " by KR";
                        else if (!String.IsNullOrEmpty(dtsEmails.GroupInvestorName) && dtsEmails.GroupInvestorName != "Investor")
                            subject = "E-mail received from" + dtsEmails.GroupInvestorName + " by KR";
                        else
                            subject = "E-mail received from " + dtsEmails.EmailReceivedFrom + " by KR";



                        if (File.Exists(templateDir + "\\" + "Email_reply.html"))
                        {
                            using (StreamReader reader = new StreamReader(templateDir + "\\" + "Email_reply.html"))
                            {
                                emailBody = reader.ReadToEnd();
                            }

                            if (!String.IsNullOrEmpty(dtsEmails.DistributorId))
                            {
                                if (Convert.ToInt32(dtsEmails.DistributorId) > 0)
                                    emailBody = emailBody.Replace("{DistributorORInvestor}", "Distributor");
                                else
                                    emailBody = emailBody.Replace("{DistributorORInvestor}", "Investor");
                            }
                            else
                                emailBody = emailBody.Replace("{DistributorORInvestor}", "Investor");

                            if (!String.IsNullOrEmpty(dtsEmails.InvestorName))
                                emailBody = emailBody.Replace("{ClientName}", dtsEmails.InvestorName);
                            else
                            {
                                if (!String.IsNullOrEmpty(dtsEmails.GroupInvestorName) && dtsEmails.GroupInvestorName != "Investor")
                                    emailBody = emailBody.Replace("{ClientName}", dtsEmails.GroupInvestorName);
                                else
                                    emailBody = emailBody.Replace("{ClientName}", " you");

                            }

                            emailBody = emailBody.Replace("{EmailTimeDate}", dtsEmails.EmailReceivedDateTime.ToString("dd-MMM-yyyy HH:mm:ss"));
                            emailBody = emailBody.Replace("{atMobileNo}", dtsEmails.RMMobileNumber);
                            emailBody = emailBody.Replace("{attachment}", dtsEmails.AttachmentNames.Count.ToString());
                            emailBody = emailBody.Replace("{page}", dtsEmails.PageCount.ToString());
                        }
                        else
                            Console.WriteLine("Email template " + templateDir + "\\Email_reply.html does not exists.");

                        SendEmail(toRecipients.ToArray(), ccRecipients.ToArray(), subject, emailBody, dtsEmails.AttachmentNames.ToArray(), dtsEmails.TimestampDirectory);
                    }
                    break;
                default:
                    break;
            }
        }

        private void SendEmail(string[] toRecipients, string[] ccRecipients, string subject, string emailBody, string[] attachments, string attachmentDir = "")
        {
            string PrimaryMailServer = ConfigurationManager.AppSettings["PrimaryMailServer"];
            string SecondryMailServer = ConfigurationManager.AppSettings["SecondryMailServer"];
            string SenderEmailId = "";
            int serverFlag = 0;

            if (ccRecipients == null)
                SenderEmailId = ConfigurationManager.AppSettings["DTSAdmin"];
            else
                SenderEmailId = ConfigurationManager.AppSettings["SenderEmailId"];

            string EmailTemplateDir = ConfigurationManager.AppSettings["EmailTemplateDir"];
            SmtpClient smtpClient;

            retry:
            if (serverFlag == 2)
                return;
            if (serverFlag == 0)
                smtpClient = new SmtpClient(PrimaryMailServer);
            else
                smtpClient = new SmtpClient(SecondryMailServer);

            try
            {

                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                MailAddress mailAddress = new MailAddress(SenderEmailId);
                message.Sender = mailAddress;
                message.From = mailAddress;
                message.Body = emailBody;
                message.Subject = subject;

                if (toRecipients != null)
                {
                    toRecipients = toRecipients.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    for (int i = 0; i < toRecipients.Length; i++)
                    {
                        if (!toRecipients[i].Contains("kreality00@gmail.com"))
                            message.To.Add(toRecipients[i]);
                    }
                }

                if (ccRecipients != null)
                {
                    ccRecipients = ccRecipients.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    for (int i = 0; i < ccRecipients.Length; i++)
                    {
                        if (!ccRecipients[i].Contains("kreality00@gmail.com"))
                            message.CC.Add(ccRecipients[i]);
                    }
                }

                if (attachments != null)
                    for (int k = 0; k < attachments.Length; k++)
                    {
                        if (File.Exists(attachmentDir + "\\" + attachments[k]))
                        {
                            System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(attachmentDir + "\\" + attachments[k]);
                            message.Attachments.Add(attach);
                        }
                    }

                smtpClient.Send(message);
                smtpClient.Dispose();
            }
            catch (SmtpException se)
            {
                serverFlag++;
                WriteLog("Error sending email via secondry mail sever " + se.ToString());
                Console.WriteLine("Error sending email " + se.ToString());
                goto retry;
            }
            catch (Exception ex)
            {
                WriteLog("Error sending email via primary mail server " + ex.ToString());
                Console.WriteLine("Error sending email " + ex.ToString());
            }
        }



        private void PrepareSMS(DTSEmail dtsEmail)
        {
            string smsFTPurl = ConfigurationManager.AppSettings["smsFTPurl"].ToString();
            string smsFTPusername = ConfigurationManager.AppSettings["smsFTPusername"].ToString();
            string smsFTPpass = ConfigurationManager.AppSettings["smsFTPpass"].ToString();
            string mobileno = "";

            string messageText = "Dear " + dtsEmail.SMSInvestorName.Replace("&", "") + ", your transaction has been received by UTI. Please contact 022-66786258 or your RM for any queries.";

            try
            {
                for (int i = 0; i < dtsEmail.ClientCellnumbers.Count + 1; i++)
                {
                    if (i >= dtsEmail.ClientCellnumbers.Count)
                        mobileno = dtsEmail.RMMobileNumber;
                    else
                        mobileno = dtsEmail.ClientCellnumbers[i];

                    Guid id = Guid.NewGuid();
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(smsFTPurl + "BkQyeryFaxSMS" + id + i.ToString() + ".txt");
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(smsFTPusername, smsFTPpass);

                    byte[] fileContents = Encoding.UTF8.GetBytes(mobileno + "," + messageText);
                    request.ContentLength = fileContents.Length;

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(fileContents, 0, fileContents.Length);
                    requestStream.Close();

                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    string strMessage = response.StatusDescription;
                }
            }
            catch (Exception ex)
            {
                WriteLog("Unable to  create or send sms." + ex.ToString());
                Console.WriteLine("Unable to  create or send sms." + ex.ToString());
            }
        }

        private void WriteLog(string message)
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

        private void PushToDatabase(DTSEmail dtsemail)
        {
            string filenames = "";
            string defaultcc = ConfigurationManager.AppSettings["DefaultCCRecipients"];
            string[] tempcc;
            try
            {
                for (int k = 0; k < dtsemail.AttachmentNames.Count; k++)
                {
                    filenames += dtsemail.AttachmentNames[k] + ",";
                }
                filenames = filenames.TrimEnd(',');

                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString").ToString()))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "InsertUpdateFaxRecords";
                    cmd.Parameters.AddWithValue("FaxSeq", 0);
                    cmd.Parameters.AddWithValue("Clientid", dtsemail.ClientId);
                    cmd.Parameters.AddWithValue("Name", dtsemail.InvestorName);
                    cmd.Parameters.AddWithValue("FaxGUID", 0);
                    cmd.Parameters.AddWithValue("FolderPath", dtsemail.ArchivedFolder + "\\");
                    cmd.Parameters.AddWithValue("Timestamp", dtsemail.TimeStamp);
                    cmd.Parameters.AddWithValue("Pagecount", dtsemail.PageCount);
                    cmd.Parameters.AddWithValue("Callingno", 0);
                    cmd.Parameters.AddWithValue("DistributorId", dtsemail.DistributorId);
                    cmd.Parameters.AddWithValue("Createtime", Convert.ToDateTime(dtsemail.EmailReceivedDateTime).ToString("dd-MMM-yyyy hh:mm"));
                    cmd.Parameters.AddWithValue("FromEmail", dtsemail.EmailReceivedFrom);
                    cmd.Parameters.AddWithValue("FileName", filenames);
                    if (dtsemail.GroupInvestorName != null && dtsemail.GroupInvestorName.Trim().Length > 1)
                        cmd.Parameters.AddWithValue("IsGroup", 1);
                    else
                        cmd.Parameters.AddWithValue("IsGroup", 0);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog("Unable to insert record into tblfax. " + ex.ToString());
                Console.WriteLine("Unable to Insert Record into database. Error is " + ex.ToString());
            }


            try
            {
                string emailids = "";
                string mobilenos = "";
                string action = "Reply";
                string remark = "";

                if (dtsemail.EmailReceivedFrom != null)
                    emailids = dtsemail.EmailReceivedFrom + ",";

                if (dtsemail.ClientEmailIds != null)
                    for (int i = 0; i < dtsemail.ClientEmailIds.Count; i++)
                    {
                        emailids += dtsemail.ClientEmailIds[i] + ",";
                    }

                if (dtsemail.CCEmailIds != null)
                    for (int m = 0; m < dtsemail.CCEmailIds.Count; m++)
                    {
                        emailids += dtsemail.CCEmailIds[m] + ",";
                    }

                tempcc = defaultcc.Split(',');
                for (int k = 0; k < tempcc.Length; k++)
                {
                    emailids += tempcc[k] + ",";
                }

                if (dtsemail.RMEmailId != null)
                    emailids += dtsemail.RMEmailId;

                if (dtsemail.ClientCellnumbers != null)
                    for (int j = 0; j < dtsemail.ClientCellnumbers.Count; j++)
                    {
                        mobilenos += dtsemail.ClientCellnumbers[j] + ",";
                    }
                if (dtsemail.RMMobileNumber != null)
                    mobilenos += dtsemail.RMMobileNumber;

                if (dtsemail.Status == EmailStatus.RecipientNotRegistered)
                {
                    remark = "Client Detail not found";
                    action = "NotRegisteredMailID";
                }

                string strconn = ConfigurationManager.AppSettings.Get("ConnectionString").ToString();
                using (SqlConnection con = new SqlConnection(strconn))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "InsertTransactionAudit";
                    cmd.Parameters.AddWithValue("Utility", "TS");
                    cmd.Parameters.AddWithValue("Clientid", dtsemail.ClientId);
                    cmd.Parameters.AddWithValue("Name", dtsemail.InvestorName);
                    cmd.Parameters.AddWithValue("FaxGUID", "0");
                    cmd.Parameters.AddWithValue("FolderPath", dtsemail.ArchivedFolder + "\\");
                    cmd.Parameters.AddWithValue("Timestamp", dtsemail.TimeStamp);
                    cmd.Parameters.AddWithValue("Pagecount", dtsemail.PageCount);
                    cmd.Parameters.AddWithValue("Callingno", 0);
                    cmd.Parameters.AddWithValue("Createtime", Convert.ToDateTime(dtsemail.EmailReceivedDateTime).ToString("dd-MMM-yyyy hh:mm"));
                    cmd.Parameters.AddWithValue("Filename", filenames);
                    cmd.Parameters.AddWithValue("Emailids", emailids.TrimEnd(','));
                    cmd.Parameters.AddWithValue("Attachments", dtsemail.AttachmentNames.Count);
                    cmd.Parameters.AddWithValue("SMSnumbers", mobilenos.TrimEnd(','));
                    cmd.Parameters.AddWithValue("Actions", action);
                    cmd.Parameters.AddWithValue("Remarks", remark);
                    cmd.Parameters.AddWithValue("FromEmail", dtsemail.EmailReceivedFrom);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                WriteLog("Unable to insert record into tblTransaction_audit. " + ex.ToString());
                Console.WriteLine("Unable to Insert Record into database. Error is " + ex.ToString());
            }
        }

        private void ArchiveFiles(DTSEmail dtsEmail)
        {
            string FileArchivePath = ConfigurationManager.AppSettings["FileArchivePath"];
            string strDate = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();

            if (!Directory.Exists(FileArchivePath + "\\" + strDate))
                Directory.CreateDirectory(FileArchivePath + "\\" + strDate);

            int i = 0;
            bool blnFolderCount = true;
            while (blnFolderCount)
            {
                ++i;
                if (!Directory.Exists(FileArchivePath + "\\" + strDate + "\\" + i.ToString()))
                {
                    Directory.CreateDirectory(FileArchivePath + "\\" + strDate + "\\" + i.ToString());
                    FileArchivePath = FileArchivePath + "\\" + strDate + "\\" + i.ToString();
                    blnFolderCount = false;
                }
            }

            for (int j = 0; j < dtsEmail.AttachmentNames.Count; j++)
            {
                try
                {
                    if (File.Exists(dtsEmail.TimestampDirectory + "\\" + dtsEmail.AttachmentNames[j]))
                    {
                        File.Copy(dtsEmail.TimestampDirectory + "\\" + dtsEmail.AttachmentNames[j], FileArchivePath + "\\" + dtsEmail.AttachmentNames[j], true);
                        dtsEmail.ArchivedFolder = FileArchivePath;
                    }
                }
                catch (Exception ex)
                {
                    WriteLog("Unable to archive files " + ex.ToString());
                    Console.WriteLine("Unable to archive files " + ex.ToString());
                }
            }
        }

        private void RemovePrints(DirectoryInfo dirInfo)
        {
            try
            {
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo subfolder in dirInfo.GetDirectories())
                {
                    RemovePrints(subfolder);
                }
                dirInfo.Delete(true);
            }
            catch (Exception ex)
            {

            }
        }

        private void PreserveEmail(Message message, string MessageUid)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings.Get("ConnectionString").ToString()))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "InsertEmailUtilityEmailAudit";
                    cmd.Parameters.AddWithValue("EmailId", message.Headers.From.Address.ToString());
                    cmd.Parameters.AddWithValue("EmailInfo", message.Headers.From.ToString());
                    cmd.Parameters.AddWithValue("MessageUid", MessageUid);
                    cmd.Parameters.AddWithValue("EmailReceivedDate", getEmailDate(message).ToString("dd-MMM-yyyy HH:mm:ss"));
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to Preserve Email. " + ex.ToString());
            }
        }
    }
}
