using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System.Data.SqlClient;
using System.Data;
using Runbow.TWS.MessageContracts.WMS.Order;
using Runbow.TWS.Entity.WMS.Order;
using Runbow.TWS.Entity.DataBaseEntity;

namespace Runbow.TWS.Dao.RFWeb
{
    public class OrderManagementAccessor : BaseAccessor
    {
        public IEnumerable<OrderInfo> GetOrderList(string customerid, string WarehouseName, string ExternOrderNumber)
        {
            IEnumerable<OrderInfo> list = new List<OrderInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                              new DbParam("@WarehouseName", DbType.String,WarehouseName,ParameterDirection.Input),
                              new DbParam("@ExternOrderNumber", DbType.String,ExternOrderNumber,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetOrderList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<OrderInfo>();
                    return list;
                }

            }
        }

        public string AddPackageAndDetail(long ID, AddPackageAndDetailRequest request, int flag)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddPackageANDDetail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@Package", request.packages.Select(p => new WMSPackageToDb(p)));
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@PackageDetail", request.packageDetails.Select(p => new WMSPackageDetailToDb(p)));
                    cmd.Parameters[2].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@flag", flag);
                    cmd.Parameters[3].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[4].Direction = ParameterDirection.Output;
                    cmd.Parameters[4].Size = 500;
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
        /// redis插入拣货表
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public bool InsertPick(IEnumerable<OrderDetailForRedisRF> Request,string name)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    string message = "";
                    SqlCommand cmd = new SqlCommand("[Proc_WMS_AddPick_RF]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Pick", Request.Select(p => new WMSOrderDetailToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@UserName", name);
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    conn.Open();
                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    conn.Close();
                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    return false;

                }
                return false;
            }
        }

        public bool UpdateOrderStatusByOrderNumber(string OrderNumber, string Picker, AddPackageAndDetailRequest request,string CustomerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Proc_WMS_RF_UpdateOrderStatusByOrderNumber]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Picker", Picker);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Package", request.packages.Select(p => new WMSPackageToDb(p)));
                cmd.Parameters[2].SqlDbType = SqlDbType.Structured;
                  cmd.Parameters.AddWithValue("@PackageDetail", request.packageDetails.Select(p => new WMSPackageDetailToDb(p)));
                     cmd.Parameters[3].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                conn.Open();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public IEnumerable<OrderDetailForRedisRF> GetOrderDetailListByOrderNumber(string OrderNumber, string CustomerID)
        {
            IEnumerable<OrderDetailForRedisRF> list = new List<OrderDetailForRedisRF>();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@OrderNumber", DbType.String,OrderNumber,ParameterDirection.Input),
                        new DbParam("@CustomerID", DbType.String,CustomerID,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetOrderDetailListByOrderNumber", param);
                    list = ds.Tables[0].ConvertToEntityCollection<OrderDetailForRedisRF>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<OrderDetailForRedisRF>();
                    return list;
                }

            }
        }
        public bool UpdatePackage(string updatesql)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_RF_UpdatePackage", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UpdateSql", updatesql);
                    cmd.Parameters[0].SqlDbType = SqlDbType.Text;

                    conn.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    return false;
                }

            }
        }
        public GetOrderByConditionResponse GetPackageByCondition(long ID)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();
            long IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@IDs", DbType.Int64, IDs, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPackageByID", dbParams);
            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            response.packages = ds.Tables[1].ConvertToEntityCollection<PackageInfo>();
            response.packageDetails = ds.Tables[2].ConvertToEntityCollection<PackageDetailInfo>();
            return response;
        }
        public bool UpdateOrderStatus(string ReceiptNumber, string customerid, string warehousename, string Picker, string type)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("[Proc_WMS_RF_UpdateOrderStatus]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderNumber", ReceiptNumber);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@CustomerID", customerid);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@WarehouseName", warehousename);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Picker", Picker);
                cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                conn.Open();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    return true;
                }
                return false;
            }
        }
        public IEnumerable<OrderDetailForRedisRF> GetOrderDetailList(string ReceiptNumber, string customerid, string warehousename)
        {
            IEnumerable<OrderDetailForRedisRF> list = new List<OrderDetailForRedisRF>();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@OrderNumber", DbType.String,ReceiptNumber,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                          new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetOrderDetailList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<OrderDetailForRedisRF>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<OrderDetailForRedisRF>();
                    return list;
                }

            }
        }
        public IEnumerable<OrderDetailInfo> GetOrderDetailListByWave(string ReceiptNumber, string customerid, string warehousename)
        {
            IEnumerable<OrderDetailInfo> list = new List<OrderDetailInfo>();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@OrderNumber", DbType.String,ReceiptNumber,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                          new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetOrderDetailListByWave", param);
                    list = ds.Tables[0].ConvertToEntityCollection<OrderDetailInfo>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<OrderDetailInfo>();
                    return list;
                }

            }
        }
        public IEnumerable<PackageDetailInfo> GetPackageDetailList(string OrderID, string customerid, string warehousename)
        {
            IEnumerable<PackageDetailInfo> list = new List<PackageDetailInfo>();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@OrderID", DbType.String,OrderID,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                          new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetPackageDetailList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<PackageDetailInfo>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<PackageDetailInfo>();
                    return list;
                }

            }
        }
        public IEnumerable<PackageInfo> GetPackageList(string OrderNumber, string customerid, string warehousename)
        {
            IEnumerable<PackageInfo> list = new List<PackageInfo>();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@OrderNumber", DbType.String,OrderNumber,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                          new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetPackageList", param);
                    list = ds.Tables[0].ConvertToEntityCollection<PackageInfo>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<PackageInfo>();
                    return list;
                }

            }
        }
        public IEnumerable<OrderDetailInfo> GetOrderDetailList2(string ReceiptNumber, string customerid, string warehousename)
        {
            IEnumerable<OrderDetailInfo> list = new List<OrderDetailInfo>();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                        new DbParam("@OrderNumber", DbType.String,ReceiptNumber,ParameterDirection.Input),
                         new DbParam("@CustomerID", DbType.String,customerid,ParameterDirection.Input),
                          new DbParam("@WarehouseName", DbType.String,warehousename,ParameterDirection.Input)
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_RF_GetOrderDetailList2", param);
                    list = ds.Tables[0].ConvertToEntityCollection<OrderDetailInfo>();
                    return list;

                }
                catch (Exception ex)
                {
                    list = new List<OrderDetailInfo>();
                    return list;
                }

            }
        }

        public ExpressPackageResponse CheckExpress(string ExpressNumber, long CustomerID, string WarehouseName)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                int message = 100;
                ExpressPackageResponse EPR = new ExpressPackageResponse();
                try
                {

                    DataSet dt = new DataSet();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_CheckExpressAndGetOrder", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ExpressNumber", ExpressNumber);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;

                    cmd.Parameters.AddWithValue("@WarehouseName", WarehouseName);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;

                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.Int;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;

                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    cmd.CommandTimeout = 300;
                    EPR.AssociationStatus = Convert.ToInt32(sda.SelectCommand.Parameters["@message"].Value);
                    message = Convert.ToInt32(sda.SelectCommand.Parameters["@message"].Value);
                    conn.Close();
                    if (message > 0)
                    {
                        EPR.PackageCollection = dt.Tables[0].ConvertToEntity<PackageInfo>();
                        EPR.OrderDetailCollection = dt.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
                    }
                    return EPR;
                }
                catch (Exception)
                {
                    return EPR;
                }
            }
        }
        public DataSet CheckExpress(string ExpressNumber, long CustomerID, string WarehouseName, string Type)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                int message = 100;
                DataSet dt = new DataSet();
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_CheckExpressAndGetOrder", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExpressNumber", ExpressNumber);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@WarehouseName", WarehouseName);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.Int;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;

                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    cmd.CommandTimeout = 300;
                    conn.Close();
                    return dt;
                }
                catch (Exception)
                {
                    return dt;
                }
            }
        }
        public DataSet SaveExpressPackage(string ExpressNumber, string PackageType, long CustomerID, string WarehouseName)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                DataSet dt = new DataSet();
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_SaveExpressPackage", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExpressNumber", ExpressNumber);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@PackageType", PackageType);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[2].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@WarehouseName", WarehouseName);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[4].Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = 540;

                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    conn.Close();
                    return dt;
                }
                catch (Exception e)
                {
                    dt.Tables[0].Rows[0][1] = e.Message;
                    return dt;
                }
            }
        }

        /// <summary>
        /// 爱库存扫描波次号拣货，将订单更新为已拣货状态
        /// </summary>
        /// <param name="printKey"></param>
        /// <param name="userName"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool RFSaveWavePicking_AKC(string printKey, string userName, out string msg)
        {
            msg = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DataSet dt = new DataSet();

                SqlCommand cmd = new SqlCommand("Proc_WMS_RFSaveWavePicking_AKC", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PrintKey", printKey);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@message", msg);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.Parameters[2].Size = 500;
                cmd.CommandTimeout = 540;

                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                msg = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                if (!string.IsNullOrEmpty(msg))
                    return false;
                else
                    return true;

            }
        }

        /// <summary>
        /// 获取订单头和明细 爱库存
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="BatchNumber"></param>
        /// <param name="QueryType"></param>
        /// <returns></returns>
        public OrderAndDetailModel RFGetOrderAndDetail_AKC(string PrintKey)
        {
            OrderAndDetailModel model = new OrderAndDetailModel();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_WMS_RFGetOrderAndDetail_AKC", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PrintKey", PrintKey);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.CommandTimeout = 300;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                conn.Close();
                if (ds.Tables.Count > 0)
                {
                    model.orderInfos = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                    model.orderDetailInfos = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// 查询已经分拣的信息
        /// </summary>
        /// <param name="OID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public IEnumerable<OrderPickDetail> RFSearchSecondSorting_AKC(string PrintKey)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                DbParam[] param = new DbParam[] {
                        new DbParam("@PrintKey", DbType.String,PrintKey,ParameterDirection.Input)
                    };
                DataSet ds = ExecuteDataSet("Proc_WMS_RFSearchSecondSorting_AKC", param);
                return ds.Tables[0].ConvertToEntityCollection<OrderPickDetail>();
            }
        }

        /// <summary>
        /// 二次分拣数据提交 爱库存
        /// </summary>
        /// <param name="PrintKey"></param>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool RFSaveSecondSorting_AKC(string PrintKey, string username, out string msg)
        {
            msg = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_WMS_RFSaveSecondSorting_AKC", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PrintKey", PrintKey);
                cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@message", msg);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[2].Direction = ParameterDirection.Output;
                cmd.Parameters[2].Size = 500;
                cmd.CommandTimeout = 300;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                msg = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                if (msg == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 验证快递单号是否存在
        /// </summary>
        /// <param name="number"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public OrderInfo RFValidateExpressNumber_AKC(string number, long customerID)
        {
            string SqlStr = "	 SELECT * FROM dbo.WMS_Order WHERE CustomerID=" + customerID + " AND ExpressNumber='" + number + "'";

            return ExecuteDataTableBySqlString(SqlStr).ConvertToEntity<OrderInfo>();
        }

        /// <summary>
        /// 快递包装保存爱库存 rf
        /// </summary>
        /// <param name="PrintKey"></param>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool RFSaveExpressPackage_AKC(long customerID, string OrderNumber,string ExpressNumber, string username, out string msg)
        {
            msg = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_WMS_RFSaveExpressPackage_AKC", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);
                cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
                cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@ExpressNumber", ExpressNumber);
                cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@UserName", username);
                cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters.AddWithValue("@message", msg);
                cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                cmd.Parameters[4].Direction = ParameterDirection.Output;
                cmd.Parameters[4].Size = 500;
                cmd.CommandTimeout = 300;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                msg = sda.SelectCommand.Parameters["@message"].Value.ToString();
                conn.Close();
                if (msg == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
