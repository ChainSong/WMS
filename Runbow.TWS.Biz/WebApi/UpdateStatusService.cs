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
    public class UpdateStatusService : BaseService
    {
        public Response<string> GetUpdateStatus(string OrderKey)
        {
            Response<string> response = new Response<string>();
            if (OrderKey == null )
            {
                ArgumentNullException ex = new ArgumentNullException("GetUpdateStatus request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                int RowCount = 0;
                response.Result=new UpdateStatusServiceAccessor().GetUpdateStatus(OrderKey);
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
