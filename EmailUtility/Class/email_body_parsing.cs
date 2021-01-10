using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace EmailUtility.Class
{
    public class email_body_parsing
    {

        public static int get_parse_value_as_number(DataTable dt_config, string tag, string email_body, string EnqSoure)
        {

            DataRow drow = dt_config.Select("pattern = '" + tag + "'").SingleOrDefault();

            string json_value = drow["configuration"].ToString();
            switch (tag)
            {
                case "location":
                    List<parse_location_model> parse_list = new List<parse_location_model>();

                    parse_list = JsonConvert.DeserializeObject<List<parse_location_model>>(json_value);
                    foreach (parse_location_model plm in parse_list)
                    {
                        if (email_body.ToLower().Contains(plm.location.Trim()))
                        {
                            return plm.location_id;
                        }

                    }


                    break;
                case "property":

                    List<parse_property_model> parse_list2 = new List<parse_property_model>();

                    parse_list2 = JsonConvert.DeserializeObject<List<parse_property_model>>(json_value);
                    foreach (parse_property_model plm in parse_list2)
                    {
                        if (email_body.Contains(plm.property.Trim()))
                        {
                            return plm.property_type_id;
                        }

                    }
                    break;
                case "cost_upto":

                    List<parse_costupto_model> parse_list3 = new List<parse_costupto_model>();
                    StringReader reader = new StringReader(email_body);
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if ((line.Contains("₹") && line.ToLower().Contains("l")) || (line.ToLower().Contains("rs") && line.ToLower().Contains("lac")) ||(line.Contains("₹") && line.ToLower().Contains("k")))
                        {
                            return Convert.ToInt32(Regex.Match(line, @"\d+").Value);
                        }
                    }
                    //parse_list3 = JsonConvert.DeserializeObject<List<parse_costupto_model>>(json_value);
                    //foreach (parse_costupto_model plm in parse_list3)
                    //{
                    //    if (email_body.Contains(plm.cost_upto_lebel.Trim()))
                    //    {
                    //        return Convert.ToInt32(plm.cost_upto_lebel);
                    //    }

                    //}

                    break;
                case "enquiry_source":
                    List<parse_enquiry_source_model> parse_list4 = new List<parse_enquiry_source_model>();

                    parse_list4 = JsonConvert.DeserializeObject<List<parse_enquiry_source_model>>(json_value);
                    foreach (parse_enquiry_source_model plm in parse_list4)
                    {
                        if (EnqSoure.Contains(plm.enquiry_source.Trim()))
                        {
                            return plm.enquiry_source_id;
                        }

                    }
                    break;
                case "enquiry_type":

                    List<parse_enquiry_type_model> parse_list5 = new List<parse_enquiry_type_model>();
                    string enquiry_type = Helper.get_enquiry_type(email_body);
                    parse_list5 = JsonConvert.DeserializeObject<List<parse_enquiry_type_model>>(json_value);
                    foreach (parse_enquiry_type_model plm in parse_list5)
                    {
                        if (enquiry_type.Contains(plm.enquiry_type.Trim()))
                        {
                            return plm.enquiry_type_id;
                        }

                    }
                    break;

            }

            return 0;
        }
    }
}
