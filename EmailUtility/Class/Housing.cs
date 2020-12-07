using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EmailUtility.Class
{
    public class Housing
    {
        public static void GetHousingEnquiry(StringReader reader, DateTime EnquiryDate,DateTime EmailReceivedDate)
        {
            EnquiryModl EnqModel = new EnquiryModl();
            StringBuilder sb = new StringBuilder();
            List<string> list_equiry = new List<string>();
            EnqModel.EnquiryDate = EnquiryDate;
            int CheckCounter = 0;
            
            string line;
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
                    if (!string.IsNullOrWhiteSpace(line.Trim()))
                        list_equiry.Add(line.Trim());
                }
                if (list_equiry.Count == 6)
                {
                    EnqModel.Name = string.IsNullOrWhiteSpace(list_equiry[1]) ? "" : list_equiry[1];
                    EnqModel.Email= string.IsNullOrWhiteSpace(list_equiry[3]) ? "" : list_equiry[3];
                    EnqModel.phone= string.IsNullOrWhiteSpace(list_equiry[5]) ? "" : list_equiry[5];
                    EnqModel.EnquiryDate = EmailReceivedDate;

                    EnqModel.additional_Info = sb.ToString();
                    EnqModel.EnqSoure = "Housing";

                    Helper.InsertInquery(EnqModel);
                    break;
                }

            }
        }
    }
}
