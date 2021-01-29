using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz
{
    public class ReceiveOrPayOrdersService : BaseService
    {
        public Response<bool> CompleteOrCancelInvoice(CompleteOrCancelInvoiceRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.ID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("CompleteOrCancelInvoice request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ReceiveOrPayOrdersAccessor accessor = new ReceiveOrPayOrdersAccessor();
                accessor.CompleteOrCancelInvoice(request.ID, request.CurrentCompleteState);
                response.Result = true;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<bool> DeleteReceiveOrPayOrder(DeleteReceiveOrPayOrderRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.ID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("DeleteReceiveOrPayOrder request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ReceiveOrPayOrdersAccessor accessor = new ReceiveOrPayOrdersAccessor();
                accessor.DeleteReceiveOrPayOrder(request.ID);
                response.Result = true;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        
        public Response<long> AddReceiveOrPayOrders(AddReceiveOrPayOrdersRequest request)
        {
            Response<long> response = new Response<long>();
            if (request == null || request.ReceiveOrPayOrders == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddReceiveOrPayOrders request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ReceiveOrPayOrdersAccessor accessor = new ReceiveOrPayOrdersAccessor();
                response.Result = accessor.AddReceiveOrPayOrders(request.ReceiveOrPayOrders);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<IEnumerable<ReceiveOrPayOrders>> GetReceiveOrPayOrderByInvoiceID(GetReceiveOrPayOrderByInvoiceIDRequest request)
        {
            Response<IEnumerable<ReceiveOrPayOrders>> response = new Response<IEnumerable<ReceiveOrPayOrders>>();
            if (request == null || request.InvoiceID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReceiveOrPayOrderByInvoiceID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ReceiveOrPayOrdersAccessor accessor = new ReceiveOrPayOrdersAccessor();
                response.Result = accessor.GetReceiveOrPayOrderByInvoiceID(request.InvoiceID);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
    }
}
