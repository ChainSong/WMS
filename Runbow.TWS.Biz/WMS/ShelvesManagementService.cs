
using System;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Shelves = Runbow.TWS.MessageContracts.WMS.Shelves;
using System.Collections.Generic;
using Runbow.TWS.MessageContracts.WMS.Shelves;
using Runbow.TWS.MessageContracts.WMS.Order;

namespace Runbow.TWS.Biz
{
    public class ShelvesManagementService:BaseService
    {
        /// <summary>
        /// 查询入库  （当上架表没有上架信息的时候再其他表查询）
        /// </summary>
        /// <returns></returns>
        public Response<Shelves.GetShelvesByConditionResponse> GetShelves(Shelves.GetShelvesByConditionRequest Request)
        {
            Response<Shelves.GetShelvesByConditionResponse> response = new Response<Shelves.GetShelvesByConditionResponse>() { Result = new Shelves.GetShelvesByConditionResponse() };
            try
            {
                ShelvesManagementAccessor accessor = new ShelvesManagementAccessor();

                response.Result = accessor.GetShelves(Request.SearchCondition);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                response.IsSuccess = false;
            }

            return response;
        }

        /// <summary>
        /// 查询入库 (当上架表有上架信息的时候查新上架表)
        /// </summary>
        /// <returns></returns>
        //public Response<Shelves.GetShelvesByConditionResponse> GetReceiptReceiving(Shelves.GetShelvesByConditionRequest Request)
        //{
        //    Response<Shelves.GetShelvesByConditionResponse> response = new Response<Shelves.GetShelvesByConditionResponse>() { Result = new Shelves.GetShelvesByConditionResponse() };
        //    try
        //    {
        //        ShelvesManagementAccessor accessor = new ShelvesManagementAccessor();

        //        response.Result = accessor.GetReceiptReceiving(Request.SearchCondition);
        //        response.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Exception = ex;
        //        response.IsSuccess = false;
        //    }

