using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;

namespace Runbow.TWS.Common
{
    public class SignHelper
    {
        /// <summary>
        /// 类转参
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionary<T>(T t)
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            PropertyInfo[] propertyInfos = t.GetType().GetProperties();
            foreach (PropertyInfo pi in propertyInfos)
            {
                var key = pi.Name;
                var value = pi.GetValue(t);
                if (value == DBNull.Value || value == null)
                    value = "";
                else
                {
                    if (pi.PropertyType == typeof(DateTime?) || pi.PropertyType == typeof(DateTime))
                        value = Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss");
                }

                keyValuePairs.Add(key, value.ToString());
            }
            return keyValuePairs;
        }

        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="signMethod"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public static string SignTopRequest(IDictionary<string, string> parameters, string signMethod, string appSecret)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();
            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder();
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }
            query.Append(appSecret);
            // 第三步：使用MD5加密
            byte[] bytes;
            MD5 md5 = MD5.Create();
            bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }
            return result.ToString();
        }

    }
}
