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
    public class InvoiceService : BaseService
    {
        public Response<long> AddInvoice(AddInvoiceRequest request)
        {
            Response<long> response = new Response<long>();
            if (request == null || request.SettledPods == null || request.Invoice == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddInvoice request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                InvoiceAccessor accessor = new InvoiceAccessor();
                response.Result = accessor.AddInvoice(request.SettledPods, request.Invoice);
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

        public Response<bool> AddInvoiceAndPayOrders(AddInvoiceAndPayOrdersRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.SettledPodID == 0 || string.IsNullOrEmpty(request.ReceiveOrPayOrderNumber) || string.IsNullOrEmpty(request.InvoiceNumber) || string.IsNullOrEmpty(request.InvoiceSystemNumber) || request.TotalAmt == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("AddInvoiceAndPayOrders request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                InvoiceAccessor accessor = new InvoiceAccessor();
                accessor.AddInvoiceAndPayOrders(request.SettledPodID, request.Date, request.TotalAmt,request.InvoiceNumber, request.InvoiceSystemNumber,request.ReceiveOrPayOrderNumber, request.Remark, request.Creator);
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

        public Response<Invoice> GetInvoiceByID(GetInvoiceByIDRequest request)
        {
            Response<Invoice> response = new Response<Invoice>();
            if (request == null || request.ID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInvoiceByID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                InvoiceAccessor accessor = new InvoiceAccessor();
                response.Result = accessor.GetInvoiceByID(request.ID);
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

        public Response<IEnumerable<Invoice>> GetInvoicesByCondition(GetInvoicesByConditionRequest request)
        {
            Response<IEnumerable<Invoice>> response = new Response<IEnumerable<Invoice>>();
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInvoicesByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                InvoiceAccessor accessor = new InvoiceAccessor();
                response.Result = accessor.GetInvoicesByCondition(request.SearchCondition);
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

        public Response<bool> UpdateInvoiceNumber(UpdateInvoiceNumberRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.ID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateInvoiceNumber request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                InvoiceAccessor accessor = new InvoiceAccessor();
                accessor.UpdateInvoiceNumber(request.ID, request.InvoiceNumber);
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

        public Response<bool> DeleteInvoice(DeleteInvoiceRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.ID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("DeleteInvoice request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                InvoiceAccessor accessor = new InvoiceAccessor();
                accessor.DeleteInvoice(request.ID);
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
