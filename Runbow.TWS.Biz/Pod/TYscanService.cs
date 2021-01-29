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
    public class TYscanService : BaseService
    {
        /// <summary>
        /// 查询天翼扫描记录
        /// </summary>
        public Response<QueryTYscanResponses> GetQueryTYscan(QueryTYscanRequests request)
        {
            Response<QueryTYscanResponses> response = new Response<QueryTYscanResponses>() { Result = new QueryTYscanResponses() };
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetQueryTYscan request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                TYscanAccessor accessor = new TYscanAccessor();
                int RowCount;
                response.Result.TYscanCollection = accessor.GetQueryTYscan(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
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
        /// 查询天翼同步记录汇总
        /// </summary>
        public Response<QueryTYscanResponses> GetQueryTYscanGroupBy(QueryTYscanRequests request)
        {
            Response<QueryTYscanResponses> response = new Response<QueryTYscanResponses>() { Result = new QueryTYscanResponses() };
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("Proc_QueryTYscanGroupBy request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                TYscanAccessor accessor = new TYscanAccessor();
                response.Result.TYscanGroupByCollection = accessor.GetQueryTYscanGroupBy(request.SearchCondition);
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
        /// 查询天翼扫描明细
        /// </summary>
        public Response<QueryTYscanResponses> GetQueryTYscanDetail(QueryTYscanRequests request)
        {
            Response<QueryTYscanResponses> response = new Response<QueryTYscanResponses>() { Result = new QueryTYscanResponses() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetQueryTYscanDetail");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                TYscanAccessor accessor = new TYscanAccessor();
                response.Result.TYscanDetailCollection = accessor.GetQueryTYscanDetail(request.Customers);
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
        /// 导出天翼扫描明细报表
        /// </summary>
        public DataTable Proc_GetTYscanData(string starTime, string endsTime)
        {
            try
            {
                TYscanAccessor accessor = new TYscanAccessor();
                return accessor.Proc_GetTYscanData(starTime, endsTime);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return null;
        }

        ///// <summary>
        ///// 微信用户审核
        ///// </summary>
        //public Response<int> UploadWXCustomer(QueryWXCustomerRequests request)
        //{
        //    Response<int> response = new Response<int>();

        //    if (request == null)
        //    {
        //        ArgumentNullException ex = new ArgumentNullException("UploadWXCustomer request");
        //        LogError(ex);
        //        response.ErrorCode = ErrorCode.Argument;
        //        response.Exception = ex;
        //        return response;
        //    }

        //    try
        //    {
        //        WXCustomerAccessor accessor = new WXCustomerAccessor();
        //        response.Result = accessor.UploadWXCustomer(request.ID);
        //        if (response.Result == 1)
        //        {
        //            response.IsSuccess = true;
        //        }
        //        else
        //        {
        //            response.IsSuccess = false;
        //            if (response.Result == -1)
        //            {
        //                response.ErrorCode = ErrorCode.Technical;
        //            }
        //            else
        //            {
        //                response.ErrorCode = ErrorCode.Permission;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //        response.IsSuccess = false;
        //        response.ErrorCode = ErrorCode.Technical;
        //    }

        //    return response;
        //}
    }
}
