using EMS.Common;
using MicroORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Npgsql;
using System.Text;

namespace EMS.Web.Models
{
    public class ReleasingOfPayment : EntityBusinessLogicBase<ReleasingOfPayment>
    {

        public List<ReleasePaymentModel> GetGridDetail(DateTime AsOnDate, string ClaimType)
        {
            List<ReleasePaymentModel> RPM_LIST = new List<ReleasePaymentModel>();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"select * from ems_tbl_release_for_payment
                                    where batch_number >0 and payment_done is null and batch_number_date <='" + AsOnDate.ToString("yyyy-MM-dd") + "'");

            if (ClaimType.Trim() != "Select")
            {
                sb.AppendLine("and claim_type='" + ClaimType + "'");
            }
            DataTable DT = DataAccess.ExecuteQuery(sb.ToString());
            //DataTable DT = DataAccess.ExecuteParaQuery(@"select * from ems_tbl_release_for_payment where activity_date=@activity_date", param);

            if (DT.Rows.Count > 0)
            {
                RPM_LIST = QueryHandler.GetReleaseRecordList(DT, AsOnDate);
            }
            return RPM_LIST;
        }

        public List<RecordException> PaymentStatus(List<ReleasePaymentModel> releasePaymentModel)
        {
            List<RecordException> RPM_LIST = new List<RecordException>();

            if (releasePaymentModel.Count > 0)
            {
                DbCommonHelper bll = new DbCommonHelper();
                RPM_LIST = bll.PushToTBLPaymentDone(releasePaymentModel);
            }
            return RPM_LIST;
        }
    }
}