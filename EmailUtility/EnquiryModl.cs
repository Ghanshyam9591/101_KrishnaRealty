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
        public string additional_Info { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string EnqSoure { get; set; }


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
}
