using Runbow.TWS.Common;
using Runbow.TWS.Dao.RFWeb;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Runbow.TWS.MessageContracts.WMS.Order;
using Runbow.TWS.Entity.WMS.Order;

namespace Runbow.TWS.Biz.RFWeb
{
    public class OrderManagementService : BaseService
    {
        public IEnumerable<OrderInfo> GetOrderList(string customerid, string WarehouseName,string ExternOrderNumber)
        {

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                return accessor.GetOrderList(customerid, WarehouseName, ExternOrderNumber);

            }
            catch (Exception ex)
            {
                return new List<OrderInfo>();

            }

        }

        public Response<GetOrderByConditionResponse> GetPackageByCondition(long ID)
        {
            Response<GetOrderByConditionResponse> response = new Response<GetOrderByConditionResponse>() { Result = new GetOrderByConditionResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPackageByCondition(ID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<string> AddPackageAndDetail(long ID, AddPackageAndDetailRequest request, int flag)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                message = new OrderManagementAccessor().AddPackageAndDetail(ID, request, flag);
                if (message == "")
                {
                    response.Result = message;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = message;
                    response.IsSuccess = false;

                }
                return response;

            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Result = message + ex.Message;
            }
            return response;
        }
        public bool UpdatePackage(long ID, AddPackageAndDetailRequest request)
        {
            bool resualt = false;

            try
            {
                string sql = "";
                List<PackageInfo> Packagelist = request.packages.ToList();
                for (int i = 0; i < Packagelist.Count; i++)
                {
                    sql += "Update WMS_Package set ExpressNumber ='" + Packagelist[i].ExpressNumber + "' where PackageNumber='" + Packagelist[i].PackageNumber + "' and OID=" + Packagelist[i].OID + ";";
                }
                resualt = new OrderManagementAccessor().UpdatePackage(sql);


            }
            catch (Exception ex)
            {
                LogError(ex);
                resualt = false;
            }
            return resualt;
        }
        /// <summary>
        /// 插入拣货表
        /// </summary>
        /// <param name="orderPicklist"></param>
        /// <returns></returns>
        public bool InsertPick(IEnumerable<OrderDetailForRedisRF> orderPicklist, string name)
        {
            bool IsSuccess = false;
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                IsSuccess = accessor.InsertPick(orderPicklist,name);
            }
            catch (Exception ex)
            {
            }
            return IsSuccess;
        }
        public bool UpdateOrderStatusByOrderNumber(string OrderNumber, string Picker, AddPackageAndDetailRequest request,string CustomerID)
        {
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                return accessor.UpdateOrderStatusByOrderNumber(OrderNumber, Picker,  request,CustomerID);
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<OrderDetailForRedisRF> GetOrderDetailListByOrderNumber(string ordernum,string CustomerID)
        {

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                return accessor.GetOrderDetailListByOrderNumber(ordernum,CustomerID);

            }
            catch (Exception ex)
            {
                return new List<OrderDetailForRedisRF>();

            }

        }

