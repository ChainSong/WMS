using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Dao;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.Reports;
using Runbow.TWS.Common;

namespace Runbow.TWS.Biz
{
    public class WMSOrderStatusService : BaseService
    {
        public Response<WMSOrderStatusResponse> QueryWMSOrderRange(WMSOrderStatusRequest request)
        {
            Response<WMSOrderStatusResponse> response = new Response<WMSOrderStatusResponse>() { Result = new WMSOrderStatusResponse() };

            try
            {
                WMSOrderStatusAccessor accessor = new WMSOrderStatusAccessor();
                int RowCount;
                response.Result.WMSOrderCollection = accessor.QueryWMSOrderRange(request);
                //response.Result.PageIndex = 1;
                //response.Result.transOrder = accessor.QueryTransOrderRange(request, out RowCount);
                //response.Result.PageIndex
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
    }
}
