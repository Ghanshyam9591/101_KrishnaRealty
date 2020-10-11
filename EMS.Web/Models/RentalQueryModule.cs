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
    public class RentalQueryModule
    {
        public List<RentalQueryModuleModel> GetGridDetail(int owner_id, int renter_id, int location_id, int building_id)
        {
            List<RentalQueryModuleModel> RPM_LIST = new List<RentalQueryModuleModel>();

            NpgsqlParameter[] parameters = {
                new NpgsqlParameter("plocation_id", location_id),
                new NpgsqlParameter("pbuilding_id", building_id),
                new NpgsqlParameter("powner_id", owner_id),
                new NpgsqlParameter("prenter_id", renter_id)
            };

            DataTable DT = DataAccess.ExecuteParaQuery(GetQuery.RENTAL_QUERY_MODULE_DATA_BY_PARA, parameters);
            if (DT.Rows.Count > 0)
            {
                RPM_LIST = QueryHandler.GetRentalQueryModuleGRDData(DT);
            }
            return RPM_LIST;
        }
    }
}