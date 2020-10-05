using MicroORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Web.Models
{
    public class SAPData : EntityBusinessLogicBase<SAPData>
    {
        public List<dynamic> GetGridDetail(DateTime AsOnDate, string ClaimType)
        {
            DbContext ctx = new DbContext(CONNECTION_STRING_NAME);
            var result = ctx.SetCommand(@"
                     SELECT	activity_number,activity_name,activity_creation_date,emp_code,emp_name,activity_date,meeting_duration
                                    			,mode_of_activity,mode_of_conveyance,claim_conveyance_expense,conveyance_expense_amount
                                    			,claim_con_exp_for_ret_trip,return_trip_amount,additional_disributors,product_type,supervisor_id
                                    			,supervisor_name,supervisor_approval_date,claim_amt,partner_code,partner_name,emp_ufc_code,emp_bank_acc_number
                                    			,activity_description,claim_type,approved_rejected,batch_number,batch_number_date,data_upload_date,created_date,runtime_status
                                    FROM ems_tbl_record_waiting_for_payment
                                    where batch_number is null and activity_date=@activity_date 
                                            and claim_type= 
											case 
											when @claim_type is null then claim_type
											else claim_type end
											")
                                    .AddParameter("@activity_date", Convert.ToDateTime(AsOnDate.ToString("yyyy-MM-dd")))
                                    .AddParameter("@claim_type", ClaimType)
                .ExecuteReader().ToList();
            return result;
        }
    }
}