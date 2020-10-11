using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Common
{
    public class QueryHandler
    {


        static List<RecordException> RECORDED_ERR_LIST = new List<RecordException>();

        static List<ReleasePaymentModel> RPM_LIST = new List<ReleasePaymentModel>();



        public static List<ReleasePaymentModel> GetReleaseRecordList(DataTable DT, DateTime AsOnDate)
        {
            List<ReleasePaymentModel> REM_LIST2 = new List<ReleasePaymentModel>();
            List<RecordException> RECORDED_ERR_LIST = new List<RecordException>();


            int RecordCount = 0;
            foreach (DataRow drow in DT.Rows)
            {
                RecordCount++;
                ReleasePaymentModel REM = new ReleasePaymentModel();
                try
                {
                    REM.ID = string.IsNullOrWhiteSpace(drow["seqid"].ToString()) ? 0 : Convert.ToInt32(drow["seqid"]);
                    REM.EMP_CODE = string.IsNullOrWhiteSpace(drow["emp_code"].ToString()) ? 0 : Convert.ToInt32(drow["emp_code"]);
                    REM.EMP_NAME = string.IsNullOrWhiteSpace(drow["emp_name"].ToString()) ? "" : drow["emp_name"].ToString();
                    REM.CLAIM_AMT = string.IsNullOrWhiteSpace(drow["claim_amt"].ToString()) ? 0 : Convert.ToDecimal(drow["claim_amt"]);
                    REM.BATCH_NUMBER = string.IsNullOrWhiteSpace(drow["batch_number"].ToString()) ? 0 : Convert.ToInt32(drow["batch_number"]);
                    REM.BATCH_NUMBER_DATE = string.IsNullOrWhiteSpace(drow["batch_number_date"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["batch_number_date"]);
                    REM.ACTIVITY_DATE = AsOnDate;
                    REM_LIST2.Add(REM);
                }
                catch (Exception ex)
                {
                    if (RecordCount < 1)
                    {
                        RecordException RException = new RecordException();

                        RException.ERR_MSG = ex.Message;
                        RECORDED_ERR_LIST.Add(RException);
                    }
                }

            }
            return REM_LIST2;
        }

        public static List<CALL_HISTORY_MODEL> GetFileUploadStatusList(DataTable DT)
        {
            List<CALL_HISTORY_MODEL> REM_LIST2 = new List<CALL_HISTORY_MODEL>();


            int RecordCount = 0;
            foreach (DataRow drow in DT.Rows)
            {
                RecordCount++;
                CALL_HISTORY_MODEL REM = new CALL_HISTORY_MODEL();
                try
                {
                    REM.enquiry_no = string.IsNullOrWhiteSpace(drow["enquiry_no"].ToString()) ? 0 : Convert.ToInt32(drow["enquiry_no"].ToString());
                    REM.call_response = string.IsNullOrWhiteSpace(drow["call_response"].ToString()) ? "" : drow["call_response"].ToString();
                    REM.call_date = string.IsNullOrWhiteSpace(drow["call_date"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["call_date"]);
                    REM.employee_name = string.IsNullOrWhiteSpace(drow["employee_name"].ToString()) ? "" : drow["employee_name"].ToString();
                    REM.action_type = string.IsNullOrWhiteSpace(drow["action_type"].ToString()) ? "" : drow["action_type"].ToString();
                    REM_LIST2.Add(REM);
                }
                catch (Exception ex)
                {
                    if (RecordCount < 1)
                    {
                        RecordException RException = new RecordException();
                        RException.ERR_MSG = ex.Message;
                        RECORDED_ERR_LIST.Add(RException);
                    }
                }

            }
            return REM_LIST2;
        }

        public static List<RoleModel> GetRoleGridList(DataTable DT)
        {
            List<RoleModel> REM_LIST2 = new List<RoleModel>();


            int RecordCount = 0;
            foreach (DataRow drow in DT.Rows)
            {
                RecordCount++;
                RoleModel REM = new RoleModel();
                try
                {
                    REM.seqid = string.IsNullOrWhiteSpace(drow["seqid"].ToString()) ? 0 : Convert.ToInt32(drow["seqid"].ToString());
                    REM.role_code = string.IsNullOrWhiteSpace(drow["role_code"].ToString()) ? "" : drow["role_code"].ToString();
                    REM.role_name = string.IsNullOrWhiteSpace(drow["role_name"].ToString()) ? "" : drow["role_name"].ToString();
                    if (string.IsNullOrWhiteSpace(drow["is_active"].ToString()))
                        REM.is_active = false;
                    else
                        REM.is_active = true;

                    REM_LIST2.Add(REM);
                }
                catch (Exception ex)
                {
                    if (RecordCount < 1)
                    {
                        RecordException RException = new RecordException();
                        RException.ERR_MSG = ex.Message;
                        RECORDED_ERR_LIST.Add(RException);
                    }
                }

            }
            return REM_LIST2;
        }
        public static List<LovCategoryModel> GetLovCategoryGridList(DataTable DT)
        {
            List<LovCategoryModel> REM_LIST2 = new List<LovCategoryModel>();


            int RecordCount = 0;
            foreach (DataRow drow in DT.Rows)
            {
                RecordCount++;
                LovCategoryModel REM = new LovCategoryModel();
                try
                {
                    REM.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                    REM.code = string.IsNullOrWhiteSpace(drow["code"].ToString()) ? "" : drow["code"].ToString();
                    REM.name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                    REM.is_active = string.IsNullOrWhiteSpace(drow["is_active"].ToString()) ? false : Convert.ToBoolean(drow["is_active"]);
                    REM_LIST2.Add(REM);
                }
                catch (Exception ex)
                {
                    if (RecordCount < 1)
                    {
                        RecordException RException = new RecordException();
                        RException.ERR_MSG = ex.Message;
                        RECORDED_ERR_LIST.Add(RException);
                    }
                }

            }
            return REM_LIST2;
        }

        public static List<LovModel> GetLovGridList(DataTable DT)
        {
            List<LovModel> REM_LIST2 = new List<LovModel>();

            DataColumnCollection columns = DT.Columns;



            int RecordCount = 0;
            foreach (DataRow drow in DT.Rows)
            {
                RecordCount++;
                LovModel REM = new LovModel();
                try
                {
                    REM.id = string.IsNullOrWhiteSpace(drow["id"].ToString()) ? 0 : Convert.ToInt32(drow["id"].ToString());
                    REM.code = string.IsNullOrWhiteSpace(drow["code"].ToString()) ? "" : drow["code"].ToString();
                    REM.name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                    REM.categoryid = string.IsNullOrWhiteSpace(drow["category_id"].ToString()) ? 0 : Convert.ToInt32(drow["category_id"]);

                    if (columns.Contains("category"))
                    {
                        REM.category = string.IsNullOrWhiteSpace(drow["category"].ToString()) ? "" : drow["category"].ToString();
                    }
                    REM.is_active = string.IsNullOrWhiteSpace(drow["is_active"].ToString()) ? false : Convert.ToBoolean(drow["is_active"]);
                    REM_LIST2.Add(REM);
                }
                catch (Exception ex)
                {
                    if (RecordCount < 1)
                    {
                        RecordException RException = new RecordException();
                        RException.ERR_MSG = ex.Message;
                        RECORDED_ERR_LIST.Add(RException);
                    }
                }

            }
            return REM_LIST2;
        }

        public static List<MenuModel> GetMenuRoleGridList(DataTable DT)
        {
            List<MenuModel> REM_LIST2 = new List<MenuModel>();


            int RecordCount = 0;
            foreach (DataRow drow in DT.Rows)
            {
                RecordCount++;
                MenuModel REM = new MenuModel();
                try
                {
                    REM.seqid = string.IsNullOrWhiteSpace(drow["seqid"].ToString()) ? 0 : Convert.ToInt32(drow["seqid"].ToString());
                    REM.title = string.IsNullOrWhiteSpace(drow["title"].ToString()) ? "" : drow["title"].ToString();
                    if (string.IsNullOrWhiteSpace(drow["is_active"].ToString()))
                        REM.is_active = false;
                    else
                        REM.is_active = true;
                    REM_LIST2.Add(REM);
                }
                catch (Exception ex)
                {
                    if (RecordCount < 1)
                    {
                        RecordException RException = new RecordException();
                        RException.ERR_MSG = ex.Message;
                        RECORDED_ERR_LIST.Add(RException);
                    }
                }

            }
            return REM_LIST2;
        }

        public static List<MenuRoleMappingModel> GetAssignedMenuToRole(DataTable DT)
        {
            List<MenuRoleMappingModel> REM_LIST2 = new List<MenuRoleMappingModel>();


            int RecordCount = 0;
            foreach (DataRow drow in DT.Rows)
            {
                RecordCount++;
                MenuRoleMappingModel REM = new MenuRoleMappingModel();
                try
                {
                    REM.seqid = string.IsNullOrWhiteSpace(drow["seqid"].ToString()) ? 0 : Convert.ToInt32(drow["seqid"].ToString());
                    REM.menu_id = string.IsNullOrWhiteSpace(drow["menu_id"].ToString()) ? 0 : Convert.ToInt32(drow["menu_id"]);
                    REM.role_id = string.IsNullOrWhiteSpace(drow["role_id"].ToString()) ? 0 : Convert.ToInt32(drow["role_id"]);
                    if (string.IsNullOrWhiteSpace(drow["is_active"].ToString()))
                        REM.is_active = false;
                    else
                        REM.is_active = true;
                    REM_LIST2.Add(REM);
                }
                catch (Exception ex)
                {
                    if (RecordCount < 1)
                    {
                        RecordException RException = new RecordException();
                        RException.ERR_MSG = ex.Message;
                        RECORDED_ERR_LIST.Add(RException);
                    }
                }

            }
            return REM_LIST2;
        }


        public static List<UserModel> GetUserGridList(DataTable DT)
        {
            List<UserModel> REM_LIST2 = new List<UserModel>();


            int RecordCount = 0;
            foreach (DataRow drow in DT.Rows)
            {
                RecordCount++;
                UserModel REM = new UserModel();
                try
                {
                    REM.seqid = string.IsNullOrWhiteSpace(drow["seqid"].ToString()) ? 0 : Convert.ToInt32(drow["seqid"].ToString());
                    REM.employee_code = string.IsNullOrWhiteSpace(drow["employee_code"].ToString()) ? 0 : Convert.ToInt32(drow["employee_code"].ToString());
                    REM.employee_name = string.IsNullOrWhiteSpace(drow["employee_name"].ToString()) ? "" : drow["employee_name"].ToString();
                    REM.email_address = string.IsNullOrWhiteSpace(drow["email_address"].ToString()) ? "" : drow["email_address"].ToString();
                    REM.role_id = string.IsNullOrWhiteSpace(drow["role_id"].ToString()) ? 0 : Convert.ToInt32(drow["role_id"].ToString());
                    if (string.IsNullOrWhiteSpace(drow["is_active"].ToString()))
                        REM.is_active = false;
                    else
                        REM.is_active = true;

                    REM_LIST2.Add(REM);
                }
                catch (Exception ex)
                {
                    if (RecordCount < 1)
                    {
                        RecordException RException = new RecordException();
                        RException.ERR_MSG = ex.Message;
                        RECORDED_ERR_LIST.Add(RException);
                    }
                }

            }
            return REM_LIST2;
        }

        public static List<EnquiryDataModel> GetEnquiryDataGridList(DataTable DT)
        {
            List<EnquiryDataModel> REM_LIST2 = new List<EnquiryDataModel>();


            int RecordCount = 0;
            foreach (DataRow drow in DT.Rows)
            {
                RecordCount++;
                EnquiryDataModel REM = new EnquiryDataModel();
                try
                {

                    REM.seqid = string.IsNullOrWhiteSpace(drow["seqid"].ToString()) ? 0 : Convert.ToInt32(drow["seqid"]);
                    REM.name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                    REM.mobile1 = string.IsNullOrWhiteSpace(drow["mobile1"].ToString()) ? 0 : Convert.ToInt64(drow["mobile1"]);
                    REM.mobile2 = string.IsNullOrWhiteSpace(drow["mobile2"].ToString()) ? 0 : Convert.ToInt64(drow["mobile2"]);
                    REM.email = string.IsNullOrWhiteSpace(drow["email"].ToString()) ? "" : drow["email"].ToString();
                    REM.location_id = string.IsNullOrWhiteSpace(drow["location_id"].ToString()) ? 0 : Convert.ToInt32(drow["location_id"]);
                    REM.cost_upto = string.IsNullOrWhiteSpace(drow["cost_upto"].ToString()) ? 0 : Convert.ToInt32(drow["cost_upto"]);
                    REM.property_type_id = string.IsNullOrWhiteSpace(drow["property_type_id"].ToString()) ? 0 : Convert.ToInt32(drow["property_type_id"]);
                    REM.enquiry_source_id = string.IsNullOrWhiteSpace(drow["enquiry_source_id"].ToString()) ? 0 : Convert.ToInt32(drow["enquiry_source_id"]);
                    REM.remark = string.IsNullOrWhiteSpace(drow["remark"].ToString()) ? "" : drow["remark"].ToString();
                    REM.created_by = string.IsNullOrWhiteSpace(drow["created_by"].ToString()) ? 0 : Convert.ToInt32(drow["created_by"]);
                    REM.created_date = string.IsNullOrWhiteSpace(drow["created_date"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["created_date"]);
                    REM.enquiry_type_id = string.IsNullOrWhiteSpace(drow["enquiry_type_id"].ToString()) ? 0 : Convert.ToInt32(drow["enquiry_type_id"]);
                    REM.enquiry_date = string.IsNullOrWhiteSpace(drow["enquiry_date"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["enquiry_date"]);
                    REM.assign_to_id = string.IsNullOrWhiteSpace(drow["assign_to_id"].ToString()) ? 0 : Convert.ToInt32(drow["assign_to_id"]);
                    REM_LIST2.Add(REM);

                }
                catch (Exception ex)
                {
                    if (RecordCount < 1)
                    {
                        RecordException RException = new RecordException();
                        RException.ERR_MSG = ex.Message;
                        RECORDED_ERR_LIST.Add(RException);
                    }
                }

            }
            return REM_LIST2;
        }


        public static List<FlatOwnerModel> GetFlatOwnersGridList(DataTable DT)
        {
            List<FlatOwnerModel> REM_LIST2 = new List<FlatOwnerModel>();


            int RecordCount = 0;
            foreach (DataRow drow in DT.Rows)
            {
                RecordCount++;
                FlatOwnerModel REM = new FlatOwnerModel();
                try
                {

                    REM.seqid = string.IsNullOrWhiteSpace(drow["flat_owner_seqid"].ToString()) ? 0 : Convert.ToInt32(drow["flat_owner_seqid"]);
                    REM.name = string.IsNullOrWhiteSpace(drow["flat_owner_name"].ToString()) ? "" : drow["flat_owner_name"].ToString();
                    REM.mobile1 = string.IsNullOrWhiteSpace(drow["flat_owner_mobile1"].ToString()) ? "" : drow["flat_owner_mobile1"].ToString();
                    REM.mobile2 = string.IsNullOrWhiteSpace(drow["flat_owner_mobile2"].ToString()) ? "" : drow["flat_owner_mobile2"].ToString();
                    REM.email = string.IsNullOrWhiteSpace(drow["flat_owner_email"].ToString()) ? "" : drow["flat_owner_email"].ToString();
                    REM.renter_seqid = string.IsNullOrWhiteSpace(drow["renter_seqid"].ToString()) ? 0 : Convert.ToInt32(drow["renter_seqid"]);
                    REM.renter_name = string.IsNullOrWhiteSpace(drow["renter_name"].ToString()) ? "" : drow["renter_name"].ToString();
                    REM.renter_mobile1 = string.IsNullOrWhiteSpace(drow["renter_mobile1"].ToString()) ? "" : drow["renter_mobile1"].ToString();
                    REM.renter_mobile2 = string.IsNullOrWhiteSpace(drow["renter_mobile2"].ToString()) ? "" : drow["renter_mobile2"].ToString();
                    REM.renter_email = string.IsNullOrWhiteSpace(drow["renter_email"].ToString()) ? "" : drow["renter_email"].ToString();

                    REM.location_id = string.IsNullOrWhiteSpace(drow["location_id"].ToString()) ? 0 : Convert.ToInt32(drow["location_id"]);
                    REM.deposit = string.IsNullOrWhiteSpace(drow["deposit"].ToString()) ? 0 : Convert.ToDecimal(drow["deposit"]);
                    REM.monthly_rent = string.IsNullOrWhiteSpace(drow["monthly_rent"].ToString()) ? 0 : Convert.ToDecimal(drow["monthly_rent"]);
                    REM.property_type_id = string.IsNullOrWhiteSpace(drow["property_type_id"].ToString()) ? 0 : Convert.ToInt32(drow["property_type_id"]);
                    REM.location_id = string.IsNullOrWhiteSpace(drow["location_id"].ToString()) ? 0 : Convert.ToInt32(drow["location_id"]);
                    REM.building_id = string.IsNullOrWhiteSpace(drow["building_id"].ToString()) ? 0 : Convert.ToInt32(drow["building_id"]);
                    REM.flat_no = string.IsNullOrWhiteSpace(drow["flat_no"].ToString()) ? "" : drow["flat_no"].ToString();
                    REM.duration_from = string.IsNullOrWhiteSpace(drow["duration_from"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["duration_from"]);
                    REM.duration_to = string.IsNullOrWhiteSpace(drow["duration_to"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["duration_to"]);
                    REM.remark = string.IsNullOrWhiteSpace(drow["renter_remark"].ToString()) ? "" : drow["renter_remark"].ToString();
                    //REM.created_by = string.IsNullOrWhiteSpace(drow["created_by"].ToString()) ? 0 : Convert.ToInt32(drow["created_by"]);
                    //REM.created_date = string.IsNullOrWhiteSpace(drow["created_date"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["created_date"]);
                    // REM.enquiry_date = string.IsNullOrWhiteSpace(drow["enquiry_date"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["enquiry_date"]);
                    REM_LIST2.Add(REM);

                }
                catch (Exception ex)
                {
                    if (RecordCount < 1)
                    {
                        RecordException RException = new RecordException();
                        RException.ERR_MSG = ex.Message;
                        RECORDED_ERR_LIST.Add(RException);
                    }
                }

            }
            return REM_LIST2;
        }
        public static List<FlatOwnerModel> GetFlatOwnersGridData(DataTable DT)
        {
            List<FlatOwnerModel> REM_LIST2 = new List<FlatOwnerModel>();


            int RecordCount = 0;
            foreach (DataRow drow in DT.Rows)
            {
                RecordCount++;
                FlatOwnerModel REM = new FlatOwnerModel();
                try
                {

                    REM.seqid = string.IsNullOrWhiteSpace(drow["seqid"].ToString()) ? 0 : Convert.ToInt32(drow["seqid"]);
                    REM.name = string.IsNullOrWhiteSpace(drow["name"].ToString()) ? "" : drow["name"].ToString();
                    REM.mobile1 = string.IsNullOrWhiteSpace(drow["mobile1"].ToString()) ? "" : drow["mobile1"].ToString();
                    REM.mobile2 = string.IsNullOrWhiteSpace(drow["mobile2"].ToString()) ? "" : drow["mobile2"].ToString();
                    REM.email = string.IsNullOrWhiteSpace(drow["email"].ToString()) ? "" : drow["email"].ToString();
                    REM.created_by = string.IsNullOrWhiteSpace(drow["created_by"].ToString()) ? 0 : Convert.ToInt32(drow["created_by"]);
                    REM.created_date = string.IsNullOrWhiteSpace(drow["created_date"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["created_date"]);
                    REM.is_active = string.IsNullOrWhiteSpace(drow["is_active"].ToString()) ? false : Convert.ToBoolean(drow["is_active"]);
                    // REM.enquiry_date = string.IsNullOrWhiteSpace(drow["enquiry_date"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["enquiry_date"]);
                    REM_LIST2.Add(REM);

                }
                catch (Exception ex)
                {
                    if (RecordCount < 1)
                    {
                        RecordException RException = new RecordException();
                        RException.ERR_MSG = ex.Message;
                        RECORDED_ERR_LIST.Add(RException);
                    }
                }

            }
            return REM_LIST2;
        }
        public static List<QueryModelRPT> GetQueryModuleGRDData(DataTable DT)
        {
            List<QueryModelRPT> REM_LIST2 = new List<QueryModelRPT>();


            int RecordCount = 0;
            foreach (DataRow drow in DT.Rows)
            {
                RecordCount++;
                QueryModelRPT REM = new QueryModelRPT();
                try
                {

                    REM.seqid = string.IsNullOrWhiteSpace(drow["seqid"].ToString()) ? 0 : Convert.ToInt32(drow["seqid"]);
                    REM.Customer = string.IsNullOrWhiteSpace(drow["Customer"].ToString()) ? "" : drow["Customer"].ToString();
                    REM.mobile1 = string.IsNullOrWhiteSpace(drow["mobile1"].ToString()) ? 0 : Convert.ToInt64(drow["mobile1"]);
                    REM.mobile2 = string.IsNullOrWhiteSpace(drow["mobile2"].ToString()) ? 0 : Convert.ToInt64(drow["mobile2"]);
                    REM.email = string.IsNullOrWhiteSpace(drow["email"].ToString()) ? "" : drow["email"].ToString();
                    REM.Location = string.IsNullOrWhiteSpace(drow["location"].ToString()) ? "" : drow["location"].ToString();
                    REM.cost_upto = string.IsNullOrWhiteSpace(drow["cost_upto"].ToString()) ? 0 : Convert.ToInt32(drow["cost_upto"]);
                    //REM.remark = string.IsNullOrWhiteSpace(drow["remark"].ToString()) ? "" : drow["remark"].ToString();
                    REM.propertytype = string.IsNullOrWhiteSpace(drow["propertytype"].ToString()) ? "" : drow["propertytype"].ToString();
                    REM.enquiry_date = string.IsNullOrWhiteSpace(drow["enquiry_date"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["enquiry_date"]);
                    REM.action_type_id = string.IsNullOrWhiteSpace(drow["action_type_id"].ToString()) ? 0 : Convert.ToInt32(drow["action_type_id"]);
                    REM.last_remark = string.IsNullOrWhiteSpace(drow["lastremark"].ToString()) ? "" : drow["lastremark"].ToString();
                    REM_LIST2.Add(REM);

                }
                catch (Exception ex)
                {
                    if (RecordCount < 1)
                    {
                        RecordException RException = new RecordException();
                        RException.ERR_MSG = ex.Message;
                        RECORDED_ERR_LIST.Add(RException);
                    }
                }

            }
            return REM_LIST2;
        }

        public static List<RentalQueryModuleModel> GetRentalQueryModuleGRDData(DataTable DT)
        {
            List<RentalQueryModuleModel> REM_LIST2 = new List<RentalQueryModuleModel>();


            int RecordCount = 0;
            foreach (DataRow drow in DT.Rows)
            {
                RecordCount++;
                RentalQueryModuleModel REM = new RentalQueryModuleModel();
                try
                {

                    REM.seqid = string.IsNullOrWhiteSpace(drow["owner_seqid"].ToString()) ? 0 : Convert.ToInt32(drow["owner_seqid"]);
                    REM.name = string.IsNullOrWhiteSpace(drow["owner_name"].ToString()) ? "" : drow["owner_name"].ToString();
                    REM.mobile1 = string.IsNullOrWhiteSpace(drow["owner_mobile1"].ToString()) ? "" : drow["owner_mobile1"].ToString();
                    REM.mobile2 = string.IsNullOrWhiteSpace(drow["owner_mobile2"].ToString()) ? "" : drow["owner_mobile2"].ToString();
                    REM.email = string.IsNullOrWhiteSpace(drow["owner_email"].ToString()) ? "" : drow["owner_email"].ToString();
                    REM.renter_seqid = string.IsNullOrWhiteSpace(drow["renter_seqid"].ToString()) ? 0 : Convert.ToInt32(drow["renter_seqid"]);
                    REM.renter_name = string.IsNullOrWhiteSpace(drow["renter_name"].ToString()) ? "" : drow["renter_name"].ToString();
                    REM.renter_mobile1 = string.IsNullOrWhiteSpace(drow["renter_mobile1"].ToString()) ? "" : drow["renter_mobile1"].ToString();
                    REM.renter_mobile2 = string.IsNullOrWhiteSpace(drow["renter_mobile2"].ToString()) ? "" : drow["renter_mobile2"].ToString();
                    REM.renter_email = string.IsNullOrWhiteSpace(drow["renter_email"].ToString()) ? "" : drow["renter_email"].ToString();

                    REM.location_name = string.IsNullOrWhiteSpace(drow["location_name"].ToString()) ? "" : drow["location_name"].ToString();
                    REM.deposit = string.IsNullOrWhiteSpace(drow["deposit"].ToString()) ? 0 : Convert.ToDecimal(drow["deposit"]);
                    REM.monthly_rent = string.IsNullOrWhiteSpace(drow["monthly_rent"].ToString()) ? 0 : Convert.ToDecimal(drow["monthly_rent"]);
                    REM.propery_name = string.IsNullOrWhiteSpace(drow["property_type"].ToString()) ? "" : drow["property_type"].ToString();
                    REM.building_name = string.IsNullOrWhiteSpace(drow["building"].ToString()) ? "" : drow["building"].ToString();
                    REM.flat_no = string.IsNullOrWhiteSpace(drow["flat_no"].ToString()) ? "" : drow["flat_no"].ToString();
                    REM.duration_from = string.IsNullOrWhiteSpace(drow["duration_from"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["duration_from"]);
                    REM.duration_to = string.IsNullOrWhiteSpace(drow["duration_to"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["duration_to"]);
                    REM.agreement_expire_inmonth = string.IsNullOrWhiteSpace(drow["agreement_expire_inmonth"].ToString()) ? 0 : Convert.ToInt32(drow["agreement_expire_inmonth"]);
                    REM.agreement_expire_indays = string.IsNullOrWhiteSpace(drow["agreement_expire_indays"].ToString()) ? 0 : Convert.ToInt32(drow["agreement_expire_indays"]);
                    REM.remark = string.IsNullOrWhiteSpace(drow["renter_remark"].ToString()) ? "" : drow["renter_remark"].ToString();
                    REM.css_class = string.IsNullOrWhiteSpace(drow["css_class"].ToString()) ? "" : drow["css_class"].ToString();
                    //REM.created_by = string.IsNullOrWhiteSpace(drow["created_by"].ToString()) ? 0 : Convert.ToInt32(drow["created_by"]);
                    //REM.created_date = string.IsNullOrWhiteSpace(drow["created_date"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["created_date"]);
                    // REM.enquiry_date = string.IsNullOrWhiteSpace(drow["enquiry_date"].ToString()) ? Convert.ToDateTime(DateTime.Now.ToString("1900-01-01")) : Convert.ToDateTime(drow["enquiry_date"]);
                    REM_LIST2.Add(REM);

                }
                catch (Exception ex)
                {
                    if (RecordCount < 1)
                    {
                        RecordException RException = new RecordException();
                        RException.ERR_MSG = ex.Message;
                        RECORDED_ERR_LIST.Add(RException);
                    }
                }


            }
            return REM_LIST2;
        }
        public static UserDetailsModel GetUserDetails(DataTable DT)
        {
            UserDetailsModel bll = new UserDetailsModel();


            int RecordCount = 0;
            foreach (DataRow drow in DT.Rows)
            {
                RecordCount++;

                try
                {
                    bll.employee_code = string.IsNullOrWhiteSpace(drow["employee_code"].ToString()) ? 0 : Convert.ToInt32(drow["employee_code"].ToString());
                    bll.employee_name = string.IsNullOrWhiteSpace(drow["employee_name"].ToString()) ? "" : drow["employee_name"].ToString();
                    bll.email_address = string.IsNullOrWhiteSpace(drow["email_address"].ToString()) ? "" : drow["email_address"].ToString();
                    bll.FullAccess = string.IsNullOrWhiteSpace(drow["fullaccess"].ToString()) ? false : Convert.ToBoolean(drow["fullaccess"]);
                }
                catch (Exception ex)
                {
                    if (RecordCount < 1)
                    {
                        RecordException RException = new RecordException();
                        RException.ERR_MSG = ex.Message;
                        RECORDED_ERR_LIST.Add(RException);
                    }
                }

            }
            return bll;

        }
        public static int GetbatchNumber()
        {
            int _min = 100;
            int _max = 999999999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }


    }
}
