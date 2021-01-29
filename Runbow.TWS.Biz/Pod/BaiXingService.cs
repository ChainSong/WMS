using Runbow.TWS.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.MessageContracts.POD.Nike;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using System.Data;
using Runbow.TWS.Dao.POD;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.POD;

namespace Runbow.TWS.Biz.POD
{
    /// <summary>
    /// 百姓网项目订单操作
    /// </summary>
    public class BaiXingService : BaseService
    {
        /// <summary>
        /// 获取百姓网订单信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<IEnumerable<Pod>> GetBaiXingPod(GetBaiXingRequest request)
        {
            Response<IEnumerable<Pod>> response = new Response<IEnumerable<Pod>>();
            if (request == null || string.IsNullOrEmpty(request.CustomerOrderNumber))
            {
                ArgumentNullException ex = new ArgumentNullException("GetBaiXingPod request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            BaiXingAccessor accessor = new BaiXingAccessor();

            try
            {
                response.Result = accessor.GetBaiXingPod(request.CustomerOrderNumber);
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

        /// <summary>
        /// 根据运单号获取订单跟踪信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<IEnumerable<PodTrack>> GetPodTracksByCustomerOrderNumber(string request)
        {
            Response<IEnumerable<PodTrack>> response = new Response<IEnumerable<PodTrack>>();
            if (string.IsNullOrEmpty(request))
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodTracksByCustomerOrderNumber request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                BaiXingAccessor accessor = new BaiXingAccessor();
                response.Result = accessor.GetPodTracksByCustomerOrderNumber(request);
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

        /// <summary>
        /// 百姓网订单导入顺丰快递信息
        /// </summary>
        public Response<IEnumerable<PodKey>> AddPods_BX(AddPodsRequest request)
        {
            Response<IEnumerable<PodKey>> response = new Response<IEnumerable<PodKey>>();

            if (request == null || request.Pods == null || !request.Pods.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddPods request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                BaiXingAccessor accessor = new BaiXingAccessor();
                response.Result = accessor.AddPods_BX(request.Pods);
                if (response.Result.Count() == 0)
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
                }
                else
                {
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }


        public Response<QueryPodResponse> QueryPod(QueryPodRequest request)
        {
            Response<QueryPodResponse> response = new Response<QueryPodResponse>() { Result = new QueryPodResponse() };
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("QueryPod request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                BaiXingAccessor accessor = new BaiXingAccessor();
                int rowCount;
                response.Result.PodCollections = accessor.QueryPod(request.SearchCondition, request.ProjectID, request.PageIndex, request.PageSize, out rowCount).ToList().Distinct(new Runbow.TWS.Biz.PodService.ComparePod());
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = rowCount % request.PageSize == 0 ? rowCount / request.PageSize : rowCount / request.PageSize + 1;
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



        public DataTable ExportAll(QueryPodRequest request)
        {
            DataTable dt = new DataTable();
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("ExportAll request");
                LogError(ex);
                return null;
            }

            try
            {
                BaiXingAccessor accessor = new BaiXingAccessor();
                dt = accessor.ExportAll(request.SearchCondition, request.ProjectID, request.PageIndex, request.PageSize);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return dt;
        }
    }
}
