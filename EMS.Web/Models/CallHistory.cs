using EMS.Common;
using MicroORM;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace EMS.Web.Models
{
    public class CallHistory : EntityBusinessLogicBase<CALL_HISTORY_MODEL>
    {
        public List<CALL_HISTORY_MODEL> GetGridDetail(int enquiry_no)
        {
            List<CALL_HISTORY_MODEL> RPM_LIST = new List<CALL_HISTORY_MODEL>();

            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@enquiry_no",enquiry_no)
            };

            DataTable DT = DataAccess.ExecuteParaQuery(GetQuery.CALL_HISTRY_CUSTOMER_GRID, parameters);

            if (DT.Rows.Count > 0)
            {
                RPM_LIST = QueryHandler.GetFileUploadStatusList(DT);
            }
            return RPM_LIST;
        }
    }
}