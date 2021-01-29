using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WebApi;
using Runbow.TWS.Entity.WebApi;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WebApi;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Biz.WebApi
{
    public class OrderService : BaseService
    {
        public IEnumerable<APIAndBackSetting> GetAPISetting(string UserName)
        {
            OrderServiceAccessor osa = new OrderServiceAccessor();
            try
            {
                return osa.GetAPISetting(UserName);
            }
            catch
            {
                return null;
            }
           // return IEnumerable<APIAndBackSetting>;
        }

        public Response<DataSet> GetType(string type)
        {
            Response<DataSet> response = new Response<DataSet>();
            if (type == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetType request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                int RowCount = 0;
                response.Result = new WXPODServiceAccessor().GetType(type);
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
