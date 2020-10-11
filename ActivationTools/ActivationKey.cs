using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivationTools
{
    public class ActivationKey
    {
        static DateTime ValidTill = Convert.ToDateTime("2021-10-10");

        public static bool IsLock()
        {

            if (DateTime.Now < ValidTill)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
