using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.DeliveryConfirm;
using Runbow.TWS.MessageContracts.WMS.DeliverConfirm;
using Runbow.TWS.Common;
using System.Data.SqlClient;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.Entity.WMS.Order;

namespace Runbow.TWS.Dao.WMS
{
    public class DeliverConfirmAccessor : BaseAccessor
    {

        /// <summary>
        /// 交接单主表查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public GetDeliverByConditionResponse GetDeliverHeaderByCondition(DeliverHeaderSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            GetDeliverByConditionResponse response = new GetDeliverByConditionResponse();
            string sqlWhere = this.GenGetDeliverHeaderWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetDeliverHeaderByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            response.DeliverHeaderConnection = ds.Tables[0].ConvertToEntityCollection<DeliverHeader>();
            return response;
        }

        /// <summary>
        /// 交接单列表查询条件
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        public string GenGetDeliverHeaderWhere(DeliverHeaderSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(SearchCondition.DeliverKey))
                sb.Append(" and d.DeliverKey='" + SearchCondition.DeliverKey + "'");
            if (!String.IsNullOrEmpty(SearchCondition.CustomerID.ToString()))
                sb.Append(" and d.CustomerID='" + SearchCondition.CustomerID + "'");
            if (!string.IsNullOrEmpty(SearchCondition.WarehouseName))
            {
                sb.Append(" AND d.WarehouseName in  (select WarehouseName from wms_warehouse where id in ( ").Append(SearchCondition.WarehouseName.Trim()).Append(")) ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// ID查询交接单明细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public GetDeliverByConditionResponse GetDeliverHeaderAndDetailByID(int ID)
        {
            GetDeliverByConditionResponse response = new GetDeliverByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int32, ID, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetDeliverHeaderAndDetailByID", dbParams);
            response.DeliverHeaderConnection = ds.Tables[0].ConvertToEntityCollection<DeliverHeader>();
            //response.DeliverDetailConnection = ds.Tables[1].ConvertToEntityCollection<DeliverDetail>();
            response.DeliverExpressNoConnection = ds.Tables[1].ConvertToEntityCollection<DeliverDetail>();
            return response;
        }

        /// <summary>
        /// 验证快递单
        /// </summary>
        /// <param name="express"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public string VilidateDeliverExpress(string express, long customerID, string warehouse)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_DeliverKey_Valdate", conn);//验证快递单号当前状态
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ExpressNo", express);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@Warehouse", warehouse);
                    cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@ReturnValue", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@ReturnValue"].Value.ToString();//返回
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
        /// ID查询交接单明细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public GetDeliverByConditionResponse GetDeliverUploadData(string express, long customerID, string warehouse)
        {
            GetDeliverByConditionResponse response = new GetDeliverByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Expresskey", DbType.String, express, ParameterDirection.Input),
                new DbParam("@CustomerID", DbType.String, customerID, ParameterDirection.Input),
                new DbParam("@Warehouse", DbType.String, warehouse, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetDeliverUploadData", dbParams);
            response.DeliverDetailConnection = ds.Tables[0].ConvertToEntityCollection<DeliverDetail>();
            return response;
        }

        /// <summary>
        /// 验证订单是否是取消单
        /// </summary>
        /// <param name="express"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public string ValidOrderCancel(string express, long customerID, string Proc, string warehouse, int type)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand(Proc, conn);//验证快递单号当前状态
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderNumber", express);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@Warehouse", warehouse);
                    cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters[3].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@ReturnValue", message);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[4].Direction = ParameterDirection.Output;
                    cmd.Parameters[4].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@ReturnValue"].Value.ToString();//返回
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
        /// 验证取消单，type：4 用于分配时验证取消单，传入多个订单id
        /// </summary>
        /// <param name="preoders"></param>
        /// <param name="Proc"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<PreOrder> ValidOrderCancel(IEnumerable<PreOrderSearchCondition> preoders, string Proc, int type)
        {
            string preids = "";
            preoders.Select(m => m.ID).Distinct().ToList().ForEach((item) => { preids += item + ","; });
            preids = preids.Substring(0, preids.Length - 1);

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand(Proc, conn);//验证快递单号当前状态
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderNumber", preids);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@CustomerID", "");
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@Warehouse", "");
                    cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters[3].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@ReturnValue", message);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[4].Direction = ParameterDirection.Output;
                    cmd.Parameters[4].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@ReturnValue"].Value.ToString();//返回
                    conn.Close();
                    return ds.Tables[0].ConvertToEntityCollection<PreOrder>();
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }


        /// <summary>
        /// 交接清单及明细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string AddDeliverAndDetail(AddDeliverAndDetailRequest request, int flag)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddDeliverAndDetail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Deliver", request.DeliverHeaderConnection.Select(p => new WMSDeliverToDb(p)));//表头
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@DeliverDetail", request.DeliverDetailConnection.Select(p => new WMSDeliverDetailToDb(p)));//明细
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Flag", flag);//增加一条和保存所有类型判断
                    cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@message", message);//返回
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();//传出交接单ID
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    return "";
                }

            }
        }

        /// <summary>
        /// 根据快递单号删除交接明细
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="DeliverKey"></param>
        /// <param name="ExpressNumber"></param>
        /// <returns></returns>
        public bool DeliverDetailDelete(long customerID, string DeliverKey, string ExpressNumber, string warehouse)
        {
            try
            {
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@CustomerID", DbType.String,customerID, ParameterDirection.Input),
                new DbParam("@DeliverKey", DbType.String,DeliverKey, ParameterDirection.Input),
                new DbParam("@ExpressNumber", DbType.String,ExpressNumber, ParameterDirection.Input),
                 new DbParam("@Warehouse", DbType.String,warehouse, ParameterDirection.Input)
                };
                base.ExecuteNoQuery("Proc_WMS_DeliveryDetailDelete", dbParams);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 打印交接清单
        /// </summary>
        /// <param name="deliverID"></param>
        /// <param name="Proc">存储过程可配置</param>
        /// <returns></returns>
        public GetDeliverByConditionResponse GetPrintDelivery(long deliverID, string Proc)
        {
            GetDeliverByConditionResponse response = new GetDeliverByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@DeliverID", DbType.String, deliverID, ParameterDirection.Input)//交接单主表ID               
            };
            DataSet ds = this.ExecuteDataSet(Proc, dbParams);
            response.DeliverHeaderConnection = ds.Tables[0].ConvertToEntityCollection<DeliverHeader>();
            response.DeliverDetailConnection = ds.Tables[1].ConvertToEntityCollection<DeliverDetail>();
            return response;
        }


        /// <summary>
        /// 交接单在提交出库时验证交接明细
        /// </summary>
        /// <param name="DeliverID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public string DeliverCompleteInfoValidate(long DeliverID, long customerID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_DeliverDetail_Validate", conn);//验证快递单号当前状态
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DeliverID", DeliverID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@returnOrder", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@returnOrder"].Value.ToString();//返回
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
        /// 交接单提交出库
        /// </summary>
        /// <param name="DeliverID"></param>
        /// <returns></returns>
        public string DeliverOut(long DeliverID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_DeliverSubmitOut", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DeliverID", DeliverID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@ReturnMessage", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@ReturnMessage"].Value.ToString();//返回错误信息
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
        /// 验证交接单里面的明细是否有前面称重的时候没拦住的取消单
        /// </summary>
        /// <param name="deliverID"></param>
        /// <returns></returns>
        public IEnumerable<OrderInfo> ValidDeliverOrderCancel(long deliverID)
        {
            GetDeliverByConditionResponse response = new GetDeliverByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@DeliverID", DbType.Int64, deliverID, ParameterDirection.Input)//交接单主表ID               
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_ValidDeliverOrderCancel", dbParams);
            return ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
        }

        /// <summary>
        /// 验证对应快递单是否为可出库状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<WMS_CheckExpress> CheckExpressStatus(long DeliverID, long customerID)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@DeliverID", DbType.String, DeliverID, ParameterDirection.Input),
                new DbParam("@customerID", DbType.String, customerID, ParameterDirection.Input),

            };
            DataTable dt = this.ExecuteDataTable("[Proc_WMS_CheckExpressStatus]", dbParams);
            return dt.ConvertToEntityCollection<WMS_CheckExpress>().ToList();
        }


    }
}
