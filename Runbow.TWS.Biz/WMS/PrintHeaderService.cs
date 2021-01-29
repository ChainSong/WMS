using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.PreOrders;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.Print;

namespace Runbow.TWS.Biz.WMS
{
    public class PrintHeaderService : BaseService
    {
        public Response<GetPrintByConditionResponse> GetPrintHeaderByCondition(GetPrintByConditionRequest request)
        {
            Response<GetPrintByConditionResponse> response = new Response<GetPrintByConditionResponse>() { Result = new GetPrintByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetWaveByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PrintHeaderAccessor accessor = new PrintHeaderAccessor();
                int RowCount;
                response.Result = accessor.GetPrintHeaderByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
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

        public Response<GetPrintByConditionResponse> GetPrintHeaderAndDetailByID(int ID)
        {
            Response<GetPrintByConditionResponse> response = new Response<GetPrintByConditionResponse>() { Result = new GetPrintByConditionResponse() };


            try
            {
                PrintHeaderAccessor accessor = new PrintHeaderAccessor();
                int RowCount;
                response.Result = accessor.GetPrintHeaderAndDetailByID(ID);
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

        public Response<GetPrintByConditionResponse> CreateOrUpdatePrintHeaderAndDetail(int CustomerID, string CustomerName, int WarehouseID, string WarehouseName, string Creator, IEnumerable<PreOrderIds> ids, int PrintID, string PrintKey)
        {
            Response<GetPrintByConditionResponse> response = new Response<GetPrintByConditionResponse>() { Result = new GetPrintByConditionResponse() };



            try
            {
                PrintHeaderAccessor accessor = new PrintHeaderAccessor();
                response.Result = accessor.CreateOrUpdatePrintHeaderAndDetail(CustomerID, CustomerName, WarehouseID, WarehouseName, Creator, ids, PrintID, PrintKey);
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

        //public Response<GetPrintByConditionResponse> RelateExpressKey(int ID, string OrderKey, string ExpressKey, string Updator)
        //{
        //    Response<GetPrintByConditionResponse> response = new Response<GetPrintByConditionResponse>() { Result = new GetPrintByConditionResponse() };


        //    try
        //    {
        //        PrintHeaderAccessor accessor = new PrintHeaderAccessor();
        //        response.Result = accessor.RelateExpressKey(ID, OrderKey, ExpressKey, Updator);
        //        response.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //        response.Exception = ex;
        //        response.IsSuccess = false;
        //        response.ErrorCode = ErrorCode.Technical;
        //    }

        //    return response;
        //}


        public string RelateExpressKey(int ID, string OrderKey, string ExpressKey, string Updator)
        {
            string result = "";


            try
            {
                PrintHeaderAccessor accessor = new PrintHeaderAccessor();
                result = accessor.RelateExpressKey(ID, OrderKey, ExpressKey, Updator);

            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }

            return result;
        }

        public string UpdatePrintStatus(string Ids)
        {
            string resualt = "";
            try
            {
                PrintHeaderAccessor accessor = new PrintHeaderAccessor();
                resualt = accessor.UpdatePrintStatus(Ids);
            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return resualt;
        }


        ///// <summary>
        ///// 验证是否超过波次大小
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public int VerifyWaveSize(long HeaderID, int Type, out string msg)
        //{
        //    msg = "";
        //    try
        //    {
        //        PrintHeaderAccessor accessor = new PrintHeaderAccessor();
        //        int qty = accessor.VerifyWaveSize(HeaderID, Type);
        //        return qty;
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.Message.ToString();
        //        return 0;
        //    }

        //}

        /// <summary>
        /// 将当前波次明细查询出来
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrderInfo> GetOrderInfoByPrintID(long HeaderID, string ids)
        {
            try
            {
                return new PrintHeaderAccessor().GetOrderInfoByPrintID(HeaderID, ids);

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
