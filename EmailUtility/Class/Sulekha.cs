using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EmailUtility.Class
{
    public class Sulekha
    {
        public static void GetSulekhEnquiry(StringReader reader, DateTime EnquiryDate)
        {
            EnquiryModl EnqModel = new EnquiryModl();
            StringBuilder sb = new StringBuilder();
            EnqModel.EnquiryDate = EnquiryDate;
            int CheckCounter = 0;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(':');
                if (values.Length > 1)
                {
                    if (line.Contains("Name :"))
                    {
                        EnqModel.Name = values[1];
                        CheckCounter++;
                    }
                    if (line.Contains("Phone :"))
                    {
                        EnqModel.phone = values[1];
                        CheckCounter++;
                    }
                    if (line.Contains("Email :"))
                    {
                        EnqModel.Email = values[1];
                        CheckCounter++;
                    }
                    if (line.Contains("Project :"))
                    {
                        sb.AppendLine(values[1] + "|");
                        CheckCounter++;
                    }
                    if (line.Contains("Title :"))
                    {
                        sb.AppendLine(values[1] + "|");
                        CheckCounter++;
                    }
                }

            }
            if (CheckCounter > 3)
            {
                EnqModel.additional_Info = sb.ToString();
                EnqModel.EnqSoure = "Sulekha";
                Helper.InsertInquery(EnqModel);
            }
        }
    }
}
