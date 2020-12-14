using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EmailUtility.Class
{
    public class _99acres
    {
        public static void GetEnquiry99Acrs(string EnqText, DateTime EmailReceivedDate)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(EnqText.ToString());
                List<string> liststring = new List<string>();

                foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
                {
                    string tabkeString = Regex.Replace(table.InnerText, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
                    EnquiryModl OWM = new EnquiryModl();
                    StringBuilder sb = new StringBuilder();
                    string line;
                    bool IsOk2 = false;
                    bool IsInsert = false;
                    using (StringReader reader = new StringReader(tabkeString))
                    {
                        int line_count = 0;
                        List<string> list_enquiry = new List<string>();
                        while ((line = reader.ReadLine()) != null)
                            if (!string.IsNullOrEmpty(line.Trim()))
                            {
                                line = line.Trim();
                                if (line.Trim().Contains("Property Advertisement Query"))
                                {
                                    IsOk2 = true;
                                    continue;
                                }
                                if (IsOk2 && !string.IsNullOrWhiteSpace(line.Trim()) && line_count <= 10)
                                {
                                    if (line.Contains("@") && line.Contains("+91"))
                                    {
                                        string[] line2 = System.Text.RegularExpressions.Regex.Split(line, @"\s{2,}");

                                        list_enquiry.Add(line2[0]);
                                        list_enquiry.Add(line2[1]);
                                    }
                                    else
                                    {
                                        list_enquiry.Add(line.Trim());
                                    }
                                    line_count++;
                                }
                                int index_count = 0;
                                if (line_count == 10)
                                {
                                    foreach (string detail in list_enquiry)
                                    {
                                        index_count++;
                                        if (detail.Contains("Details of the Query"))
                                        {
                                            OWM.Name = string.IsNullOrWhiteSpace(list_enquiry[index_count]) ? "" : list_enquiry[index_count];
                                            OWM.Email = string.IsNullOrWhiteSpace(list_enquiry[index_count + 1]) ? "" : list_enquiry[index_count + 1];
                                            OWM.phone = string.IsNullOrWhiteSpace(list_enquiry[index_count + 2]) ? "" : list_enquiry[index_count + 2];
                                            OWM.EnquiryDate = EmailReceivedDate;
                                            OWM.Email_body = EnqText;
                                            IsInsert = true;
                                            break;
                                        }
                                        else
                                        {
                                            if (!detail.Contains("https://www.99acres.com"))
                                                sb.AppendLine(detail);
                                        }
                                    }
                                    break;
                                }
                            }
                    }
                    if (IsInsert)
                    {
                        OWM.additional_Info = sb.ToString();
                        OWM.EnqSoure = "99acres";
                        Helper.InsertInquery(OWM);
                        break;
                    }

                }
            }
            catch (Exception exx)
            {
                Console.WriteLine("failed to read Enquiry :" + exx.Message);
            }

        }
    }
}



