using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.POD;
using Runbow.TWS.MessageContracts.POD.Hilti;
using System.Data;


namespace Runbow.TWS.Biz
{
    public class ForecastWarehouseService : BaseService
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="Sqlwhere"></param>
        /// <returns></returns>
        public Response<GetForecastWarehouseRequest> GetCRMInfo(GetForecastWarehouseRequest request)
        {
            Response<GetForecastWarehouseRequest> response = new Response<GetForecastWarehouseRequest>() { Result = new GetForecastWarehouseRequest() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetForecastOrdersRequestID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ForecastWarehouseAccessor Accessor = new ForecastWarehouseAccessor();
                int Rowcount;


                response.Result.IEnumerableForecastOrders= Accessor.GetCRMInfo(request.ForecastOrders, request.PageIndex, request.PageSize, out Rowcount);
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = Rowcount % request.PageSize == 0 ? Rowcount / request.PageSize : Rowcount / request.PageSize + 1;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="Sqlwhere"></param>
        /// <returns></returns>
        public Response<GetForecastWarehouseRequest> GetCRMInfo2(GetForecastWarehouseRequest request)
        {
            Response<GetForecastWarehouseRequest> response = new Response<GetForecastWarehouseRequest>() { Result = new GetForecastWarehouseRequest() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetForecastOrdersRequestID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                ForecastWarehouseAccessor Accessor = new ForecastWarehouseAccessor();
                int Rowcount;


                response.Result.IEnumerableForecastOrders = Accessor.GetCRMInfo2( request.PageIndex, request.PageSize, out Rowcount);
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = Rowcount % request.PageSize == 0 ? Rowcount / request.PageSize : Rowcount / request.PageSize + 1;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }
        public IEnumerable<ForecastOrders> waveList(string id)
        {
            Response<GetForecastWarehouseRequest> response = new Response<GetForecastWarehouseRequest>() { Result = new GetForecastWarehouseRequest() };
            try
            {
                ForecastWarehouseAccessor Accessor = new ForecastWarehouseAccessor();
                response.Result.IEnumerableForecastOrders = Accessor.waveList(id);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return response.Result.IEnumerableForecastOrders;
        }
        public IEnumerable<ForecastOrders> carriers(string id,string sb)
        {
            Response<GetForecastWarehouseRequest> response = new Response<GetForecastWarehouseRequest>() { Result = new GetForecastWarehouseRequest() };
            try
            {
                ForecastWarehouseAccessor Accessor = new ForecastWarehouseAccessor();
                response.Result.IEnumerableForecastOrders = Accessor.carriers(id,sb);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return response.Result.IEnumerableForecastOrders;
        }
        public IEnumerable<ForecastOrders> export( string sb)
        {
            Response<GetForecastWarehouseRequest> response = new Response<GetForecastWarehouseRequest>() { Result = new GetForecastWarehouseRequest() };
            try
            {
                ForecastWarehouseAccessor Accessor = new ForecastWarehouseAccessor();
                response.Result.IEnumerableForecastOrders = Accessor.export(sb);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return response.Result.IEnumerableForecastOrders;
        }
        public DataTable export2(string sb)
        {
            DataTable dt = new DataTable();
            Response<GetForecastWarehouseRequest> response = new Response<GetForecastWarehouseRequest>() { Result = new GetForecastWarehouseRequest() };
            try
            {
                ForecastWarehouseAccessor Accessor = new ForecastWarehouseAccessor();
                dt = Accessor.export2(sb);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return dt;
        }
        public IEnumerable<ForecastOrders> carrierslist(string id, string cs, string WaveReleaseTime,string sb)
        {
            Response<GetForecastWarehouseRequest> response = new Response<GetForecastWarehouseRequest>() { Result = new GetForecastWarehouseRequest() };
            try
            {
                ForecastWarehouseAccessor Accessor = new ForecastWarehouseAccessor();
                response.Result.IEnumerableForecastOrders = Accessor.carrierslist(id, cs, WaveReleaseTime,sb);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return response.Result.IEnumerableForecastOrders;
        }
        public IEnumerable<ForecastOrders> xiangxi(string id)
        {
            Response<GetForecastWarehouseRequest> response = new Response<GetForecastWarehouseRequest>() { Result = new GetForecastWarehouseRequest() };
            try
            {
                ForecastWarehouseAccessor Accessor = new ForecastWarehouseAccessor();
                response.Result.IEnumerableForecastOrders = Accessor.xiangxi(id);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return response.Result.IEnumerableForecastOrders;
        }
        public int confirmation(string id)
        {
            int acc = 0;
            try
            {
                ForecastWarehouseAccessor Accessor = new ForecastWarehouseAccessor();
                acc = Accessor.confirmation(id);
              
            }
            catch (Exception ex)
            {
                LogError(ex);
                
            }

            return acc;
        }
        public int cancellation(string id)
        {
            int acc = 0;
            try
            {
                ForecastWarehouseAccessor Accessor = new ForecastWarehouseAccessor();
                acc = Accessor.cancellation(id);
              
            }
            catch (Exception ex)
            {
                LogError(ex);
                
            }

            return acc;
        }
        public int deblocking2(string id)
        {
            int acc = 0;
            try
            {
                ForecastWarehouseAccessor Accessor = new ForecastWarehouseAccessor();
                acc = Accessor.deblocking2(id);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return acc;
        }
        public int appointed(string time, string id)
        {
            int acc = 0;
            try
            {
                ForecastWarehouseAccessor Accessor = new ForecastWarehouseAccessor();
                acc = Accessor.appointed(time,id);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return acc;
        }
        public int require(string time, string id)
        {
            int acc = 0;
            try
            {
                ForecastWarehouseAccessor Accessor = new ForecastWarehouseAccessor();
                acc = Accessor.require(time, id);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return acc;
        }
        public int mail_Select(ForecastOrders info)
        {
            int acc = 0;
            try
            {
                ForecastWarehouseAccessor Accessor = new ForecastWarehouseAccessor();
                acc = Accessor.mail_Select(info);

            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return acc;
        }
    }
}
