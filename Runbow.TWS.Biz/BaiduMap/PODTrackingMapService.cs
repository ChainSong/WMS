using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Dao;
using Runbow.TWS.Dao.BaiduMap;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.BaiduMap;

namespace Runbow.TWS.Biz.BaiduMap
{
    public class PODTrackingMapService : BaseService
    {
        public Response<PODTrackingMapResponse> GPSALLGlobalTracking (PODTrackingMapRequest request)
        {
            Response<PODTrackingMapResponse> response = new Response<PODTrackingMapResponse>() { Result =new PODTrackingMapResponse()}; 
            try
            {
                PODTrackingMapAccessor accessor = new PODTrackingMapAccessor();
                response.Result = accessor.GPSALLGlobalTracking(request);
 
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex; 
            }

            return response;
        }

        public Response<PODTrackingMapResponse> GetCarInfoPOD(PODTrackingMapRequest request)
        {
            Response<PODTrackingMapResponse> response = new Response<PODTrackingMapResponse>() { Result =new PODTrackingMapResponse()}; 
            try
            {
                PODTrackingMapAccessor accessor = new PODTrackingMapAccessor();
                response.Result.PODTrackingMap = accessor.GetCarInfoPOD(request); 
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex; 
            }

            return response;
        }
        public Response<PODTrackingMapResponse> PartialPODView(PODTrackingMapRequest request)
        {
            Response<PODTrackingMapResponse> response = new Response<PODTrackingMapResponse>() { Result = new PODTrackingMapResponse() };
            try
            {
                PODTrackingMapAccessor accessor = new PODTrackingMapAccessor();
                response.Result = accessor.PartialPODView(request);

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex;
            }

            return response;
        }
    }
}
