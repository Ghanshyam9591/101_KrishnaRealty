using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Common;
using Npgsql;
using MicroORM;
using System.Data;
using System.Text;

namespace EMS.Web.Models
{
    public class QueryModule : EntityBusinessLogicBase<QueryModule>
    {
        public List<QueryModelRPT> GetGridDetail(int cust_id, int location_id, int enquiry_source_id, string enquiry_from, string enquiry_to, int action_type_id, string query_type)
        {
            List<QueryModelRPT> RPM_LIST = new List<QueryModelRPT>();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(GetQuery.QUERY_MODULE_GRID_DATA);
            if (cust_id != 0)
            {
                sb.AppendLine(" and et.seqid=" + cust_id + "");
            }
            if (location_id != 0)
            {
                sb.AppendLine(" and et.location_id=" + location_id + "");
            }
            if (enquiry_source_id != 0)
            {
                sb.AppendLine(" and et.enquiry_source_id=" + enquiry_source_id + "");
            }
            if (!string.IsNullOrEmpty(enquiry_from) && !string.IsNullOrEmpty(enquiry_to))
            {
                DateTime VAL_ENQUIRY_FROM = DateTime.Parse(enquiry_from);
                DateTime VAL_ENQUIRY_TO = DateTime.Parse(enquiry_to);
                sb.AppendLine(" and et.enquiry_date between '" + VAL_ENQUIRY_FROM.ToString("yyyy-MM-dd") + "' and '" + VAL_ENQUIRY_TO.ToString("yyyy-MM-dd") + "'");
            }
            if (action_type_id != 0)
            {
                sb.AppendLine(" and es.action_type_id=" + action_type_id + "");
            }
            sb.AppendLine(GetQuery.QUERY_MODULE_GRID_GROUP_BY);

            DataTable DT = DataAccess.ExecuteQuery(sb.ToString());
            if (DT.Rows.Count > 0)
            {
                RPM_LIST = QueryHandler.GetQueryModuleGRDData(DT);


            }
            return RPM_LIST;
        }

        public string Save(Int64 Enquiry_No, Int64 Action_Type_ID, string Comments)
        {
            string Response = string.Empty;
            List<EnquiryStatusModel> bll = new List<EnquiryStatusModel>();
            DbCommonHelper dbcom = new DbCommonHelper();

            NpgsqlParameter[] update_Parameters = {

                                             new NpgsqlParameter("@enquiry_no",Enquiry_No),
                                             new NpgsqlParameter("@action_taken_by",UserManager.User.employee_code),
                                             new NpgsqlParameter("@action_type_id",Action_Type_ID),
                                             new NpgsqlParameter("@remark",Comments),
                                             new NpgsqlParameter("@action_date",DateTime.Now),
                                             new NpgsqlParameter("@created_by",UserManager.User.employee_code),
                                             new NpgsqlParameter("@created_date",DateTime.Now)
            };


            Response = dbcom.Save(GetQuery.ADD_ENQUIRY_CALL_UP_COMMENTS, update_Parameters);
            return Response;
        }
    }
}