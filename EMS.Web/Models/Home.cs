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
    public class Home
    {
        public List<DASHBOARD_DETAIL_MODEL> GetDashboardBoxDetail()
        {
            List<DASHBOARD_DETAIL_MODEL> RPM_LIST = new List<DASHBOARD_DETAIL_MODEL>();

            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@pdate",Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
            };

            DataTable DT = DataAccess.ExecuteParaQuery("select * from ems_fn_get_dashboad_detail('"+ DateTime.Now.ToString("yyyy-MM-dd") + "')", parameters);

            if (DT.Rows.Count > 0)
            {
                RPM_LIST = QueryHandler.Getdashbordboxdetail(DT);
            }
            return RPM_LIST;
        }
        public List<DASHBOARD_LOCATION_WISE_MODEL> GetDashboardLocationWise_Detail()
        {
            List<DASHBOARD_LOCATION_WISE_MODEL> RPM_LIST = new List<DASHBOARD_LOCATION_WISE_MODEL>();

            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@pdate",Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
            };

            DataTable DT = DataAccess.ExecuteParaQuery("select * from ems_fn_get_dashboad_detail_locationwise('" + DateTime.Now.ToString("yyyy-MM-dd") + "')", parameters);

            if (DT.Rows.Count > 0)
            {
                RPM_LIST = QueryHandler.Getdashbordlocationwise_detail(DT);
            }
            return RPM_LIST;
        }
        public List<RentalQueryModuleModel> Get_Rental_Dashboard_Detail()
        {
            List<RentalQueryModuleModel> RPM_LIST = new List<RentalQueryModuleModel>();

            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("@pdate",Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
            };

            DataTable DT = DataAccess.ExecuteParaQuery("select * from ems_fn_get_rental_dashboard()", parameters);

            if (DT.Rows.Count > 0)
            {
                RPM_LIST = QueryHandler.Get_rental_dashboard_detail(DT);
            }
            return RPM_LIST;
        }
    }
}