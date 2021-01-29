using System.Data.SqlClient;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;

namespace Runbow.TWS.Dao
{
    public class TYscanAccessor : BaseAccessor
    {
        /// <summary>
        /// 查询天翼扫描记录
        /// </summary>
        public IEnumerable<TYscan> GetQueryTYscan(TYscanSearchCondition SearchCondition, int PageIndex, int PageSize, out int RowCount)
        {
            string sqlWhere = this.GenQueryWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_QueryTYscan", dbParams);
            RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<TYscan>();
        }

        /// <summary>
        /// 查询天翼同步记录汇总
        /// </summary>
        public IEnumerable<TYscanGroupBy> GetQueryTYscanGroupBy(TYscanSearchCondition SearchCondition)
        {
            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@Start_ActualDeliveryDate", DbType.String, SearchCondition.StatCreateTime, ParameterDirection.Input),
                new DbParam("@End_ActualDeliveryDate", DbType.String, SearchCondition.EndCreateTime, ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable("Proc_QueryTYscanGroupBy", dbParams);
            return dt.ConvertToEntityCollection<TYscanGroupBy>();
        }

        /// <summary>
        /// 查询天翼扫描明细
        /// </summary>
        public IEnumerable<TYscanDetail> GetQueryTYscanDetail(string CustomerOrderNumbers)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(CustomerOrderNumbers))
            {
                string[] customers = CustomerOrderNumbers.Split(',');
                sb.Append(" CustomerOrderNumber IN(");
                foreach (string s in customers)
                {
                    sb.Append("'").Append(s).Append("',");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(")");
            }
            else
            {
                sb.Append(" 1=1 ");
            }

            //int tempRowCount = 0;
            //DbParam[] dbParams = new DbParam[]
            //{
            //    new DbParam("@PodID", DbType.String, sb.ToString(), ParameterDirection.Input),
            //    new DbParam("@PageIndex", DbType.Int32, PageIndex, ParameterDirection.Input),
            //    new DbParam("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input),
            //    new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            //};
            string sql = "select R.PODID,R.CustomerOrderNumber,R.Creator,R.CreateTime,R.Str1 AS 'LabelNo',R.str3 AS 'SHTor',R.str4 AS 'SHTime',  ";
            sql += " R.str5 AS 'FHTor',R.DateTime1 AS 'FHTime' FROM dbo.PODDetail R Where " + sb.ToString() +"  ORDER BY R.CustomerOrderNumber";
            DataTable dt = this.ExecuteDataTableBySqlString(sql);
            //DataTable dt = this.ExecuteDataTable("Proc_QueryTYscanDetail", dbParams);
            //RowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<TYscanDetail>();
        }


        /// <summary>
        /// 查询条件
        /// </summary>
        private string GenQueryWhere(TYscanSearchCondition scan)
        {
            StringBuilder sb = new StringBuilder();


            if (!string.IsNullOrEmpty(scan.CustomerOrderNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (scan.CustomerOrderNumber.IndexOf("\n") > 0)
                {
                    numbers = scan.CustomerOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (scan.CustomerOrderNumber.IndexOf(',') > 0)
                {
                    numbers = scan.CustomerOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and CustomerOrderNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and CustomerOrderNumber  like '%" + scan.CustomerOrderNumber.Trim() + "%' ");
                }
            }

            //if (!string.IsNullOrEmpty(scan.CustomerOrderNumber))
            //{
            //    sb.Append(" and CustomerOrderNumber like '%" + scan.CustomerOrderNumber.Trim() + "%' ");
            //}

            if (scan.StatCreateTime.HasValue)
            {
                sb.Append(" and Convert(varchar(10),ActualDeliveryDate,120)>='" + scan.StatCreateTime.Value.ToString("yyyy-MM-dd") + "' ");
            }

            if (scan.EndCreateTime.HasValue)
            {
                sb.Append(" and Convert(varchar(10),ActualDeliveryDate,120)<='" + scan.EndCreateTime.Value.ToString("yyyy-MM-dd") + "' ");

            }

            if (scan.type.HasValue)
            {
                if (scan.type == 1)
                {
                    sb.Append(" and [str42] ='1' ");
                }
                else if(scan.type == 0)
                {
                    sb.Append(" and [str42] <>'1' ");
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// 导出天翼扫描明细报表
        /// </summary>
        public DataTable Proc_GetTYscanData(string starTime, string endsTime)
        {
            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@starTime", DbType.String, starTime, ParameterDirection.Input),
                new DbParam("@endsTime", DbType.String, endsTime, ParameterDirection.Input)
            };
            return this.ExecuteDataTable("Proc_GetTYscanData", dbParams);
        }

        ///// <summary>
        ///// 微信用户审核
        ///// </summary>
        //public int UploadWXCustomer(long podID)
        //{
        //    DbParam[] dbParams = new DbParam[]{
        //        new DbParam("@ID", DbType.Int64, podID, ParameterDirection.Input),
        //        new DbParam("@ReturnVal", DbType.Int32, 0, ParameterDirection.Output),
        //    };

        //    base.ExecuteNoQuery("Proc_UploadWXCustomerById", dbParams);

        //    return dbParams[1].Value.ObjectToInt32();
        //}
    }
}
