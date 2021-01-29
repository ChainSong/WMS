using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Common;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.MessageContracts.WMS.BarCode;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Order;

namespace Runbow.TWS.Dao.WMS
{
    public class BarCodeAccessor : BaseAccessor
    {
        public IEnumerable<BarCodeInfo> GetBarCodeByOID(long OrderID)
        {
            string Where = " and OrderID in(" + OrderID + ")";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = string.Empty;
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_GetBarCodeByCondition", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Where", Where);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    IEnumerable<BarCodeInfo> list = ds.Tables[0].ConvertToEntityCollection<BarCodeInfo>();
                    conn.Close();
                    return list;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public IEnumerable<BarCodeInfo> GetBarCodeByOrderID(long OrderID, string Type,long DetailID,string SKU)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = string.Empty;
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_GetBarCodeByOrderID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@Type", Type);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@DetailID", DetailID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@SKU", SKU);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    IEnumerable<BarCodeInfo> list = ds.Tables[0].ConvertToEntityCollection<BarCodeInfo>();
                    conn.Close();
                    return list;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public IEnumerable<BarCodeInfo> GetBarCodeByDetailIDS(string DetailIDS)
        {
            string Where = " and DetailID in(" + DetailIDS + ")";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = string.Empty;
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_GetBarCodeByCondition", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Where", Where);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    IEnumerable<BarCodeInfo> list = ds.Tables[0].ConvertToEntityCollection<BarCodeInfo>();
                    conn.Close();
                    return list;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public DataTable GetSKUAndBarCodeByBarCode(string BarCode)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = string.Empty;
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_GetSKUAndBarCodeByBarCode", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BarCode", BarCode);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    //IEnumerable<BarCodeInfo> list = ds.Tables[0].ConvertToEntityCollection<BarCodeInfo>();
                    conn.Close();
                    //return list;
                    return ds.Tables[0];
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public IEnumerable<BarCodeInfo> GetBarCodeByIDS(string IDS)
        {
            string Where = " and ID in(" + IDS + ")";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = string.Empty;
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_GetBarCodeByCondition", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Where", Where);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    IEnumerable<BarCodeInfo> list = ds.Tables[0].ConvertToEntityCollection<BarCodeInfo>();
                    conn.Close();
                    return list;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public string GenerateBarCode(IEnumerable<BarCodeInfo> list)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = string.Empty;
                try
                {
                    SqlCommand cmd = new SqlCommand("Prco_WMS_GenerateBarCode", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BarCodeTable", list.Select(a => new BarCodeTableToDb(a)));//这是声明一个参数 并赋值
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;  //声明第一个参数的类型
                    cmd.Parameters.AddWithValue("@Message", message);//声明第二个参数
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;//声明参数的类型
                    cmd.Parameters[1].Direction = ParameterDirection.Output;//声明参数是输出类型
                    cmd.Parameters[1].Size = 8000;
                    cmd.CommandTimeout = 300;
                    conn.Open();

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

        public string GenerateBarCodeOrder(IEnumerable<BarCodeInfo> list)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = string.Empty;
                try
                {
                    SqlCommand cmd = new SqlCommand("Prco_WMS_GenerateBarCodeOrder", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BarCodeTable", list.Select(a => new BarCodeTableToDb(a)));//这是声明一个参数 并赋值
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;  //声明第一个参数的类型
                    cmd.Parameters.AddWithValue("@Message", message);//声明第二个参数
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;//声明参数的类型
                    cmd.Parameters[1].Direction = ParameterDirection.Output;//声明参数是输出类型
                    cmd.Parameters[1].Size = 8000;
                    cmd.CommandTimeout = 300;
                    conn.Open();

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

        public GetBarCodeByConditionResponse QueryBarCodeList(BarCodeSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            string sqlWhere = "";
            if (SearchCondition != null)
            {
                sqlWhere = GetConditionStr(SearchCondition);
            }
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetBarCodeByConditionPage]", dbParams);
            rowCount = (int)dbParams[3].Value;
            GetBarCodeByConditionResponse response = new GetBarCodeByConditionResponse();
            response.BarCodeCollection = dt.ConvertToEntityCollection<BarCodeInfo>();
            return response;
        }

        public string GetConditionStr(BarCodeSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND CustomerID='").Append(SearchCondition.CustomerID).Append("' ");
            }
            if (SearchCondition.WarehouseID != 0)
            {
                sb.Append(" AND WarehouseID='").Append(SearchCondition.WarehouseID).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Type))
            {
                sb.Append(" AND Type='").Append(SearchCondition.Type).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.OrderNumber))
            {
                sb.Append(" AND OrderNumber='").Append(SearchCondition.OrderNumber).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.SKU))
            {
                sb.Append(" AND SKU='").Append(SearchCondition.SKU).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.BarCode))
            {
                sb.Append(" AND BarCode='").Append(SearchCondition.BarCode).Append("' ");
            }
            if (SearchCondition.StartCreateTime!=null)
            {
                sb.Append(" AND CreateTime>='").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime!=null)
            {
                sb.Append(" AND CreateTime<='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            return sb.ToString();
        }

        public bool DeleteBarCode(string PackageKey)
        {
            try
            {
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@PackageKey", DbType.String,PackageKey, ParameterDirection.Input)
                };
                base.ExecuteNoQuery("Proc_WMS_DeleteBarCode", dbParams);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<BarCodeInfo> CheckScanBarCode(IEnumerable<BarCodeInfo> list)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Prco_CheckScanBarCode", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BarCodeTable", list.Select(a => new BarCodeTableToDb(a)));//这是声明一个参数 并赋值
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;  //声明第一个参数的类型
                    cmd.CommandTimeout = 300;

                    conn.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    IEnumerable<BarCodeInfo> list2 = ds.Tables[0].ConvertToEntityCollection<BarCodeInfo>();
                    conn.Close();
                    return list2;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public string GenerateBarCodeByScan(long OrderID, IEnumerable<BarCodeInfo> list)
        {
            string message = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Prco_WMS_GenerateBarCodeByScan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", OrderID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters[0].Direction = ParameterDirection.Input;
                    //cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
                    //cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    //cmd.Parameters[1].Direction = ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("@BarCodeTable", list.Select(a => new BarCodeTableToDb(a)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 8000;
                    cmd.CommandTimeout = 300;
                    conn.Open();

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

        public string SupplyBarCode(long ID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = string.Empty;
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_SupplyBarCode", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);//这是声明一个参数 并赋值
                    cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;  //声明第一个参数的类型
                    cmd.Parameters.AddWithValue("@Message", message);//声明第二个参数
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;//声明参数的类型
                    cmd.Parameters[1].Direction = ParameterDirection.Output;//声明参数是输出类型
                    cmd.Parameters[1].Size = 8000;
                    cmd.CommandTimeout = 300;
                    conn.Open();

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

        public void GetBarCodeCount(long ID, out int BarCodeCount, out int QtyCount)
        {
            int count1 = 0;
            int count2 = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.String, ID, ParameterDirection.Input),
                new DbParam("@BarCodeCount", DbType.Int32, count1, ParameterDirection.Output),
                new DbParam("@QtyCount", DbType.Int32, count2, ParameterDirection.Output)
            };

            DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetBarCodeCountByOrderID]", dbParams);
            BarCodeCount = (int)dbParams[1].Value;
            QtyCount = (int)dbParams[2].Value;
        }
    }
}
