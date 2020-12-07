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
        public static void GetEnquiry99Acrs(string EnqText,DateTime EmailReceivedDate)
        {
            EnquiryModl OWM = new EnquiryModl();
            try
            {
                StringBuilder sb = new StringBuilder();
                string resultString = Regex.Replace(EnqText, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
                string line;
                bool IsOk = false;
                bool IsOk2 = false;
                bool IsInsert = false;
                using (StringReader reader = new StringReader(resultString))
                {
                    int line_count = 0;
                    List<string> list_enquiry = new List<string>();
                    while ((line = reader.ReadLine()) != null)
                        if (!string.IsNullOrEmpty(line.Trim()))
                        {
                            line = line.Replace(">", "");
                            if (line.Trim().Contains("Property Detail"))
                            {
                                IsOk2 = true;
                                continue;
                            }
                            if (IsOk2 && !string.IsNullOrWhiteSpace(line.Trim()) && line_count <= 10)
                            {
                                list_enquiry.Add(line.Trim());
                                line_count++;
                            }
                            int index_count = 0;
                            if (line_count == 10)
                            {
                                foreach (string detail in list_enquiry)
                                {
                                    index_count++;
                                    if (detail.Contains("Owner details"))
                                    {
                                        OWM.Name = string.IsNullOrWhiteSpace(list_enquiry[index_count]) ? "" : list_enquiry[index_count];
                                        OWM.Email = string.IsNullOrWhiteSpace(list_enquiry[index_count + 1]) ? "" : list_enquiry[index_count + 1];
                                        OWM.phone = string.IsNullOrWhiteSpace(list_enquiry[index_count + 2]) ? "" : list_enquiry[index_count + 2];
                                        OWM.EnquiryDate = EmailReceivedDate;
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
                }

            }
            catch (Exception exx)
            {
                Console.WriteLine("failed to read Enquiry :" + exx.Message);
            }
        }
    }
}
