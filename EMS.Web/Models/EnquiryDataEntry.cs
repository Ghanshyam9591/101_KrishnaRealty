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
    public class EnquiryDataEntry
    {
        public EnquiryDataModel GetItem(int id)
        {
            EnquiryDataModel bll = new EnquiryDataModel();
            if (id == 0)
            {
                bll = new EnquiryDataModel();
            }
            else
            {
                NpgsqlParameter[] parameters = { new NpgsqlParameter("@seqid", id) };

                DataTable DT = DataAccess.ExecuteParaQuery(GetQuery.ENQUIRY_DATA_BY_SEQID, parameters);
                if (DT.Rows.Count > 0)
                {
                    List<EnquiryDataModel> userlist = QueryHandler.GetEnquiryDataGridList(DT);
                    if (userlist.Count > 0)
                        bll = userlist[0];
                }
            }
            return bll;
        }
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

            NpgsqlParameter[] Insert_Parameters = {
                                             new NpgsqlParameter("@name",string.IsNullOrEmpty(model.name) ? "":model.name),
                                             new NpgsqlParameter("@mobile1",model.mobile1),
                                             new NpgsqlParameter("@mobile2",model.mobile2),
                                             new NpgsqlParameter("@email",string.IsNullOrEmpty(model.email)?"":model.email),
                                             new NpgsqlParameter("@location_id",model.location_id),
                                             new NpgsqlParameter("@cost_upto",model.cost_upto),
                                             new NpgsqlParameter("@property_type_id",model.property_type_id),
                                             new NpgsqlParameter("@enquiry_source_id",model.enquiry_source_id),
                                             new NpgsqlParameter("@remark",string.IsNullOrEmpty(model.remark) ? "":model.remark),
                                             new NpgsqlParameter("@created_by",model.created_by),
                                             new NpgsqlParameter("@created_date",DateTime.Now),
                                             new NpgsqlParameter("@enquiry_date",model.enquiry_date)
            };

            NpgsqlParameter[] Select_Parameters = {
                                             new NpgsqlParameter("@seqid",model.seqid)
            };


            NpgsqlParameter[] update_Parameters = {
                                             new NpgsqlParameter("@name",string.IsNullOrEmpty(model.name) ? "":model.name),
                                             new NpgsqlParameter("@mobile1",model.mobile1),
                                             new NpgsqlParameter("@mobile2",model.mobile2),
                                             new NpgsqlParameter("@email", string.IsNullOrEmpty(model.email)?"":model.email),
                                             new NpgsqlParameter("@location_id",model.location_id),
                                             new NpgsqlParameter("@cost_upto",model.cost_upto),
                                             new NpgsqlParameter("@property_type_id",model.property_type_id),
                                             new NpgsqlParameter("@enquiry_source_id",model.enquiry_source_id),
                                             new NpgsqlParameter("@remark",string.IsNullOrEmpty(model.remark) ? "":model.remark),
                                             new NpgsqlParameter("@enquiry_date",model.enquiry_date),
                                             new NpgsqlParameter("@seqid",model.seqid)
            };



            Response = dbcom.Save(GetQuery.ENQUIRY_INSERT_QUERY, GetQuery.ENQUIRY_RECORD_CHECK_QUERY, GetQuery.ENQUIRY_DATA_ENTRY_UPDATE_QUERY, Insert_Parameters, Select_Parameters, update_Parameters);
            return Response;
        }
    }
}