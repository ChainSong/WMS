using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.Order;

namespace Runbow.TWS.Biz.WMS
{
    public class WaveService : BaseService
    {
        public string CreateWave(string IsSinglePriece, string IsExpressCompany, string CustomerID, string CustomerName, string WarehouseID, string WarehouseName, string WaveCount, string Creator)
        {
            string message = "";
            try
            {
                WaveAccessor accessor = new WaveAccessor();
                message = accessor.CreateWave(IsSinglePriece, IsExpressCompany, CustomerID, CustomerName, WarehouseID, WarehouseName, WaveCount, Creator);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return message;
        }

        public Response<GetWaveByConditionResponse> GetWaveHeaderByCondition(GetWaveByConditionRequest request)
        {
            Response<GetWaveByConditionResponse> response = new Response<GetWaveByConditionResponse>() { Result = new GetWaveByConditionResponse() };

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
                WaveAccessor accessor = new WaveAccessor();
                int RowCount;
                response.Result = accessor.GetWaveHeaderByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
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

        public Response<GetWaveByConditionResponse> GetWaveHeaderAndDetail(int ID)
        {
            Response<GetWaveByConditionResponse> response = new Response<GetWaveByConditionResponse>() { Result = new GetWaveByConditionResponse() };
            try
            {
                WaveAccessor accessor = new WaveAccessor();
                response.Result = accessor.GetWaveHeaderAndDetail(ID);
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
