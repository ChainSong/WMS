using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System.Data;
using Runbow.TWS.MessageContracts.AMS;
using System.Text;

namespace Runbow.TWS.Biz
{
    public class WXCustomerService : BaseService
    {
        /// <summary>
        /// 查询微信注册用户
        /// </summary>
        public Response<QueryWXCustomerResponses> GetQueryWXCustomer(QueryWXCustomerRequests request)
        {
            Response<QueryWXCustomerResponses> response = new Response<QueryWXCustomerResponses>() { Result = new QueryWXCustomerResponses() };
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("QueryWXCustomer request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                WXCustomerAccessor accessor = new WXCustomerAccessor();
                int RowCount;
                response.Result.WXCustomerCollection = accessor.GetQueryWXCustomer(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
        /// 微信用户审核
        /// </summary>
        public Response<int> UploadWXCustomer(QueryWXCustomerRequests request)
        {
            Response<int> response = new Response<int>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("UploadWXCustomer request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                WXCustomerAccessor accessor = new WXCustomerAccessor();
                response.Result = accessor.UploadWXCustomer(request.ID);
                if (response.Result == 1)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    if (response.Result == -1)
                    {
                        response.ErrorCode = ErrorCode.Technical;
                    }
                    else
                    {
                        response.ErrorCode = ErrorCode.Permission;
                    }
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

        /// <summary>
        /// 查询AccessToken
        /// </summary>
        public WXAccessToken GetQueryWXAccessToken()
        {
            return new WXCustomerAccessor().GetQueryWXAccessToken();
        }

        /// <summary>
        /// 更新最新AccessToken
        /// </summary>
        /// <param name="wx"></param>
        public void UploadWXAccessToken(WXAccessToken wx)
        {
            (new WXCustomerAccessor()).UploadWXAccessToken(wx);
        }

        /// <summary>
        /// 查询未生成二维码的PODID
        /// </summary>
        public IEnumerable<WXPODBarCode> GetPodBarCode(string podIds)
        {
            return new WXCustomerAccessor().GetPodBarCode(podIds);
        }


        /// <summary>
        /// 查询最大TicketID
        /// </summary>
        public WXPODBarCode GetWXTicketID()
        {
            return new WXCustomerAccessor().GetWXTicketID();
        }


        /// <summary>
        /// 新增微信二维码配置表
        /// </summary>
        public void AddWXPODBarCode(WXPODBarCode pb)
        {
            new WXCustomerAccessor().AddWXPODBarCode(pb);
        }
    }
}
