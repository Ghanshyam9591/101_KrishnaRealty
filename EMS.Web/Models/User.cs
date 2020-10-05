using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMS.Common;
using Npgsql;
using System.Data;
using MicroORM;

namespace EMS.Web.Models
{
    public class User
    {
        public UserModel GetItem(int id)
        {
            UserModel bll = new UserModel();
            if (id == 0)
            {
                bll = new UserModel();
            }
            else
            {
                NpgsqlParameter[] parameters = { new NpgsqlParameter("@seqid", id) };

                DataTable DT = DataAccess.ExecuteParaQuery(GetQuery.USER_DATA_BY_ID, parameters);
                if (DT.Rows.Count > 0)
                {
                    List<UserModel> userlist = QueryHandler.GetUserGridList(DT);
                    if (userlist.Count > 0)
                        bll = userlist[0];
                }
            }
            return bll;
        }
        public string Save(UserModel model)
        {
            string Response = string.Empty;
            List<UserModel> bll = new List<UserModel>();
            DbCommonHelper dbcom = new DbCommonHelper();

            NpgsqlParameter[] Insert_Parameters = {
                                             new NpgsqlParameter("@employee_code",model.employee_code),
                                             new NpgsqlParameter("@employee_name",model.employee_name),
                                             new NpgsqlParameter("@email_address",model.email_address),
                                             new NpgsqlParameter("@role_id",model.role_id),
                                             new NpgsqlParameter("@is_active",model.is_active),
                                             new NpgsqlParameter("@created_date",DateTime.Now)
            };

            NpgsqlParameter[] Select_Parameters = {
                                             new NpgsqlParameter("@employee_code",model.employee_code),
            };

            NpgsqlParameter[] update_Parameters = {
                                             new NpgsqlParameter("@seqid",model.seqid),
                                             new NpgsqlParameter("@employee_name",model.employee_name),
                                             new NpgsqlParameter("@email_address",model.email_address),
                                             new NpgsqlParameter("@role_id",model.role_id),
                                             new NpgsqlParameter("@is_active",model.is_active),
                                            
            };

            Response = dbcom.Save(GetQuery.USER_INSERT_QUERY, GetQuery.USER_RECORD_CHECK_QUERY, GetQuery.USER_RECORD_UPDATION_QUERY, Insert_Parameters, Select_Parameters, update_Parameters);
            return Response;
        }

        public List<UserModel> GetRoleGrid()
        {
            UserModel bll = new UserModel();

            List<UserModel> RM_LIST = new List<UserModel>();

            DataTable DT = DataAccess.ExecuteQuery(GetQuery.USER_GRID_DATA);
            //DataTable DT = DataAccess.ExecuteParaQuery(@"select * from ems_tbl_release_for_payment where activity_date=@activity_date", param);

            if (DT.Rows.Count > 0)
            {
                RM_LIST = QueryHandler.GetUserGridList(DT);
            }
            return RM_LIST;
        }
    }
}