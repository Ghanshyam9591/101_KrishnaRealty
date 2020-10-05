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
    public class FlatOwners
    {
        public FlatOwnerModel GetItem(int id)
        {
            FlatOwnerModel bll = new FlatOwnerModel();
            if (id == 0)
            {
                bll = new FlatOwnerModel();
            }
            else
            {
                NpgsqlParameter[] parameters = { new NpgsqlParameter("pseqid", id) };

                DataTable DT = DataAccess.ExecuteParaQuery(GetQuery.FLAT_OWNER_DATA_BY_SEQID, parameters);
                if (DT.Rows.Count > 0)
                {
                    List<FlatOwnerModel> userlist = QueryHandler.GetFlatOwnersGridList(DT);
                    if (userlist.Count > 0)
                        bll = userlist[0];
                }
            }
            return bll;
        }
        public List<FlatOwnerModel> LIST_DATA()
        {
            List<FlatOwnerModel> listmodel = new List<FlatOwnerModel>();

            //NpgsqlParameter[] parameters = { new NpgsqlParameter("@seqid", "") };

            DataTable DT = DataAccess.ExecuteParaQuery(GetQuery.FLAT_OWNER_GRID_DATA, null);
            if (DT.Rows.Count > 0)
            {
                listmodel = QueryHandler.GetFlatOwnersGridData(DT);

            }
            return listmodel;
        }
        public string Save(FlatOwnerModel model)
        {
            string Response = string.Empty;
            List<FlatOwnerModel> bll = new List<FlatOwnerModel>();
            DbCommonHelper dbcom = new DbCommonHelper();

            NpgsqlParameter[] Insert_Parameters = {
                                             new NpgsqlParameter("@name",string.IsNullOrEmpty(model.name)?"":model.name),
                                             new NpgsqlParameter("@mobile1",model.mobile1),
                                             new NpgsqlParameter("@mobile2",model.mobile2),
                                             new NpgsqlParameter("@email",string.IsNullOrEmpty(model.email)?"":model.email),
                                             new NpgsqlParameter("@location_id",model.location_id),
                                             new NpgsqlParameter("@deposit",model.deposit),
                                             new NpgsqlParameter("@monthly_rent",model.monthly_rent),
                                             new NpgsqlParameter("@property_type_id",model.property_type_id),
                                             new NpgsqlParameter("@remark",string.IsNullOrEmpty(model.remark) ? "":model.remark),
                                             new NpgsqlParameter("@created_by",model.created_by),
                                             new NpgsqlParameter("@created_date",DateTime.Now)

            };

            NpgsqlParameter[] Select_Parameters = {
                                             new NpgsqlParameter("@seqid",model.seqid)
            };


            NpgsqlParameter[] update_Parameters = {
                                             new NpgsqlParameter("@name",string.IsNullOrEmpty(model.name)?"":model.name),
                                             new NpgsqlParameter("@mobile1",model.mobile1),
                                             new NpgsqlParameter("@mobile2",model.mobile2),
                                             new NpgsqlParameter("@email",string.IsNullOrEmpty(model.email)?"":model.email),
                                             new NpgsqlParameter("@location_id",model.location_id),
                                             new NpgsqlParameter("@deposit",model.deposit),
                                             new NpgsqlParameter("@monthly_rent",model.monthly_rent),
                                             new NpgsqlParameter("@property_type_id",model.property_type_id),
                                             new NpgsqlParameter("@remark",string.IsNullOrEmpty(model.remark) ? "":model.remark),
                                             new NpgsqlParameter("@seqid",model.seqid)

            };



            Response = dbcom.Save(GetQuery.FLAT_OWNER_INSERT_QUERY, GetQuery.FLAT_OWNER_RECORD_CHECK_QUERY, GetQuery.FLATE_OWNER_UPDATE_QUERY, Insert_Parameters, Select_Parameters, update_Parameters);
            return Response;
        }
    }
}