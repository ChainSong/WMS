using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Entity.WMS.PreOrders;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.Order;
using Runbow.TWS.MessageContracts.WMS.PreOrders;
using Runbow.TWS.MessageContracts.WMS.Inventory;
using Runbow.TWS.Entity.WMS.NIKECE.ShipRequest;

namespace Runbow.TWS.Biz.WMS
{
    public class PreOrderService : BaseService
    {
        /// <summary>
        /// 查询预入库单
        /// </summary>
        /// <returns></returns>
        public Response<PreOrderResponse> GetPreOrder(PreOrderRequest request)
        {
            Response<PreOrderResponse> response = new Response<PreOrderResponse>() { Result = new PreOrderResponse() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("sp_wms_GetSKU request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                if (request.PageSize == 0)
                {
                    response.Result.SearchCondition = new PreOrderAccessor().GetAllPrdOrder(request.SearchCondition);
                }
                else
                {
                    int RowCount = 0;
                    response.Result.SearchCondition = new PreOrderAccessor().GetPrdOrder(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
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

        /// <summary>
        /// 查询预入库单和明细导出
        /// </summary>
        /// <returns></returns>
        public Response<PreOrderResponse> GetPreOrderExecl(PreOrderRequest request)
        {
            Response<PreOrderResponse> response = new Response<PreOrderResponse>() { Result = new PreOrderResponse() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("sp_wms_GetSKU request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                //int RowCount = 0;
                response.Result = new PreOrderAccessor().GetPreOrderExecl(request.SearchCondition);
                //response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                //response.Result.PageIndex = request.PageIndex;
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

        /// <summary>
        /// 勾选ID导出预出库单
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public Response<PreOrderResponse> GetPreOrderByIDs(string IDs)
        {
            Response<PreOrderResponse> response = new Response<PreOrderResponse>() { Result = new PreOrderResponse() };
            try
            {
                PreOrderAccessor accessor = new PreOrderAccessor();

                response.Result = accessor.GetPreorderByIDs(IDs);

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

        //获取要上传的出库订单和出库订单明细
        public Response<GetOrderAndOrderDetailByConditionResponse> GetConfirmOrderAndOrderDetail(string mark)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };


            try
            {
                PreOrderAccessor accessor = new PreOrderAccessor();

                response.Result = accessor.GetConfirmOrderAndOrderDetail(mark);

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

        //获取要上传的出库订单和出库订单明细
        public Response<GetOrderAndOrderDetailByConditionResponse> GetConfirmOrderAndOrderDetailByNikeCE(string mark)
        {
            Response<GetOrderAndOrderDetailByConditionResponse> response = new Response<GetOrderAndOrderDetailByConditionResponse>() { Result = new GetOrderAndOrderDetailByConditionResponse() };


            try
            {
                PreOrderAccessor accessor = new PreOrderAccessor();

                response.Result = accessor.GetConfirmOrderAndOrderDetailByNikeCE(mark);

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


        //获取取消订单
        public PreOrderAndPreOrderDetail GetPreOrderAndPreOrderDetail(string mark)
        {
            PreOrderAndPreOrderDetail response = new PreOrderAndPreOrderDetail();


            try
            {
                PreOrderAccessor accessor = new PreOrderAccessor();

                response = accessor.GetPreOrderAndPreOrderDetail(mark);

            }
            catch (Exception ex)
            {
                return null;
            }

            return response;
        }
        /// <summary>
        /// 查询预入库单明细
        /// </summary>
        /// <returns></returns>
        public Response<PreOrderAndPreOrderDetail> GetInventoryOfOutbound(InventoryOfOutboundRequest request)
        {
            Response<PreOrderAndPreOrderDetail> response = new Response<PreOrderAndPreOrderDetail>() { Result = new PreOrderAndPreOrderDetail() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("InventoryOfOutbound request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                int RowCount = 0;
                response.Result = new PreOrderAccessor().GetInventoryOfOutbound(request);
                response.Result.PreO = new PreOrder();

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


        /// <summary>
        /// 查询预入库单明细
        /// </summary>
        /// <returns></returns>
        public Response<PreOrderAndPreOrderDetail> GetPreOrderAndDetail(PreOrderRequest request)
        {
            Response<PreOrderAndPreOrderDetail> response = new Response<PreOrderAndPreOrderDetail>() { Result = new PreOrderAndPreOrderDetail() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("Proc_WMS_GetPreOrderAndDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                int RowCount = 0;
                response.Result = new PreOrderAccessor().GetPrdOrder(request.SearchCondition);
                //response.Result.PreO = new PreOrder();

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

        /// <summary>
        /// 查询预入库单明细
        /// </summary>
        /// <returns></returns>
        public Response<int> UpdateAssociateFG(PreOrderRequest request)
        {
            Response<int> response = new Response<int>() { Result = 0 };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateAssociateFG request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                int RowCount = 0;
                response.Result = new PreOrderAccessor().UpdateAssociateFG(request.PreOd);
                //response.Result.PreO = new PreOrder();

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

        public Response<PreOrderAndPreOrderDetail> Allocation_GetPreOrderAndDetail(PreOrderRequest request)
        {
            Response<PreOrderAndPreOrderDetail> response = new Response<PreOrderAndPreOrderDetail>() { Result = new PreOrderAndPreOrderDetail() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("Proc_WMSAllocationGetWMS_PreOrderAndDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                int RowCount = 0;
                response.Result = new PreOrderAccessor().Allocation_GetPrdOrder(request.SearchCondition);
                //response.Result.PreO = new PreOrder();

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

        public Response<AddInventroyRequest> GetPrdOrder_distributionInventory(long PREID)
        {
            Response<AddInventroyRequest> response = new Response<AddInventroyRequest>() { Result = new AddInventroyRequest() };

            try
            {
                response.Result = new PreOrderAccessor().GetPrdOrder_distributionInventory(PREID);
                //response.Result.PreO = new PreOrder();

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PREID"></param>
        /// <param name="ProcName">存过名称</param>
        /// <returns></returns>
        public Response<AddInventroyRequest> GetPrdOrder_distributionInventory(long PREID, string ProcName)
        {
            Response<AddInventroyRequest> response = new Response<AddInventroyRequest>() { Result = new AddInventroyRequest() };

            try
            {
                response.Result = new PreOrderAccessor().GetPrdOrder_distributionInventory(PREID, ProcName);
                //response.Result.PreO = new PreOrder();

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
        public IEnumerable<Inventorys> GetBatchBySKU(string SKU, long? CustomerID, string Warehouse, string BatchNumber, string GoodsType, string BoxNumber, string Unit, string Specifications, string UPC)
        {
            IEnumerable<Inventorys> list = null;

            try
            {
                list = new PreOrderAccessor().GetBatchBySKU(SKU, CustomerID, Warehouse, BatchNumber, GoodsType, BoxNumber, Unit, Specifications, UPC);
                //response.Result.PreO = new PreOrder();


            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return list;
        }

        public Response<PreOrderAndPreOrderDetail> ManualAllocationJson(ManualAllocationRequest request)
        {
            Response<PreOrderAndPreOrderDetail> response = new Response<PreOrderAndPreOrderDetail> { Result = new PreOrderAndPreOrderDetail() };
            //Response<string> str=new Response<string>{ Result=""};
            //string str = "";
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("Proc_WMS_ManualAllocation request");

                return response;
            }
            try
            {
                response.Result.DisInfo = new PreOrderAccessor().ManualAllocationJson(request.PodRequest, request.ID, request.Creator, request.CustomerId, request.Criterion, request.SqlProc);

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                LogError(ex);

            }
            return response;
        }

        public string ManualAllocationSaveJson(ManualAllocationRequest request)
        {

            string message = "";

            try
            {
                message = new PreOrderAccessor().ManualAllocationSaveJson(request.PodRequest, request.ID, request.Creator);


            }
            catch (Exception ex)
            {
                message = ex.ToString();
                LogError(ex);

            }
            return message;
        }

        /// <summary>
        /// 现场分配
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<PreOrderAndPreOrderDetail> WorkersAlloctions(ManualAllocationRequest request)
        {
            Response<PreOrderAndPreOrderDetail> response = new Response<PreOrderAndPreOrderDetail> { Result = new PreOrderAndPreOrderDetail() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("Proc_WMS_ManualAllocation request");

                return response;
            }
            try
            {
                response.Result.DisInfo = new PreOrderAccessor().WorkersAlloctions(request.PodRequest, request.ID, request.Creator, request.CustomerId, request.Criterion, request.SqlProc);

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                LogError(ex);

            }
            return response;
        }

        public Response<PreOrderAndPreOrderDetail> AddPreOrderAndPreOrderDetail(PreOrderRequest request, string Creator)
        {
            Response<PreOrderAndPreOrderDetail> response = new Response<PreOrderAndPreOrderDetail> { Result = new PreOrderAndPreOrderDetail() };
            if (request == null || request.PreOd == null || !request.PreOd.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("sp_wms_GetSKU request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                PreOrderAccessor accessor = new PreOrderAccessor();
                response.Result = accessor.AddPreOrderAndPreOrderDetail(request.PreOrderList, request.PreOd, Creator);
                //response.Result = accessor.AddPreOrderAndPreOrderDetail(request.PreOrderList, request.PreOd, Creator);
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


        public Response<PreOrderAndPreOrderDetail> AddPreOrderAndPreOrderDetail(PreOrderRequest request, string Creator, string OperationType)
        {
            Response<PreOrderAndPreOrderDetail> response = new Response<PreOrderAndPreOrderDetail> { Result = new PreOrderAndPreOrderDetail() };
            if (request == null || request.PreOd == null || !request.PreOd.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("sp_wms_GetSKU request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                PreOrderAccessor accessor = new PreOrderAccessor();
                response.Result = accessor.AddPreOrderAndPreOrderDetail(request.PreOrderList, request.PreOd, Creator, OperationType);
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

        /// <summary>
        /// 动态调用不同数据库
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Creator"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public Response<PreOrderAndPreOrderDetail> AddPreOrderAndPreOrderDetailDynamicConn(IEnumerable<PreOrder> PreOrderList, IEnumerable<PreOrderDetail> PreDetail, string Creator, string conn)
        {
            Response<PreOrderAndPreOrderDetail> response = new Response<PreOrderAndPreOrderDetail> { Result = new PreOrderAndPreOrderDetail() };
            //if (request == null || request.PreOd == null || !request.PreOd.Any())
            //{
            //    ArgumentNullException ex = new ArgumentNullException("sp_wms_GetSKU request");
            //    LogError(ex);
            //    response.ErrorCode = ErrorCode.Argument;
            //    response.Exception = ex;
            //    return response;
            //}
            try
            {
                PreOrderAccessor accessor = new PreOrderAccessor();
                response.Result = accessor.AddPreOrderAndPreOrderDetailDynamicConn(PreOrderList, PreDetail, Creator, conn);
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

        public IEnumerable<PreOrder> GetWMSPreOrdersByExternNumber(IEnumerable<PreOrder> preorders, string connectionStr)
        {
            try
            {
                return new PreOrderAccessor().GetWMSPreOrdersByExternNumber(preorders, connectionStr);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public Response<string> AddInventoryOfOutbound(PreOrderRequest request, string Creator)
        {
            Response<string> response = new Response<string> { Result = "" };
            string str = "";
            if (request == null || request.PreOd == null || !request.PreOd.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("Proc_WMS_AutomatedOutbound request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;

            }
            try
            {
                PreOrderAccessor accessor = new PreOrderAccessor();
                int RowCount = 0;
                response.Result = accessor.AddInventoryOfOutbound(request.PreOrderList, request.PreOd, Creator);
                response.IsSuccess = true;
                //  response.Result = new PreOrderAccessor().GetPrdOrder(request.SearchCondition);
                //response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<PreOrderAndPreOrderDetail> CheckOutboundOrder(string Id)
        {
            Response<PreOrderAndPreOrderDetail> response = new Response<PreOrderAndPreOrderDetail>() { Result = new PreOrderAndPreOrderDetail() };
            try
            {
                PreOrderAccessor accessor = new PreOrderAccessor();
                response.Result = accessor.CheckOutboundOrder(Id);
                response.IsSuccess = true;
                //  response.Result = new PreOrderAccessor().GetPrdOrder(request.SearchCondition);
                //response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string Cancel(List<PreOrderBackStatus> preorderlist, string CustomerID, string reasonCode, string reasonRemark)// string Criterion  
        {
            string message = "";
            try
            {
                PreOrderAccessor accessor = new PreOrderAccessor();
                message = new PreOrderAccessor().Cancel(preorderlist, CustomerID, reasonCode, reasonRemark);//Criterion
            }
            catch (Exception ex)
            {
                LogError(ex);
                LogError(ex);

            }
            return message;
            //PreOrderAccessor accessor = new PreOrderAccessor();
            //var c = accessor.Cancel(ids);

            //return c;
        }

        /// <summary>
        /// 完成操作
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string OrderFinish(List<PreOrderBackStatus> preorderlist)// string Criterion  
        {
            string message = "";
            try
            {
                PreOrderAccessor accessor = new PreOrderAccessor();
                message = new PreOrderAccessor().OrderFinish(preorderlist);//Criterion
            }
            catch (Exception ex)
            {
                LogError(ex);
                LogError(ex);

            }
            return message;
            //PreOrderAccessor accessor = new PreOrderAccessor();
            //var c = accessor.Cancel(ids); 
            //return c;
        }
        /// <summary>
        /// 自动分配
        /// </summary>
        /// <param name="preorderlist"></param>
        /// <param name="ids"></param>
        /// <param name="Criterion"></param>
        /// <returns></returns>
        public Response<PreOrderAndPreOrderDetail> AutomaticAllocation(IEnumerable<PreOrderIds> preorderlist, string Creator, string Criterion, string SqlProc)
        {
            Response<PreOrderAndPreOrderDetail> response = new Response<PreOrderAndPreOrderDetail> { Result = new PreOrderAndPreOrderDetail() };
            if (preorderlist == null)
            {
                ArgumentNullException ex = new ArgumentNullException("Proc_WMS_AutomatedOutbound request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;

            }
            try
            {
                response.Result.DisInfo = new PreOrderAccessor().AutomaticAllocation(preorderlist, Creator, Criterion, SqlProc);
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

        public string AddShipRequestHeaderAndDetail(IEnumerable<WMS_ShipRequestHeader> header, IEnumerable<WMS_ShipRequestDetail> detail)
        {
            string message = "";
            try
            {
                message = new PreOrderAccessor().AddShipRequestHeaderAndDetail(header, detail);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return message;
        }

        public IEnumerable<Inventorys> GetOutboundInventory()
        {
            return new PreOrderAccessor().GetOutboundInventory();
        }


        public Response<PreOrderResponse> BatchIimportUpdateLoadKey(string CustomerID, PreOrderRequest request)
        {
            Response<PreOrderResponse> response = new Response<PreOrderResponse>();
            response.Result = new PreOrderResponse();        //结果集初始化
            response.SuccessMessage = new PreOrderAccessor().BatchIimportUpdateLoadKey(CustomerID, request.PreOrderList);
            if (response.SuccessMessage.Contains("更新成功"))
            {///根据数据库传出的值判断是否执行成功
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
            }
            return response;

            //Response<PreOrderAndPreOrderDetail> response = new Response<PreOrderAndPreOrderDetail> { Result = new PreOrderAndPreOrderDetail() };
            //if (request == null || request.PreOd == null)
            //{
            //    ArgumentNullException ex = new ArgumentNullException("sp_wms_GetSKU request");
            //    LogError(ex);
            //    response.ErrorCode = ErrorCode.Argument;
            //    response.Exception = ex;
            //    return response;
            //}
            //try
            //{
            //    PreOrderAccessor accessor = new PreOrderAccessor();
            //    response.Result = accessor.BatchIimportUpdateLoadKey(CustomerID, request.PreOrderList);
            //    //response.Result = accessor.AddPreOrderAndPreOrderDetail(request.PreOrderList, request.PreOd, Creator);
            //    response.IsSuccess = true;
            //}
            //catch (Exception ex)
            //{
            //    LogError(ex);
            //    response.IsSuccess = false;
            //    response.ErrorCode = ErrorCode.Technical;
            //}
            //return response;
        }

        public Response<AddASNAndASNDetailRequest> BatchIimportUpdateGoodsType(string CustomerID, AddASNAndASNDetailRequest request)
        {
            Response<AddASNAndASNDetailRequest> response = new Response<AddASNAndASNDetailRequest>();
            response.Result = new AddASNAndASNDetailRequest();        //结果集初始化
            response.SuccessMessage = new PreOrderAccessor().BatchIimportUpdateGoodsType(CustomerID, request.asnDetail);
            if (response.SuccessMessage.Contains("更新成功"))
            {///根据数据库传出的值判断是否执行成功
                response.IsSuccess = true;
            }
            else if (response.SuccessMessage == "失败，进行变更的数量与订单数量总和不符")
            {
                response.IsSuccess = false;
            }
            else
            {
                response.IsSuccess = false;
            }
            return response;


        }

        /// <summary>
        /// 查询取消单信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="rowcounts"></param>
        /// <returns></returns>    
        public IEnumerable<CancelOrderInfo> GetCancelOrderList(CancelOrderSearchCondition search, string Proc, out string msg, out int rowcounts)
        {
            msg = "";
            rowcounts = 0;
            try
            {
                return new PreOrderAccessor().GetCancelOrderList(search, Proc, out rowcounts);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                rowcounts = 0;
                return null;
            }
        }

        /// <summary>
        /// 验证loadkey是否存在
        /// </summary>
        /// <param name="preorders"></param>
        /// <returns></returns>
        public IEnumerable<PreOrder> GetWMSPreOrderlistByLoadKey(IEnumerable<PreOrder> preorders)
        {
            try
            {
                return new PreOrderAccessor().GetWMSPreOrderlistByLoadKey(preorders);
            }
            catch (Exception ex)
            {
                return null;
            }

        }


    }
}
