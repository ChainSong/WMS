using System;
using System.Collections.Generic;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz
{
    public class SegmentService : BaseService
    {
        public Response<IEnumerable<Segment>> GetSegmentsByCondition(GetSegmentsByConditionRequest request)
        {
            Response<IEnumerable<Segment>> response = new Response<IEnumerable<Segment>>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSegmentsByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SegmentAccessor accessor = new SegmentAccessor();
                response.Result = accessor.GetSegmentsByCondition(request.Name, request.State);
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

        public Response<bool> SetSegmentState(SetSegmentStateRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.ID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("SetSegmentState request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                new SegmentAccessor().SetSegmentState(request.ID, request.State);
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

        public Response<string> GetSegmentByCursterId(int pId,int cId)
        {
            Response<string> response = new Response<string>();
            try
            {
                SegmentAccessor accessor = new SegmentAccessor();
                response.Result = accessor.GetSegmentByCursterId(pId, cId);
            }
            catch(Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return response;
        }

        public Response<long> AddSegmentDetail(SegmentDetailRequest request)
        {
            Response<long> response = new Response<long>();
            if (request == null || request.SegmentDetail == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddSegmentDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SegmentAccessor accessor = new SegmentAccessor();
                int returnVal = 0;
                long segmentID = accessor.AddSegmentDetail(request.SegmentDetail.SegmentID, request.SegmentDetail.StartVal, request.SegmentDetail.EndVal, request.SegmentDetail.Description,
                                                     request.SegmentDetail.Creator, request.SegmentDetail.CreateTime ?? DateTime.Now, request.SegmentDetail.Str1,
                                                     request.SegmentDetail.Str2, request.SegmentDetail.Str3, out returnVal);
                if (returnVal == 1)
                {
                    response.IsSuccess = true;
                    response.Result = segmentID;
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

        public Response<long> UpdateSegmentDetail(SegmentDetailRequest request)
        {
            Response<long> response = new Response<long>();
            if (request == null || request.SegmentDetail == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddSegmentDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SegmentAccessor accessor = new SegmentAccessor();
                int returnVal = 0;
                long segmentID = accessor.UpdateSegmentDetail(request.SegmentDetail.ID, request.SegmentDetail.SegmentID, request.SegmentDetail.StartVal, request.SegmentDetail.EndVal, request.SegmentDetail.Description,
                                                            request.SegmentDetail.Str1, request.SegmentDetail.Str2, request.SegmentDetail.Str3, out returnVal);
                if (returnVal == 1)
                {
                    response.IsSuccess = true;
                    response.Result = segmentID;
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

        public Response<long> AddSegment(AddSegmentRequest request)
        {
            Response<long> response = new Response<long>();
            if (request == null || request.Segment == null || string.IsNullOrEmpty(request.Segment.Name))
            {
                ArgumentNullException ex = new ArgumentNullException("AddSegment request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SegmentAccessor accessor = new SegmentAccessor();
                int returnVal = 0;
                long segmentID = accessor.AddSegment(request.Segment.Name, request.Segment.Description, request.Segment.Creator, request.Segment.CreateTime ?? DateTime.Now, request.Segment.Str1, request.Segment.Str2, request.Segment.Str3, request.Segment.State, out returnVal);
                if (returnVal == 1)
                {
                    response.IsSuccess = true;
                    response.Result = segmentID;
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

        public Response<long> DeleteSegmentDetail(SegmentRequest request)
        {
            Response<long> response = new Response<long>();
            if (request == null || request.ID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSegmentAndDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SegmentAccessor accessor = new SegmentAccessor();
                int returnVal = 0;
                long segmentID = accessor.DeleteSegmentDetail(request.ID, out returnVal);
                if (returnVal == 1)
                {
                    response.IsSuccess = true;
                    response.Result = segmentID;
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

        public Response<GetSegmentAndDetailResponse> GetSegmentAndDetail(SegmentRequest request)
        {
            Response<GetSegmentAndDetailResponse> response = new Response<GetSegmentAndDetailResponse>() { Result = new GetSegmentAndDetailResponse() };
            if (request == null || request.ID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSegmentAndDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SegmentAccessor accessor = new SegmentAccessor();
                Segment segment;
                IEnumerable<SegmentDetail> segmentDetailCollection;
                accessor.GetSegmentAndDetail(request.ID, out segment, out segmentDetailCollection);
                response.Result.Segment = segment;
                response.Result.SegmentDetailCollection = segmentDetailCollection;
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