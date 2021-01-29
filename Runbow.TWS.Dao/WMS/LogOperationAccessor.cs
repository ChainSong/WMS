using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.Common;

namespace Runbow.TWS.Dao.WMS
{
    public class LogOperationAccessor : BaseAccessor
    {
        public string AddLogOperation(IEnumerable<WMS_Log_Operation> LogOperations)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddLogOperation", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LogOperation", LogOperations.Select(operation => new LogOperationToDb(operation)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 50;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }
            }
        }
        public string AddLogOperationRF(IEnumerable<WMS_Log_OperationRF> LogOperations)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddLogOperationRF", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LogOperation", LogOperations.Select(operation => new LogOperationRFToDb(operation)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 50;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }
            }
        }

        public string UpdateLogOperationPackageStatusRF(string username, string BoxNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_UpdateLogOperationPackageStatusRF", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@BoxNumber", BoxNumber);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size = 100;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 50;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }
            }
        }
        /// <summary>
        /// 查询显示 分页查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public IEnumerable<WMS_Log_Operation> GetLogOperationByCondition(LogOperationSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {

            string sqlWhere = this.GenGetWhere(SearchCondition);

            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetLogOperationByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<WMS_Log_Operation>();
        }
        /// <summary>
        /// 查询显示 分页查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public IEnumerable<WMS_Log_OperationRF> GetLogOperationRFByCondition(LogOperationRFSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {

            string sqlWhere = this.GenGetRFWhere(SearchCondition);

            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_GetLogOperationRFByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            return dt.ConvertToEntityCollection<WMS_Log_OperationRF>();
        }
        public IEnumerable<WMS_Log_OperationRF> ExportLogOperationRFByCondition(LogOperationRFSearchCondition SearchCondition)
        {
            string sqlWhere = this.GenGetRFWhere(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable("Proc_WMS_ExportLogOperationRFByCondition", dbParams);
            return dt.ConvertToEntityCollection<WMS_Log_OperationRF>();
        }
        /// <summary>
        /// 条件拼接
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        private string GenGetWhere(LogOperationSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND a.CustomerID='").Append(SearchCondition.CustomerID).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.OrderNumber))
            {
                sb.Append(" AND a.OrderNumber='").Append(SearchCondition.OrderNumber).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExternOrderNumber))
            {
                sb.Append(" AND a.ExternOrderNumber='").Append(SearchCondition.ExternOrderNumber).Append("' ");
            }
            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append(" AND a.CreateTime>='" + SearchCondition.StartCreateTime.ToString() + "'");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" AND a.CreateTime<='" + SearchCondition.EndCreateTime.ToString() + "'");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND a.WarehouseName='").Append(SearchCondition.Warehouse).Append("' ");
            }
            return sb.ToString();
        }



        /// <summary>
        /// 条件拼接
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        private string GenGetRFWhere(LogOperationRFSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.ReleateNumber))
            {
                sb.Append(" and ReleateNumber  like '%" + SearchCondition.ReleateNumber.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.PackageNumber))
            {
                sb.Append(" and PackageNumber  like '%" + SearchCondition.PackageNumber.Trim() + "%' ");
            }
            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append(" AND CreateTime>='" + SearchCondition.StartCreateTime.ToString() + "'");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" AND CreateTime<='" + SearchCondition.EndCreateTime.ToString() + "'");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Creator))
            {
                sb.Append(" AND Creator='").Append(SearchCondition.Creator).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.LogType))
            {
                sb.Append(" AND LogType='").Append(SearchCondition.LogType).Append("' ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// cord中间表订单日志
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        public bool AddNikeCordOrderLog(List<NikeCrodOrderLog> logs)
        {
            string sql = "";
            foreach (var item in logs)
            {
                sql += "   INSERT INTO dbo.NikeCrodOrderLog(OrderCode,Type,Operation,Remark,Creator,CreateTime,Str1,Str2,Str3,Str4,Str5 )" +
                       "   Values('" + item.OrderCode + "','" + item.Type + "','" + item.Operation + "','" + item.Remark + "','" + item.Creator + "'," +
                       "   getdate(),'" + item.Str1 + "','" + item.Str2 + "','" + item.Str3 + "','" + item.Str4 + "','" + item.Str5 + "') ";

            }
            return this.ScanExecuteNonQuery(sql) > 0;
        }

        /// <summary>
        /// 退货仓 sftp日志
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        public bool AddNikeReturnSFTPLogs(List<WMS_NikeReturnSFTP_Log> logs)
        {
            if (logs.Any())
            {
                string sql = "";
                foreach (var item in logs)
                {
                    sql += "INSERT INTO dbo.WMS_NikeReturnSFTP_Log " +
                            " (CreateTime, SourceFileName, ToFileName, Type, Flag, ResultDesc, Str1, Str2, Str3, Str4, Str5," +
                            " Str6, Str7, Str8, Str9, Str10, Int1, Int2, Int3, DateTime1, DateTime2, DateTime3)" +
                            " VALUES(GETDATE(),'" + item.SourceFileName + "','" + item.ToFileName + "','" + item.Type + "','" + item.Flag + "','" + item.ResultDesc + "','" + item.Str1 + "',  " +
                            " '" + item.Str2 + "','" + item.Str3 + "','" + item.Str4 + "','" + item.Str5 + "','" + item.Str6 + "','" + item.Str7 + "','" + item.Str8 + "','" + item.Str9 + "', " +
                            " '" + item.Str10 + "'," + item.Int1.ObjectToString2() + "," + item.Int2.ObjectToString2() + "," + item.Int3.ObjectToString2() + ", " +
                            " " + item.DateTime1.ObjectToString2() + "," + item.DateTime2.ObjectToString2() + "," + item.DateTime3.ObjectToString2() + "  " +
                            "  ); ";
                }
                return this.ScanExecuteNonQuery(sql) > 0;
            }
            else
            {
                return false;
            }
        }
        
    }
}
