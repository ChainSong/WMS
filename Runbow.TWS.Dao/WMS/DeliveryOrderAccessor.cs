using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.DeliveryConfirm;
using Runbow.TWS.MessageContracts.WebApi;
using Runbow.TWS.Common;
using System.Data.SqlClient;
using Runbow.TWS.Entity.DataBaseEntity;
using System.Configuration;
using Runbow.TWS.Entity.WMS.OnlineOrder;
using System.Collections;
using System.Reflection;

namespace Runbow.TWS.Dao.WMS
{
    public class DeliveryOrderAccessor : BaseAccessor
    {
        //测试库，发布后更改成正式库
        private string connStr = ConfigurationManager.ConnectionStrings["SampleTest"].ConnectionString.ToString();
        public DataTable ds = new DataTable();
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DeliveryOrderManagementResponse GetDeliveryOrderBindingDAL(DeliveryOrderManagementRequest request)
        {
            DeliveryOrderManagementResponse Responest = new DeliveryOrderManagementResponse();
            string Where = SqlWheres(request.SearchCondition);
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_SampleInventory", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Where", Where);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[0].Size = 8000;

                    cmd.Parameters.AddWithValue("@PageIndex", request.PageIndex);
                    cmd.Parameters[1].SqlDbType = SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@PageSize", request.PageSize);
                    cmd.Parameters[2].SqlDbType = SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@RowCount", 0);
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].SqlDbType = SqlDbType.Int;

                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    cmd.CommandTimeout = 180;
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                   
                    Responest.PageCount = (int)cmd.Parameters[3].Value;
                    Responest.PageIndex = request.PageIndex;
                    Responest.EnumerableDeliveryOrder = ds.ConvertToEntityCollection<DeliveryOrder>();
                    conn.Close();
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
            
