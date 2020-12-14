using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EmailUtility.Class
{
    public class Housing
    {
        public static void GetHousingEnquiry(string htmlstring, DateTime EnquiryDate, DateTime EmailReceivedDate)
        {
            try
            {
                EnquiryModl EnqModel = new EnquiryModl();
                StringBuilder sb = new StringBuilder();
                List<string> list_equiry = new List<string>();
                EnqModel.EnquiryDate = EnquiryDate;
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlstring);
                List<string> liststring = new List<string>();
                int CheckCounter = 0;
                foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
                {
                    string tabkeString = Regex.Replace(table.InnerText, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
                    string line;
                    using (StringReader reader = new StringReader(tabkeString))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            bool isnoadd = false;
                            line = line.Replace(">", "");
                            string[] values = line.Split(':');
                            if (values.Length > 1)
                            {
                                if (line.Contains("Name:"))
                                {
                                    isnoadd = true;
                                    list_equiry.Add("Name:");
                                    //EnqModel.Name = values[1];
                                    CheckCounter++;
                                }
                                if (line.Contains("Number:"))
                                {
                                    isnoadd = true;
                                    list_equiry.Add("Number:");
                                    //EnqModel.phone = values[1];
                                    CheckCounter++;
                                }
                                if (line.Contains("Email:"))
                                {
                                    isnoadd = true;
                                    list_equiry.Add("Email:");
                                    //EnqModel.Email = values[1];
                                    CheckCounter++;
                                }

                            }
                            if (list_equiry.Count > 0 && isnoadd == false)
                            {
                                if (list_equiry.Count >= 6)
                                {
                                    if (!string.IsNullOrWhiteSpace(line.Trim()))
                                        sb.AppendLine(line.Trim());
                                    list_equiry.Add(line.Trim());
                                }
                                else
                                {
                                    if (!string.IsNullOrWhiteSpace(line.Trim()))
                                        list_equiry.Add(line.Trim());
                                }
                            }
                            if (list_equiry.Count == 15)
                            {
                                EnqModel.Name = string.IsNullOrWhiteSpace(list_equiry[1]) ? "" : list_equiry[1];
                                EnqModel.Email = string.IsNullOrWhiteSpace(list_equiry[3]) ? "" : list_equiry[3];
                                EnqModel.phone = string.IsNullOrWhiteSpace(list_equiry[5]) ? "" : list_equiry[5];
                                EnqModel.EnquiryDate = EmailReceivedDate;
                                EnqModel.Email_body = htmlstring;
                                EnqModel.additional_Info = sb.ToString();
                                EnqModel.EnqSoure = "Housing";

                                Helper.InsertInquery(EnqModel);
                                break;
                            }

                        }
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                Helper.WriteLog("error in function [GetHousingEnquiry] " + ex.Message);
            }
        }
    }
}