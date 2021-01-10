using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;

namespace EmailUtility.Class
{
    public class Sulekha
    {
        public static void GetSulekhEnquiry(StringReader reader, DateTime EnquiryDate, DateTime EmailReceivedDate,DataTable dt_config)
        {
            try
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
                    EnqModel.EnquiryDate = EmailReceivedDate;
                    EnqModel.EnqSoure = "Sulekha";
                    EnqModel.property_type_id = email_body_parsing.get_parse_value_as_number(dt_config, "property", sb.ToString(), EnqModel.EnqSoure);
                    EnqModel.enquiry_source_id = email_body_parsing.get_parse_value_as_number(dt_config, "enquiry_source", sb.ToString(), EnqModel.EnqSoure);
                    EnqModel.location_id = email_body_parsing.get_parse_value_as_number(dt_config, "location", sb.ToString(), EnqModel.EnqSoure);
                    EnqModel.cost_upto = email_body_parsing.get_parse_value_as_number(dt_config, "cost_upto", sb.ToString(), EnqModel.EnqSoure);
                    EnqModel.enquiry_type_id = email_body_parsing.get_parse_value_as_number(dt_config, "enquiry_type", sb.ToString(), EnqModel.EnqSoure);
                    Helper.InsertInquery(EnqModel);
                }
            }
            catch (Exception exx)
            {
                Helper.WriteLog("error in function [GetSulekhEnquiry] :" + exx.Message);
            }
        }
    }
}