            return Responest;
        }
        /// <summary>
        /// Where条件拼接
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private string SqlWheres(DeliveryOrderSearchCondition condition)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" 1=1");
            if (condition.UPC != null && condition.UPC != "")
            {
                sb.Append(" AND sk.sku in(" + condition.UPC.Trim() + ")");
            }
            if (condition.SKU != null && condition.SKU != "")
            {
                sb.Append(" AND LEFT(SKUCategory10,10) in(" + condition.SKU.Trim() + ")");
            }
            if (condition.PE != null && condition.PE != "")
            {
                sb.Append(" AND SKUCategory07 ='" + condition.PE + "'");
            }
            //if (condition.Gender != null && condition.Gender != "")
            //{
            //    sb.Append(" AND SKUCategory01 in(" + condition.Gender + ")");
            //}
            if (condition.ModelName != null && condition.ModelName != "")
            {
                sb.Append(" AND Description in(" + condition.ModelName.Trim() + ")");
            }
            if (condition.Size != null && condition.Size != "")
            {
                sb.Append(" AND SKUCategory09 in(" + condition.Size.Trim() + ")");
            }
            if (condition.Category != null && condition.Category != "")
            {
                sb.Append(" AND sk.SKUCategory03 ='" + condition.Category + "'");
            }
            //if (condition.Silhouette != null && condition.Silhouette != "")
            //{
            //    if (condition.Silhouette.IndexOf(',') > 0)
            //    {
            //        sb.Append(" AND (1=1");
            //        string[] str = condition.Silhouette.Split(',');
            //        for (int i = 0; i < str.Length; i++)
            //        {
            //            sb.Append(" OR sk.AllocationRule LIKE '%" + str[i].ToString() + "%'");
            //        }
            //        sb.Append(") ");
            //    }
            //    else
            //        sb.Append(" AND sk.AllocationRule LIKE '%" + condition.Silhouette + "%'");
            //}
            if (condition.Season != null && condition.Season != "")
            {
                if (condition.Season.IndexOf(',') > 0)
                {
                    sb.Append(" AND (1=1");
                    string[] str = condition.Season.Split(',');

                    for (int i = 0; i < str.Length; i++)
                    {
                        sb.Append(" OR RIGHT(SKUCategory10,6) LIKE '%" + str[i].ToString().Trim() + "%'");
                    }
                    sb.Append(") ");
                }
                else
                    sb.Append(" AND RIGHT(SKUCategory10,6) LIKE '%" + condition.Season.Trim() + "%'");

            }
            return sb.ToString();
        }
        /// <summary>
        /// Save创建半成品订单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string SaveOrderDAL(DeliveryOrderManagementRequest request)
        {
            DataTable dt = ModelToTable(request.DeliveryOrderinfo).DefaultView.ToTable(false, new string[] { "UPC", "Season", "RequireQTY" });
            string result = string.Empty;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    if (request.Operation == '1')
                    {
                        cmd.CommandText = "pro_wms_SampleCreateOrder";
                    }
                    else if (request.Operation == '2')
                    {
                        cmd.CommandText = "pro_wms_DeleteOrderDetail";
                    }
                    //SqlCommand cmd = new SqlCommand("pro_wms_SampleCreateOrder", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DeliveryOrder", dt);
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;

                    cmd.Parameters.AddWithValue("@UserName", request.UserName);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size = 50;

                    cmd.Parameters.AddWithValue("@Result", "");
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[2].Size = 8000;

                    conn.Open();
                    cmd.CommandTimeout = 180;
                    cmd.ExecuteNonQuery();
                    result = cmd.Parameters[2].Value.ToString();
                    conn.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return result;
        }
        /// <summary>
        /// Model转DateTable
        /// </summary>
        /// <param name="IEnumDelivery"></param>
        /// <returns></returns>
        public DataTable ModelToTable(IEnumerable<DeliveryOrder> IEnumDelivery)
        {
            var props = typeof(DeliveryOrder).GetProperties();
            var dt = new DataTable();

            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (IEnumDelivery.Count() > 0)
            {  
                for (int i = 0; i < IEnumDelivery.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(IEnumDelivery.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        public string CreateOrderSubmitDAL(ConsigneeSearchCondition consignee)
        {
            string result = string.Empty;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_CreateOrderConsignee", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", consignee.UserName);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[0].Size = 50;

                    cmd.Parameters.AddWithValue("@UserPhone", consignee.UserPhone);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size = 100;

                    cmd.Parameters.AddWithValue("@UserAddress", consignee.UserAddress);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Size = 100;

                    cmd.Parameters.AddWithValue("@UserEmail", consignee.UserEmail);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Size = 100;

                    cmd.Parameters.AddWithValue("@DeliveryDate", consignee.DeliveryDate);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[4].Size = 50;

                    cmd.Parameters.AddWithValue("@ReturnDate", consignee.ReturnDate);
                    cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[5].Size = 50;

                    cmd.Parameters.AddWithValue("@Result", "");
                    cmd.Parameters[6].Direction = ParameterDirection.Output;
                    cmd.Parameters[6].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[6].Size = 8000;

                    conn.Open();
                    cmd.CommandTimeout = 180;
                    cmd.ExecuteNonQuery();
                    result = cmd.Parameters[6].Value.ToString();
                    conn.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return result;
        }

        public DeliveryOrderManagementResponse GetApplicationRecordDAL(DeliveryOrderManagementRequest request)
        {
            DeliveryOrderManagementResponse Responest = new DeliveryOrderManagementResponse();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_UserInventory", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", request.UserName);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[0].Size = 100;

                    cmd.Parameters.AddWithValue("@PageIndex", request.PageIndex);
                    cmd.Parameters[1].SqlDbType = SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@PageSize", request.PageSize);
                    cmd.Parameters[2].SqlDbType = SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@RowCount", 0);
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].SqlDbType = SqlDbType.Int;


                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    cmd.CommandTimeout = 180;
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);

                    Responest.PageCount = (int)cmd.Parameters[3].Value;
                    Responest.PageIndex = request.PageIndex;
                    Responest.EnumerableDeliveryOrder = ds.ConvertToEntityCollection<DeliveryOrder>();
                    conn.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return Responest;
        }

        public DeliveryOrderManagementResponse GetReturnOrderDAL(DeliveryOrderManagementRequest request)
        {
            DeliveryOrderManagementResponse Responest = new DeliveryOrderManagementResponse();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_SampleReturnOrder", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", request.UserName);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[0].Size = 100;

                    cmd.Parameters.AddWithValue("@PageIndex", request.PageIndex);
                    cmd.Parameters[1].SqlDbType = SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@PageSize", request.PageSize);
                    cmd.Parameters[2].SqlDbType = SqlDbType.Int;

                    cmd.Parameters.AddWithValue("@RowCount", 0);
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].SqlDbType = SqlDbType.Int;


                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    cmd.CommandTimeout = 180;
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);

                    Responest.PageCount = (int)cmd.Parameters[3].Value;
                    Responest.PageIndex = request.PageIndex;
                    Responest.EnumerableDeliveryOrder = ds.ConvertToEntityCollection<DeliveryOrder>();
                    conn.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return Responest;
        }

        public DeliveryOrderManagementResponse GetOrderDetailDAL(string OrderKey)
        {
            DeliveryOrderManagementResponse Responest = new DeliveryOrderManagementResponse();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_SampleReturnOrderDetail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderKey", OrderKey);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[0].Size = 100;

                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    cmd.CommandTimeout = 180;
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);

                    Responest.EnumerableDeliveryOrder = ds.ConvertToEntityCollection<DeliveryOrder>();
                    conn.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return Responest;
        }

        public string GetReturnOrderDAL(string UserName)
        {
            string result = string.Empty;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("pro_wms_SampleReturnOrderUserName", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[0].Size = 100;

                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    cmd.CommandTimeout = 180;
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    if (ds.Rows.Count > 0)
                    {
                        result = ds.Rows[0][0].ToString();
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    result = ex.Message.ToString();
                }
            }

            return result;
        }
    }
}