        public bool UpdateOrderStatus(string ReceiptNumber, string customerid, string warehousename, string Picker, string type)
        {
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                return accessor.UpdateOrderStatus(ReceiptNumber, customerid, warehousename, Picker, type);
            }
            catch
            {
                return false;
            }
        }
        public IEnumerable<OrderDetailForRedisRF> GetOrderDetailList(string ordernum, string customerid, string warehousename)
        {

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                return accessor.GetOrderDetailList(ordernum, customerid, warehousename);

            }
            catch (Exception ex)
            {
                return new List<OrderDetailForRedisRF>();

            }

        }
        public IEnumerable<OrderDetailInfo> GetOrderDetailListByWave(string ordernum, string customerid, string warehousename)
        {

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                return accessor.GetOrderDetailListByWave(ordernum, customerid, warehousename);

            }
            catch (Exception ex)
            {
                return new List<OrderDetailInfo>();

            }

        }
        public IEnumerable<PackageDetailInfo> GetPackageDetailList(string orderid, string customerid, string warehousename)
        {

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                return accessor.GetPackageDetailList(orderid, customerid, warehousename);

            }
            catch (Exception ex)
            {
                return new List<PackageDetailInfo>();

            }

        }
        public IEnumerable<PackageInfo> GetPackageList(string ordernumber, string customerid, string warehousename)
        {

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                return accessor.GetPackageList(ordernumber, customerid, warehousename);

            }
            catch (Exception ex)
            {
                return new List<PackageInfo>();

            }

        }
        public IEnumerable<OrderDetailInfo> GetOrderDetailList2(string ordernum, string customerid, string warehousename)
        {

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                return accessor.GetOrderDetailList2(ordernum, customerid, warehousename);

            }
            catch (Exception ex)
            {
                return new List<OrderDetailInfo>();

            }

        }

        public ExpressPackageResponse CheckExpress(string ExpressNumber, long CustomerID, string WarehouseName)
        {
            ExpressPackageResponse EPR = new ExpressPackageResponse();
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                EPR = accessor.CheckExpress(ExpressNumber, CustomerID, WarehouseName);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return EPR;
        }
        public DataSet CheckExpress(string ExpressNumber, long CustomerID, string WarehouseName, string Type)
        {
            DataSet EPR = new DataSet();
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                EPR = accessor.CheckExpress(ExpressNumber, CustomerID, WarehouseName, Type);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return EPR;
        }
        public DataSet SaveExpressPackage(string ExpressNumber, string PackageType, long CustomerID, string WarehouseName)
        {
            DataSet EPR = new DataSet();
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                EPR = accessor.SaveExpressPackage(ExpressNumber, PackageType, CustomerID, WarehouseName);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return EPR;
        }

        /// <summary>
        /// 爱库存扫描波次号拣货
        /// </summary>
        /// <param name="printKey"></param>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool RFSaveWavePicking_AKC(string printKey, string username, out string msg)
        {
            msg = "";
            try
            {
                return new OrderManagementAccessor().RFSaveWavePicking_AKC(printKey, username, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// 根据波次号获取订单头和明细RF枪  爱库存
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="BatchNumber"></param>
        /// <param name="QueryType"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public OrderAndDetailModel RFGetOrderAndDetail_AKC(string PrintKey, out string msg)
        {
            msg = string.Empty;
            try
            {
                OrderAndDetailModel model = new OrderAndDetailModel();
                model = new OrderManagementAccessor().RFGetOrderAndDetail_AKC(PrintKey);
                msg = "200";
                return model;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        /// <summary>
        /// 查询已分拣信息
        /// </summary>
        /// <param name="OID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public IEnumerable<OrderPickDetail> RFSearchSecondSorting_AKC(string PrintKey)
        {
            try
            {
                return new OrderManagementAccessor().RFSearchSecondSorting_AKC(PrintKey);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 提交二次分拣数据，爱库存
        /// </summary>
        /// <param name="PrintKey"></param>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool RFSaveSecondSorting_AKC(string PrintKey, string username, out string msg)
        {
            msg = "";
            try
            {
                return new OrderManagementAccessor().RFSaveSecondSorting_AKC(PrintKey, username, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }

        /// <summary>
        /// 验证快递单号是否存在
        /// </summary>
        /// <param name="number"></param>
        /// <param name="customerID"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public OrderInfo RFValidateExpressNumber_AKC(string number, long customerID, out string msg)
        {
            msg = "";
            try
            {
                return new OrderManagementAccessor().RFValidateExpressNumber_AKC(number, customerID);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        /// <summary>
        /// 快递包装保存
        /// </summary>
        /// <param name="PrintKey"></param>
        /// <param name="username"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool RFSaveExpressPackage_AKC(long CustomerID, string OrderNumber,string ExpressNumber,string username, out string msg)
        {
            msg = "";
            try
            {
                return new OrderManagementAccessor().RFSaveExpressPackage_AKC(CustomerID, OrderNumber,ExpressNumber,username, out msg);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }
        }

    }
}
