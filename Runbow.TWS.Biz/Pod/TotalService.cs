using Runbow.TWS.Common;
using Runbow.TWS.Dao.POD;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.POD.Total;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Biz.POD
{
    public class TotalService : BaseService
    {
        public Response<GetMessageHistoryQueryRequest> GetMessageHistoryInfo(GetMessageHistoryQueryRequest request)
        {
            Response<GetMessageHistoryQueryRequest> response = new Response<GetMessageHistoryQueryRequest>() { Result = new GetMessageHistoryQueryRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetMessageHistoryInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                TotalAccessor accessor = new TotalAccessor();
                int Rowcount;
                response.Result.MessageHistoryTable = accessor.GetMessageHistoryInfo(request.SqlWhere, request.PageIndex, request.PageSize, out Rowcount);
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = Rowcount % request.PageSize == 0 ? Rowcount / request.PageSize : Rowcount / request.PageSize + 1;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }




        public Response<GetMessageHistoryQueryRequest> GetMessageHistoryInfoReport(GetMessageHistoryQueryRequest request)
        {
            Response<GetMessageHistoryQueryRequest> response = new Response<GetMessageHistoryQueryRequest>() { Result = new GetMessageHistoryQueryRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetMessageHistoryInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                TotalAccessor accessor = new TotalAccessor();
                
                response.Result.MessageHistoryTable = accessor.GetMessageHistoryInfoReport(request.SqlWhere);
                
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }
          public Response<TotalPODResponse> GetTotalPODReport(GetTotalPODRequest request)
        {
            Response<TotalPODResponse> response = new Response<TotalPODResponse>() { Result = new TotalPODResponse() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("proc_Total_QueryPodHasTracks request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                TotalAccessor accessor = new TotalAccessor();
                int RowCount;
                response.Result.TotalPODCollection = accessor.GetPODTotalInfoReport(request.SearchCondition,  request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
                response.Result.RowCount = RowCount;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }

            return response;
        }
    
    }
    
}
