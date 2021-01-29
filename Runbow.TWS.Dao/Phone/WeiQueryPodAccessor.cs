using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Common;
using Runbow.TWS.MessageContracts;
using System.Text.RegularExpressions;

namespace Runbow.TWS.Dao
{
    public class WeiQueryPodAccessor : BaseAccessor
    {
        public IEnumerable<WeiQueryPod> GetWeiQueryPodInfo(string Id,WeiQueryPod into, string Type, int PageIndex, int PageSize, out int RowCount)
        {
                string sqlWhere = this.GenQueryWhere(Id);
                int tempRowCount = 0;
                DbParam[] dbParams = new DbParam[]
               {
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
                };
                DataTable dt = this.ExecuteDataTable("proc_WeiChart_UserConfig", dbParams);
                RowCount = (int)dbParams[3].Value;
                return dt.ConvertToEntityCollection<WeiQueryPod>();
           
            }
         /// <summary>
        /// 查询条件
        /// </summary>
        private string GenQueryWhere(string id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("and CustomerCode='" + id + "'");
            return sb.ToString();
        }
        public string PingFen(string Id, int ping, string ValFrom)
        {
            string sqlWhere = this.PingFensqlWhere(Id);
          
            DbParam[] dbParams = new DbParam[]
               {
                new DbParam("@Ping", DbType.String, ping, ParameterDirection.Input),
                  new DbParam("@ValFrom", DbType.String, ValFrom, ParameterDirection.Input),
                    new DbParam("@Sqlwhere", DbType.String, sqlWhere, ParameterDirection.Input),
                };
            DataTable dt = this.ExecuteDataTable("proc_PingFen", dbParams);
            //RowCount = (int)dbParams[3].Value;
            return "";
        }
        private string PingFensqlWhere(string Id)
        {
            Regex r = new Regex("[a-z|A-Z]+");
            Match m = r.Match(Id);
            StringBuilder sb = new StringBuilder();
            sb.Append(" and CarrierCode=" + m.Value).Append(" and ArrTime='" + Id.Substring(Id.Length - 10)+"'");

            return sb.ToString();
        }
    }
}
