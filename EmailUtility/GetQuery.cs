using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailUtility
{
    public class GetQuery
    {
        public static string Get_Select_Email_Information
        {
            get
            {
               return @"select * from lms_tbl_emailaudit where date(createddate)=current_date order by 1 desc";
            }
        }
    }
}
