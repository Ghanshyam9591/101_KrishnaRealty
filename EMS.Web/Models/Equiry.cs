using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using EMS.Common;
using MicroORM;
using Npgsql;
using System.Text;

namespace EMS.Web.Models
{
    public class Equiry : EntityBusinessLogicBase<Equiry>
    {
        List<EnquiryDataModel> REM_LIST = new List<EnquiryDataModel>();

        List<RecordException> RECORDED_ERR_LIST = new List<RecordException>();


        public List<EnquiryDataModel> GetGridDetail(DateTime enquiry_date, int location_id)
        {

            List<EnquiryDataModel> RPM_LIST = new List<EnquiryDataModel>();

            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@activity_date",enquiry_date.ToString("yyyy-MM-dd"))

            };
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"select * from ems_tbl_enquiry_trans
                                    where enquiry_date ='" + enquiry_date.ToString("yyyy-MM-dd") + "'");

            if (location_id != 0)
            {
                sb.AppendLine("and location_id='" + location_id + "'");
            }
            DataTable DT = DataAccess.ExecuteQuery(sb.ToString());

            if (DT.Rows.Count > 0)
            {
                RPM_LIST = QueryHandler.GetEnquiryDataGridList(DT);
            }
            return RPM_LIST;
        }


        //public List<RevisedExpenseModel> GetSapDataGridDetail(DateTime AsOnDate, string ClaimType)
        //{

        //    List<RevisedExpenseModel> RPM_LIST = new List<RevisedExpenseModel>();

        //    NpgsqlParameter[] parameters = {
        //        new NpgsqlParameter("@activity_creation_date",AsOnDate.ToString("yyyy-MM-dd"))
        //    };
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine(@"SELECT sd.seqid,sd.activity_number, sd.activity_name, 
        //                                 sd.activity_creation_date, sd.emp_code, sd.emp_name, sd.activity_date, sd.meeting_duration
        //                                 , sd.mode_of_activity, sd.mode_of_conveyance, sd.claim_conveyance_expense, sd.conveyance_expense_amount
        //                                  , sd.claim_con_exp_for_ret_trip, sd.return_trip_amount, sd.additional_disributors, sd.product_type, sd.supervisor_id
        //                                   , sd.supervisor_name, sd.supervisor_approval_date, sd.claim_amt, sd.partner_code, sd.partner_name,sd.claim_type,  
        //                      		sd.batch_number, sd.batch_number_date, sd.data_upload_date, sd.created_date
        //                      		,sd.last_updated_date,sd.group_activity,sd.mode,sd.activity_type,sd.claim_status,sd.status
        //                             FROM ems_tbl_sap_data sd
        //                       left outer join ems_tbl_release_for_payment rp on rp.emp_code=sd.emp_code 
        //                             where sd.batch_number=rp.batch_number
        //                       and rp.claim_type=rp.claim_type
        //                       and  sd.sap_generate_data is null
        //                       and rp.payment_date <='" + AsOnDate.ToString("yyyy-MM-dd") + "'");

        //    if (ClaimType.Trim() != "Select")
        //    {
        //        sb.AppendLine("and claim_type='" + ClaimType + "'");
        //    }
        //    DataTable DT = DataAccess.ExecuteQuery(sb.ToString());

        //    if (DT.Rows.Count > 0)
        //    {
        //        RPM_LIST = QueryHandler.GetRecordList(DT);
        //    }
        //    return RPM_LIST;
        //}
        //public List<RecordException> AssignBatchNumber(List<EnquiryModel> model_list)
        //{
        //    List<RecordException> recException = new List<RecordException>();
        //    if (model_list.Count > 0)
        //    {
        //        MultiModel MM_MODEL = QueryHandler.GetMultiRecordList(model_list);
        //        if ((MM_MODEL.Revised_Expense_model.Count > 0) && (MM_MODEL.Release_Payment_Model.Count > 0) && MM_MODEL.Record_Exception.Count < 1)
        //        {
        //            DbCommonHelper DCH = new DbCommonHelper();
        //            recException = DCH.PushToDb2(MM_MODEL.Revised_Expense_model, MM_MODEL.Release_Payment_Model);
        //        }
        //    }
        //    return recException;
        //}



        //public List<RecordException> ChangeSapFileStatus(List<RevisedExpenseModel> model_list)
        //{
        //    List<RecordException> recException = new List<RecordException>();
        //    if (model_list.Count > 0)
        //    {
        //        DbCommonHelper DCH2 = new DbCommonHelper();
        //        recException = DCH2.StatusChangeSapFileGeneration(model_list);
        //    }
        //    return recException;
        //}
    }
}