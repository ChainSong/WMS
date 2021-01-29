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
using Runbow.TWS.MessageContracts.WMS.Order;
using Runbow.TWS.Entity.DataBaseEntity;
using Runbow.TWS.MessageContracts.WebApi;
using Runbow.TWS.Entity.WebApi.Express;

namespace Runbow.TWS.Dao
{
    public class OrderManagementAccessor : BaseAccessor
    {
        public GetOrderByConditionResponse GetOrderByCondition(OrderSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();
            string sqlWhere = this.GenGetOrderWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetOrderByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            return response;
        }

        public GetOrderByConditionResponse GetOrderDetailByIDS(string IDS)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_GetOrderDetailByIDS", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDS", IDS.Split(',').Select(p => new IdsForInt32(Convert.ToInt32(p))));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.CommandTimeout = 300;
                    conn.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    response.OrderDetailCollection = ds.Tables[0].ConvertToEntityCollection<OrderDetailInfo>();
                    conn.Close();
                }
                catch (Exception ex)
                {

                }

            }

            return response;
        }

        public string OrderTask(string IDS, string Name)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_OrderTask", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDS", IDS.Split(',').Select(p => new IdsForInt32(Convert.ToInt32(p))));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 500;
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

        ///// <summary>
        ///// 检查差异
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public List<OrderDetailForRedisRF> CheckDiff(string id)
        //{

        //    DbParam[] dbParams = new DbParam[]{
        //        new DbParam("@id", DbType.String, id, ParameterDirection.Input),
        //    };
        //    DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetOrderDifferent]", dbParams);
        //    return dt.ConvertToEntityCollection<OrderDetailForRedisRF>().ToList();
        //}

        /// <summary>
        /// 检查差异根据包装表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<OrderDetailForRedisRF> CheckDiff(string id)
        {

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@id", DbType.String, id, ParameterDirection.Input),
            };
            DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetOrderDifferentByPackageDetail]", dbParams);
            return dt.ConvertToEntityCollection<OrderDetailForRedisRF>().ToList();
        }

        public List<OrderDetailForRedisRF> CheckDiffBatch(IEnumerable<OrderBackStatus> Orders)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                string Ids = string.Join(",", Orders.Select(a => a.ID));
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_ExportOrderDifferentByPackageDetail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Orderinfo", Orders.Select(p => new WMSOrderToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@ids", Ids);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return ds.Tables[0].ConvertToEntityCollection<OrderDetailForRedisRF>().ToList();
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }

        public GetOrderByConditionResponse GetOrderImportByCondition(OrderSearchCondition SearchCondition)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();
            string sqlWhere = this.GenGetOrderWhere(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetOrderImportByCondition", dbParams);
            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            return response;
        }

        public GetOrderByConditionResponse GetOrderByIDs(string IDs)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@IDs", DbType.String, IDs, ParameterDirection.Input)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetOrderByIDs", dbParams);
            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            return response;
        }

        //出库单界面加载数据缓慢，不需要加载明细数据
        public GetOrderByConditionResponse GetOrderHeaderByCondition(OrderSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();
            string sqlWhere = this.GenGetOrderWhere(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetOrderHeaderByCondition", dbParams);
            rowCount = (int)dbParams[3].Value;
            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            //response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            return response;
        }

        /// <summary>
        /// 订单状态统计查询
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public GetOrderByConditionResponse GetOrderStatusByCondition(OrderSearchCondition SearchCondition)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();
            string sqlWhere = this.GenGetOrderStatusWhere(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetOrderStatusByCondition", dbParams);

            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            //response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            return response;
        }

        /// <summary>
        /// 查询此状态下的所有订单
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public GetOrderByConditionResponse SearchOrderTotal(OrderSearchCondition SearchCondition, int type)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();
            string sqlWhere = this.GenGetOrderStatusTotal(SearchCondition);
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@Type",DbType.Int32,type,ParameterDirection.Input),
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetOrderStatusTotalByCondition", dbParams);

            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            //response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            return response;
        }

        public GetOrderByConditionResponse GetOrderByCondition_Wave(OrderSearchCondition SearchCondition, int pageIndex, int pageSize, out int rowCount)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();
            string sqlWhere = this.GenGetOrderWhere_Wave(SearchCondition);
            string Having = this.GenGetOrderWhere_Wave_Having(SearchCondition);
            int tempRowCount = 0;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input),
                new DbParam("@Having", DbType.String, Having, ParameterDirection.Input),
                new DbParam("@PageIndex", DbType.Int32, pageIndex, ParameterDirection.Input),
                new DbParam("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input),
                new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetOrderByCondition_Wave", dbParams);
            rowCount = (int)dbParams[4].Value;
            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            //response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            return response;
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
            response.OrderDetailCollection = ds.Tables[3].ConvertToEntityCollection<OrderDetailInfo>();
            return response;
        }

        //箱唛打印(GZ+OSR) 20161227
        public BoxListManagementResponse GetPackageBoxCarton(string ID, string Type)
        {
            BoxListManagementResponse response = new BoxListManagementResponse();
            string IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, IDs, ParameterDirection.Input),
                new DbParam("@Type", DbType.String, Type, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_PackageBoxCarton", dbParams);
            response.EnumerableBoxListinfo = ds.Tables[0].ConvertToEntityCollection<PrintBoxInfo>();
            return response;
        }

        //吉特面单打印
        public BoxListManagementResponse GetPackageBoxCartonJite(string ID, string Type)
        {
            BoxListManagementResponse response = new BoxListManagementResponse();
            string IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@PackageNumber", DbType.String, IDs, ParameterDirection.Input),
                new DbParam("@Type", DbType.String, Type, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_PackageBoxCartonJite", dbParams);
            response.EnumerableExpressListInfo = ds.Tables[0].ConvertToEntityCollection<PrintExpressJite>();
            return response;
        }

        public IEnumerable<PackageInfo> GetPackageInfos(string id, string type)
        {
            string sql = string.Empty;
            if (type == "A")//批量
            {
                sql = string.Format(@"SELECT * FROM dbo.WMS_Package WHERE  ExternOrderNumber IN (SELECT ExternOrderNumber FROM dbo.WMS_Package 
                                            WHERE PackageNumber='{0}')
                                            AND ISNULL(ExpressCompany,'')='' AND ISNULL(ExpressNumber,'')=''", id);
            }
            else//单条
            {
                sql = string.Format(@"SELECT * FROM dbo.WMS_Package WHERE PackageNumber='{0}' AND ISNULL(ExpressCompany,'')='' AND ISNULL(ExpressNumber,'')=''", id);
            }
            DataTable dt = base.ExecuteDataTableBySqlString(sql);
            return dt.ConvertToEntityCollection<PackageInfo>();
        }

        //箱唛打印(GZ+OSR) 20161227
        public BoxListManagementResponse GetPackageBoxCarton(string ID, string Type, string Proc)
        {
            BoxListManagementResponse response = new BoxListManagementResponse();
            string IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, IDs, ParameterDirection.Input),
                new DbParam("@Type", DbType.String, Type, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet(Proc, dbParams);
            response.EnumerableBoxListinfo = ds.Tables[0].ConvertToEntityCollection<PrintBoxInfo>();
            return response;
        }

        //箱唛打印箱清单 20170111
        public NFSBoxListManagementResponse GetNFSPackageBoxCarton(string ID, string Type)
        {
            NFSBoxListManagementResponse response = new NFSBoxListManagementResponse();
            string IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, IDs, ParameterDirection.Input),
                new DbParam("@Type", DbType.String, Type, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Wms_NFSPrintBoxm", dbParams);
            response.EnumerableBoxListinfo = ds.Tables[0].ConvertToEntityCollection<NFSPrintBoxInfo>();
            return response;
        }

        /// <summary>
        /// 打印报关箱唛
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Type"></param>
        /// <param name="Proc"></param>
        /// <returns></returns>
        public BoxListManagementResponse GetPackageCustomerCarton(string ID, string Type)
        {
            BoxListManagementResponse response = new BoxListManagementResponse();
            string IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, IDs, ParameterDirection.Input),
                new DbParam("@Type", DbType.String, Type, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_PackageCustomerCarton_YXDR", dbParams);
            response.EnumerableBoxListinfo = ds.Tables[0].ConvertToEntityCollection<PrintBoxInfo>();
            return response;
        }

        public GetOrderByConditionResponse GetPackageByID(long ID)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();
            long IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@IDs", DbType.Int64, IDs, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPackageByPID", dbParams);
            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            response.packages = ds.Tables[1].ConvertToEntityCollection<PackageInfo>();
            response.packageDetails = ds.Tables[2].ConvertToEntityCollection<PackageDetailInfo>();
            return response;
        }

        public BoxListManagementResponse GetPrintBoxListCondition(string ID, string Type, string Proc)
        {
            BoxListManagementResponse response = new BoxListManagementResponse();
            string IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, IDs, ParameterDirection.Input),
                new DbParam("@Type", DbType.String, Type, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet(Proc, dbParams);//存储过程名改成可配置
            response.EnumerableBoxListinfo = ds.Tables[0].ConvertToEntityCollection<PrintBoxInfo>();
            return response;
        }

        /// <summary>
        /// 获取打印面单信息
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Type"></param>
        /// <param name="Proc"></param>
        /// <returns></returns>
        public IEnumerable<OrderInfo> GetPrintExpressListCondition(string ID, string Proc, int type)
        {
            string IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@IDs", DbType.String, IDs, ParameterDirection.Input),
                new DbParam("@type", DbType.Int32, type, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet(Proc, dbParams);//存储过程名改成可配置
            return ds.Tables[0].ConvertToEntityCollection<OrderInfo>();

        }

        /// <summary>
        /// NIKE打印面单
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Proc"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<OrderInfo> GetPrintExpressNike(string ID)
        {
            string IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@IDs", DbType.String, IDs, ParameterDirection.Input) };
            DataSet ds = this.ExecuteDataSet("Proc_PrintExpressNike", dbParams);
            return ds.Tables[0].ConvertToEntityCollection<OrderInfo>();

        }


        public IEnumerable<PrintExpressJite> GetPrintExpressDeppon(string ID)
        {
            string sql = string.Format(@"SELECT a.OrderNumber,a.ExternOrderNumber,b.PackageNumber,b.arrivedOrgSimpleName,a.Consignee,a.Contact,a.Province,a.City,a.District,a.Address,
            c.Contractor AS senderContractor,c.Mobile AS senderMobile,c.ProvinceCity AS senderProvinceCity,c.Address AS senderAddress,
            b.ExpressNumber,b.parentMailNo
            FROM dbo.WMS_Order a
            RIGHT JOIN dbo.WMS_ExpressDelivery b ON b.OrderNumber=a.OrderNumber AND a.CustomerID=b.CustomerID
            LEFT JOIN dbo.WMS_Warehouse c ON c.WarehouseName=a.Warehouse
            WHERE b.CustomerID=108 and b.PackageNumber in ({0})", ID);
            DataTable dt = this.ExecuteDataTableBySqlString(sql);
            return dt.ConvertToEntityCollection<PrintExpressJite>();

        }

        public IEnumerable<PrintExpressYd> GetPrintExpressYd(string ID)
        {
            string sql = string.Format(@"SELECT o.OrderNumber,o.ExternOrderNumber,o.CustomerID,o.ExpressNumber,ed.PackageNumber,w.ID AS WarehouseID,w.WarehouseName,
                                            o.Consignee AS ReceiverName,o.Contact AS ReceiverMobile,o.Address AS ReceiverAddress,
                                            w.Contractor AS SenderName,w.Mobile AS SenderMobile,w.Address AS SenderAddress,
                                            ed.position,ed.position_no,ed.mailNo,ed.four_code,ed.package_wdjc,ed.cus_area1
                                            FROM dbo.WMS_Order o 
                                            RIGHT JOIN dbo.WMS_ExpressDelivery ed ON o.ExternOrderNumber= ed.ExternOrderNumber AND o.CustomerID = ed.CustomerID
                                            LEFT JOIN dbo.WMS_Warehouse w ON o.Warehouse = w.WarehouseName
                                            WHERE ed.PackageNumber in ({0})", ID);
            DataTable dt = base.ExecuteDataTableBySqlString(sql);
            return dt.ConvertToEntityCollection<PrintExpressYd>();
        }

        /// <summary>
        /// 打印总箱单
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public BoxListManagementResponse GetPrintTotalBoxListCondition(string ID, string Type)
        {
            BoxListManagementResponse response = new BoxListManagementResponse();
            string IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, IDs, ParameterDirection.Input),
                new DbParam("@Type", DbType.String, Type, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_PackageTotalBoxList_YXDR", dbParams);
            response.EnumerableBoxListinfo = ds.Tables[0].ConvertToEntityCollection<PrintBoxInfo>();
            return response;
        }

        public DataSet ExportBoxDetails(string OrderNumber)
        {
            BoxListManagementResponse response = new BoxListManagementResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, OrderNumber, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("WMS_ExportBoxDetails", dbParams);
            return ds;
        }

        /// <summary>
        /// 退货仓  批量导出箱清单
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns></returns>
        public DataSet ExportBoxDetails_TH(string OrderNumber)
        {
            BoxListManagementResponse response = new BoxListManagementResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, OrderNumber, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("WMS_ExportBoxDetails_TH", dbParams);
            return ds;
        }

        /// <summary>
        /// YXDR包装箱清单导出查询
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns></returns>
        public DataSet ExportBoxDetailsYXDR(string OrderNumber)
        {
            BoxListManagementResponse response = new BoxListManagementResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, OrderNumber, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("WMS_ExportBoxDetails_YXDR", dbParams);
            return ds;
        }

        public ConsignmentManagementResponse GetPrintPodCondition(string ID, string Type)
        {
            ConsignmentManagementResponse response = new ConsignmentManagementResponse();
            string IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, IDs, ParameterDirection.Input),
                new DbParam("@Type", DbType.String, Type, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_PackagePod", dbParams);
            response.EnumerableBoxListinfo = ds.Tables[0].ConvertToEntityCollection<PrintPodInfo>();
            return response;
        }

        /// <summary>
        /// 永兴东润单个POD
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public YXDRBJPODManagementResponse GetYXDRPrintPodCondition(string ID, string Type)
        {
            YXDRBJPODManagementResponse response = new YXDRBJPODManagementResponse();
            string IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, IDs, ParameterDirection.Input),
                new DbParam("@Type", DbType.String, Type, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_PackagePod_YXDR", dbParams);
            response.EnumerableYXDRPodInfo = ds.Tables[0].ConvertToEntityCollection<YXDRBJPrintPodInfo>();
            return response;
        }

        /// <summary>
        /// 永兴东润批量POD
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public YXDRBJPODManagementResponse GetYXDRPrintAllPodCondition(string IDs)
        {
            YXDRBJPODManagementResponse response = new YXDRBJPODManagementResponse();

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, IDs, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_PackageAllPod_YXDR", dbParams);
            response.EnumerableYXDRPodInfo = ds.Tables[0].ConvertToEntityCollection<YXDRBJPrintPodInfo>();
            return response;
        }

        public ConsignmentManagementResponse GetPrintAllPodCondition(string IDs)
        {
            ConsignmentManagementResponse response = new ConsignmentManagementResponse();

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, IDs, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_PackageAllPod", dbParams);
            response.EnumerableBoxListinfo = ds.Tables[0].ConvertToEntityCollection<PrintPodInfo>();
            response.PrintPodDetails = ds.Tables[1].ConvertToEntityCollection<PrintPodInfo>();
            return response;
        }

        /// <summary>
        /// 汇总查询托运单
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="customerID"></param>
        /// <param name="warehouseName"></param>
        /// <returns></returns>
        public ConsignmentManagementResponse GetPrintSumAllPodCondition(string IDs, long customerID, string warehouseName, string searchTime)
        {
            ConsignmentManagementResponse response = new ConsignmentManagementResponse();
            DbParam[] dbparams = new DbParam[] {
                new DbParam("@OrderNumber",DbType.String,IDs,ParameterDirection.Input),
                new DbParam("@CustomerID",DbType.Int64,customerID,ParameterDirection.Input),
                new DbParam("@WarehouseName",DbType.String,warehouseName,ParameterDirection.Input),
                 new DbParam("@SearchTime",DbType.String,searchTime,ParameterDirection.Input),
            };
            DataSet ds = this.ExecuteDataSet("Proc_PackageSumAllPod", dbparams);
            response.EnumerableBoxListinfo = ds.Tables[0].ConvertToEntityCollection<PrintPodInfo>();
            return response;
        }

        /// <summary>
        /// 退货仓托运单打印（包装界面）
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public ConsignmentManagementResponse GetPrintPodCondition_TH(string ID, string Type)
        {
            ConsignmentManagementResponse response = new ConsignmentManagementResponse();
            string IDs = ID;
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, IDs, ParameterDirection.Input),
                new DbParam("@Type", DbType.String, Type, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_PackagePod_TH", dbParams);
            response.EnumerableBoxListinfo = ds.Tables[0].ConvertToEntityCollection<PrintPodInfo>();
            return response;
        }

        /// <summary>
        /// 退货仓批量打印托运单（出库界面）
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public ConsignmentManagementResponse GetPrintAllPodCondition_TH(string IDs)
        {
            ConsignmentManagementResponse response = new ConsignmentManagementResponse();

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@OrderNumber", DbType.String, IDs, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_PackageAllPod_TH", dbParams);
            response.EnumerableBoxListinfo = ds.Tables[0].ConvertToEntityCollection<PrintPodInfo>();
            return response;
        }

        /// <summary>
        /// 退货仓批量汇总打印托运单
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="customerID"></param>
        /// <param name="warehouseName"></param>
        /// <returns></returns>
        public ConsignmentManagementResponse GetPrintSumAllPodCondition_TH(string IDs, long customerID, string warehouseName, string searchTime)
        {
            ConsignmentManagementResponse response = new ConsignmentManagementResponse();
            DbParam[] dbparams = new DbParam[] {
                new DbParam("@OrderNumber",DbType.String,IDs,ParameterDirection.Input),
                new DbParam("@CustomerID",DbType.Int64,customerID,ParameterDirection.Input),
                new DbParam("@WarehouseName",DbType.String,warehouseName,ParameterDirection.Input),
                 new DbParam("@SearchTime",DbType.String,searchTime,ParameterDirection.Input),
            };
            DataSet ds = this.ExecuteDataSet("Proc_PackageSumAllPod_TH", dbparams);
            response.EnumerableBoxListinfo = ds.Tables[0].ConvertToEntityCollection<PrintPodInfo>();
            return response;
        }

        public GetOrderAndOrderDetailByConditionResponse GetOrderAndOrderDetailByCondition(long ID)
        {
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64,ID, ParameterDirection.Input)

            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetOrderOrOrderDetailByCondition", dbParams);
            response.order = ds.Tables[0].ConvertToEntity<OrderInfo>();
            response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            return response;

        }

        public GetOrderAndOrderDetailByConditionResponse GetSkuListByCondition(long ID)
        {
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID", DbType.Int64,ID, ParameterDirection.Input)

            };

            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetSkuListByCondition", dbParams);
            response.OrderDetailCollection = ds.Tables[0].ConvertToEntityCollection<OrderDetailInfo>();
            return response;

        }

        public IEnumerable<OrderInfo> GetProvinceList()
        {
            IEnumerable<OrderInfo> list = new List<OrderInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {

                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetProvinceList", param);
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

        public IEnumerable<OrderInfo> GetCityList(string province)
        {
            IEnumerable<OrderInfo> list = new List<OrderInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                       new DbParam("@ProvinceName", DbType.String, province, ParameterDirection.Input),
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetCityList", param);
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

        public IEnumerable<OrderInfo> GetDistrictList(string city)
        {
            IEnumerable<OrderInfo> list = new List<OrderInfo>();   //库区列表
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {

                    DbParam[] param = new DbParam[] {
                       new DbParam("@CityName", DbType.String, city, ParameterDirection.Input),
                    };
                    DataSet ds = ExecuteDataSet("Proc_WMS_GetDistrictList", param);
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

        public string Pick(IEnumerable<OrderBackStatus> Orders, string type, string PickerOrConfirmer)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_PickAndConfirm", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Orderinfo", Orders.Select(p => new WMSOrderToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters[1].Size = 50;
                    cmd.Parameters.AddWithValue("@PickerOrConfirmer", PickerOrConfirmer);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Size = 50;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    //message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }

            }
        }

        public string Handover(IEnumerable<OrderBackStatus> Orders, string Handoveror)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_Handover", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Orderinfo", Orders.Select(p => new WMSOrderToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Handoveror", Handoveror);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size = 50;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    //message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return message;
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
                }

            }
        }

        public string Outs(IEnumerable<OrderBackStatus> Orders)
        {
            //using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            //{
            //    string message = "";
            //    try
            //    {
            //        SqlCommand cmd = new SqlCommand("Proc_WMS_Out", conn);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@Orderinfo", Orders.Select(p => new WMSOrderToDb(p)));
            //        cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
            //        cmd.Parameters.AddWithValue("@message", message);
            //        cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
            //        cmd.Parameters[1].Direction = ParameterDirection.Output;
            //        cmd.Parameters[1].Size = 500;
            //        cmd.CommandTimeout = 300;
            //        conn.Open();

            //        DataSet ds = new DataSet();
            //        SqlDataAdapter sda = new SqlDataAdapter();
            //        sda.SelectCommand = cmd;
            //        sda.Fill(ds);
            //        message = sda.SelectCommand.Parameters["@message"].Value.ToString();
            //        conn.Close();
            //        return message;
            //    }
            //    catch (Exception ex)
            //    {
            //        return message + "(" + ex.Message + ")";
            //    }

            //}
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                string Ids = string.Join(",", Orders.Select(a => a.ID));
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_Out", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Orderinfo", Orders.Select(p => new WMSOrderToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@ids", Ids);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 500;
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
        /// 差异出库
        /// </summary>
        /// <param name="Orders"></param>
        /// <returns></returns>
        public string OutsWithDiff(IEnumerable<OrderBackStatus> Orders)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                string Ids = string.Join(",", Orders.Select(a => a.ID));
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_OutWithDiff", conn);//Proc_WMS_OutWithDiff
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Orderinfo", Orders.Select(p => new WMSOrderToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@ids", Ids);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 500;
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

        public string ChangeExpress(string IDs, string ExpressCompany)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_ChangeExpress", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDs", IDs);
                    cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@ExpressCompany", ExpressCompany);
                    cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 500;
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

        public string AllocatedWave(string IDS, string WaveNumber)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AllocatedWave", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDS", IDS);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[0].Size = 500;
                    cmd.Parameters.AddWithValue("@WaveNumber", WaveNumber);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Size = 50;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.Parameters[2].Size = 500;
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

        public string UnionOrder(IEnumerable<OrderBackStatus> Orders)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_UnionOrder", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Orderinfo", Orders.Select(p => new WMSOrderToDb(p)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 500;
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

        public IEnumerable<InstructionInfo> AddInstructions(string IDs, string WorkStation, string ReleatedType, int Priority, string UserName)
        {
            //Request.Ids.Split(',').Select(c => new )IdsForInt64(c.ObjectToInt64()));
            string message = "";
            IEnumerable<InstructionInfo> info = null;

            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddInstructions", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ids", IDs.Split(',').Select(c => new IdsForInt64(c.ObjectToInt64())));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@WorkStation", WorkStation);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@ReleatedType", ReleatedType);
                    cmd.Parameters[2].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@Priority", Priority);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters[4].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[5].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[5].Direction = ParameterDirection.Output;
                    cmd.Parameters[5].Size = 500;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    info = dt.ConvertToEntityCollection<InstructionInfo>();
                    // cmd.Parameters.AddWithValue("@Pod", Condition.Select(i => new NikeforBSPODToDb(i)));
                    return info;
                }
                catch (Exception e)
                {
                    return info;

                }

            }
        }

        public bool UpdateResults(string IDs, string UserName)
        {
            //Request.Ids.Split(',').Select(c => new )IdsForInt64(c.ObjectToInt64()));
            int message = 0;
            IEnumerable<InstructionInfo> info = null;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("Proc_UpddateOrder_Robot", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ids", IDs.Split(',').Select(c => new IdsForInt64(c.ObjectToInt64())));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[2].SqlDbType = SqlDbType.Int;
                    cmd.Parameters[2].Direction = ParameterDirection.Output;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    message = Convert.ToInt32(sda.SelectCommand.Parameters["@message"].Value);
                    conn.Close();
                    if (message > 0)
                    {
                        return true;
                    }
                    //info = dt.ConvertToEntityCollection<InstructionInfo>();
                    // cmd.Parameters.AddWithValue("@Pod", Condition.Select(i => new NikeforBSPODToDb(i)));
                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }

            }
        }

        public string UpdatePrintStatus(string IDs)
        {
            //Request.Ids.Split(',').Select(c => new )IdsForInt64(c.ObjectToInt64()));
            string message = "";
            IEnumerable<InstructionInfo> info = null;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("Proc_UpdatePrintStatus", conn);
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

        public string ChangeExpressByOrderNumber(List<OrderInfo> OrderList)
        {
            //Request.Ids.Split(',').Select(c => new )IdsForInt64(c.ObjectToInt64()));
            string message = "";
            IEnumerable<InstructionInfo> info = null;
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Pro_WMS_ChangeExpressCompany", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@WMS_OrderNumber", OrderList.Select(order => new WMSOrderExpressToDB(order)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[1].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[1].Direction = ParameterDirection.Output;
                    cmd.Parameters[1].Size = 500;
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
                catch (Exception e)
                {
                    return "系统错误！";
                }

            }
        }

        public string GetMaxBoxnumber(string OrderID)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_ReturnPackageKey", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                        return dt.Rows[0][0].ToString();
                    return "";
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }

        public string GetMaxBoxnumber(string OrderID, string ProcName)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand(ProcName, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderID", OrderID);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                        return dt.Rows[0][0].ToString();
                    return "";
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }

        public string GetQueryDetail(string p, string querykey)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand(p, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Querykey", querykey);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                        return dt.Rows[0][0].ToString();
                    return "";
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }

        public bool DeletePackInfo(string PackageKey)
        {
            try
            {
                DbParam[] dbParams = new DbParam[]{
                new DbParam("@PackageKey", DbType.String,PackageKey, ParameterDirection.Input)
                };
                base.ExecuteNoQuery("Proc_WMS_DeletePackageKey", dbParams);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string OrderBackStatus(GetOrderByConditionRequest request, int ToStatus, int type)
        {

            string CustomerId = "";
            try
            {
                string sql = @"SELECT top 1    status,   customerid,   warehouse  FROM dbo.wms_order  WHERE  id =   " + request.Orders.FirstOrDefault().ID;
                var receiptreceiving = this.ExecuteDataTableBySqlString(sql).ConvertToEntity<OrderInfo>();
                if (receiptreceiving == null)
                {
                    return "失败";
                }

                CustomerId = receiptreceiving.CustomerID.ToString();


            }
            catch (Exception e)
            {
                return "失败,请检查上架单";
            }
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = "";
                try
                {
                    if (request.Orders.Count() == 1)
                    {
                        SqlCommand cmd = new SqlCommand("Proc_WMS_OrderStatusBack_" + CustomerId, conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", request.Orders.Select(m => m.ID).FirstOrDefault().ObjectToInt64());
                        cmd.Parameters[0].SqlDbType = SqlDbType.BigInt;
                        cmd.Parameters.AddWithValue("@ToStatus", ToStatus);
                        cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters[2].SqlDbType = SqlDbType.Int;
                        cmd.Parameters.AddWithValue("@message", message);
                        cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                        cmd.Parameters[3].Direction = ParameterDirection.Output;
                        cmd.Parameters[3].Size = 500;
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
                    else
                    {
                        //这写的是什么逻辑哦？
                        //foreach (var item in request.Orders)
                        //{
                        //    SqlCommand cmd = new SqlCommand("Proc_WMS_OrderStatusBack_" + CustomerId, conn);//Proc_WMS_OrderStatusBack_Batch
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    cmd.Parameters.AddWithValue("@UdtOrder", request.Orders.Where(a => a.ID == item.ID).Select(order => new WMSOrderToDb(order)));
                        //    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                        //    cmd.Parameters.AddWithValue("@ToStatus", ToStatus);
                        //    cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;

                        //    cmd.Parameters.AddWithValue("@type", type);
                        //    cmd.Parameters[2].SqlDbType = SqlDbType.Int;

                        //    cmd.Parameters.AddWithValue("@message", message);
                        //    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                        //    cmd.Parameters[3].Direction = ParameterDirection.Output;
                        //    cmd.Parameters[3].Size = 500;
                        //    conn.Open();
                        //    DataSet ds = new DataSet();
                        //    SqlDataAdapter sda = new SqlDataAdapter();
                        //    sda.SelectCommand = cmd;
                        //    sda.Fill(ds);
                        //    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                        //    conn.Close();
                        //}

                        SqlCommand cmd = new SqlCommand("Proc_WMS_OrderStatusBack_Batch_" + CustomerId, conn);//Proc_WMS_OrderStatusBack_Batch
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UdtOrder", request.Orders.Select(order => new WMSOrderToDb(order)));
                        cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                        cmd.Parameters.AddWithValue("@ToStatus", ToStatus);
                        cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;

                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters[2].SqlDbType = SqlDbType.Int;

                        cmd.Parameters.AddWithValue("@message", message);
                        cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                        cmd.Parameters[3].Direction = ParameterDirection.Output;
                        cmd.Parameters[3].Size = 500;
                        conn.Open();
                        DataSet ds = new DataSet();
                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = cmd;
                        sda.Fill(ds);
                        message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                        conn.Close();

                        return message;
                    }
                }
                catch (Exception ex)
                {
                    return message + "(" + ex.Message + ")";
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

        private string GenGetOrderWhere(OrderSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SearchCondition.OrderNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.OrderNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.OrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.OrderNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.OrderNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and o.OrderNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and o.OrderNumber  like '%" + SearchCondition.OrderNumber.Trim() + "%' ");
                }
                //sb.Append(" AND a.ReceiptNumber ='").Append(SearchCondition.ReceiptNumber).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExternOrderNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.ExternOrderNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.ExternOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.ExternOrderNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.ExternOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and o.ExternOrderNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and o.ExternOrderNumber  like '%" + SearchCondition.ExternOrderNumber.Trim() + "%' ");
                }
                //sb.Append(" AND a.ExternReceiptNumber='").Append(SearchCondition.ExternReceiptNumber).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.WaveNumber))
            {
                sb.Append(" and o.WaveNumber  like '%" + SearchCondition.WaveNumber.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str19))
            {
                if (SearchCondition.str19 == "1")
                {
                    sb.Append(" and o.str19  = '1' ");
                }
                else
                {
                    sb.Append(" and ISNULL(o.str19,'0')='0'");
                }
            }
            if (!string.IsNullOrEmpty(SearchCondition.PreOrderNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.PreOrderNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.PreOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.PreOrderNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.PreOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and o.PreOrderNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and o.PreOrderNumber  like '%" + SearchCondition.PreOrderNumber.Trim() + "%' ");
                }
                //sb.Append(" AND a.ExternReceiptNumber='").Append(SearchCondition.ExternReceiptNumber).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExpressNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.ExpressNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.ExpressNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.ExpressNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.ExpressNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and o.ExpressNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and o.ExpressNumber  like '%" + SearchCondition.ExpressNumber.Trim() + "%' ");
                }
                //sb.Append(" AND a.ExternReceiptNumber='").Append(SearchCondition.ExternReceiptNumber).Append("' ");
            }
            if (SearchCondition.CustomerID != 0)
            {
                sb.Append(" AND o.CustomerID=").Append(SearchCondition.CustomerID).Append(" ");
            }
            if (SearchCondition.CustomerID == 0 && SearchCondition.CustomerIDs != null)
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                numbers = SearchCondition.CustomerIDs.Split(',').Select(s => { return s.Trim(); });
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                sb.Append(" and o.CustomerID in ( ");
                foreach (string s in numbers)
                {
                    sb.Append("'").Append(s).Append("',");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" ) ");
            }
            if (SearchCondition.ID != 0)
            {
                sb.Append(" AND o.ID=").Append(SearchCondition.ID).Append(" ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND o.Warehouse in  (select WarehouseName from wms_warehouse where id in ( ").Append(SearchCondition.Warehouse.Trim()).Append(")) ");
                //sb.Append(" AND o.Warehouse='").Append(SearchCondition.Warehouse).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.OrderType))
            {
                sb.Append(" AND o.OrderType='").Append(SearchCondition.OrderType).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExpressCompany))
            {
                sb.Append(" AND o.ExpressCompany='").Append(SearchCondition.ExpressCompany).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Province))
            {
                sb.Append(" and o.Province  like '%" + SearchCondition.Province.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.City))
            {
                sb.Append(" and o.City  like '%" + SearchCondition.City.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.District))
            {
                sb.Append(" and o.District  like '%" + SearchCondition.District.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Consignee))
            {
                sb.Append(" and o.Consignee  like '%" + SearchCondition.Consignee.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Contact))
            {
                sb.Append(" and o.Contact  like '%" + SearchCondition.Contact.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Address))
            {
                sb.Append(" and o.Address  like '%" + SearchCondition.Address.Trim() + "%' ");
            }
            if (SearchCondition.IsMerged != null)
            {
                sb.Append(" AND o.IsMerged=").Append(SearchCondition.IsMerged).Append(" ");
            }
            if (SearchCondition.Status != null)
            {
                sb.Append(" AND o.Status='").Append(SearchCondition.Status).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.IsPrintPick))
            {
                if (SearchCondition.IsPrintOther == "是")
                {
                    sb.Append(" and o.PickPrintCount  >0");
                }
                else
                {
                    sb.Append(" and o.PickPrintCount  =0");
                }
            }
            if (!string.IsNullOrEmpty(SearchCondition.IsPrintExpress))
            {
                if (SearchCondition.IsPrintOther == "是")
                {
                    sb.Append(" and o.ExpressPrintCount  >0");
                }
                else
                {
                    sb.Append(" and o.ExpressPrintCount  =0");
                }
            }
            if (!string.IsNullOrEmpty(SearchCondition.IsPrintOther))
            {
                if (SearchCondition.IsPrintOther == "是")
                {
                    sb.Append(" and o.OtherPrintCount  >0");
                }
                else
                {
                    sb.Append(" and o.OtherPrintCount  =0");
                }
            }
            if (!string.IsNullOrEmpty(SearchCondition.IsGetExpressNumber))
            {
                sb.Append(" and isnull(o.ExpressStatus,0)='").Append(SearchCondition.IsGetExpressNumber).Append("' ");
                //if (SearchCondition.IsGetExpressNumber == "已获取")
                //{
                //    sb.Append(" and o.ExpressStatus  =1");
                //}
                //else if (SearchCondition.IsGetExpressNumber == "未获取")
                //{
                //    sb.Append(" and o.ExpressStatus  =2");
                //}
                //else
                //{
                //    sb.Append(" and o.ExpressStatus  =3");
                //}
            }
            if (!string.IsNullOrEmpty(SearchCondition.IsSkuOne))
            {
                if (SearchCondition.IsSkuOne == "单SKU")
                {
                    sb.Append(" and o.DetailCount  <=1");
                }
                else
                {
                    sb.Append(" and o.DetailCount  >1");
                }
            }
            //if (SearchCondition.StartCreateTime != null)
            //{
            //    sb.Append(" AND o.CreateTime >='").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            //}
            //if (SearchCondition.EndCreateTime != null)
            //{
            //    sb.Append(" AND o.CreateTime <='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            //}
            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append(" AND o.CreateTime >='").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd HH:mm:ss.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" AND o.CreateTime <='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd HH:mm:59.99")).Append("' ");
            }
            if (SearchCondition.StartOrderTime != null)
            {
                sb.Append(" AND o.OrderTime >='").Append(SearchCondition.StartOrderTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndOrderTime != null)
            {
                sb.Append(" AND o.OrderTime <'").Append(SearchCondition.EndOrderTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str1))
            {
                sb.Append(" and o.str1 like '%" + SearchCondition.str1 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str2))
            {
                sb.Append(" and o.str2 like '%" + SearchCondition.str2 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str3))
            {
                sb.Append(" and o.str3 like '%" + SearchCondition.str3 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str4))
            {
                sb.Append(" and o.str4 like '%" + SearchCondition.str4 + "%' ");
            }
            if (!string.IsNullOrWhiteSpace(SearchCondition.str5))
            {
                sb.Append(" and o.str5 like '%" + SearchCondition.str5.Trim() + "%' ");
            }
            //if (!string.IsNullOrEmpty(SearchCondition.str5))
            //{
            //    if (!string.IsNullOrEmpty(SearchCondition.str5.Trim()))
            //    {
            //        sb.Append(" and o.str5 like '%" + SearchCondition.str5.Trim() + "%' ");
            //    }
            //}
            if (!string.IsNullOrEmpty(SearchCondition.str6))
            {
                sb.Append(" and o.str6 like '%" + SearchCondition.str6 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str7))
            {
                sb.Append(" and o.str7 like '%" + SearchCondition.str7 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str8))
            {
                sb.Append(" and o.str8 like '%" + SearchCondition.str8 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str9))
            {
                sb.Append(" and o.str9 like '%" + SearchCondition.str9 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str10))
            {
                sb.Append(" and o.str10 like '%" + SearchCondition.str10 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str11))
            {
                sb.Append(" and o.str11 like '%" + SearchCondition.str11 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str12))
            {
                sb.Append(" and o.str12 like '%" + SearchCondition.str12 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str13))
            {
                sb.Append(" and o.str13 like '%" + SearchCondition.str13 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str14))
            {
                sb.Append(" and o.str14 like '%" + SearchCondition.str14 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str15))
            {
                sb.Append(" and ( o.str15 like '%" + SearchCondition.str15 + "%' or o.str6 like '%" + SearchCondition.str15 + "%') ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str16))
            {
                sb.Append(" and o.str16 like '%" + SearchCondition.str16 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str17))
            {
                sb.Append(" and o.str17 like '%" + SearchCondition.str17 + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.str18))
            {
                sb.Append(" and o.str18 like '%" + SearchCondition.str18 + "%' ");
            }
            //if (!string.IsNullOrEmpty(SearchCondition.str19))
            //{
            //    sb.Append(" and o.str19 like '%" + SearchCondition.str19 + "%' ");
            //}
            if (!string.IsNullOrEmpty(SearchCondition.str20))
            {
                sb.Append(" and o.str20 like '%" + SearchCondition.str20 + "%' ");
            }
            if (SearchCondition.StartDateTime1 != null)
            {
                sb.Append(" AND o.DateTime1 >='").Append(SearchCondition.StartDateTime1).Append("' ");
            }
            if (SearchCondition.EndDateTime1 != null)
            {
                sb.Append(" AND o.DateTime1 <'").Append(SearchCondition.EndDateTime1.Value.AddDays(1).DateTimeToString()).Append("' ");
            }
            if (SearchCondition.StartDateTime2 != null)
            {
                sb.Append(" AND a.DateTime2 >='").Append(SearchCondition.StartDateTime2).Append("' ");
            }
            if (SearchCondition.EndDateTime2 != null)
            {
                sb.Append(" AND o.DateTime2 <'").Append(SearchCondition.EndDateTime2.Value.AddDays(1).DateTimeToString()).Append("' ");
            }
            if (SearchCondition.StartDateTime3 != null)
            {
                sb.Append(" AND o.DateTime3 >='").Append(SearchCondition.StartDateTime3).Append("' ");
            }
            if (SearchCondition.EndDateTime3 != null)
            {
                sb.Append(" AND o.DateTime3 <'").Append(SearchCondition.EndDateTime3.Value.AddDays(1).DateTimeToString()).Append("' ");
            }
            if (SearchCondition.StartDateTime4 != null)
            {
                sb.Append(" AND o.DateTime4 >='").Append(SearchCondition.StartDateTime4).Append("' ");
            }
            if (SearchCondition.EndDateTime4 != null)
            {
                sb.Append(" AND o.DateTime4 <'").Append(SearchCondition.EndDateTime4.Value.AddDays(1).DateTimeToString()).Append("' ");
            }
            if (SearchCondition.StartDateTime5 != null)
            {
                sb.Append(" AND o.DateTime5 >='").Append(SearchCondition.StartDateTime5).Append("' ");
            }
            if (SearchCondition.EndDateTime5 != null)
            {
                sb.Append(" AND o.DateTime5 <'").Append(SearchCondition.EndDateTime5.Value.AddDays(1).DateTimeToString()).Append("' ");
            }
            if (SearchCondition.Int1 != null)
            {
                sb.Append(" AND o.Int1=").Append(SearchCondition.Int1).Append(" ");
            }
            if (SearchCondition.Int2 != null)
            {
                sb.Append(" AND o.Int2=").Append(SearchCondition.Int2).Append(" ");
            }
            if (SearchCondition.Int3 != null)
            {
                sb.Append(" AND isnull(o.Int3,'0')=").Append(SearchCondition.Int3).Append(" ");
            }
            if (SearchCondition.Int4 != null)
            {
                sb.Append(" AND o.Int4=").Append(SearchCondition.Int4).Append(" ");
            }
            if (SearchCondition.Int5 != null)
            {
                sb.Append(" AND o.Int5=").Append(SearchCondition.Int5).Append(" ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 订单状态统计条件
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        private string GenGetOrderStatusWhere(OrderSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (SearchCondition.CustomerID != null)
            {
                sb.Append(" AND o.CustomerID=").Append(SearchCondition.CustomerID).Append(" ");
            }
            if (SearchCondition.CustomerID == null && SearchCondition.CustomerIDs != null)
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                numbers = SearchCondition.CustomerIDs.Split(',').Select(s => { return s.Trim(); });
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                sb.Append(" and o.CustomerID in ( ");
                foreach (string s in numbers)
                {
                    sb.Append("'").Append(s).Append("',");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" ) ");
            }
            if (SearchCondition.Warehouse != null)
            {
                sb.Append(" AND o.Warehouse='").Append(SearchCondition.Warehouse).Append("' ");
            }

            if (SearchCondition.ID != 0)
            {
                sb.Append(" AND o.ID=").Append(SearchCondition.ID).Append(" ");

            }

            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append(" AND o.CreateTime >='").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" AND o.CreateTime <='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 订单状态统计明细
        /// </summary>
        /// <param name="SearchCondition"></param>
        /// <returns></returns>
        private string GenGetOrderStatusTotal(OrderSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (SearchCondition.CustomerID != null)
            {
                sb.Append(" AND o.CustomerID=").Append(SearchCondition.CustomerID).Append(" ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Warehouse))
            {
                sb.Append(" AND o.Warehouse='").Append(SearchCondition.Warehouse).Append("' ");
            }
            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append(" AND o.CreateTime >='").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" AND o.CreateTime <='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.Status != null)
            {
                sb.Append(" AND o.Status=").Append(SearchCondition.Status).Append(" ");
            }
            return sb.ToString();
        }

        private string GenGetOrderWhere_Wave(OrderSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();

            if (!string.IsNullOrEmpty(SearchCondition.OrderNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.OrderNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.OrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.OrderNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.OrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and o.OrderNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and o.OrderNumber  like '%" + SearchCondition.OrderNumber.Trim() + "%' ");
                }

                //sb.Append(" AND a.ReceiptNumber ='").Append(SearchCondition.ReceiptNumber).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.ExternOrderNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.ExternOrderNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.ExternOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.ExternOrderNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.ExternOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and o.ExternOrderNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and o.ExternOrderNumber  like '%" + SearchCondition.ExternOrderNumber.Trim() + "%' ");
                }

                //sb.Append(" AND a.ExternReceiptNumber='").Append(SearchCondition.ExternReceiptNumber).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.PreOrderNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.PreOrderNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.PreOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.PreOrderNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.PreOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and o.PreOrderNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and o.PreOrderNumber  like '%" + SearchCondition.PreOrderNumber.Trim() + "%' ");
                }

                //sb.Append(" AND a.ExternReceiptNumber='").Append(SearchCondition.ExternReceiptNumber).Append("' ");
            }

            if (!string.IsNullOrEmpty(SearchCondition.ExpressNumber))
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (SearchCondition.ExpressNumber.IndexOf("\n") > 0)
                {
                    numbers = SearchCondition.ExpressNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (SearchCondition.ExpressNumber.IndexOf(',') > 0)
                {
                    numbers = SearchCondition.ExpressNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }

                if (numbers != null && numbers.Any())
                {
                    sb.Append(" and o.ExpressNumber in ( ");
                    foreach (string s in numbers)
                    {
                        sb.Append("'").Append(s).Append("',");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append(" ) ");
                }
                else
                {
                    sb.Append(" and o.ExpressNumber  like '%" + SearchCondition.ExpressNumber.Trim() + "%' ");
                }

                //sb.Append(" AND a.ExternReceiptNumber='").Append(SearchCondition.ExternReceiptNumber).Append("' ");
            }
            if (SearchCondition.CustomerID != null)
            {
                sb.Append(" AND o.CustomerID=").Append(SearchCondition.CustomerID).Append(" ");

            }
            if (SearchCondition.CustomerID == null && SearchCondition.CustomerIDs != null)
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                numbers = SearchCondition.CustomerIDs.Split(',').Select(s => { return s.Trim(); });
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                sb.Append(" and o.CustomerID in ( ");
                foreach (string s in numbers)
                {
                    sb.Append("'").Append(s).Append("',");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(" ) ");
            }
            if (SearchCondition.ID != 0)
            {
                sb.Append(" AND o.ID=").Append(SearchCondition.ID).Append(" ");

            }
            if (SearchCondition.Warehouse != null)
            {
                sb.Append(" AND o.Warehouse='").Append(SearchCondition.Warehouse).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.OrderType))
            {
                sb.Append(" AND o.OrderType='").Append(SearchCondition.OrderType).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.IsHaveWave))
            {
                if (SearchCondition.IsHaveWave == "1")
                {
                    sb.Append(" AND o.WaveNumber is not null ");
                }
                else
                {
                    sb.Append(" AND o.WaveNumber is  null ");
                }

            }
            if (!string.IsNullOrEmpty(SearchCondition.WaveNumber))
            {
                sb.Append(" and o.WaveNumber  like '%" + SearchCondition.WaveNumber.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.ExpressCompany))
            {
                sb.Append(" AND o.ExpressCompany='").Append(SearchCondition.ExpressCompany).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Province))
            {
                sb.Append(" and o.Province  like '%" + SearchCondition.Province.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.City))
            {
                sb.Append(" and o.City  like '%" + SearchCondition.City.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.District))
            {
                sb.Append(" and o.District  like '%" + SearchCondition.District.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Consignee))
            {
                sb.Append(" and o.Consignee  like '%" + SearchCondition.Consignee.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Contact))
            {
                sb.Append(" and o.Contact  like '%" + SearchCondition.Contact.Trim() + "%' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.Address))
            {
                sb.Append(" and o.Address  like '%" + SearchCondition.Address.Trim() + "%' ");
            }
            if (SearchCondition.IsMerged != null)
            {
                sb.Append(" AND o.IsMerged=").Append(SearchCondition.IsMerged).Append(" ");
            }
            if (SearchCondition.Status != null)
            {
                sb.Append(" AND o.Status='").Append(SearchCondition.Status).Append("' ");
            }
            if (!string.IsNullOrEmpty(SearchCondition.IsPrintPick))
            {
                if (SearchCondition.IsPrintOther == "是")
                {
                    sb.Append(" and o.PickPrintCount  >0");
                }
                else
                {
                    sb.Append(" and o.PickPrintCount  =0");
                }
            }
            if (!string.IsNullOrEmpty(SearchCondition.IsPrintExpress))
            {
                if (SearchCondition.IsPrintOther == "是")
                {
                    sb.Append(" and o.ExpressPrintCount  >0");
                }
                else
                {
                    sb.Append(" and o.ExpressPrintCount  =0");
                }

            }
            if (!string.IsNullOrEmpty(SearchCondition.IsPrintOther))
            {
                if (SearchCondition.IsPrintOther == "是")
                {
                    sb.Append(" and o.OtherPrintCount  >0");
                }
                else
                {
                    sb.Append(" and o.OtherPrintCount  =0");
                }

            }
            if (!string.IsNullOrEmpty(SearchCondition.IsGetExpressNumber))
            {
                if (SearchCondition.IsGetExpressNumber == "已获取")
                {
                    sb.Append(" and o.ExpressStatus  =1");
                }
                else if (SearchCondition.IsGetExpressNumber == "未获取")
                {
                    sb.Append(" and o.ExpressStatus  =2");
                }
                else
                {
                    sb.Append(" and o.ExpressStatus  =3");
                }

            }
            if (!string.IsNullOrEmpty(SearchCondition.IsSkuOne))
            {
                if (SearchCondition.IsSkuOne == "单SKU")
                {
                    sb.Append(" and o.DetailCount  <=1");
                }
                else
                {
                    sb.Append(" and o.DetailCount  >1");
                }

            }
            if (SearchCondition.StartCreateTime != null)
            {
                sb.Append(" AND o.CreateTime >='").Append(SearchCondition.StartCreateTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndCreateTime != null)
            {
                sb.Append(" AND o.CreateTime <='").Append(SearchCondition.EndCreateTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }
            if (SearchCondition.StartOrderTime != null)
            {
                sb.Append(" AND o.WaveTime >='").Append(SearchCondition.StartWaveTime.DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (SearchCondition.EndOrderTime != null)
            {
                sb.Append(" AND o.WaveTime <'").Append(SearchCondition.EndWaveTime.DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
            }

            return sb.ToString();
        }

        private string GenGetOrderWhere_Wave_Having(OrderSearchCondition SearchCondition)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SearchCondition.SKUTypeAndCount) && SearchCondition.SKUTypeCounts == null)
            {
                if (SearchCondition.SKUTypeAndCount == "1")
                {
                    sb.Append(" HAVING COUNT( DISTINCT d.SKU)=1 and SUM(d.Qty)=1 ");
                }
                else if (SearchCondition.SKUTypeAndCount == "2")
                {
                    sb.Append(" HAVING COUNT( DISTINCT d.SKU)=1 and SUM(d.Qty)>1 ");
                }
                else
                {
                    sb.Append(" HAVING COUNT( DISTINCT d.SKU)>1 and SUM(d.Qty)>1 ");
                }
            }
            if (!string.IsNullOrEmpty(SearchCondition.SKUTypeAndCount) && SearchCondition.SKUTypeCounts > 0)
            {
                if (SearchCondition.SKUTypeAndCount == "1")
                {
                    sb.Append(" HAVING COUNT( DISTINCT d.SKU)=1 and SUM(d.Qty)=1 ");
                }
                else if (SearchCondition.SKUTypeAndCount == "2")
                {
                    sb.Append(" HAVING COUNT( DISTINCT d.SKU)=1 and SUM(d.Qty)>1 ");
                }
                else
                {
                    sb.Append(" HAVING COUNT( DISTINCT d.SKU)> ").Append(SearchCondition.SKUTypeCounts).Append(" and SUM(d.Qty)>1 ");
                }
            }
            if (string.IsNullOrEmpty(SearchCondition.SKUTypeAndCount) && SearchCondition.SKUTypeCounts > 0)
            {
                sb.Append(" HAVING COUNT( DISTINCT d.SKU)> ").Append(SearchCondition.SKUTypeCounts);
            }
            return sb.ToString();
        }

        public GetOrderAndOrderDetailByConditionResponse GetPrintOrder(string id, int Flag)
        {
            string Where = this.GenGetPrintOrder(id);
            string sqlWhere = this.GenGetPrintOrders(id);
            string sqlWhere2 = this.GenGetPrintOrderss(id);
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID",DbType.String,Where,ParameterDirection.Input),
                new DbParam("@OID",DbType.String,sqlWhere,ParameterDirection.Input),
                new DbParam("@POID",DbType.String,sqlWhere2,ParameterDirection.Input),
                new DbParam("@Flag",DbType.Int32,Flag,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintOrder", dbParams);
            if (Flag == 1 || Flag == 3)
            {
                response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            }
            if (Flag == 2)
            {
                response.PreOrderCollection = ds.Tables[0].ConvertToEntityCollection<PreOrder>();
                response.PreOrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();
            }

            return response;
        }

        public GetOrderAndOrderDetailByConditionResponse GetPrintOrderYFBLD(string id, int Flag)
        {
            string Where = this.GenGetPrintOrder(id);
            string sqlWhere = this.GenGetPrintOrders(id);
            string sqlWhere2 = this.GenGetPrintOrderss(id);
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID",DbType.String,Where,ParameterDirection.Input),
                new DbParam("@OID",DbType.String,sqlWhere,ParameterDirection.Input),
                new DbParam("@POID",DbType.String,sqlWhere2,ParameterDirection.Input),
                new DbParam("@Flag",DbType.Int32,Flag,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintOrderYFBLD", dbParams);
            if (Flag == 1 || Flag == 3)
            {
                response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            }
            if (Flag == 2)
            {
                response.PreOrderCollection = ds.Tables[0].ConvertToEntityCollection<PreOrder>();
                response.PreOrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();
            }

            return response;
        }

        public GetOrderAndOrderDetailByConditionResponse GetBatchPrintOrderYFBLD(string id)
        {
            string Where = this.GenGetPrintOrder(id);
            string sqlWhere = this.GenGetPrintOrders(id);
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID",DbType.String,Where,ParameterDirection.Input),
                new DbParam("@OID",DbType.String,sqlWhere,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetBatchPrintOrderYFBLD", dbParams);
            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();

            return response;
        }

        public GetOrderAndOrderDetailByConditionResponse GetPrintOrderNike(string id, int Flag)
        {
            string Where = this.GenGetPrintOrder(id);
            string sqlWhere = this.GenGetPrintOrdersNike(id);
            string sqlWhere2 = this.GenGetPrintOrderss(id);
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID",DbType.String,Where,ParameterDirection.Input),
                new DbParam("@OID",DbType.String,sqlWhere,ParameterDirection.Input),
                new DbParam("@POID",DbType.String,sqlWhere2,ParameterDirection.Input),
                new DbParam("@Flag",DbType.Int32,Flag,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintOrder_Nike", dbParams);
            if (Flag == 1 || Flag == 3)
            {
                response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            }
            if (Flag == 2)
            {
                response.PreOrderCollection = ds.Tables[0].ConvertToEntityCollection<PreOrder>();
                response.PreOrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();
            }

            return response;
        }

        /// <summary>
        /// nike拣货单b2c
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GetOrderAndOrderDetailByConditionResponse GetPrintOrderNikeB2C(string id)
        {
            string Where = this.GenGetPrintOrder(id);
            string sqlWhere = this.GenGetPrintOrdersNike(id);

            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID",DbType.String,Where,ParameterDirection.Input),
                new DbParam("@OID",DbType.String,sqlWhere,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintOrder_NikeB2C", dbParams);

            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();


            return response;
        }

        /// <summary>
        /// HABA拣货单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public GetOrderAndOrderDetailByConditionResponse GetPrintOrderHABA(string id, int Flag)
        {
            string Where = this.GenGetPrintOrder(id);
            string sqlWhere = this.GenGetPrintOrders(id);
            string sqlWhere2 = this.GenGetPrintOrderss(id);
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID",DbType.String,Where,ParameterDirection.Input),
                new DbParam("@OID",DbType.String,sqlWhere,ParameterDirection.Input),
                new DbParam("@POID",DbType.String,sqlWhere2,ParameterDirection.Input),
                new DbParam("@Flag",DbType.Int32,Flag,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintOrder_HABA", dbParams);
            if (Flag == 1 || Flag == 3)
            {
                response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            }
            if (Flag == 2)
            {
                response.PreOrderCollection = ds.Tables[0].ConvertToEntityCollection<PreOrder>();
                response.PreOrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();
            }

            return response;
        }

        /// <summary>
        /// 批量汇总打印
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public GetOrderAndOrderDetailByConditionResponse GetPrintOrderAkzo(string id, int Flag)
        {
            string Where = this.GenGetPrintOrder(id);
            string sqlWhere = this.GenGetPrintOrders(id);
            string sqlWhere2 = this.GenGetPrintOrderss(id);
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID",DbType.String,Where,ParameterDirection.Input),
                new DbParam("@OID",DbType.String,sqlWhere,ParameterDirection.Input),
                new DbParam("@POID",DbType.String,sqlWhere2,ParameterDirection.Input),
                new DbParam("@Flag",DbType.Int32,Flag,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintOrder_Akzo", dbParams);
            if (Flag == 1 || Flag == 3 || Flag == 4 || Flag == 5)
            {
                response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            }
            if (Flag == 2)
            {
                response.PreOrderCollection = ds.Tables[0].ConvertToEntityCollection<PreOrder>();
                response.PreOrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();
            }

            return response;
        }

        public GetOrderAndOrderDetailByConditionResponse GetPrintOrder_Wave(string ID)
        {
            string Where = this.GenGetPrintOrder_Wave(ID);
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Where",DbType.String,Where,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintOrder_Wave", dbParams);
            response.OrderDetailCollection = ds.Tables[0].ConvertToEntityCollection<OrderDetailInfo>();
            response.UPCSum = ds.Tables[1].Rows[0][0].ObjectToInt64();
            response.OrderSum = ds.Tables[2].Rows[0][0].ObjectToInt64();
            response.OrderCollection = ds.Tables[3].ConvertToEntityCollection<OrderInfo>();
            return response;
        }

        /// <summary>
        /// 西安吉特仓拣货单打印
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public GetOrderAndOrderDetailByConditionResponse GetPrintOrder_JT(string id, int Flag)
        {
            string Where = this.GenGetPrintOrder(id);
            string sqlWhere = this.GenGetPrintOrdersNike(id);
            string sqlWhere2 = this.GenGetPrintOrderss(id);
            String sqlwhere3 = this.GenGetPrintOrderss_TH(id);
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID",DbType.String,Where,ParameterDirection.Input),
                new DbParam("@OID",DbType.String,sqlWhere,ParameterDirection.Input),
                new DbParam("@POID",DbType.String,sqlWhere2,ParameterDirection.Input),
                new DbParam("@Flag",DbType.Int32,Flag,ParameterDirection.Input),
                new DbParam("@OID1",DbType.String,sqlwhere3,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintOrder_JT", dbParams);
            if (Flag == 1 || Flag == 3)
            {
                response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            }
            if (Flag == 2)
            {
                response.PreOrderCollection = ds.Tables[0].ConvertToEntityCollection<PreOrder>();
                response.PreOrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();
            }

            return response;
        }

        /// <summary>
        /// 退货仓批量汇总打印
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public GetOrderAndOrderDetailByConditionResponse GetPrintOrderAkzo_TH(string id, int Flag)
        {
            string Where = this.GenGetPrintOrder(id);
            string sqlWhere = this.GenGetPrintOrders(id);
            string sqlWhere2 = this.GenGetPrintOrderss(id);
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@ID",DbType.String,Where,ParameterDirection.Input),
                new DbParam("@OID",DbType.String,sqlWhere,ParameterDirection.Input),
                new DbParam("@POID",DbType.String,sqlWhere2,ParameterDirection.Input),
                new DbParam("@Flag",DbType.Int32,Flag,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintOrder_Akzo_TH", dbParams);
            if (Flag == 1 || Flag == 3 || Flag == 4 || Flag == 5)
            {
                response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            }
            if (Flag == 2)
            {
                response.PreOrderCollection = ds.Tables[0].ConvertToEntityCollection<PreOrder>();
                response.PreOrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<PreOrderDetail>();
            }

            return response;
        }

        public string GenGetPrintHeader(string id)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(id))
            {
                sb.Append("AND ID IN (SELECT OrderID FROM dbo.WMS_PrintDetail WHERE PrintID IN(" + id + "))");
            }
            return sb.ToString();
        }

        public string GenGetPrintOrder(string id)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(id))
            {
                sb.Append("AND ID IN (").Append(id).Append(")");
            }
            return sb.ToString();
        }

        public string GenGetPrintOrder_Wave(string ID)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(ID))
            {
                sb.Append("AND o.ID IN (").Append(ID).Append(")");
            }
            return sb.ToString();
        }

        public string GenGetPrintOrders(string id)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(id))
            {
                sb.Append("AND OID IN (").Append(id).Append(")");
            }
            return sb.ToString();
        }

        public string GenGetPrintOrders2(string id)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(id))
            {
                sb.Append("AND OID IN (SELECT OrderID FROM dbo.WMS_PrintDetail WHERE PrintID IN(" + id + "))");
            }
            return sb.ToString();
        }

        public string GenGetPrintOrdersNike(string id)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(id))
            {
                sb.Append("AND od.OID IN (").Append(id).Append(")");
            }
            return sb.ToString();
        }

        public string GenGetPrintOrderss(string id)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(id))
            {
                sb.Append("AND POID IN (").Append(id).Append(")");
            }
            return sb.ToString();
        }

        public string GenGetPrintOrderss_TH(string id)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(id))
            {
                sb.Append("AND OID IN (").Append(id).Append(")");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 根据订单号查询订单信息并验证
        /// </summary>
        /// <param name="Orderkey"></param>
        /// <returns></returns>
        public GetOrderByConditionResponse OrderKeyCheck(IEnumerable<OrderNumbers> list, int CustomerID)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = string.Empty;
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_CheckOrderNumber", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NumberTable", list.Select(a => new OrderNumberToDb(a)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                    conn.Close();
                    return response;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public GetOrderByConditionResponse UpdateSerialNumberByOrderNumber(IEnumerable<OrderNumbers> list, int CustomerID)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                string message = string.Empty;
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_UpdateSerialNumberByOrderNumber", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NumberTable", list.Select(a => new OrderNumberToDb(a)));
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters[1].SqlDbType = SqlDbType.BigInt;
                    cmd.CommandTimeout = 300;
                    conn.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                    conn.Close();
                    return response;
                }
                catch (Exception ex)
                {
                    //return message + "(" + ex.Message + ")";
                    throw ex;
                }
            }
        }

        public IEnumerable<BarCodeInfo> GetBarCodeByOrderID(long OrderID, string Type)
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

        /// <summary>
        /// YXDR拣货单打印查询
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public GetOrderAndOrderDetailByConditionResponse GetPrintOrderYXDR(string id, int Flag)
        {
            string Where = this.GenGetPrintOrder(id);

            string SqlWhere = this.GenGetPrintOrders(id);
            if (Flag == 4)
            {
                Where = this.GenGetPrintHeader(id);
                SqlWhere = this.GenGetPrintOrders2(id);
            }
            Flag = 1;
            string SqlWhere2 = this.GenGetPrintOrderss(id);
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            DbParam[] dbParams = new DbParam[] {
            new DbParam("@ID",DbType.String,Where,ParameterDirection.Input),
             new DbParam("@OID",DbType.String,SqlWhere,ParameterDirection.Input),
              new DbParam("@POID",DbType.String,SqlWhere2,ParameterDirection.Input),
               new DbParam("@Flag",DbType.Int32,Flag,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintOrder_YXDR", dbParams);
            if (Flag == 1 || Flag == 3 || Flag == 4)
            {
                response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
                response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            }
            if (Flag == 2)
            {
                response.PreOrderCollection = ds.Tables[0].ConvertToEntityCollection<PreOrder>();
                response.PreOrderDetailCollection = ds.Tables[0].ConvertToEntityCollection<PreOrderDetail>();
            }
            return response;
        }

        /// <summary>
        /// 打印波次拣货单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public GetOrderAndOrderDetailByConditionResponse GetPrintWaveOrderAKC(string ids)
        {
            DbParam[] dbParams = new DbParam[] {
            new DbParam("@IDs",DbType.String,ids,ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetPrintWaveOrderAKC", dbParams);
            GetOrderAndOrderDetailByConditionResponse response = new GetOrderAndOrderDetailByConditionResponse();
            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            return response;
        }

        /// <summary>
        /// HABA出库单更新体积
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public bool UpdateOrderVolume(string id, string Volume, string ShipmentType, string UserName, out string msg)
        {
            msg = "";
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Proc_WMS_UpdateOrderVolume", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters[0].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@Volume", Volume);
                cmd.Parameters[1].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@ShipmentType", ShipmentType);
                cmd.Parameters[2].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters[3].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@Message", msg);
                cmd.Parameters[4].SqlDbType = SqlDbType.VarChar;
                cmd.Parameters[4].Direction = ParameterDirection.Output;
                cmd.Parameters[4].Size = 500;
                cmd.CommandTimeout = 300;
                conn.Open();
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(ds);
                msg = sda.SelectCommand.Parameters["@Message"].Value.ToString();
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
        /// 接口新增阿克苏运单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OrderShipmentResponse AddOrderShipmentAndDetail(OrderShipmentRequest request, int type)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {

                try
                {
                    OrderShipmentResponse response = new OrderShipmentResponse();
                    string message = "";
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Proc_WMS_AddOrderShipmentAndDetail", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Header", request.shipments.ToDataTable());
                    cmd.Parameters[0].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Detail", request.shipmentDetails.ToDataTable());
                    cmd.Parameters[1].SqlDbType = SqlDbType.Structured;
                    cmd.Parameters.AddWithValue("@Type", type);
                    cmd.Parameters[2].SqlDbType = SqlDbType.Int;
                    cmd.Parameters.AddWithValue("@message", message);
                    cmd.Parameters[3].SqlDbType = SqlDbType.NVarChar;
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[3].Size = 500;
                    cmd.CommandTimeout = 300;//超时时间
                    conn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    response.shipments = ds.Tables[0].ConvertToEntityCollection<WMS_OrderShipment>();//得到真实插入数据库的数据
                    response.shipmentDetails = ds.Tables[1].ConvertToEntityCollection<WMS_OrderShipmentDetail>();
                    return response;
                }
                catch (Exception e)
                {
                    return null;
                }

            }
        }

        /// <summary>
        /// 获取阿克苏运单列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public OrderShipmentResponse GetOrderShipmentList(OrderShipmentSearchCondition request, out int rowCount)
        {
            int tempRowCount = 0;
            string Where = this.GenGetOrderShipmentWhere(request);
            OrderShipmentResponse response = new OrderShipmentResponse();
            DbParam[] dbParams = new DbParam[] {
            new DbParam("@Where",DbType.String,Where,ParameterDirection.Input),
            new DbParam("@PageIndex",DbType.Int32,request.PageIndex,ParameterDirection.Input),
            new DbParam("@PageSize",DbType.Int32,request.PageSize,ParameterDirection.Input),
            new DbParam("@RowCount", DbType.Int32, tempRowCount, ParameterDirection.Output)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetOrderShipmentList", dbParams);
            rowCount = (int)dbParams[3].Value;
            response.shipmentAndDetails = ds.Tables[0].ConvertToEntityCollection<WMS_OrderShipmentAndDetail>();
            response.shipmentDetails = ds.Tables[1].ConvertToEntityCollection<WMS_OrderShipmentDetail>();
            return response;
        }

        private string GenGetOrderShipmentWhere(OrderShipmentSearchCondition request)
        {
            StringBuilder sb = new StringBuilder();
            if (request.CustomerID != 0)
            {
                sb.Append(" AND s.CustomerID=").Append(request.CustomerID).Append(" ");
            }
            if (request.WarehouseID != null && request.WarehouseID > 0)
            {
                sb.Append(" AND s.WarehouseID=").Append(request.WarehouseID).Append(" ");
            }
            if (!string.IsNullOrEmpty(request.ShipmentNumber))
            {
                sb.Append(" AND s.ShipmentNumber like '%").Append(request.ShipmentNumber).Append("%' ");
            }
            if (!string.IsNullOrEmpty(request.ExternOrderNumber))
            {
                sb.Append(" AND sd.ExternOrderNumber like '%").Append(request.ExternOrderNumber).Append("%' ");
            }
            if (request.Type != null)
            {
                sb.Append(" AND s.Type=").Append(request.Type).Append(" ");
            }
            if (request.Status != null)
            {
                sb.Append(" AND s.Status=").Append(request.Status).Append(" ");
            }
            if (request.StartCreateTime != null)
            {
                sb.Append(" AND s.CreateTime >='").Append(DateTime.Parse(request.StartCreateTime).DateTimeToString("yyyy-MM-dd 00:00:00.00")).Append("' ");
            }
            if (request.EndCreateTime != null)
            {
                sb.Append(" AND s.CreateTime <='").Append(DateTime.Parse(request.EndCreateTime).DateTimeToString("yyyy-MM-dd 23:59:59.99")).Append("' ");
                //sb.Append(" AND s.CreateTime <='").Append(request.EndCreateTime.DateTimeToString("yyyy-MM-dd HH:mm:59.99")).Append("' ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 根据id查询订单头信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<OrderInfo> GetOrderInfosByIDs(string ids)
        {
            try
            {
                string sql = @"
                         SELECT ID, WaveNumber, OrderNumber, POID, PreOrderNumber, ExternOrderNumber, CustomerID, CustomerName, Warehouse, OrderType,
                          Status, OrderTime, Province, City, District, Address, Consignee, Contact, IsMerged, MergeNumber, ExpressCompany,
                          ExpressNumber, ExpressStatus, PickPrintCount, ExpressPrintCount, PodPrintCount, OtherPrintCount, DetailCount, Creator,
                          CreateTime, Updator, UpdateTime, CompleteDate, WaveTime, Remark, str1, str2, str3, str4, str5, str6, str7,
                          str8, str9, str10, str11, str12, str13, str14, str15, str16, str17, str18, str19, str20, DateTime1, DateTime2,
                          DateTime3, DateTime4, DateTime5, Int1, Int2, Int3, Int4, (SELECT TOP 1 CONVERT(INT,w.ID) FROM dbo.WMS_Warehouse w WHERE w.WarehouseName = Warehouse ) Int5 
                          FROM dbo.WMS_Order WHERE id IN(" + ids + ")";
                return this.ExecuteDataTableBySqlString(sql).ConvertToEntityCollection<OrderInfo>();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据id查询运单信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public OrderShipmentResponse GetOrderShipmentByID(long ID, int Type)
        {
            OrderShipmentResponse response = new OrderShipmentResponse();

            DbParam[] dbParams = new DbParam[] {
            new DbParam("@ID",DbType.Int64,ID,ParameterDirection.Input),
             new DbParam("@Type",DbType.Int32,Type,ParameterDirection.Input),
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetOrderShipmentByID", dbParams);
            if (ds != null && ds.Tables.Count > 0)
            {
                response.shipments = ds.Tables[0].ConvertToEntityCollection<WMS_OrderShipment>();
                response.shipmentDetails = ds.Tables[1].ConvertToEntityCollection<WMS_OrderShipmentDetail>();
                return response;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// type=1,更新s1-4场景状态为2，发送打印人，发送打印时间，type=2,更新s5场景状态为3，发送货物离场人，离场时间
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public bool UpdateShipmentstatusByID(long ID, string UserName, int type)
        {
            string sql = "";
            if (type == 1)
            {
                sql = @" UPDATE  dbo.WMS_OrderShipment SET Status=2,PrintCreator = '" + UserName + "',PrintTime=GETDATE() WHERE ID=" + ID + "";
            }
            else if (type == 2)
            {
                sql = @"    UPDATE  dbo.WMS_OrderShipment SET Status=3,GoodsissueCreator = '" + UserName + "',GoodsissueTime=GETDATE() WHERE ID=" + ID + "";
            }

            return this.ScanExecuteNonQuery(sql) > 0;

        }


        /// <summary>
        /// 获取需要反馈的运单
        /// </summary>
        /// <param name="mark"></param>
        /// <returns></returns>
        public OrderShipmentResponse GetConfirmOrderShipment(string mark)
        {
            OrderShipmentResponse response = new OrderShipmentResponse();
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@mark", DbType.String, mark, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetAkzoConfirmOrderShipment", dbParams);
            response.shipments = ds.Tables[0].ConvertToEntityCollection<WMS_OrderShipment>();
            response.shipmentDetails = ds.Tables[1].ConvertToEntityCollection<WMS_OrderShipmentDetail>();
            return response;
        }

        public List<OrderDetailInfo> AcquireIDS(string IDS)
        {
            using (SqlConnection conn = new SqlConnection(BaseAccessor._dataBase.ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Proc_WMS_GetAcquireIDS", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDS", IDS);
                    cmd.Parameters[0].SqlDbType = SqlDbType.NVarChar;
                    cmd.CommandTimeout = 300;
                    conn.Open();
                    DataSet ds = new DataSet();
                    SqlDataAdapter sda = new SqlDataAdapter();
                    sda.SelectCommand = cmd;
                    sda.Fill(ds);
                    //message = sda.SelectCommand.Parameters["@message"].Value.ToString();
                    conn.Close();
                    return ds.Tables[0].ConvertToEntityCollection<OrderDetailInfo>().ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            //DbParam[] dbParams = new DbParam[]{
            //    new DbParam("@IDS", DbType.String, IDS, ParameterDirection.Input),
            //};
            //DataTable dt = this.ExecuteDataTable("[Proc_WMS_GetAcquireIDS]", dbParams);
            //return dt.ConvertToEntityCollection<OrderDetailInfo>().ToList();

        }

        /// <summary>
        /// 查询退货仓待回传的订单包装信息
        /// </summary>
        /// <returns></returns>
        public GetOrderByConditionResponse GetReturnSFTPPackage(int type)
        {
            GetOrderByConditionResponse response = new GetOrderByConditionResponse();

            DbParam[] dbParams = new DbParam[]{
                new DbParam("@Type", DbType.Int32,type, ParameterDirection.Input)
            };
            DataSet ds = this.ExecuteDataSet("Proc_WMS_GetReturnSFTPPackage", dbParams);

            response.OrderCollection = ds.Tables[0].ConvertToEntityCollection<OrderInfo>();
            response.OrderDetailCollection = ds.Tables[1].ConvertToEntityCollection<OrderDetailInfo>();
            response.packages = ds.Tables[2].ConvertToEntityCollection<PackageInfo>();
            response.packageDetails = ds.Tables[3].ConvertToEntityCollection<PackageDetailInfo>();
            return response;
        }

        /// <summary>
        /// 退货仓包装回传之后更新订单int1
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="type"></param>
        public void UpdateReturnSFTPOrderFlag(string ids, int type)
        {
            DbParam[] dbParams = new DbParam[]{
                new DbParam("@IDs", DbType.String,ids, ParameterDirection.Input),
                new DbParam("@Type", DbType.Int32,type, ParameterDirection.Input)
            };
            this.ExecuteNoQuery("Proc_WMS_UpdateReturnSFTPOrderFlag", dbParams);
        }

        /// <summary>
        /// 圆通打印
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="type"></param>
        public YtoRequest PrintExpressYto(string boxnumber)
        {
            YtoRequest ytoRequest = new YtoRequest();
            try
            {

                StringBuilder sb = new StringBuilder();
                sb.Append(@" select  top 1 ed.[CustomerID]
              ,ed.[CustomerName]
              ,ed.[WarehouseID]
              ,ed.[WarehouseName]
              ,ed.[OID]
              ,ed.[OrderNumber]
              ,ed.[ExternOrderNumber]
              ,ed.[ExpressNumber]
              ,ed.[ExpressCompany]
              ,ed.[PackageNumber]
              ,ed.[Status]
              ,ed.[success]
              ,ed.[code]
              ,ed.[logisticProviderID]
              ,ed.[mailNo]
              ,ed.[txLogisticID]
              ,ed.[clientID]
              ,ed.[shortAddress]
              ,ed.[consigneeBranchCode]
              ,ed.[packageCenterCode]
              ,ed.[packageCenterName]
              ,ed.[arrivedOrgSimpleName]
              ,ed.[uniquerRequestNumber]
              ,ed.[parentMailNo]
              ,ed.[printKeyWord]
              ,ed.[reason]
              ,ed.[originateOrgCode]
              ,ed.[mn]
              ,ed.[pcn]
              ,ed.[rbc]
              ,ed.[sbc]
              ,ed.[ssc]
              ,ed.[tsc]
	          ,o.Consignee receivername
	          ,'' receiverpostCode
	          ,o.Contact receiverphone
	          ,o.Contact receivermobile
	          ,o.Province receiverprov
	          ,o.City receivercity
	          ,o.Address receiveraddress
	          ,w.Contractor sendname
	          ,'' sendpostCode
	          ,w.Mobile sendphone
	          ,w.Mobile sendmobile
	          ,w.ProvinceCity sendprov
	          ,w.Str1 sendcity
	          ,w.Address sendaddress
	          from   WMS_ExpressDelivery ed 
              left join  WMS_Order o on
              ed.OrderNumber=o.OrderNumber 
              left join WMS_Warehouse w on
              o.Warehouse=w.WarehouseName
              where  PackageNumber='" + boxnumber + "' order by ed.id desc ");
                sb.Append(@" select * from  WMS_PackageDetail  where  PackageNumber='" + boxnumber + "'");
                var data = this.ScanDataSet(sb.ToString());
                ytoRequest.expressDeliverys = data.Tables[0].ConvertToEntityCollection<ExpressDelivery>();
                ytoRequest.packageDetailInfos = data.Tables[1].ConvertToEntityCollection<PackageDetailInfo>();

            }
            catch (Exception e)
            {
                return ytoRequest;

            }

            return ytoRequest;
        }

    }
}

