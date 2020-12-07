using EMS.Common;
using MicroORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EMS.Web.Models
{
    public class AutoEnquiry
    {
        public List<AutoEnquiryModl> GetAutoEnquiryGrid()
        {
          
            List<AutoEnquiryModl> RM_LIST = new List<AutoEnquiryModl>();

            DataTable DT = DataAccess.ExecuteQuery(GetQuery.AUTO_ENQUIRY_GRID_LIST);
            //DataTable DT = DataAccess.ExecuteParaQuery(@"select * from ems_tbl_release_for_payment where activity_date=@activity_date", param);

            if (DT.Rows.Count > 0)
            {
                RM_LIST = QueryHandler.GetAutoEnquiryGridList(DT);
            }
            return RM_LIST;
        }
    }
}