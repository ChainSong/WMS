using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Dao.POD;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.POD;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.POD.Nike;

namespace Runbow.TWS.Biz
{
    public class PodService : BaseService
    {
        public Response<Pod> AddPod(AddPodRequest request)
        {
            Response<Pod> response = new Response<Pod>();

            if (request == null || request.Pod == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddPod request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            if (string.IsNullOrEmpty(request.Pod.SystemNumber) || string.IsNullOrEmpty(request.Pod.CustomerOrderNumber)
                || request.Pod.StartCityID == 0 || request.Pod.EndCityID == 0 || request.Pod.ShipperTypeID == 0 || request.Pod.PODStateID == 0)
            {
                ArgumentException ex = new ArgumentException("Add Pod, SystemNumber or CustomerOrderNumber or StartCity or EndCity or ShipperType or PODState can't be null");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPod(request.Pod);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
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

        public Response<IEnumerable<string>> CheckIfPodExistsByPodCustomerOrderNumber(CheckIfPodExistsByPodCustomerOrderNumberRequest request)
        {
            Response<IEnumerable<string>> response = new Response<IEnumerable<string>>();
            if (request == null || request.CustomerOrderNumberCollection == null || !request.CustomerOrderNumberCollection.Any() || request.CustomerID == 0 || request.ProjectID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("CheckIfPodExistsByPodCustomerOrderNumber request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.CheckIfPodExistsByPodCustomerOrderNumber(request.ProjectID, request.CustomerID, request.CustomerOrderNumberCollection);
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

        public Response<IEnumerable<PodKey>> AddPods(AddPodsRequest request)
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
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPods(request.Pods);
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

        public Response<int> DeletePodAndRelatedInfo(DeletePodInfoRequest request)
        {
            Response<int> response = new Response<int>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("DeletePodAndRelatedInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.DeletePodAndRelatedInfo(request.ID);
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

        public Response<PodDetail> AddPodDetail(AddPodDetailRequest request)
        {
            Response<PodDetail> response = new Response<PodDetail>();

            if (request == null || request.PodDetail == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            if (request.PodDetail.PodID == 0 || string.IsNullOrEmpty(request.PodDetail.SystemNumber) || string.IsNullOrEmpty(request.PodDetail.CustomerOrderNumber))
            {
                ArgumentException ex = new ArgumentException("Add PodDetail, PodID SystemNumber or CustomerOrderNumber can't be null");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPodDetail(request.PodDetail);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
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

        public Response<IEnumerable<PodDetail>> AddPodDetails(AddPodDetailsRequest request)
        {
            Response<IEnumerable<PodDetail>> response = new Response<IEnumerable<PodDetail>>();

            if (request == null || request.PodDetails == null || !request.PodDetails.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPodDetails(request.PodDetails, request.CustomerID);
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

        public Response<PodDetail> DeletePodDetailByID(DeletePodInfoRequest request)
        {
            Response<PodDetail> response = new Response<PodDetail>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddPoDeletePodDetailByIDdDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.DeletePodDetailByID(request.ID);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
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

        public Response<PodStatusLog> AddPodStatusLog(AddPodStatusLogRequest request)
        {
            Response<PodStatusLog> response = new Response<PodStatusLog>();

            if (request == null || request.PodStatusLog == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodStatusLog request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            if (request.PodStatusLog.PodID == 0 || string.IsNullOrEmpty(request.PodStatusLog.SystemNumber) || string.IsNullOrEmpty(request.PodStatusLog.CustomerOrderNumber))
            {
                ArgumentException ex = new ArgumentException("Add PodStatusLog, PodID SystemNumber or CustomerOrderNumber can't be null");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPodStatusLog(request.PodStatusLog);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
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

        public Response<PodStatusTrack> AddPodStatusTrack(AddPodStatusTrackRequest request)
        {
            Response<PodStatusTrack> response = new Response<PodStatusTrack>();

            if (request == null || request.PodStatusTrack == null || request.PodStatusTrack.PodID == 0 || string.IsNullOrEmpty(request.PodStatusTrack.SystemNumber) || string.IsNullOrEmpty(request.PodStatusTrack.CustomerOrderNumber))
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodStatusTrack request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPodStatusTrack(request.PodStatusTrack);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
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

        public Response<IEnumerable<PodStatusLog>> AddPodStatusLogs(AddPodStatusLogsRequest request)
        {
            Response<IEnumerable<PodStatusLog>> response = new Response<IEnumerable<PodStatusLog>>();

            if (request == null || request.PodStatusLogs == null || !request.PodStatusLogs.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodStatusLog request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPodStatusLogs(request.PodStatusLogs, request.CustomerID);
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

        public Response<IEnumerable<PodStatusTrack>> AddPodStatusTracks(AddPodStatusTracksRequest request)
        {
            Response<IEnumerable<PodStatusTrack>> response = new Response<IEnumerable<PodStatusTrack>>();

            if (request == null || request.PodStatusTracks == null || !request.PodStatusTracks.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodStatusTracks request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPodStatusTracks(request.PodStatusTracks, request.CustomerID);
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

        public Response<PodStatusLog> DeletePodStatusLogByID(DeletePodInfoRequest request)
        {
            Response<PodStatusLog> response = new Response<PodStatusLog>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("DeletePodStatusLogByID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.DeletePodStatusLogByID(request.ID);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
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

        public Response<PodStatusTrack> DeletePodStatusTrackByID(DeletePodInfoRequest request)
        {
            Response<PodStatusTrack> response = new Response<PodStatusTrack>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("DeletePodStatusTrackByID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.DeletePodStatusTrackByID(request.ID);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
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


        public Response<PodAll> GetPodAndReleatedInfo(GetPodAndReleatedInfoRequest request)
        {
            Response<PodAll> response = new Response<PodAll>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodAndReleatedInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.GetPodAndReleatedInfo(request.ID);
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

        public Response<IEnumerable<PodDetail>> GetPodDetailsByPodIDs(GetPodInfoRequest request)
        {
            Response<IEnumerable<PodDetail>> response = new Response<IEnumerable<PodDetail>>();
            if (request == null || request.PodIDs == null || !request.PodIDs.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodDetailsByPodIDs request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.GetPodDetailsByPodIDs(request.PodIDs);
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

        public Response<IEnumerable<PodException>> GetPodExceptionsByPodIDs(GetPodInfoRequest request)
        {
            Response<IEnumerable<PodException>> response = new Response<IEnumerable<PodException>>();
            if (request == null || request.PodIDs == null || !request.PodIDs.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodExceptionsByPodIDs request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.GetPodExceptionsByPodIDs(request.PodIDs);
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

        public Response<PodFeadBack> GetPodFeadBackByPodID(GetPodInfoRequest request)
        {
            Response<PodFeadBack> response = new Response<PodFeadBack>();
            if (request == null || request.PodID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodFeadBackByPodID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.GetPodFeadBackByPodID(request.PodID);
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

        public Response<PodReplyDocument> GetPodReplyDocumentByPodID(GetPodInfoRequest request)
        {
            Response<PodReplyDocument> response = new Response<PodReplyDocument>();
            if (request == null || request.PodID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodReplyDocumentByPodID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.GetPodReplyDocumentByPodID(request.PodID);
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

        public Response<PodFee> GetPodFeeByPodID(GetPodInfoRequest request)
        {
            Response<PodFee> response = new Response<PodFee>();
            if (request == null || request.PodID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodFeeByPodID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.GetPodFeeByPodID(request.PodID);
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

        public Response<IEnumerable<PodStatusLog>> GetPodStatusLogsByPodIDs(GetPodInfoRequest request)
        {
            Response<IEnumerable<PodStatusLog>> response = new Response<IEnumerable<PodStatusLog>>();
            if (request == null || request.PodIDs == null || !request.PodIDs.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodStatusLogsByPodIDs request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.GetPodStatusLogsByPodIDs(request.PodIDs);
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

        public Response<IEnumerable<PodStatusTrack>> GetPodStatusTracksByPodIDs(GetPodInfoRequest request)
        {
            Response<IEnumerable<PodStatusTrack>> response = new Response<IEnumerable<PodStatusTrack>>();
            if (request == null || request.PodIDs == null || !request.PodIDs.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodStatusTracksByPodIDs request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.GetPodStatusTracksByPodIDs(request.PodIDs);
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

        public Response<IEnumerable<PodFeadBack>> GetPodFeadBacksByPodIDs(GetPodInfoRequest request)
        {
            Response<IEnumerable<PodFeadBack>> response = new Response<IEnumerable<PodFeadBack>>();
            if (request == null || request.PodIDs == null || !request.PodIDs.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodFeadBacksByPodIDs request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.GetPodFeadBacksByPodIDs(request.PodIDs);
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

        public Response<IEnumerable<PodTrack>> GetPodTracksByPodIDs(GetPodInfoRequest request)
        {
            Response<IEnumerable<PodTrack>> response = new Response<IEnumerable<PodTrack>>();
            if (request == null || request.PodIDs == null || !request.PodIDs.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodTracksByPodIDs request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.GetPodTracksByPodIDs(request.PodIDs);
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

        public Response<PodException> AddPodException(AddPodExceptionRequest request)
        {
            Response<PodException> response = new Response<PodException>();

            if (request == null || request.PodException == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodException request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            if (request.PodException.PodID == 0 || string.IsNullOrEmpty(request.PodException.SystemNumber) || string.IsNullOrEmpty(request.PodException.CustomerOrderNumber))
            {
                ArgumentException ex = new ArgumentException("Add PodException, PodID SystemNumber or CustomerOrderNumber can't be null");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPodException(request.PodException);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
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

        public Response<IEnumerable<PodException>> AddPodExceptions(AddPodExceptionsRequest request)
        {
            Response<IEnumerable<PodException>> response = new Response<IEnumerable<PodException>>();

            if (request == null || request.PodExceptions == null || !request.PodExceptions.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodExceptions request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPodExceptions(request.PodExceptions, request.CustomerID);
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

        public Response<PodException> DeletePodExceptionByID(DeletePodInfoRequest request)
        {
            Response<PodException> response = new Response<PodException>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("DeletePodExceptionByID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.DeletePodExceptionByID(request.ID);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
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

        public Response<PodTrack> AddPodTrack(AddPodTrackRequest request)
        {
            Response<PodTrack> response = new Response<PodTrack>();

            if (request == null || request.PodTrack == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodTrack request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            if (request.PodTrack.PodID == 0 || string.IsNullOrEmpty(request.PodTrack.SystemNumber) || string.IsNullOrEmpty(request.PodTrack.CustomerOrderNumber))
            {
                ArgumentException ex = new ArgumentException("Add PodTrack, PodID SystemNumber or CustomerOrderNumber can't be null");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPodTrack(request.PodTrack);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
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

        public Response<IEnumerable<string>> CheckNikePodTrack(AddPodTracksRequest request)
        {
            Response<IEnumerable<string>> response = new Response<IEnumerable<string>>();

            if (request == null || request.PodTracks == null || !request.PodTracks.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodTracks request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.CheckNikePodTrack(request.PodTracks);
                if (response.Result.Count() > 0)
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.DataEffective;
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

        public Response<IEnumerable<PodTrack>> AddPodTracks(AddPodTracksRequest request)
        {
            Response<IEnumerable<PodTrack>> response = new Response<IEnumerable<PodTrack>>();

            if (request == null || request.PodTracks == null || !request.PodTracks.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodTracks request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPodTracks(request.PodTracks, request.CustomerID);
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

        public Response<PodTrack> DeletePodTrackByID(DeletePodInfoRequest request)
        {
            Response<PodTrack> response = new Response<PodTrack>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("DeletePodTrackByID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.DeletePodTrackByID(request.ID);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
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

        public Response<PodFeadBack> AddOrUpdatePodFeadBack(AddOrUpdatePodFeadBackRequest request)
        {
            Response<PodFeadBack> response = new Response<PodFeadBack>();

            if (request == null || request.PodFeadBack == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddOrUpdatePodFeadBack request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            if (request.PodFeadBack.PodID == 0 || string.IsNullOrEmpty(request.PodFeadBack.SystemNumber) || string.IsNullOrEmpty(request.PodFeadBack.CustomerOrderNumber))
            {
                ArgumentException ex = new ArgumentException("AddOrUpdate PodFeadBack, PodID SystemNumber or CustomerOrderNumber can't be null");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddOrUpdatePodFeadBack(request.PodFeadBack);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
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

        public Response<IEnumerable<PodFeadBack>> AddPodFeadBacks(AddPodFeadBacksRequest request)
        {
            Response<IEnumerable<PodFeadBack>> response = new Response<IEnumerable<PodFeadBack>>();

            if (request == null || request.PodFeadBacks == null || !request.PodFeadBacks.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodFeadBacks request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPodFeadBacks(request.PodFeadBacks, request.CustomerID);
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

        public Response<PodReplyDocument> AddOrUpdatePodReplyDocument(AddOrUpdatePodReplyDocumentRequest request)
        {
            Response<PodReplyDocument> response = new Response<PodReplyDocument>();

            if (request == null || request.PodReplyDocument == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddOrUpdatePodReplyDocument request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            if (request.PodReplyDocument.PodID == 0 || string.IsNullOrEmpty(request.PodReplyDocument.SystemNumber) || string.IsNullOrEmpty(request.PodReplyDocument.CustomerOrderNumber))
            {
                ArgumentException ex = new ArgumentException("AddOrUpdate PodReplyDocument, PodID SystemNumber or CustomerOrderNumber can't be null");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddOrUpdatePodReplyDocument(request.PodReplyDocument);
                if (response.Result.ID > 0)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
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

        public Response<IEnumerable<PodFee>> AddPodFees(AddPodFeesRequest request)
        {
            Response<IEnumerable<PodFee>> response = new Response<IEnumerable<PodFee>>();

            if (request == null || request.PodFees == null || !request.PodFees.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodFees request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPodFees(request.PodFees, request.CustomerID);
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

        public Response<IEnumerable<PodReplyDocument>> AddPodReplyDocuments(AddPodReplyDocumentsRequest request)
        {
            Response<IEnumerable<PodReplyDocument>> response = new Response<IEnumerable<PodReplyDocument>>();

            if (request == null || request.PodReplyDocuments == null || !request.PodReplyDocuments.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodReplyDocuments request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddPodReplyDocuments(request.PodReplyDocuments, request.CustomerID);
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

        public Response<IEnumerable<Pod>> QueryPodByPodIDs(QueryPodByIDsRequest request)
        {
            Response<IEnumerable<Pod>> response = new Response<IEnumerable<Pod>>();
            if (request == null || request.PodIDs == null)
            {
                ArgumentNullException ex = new ArgumentNullException("QueryPodByPodIDs request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.QueryPodByPodIDs(request.PodIDs);
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

        public Response<DataTable> GetExportPodReplyDocumentWhtiAttachmentByCondityin(GetPodReplyDocumentWithAttachmentByConditionRequest request)
        {
            Response<DataTable> response = new Response<DataTable>();
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetExportPodReplyDocumentWhtiAttachmentByCondityin request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.GetExportPodReplyDocumentWithAttachmentByCondition(request.SearchCondition, request.ProjectID);
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
        public Response<IEnumerable<PodReplyDocumentWithAttachment>> GetPodReplyDocumentWithAttachmentByCondition(GetPodReplyDocumentWithAttachmentByConditionRequest request)
        {
            Response<IEnumerable<PodReplyDocumentWithAttachment>> response = new Response<IEnumerable<PodReplyDocumentWithAttachment>>();
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodReplyDocumentWithAttachmentByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.GetPodReplyDocumentWithAttachmentByCondition(request.SearchCondition, request.ProjectID);
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
                PodAccessor accessor = new PodAccessor();
                int rowCount;
                response.Result.PodCollections = accessor.QueryPod(request.SearchCondition, request.ProjectID, request.PageIndex, request.PageSize, out rowCount).ToList().Distinct(new ComparePod());
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
        public int PODDistributionVehicle(PODDistributionVehicle req)
        {
            int str = 0;
            try
            {
                PodAccessor accessor = new PodAccessor();
                str = accessor.PODDistributionVehicle(req);
            }
            catch (Exception)
            {

                return 0;
            }
            return str;
        }
        public int CancelPODDistributionVehicle(PODDistributionVehicle req)
        {
            int str = 0;
            try
            {
                PodAccessor accessor = new PodAccessor();
                str = accessor.CancelPODDistributionVehicle(req);
            }
            catch (Exception)
            {

                return 0;
            }
            return str;
        }
        public int WaybillReach(PODDistributionVehicle req)
        {
            int str = 0;
            try
            {
                PodAccessor accessor = new PodAccessor();
                str = accessor.WaybillReach(req);
            }
            catch (Exception)
            {

                throw;
            }
            return str;
        }
        /// <summary>
        /// 打印宝胜运单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<QueryPodResponse> QueryBSPod(QueryPodRequest request)//DataTable
        {
            //DataTable dt = new DataTable();
            Response<QueryPodResponse> response = new Response<QueryPodResponse>() { Result = new QueryPodResponse() };
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("QueryPod request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
                //return dt;
            }

            try
            {
                //DataTable dt = new DataTable();
                PodAccessor accessor = new PodAccessor();
                int rowCount;//response.Result.PodCollections
                response.Result.PodCollections = accessor.QueryBSPod(request.SearchCondition, request.ProjectID, request.PageIndex, request.PageSize, out rowCount).ToList().Distinct(new ComparePod());

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

        public class ComparePod : IEqualityComparer<PodWithAttachment>
        {
            public bool Equals(PodWithAttachment x, PodWithAttachment y)
            {
                return x.ID == y.ID;
            }

            public int GetHashCode(PodWithAttachment obj)
            {
                return obj.ToString().GetHashCode();
            }
        }

        public Response<IEnumerable<Pod>> QueryPodWithNoPaging(QueryPodRequest request)
        {
            Response<IEnumerable<Pod>> response = new Response<IEnumerable<Pod>>();
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("QueryPodWithNoPaging request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.QueryPodWithNoPaging(request.SearchCondition, request.ProjectID);
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
        //导出VF跟踪信息
        public Response<PodAll> VFTrackingReport(QueryPodRequest request)
        {
            Response<PodAll> response = new Response<PodAll>();
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("VFTrackingReport request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                if (request.PageSize != 0)
                {
                    response.Result = accessor.VFTrackingReport(request.SearchCondition, request.ProjectID, request.PageIndex, request.PageSize);
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = accessor.VFTrackingReportALL(request.SearchCondition, request.ProjectID);
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
        //导出Adidas信息报表
        public Response<PodAll> AdidasTrackingReport(QueryPodRequest request)
        {
            Response<PodAll> response = new Response<PodAll>();
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AdidasTrackingReport request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AdidasTrackingReport(request.SearchCondition, request.ProjectID, request.PageIndex, request.PageSize);
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

        // <summary>
        /// 导出艺康报表
        /// </summary>
        public DataTable YKTrackingReport(QueryPodRequest request)
        {
            DataTable dt = new DataTable();
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("YKTrackingReport request");
                LogError(ex);
                //response.ErrorCode = ErrorCode.Argument;
                //response.Exception = ex;
                return null;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                dt = accessor.YKTrackingReport(request.SearchCondition, request.ProjectID, request.PageIndex, request.PageSize);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return dt;
        }

        //导出宝胜跟踪信息
        public Response<SuperPodAll> BSTrackingReport(QueryPodRequest request)
        {
            Response<SuperPodAll> response = new Response<SuperPodAll>();
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("BSTrackingReport request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                if (request.PageSize != 0)
                {
                    response.Result = accessor.BSTrackingReport(request.SearchCondition, request.ProjectID, request.PageIndex, request.PageSize);
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = accessor.BSTrackingReportALL(request.SearchCondition, request.ProjectID);
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
        public Response<int> GetTodaysPodCount(GetTodaysPodCountRequest request)
        {
            Response<int> response = new Response<int>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetTodaysPodCount request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.GetTodaysPodCount(request.ProjectID, request.SystemNumberPrefix);
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

        public Response<bool> SetPodStatus(SetPodStatusRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.IDs == null || !request.IDs.Any() || request.PodStatusID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("SetPodStatus request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.SetPodStatus(request.IDs, request.PodStatusID, request.PodStatusName, request.IsSendMessage);
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

        public Response<SettledPodResponse> SettledPodSearch(SettledPodRequest request)
        {
            Response<SettledPodResponse> response = new Response<SettledPodResponse>() { Result = new SettledPodResponse() };
            if (request == null || request.IDs == null || !request.IDs.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("SettledPodSearch request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result.GroupedPods = accessor.GetGroupedPodsByPodIDs(request.IDs, request.SettledType, request.IsID);
                response.Result.PodIDs = request.IDs;
                response.Result.SettledType = request.SettledType;
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


        public Response<SettledPodResponse> SettledPodByAddress(SettledPodRequest request)
        {
            Response<SettledPodResponse> response = new Response<SettledPodResponse>() { Result = new SettledPodResponse() };
            if (request == null || request.IDs == null || !request.IDs.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("SettledPodByAddress request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result.GroupedPods = accessor.SettledPodByAddress(request.IDs, request.SettledType);
                response.Result.PodIDs = request.IDs;
                response.Result.SettledType = request.SettledType;
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

        public Response<bool> AuditPodReplyDocument(AuditPodReplyDocumentRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.SystemNumbers == null || !request.SystemNumbers.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AuditPodReplyDocument request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                accessor.AuditPodReplyDocument(request.SystemNumbers, request.AuditUser);
                response.Result = true;
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

        public Response<IEnumerable<string>> SplitPod(SplitPodRequest request)
        {
            Response<IEnumerable<string>> response = new Response<IEnumerable<string>>();
            if (request == null || request.ID == 0 || request.SplitNumber == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("SplitPod request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.SplitPod(request.ID, request.SplitNumber);
                if (response.Result != null && response.Result.Count() == request.SplitNumber)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.DataEffective;
                }
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

        public Response<IEnumerable<PodDescription>> SetPodShipperManually(SetPodShipperManuallyRequest request)
        {
            Response<IEnumerable<PodDescription>> response = new Response<IEnumerable<PodDescription>>();
            if (request == null || request.IDs == null || !request.IDs.Any() || request.ShipperID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("SetPodShipperManually request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.SetPodShipperManually(request.ProjectID, request.IDs, request.ShipperID, request.ShipperName);
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

        public Response<IEnumerable<PodDescription>> SetPodShipper(SetPodShipperRequest request)
        {
            Response<IEnumerable<PodDescription>> response = new Response<IEnumerable<PodDescription>>();
            if (request == null || request.IDs == null || !request.IDs.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("SetPodShipper request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.SetPodShipper(request.ProjectID, request.IDs);
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

        public Response<bool> ManualSettledPod(ManualSettledPodRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.PodIDs == null || !request.PodIDs.Any() || request.ShipperID == 0 || string.IsNullOrEmpty(request.ShipperName)
                || request.SettledPodCOllection == null || !request.SettledPodCOllection.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("ManualSettledPod request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                accessor.ManualSettledPod(request.PodIDs, request.ShipperID, request.ShipperName, request.SettledPodCOllection);
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

        public Response<bool> AddPodExtensionFee(AddPodExtensionFeeRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.PodIDs == null || !request.PodIDs.Any() || request.PodCollection == null || !request.PodCollection.Any()
                || request.SettledPodCOllection == null || !request.SettledPodCOllection.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddPodExtensionFee request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                accessor.AddPodExtensionFee(request.PodIDs, request.PodCollection, request.SettledPodCOllection, request.Type);
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

        public Response<IEnumerable<PodForecastInfo>> GetPodForecastInfoCollection(GetPodForecastInfoCollectionRequest request)
        {
            Response<IEnumerable<PodForecastInfo>> response = new Response<IEnumerable<PodForecastInfo>>();

            if (request == null || request.PodIDs == null || !request.PodIDs.Any() || request.ProjectID == 0 || request.CustomerID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodForecastInfoCollection request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.GetPodForecastInfoCollection(request.PodIDs, request.CustomerID, request.ProjectID);
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

        public Response<AddLog56TracksResult> AddLog56Tracks(AddLog56TracksRequest request)
        {
            Response<AddLog56TracksResult> response = new Response<AddLog56TracksResult>();
            if (request == null || request.UsefulTracks == null || request.AllTracks == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddLog56Tracks request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.AddLog56Tracks(request.UsefulTracks, request.UselessTracks, request.AllTracks);
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

        public Response<IEnumerable<Log56PhoneStatus>> UpdateLog56PhoneStatus(UpdateLog56PhoneStatusRequest request)
        {
            Response<IEnumerable<Log56PhoneStatus>> response = new Response<IEnumerable<Log56PhoneStatus>>();
            if (request == null || request.Log56PhoneStatus == null || !request.Log56PhoneStatus.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateLog56PhoneStatus request Log56PhoneStatus");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.UpdateLog56PhoneStatus(request.Log56PhoneStatus);
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

        public Response<bool> UpdateLog56PhoneStatusFromLog56(UpdateLog56PhoneStatusRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.Log56PhoneStatus == null || !request.Log56PhoneStatus.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateLog56PhoneStatusFromLog56 request Log56PhoneStatus");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                accessor.UpdateLog56PhoneStatusFromLog56(request.Log56PhoneStatus);
                response.Result = true;
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

        public Response<bool> CancelAttachmentAudit(CancenAttachmentAuditRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.AttachmentID == 0 || request.PodID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("CancelAttachmentAudit request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                if (accessor.CancelAttachmentAudit(request.AttachmentID, request.PodID))
                {
                    response.Result = true;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = false;
                    response.IsSuccess = false;
                }
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

        public Response<bool> SetAttachmentRemark(SetAttachmentRemarkRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.ID == 0 || string.IsNullOrEmpty(request.Remark))
            {
                ArgumentNullException ex = new ArgumentNullException("SetAttachmentRemark request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                accessor.SetAttachmentRemark(request.ID, request.Remark, request.AuditUser);
                response.Result = true;
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

        ////导出图片
        //public Response<IEnumerable<Attachment>> PodWithAttachment(QueryPodRequest request)
        //{
        //    Response<IEnumerable<Attachment>> response = new Response<IEnumerable<Attachment>>();
        //    try
        //    {
        //        AttachmentAccessor accessor = new AttachmentAccessor();
        //        response.Result = accessor.PodWithAttachment(request.SearchCondition);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //    }


        //    return response;
        //}

        public Response<IEnumerable<Attachment>> PodWithAttachment(QueryPodRequest request)
        {
            //验证request;
            Response<IEnumerable<Attachment>> response = new Response<IEnumerable<Attachment>>();

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("PodWithAttachment request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.PodWithAttachment(request.SearchCondition);
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

        public Response<NikePODForBSResponses> GetPODDistributionVehicle(NikePODForBSRequest request)
        {
            Response<NikePODForBSResponses> response = new Response<NikePODForBSResponses>() { Result = new NikePODForBSResponses() };
            try
            {
                PodAccessor accessor = new PodAccessor();
                int Rowcount = 0;
                response.Result.PodCollection = accessor.GetPODDistributionVehicle(request.Condition, request.PageIndex, request.PageSize, out Rowcount);//
                //NikeAccessor accessor = new NikeAccessor();

                //response.Result.PodCollection = accessor.GetNikePOD(request.Condition, request.PageIndex, request.PageSize, out Rowcount);//
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = Rowcount % request.PageSize == 0 ? Rowcount / request.PageSize : Rowcount / request.PageSize + 1;
                response.Result.RowCount = Rowcount;
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

        /// <summary>
        /// 导出百姓网订单报表
        /// </summary>
        public Response<PodAll> BaiXingTrackingReport(QueryPodRequest request)
        {
            Response<PodAll> response = new Response<PodAll>();
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("BaiXingTrackingReport request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodAccessor accessor = new PodAccessor();
                response.Result = accessor.BaiXingTrackingReport(request.SearchCondition, request.ProjectID, request.PageIndex, request.PageSize);
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