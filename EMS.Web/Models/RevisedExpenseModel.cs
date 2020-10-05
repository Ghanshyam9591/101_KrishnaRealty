using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data;

namespace EMS.Web.Models
{
    //public class RevisedExpenseModel : EntityBusinessLogicBase<RevisedExpenseModel>
    //{
    //    //public string ACTIVITY_NUMBER { get; set; }
    //    //public string ACTIVITY_NAME { get; set; }
    //    //public DateTime? ACTIVITY_CREATION_DATE { get; set; }
    //    //public int EMP_CODE { get; set; }
    //    //public string EMP_NAME { get; set; }
    //    //public DateTime? ACTIVITY_DATE { get; set; }
    //    //public int MEETING_DURATION { get; set; }
    //    //public string MODE_OF_ACTIVITY { get; set; }
    //    //public string MODE_OF_CONVEYANCE { get; set; }
    //    //public string CLAIM_CONVEYANCE_EXPENSE { get; set; }
    //    //public decimal CONVEYANCE_EXPENSE_AMOUNT { get; set; }
    //    //public string CLAIM_CONVEYANCE_EXPENSE_FOR_RETURN_TRIP { get; set; }
    //    //public decimal RETURN_TRIP_AMOUNT { get; set; }
    //    //public string ADDITIONAL_DISRIBUTORS { get; set; }
    //    //public string PRODUCT_TYPE { get; set; }
    //    //public int SUPERVISOR_ID { get; set; }
    //    //public string SUPERVISOR_NAME { get; set; }
    //    //public DateTime? SUPERVISOR_APPROVAL_DATE { get; set; }
    //    //public decimal CLAIM_AMT { get; set; }
    //    //public string PARTNER_CODE { get; set; }
    //    //public string PARTNER_NAME { get; set; }
    //    //public int EMP_UFC_CODE { get; set; }
    //    //public int EMP_BANK_ACC_NUMBER { get; set; }
    //    //public string ACTIVITY_DESCRIPTION { get; set; }
    //    //public string CLAIM_TYPE { get; set; }
    //    //public string APPROVED_REJECTED { get; set; }
    //    //public int BATCH_NUMBER { get; set; }
    //    //public DateTime BATCH_NUMBER_DATE { get; set; }
    //    //public DateTime DATA_UPLOAD_DATE { get; set; }
    //    //public DateTime CREATED_DATE { get; set; }
    //    //public string TBL_SHORT_NAME { get; set; }
    //    //public string REMARK1 { get; set; } = null;
    //    //public string REMARK2 { get; set; } = null;
    //    //public string REMARK3 { get; set; } = null;

       

    //    public string AssignBatchNumber(DateTime AsOnDate, string ClaimType)
    //    {
    //        DbContext ctx = new DbContext(CONNECTION_STRING_NAME);

    //        NpgsqlParameter[] parameters = { new NpgsqlParameter("activity_date", Convert.ToDateTime(AsOnDate.ToString("yyyy-MM-dd"))),
    //                                         new NpgsqlParameter("claim_type",CLAIM_TYPE)
    //        };

    //        string result = ctx.ExecuteSclar("sp_record_waiting_for_payment", parameters);
    //        return result;
    //    }
    //}
}