using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EmailUtility
{
    public class EnquiryModl
    {
        public string Name { get; set; }
        public string phone { get; set; }
        public string Email { get; set; }
        public string Email_body { get; set; }
        public int location_id { get; set; }
        public string location { get; set; }
        public string actual_cost_upto { get; set; }
        public int enquiry_type_id { get; set; }
        public decimal cost_upto { get; set; }
        public int property_type_id { get; set; }
        public string property_type { get; set; }
        public int sqft_area { get; set; }
        public string additional_Info { get; set; }
        public DateTime EnquiryDate { get; set; }
        public int enquiry_source_id { get; set; }
        public string EnqSoure { get; set; }


    }
    public class EmailUtilityModel
    {
        public string email_id { get; set; }
        public string messageuid { get; set; }
        public DateTime emailreceived_date { get; set; }
        public string emailinfo { get; set; }

    }
    public class OwnerModel
    {
        public int ID { get; set; }
        public string FlatType { get; set; }

        public string Title { get; set; }
        public int SiteID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Mobile3 { get; set; }
        public string Location { get; set; }
        public string Source { get; set; }
    }
    public class parse_location_model
    {
        public int location_id { get; set; }
        public string location { get; set; }
    }
    public class parse_property_model
    {
        public int property_type_id { get; set; }
        public string property { get; set; }
    }
    public class parse_costupto_model
    {
        public decimal cost_upto { get; set; }
        public string cost_upto_lebel { get; set; }
    }
    public class parse_enquiry_source_model
    {
        public int enquiry_source_id { get; set; }
        public string enquiry_source { get; set; }
    }
    public class parse_enquiry_type_model
    {
        public int enquiry_type_id { get; set; }
        public string enquiry_type { get; set; }
    }
}
