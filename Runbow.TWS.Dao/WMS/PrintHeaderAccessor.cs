using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Print;
using Runbow.TWS.MessageContracts.WMS.Print;
using Runbow.TWS.Common;
using System.Data.SqlClient;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.WMS.PreOrders;

namespace Runbow.TWS.Dao.WMS
{
    public class PrintHeaderAccessor : BaseAccessor
    {
        public GetPrintByConditionResponse GetPrintHeaderByCondition(PrintHeaderSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            GetPrintByConditionResponse response = new GetPrintByConditionResponse();
            string sqlWhere = this.GenGetPrintHeaderWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintHeaderByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            response.PrintHeaderCollection = ds.Tables[0].ConvertToEntityCollection<PrintHeader>();
            return response;
        }

        public GetPrintByConditionResponse GetPrintHeaderAndDetailByID(int ID)
        {
            GetPrintByConditionResponse response = new GetPrintByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int16, ID, ParameterDirection.Input)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintHeaderAndDetailByID", dbParams);
            response.PrintHeaderCollection = ds.Tables[0].ConvertToEntityCollection<PrintHeader>();
            response.PrintDetailCollection = ds.Tables[1].ConvertToEntityCollection<PrintDetail>();
            return response;
        }

        public string GenGetPrintHeaderWhere(PrintHeaderSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(SearchCondition.PrintKey))
                sb.Append(" and w.PrintKey='" + SearchCondition.PrintKey + "'");
            if (!String.IsNullOrEmpty(SearchCondition.CustomerID.ToString()))
                sb.Append(" and w.CustomerID='" + SearchCondition.CustomerID + "'");
            //if (!String.IsNullOrEmpty(SearchCondition.WarehouseID.ToString()))
            //    sb.Append(" and w.WarehouseID='" + SearchCondition.WarehouseID + "'");
            if (!string.IsNullOrEmpty(SearchCondition.WarehouseName))
            {
                sb.Append(" AND w.WarehouseName in  (select WarehouseName from wms_warehouse where id in ( ").Append(SearchCondition.WarehouseName.Trim()).Append(")) ");
                //sb.Append(" AND o.Warehouse='").Append(SearchCondition.Warehouse).Append("' ");
            }
            return sb.ToString();
        }

        public GetPrintByConditionResponse CreateOrUpdatePrintHeaderAndDetail(int CustomerID, string CustomerName, int WarehouseID, string WarehouseName, string Creator, IEnumerable<PreOrderIds> ids, int PrintID, string PrintKey)
        {
            GetPrintByConditionResponse response = new GetPrintByConditionResponse();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Proc_CreateOrUpdatePrintHeaderAndDetail", conn);//Proc_WMS_AutomatedOutbound     Proc_WMS_AutomatedOutbound_Total
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDS", ids.Select(a => new WMSPreOrderIdsToDb(a)));//这是声明一个参数 并赋值
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;  //声明第一个参数的类型
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);//声明第二个参数  并赋值
                    cmd.Parameters[1].SqlDbType = SqlDbType.Int; // 声明第二个参数的类型
                    cmd.Parameters.AddWithValue("@CustomerName", CustomerName);//声明第三个参数
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;//声明参数类型
                    cmd.Parameters.AddWithValue("@WarehouseID", WarehouseID);//声明第二个参数  并赋值
                    cmd.Parameters[3].SqlDbType = SqlDbType.Int; // 声明第二个参数的类型
                    cmd.Parameters.AddWithValue("@WarehouseName", WarehouseName);//声明第三个参数
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;//声明参数类型
                    cmd.Parameters.AddWithValue("@Creator", Creator);//声明第三个参数
                    cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;//声明参数类型
                    cmd.Parameters.AddWithValue("@PrintID", PrintID);//声明第二个参数  并赋值
                    cmd.Parameters[6].SqlDbType = SqlDbType.Int; // 声明第二个参数的类型
                    cmd.Parameters.AddWithValue("@PrintKey", PrintKey);//声明第三个参数
                    cmd.Parameters[7].SqlDbType = SqlDbType.NVarChar;//声明参数类型
                    //cmd.Parameters.AddWithValue("@Message", message);//声明第四个参数
                    //cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;//声明参数的类型
                    //cmd.Parameters[3].Direction = ParameterDirection.Output;//声明参数是输出类型
                    //cmd.Parameters[3].Size = 8000;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);//将得到的数据 填充到DataTable中
                    //message = sda.SelectCommand.Parameters["@Message"].Value.ToString();//获得数据库  out出来的参数的值 （并不是由return 而来）
                    conn.Close();

                    response.PrintHeaderCollection = ds.Tables[0].ConvertToEntityCollection<PrintHeader>();
                    response.PrintDetailCollection = ds.Tables[1].ConvertToEntityCollection<PrintDetail>();
                    //return ds.Tables[ds.Tables.Count - 1].ConvertToEntityCollection<DistributionInformation>();
                    //if (ds.Tables.Count > 1)
                    //{
                    //    return ds.Tables[1].ConvertToEntityCollection<DistributionInformation>();
                    //}
                    //else
                    //{
                    //    return ds.Tables[0].ConvertToEntityCollection<DistributionInformation>();
                    //}

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return response;
        }

        //public GetPrintByConditionResponse RelateExpressKey(int ID, string OrderKey, string ExpressKey, string Updator)
        //{
        //    GetPrintByConditionResponse response = new GetPrintByConditionResponse();
        //    DbParam[] dbParams = new DbParam[]{
        //        new DbParam("@ID", DbType.Int16, ID, ParameterDirection.Input),
        //        new DbParam("@OrderKey", DbType.String, OrderKey, ParameterDirection.Input),
        //        new DbParam("@ExpressKey", DbType.String, ExpressKey, ParameterDirection.Input),
        //        new DbParam("@Updator", DbType.String, Updator, ParameterDirection.Input)
        //    };
        //    DataSet ds = this.ExecuteDataSet("Proc_WMS_RelateOrderKeyAndExpressKey", dbParams);
        //    response.Prompt = ds.Tables[0].Rows[0][0].ToString();
        //    return response;
        //}

        /// <summary>
        /// 关联快递单号
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="OrderKey"></param>
        /// <param name="ExpressKey"></param>
        /// <param name="Updator"></param>
        /// <returns></returns>
        public string RelateExpressKey(int ID, string OrderKey, string ExpressKey, string Updator)
        {
            string message = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_RelateOrderKeyAndExpressKey", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@OrderKey", OrderKey);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@ExpressKey", ExpressKey);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Updator", Updator);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar; ;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[4].Direction = ParameterDirection.Output;
                    cmd.Parameters[4].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    cmd.CommandTimeout = 300;
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception e)
                {
                    return e.Message.ToString();
                }

            }
        }



        public string UpdatePrintStatus(string IDs)
        {
            string message = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("Proc_UpdatePrintHeaderAndDetailStatus", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ids", IDs);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.Int;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    cmd.CommandTimeout = 300;
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception e)
                {
                    return "系统错误！";
                }

            }
        }

        ///// <summary>
        ///// /验证波次是否超过范围
        ///// </summary>
        ///// <param name="HeaderID"></param>
        ///// <param name="Type"></param>
        ///// <returns></returns>
        //public int VerifyWaveSize(long HeaderID, int Type)
        //{
        //    using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("Proc_WMS_VerifyWaveSize", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@HeaderID", HeaderID);
        //        cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
        //        cmd.Parameters.AddWithValue("@Type", Type);
        //        cmd.Parameters[1].SqlDbType = SqlDbType.Int;
        //        cmd.CommandTimeout = 300;
        //        conn.Open();
        //        DataSet ds = new DataSet();
        //        SqlDataAdapter sda = new SqlDataAdapter();
        //        sda.SelectCommand = cmd;
        //        sda.Fill(ds);
        //        int qty = cmd.ExecuteScalar().ObjectToInt32();
        //        conn.Close();
        //        return qty;

        //    }
        //}

        /// <summary>
        /// 将当前波次明细查询出来
        /// </summary>
        /// <param name="PrintID"></param>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public IEnumerable<OrderInfo> GetOrderInfoByPrintID(long PrintID, string Ids)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@PrintID", DbType.Int64, PrintID, ParameterDirection.Input),
                 new DbParam("@Ids", DbType.String, Ids, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetOrderInfoByPrintID", dbParams);
            return ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
        }

    }
}
