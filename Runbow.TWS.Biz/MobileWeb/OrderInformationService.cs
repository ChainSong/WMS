
using System;
using System.Collections.Generic;
using Runbow.TWS.Dao.MobilePod;
using Runbow.TWS.Entity.MobilePOD;
using Runbow.TWS.Entity.MobileWeb;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.MobilePOD;
namespace Runbow.TWS.Biz.MobilePOD
{
    public class OrderInformationService : BaseService
    {
        /// <summary>
        /// 获取2月内已上传回单
        /// </summary>
        public Response<QueryOrderInformationResponses> GetQueryInformation(QueryOrderInformationRequests request)
        {
            Response<QueryOrderInformationResponses> response = new Response<QueryOrderInformationResponses>() { Result = new QueryOrderInformationResponses() };
            try
            {  // GetQueryInformation(string request, int PageIndex, int PageSize, out int RowCount)
                int RowCount = 0;
                response.Result.orderManagement = new GetOrderInformationAccessor().GetQueryInformations(request.UserType, request.permissions, request.conditions, request.PageIndex, request.PageSize, out RowCount);//request.ShipperId,
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                //response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }
        public Response<OrderManagementInfo> QueryOrderInformation(string id)
        {
            Response<OrderManagementInfo> response = new Response<OrderManagementInfo> { Result = new OrderManagementInfo() };
            try
            {  // GetQueryInformation(string request, int PageIndex, int PageSize, out int RowCount)
                int RowCount = 0;
                response.Result = new GetOrderInformationAccessor().QueryOrderInformation(id);

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                //response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }

        public Response<IEnumerable<PodStatusTrack>> addGPS(string id, string SystemNumber, string CustomerOrderNumber, string address, string lng, string lat)
        {
            Response<IEnumerable<PodStatusTrack>> response = new Response<IEnumerable<PodStatusTrack>>();
            //  bool Success = false;
            try
            {  // GetQueryInformation(string request, int PageIndex, int PageSize, out int RowCount)

                response.Result = new GetOrderInformationAccessor().addGPS(id, SystemNumber, CustomerOrderNumber, address, lng, lat);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                //response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }
        public Response<OrderManagementInfo> PODInformation(string id)
        {
            Response<OrderManagementInfo> response = new Response<OrderManagementInfo>();
            //  bool Success = false;
            try
            {  // GetQueryInformation(string request, int PageIndex, int PageSize, out int RowCount)

                response.Result = new GetOrderInformationAccessor().PODInformation(id);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                //response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }
        public bool InsertImg(IEnumerable<InsetrOrderImg> request)
        {
            bool Result = false;
            try
            {
                Result = new GetOrderInformationAccessor().InsertImg(request);
                bool IsSuccess = true;
            }
            catch (Exception ex)
            {
                Result = false;
                LogError(ex);

            }
            return Result;
        }
    }
}
