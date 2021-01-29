using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Order;
using Runbow.TWS.MessageContracts.WMS.Order;
using Runbow.TWS.Common;

namespace Runbow.TWS.Dao.WMS
{
    public class WaveAccessor : BaseAccessor
    {
        public string CreateWave(string IsSinglePriece, string IsExpressCompany, string CustomerID, string CustomerName, string WarehouseID, string WarehouseName, string WaveCount,string Creator)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = string.Empty;
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_AutoDisTributionWave", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@WhereIsSinglePriece", Convert.ToInt32(IsSinglePriece));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@WhereIsExpressCompany", Convert.ToInt32(IsExpressCompany));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@CustomerID", Convert.ToInt32(CustomerID));
                    cmd.Parameters[2].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@CustomerName", CustomerName);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Size = 50;
                    cmd.Parameters.AddWithValue("@WarehouseID", Convert.ToInt32(WarehouseID));
                    cmd.Parameters[4].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@WarehouseName", @WarehouseName);
                    cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[5].Size = 50;
                    cmd.Parameters.AddWithValue("@WaveCount", Convert.ToInt32(WaveCount));
                    cmd.Parameters[6].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@Creator", Creator);
                    cmd.Parameters[7].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[7].Size = 50;
                    cmd.Parameters.AddWithValue("@Message", message);//声明第二个参数
                    cmd.Parameters[8].SqlDbType = SqlDbType.NVarChar;//声明参数的类型
                    cmd.Parameters[8].Direction = ParameterDirection.Output;//声明参数是输出类型
                    cmd.Parameters[8].Size = 8000;
                    conn.Open();
                    cmd.CommandTimeout = 300;

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    //IEnumerable<BarCodeInfo> list2 = ds.Tables[0].ConvertToEntityCollection<BarCodeInfo>();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public GetWaveByConditionResponse GetWaveHeaderByCondition(WaveSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            GetWaveByConditionResponse response = new GetWaveByConditionResponse();
            string sqlWhere = this.GenGetOrderWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetWaveHeaderByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            response.WaveCollection = ds.Tables[0].ConvertToEntityCollection<WMS_Wave>();
            return response;
        }

        public string GenGetOrderWhere(WaveSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.WaveNumber))
                sb.Append(" and w.WaveNumber='" + SearchCondition.WaveNumber + "'");
            if (SearchCondition.CustomerID != 0)
                sb.Append(" and w.CustomerID=" + SearchCondition.CustomerID);
            if (SearchCondition.WarehouseID != 0)
                sb.Append(" and w.WarehouseID=" + SearchCondition.WarehouseID);
            if (!string.IsNullOrEmpty(SearchCondition.OrderNumber))
                sb.Append(" and w.ID in(select WaveID from WMS_WaveDetail where OrderNumber='" + SearchCondition.OrderNumber + "')");
            if (!string.IsNullOrEmpty(SearchCondition.ExternOrderNumber))
                sb.Append(" and w.ID in(select WaveID from WMS_WaveDetail where ExternOrderNumber='" + SearchCondition.ExternOrderNumber + "')");
            if (!string.IsNullOrEmpty(SearchCondition.PreOrderNumber))
                sb.Append(" and w.ID in(select WaveID from WMS_WaveDetail where PreOrderNumber='" + SearchCondition.PreOrderNumber + "')");
            if (!string.IsNullOrEmpty(SearchCondition.StartCreateTime.ToString()))
                sb.Append(" and w.CreateTime>='" + SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00") + "'");
            if (!string.IsNullOrEmpty(SearchCondition.EndCreateTime.ToString()))
                sb.Append(" and w.CreateTime<='" + SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99") + "'");
            if (!string.IsNullOrEmpty(SearchCondition.StartCompleteTime.ToString()))
                sb.Append(" and w.CreateTime>='" + SearchCondition.StartCompleteTime.DateTimeToString("yyyy-MM-dd 00:00:00.00") + "'");
            if (!string.IsNullOrEmpty(SearchCondition.EndCompleteTime.ToString()))
                sb.Append(" and w.CreateTime<='" + SearchCondition.EndCompleteTime.DateTimeToString("yyyy-MM-dd 23:59:59.99") + "'");
            return sb.ToString();
        }

        public GetWaveByConditionResponse GetWaveHeaderAndDetail(int ID)
        {
            GetWaveByConditionResponse response = new GetWaveByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int32, ID, ParameterDirection.Input)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetWaveHeaderAndDetail", dbParams);
            response.WaveCollection = ds.Tables[0].ConvertToEntityCollection<WMS_Wave>();
            response.WaveDetailCollection = ds.Tables[1].ConvertToEntityCollection<WMS_WaveDetail>();
            return response;
        }
    }
}
