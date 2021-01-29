using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz
{
    public class TransportationLineService : BaseService
    {
        public Response<GetTransportationLinesByConditionResponse> GetTransportationLinesByConditon(GetTransportationLinesByConditionRequest request)
        {
            Response<GetTransportationLinesByConditionResponse> response = new Response<GetTransportationLinesByConditionResponse>() { Result = new GetTransportationLinesByConditionResponse() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetTransportationLinesByConditon request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                TransportationLineAccessor accessor = new TransportationLineAccessor();
                int rowCount;
                response.Result.TransportationLines = accessor.GetTransportationLinesByCondition(request.Name, request.StartCityID, request.EndCityID, request.State, request.PageIndex, request.PageSize, out rowCount).ToList();
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = rowCount % request.PageSize == 0 ? rowCount / request.PageSize : rowCount / request.PageSize + 1;
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

        public Response<long> AddTransportationLine(AddTransportationLineRequest request)
        {
            Response<long> response = new Response<long>();
            if (request == null || request.TransportationLine.StartCityID == 0 || request.TransportationLine.EndCityID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("AddTransportationLine request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                TransportationLineAccessor accessor = new TransportationLineAccessor();
                int returnVal = 0;
                long transportationLineID = accessor.AddTransportationLine(request.TransportationLine.Name, request.TransportationLine.StartCityID, request.TransportationLine.StartCityName,
                                                             request.TransportationLine.EndCityID, request.TransportationLine.EndCityName, request.TransportationLine.Distance, request.TransportationLine.Remark,
                                                             request.TransportationLine.State, request.TransportationLine.Creator, request.TransportationLine.CreateTime ?? DateTime.Now,
                                                             request.TransportationLine.Str1, request.TransportationLine.Str2, request.TransportationLine.Str3, out returnVal);
                if (returnVal == 1)
                {
                    response.IsSuccess = true;
                    response.Result = transportationLineID;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Result = returnVal;
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

        public Response<bool> SetTransportationLineState(SetTransportationLineStateRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.ID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("SetTransportationLineState request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                new TransportationLineAccessor().SetTransportationLineState(request.ID, request.State);
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

        public Response<IEnumerable<TransportationLine>> GetTransportationLines()
        {
            Response<IEnumerable<TransportationLine>> response = new Response<IEnumerable<TransportationLine>>();
            try
            {
                response.Result = new TransportationLineAccessor().GetTransportationLines();
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