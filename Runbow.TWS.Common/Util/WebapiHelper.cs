using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Common.Util
{
    public class WebapiHelper
    {
        /// <summary>
        /// BASE64 编码(string  to  base64)
        /// </summary>
        public static string StringToBase64(string ingput)
        {
            System.Text.Encoding encode = System.Text.Encoding.ASCII;
            byte[] bytedata = encode.GetBytes(ingput);
            return Convert.ToBase64String(bytedata, 0, bytedata.Length);
        }


        /// <summary>
        /// BASE64 解码(base64  to  string)
        /// </summary>
        public static string ToBase64String(string ingput)
        {
            //string strPath =  "aHR0cDovLzIwMy44MS4yOS40Njo1NTU3L19iYWlkdS9yaW5ncy9taWRpLzIwMDA3MzgwLTE2Lm1pZA==";
            byte[] bpath = Convert.FromBase64String(ingput);
            return System.Text.ASCIIEncoding.Default.GetString(bpath);
        }
    }
}
