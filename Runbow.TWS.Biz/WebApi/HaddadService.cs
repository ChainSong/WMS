using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WebApi;
using Runbow.TWS.Entity.WebApi;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WebApi;

namespace Runbow.TWS.Biz.WebApi
{
    public class HaddadService : BaseService
    {
        public Response<OrderStatus> GetOrderStatus(GetOrderStatusRequest request)
        {
            Response<OrderStatus> response = new Response<OrderStatus>();
            if (request == null || request.OrderCancel == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetOrderStatus request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                int RowCount = 0;
                response.Result = new HaddadServiceAccessor().GetOrderStatus(request.OrderCancel);
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
    }
}
