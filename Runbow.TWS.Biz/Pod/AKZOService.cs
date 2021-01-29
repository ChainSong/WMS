using Runbow.TWS.Common;
using Runbow.TWS.Dao.POD;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.POD.Adidas;
using Runbow.TWS.MessageContracts.POD.AKZO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Biz.POD
{
    public class AKZOService : BaseService
    {
        public Response<GetAbnormalPODSearchRequest> GetAbnormalPODSearch(GetAbnormalPODSearchRequest request)
        {
            Response<GetAbnormalPODSearchRequest> response = new Response<GetAbnormalPODSearchRequest>() { Result = new GetAbnormalPODSearchRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetAbnormalPODSearch request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                AKZOAccessor accessor = new AKZOAccessor();
                int Rowcount;
                response.Result.AbnormalTable = accessor.GetAbnormalPODSearch(request.SqlWhere, request.PageIndex, request.PageSize, out Rowcount);
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


        public Response<IEnumerable<Pod>> GetAbnormalPODSearchToExcel(GetAbnormalPODSearchRequest request)
        {
            Response<IEnumerable<Pod>> response = new Response<IEnumerable<Pod>>() { Result = Enumerable.Empty<Pod>() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetAbnormalPODSearchToExcel request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                AKZOAccessor accessor = new AKZOAccessor();

                response.Result = accessor.GetAbnormalPODSearchToExcel(request.SqlWhere);

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











        public Response<IEnumerable<long>> GetAbnormalPODTrackSearchToExcel(GetAbnormalPODSearchRequest request)
        {
            Response<IEnumerable<long>> response = new Response<IEnumerable<long>>() { Result = Enumerable.Empty<long>() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetAbnormalPODTrackSearchToExcel request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                AKZOAccessor accessor = new AKZOAccessor();

                response.Result = accessor.GetAbnormalPODTrackSearchToExcel(request.SqlWhere);

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

        public Response<UpdateAkzoPodAndGetTheDifferenceResponse> UpdateAkzoPodAndGetTheDifference(UpdateAkzoPodAndGetTheDifferenceRequest request)
        {
            Response<UpdateAkzoPodAndGetTheDifferenceResponse> response = new Response<UpdateAkzoPodAndGetTheDifferenceResponse>() { Result = new UpdateAkzoPodAndGetTheDifferenceResponse() };

            if (request == null || request.Pods == null || !request.Pods.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateAkzoPodAndGetTheDifference request Pods");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                AKZOAccessor accessor = new AKZOAccessor();

                response.Result = accessor.UpdateAkzoPodAndGetTheDifference(request.Pods);

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

        //苏州阿迪运单信息同步
        public Response<UpdateAdidasPodAndGetTheDifferenceResponse> UpdateAdidasPodAndGetTheDifference(UpdateAdidasPodAndGetTheDifferenceRequest request)
        {
            Response<UpdateAdidasPodAndGetTheDifferenceResponse> response = new Response<UpdateAdidasPodAndGetTheDifferenceResponse>() { Result = new UpdateAdidasPodAndGetTheDifferenceResponse() };

            if (request == null || request.PodAD == null || !request.PodAD.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateAdidasPodAndGetTheDifference request Pods");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                AKZOAccessor accessor = new AKZOAccessor();

                response.Result = accessor.UpdateAdidasPodAndGetTheDifference(request.PodAD);

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
        //广州阿迪运单信息同步
        public Response<UpdateAdidasPurchasePodAndGetTheDifferenceResponse> UpdateAdidasPurchasePodAndGetTheDifference(UpdateAdidasPurchasePodAndGetTheDifferenceRequest request)
        {
            Response<UpdateAdidasPurchasePodAndGetTheDifferenceResponse> response = new Response<UpdateAdidasPurchasePodAndGetTheDifferenceResponse>() { Result = new UpdateAdidasPurchasePodAndGetTheDifferenceResponse() };

            if (request == null || request.PodAD == null || !request.PodAD.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateAdidasPodAndGetTheDifference request Pods");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                AKZOAccessor accessor = new AKZOAccessor();

                response.Result = accessor.UpdateAdidasPurchasePodAndGetTheDifference(request.PodAD);

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
        //车型同步
        public Response<UpdateAkzoModelsPodAndGetTheDifferenceResponse> UpdateAkzoModelsPodAndGetTheDifference(UpdateAkzoModelsPodAndGetTheDifferenceRequest request)
        {
            Response<UpdateAkzoModelsPodAndGetTheDifferenceResponse> response = new Response<UpdateAkzoModelsPodAndGetTheDifferenceResponse>() { Result = new UpdateAkzoModelsPodAndGetTheDifferenceResponse() };

            if (request == null || request.Pods == null || !request.Pods.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateAkzoModelsPodAndGetTheDifference request Pods");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                AKZOAccessor accessor = new AKZOAccessor();

                response.Result = accessor.UpdateAkzoModelsPodAndGetTheDifference(request.Pods);

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
