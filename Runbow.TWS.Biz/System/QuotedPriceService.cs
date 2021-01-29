using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz
{
    public class QuotedPriceService : BaseService
    {
        public Response<IEnumerable<SegmentDetail>> GetSegmentDetailByCustomerOrShipper(GetSegmentDetailByCustomerOrShipperRequest request)
        {
            Response<IEnumerable<SegmentDetail>> response = new Response<IEnumerable<SegmentDetail>>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSegmentDetailByCustomerOrShipper request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                QuotedPriceAccessor accessor = new QuotedPriceAccessor();
                response.Result = accessor.GetSegmentDetailByCustomerOrShipper(request.ProjectID, request.Target, request.CustomerOrShipperID,request.RelatedCustomerID);
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

        public Response<bool> AddQuotedPrice(AddQuotedPriceRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.QuotedPrices == null || !request.QuotedPrices.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddQuotedPrice request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                new QuotedPriceAccessor().AddQuotedPrice(request.QuotedPrices);
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

        public Response<bool> AddQuotedPrices(AddQuotedPriceRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.QuotedPrices_New == null || !request.QuotedPrices_New.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddQuotedPrices request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                new QuotedPriceAccessor().AddQuotedPrices(request.QuotedPrices_New);
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

        public Response<IEnumerable<QuotedPrice>> GetQuotedPriceByCondition(GetQuotedPriceByConditionRequest request)
        {
            Response<IEnumerable<QuotedPrice>> response = new Response<IEnumerable<QuotedPrice>>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetQuotedPriceByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                QuotedPriceAccessor accessor = new QuotedPriceAccessor();
                response.Result = accessor.GetQuotedPriceByCondition(request.ProjectID, request.Target, request.CustomerOrShipperID, request.TransportationLineID, request.ShipperTypeID,
                    request.PodTypeID, request.TtlOrTplID, request.EffectiveStartTime, request.EffectiveEndTime, request.StartCityID, request.EndCityID,request.RelatedCustomerID);
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
        public Response<IEnumerable<QueryBAFPrice>> GetBAFQuotedPrice()
        {
            Response<IEnumerable<QueryBAFPrice>> response = new Response<IEnumerable<QueryBAFPrice>>();
            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.QueryBAFPrice();
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
        public Response<IEnumerable<QuotedPrice>> GetAllQuotedPrice()
        {
            Response<IEnumerable<QuotedPrice>> response = new Response<IEnumerable<QuotedPrice>>();
            try
            {
                QuotedPriceAccessor accessor = new QuotedPriceAccessor();
                response.Result = accessor.GetAllQuotedPrice();
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

        //change by cyf
        public Response<IEnumerable<QuotedPrice>> GetQuotedPrice(long projectID, int target, long customerID, long relatedCustomerID)
        {
            Response<IEnumerable<QuotedPrice>> response = new Response<IEnumerable<QuotedPrice>>();
            try
            {
                QuotedPriceAccessor accessor = new QuotedPriceAccessor();
                response.Result = accessor.GetQuotedPrice(projectID,target,customerID,relatedCustomerID);
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

        public Response<QuotedPrice> DeleteQuetedPrice(DeleteQuetedPriceRequest request)
        {
            Response<QuotedPrice> response = new Response<QuotedPrice>();
            if (request == null || request.QutedPriceIDs == null || !request.QutedPriceIDs.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("DeleteQuetedPrice request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                response.Result = new QuotedPriceAccessor().DeleteQuetedPrice(request.QutedPriceIDs);
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

        public int GetrelatedCustomerID(int settledType, long customerOrShipperID)
        {
            int i = 0;
            try
            {
                QuotedPriceAccessor accessor = new QuotedPriceAccessor();
                i = accessor.GetrelatedCustomerID(settledType, customerOrShipperID);

            }
            catch(Exception ex)
            {
                LogError(ex);
            }

            return i;
        }

    }
}