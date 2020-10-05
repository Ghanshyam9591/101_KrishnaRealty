using EMS.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MicroORM;

namespace EMS.Web.Models
{
    public class Lov
    {
        public LovModel GetItem(int id)
        {
            //RoleModel bll = new RoleModel();
            //return bll;


            LovModel bll = new LovModel();
            if (id == 0)
            {
                bll = new LovModel();
            }
            else
            {
                NpgsqlParameter[] parameters = { new NpgsqlParameter("@id", id) };

                DataTable DT = DataAccess.ExecuteParaQuery(GetQuery.LOV_DATA_ID, parameters);
                if (DT.Rows.Count > 0)
                {
                    List<LovModel> rolelist = QueryHandler.GetLovGridList(DT);
                    if (rolelist.Count > 0)
                        bll = rolelist[0];
                }
            }
            return bll;


        }
        public string Save(LovModel model)
        {
            string Response = string.Empty;

            DbCommonHelper dbcom = new DbCommonHelper();

            NpgsqlParameter[] Insert_Parameters = {
                new NpgsqlParameter("pid",model.id),
                new NpgsqlParameter("pcode",model.code),
                new NpgsqlParameter("pname",model.name),
                new NpgsqlParameter("pcategoryid",model.categoryid),
                new NpgsqlParameter("pisactive",model.is_active),
                new NpgsqlParameter("plogin_user",Helper.GetUserID())
            };

            Response = dbcom.ExecuteProcedure("select ems_fun_lov_crud_activity(:pid,:pcode,:pname,:pcategoryid,:pisactive,:plogin_user)", Insert_Parameters);
            return Response;
        }

        public List<LovModel> GetLovGrid()
        {
            LovCategory bll = new LovCategory();

            List<LovModel> RM_LIST = new List<LovModel>();

            DataTable DT = DataAccess.ExecuteQuery(GetQuery.LOV_GRID_DATA);
            //DataTable DT = DataAccess.ExecuteParaQuery(@"select * from ems_tbl_release_for_payment where activity_date=@activity_date", param);

            if (DT.Rows.Count > 0)
            {
                RM_LIST = QueryHandler.GetLovGridList(DT);
            }
            return RM_LIST;
        }
    }
}