        //    return response;
        //}
        /// <summary>
        /// 上架
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public bool InsertReceiptReceiving(Shelves.GetShelvesByConditionRequest Request)
        {
            //Response<GetShelvesByConditionResponse> response = new Response<GetShelvesByConditionResponse>() { Result = new GetShelvesByConditionResponse() };

            bool IsSuccess = false;
            try
            {
                ShelvesManagementAccessor accessor = new ShelvesManagementAccessor();
                //response.Result.ReceiptReceiving
                IsSuccess = accessor.InsertReceiptReceiving(Request.receiptReceiving,Request.User);
                //response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                //response.Exception = ex;
                //response.IsSuccess = false;
            }
            return IsSuccess;
        }
        public Response<OutboundInstructionsResponse> AddInstructions(string Ids, string WorkStation, string ReleatedType, int Priority, string UserName)
        {
            Response<OutboundInstructionsResponse> response = new Response<OutboundInstructionsResponse>() { Result = new OutboundInstructionsResponse() };
            string message = "";
            try
            {
                ShelvesManagementAccessor accessor = new ShelvesManagementAccessor();
                message = accessor.AddInstructions(Ids, WorkStation, ReleatedType, Priority, UserName);
                if (message == "")
                {
                    response.IsSuccess = true;

                }
                else
                {
                    response.IsSuccess = false;
                    response.SuccessMessage = message;
                }
                
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
        /// Execl批量上架
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public Response<GetShelvesByConditionRequest> InsertReceiptReceivingExecl(Shelves.GetShelvesByConditionRequest Request)
        {
            Response<GetShelvesByConditionRequest> response = new Response<GetShelvesByConditionRequest>() { Result = new GetShelvesByConditionRequest() };

            string IsSuccess = "";
            try
            {
                ShelvesManagementAccessor accessor = new ShelvesManagementAccessor();
                //response.Result.ReceiptReceiving
                response.Result.receiptReceiving = accessor.InsertReceiptReceivingExecl(Request.receiptReceiving, Request.User);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                throw ex;
                response.IsSuccess = false;
            }
            return response;
        }
        /// <summary>
        /// 加入库存
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public bool AddshelvesAndInventory(Shelves.GetShelvesByConditionRequest Request)
        {
            //Response<GetShelvesByConditionResponse> response = new Response<GetShelvesByConditionResponse>() { Result = new GetShelvesByConditionResponse() };

            bool IsSuccess = false;
            try
            {
                ShelvesManagementAccessor accessor = new ShelvesManagementAccessor();
                //response.Result.ReceiptReceiving
                IsSuccess = accessor.AddshelvesAndInventory(Request.receiptReceiving,Request.User);
                //response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                //response.Exception = ex;
                //response.IsSuccess = false;
            }
            return IsSuccess;
        }
        public List<ReceiptDetail> CheckReceiving(string id)
        {
            List<ReceiptDetail> response = new List<ReceiptDetail>();

            ShelvesManagementAccessor accessor = new ShelvesManagementAccessor();
            try
            {
                response = accessor.CheckReceiving(id);
               
            }
            catch
            {
                response=null;
            }
            return response;
        }
        public List<ReceiptDetail> CheckRFReceiving(string id)
        {
            List<ReceiptDetail> response = new List<ReceiptDetail>();

            ShelvesManagementAccessor accessor = new ShelvesManagementAccessor();
            try
            {
                response = accessor.CheckRFReceiving(id);

            }
            catch
            {
                response = null;
            }
            return response;
        }
        /// <summary>
        /// 查询入库
        /// </summary>
        /// <returns></returns>
        public Response<Shelves.GetReceiptByConditionResponse> GetReceipt(Shelves.GetReceiptByConditionRequest request)
        {
            Response<Shelves.GetReceiptByConditionResponse> response = new Response<Shelves.GetReceiptByConditionResponse>() { Result = new Shelves.GetReceiptByConditionResponse() };
            try
            {
                ShelvesManagementAccessor accessor = new ShelvesManagementAccessor();
                int RowCount = 0;
                if (request.PageSize > 0)
                {
                    response.Result.receipt = accessor.GetReceipt(request.Condition, request.PageIndex, request.PageSize, out RowCount);
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;

                }
                else
                {
                    response.Result.PageIndex = 0;
                    response.Result.PageCount = 0;
                    response.Result.receipt = accessor.GetReceiptExecl(request.Condition);
                }
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                response.IsSuccess = false;
            }

            return response;
        }

        /// <summary>
        /// 导出所有已上架信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<Shelves.GetReceiptByConditionResponse> GetReceiptExport(Shelves.GetReceiptByConditionRequest request)
        {
            Response<Shelves.GetReceiptByConditionResponse> response = new Response<Shelves.GetReceiptByConditionResponse>() { Result = new Shelves.GetReceiptByConditionResponse() };
            try
            {
                ShelvesManagementAccessor accessor = new ShelvesManagementAccessor();
                if (request.PageSize > 0)
                {
                    response.Result.receipt = accessor.GetReceiptExport(request.Condition);

                }
                else
                {
                    response.Result.receipt = accessor.GetReceiptExport(request.Condition);
                }
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                response.IsSuccess = false;
            }

            return response;
        }

        public Response<Shelves.GetReceiptByConditionResponse> GetShelvesByIDs(string IDs)
        {
            Response<Shelves.GetReceiptByConditionResponse> response = new Response<Shelves.GetReceiptByConditionResponse>() { Result = new Shelves.GetReceiptByConditionResponse() };
            try
            {
                ShelvesManagementAccessor accessor = new ShelvesManagementAccessor();
                response.Result.receipt = accessor.GetShelvesByIDs(IDs);
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
        /// 比瑞吉定制
        /// </summary>
        /// <param name="receiptnumber"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string UpdateInventoryType(string receiptnumber, string action)
        {
            string IsSuccess = "";
            try
            {
                ShelvesManagementAccessor accessor = new ShelvesManagementAccessor();
                IsSuccess = accessor.UpdateInventoryType(receiptnumber, action);
            }
            catch (Exception)
            {
                IsSuccess = "失败";
            }
            return IsSuccess;
        }
        /// <summary>
        /// 加入库存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string AddInventory(string id,string UserName,string ProcName)
        {
            string IsSuccess = "";
            try
            {
                ShelvesManagementAccessor accessor = new ShelvesManagementAccessor();
                IsSuccess = accessor.AddInventory(id, UserName,ProcName);
            }
            catch (Exception)
            {
                IsSuccess = "失败"; 
            }
            return IsSuccess;
        }
        /// <summary>
        /// Nike差异加入库存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string AddInventoryWithFreeze(string id, string UserName)
        {
            string IsSuccess = "";
            try
            {
                ShelvesManagementAccessor accessor = new ShelvesManagementAccessor();
                IsSuccess = accessor.AddInventoryWithFreeze(id, UserName);
            }
            catch (Exception)
            {
                IsSuccess = "失败";
            }
            return IsSuccess;
        }
        /// <summary>
        /// 状态回退
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ToStatus"></param>
        /// <returns></returns>
        public Response<string> ReceiptStatusBack(AddReceiptAndReceiptDetailRequest request, int ToStatus)
        {
            Response<string> response = new Response<string>();
            string message = "";


            if (request == null || request.Receipts == null)
            {
                ArgumentNullException ex = new ArgumentNullException("Proc_WMS_ReceiptReceivingStatusBack request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                message = new ShelvesManagementAccessor().ReceiptStatusBack(request, ToStatus);
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
