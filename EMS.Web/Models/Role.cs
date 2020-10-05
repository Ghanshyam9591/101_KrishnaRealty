using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Common;
using Npgsql;
using System.Text;
using System.Data;
using MicroORM;

namespace EMS.Web.Models
{
    public class Role
    {
        public RoleModel GetItem(int id)
        {
            //RoleModel bll = new RoleModel();
            //return bll;


            RoleModel bll = new RoleModel();
            if (id == 0)
            {
                bll = new RoleModel();
            }
            else
            {
                NpgsqlParameter[] parameters = { new NpgsqlParameter("@seqid", id) };

                DataTable DT = DataAccess.ExecuteParaQuery(GetQuery.ROLE_DATA_SEQID, parameters);
                if (DT.Rows.Count > 0)
                {
                    List<RoleModel> rolelist = QueryHandler.GetRoleGridList(DT);
                    if (rolelist.Count > 0)
                        bll = rolelist[0];
                }
            }
            return bll;


        }
        public string Save(RoleModel model)
        {
            string Response = string.Empty;

            DbCommonHelper dbcom = new DbCommonHelper();

            NpgsqlParameter[] Insert_Parameters = {
                                             new NpgsqlParameter("@role_code",model.role_code),
                                             new NpgsqlParameter("@role_name",model.role_name),
                                             new NpgsqlParameter("@is_active",model.is_active),
                                             new NpgsqlParameter("@created_date",DateTime.Now)
            };

            NpgsqlParameter[] Select_Parameters = {
                                             new NpgsqlParameter("@role_code",model.role_code),
            };

            NpgsqlParameter[] Update_Parameters = {
                                             new NpgsqlParameter("@role_name",model.role_name),
                                             new NpgsqlParameter("@is_active",model.is_active),
                                             new NpgsqlParameter("@seqid",model.seqid)
            };

            Response = dbcom.Save(GetQuery.ROLE_INSERT_QUERY, GetQuery.ROLE_RECORD_CHECK_QUERY, GetQuery.ROLE_RECORD_UPDATE_QUERY, Insert_Parameters, Select_Parameters, Update_Parameters);
            return Response;
        }

        public List<RoleModel> GetRoleGrid()
        {
            RoleModel bll = new RoleModel();

            List<RoleModel> RM_LIST = new List<RoleModel>();

            DataTable DT = DataAccess.ExecuteQuery(GetQuery.ROLE_GRID_DATA);
            //DataTable DT = DataAccess.ExecuteParaQuery(@"select * from ems_tbl_release_for_payment where activity_date=@activity_date", param);

            if (DT.Rows.Count > 0)
            {
                RM_LIST = QueryHandler.GetRoleGridList(DT);
            }
            return RM_LIST;
        }
    }
}