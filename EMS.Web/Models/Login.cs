using MicroORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using EMS.Common;
using System.DirectoryServices;
using System.Configuration;
using Npgsql;

namespace EMS.Web.Models
{
    public class Login : EntityBusinessLogicBase<Login>
    {
        public UserDetailsModel GetUserDetail(string UserID, string Password)
        {
            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@employee_code",Convert.ToInt32(UserID)),
                new NpgsqlParameter("@password",Password)
               };
            UserDetailsModel umodel = new UserDetailsModel();
            DataTable DT = DataAccess.ExecuteParaQuery("select * from ems_tbl_user_master where employee_code=@employee_code and password=@password", parameters);

            if (DT.Rows.Count > 0)
            {
                umodel = QueryHandler.GetUserDetails(DT);
            }
            else
            {
                umodel = null;
            }
            return umodel;
        }
    }
}