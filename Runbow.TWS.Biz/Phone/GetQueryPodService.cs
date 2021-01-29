using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Dao;
using Runbow.TWS.Biz;
using Runbow.TWS.Common;
namespace Runbow.TWS.Biz
{
    public class GetQueryPodService : BaseService
    {
        public Response<WinQueryPodResponses> WeiQueryPodService(WinQueryPodRequest request)
        {
            Response<WinQueryPodResponses> response = new Response<WinQueryPodResponses>() { Result = new WinQueryPodResponses() };
            try
            {
                int RowCount;
                response.Result.WeiQueryPods = new WeiQueryPodAccessor().GetWeiQueryPodInfo(request.Id, request.WeiQueryPods, request.Type, request.PageIndex, request.PageSize, out  RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
        public string PingFen(string Id, int ping, string ValFrom)
        {
            try
            {
                string Pin = new WeiQueryPodAccessor().PingFen(Id, ping, ValFrom);
            }
            catch (Exception ex)
            {
                LogError(ex);

            }
            return "";
        }
    }

}