using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Entity.WMS.NikeOSRBJPrint;
using Runbow.TWS.Entity.WMS.Order;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.NikeNFSPrint;
using Runbow.TWS.MessageContracts.NikeOSRBJPrint;
using Runbow.TWS.MessageContracts.WebApi;
using Runbow.TWS.MessageContracts.WMS.Order;
using Runbow.TWS.MessageContracts.YXDBJRPrint;

namespace Runbow.TWS.Biz
{
    public class OrderManagementService : BaseService
    {
        public Response<GetOrderByConditionResponse> GetOrderByCondition(GetOrderByConditionRequest request)
        {
            Response<GetOrderByConditionResponse> response = new Response<GetOrderByConditionResponse>() { Result = new GetOrderByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetOrderByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                int RowCount;
                response.Result = accessor.GetOrderByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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

        /// <summary>
        /// 下发拣货任务
        /// </summary>
        /// <param name="IDS"></param>
        /// <returns></returns>
        public Response<string> OrderTask(string IDS, string Name)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                message = new OrderManagementAccessor().OrderTask(IDS, Name);
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

        ///// <summary>
        ///// 检查拣货差异
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public List<OrderDetailForRedisRF> CheckDiff(string id)
        //{
        //    List<OrderDetailForRedisRF> response = new List<OrderDetailForRedisRF>();

        //    OrderManagementAccessor accessor = new OrderManagementAccessor();
        //    try
        //    {
        //        response = accessor.CheckDiff(id);

        //    }
        //    catch
        //    {
        //        response = null;
        //    }
        //    return response;
        //}

        /// <summary>
        /// 检查包装差异
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<OrderDetailForRedisRF> CheckDiff(string id)
        {
            List<OrderDetailForRedisRF> response = new List<OrderDetailForRedisRF>();

            OrderManagementAccessor accessor = new OrderManagementAccessor();
            try
            {
                response = accessor.CheckDiff(id);

            }
            catch
            {
                response = null;
            }
            return response;
        }

        /// <summary>
        /// 导出包装差异
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<OrderDetailForRedisRF> CheckDiffBatch(IEnumerable<OrderBackStatus> Orders)
        {
            List<OrderDetailForRedisRF> response = new List<OrderDetailForRedisRF>();

            OrderManagementAccessor accessor = new OrderManagementAccessor();
            try
            {
                response = accessor.CheckDiffBatch(Orders);

            }
            catch
            {
                response = null;
            }
            return response;
        }

        public Response<GetOrderByConditionResponse> GetOrderDetailByIDS(string IDS)
        {
            Response<GetOrderByConditionResponse> response = new Response<GetOrderByConditionResponse>() { Result = new GetOrderByConditionResponse() };


            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();

                response.Result = accessor.GetOrderDetailByIDS(IDS);

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

        public Response<GetOrderByConditionResponse> GetOrderImportByCondition(GetOrderByConditionRequest request)
        {
            Response<GetOrderByConditionResponse> response = new Response<GetOrderByConditionResponse>() { Result = new GetOrderByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetOrderByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetOrderImportByCondition(request.SearchCondition);
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

        public Response<GetOrderByConditionResponse> GetOrderByIDs(string IDs)
        {
            Response<GetOrderByConditionResponse> response = new Response<GetOrderByConditionResponse>() { Result = new GetOrderByConditionResponse() };


            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();

                response.Result = accessor.GetOrderByIDs(IDs);

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

        public Response<GetOrderByConditionResponse> GetOrderHeaderByCondition(GetOrderByConditionRequest request)
        {
            Response<GetOrderByConditionResponse> response = new Response<GetOrderByConditionResponse>() { Result = new GetOrderByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetOrderByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                int RowCount;
                response.Result = accessor.GetOrderHeaderByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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

        /// <summary>
        /// 订单状态统计查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetOrderByConditionResponse> GetOrderStatusByCondition(GetOrderByConditionRequest request)
        {
            Response<GetOrderByConditionResponse> response = new Response<GetOrderByConditionResponse>() { Result = new GetOrderByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetOrderByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                int RowCount;
                response.Result = accessor.GetOrderStatusByCondition(request.SearchCondition);

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

        /// <summary>
        /// 查询此状态下的所有订单
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public Response<GetOrderByConditionResponse> SearchOrderTotal(OrderSearchCondition search, int type)
        {
            Response<GetOrderByConditionResponse> response = new Response<GetOrderByConditionResponse>() { Result = new GetOrderByConditionResponse() };

            if (search == null)
            {
                ArgumentNullException ex = new ArgumentNullException("OrderSearchCondition search");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.SearchOrderTotal(search, type);
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

        public Response<GetOrderByConditionResponse> GetOrderByCondition_Wave(GetOrderByConditionRequest request)
        {
            Response<GetOrderByConditionResponse> response = new Response<GetOrderByConditionResponse>() { Result = new GetOrderByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetOrderByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                int RowCount;
                response.Result = accessor.GetOrderByCondition_Wave(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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

        //箱唛打印箱清单20170111
        public NFSBoxListManagementResponse GetNFSPackageBoxCarton(string ID, string Type)
        {
            Response<NFSBoxListManagementResponse> response = new Response<NFSBoxListManagementResponse>() { Result = new NFSBoxListManagementResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetNFSPackageBoxCarton(ID, Type);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response.Result;
        }

        public Response<GetOrderByConditionResponse> GetPackageByID(long ID)
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

        //打印箱清单
        public BoxListManagementResponse GetPrintBoxListCondition(string ID, string Type, string Proc)
        {
            Response<BoxListManagementResponse> response = new Response<BoxListManagementResponse>() { Result = new BoxListManagementResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintBoxListCondition(ID, Type, Proc);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response.Result;
        }

        /// <summary>
        /// 获取打印的面单信息
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Proc"></param>
        /// <returns></returns>
        public IEnumerable<OrderInfo> GetPrintExpressListCondition(string ID, string Proc, int Type)
        {
            IEnumerable<OrderInfo> orders;
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                orders = accessor.GetPrintExpressListCondition(ID, Proc, Type);
                return orders;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// nike打印面单信息获取
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Proc"></param>
        /// <returns></returns>
        public IEnumerable<OrderInfo> GetPrintExpressNike(string ID)
        {
            IEnumerable<OrderInfo> orders;
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                orders = accessor.GetPrintExpressNike(ID);
                return orders;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<PrintExpressJite> GetPrintExpressDeppon(string ID)
        {
            IEnumerable<PrintExpressJite> orders;
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                orders = accessor.GetPrintExpressDeppon(ID);
                return orders;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<PrintExpressYd> GetPrintExpressYd(string ID)
        {
            IEnumerable<PrintExpressYd> orders;
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                orders = accessor.GetPrintExpressYd(ID);
                return orders;
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// 打印总箱单查询
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public BoxListManagementResponse GetPrintTotalBoxListCondition(string ID, string Type)
        {
            Response<BoxListManagementResponse> response = new Response<BoxListManagementResponse>() { Result = new BoxListManagementResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintTotalBoxListCondition(ID, Type);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response.Result;
        }

        public DataSet ExportBoxDetails(string OrderNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                ds = accessor.ExportBoxDetails(OrderNumber);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
            return ds;
        }

        public DataSet ExportBoxDetails_TH(string OrderNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                ds = accessor.ExportBoxDetails_TH(OrderNumber);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
            return ds;
        }

        public DataSet ExportBoxDetailsYXDR(string OrderNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                ds = accessor.ExportBoxDetailsYXDR(OrderNumber);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return null;
            }
            return ds;
        }

        //箱唛打印
        public BoxListManagementResponse GetPackageBoxCarton(string ID, string Type)
        {
            Response<BoxListManagementResponse> response = new Response<BoxListManagementResponse>() { Result = new BoxListManagementResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPackageBoxCarton(ID, Type);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response.Result;
        }

        //吉特面单打印
        public BoxListManagementResponse GetPackageBoxCartonJite(string ID, string Type)
        {
            Response<BoxListManagementResponse> response = new Response<BoxListManagementResponse>() { Result = new BoxListManagementResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPackageBoxCartonJite(ID, Type);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response.Result;
        }

        public IEnumerable<PackageInfo> GetPackageInfos(string id, string type)
        {
            Response<IEnumerable<PackageInfo>> response = new Response<IEnumerable<PackageInfo>>();
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPackageInfos(id, type);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }
            return response.Result;
        }

        //箱唛打印
        public BoxListManagementResponse GetPackageBoxCarton(string ID, string Type, string Proc)
        {
            Response<BoxListManagementResponse> response = new Response<BoxListManagementResponse>() { Result = new BoxListManagementResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPackageBoxCarton(ID, Type, Proc);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response.Result;
        }

        /// <summary>
        /// YXDR报关箱唛
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Type"></param>
        /// <param name="Proc"></param>
        /// <returns></returns>
        public BoxListManagementResponse GetPackageCustomerCarton(string ID, string Type)
        {
            Response<BoxListManagementResponse> response = new Response<BoxListManagementResponse>() { Result = new BoxListManagementResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPackageCustomerCarton(ID, Type);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response.Result;
        }

        public Response<OutboundInstructionsResponse> AddInstructions(string Ids, string WorkStation, string ReleatedType, int Priority, string UserName)
        {
            Response<OutboundInstructionsResponse> response = new Response<OutboundInstructionsResponse>() { Result = new OutboundInstructionsResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result.instructionInfo = accessor.AddInstructions(Ids, WorkStation, ReleatedType, Priority, UserName);
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

        public bool UpdateResults(string Ids, string UserName)
        {
            bool IsSuccess = false;
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                IsSuccess = accessor.UpdateResults(Ids, UserName);
            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return IsSuccess;
        }

        public string UpdatePrintStatus(string Ids)
        {
            string resualt = "";
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                resualt = accessor.UpdatePrintStatus(Ids);
            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return resualt;
        }

        public string ChangeExpressByOrderNumber(List<OrderInfo> orderList)
        {
            string resualt = "";
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                resualt = accessor.ChangeExpressByOrderNumber(orderList);
            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return resualt;
        }

        public string GetMaxBoxnumber(string OrderID)
        {
            string MaxBoxnumber = "";
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                MaxBoxnumber = accessor.GetMaxBoxnumber(OrderID);
            }
            catch (Exception ex)
            {

            }

            return MaxBoxnumber;
        }

        public string GetMaxBoxnumber(string OrderID, string ProcName)
        {
            string MaxBoxnumber = "";
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                MaxBoxnumber = accessor.GetMaxBoxnumber(OrderID, ProcName);
            }
            catch (Exception ex)
            {

            }

            return MaxBoxnumber;
        }

        public string DeletePackInfo(string PackageKey)
        {
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                if (accessor.DeletePackInfo(PackageKey))
                    return "";
                return "-1";
            }
            catch (Exception ex)
            {
                return "-1";
                //return ex.Message;
            }

            return "-1";
        }

        /// <summary>
        /// POD
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ConsignmentManagementResponse GetPrintPodCondition(string ID, string Type)
        {
            Response<ConsignmentManagementResponse> response = new Response<ConsignmentManagementResponse>() { Result = new ConsignmentManagementResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintPodCondition(ID, Type);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response.Result;
        }

        /// <summary>
        /// YXDRPOD
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public YXDRBJPODManagementResponse GetYXDRPrintPodCondition(string ID, string Type)
        {
            Response<YXDRBJPODManagementResponse> response = new Response<YXDRBJPODManagementResponse>() { Result = new YXDRBJPODManagementResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetYXDRPrintPodCondition(ID, Type);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response.Result;
        }

        /// <summary>
        /// YXDRAllPOD
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public YXDRBJPODManagementResponse GetYXDRPrintAllPodCondition(string IDs)
        {
            Response<YXDRBJPODManagementResponse> response = new Response<YXDRBJPODManagementResponse>() { Result = new YXDRBJPODManagementResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetYXDRPrintAllPodCondition(IDs);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response.Result;
        }

        public ConsignmentManagementResponse GetPrintAllPodCondition(string IDs)
        {
            Response<ConsignmentManagementResponse> response = new Response<ConsignmentManagementResponse>() { Result = new ConsignmentManagementResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintAllPodCondition(IDs);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response.Result;
        }
        /// <summary>
        /// 汇总查询托运单
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="customerID"></param>
        /// <param name="warehouseName"></param>
        /// <param name="searchTime">输入时间查询</param>
        /// <returns></returns>
        public ConsignmentManagementResponse GetPrintSumAllPodCondition(string IDs, long customerID, string warehouseName, string searchTime)
        {
            Response<ConsignmentManagementResponse> response = new Response<ConsignmentManagementResponse>() { Result = new ConsignmentManagementResponse() };
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintSumAllPodCondition(IDs, customerID, warehouseName, searchTime);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }
            return response.Result;
        }

        /// <summary>
        /// 退货仓托运单批量打印
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public ConsignmentManagementResponse GetPrintPodCondition_TH(string ID, string Type)
        {
            Response<ConsignmentManagementResponse> response = new Response<ConsignmentManagementResponse>() { Result = new ConsignmentManagementResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintPodCondition_TH(ID, Type);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response.Result;
        }

        /// <summary>
        /// 退货仓批量打印托运单（出库界面）
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public ConsignmentManagementResponse GetPrintAllPodCondition_TH(string IDs)
        {
            Response<ConsignmentManagementResponse> response = new Response<ConsignmentManagementResponse>() { Result = new ConsignmentManagementResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintAllPodCondition_TH(IDs);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response.Result;
        }

        /// <summary>
        /// 退货仓批量汇总打印托运单（出库界面）
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="customerID"></param>
        /// <param name="warehouseName"></param>
        /// <param name="searchTime">输入时间查询</param>
        /// <returns></returns>
        public ConsignmentManagementResponse GetPrintSumAllPodCondition_TH(string IDs, long customerID, string warehouseName, string searchTime)
        {
            Response<ConsignmentManagementResponse> response = new Response<ConsignmentManagementResponse>() { Result = new ConsignmentManagementResponse() };
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintSumAllPodCondition_TH(IDs, customerID, warehouseName, searchTime);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }
            return response.Result;
        }

        public Response<GetOrderAndOrderDetailByConditionResponse> GetOrderAndOrderDetailByCondition(long ID)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetOrderAndOrderDetailByCondition(ID);
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

        public Response<GetOrderAndOrderDetailByConditionResponse> GetSkuListByCondition(long ID)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetSkuListByCondition(ID);
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

        public Response<IEnumerable<OrderInfo>> GetProvinceList()
        {
            Response<IEnumerable<OrderInfo>> response = new Response<IEnumerable<OrderInfo>>();
            try
            {

                response.Result = new OrderManagementAccessor().GetProvinceList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public Response<IEnumerable<OrderInfo>> GetCityList(string province)
        {
            Response<IEnumerable<OrderInfo>> response = new Response<IEnumerable<OrderInfo>>();
            try
            {

                response.Result = new OrderManagementAccessor().GetCityList(province);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public Response<IEnumerable<OrderInfo>> GetDistrictList(string city)
        {
            Response<IEnumerable<OrderInfo>> response = new Response<IEnumerable<OrderInfo>>();
            try
            {

                response.Result = new OrderManagementAccessor().GetDistrictList(city);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public Response<string> Pick(IEnumerable<OrderBackStatus> Orders, string type, string PickerOrConfirmer)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                message = new OrderManagementAccessor().Pick(Orders, type, PickerOrConfirmer);
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

        public Response<string> Handover(IEnumerable<OrderBackStatus> Orders, string Handoveror)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                message = new OrderManagementAccessor().Handover(Orders, Handoveror);
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

        public Response<string> Outs(IEnumerable<OrderBackStatus> Orders)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                message = new OrderManagementAccessor().Outs(Orders);
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

        /// <summary>
        /// 差异出库
        /// </summary>
        /// <param name="Orders"></param>
        /// <returns></returns>
        public Response<string> OutsWithDiff(IEnumerable<OrderBackStatus> Orders)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                message = new OrderManagementAccessor().OutsWithDiff(Orders);
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

        /// <summary>
        /// 更新快递公司
        /// </summary>
        /// <param name="IDs">id集合</param>
        /// <param name="ExpressCompany">需要更新的快递公司</param>
        /// <returns></returns>
        public Response<string> ChangeExpress(string IDs, string ExpressCompany)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                message = new OrderManagementAccessor().ChangeExpress(IDs, ExpressCompany);
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

        public Response<string> AllocatedWave(string IDS, string WaveNumber)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                message = new OrderManagementAccessor().AllocatedWave(IDS, WaveNumber);
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

        public Response<string> UnionOrder(IEnumerable<OrderBackStatus> Orders)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                message = new OrderManagementAccessor().UnionOrder(Orders);
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

        public Response<string> OrderBackStatus(GetOrderByConditionRequest request, int ToStatus, int type)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                message = new OrderManagementAccessor().OrderBackStatus(request, ToStatus, type);
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

        public Response<GetOrderAndOrderDetailByConditionResponse> GetPrintOrder(string id, int Flag)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintOrder(id, Flag);
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

        /// <summary>
        /// HABA打印拣货单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public Response<GetOrderAndOrderDetailByConditionResponse> GetPrintOrderHABA(string id, int Flag)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintOrderHABA(id, Flag);
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

        public Response<GetOrderAndOrderDetailByConditionResponse> GetPrintOrderYFBLD(string id, int Flag)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintOrderYFBLD(id, Flag);
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

        public Response<GetOrderAndOrderDetailByConditionResponse> GetBatchPrintOrderYFBLD(string id)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetBatchPrintOrderYFBLD(id);
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
        /// <summary>
        /// 拣货单打印
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public Response<GetOrderAndOrderDetailByConditionResponse> GetPrintOrderNike(string id, int Flag)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintOrderNike(id, Flag);
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

        /// <summary>
        /// nike b2c拣货单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public Response<GetOrderAndOrderDetailByConditionResponse> GetPrintOrderNikeB2C(string id)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintOrderNikeB2C(id);
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

        /// <summary>
        /// 批量汇总打印
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public Response<GetOrderAndOrderDetailByConditionResponse> GetPrintOrderAkzo(string id, int Flag)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintOrderAkzo(id, Flag);
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

        public Response<GetOrderAndOrderDetailByConditionResponse> GetPrintOrder_Wave(string ID)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintOrder_Wave(ID);
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

        /// <summary>
        /// 退货仓 批量汇总打印
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public Response<GetOrderAndOrderDetailByConditionResponse> GetPrintOrderAkzo_TH(string id, int Flag)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintOrderAkzo_TH(id, Flag);
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

        /// <summary>
        /// 退货仓拣货单打印
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public Response<GetOrderAndOrderDetailByConditionResponse> GetPrintOrder_JT(string id, int Flag)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };

            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintOrder_JT(id, Flag);
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

        public string GetQueryDetail(string p, string querykey)
        {
            string MaxBoxnumber = "";
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                MaxBoxnumber = accessor.GetQueryDetail(p, querykey);
            }
            catch (Exception ex)
            {
                return "错误：" + ex.Message.ToString();
            }
            return MaxBoxnumber;
        }

        /// <summary>
        /// 根据订单号查询订单信息并验证
        /// </summary>
        /// <param name="orderkey"></param>
        /// <returns></returns>
        public Response<GetOrderByConditionResponse> OrderKeyCheck(IEnumerable<OrderNumbers> list, int CustomerID)
        {
            Response<GetOrderByConditionResponse> response = new Response<GetOrderByConditionResponse>() { Result = new GetOrderByConditionResponse() };
            try
            {
                response.Result = new OrderManagementAccessor().OrderKeyCheck(list, CustomerID);


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

        public Response<GetOrderByConditionResponse> UpdateSerialNumberByOrderNumber(IEnumerable<OrderNumbers> list, int CustomerID)
        {
            Response<GetOrderByConditionResponse> response = new Response<GetOrderByConditionResponse>() { Result = new GetOrderByConditionResponse() };
            try
            {
                response.Result = new OrderManagementAccessor().UpdateSerialNumberByOrderNumber(list, CustomerID);


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

        public Response<IEnumerable<BarCodeInfo>> GetBarCodeByOrderID(long OrderID, string Type)
        {
            Response<IEnumerable<BarCodeInfo>> response = new Response<IEnumerable<BarCodeInfo>>();
            try
            {

                response.Result = new OrderManagementAccessor().GetBarCodeByOrderID(OrderID, Type);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }
            return response;
        }

        public Response<GetOrderAndOrderDetailByConditionResponse> GetPrintOrderYXDR(string id, int Flag)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintOrderYXDR(id, Flag);
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

        /// <summary>
        /// 获取波次拣货单的打印信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Response<GetOrderAndOrderDetailByConditionResponse> GetPrintWaveOrderAKC(string id)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                response.Result = accessor.GetPrintWaveOrderAKC(id);
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

        /// <summary>
        ///  更新体积
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool UpdateOrderVolume(string id, string volume, string ShipmentType, string UserName, out string msg)
        {
            msg = "";
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                bool result = accessor.UpdateOrderVolume(id, volume, ShipmentType, UserName, out msg);
                return result;
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }

        }

        /// <summary>
        /// 接口新增阿克苏运单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<OrderShipmentResponse> AddOrderShipmentAndDetail(OrderShipmentRequest request, int type)
        {
            Response<OrderShipmentResponse> response = new Response<OrderShipmentResponse> { Result = new OrderShipmentResponse() };
            string str = "";
            if (request == null || request.shipments == null || !request.shipments.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("sp_wms_GetSKU request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                int RowCount = 0;
                response.Result = accessor.AddOrderShipmentAndDetail(request, type);
                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public OrderShipmentResponse GetOrderShipmentList(OrderShipmentSearchCondition request, out string msg, out int rowcounts)
        {
            msg = "";
            rowcounts = 0;
            try
            {
                OrderManagementAccessor accessor = new OrderManagementAccessor();
                return accessor.GetOrderShipmentList(request, out rowcounts);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                rowcounts = 0;
                return null;
            }
        }

        /// <summary>
        /// 根据ID查询订单信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public IEnumerable<OrderInfo> GetOrderInfosByIDs(string ids)
        {
            try
            {
                return new OrderManagementAccessor().GetOrderInfosByIDs(ids);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据ID查询运单信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public OrderShipmentResponse GetOrderShipmentByID(long ID, int Type)
        {
            try
            {
                return new OrderManagementAccessor().GetOrderShipmentByID(ID, Type);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 更新S1-4运单状态为2
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateShipmentstatusByID(long ID, string UserName, int type, out string msg)
        {
            msg = "";
            try
            {
                return new OrderManagementAccessor().UpdateShipmentstatusByID(ID, UserName, type);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }

        }

        /// <summary>
        /// 获取需要上传的发运单
        /// </summary>
        /// <returns></returns>
        public OrderShipmentResponse GetConfirmOrderShipment(string mark)
        {
            try
            {
                return new OrderManagementAccessor().GetConfirmOrderShipment(mark);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<OrderDetailInfo> AcquireIDS(string IDs)
        {
            List<OrderDetailInfo> response = new List<OrderDetailInfo>();

            OrderManagementAccessor accessor = new OrderManagementAccessor();
            try
            {
                response = accessor.AcquireIDS(IDs);

            }
            catch (Exception e)
            {
                response = null;
            }
            return response;


        }

        /// <summary>
        /// 获取退货仓待回传的包装信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GetOrderByConditionResponse GetReturnSFTPPackage(int type)
        {
            try
            {
                return new OrderManagementAccessor().GetReturnSFTPPackage(type);
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        /// <summary>
        /// 退货仓包装回传之后更新订单int1
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="type"></param>
        public void UpdateReturnSFTPOrderFlag(string ids, int type)
        {
            try
            {
                new OrderManagementAccessor().UpdateReturnSFTPOrderFlag(ids, type);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 退货仓包装回传之后更新订单int1
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="type"></param>
        public Response<YtoRequest> PrintExpressYto(string boxnumber)
        {
            Response<YtoRequest> response = new Response<YtoRequest>() { Result = new YtoRequest() };
            try
            {
                response.Result = new OrderManagementAccessor().PrintExpressYto(boxnumber);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
            }
            return response;
        }

    }
}
