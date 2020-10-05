using EMS.Common;
using MicroORM;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EMS.Web.Models
{
    public class AssignEnquiry
    {
        public List<EnquiryDataModel> LIST_DATA()
        {
            List<EnquiryDataModel> listmodel = new List<EnquiryDataModel>();

            //NpgsqlParameter[] parameters = { new NpgsqlParameter("@seqid", "") };

            DataTable DT = DataAccess.ExecuteParaQuery(GetQuery.ENQUIRY_GRID_DATA, null);
            if (DT.Rows.Count > 0)
            {
                listmodel = QueryHandler.GetEnquiryDataGridList(DT);

            }
            return listmodel;
        }

        public string Save(EnquiryDataModel model)
        {
            string Response = string.Empty;
            List<EnquiryDataModel> bll = new List<EnquiryDataModel>();
            DbCommonHelper dbcom = new DbCommonHelper();

            NpgsqlParameter[] update_Parameters = {
                                            
                                             new NpgsqlParameter("@assign_to_id",model.assign_to_id),
                                             new NpgsqlParameter("@assign_to_date",DateTime.Now),
                                             new NpgsqlParameter("@seqid",model.seqid)
            };


            Response = dbcom.Update(GetQuery.ASSIGN_ENQUIRY_TO_AGENT, update_Parameters);
            return Response;
        }
    }
}