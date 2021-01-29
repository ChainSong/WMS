using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Runbow.TWS.Common.Util
{
    public class JsonHelper
    {
         /// <summary>
        /// 把json字符串转成对象
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="data">json字符串</param> 
        public static T Deserialize<T>(string data)
        {
            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
            return json.Deserialize<T>(data);
        }

        /// <summary>
        /// 把对象转成json字符串
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string Serialize(object o)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
            json.Serialize(o, sb);
            return sb.ToString();
        }

        /// <summary>
        /// 把DataTable对象转成json字符串
        /// </summary> 
        public string ToJson(DataTable dt)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName]);
                }
                arrayList.Add(dictionary);
            }
            return javaScriptSerializer.Serialize(arrayList);
        }
    }

    public static class EnumerableExtension 
    {
        /// <summary>
        /// 集合添加一个对象
        /// </summary> 
        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value)
        {
            foreach (var cur in e)
            { yield return cur; }
            yield return value;
        }

        /// <summary>
        /// 把集合转成DataTable
        /// </summary> 
        public static DataTable ConvertToDataTable<T>(this IEnumerable<T> enumerable)
        {
            var dataTable = new DataTable();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(typeof(T)))
            {
                dataTable.Columns.Add(pd.Name, pd.PropertyType);
            }
            foreach (T item in enumerable)
            {
                var Row = dataTable.NewRow();

                foreach (PropertyDescriptor dp in TypeDescriptor.GetProperties(typeof(T)))
                {
                    Row[dp.Name] = dp.GetValue(item);
                }
                dataTable.Rows.Add(Row);
            }
            return dataTable;
        } 
    
    }
}
