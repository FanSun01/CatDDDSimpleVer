using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatSimpleVer
{
    public static class UtilConvert
    {
        public static bool ObjToBool(this object input)
        {
            bool ans = false;
            if (input != null && input != DBNull.Value && bool.TryParse(input.ToString(), out ans))
            {
                return ans;
            }
            return ans;
        }

        public static string ObjToString(this object input)
        {
            if (input != null)
            {
                return input.ToString().Trim();
            }
            return "";
        }

        public static string DateToTimeStamp(this DateTime value)
        {
            TimeSpan ts = value - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }



    }
}
