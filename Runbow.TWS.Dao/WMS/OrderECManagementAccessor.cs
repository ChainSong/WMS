using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Common;
using System.Data.SqlClient;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.NikeOSRBJPrint;
using Runbow.TWS.Entity.WMS.NikeOSRBJPrint;
using Runbow.TWS.Entity.RabbitMQ;
using Runbow.TWS.MessageContracts.NikeNFSPrint;
using Runbow.TWS.Entity.WMS.NikeNFSPrint;
using Runbow.TWS.Entity.WMS.Order;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.MessageContracts.YXDBJRPrint;
using Runbow.TWS.Entity.WMS.YXDRBJPrint;
using System.Web.Mvc;
using Runbow.TWS.Entity.DataBaseEntity;

namespace Runbow.TWS.Dao
{
    public class OrderECManagementAccessor : BaseAccessor
    {
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
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
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
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    conn.Close();
                    return dt;
                }
                catch (Exception)
                {
                    return dt;
                }
            }
        }
        /// <summary>
        /// 获取要打印的波次拣货信息，1.单品单拣货单，2.多品单拣货单  3面单，4补打
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public PrintWaveModel GetPrintWaveOrder(string IDs, int Type, string ExpressNumber = "")
        {
            PrintWaveModel model = new PrintWaveModel();

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@IDs", DbType.String, IDs, ParameterDirection.Input),
                new DbParam("@Type", DbType.Int32, Type, ParameterDirection.Input),
                 new DbParam("@ExpressNumber", DbType.String, ExpressNumber, ParameterDirection.Input)
            };
            DataSet ds = new DataSet();
            ds = this.ExecuteDataSet("Proc_WMS_GetPrintWaveOrder", dbParams);
            if (Type == 1)
            {
                model.WaveHeaderLists = ds.Tables[0].ConvertToEntityCollection<WMS_Wave>();
                model.OrderDetailLists = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            }
            else if (Type == 2)
            {
                model.WaveHeaderLists = ds.Tables[0].ConvertToEntityCollection<WMS_Wave>();
                model.OrderDetailLists = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            }
            else if (Type == 3)
            {
                model.WaveHeaderLists = ds.Tables[0].ConvertToEntityCollection<WMS_Wave>();
                model.OrderLists = ds.Tables[1].ConvertToEntityCollection<OrderInfo>();
                //model.OrderLists = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                //model.OrderDetailLists = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            }
            else if (Type == 4)
            {
                model.OrderLists = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            }
            else if (Type == 5)
            {
                model.OrderLists = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            }
            return model;
        }
        public DataSet SaveExpressPackage(string ExpressNumber, string PackageType, long CustomerID, string WarehouseName, string PackageCode)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                DataSet dt = new DataSet();
                try
                {
                    string[] boxattr = PackageType.Split(',');

                    SqlCommand cmd = new SqlCommand("Proc_WMS_SaveExpressPackage", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExpressNumber", ExpressNumber);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@PackageType", PackageType);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@PackageCode", PackageCode);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[3].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@WarehouseName", WarehouseName);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Length", boxattr[0]);
                    cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Width", boxattr[1]);
                    cmd.Parameters[6].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Height", boxattr[2]);
                    cmd.Parameters[7].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[8].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[8].Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = 300;

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
        /// 获取中间表里面的订单
        /// </summary>
        /// <returns></returns>
        public OrderECModel GetNikeOrderB2C(int type)
        {

            OrderECModel model = new OrderECModel();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@type", DbType.String, "", ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetNikeOrderB2C", dbParams);

            model.Order_Headers = ds.Tables[0].ConvertToEntityCollection<Order_Header>();
            model.Order_Details = ds.Tables[1].ConvertToEntityCollection<Order_Detail>();
            return model;
        }

        /// <summary>
        /// 更新状态 isused用来标识是否抓取过
        /// </summary>
        /// <returns></returns>
        public bool UpdateNikeOrderStatus(List<string> Numbers)
        {
            string sql = "";
            StringBuilder numstr = new StringBuilder();
            foreach (var item in Numbers)
            {

                numstr.Append("'").Append(item).Append("',");
            }
            string numstr2 = numstr.ToString().TrimEnd(',');

            sql = "   UPDATE dbo.Order_Header SET IsUsed=1 WHERE orderCode IN(" + numstr2 + ") AND ISNULL(IsUsed,0)=0";

            return this.ScanExecuteNonQuery(sql) > 0;
        }

        public GetOrderByConditionResponse ExternOrderNumberCheck(List<OrderNumbers> list, long CustomerID)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_WMS_ExternOrderNumberCheck", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@order", list.Select(i => new ExternOrderNumberToDb(i)));
                cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                cmd.CommandTimeout = 300;
                conn.Open();

                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                response.OrderCollection = dt.ConvertToEntityCollection<OrderInfo>();
                conn.Close();
            }

            return response;
        }

        public string UpdateOrderExpressInfo(IEnumerable<OrderInfo> orders, long CustomerID)
        {
            string message = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_UpdateOrderExpressInfo", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@orders", orders.Select(i => new WMSOrderExpressInfoToDB(i)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Size = 50;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    conn.Open();

                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                message = "系统错误！";
            }

            return message;
        }

    }
}

