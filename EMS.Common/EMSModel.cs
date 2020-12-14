using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Common
{
    public class EMSModel
    {
    }
    public class AutoEnquiryModl
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string phone { get; set; }
        public string Email { get; set; }
        public string Email_Body { get; set; }
        public string additional_Info { get; set; }
        public DateTime EnquiryDate { get; set; }
        public DateTime Created_date { get; set; }
        public string EnqSoure { get; set; }


    }
    public class baseclass
    {
        public int id { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public int modify_by { get; set; }
        public DateTime modify_date { get; set; }
        public bool is_active { get; set; }

    }
    public class ExportParaModel
    {
        public DateTime AsOnDate { get; set; }
        public string ClaimType { get; set; }
    }
    public class ExportParaModel2
    {
        public string GridHtml { get; set; }
    }
    public class FileUploadStatusModel
    {
        public string FileName { get; set; }
        public int TotalRowCount { get; set; }
        public DateTime UploadDate { get; set; }

    }
    public class CALL_HISTORY_MODEL
    {
        public int enquiry_no { get; set; }
        public DateTime call_date { get; set; }
        public string call_response { get; set; }
        public string action_type { get; set; }
        public string employee_name { get; set; }
    }
    public class DASHBOARD_DETAIL_MODEL
    {
        public int total_enquiry { get; set; }
        public int convert_to_leads { get; set; }
        public int folowups { get; set; }
        public int total_visitors { get; set; }
    }
    public class DASHBOARD_LOCATION_WISE_MODEL
    {
        public string location { get; set; }
        public int total_enquiry { get; set; }
        public int folowups { get; set; }
        public int convert_to_lead { get; set; }
    }
    public class UserDetailsModel
    {
        public int employee_code { get; set; }
        public string employee_name { get; set; }
        public string email_address { get; set; }

        public int IsActive { get; set; }
        public bool FullAccess { get; set; }
    }

    public class ReleasePaymentModel
    {
        public int ID { get; set; }
        public int EMP_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public decimal CLAIM_AMT { get; set; }
        public string CLAIM_TYPE { get; set; }
        public DateTime LAST_UPDATED_DATE { get; set; }
        public int BATCH_NUMBER { get; set; }
        public bool SELECT_STATUS { get; set; }
        public DateTime ACTIVITY_DATE { get; set; }
        public DateTime BATCH_NUMBER_DATE { get; set; }
        public DateTime CREATED_DATE { get; set; }
    }
    public class RecordException
    {

        public string PAGE_NAME { get; set; }
        public string ERR_MSG { get; set; }
    }



    public class QueryModulePara
    {
        public string cust_name { get; set; }
        public int cust_id { get; set; }
        public int location_id { get; set; }
        public int enquiry_source_id { get; set; }
        public DateTime enquiry_date { get; set; }
        public int action_type_id { get; set; }
        public string query_type { get; set; }
    }

    public class MenusModel
    {
        public int SeqID { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string css_class { get; set; }
        public int is_active { get; set; }
    }
    public class DDLModel
    {
        public int SeqID { get; set; }
        public string Name { get; set; }
    }
    public class DDLModel2
    {
        public int emp_code { get; set; }
        public string EmpName { get; set; }
    }
    public class UserModel
    {
        public int seqid { get; set; }
        public int employee_code { get; set; }
        public string employee_name { get; set; }
        public string email_address { get; set; }
        public int role_id { get; set; }
        public bool is_active { get; set; }
        public DateTime created_date { get; set; }

    }
    public class RoleModel
    {
        public int seqid { get; set; }
        public string role_code { get; set; }
        public string role_name { get; set; }
        public bool is_active { get; set; }
        public DateTime created_date { get; set; }

    }
    public class LovCategoryModel : baseclass
    {
        public string code { get; set; }
        public string name { get; set; }

    }
    public class LovModel : baseclass
    {
        public string code { get; set; }
        public string name { get; set; }
        public int categoryid { get; set; }
        public string category { get; set; }
    }

    public class MenuModel
    {
        public int seqid { get; set; }
        public string link { get; set; }
        public string title { get; set; }
        public bool select_status { get; set; }
        public string css_class { get; set; }
        public int menu_order { get; set; }
        public bool is_active { get; set; }
        public DateTime created_date { get; set; }
    }
    public class MenuRoleMappingModel
    {
        public int seqid { get; set; }
        public int role_id { get; set; }
        public int menu_id { get; set; }
        public bool is_active { get; set; }
        public DateTime created_date { get; set; }

    }
    public class EnquiryDataModel
    {
        public int seqid { get; set; }
        public string name { get; set; } = string.Empty;
        public Int64 mobile1 { get; set; }
        public Int64 mobile2 { get; set; }
        public string email { get; set; } = "";
        public int location_id { get; set; }
        public string location_name { get; set; } = string.Empty;
        public decimal cost_upto { get; set; }
        public int property_type_id { get; set; }
        public string propery_name { get; set; } = string.Empty;
        public int enquiry_source_id { get; set; }
        public int enquiry_type_id { get; set; }
        public string enquiry_source_name { get; set; } = string.Empty;
        public int assign_to_id { get; set; }
        public string remark { get; set; } = string.Empty;
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public DateTime enquiry_date { get; set; }
    }

    public class FlatOwnerModel
    {
        public int seqid { get; set; }
        public string name { get; set; } = string.Empty;
        public string mobile1 { get; set; }
        public string mobile2 { get; set; }
        public string email { get; set; } = string.Empty;

        public int renter_seqid { get; set; }
        public string renter_name { get; set; }
        public string renter_mobile1 { get; set; }
        public string renter_mobile2 { get; set; }
        public string renter_email { get; set; }
        public int location_id { get; set; }
        public string location_name { get; set; } = string.Empty;
        public decimal deposit { get; set; }
        public decimal monthly_rent { get; set; }
        public int property_type_id { get; set; }
        public string propery_name { get; set; } = string.Empty;
        public string remark { get; set; } = string.Empty;
        public int building_id { get; set; }
        public string flat_no { get; set; }
        public DateTime duration_from { get; set; }
        public DateTime duration_to { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public DateTime enquiry_date { get; set; }
        public bool is_active { get; set; }
    }

    public class RentalQueryModuleModel
    {
        public int seqid { get; set; }
        public string name { get; set; } = string.Empty;
        public string mobile1 { get; set; }
        public string mobile2 { get; set; }
        public string email { get; set; } = string.Empty;

        public int renter_seqid { get; set; }
        public string renter_name { get; set; }
        public string renter_mobile1 { get; set; }
        public string renter_mobile2 { get; set; }
        public string renter_email { get; set; }
        public string location_name { get; set; } = string.Empty;
        public string building_name { get; set; } = string.Empty;
        public decimal deposit { get; set; }
        public decimal monthly_rent { get; set; }
        public int property_type_id { get; set; }
        public string propery_name { get; set; } = string.Empty;
        public string remark { get; set; } = string.Empty;
        public int building_id { get; set; }
        public string flat_no { get; set; }
        public DateTime duration_from { get; set; }
        public DateTime duration_to { get; set; }
        public int agreement_expire_inmonth { get; set; }
        public int agreement_expire_indays { get; set; }
        public int created_by { get; set; }
        public DateTime created_date { get; set; }
        public DateTime enquiry_date { get; set; }
        public bool is_active { get; set; }
        public string css_class { get; set; }
    }

    public class EnquiryStatusModel
    {
        public int seqid { get; set; }
        public int enquiryno { get; set; }
        public int action_taken_by { get; set; }
        public int action_type_id { get; set; }
        public DateTime action_date { get; set; }
        public string comments { get; set; } = string.Empty;
    }
    public class QueryModelRPT
    {
        public int seqid { get; set; }
        public string Customer { get; set; } = string.Empty;
        public DateTime enquiry_date { get; set; }
        public string propertytype { get; set; } = string.Empty;
        public Int64 mobile1 { get; set; }
        public Int64 mobile2 { get; set; }
        public string email { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal cost_upto { get; set; }
        public string remark { get; set; } = string.Empty;
        public string last_remark { get; set; } = string.Empty;
        public int action_type_id { get; set; }
    }
}

