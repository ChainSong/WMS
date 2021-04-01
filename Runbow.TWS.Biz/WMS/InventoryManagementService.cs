using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.Inventory;
using Runbow.TWS.Entity;
using System.Data;

namespace Runbow.TWS.Biz.WMS
{
    public class InventoryManagementService : BaseService
    {
        public string Unboxing_akzo(string IDS, float UnboxingQty, string Creator, IList<PreOrderDetail> ToSKUJson)
        {
            string message = "";
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                message = accessor.Unboxing_akzo(IDS, UnboxingQty, Creator, ToSKUJson);
            }
            catch (Exception ex)
            {
                LogError(ex);
                message = ex.Message;
            }
            return message;
        }
        public Response<GetInventoryBySearchConditionResponse> GetInventoryBySearchCondition(GetInventoryBySearchConditionRequest request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            if (request == null && request.InventorySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInventoryBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                int RowCount;
                response.Result.InventoryCollection = accessor.GetInventoryBySearchCondition(request.InventorySearchCondition, request.PageIndex, request.PageSize, out RowCount);
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
        public Response<GetInventoryBySearchConditionResponse> GetInventoryBySearchConditionGroup(GetInventoryBySearchConditionRequest request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            if (request == null && request.InventorySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInventoryBySearchConditionGroupRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                int RowCount;
                response.Result = accessor.GetInventoryBySearchConditionGroup(request.InventorySearchCondition, request.PageIndex, request.PageSize, out RowCount);
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
        /// 库存快照查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetInventoryBySearchConditionResponse> GetInventorySnapshootBySearchCondition(GetInventoryBySearchConditionRequest request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            if (request == null && request.InventorySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInventoryBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                int RowCount;
                response.Result.InventorySnapCollection = accessor.GetInventorySnapshootBySearchCondition(request.InventorySearchCondition, request.PageIndex, request.PageSize, out RowCount);
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
        /// 库存快照导出
        /// </summary>
        /// <param name="request">时间</param>
        /// <returns></returns>
        public Response<GetInventoryBySearchConditionResponse> ExportInventorySnapshootBySearchCondition(GetInventoryBySearchConditionRequest request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            if (request == null && request.InventorySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInventoryBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                response.Result.InventorySnapCollection = accessor.ExportInventorySnapshootBySearchCondition(request.InventorySearchCondition);
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

        public Response<GetInventoryBySearchConditionResponse> GetInventoryByIDS(string IDS)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                response.Result.InventoryCollection = accessor.GetInventoryByIDS(IDS);
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
        public Response<GetInventoryBySearchConditionResponse> InventoryRemaining(InventorySearchCondition Condition)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            //if (request == null && request.InventorySearchCondition == null)
            //{
            //    ArgumentNullException ex = new ArgumentNullException("GetInventoryBySearchConditionRequest request");
            //    LogError(ex);
            //    response.ErrorCode = ErrorCode.Argument;
            //    response.Exception = ex;
            //    return response;
            //}
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                //int RowCount;
                response.Result.directAddInventory = accessor.InventoryRemaining(Condition);
                //response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                //response.Result.PageIndex = request.PageIndex;
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
        public Response<GetInventoryBySearchConditionResponse> TotalReport(InventorySearchCondition Condition)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                //int RowCount;
                response.Result = accessor.TotalReport(Condition);
                //response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                // response.Result.PageIndex = request.PageIndex;
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
        public Response<GetInventoryBySearchConditionResponse> dailyReport(InventorySearchCondition Condition)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                //int RowCount;
                // response.Result = accessor.dailyReport(Condition);
                response.Result = accessor.dailyReport(Condition);
                //response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                //response.Result.PageIndex = request.PageIndex;
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
        public Response<GetInventoryBySearchConditionResponse> detailReport(InventorySearchCondition Condition)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                //int RowCount;
                // response.Result = accessor.dailyReport(Condition);
                response.Result = accessor.detailReport(Condition);
                //response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                //response.Result.PageIndex = request.PageIndex;
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
        //获取要上传的库存
        public Response<GetInventoryBySearchConditionResponse> GetConfirmInventory()
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };


            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();

                response.Result = accessor.GetConfirmInventory();

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

        public Response<GetInventoryBySearchConditionResponse> InventorydDtails(string CustomerId, string ProduceType, DateTime? Date)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            //if (request == null && request.InventorySearchCondition == null)
            //{
            //    ArgumentNullException ex = new ArgumentNullException("GetInventoryBySearchConditionRequest request");
            //    LogError(ex);
            //    response.ErrorCode = ErrorCode.Argument;
            //    response.Exception = ex;
            //    return response;
            //}
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                //int RowCount;
                response.Result.directAddInventory = accessor.InventorydDtails(CustomerId, ProduceType, Date);
                //response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                //response.Result.PageIndex = request.PageIndex;
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
        public string DirectAddInventoryImports(GetInventoryBySearchConditionRequest request)
        {
            //Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };
            string IsSuccess = "";
            if (request == null && request.InventorySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInventoryBySearchConditionRequest request");
                LogError(ex);
                //response.ErrorCode = ErrorCode.Argument;
                //response.Exception = ex;

            }
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                //int RowCount;
                IsSuccess = accessor.DirectAddInventoryImports(request.directAddInventory);
            }
            catch (Exception ex)
            {
                LogError(ex);
                //response.Exception = ex;
                //response.IsSuccess = false;
                //response.ErrorCode = ErrorCode.Technical;
            }

            return IsSuccess;

        }

        public Response<GetInventoryBySearchConditionResponse> SupplyChainReport()
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };
            string IsSuccess = "";

            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                //int RowCount;
                response.Result.directAddInventory = accessor.SupplyChainReport();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                //response.Exception = ex;
                //response.IsSuccess = false;
                //response.ErrorCode = ErrorCode.Technical;
            }

            return response;

        }
        public bool DelDirectAddInventory(string Id)
        {

            bool IsSuccess = false;
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                //int RowCount;
                IsSuccess = accessor.DelDirectAddInventory(Id);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return IsSuccess;

        }
        public Response<GetInventoryBySearchConditionResponse> GetInventoryRecord(GetInventoryBySearchConditionRequest request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            if (request == null && request.InventorySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInventoryBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                int RowCount;
                response.Result.InventoryCollection = accessor.GetInventoryRecord(request.InventorySearchCondition, request.PageIndex, request.PageSize, out RowCount);
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

        public Response<GetInventoryBySearchConditionResponse> GetInventoryByLocation(string Warehouse, string CustomerID, string location)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                response.Result = accessor.GetInventoryByLocation(Warehouse, CustomerID, location);
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

        public Response<GetInventoryBySearchConditionResponse> GetInventoryBySKU(string Warehouse, string CustomerID, string SKU)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                response.Result = accessor.GetInventoryBySKU(Warehouse, CustomerID, SKU);
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

        public Response<GetInventoryBySearchConditionResponse> GetPrintByAdjust(string AdjustNumber)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();
                response.Result = accessor.GetPrintByAdjust(AdjustNumber);
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
        public Response<GetInventoryBySearchConditionResponse> GetAkzoInventoryMoveConfirm()
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };


            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();

                response.Result = accessor.GetAkzoInventoryMoveConfirm();

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
        public Response<GetInventoryBySearchConditionResponse> GetInventoryWarning()
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };


            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();

                response.Result = accessor.GetInventoryWarning();

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
        

        public Response<GetInventoryBySearchConditionResponse> InventoryCompare(DataSet ds, User u, out string message)
        {
            message = "";
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };
            try
            {
                InventoryManagementAccessor accessor = new InventoryManagementAccessor();

                response.Result = accessor.InventoryCompare(ds, u, out message);
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

        public DataTable GetNikeCEInventorySnapshot()
        {
            InventoryManagementAccessor accessor = new InventoryManagementAccessor();
            return accessor.GetNikeCEInventorySnapshot();
        }
        /// <summary>
        /// 修改批次
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<string> UpdateInventoryBatch(AddInventroyRequest request)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                message = new InventoryManagementAccessor().UpdateInventoryBatch(request);
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
    }
}
