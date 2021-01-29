using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Dao.WMS;

namespace Runbow.TWS.Biz.WMS
{
    public class CordOperationService : BaseService
    {
        public Response<string> AddCordOperation(IEnumerable<WMS_Cord_Operation> list)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {
                CordOperationAccessor accessor = new CordOperationAccessor();
                message = accessor.AddCordOperation(list);
                if (message.Contains("反馈成功"))
                {
                    response.Result = message;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = message;
                    response.IsSuccess = false;
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
    }
}
