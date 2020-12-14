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
using Npgsql;

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
            string[] enquiry_sources_list = ConfigurationManager.AppSettings["enquiry_sources_list"].Split(',');

            int TotalEmailCount = 0;

            Message EmailMessage = null;
            DTSEmail DTSemail = new DTSEmail();
            DataTable dtExisting = new DataTable();
            dtExisting = Helper.executeSelect(GetQuery.Get_Select_Email_Information, null);
            DataRow[] aRow;
            Pop3Client PopClient = new Pop3Client();
            List<string> mailCcIds = new List<string>();
            List<string> messageUids = new List<string>();
            int currentMessageId = 0;

            try
            {
                currentMessageId = 0;
                Helper.WriteLog("Email Utility started");
                PopClient.Connect(PrimaryMailServer, Convert.ToInt16(MailPort), IsSSL);
                PopClient.Authenticate(CorporateEmail, CorporatPass, AuthenticationMethod.UsernameAndPassword);
                TotalEmailCount = PopClient.GetMessageCount();
                messageUids = PopClient.GetMessageUids();

            }
            catch (Exception ex)
            {
                try
                {
                    Helper.WriteLog("Unable to connect to Mail server " + PrimaryMailServer + " Error " + ex.ToString());
                    PopClient.Connect(SecondryMailServer, Convert.ToInt16(MailPort), false);
                    PopClient.Authenticate(CorporateEmail, CorporatPass, AuthenticationMethod.UsernameAndPassword);
                    TotalEmailCount = PopClient.GetMessageCount();
                    messageUids = PopClient.GetMessageUids();
                }
                catch (Exception ex2)
                {
                    Helper.WriteLog("Email Utility trying to connect to mail server " + PrimaryMailServer + " and " + SecondryMailServer + " Error " + ex2.ToString());
                    return;
                }
            }


            // for (int i = 0; i < messageUids.Count; i++)
            for (int i = messageUids.Count; i-- > 0;)
            {
                try
                {
                    DTSemail = new DTSEmail();
                    mailCcIds = new List<string>();
                    currentMessageId = i + 1;

                    aRow = dtExisting.Select("MessageUid = '" + messageUids[i] + "'");
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
                    continue;
                }

                DTSemail.EmailReceivedDateTime = getEmailDate(EmailMessage);
                if (DTSemail.EmailReceivedDateTime.Date.CompareTo(DateTime.Now.Date) != 0)
                    continue;

                DTSemail.EmailReceivedFrom = EmailMessage.Headers.From.Address.ToString();

                if (enquiry_sources_list.Any(str => str.Contains(DTSemail.EmailReceivedFrom)))
                {
                    StringBuilder sb_html_text = new StringBuilder();
                    OpenPop.Mime.MessagePart plainHtml = EmailMessage.FindFirstHtmlVersion();
                    HtmlDocument docc = new HtmlDocument();

                    byte[] htmlbody = plainHtml.Body;
                    string emailbody = System.Text.Encoding.UTF8.GetString(htmlbody);
                    sb_html_text.AppendLine(emailbody);
                    StringBuilder sb_plain_text = new StringBuilder();
                    OpenPop.Mime.MessagePart plainText = EmailMessage.FindFirstPlainTextVersion();
                    if (plainText != null)
                    {
                        sb_plain_text.Append(plainText.GetBodyAsText());
                    }
                    Helper.CheckEnquirySource(DTSemail.EmailReceivedFrom, DTSemail.EmailReceivedDateTime, sb_html_text.ToString(), sb_plain_text.ToString());
                }
            }
            Helper.WriteLog("Email Utility completed");
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
                Helper.WriteLog("Unable to create directory " + Guid.NewGuid().ToString() + " Error " + ex.ToString());
                Console.Write("Unable to create directory " + Guid.NewGuid().ToString() + " Error " + ex.ToString());
            }
            return tempDir;
        }

        private void PreserveEmail(Message message, string MessageUid)
        {

            NpgsqlParameter[] Insert_Parameters = {
                                             new NpgsqlParameter("pemailid",message.Headers.From.Address.ToString()),
                                             new NpgsqlParameter("pmessageuid",MessageUid),
                                             new NpgsqlParameter("pemailrevieveddate",getEmailDate(message)),
                                             new NpgsqlParameter("pemailinfo",message.Headers.From.ToString())
            };
            string Response = Helper.ExecuteProcedure("select ems_insertemailutilityaudit(:pemailid,:pmessageuid,:pemailrevieveddate,:pemailinfo)", Insert_Parameters);
        }
    }
}
