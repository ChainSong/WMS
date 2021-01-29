using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Biz
{
    public class SettledService : BaseService
    {
        public Response<SettledPod> GetSettledPodByID(GetSettledPodByIDRequest request)
        {
            Response<SettledPod> response = new Response<SettledPod>();

            if (request == null || request.ID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSettledPodByID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SettledAccessor accessor = new SettledAccessor();
                response.Result = accessor.GetSettledPodByID(request.ID);
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

        public Response<bool> EditSettledPod(EditSettledPodRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.SettledPod == null)
            {
                ArgumentNullException ex = new ArgumentNullException("EditSettledPod request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SettledAccessor accessor = new SettledAccessor();
                accessor.EditSettledPod(request.SettledPod, request.SettledType, request.Updator);
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

        public Response<IEnumerable<SettledPod>> GetSettledPodByPodIDs(GetSettledPodByPodIDsRequest request)
        {
            Response<IEnumerable<SettledPod>> response = new Response<IEnumerable<SettledPod>>();

            if (request == null || request.PodIDs == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSettledPodByPodIDs request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SettledAccessor accessor = new SettledAccessor();
                response.Result = accessor.GetSettledPodByPodIDs(request.PodIDs, request.SettledType);
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

        public Response<IEnumerable<SettledPod>> GetSettledPodByCondition(GetSettledPodByConditionRequest request)
        {
            Response<IEnumerable<SettledPod>> response = new Response<IEnumerable<SettledPod>>();
            if (request == null || request.CustomerOrderNumberCollection == null || !request.CustomerOrderNumberCollection.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("GetSettledPodByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SettledAccessor accessor = new SettledAccessor();
                response.Result = accessor.GetSettledPodByCondition(request.CustomerOrderNumberCollection, request.SettledType, request.CustomerID, request.ShipperID);
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

        public Response<IEnumerable<SettledPod>> GetSettledPodByIDs(GetSettledPodByIDsRequest request)
        {
            Response<IEnumerable<SettledPod>> response = new Response<IEnumerable<SettledPod>>();

            if (request == null || request.IDs == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSettledPodByIDs request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SettledAccessor accessor = new SettledAccessor();
                response.Result = accessor.GetSettledPodByIDs(request.IDs);
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

        public Response<bool> DeleteSettledPod(DeleteSettledPodRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.ID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("DeleteSettledPod request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SettledAccessor accessor = new SettledAccessor();
                accessor.DeleteSettledPod(request.ID, request.SettledType);
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

        public Response<bool> SettlePods(SettlePodsRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.SettledPods == null)
            {
                ArgumentNullException ex = new ArgumentNullException("SettlePods request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SettledAccessor accessor = new SettledAccessor();
                accessor.SettlePods(request.SettledPods,request.SettledType);
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

        public Response<IEnumerable<SettledPodAuditHistory>> GetSettledHistoryBySettledPodIDs(GetSettledHistoryBySettledPodIDsRequest request)
        {
            Response<IEnumerable<SettledPodAuditHistory>> response = new Response<IEnumerable<SettledPodAuditHistory>>();
            if (request == null || request.SettledPodIDs == null || !request.SettledPodIDs.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("GetSettledHistoryBySettledPodIDs request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SettledAccessor accessor = new SettledAccessor();
                response.Result = accessor.GetSettledHistoryBySettledPodIDs(request.SettledPodIDs);
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
        
        public Response<IEnumerable<SettledPod>> GetSettledPodsByCondition(GetSettledPodsByConditionRequest request)
        {
            Response<IEnumerable<SettledPod>> response = new Response<IEnumerable<SettledPod>>();
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSettledPodsByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SettledAccessor accessor = new SettledAccessor();
                response.Result = accessor.GetSettledPodsByCondition(request.SearchCondition);
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

        public Response<IEnumerable<SettledPod>> GetSettledPodsByInvoiceID(GetSettledPodsByInvoiceIDRequest request)
        {
            Response<IEnumerable<SettledPod>> response = new Response<IEnumerable<SettledPod>>();

            if (request == null || request.InvoiceID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSettledPodsByInvoiceID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SettledAccessor accessor = new SettledAccessor();
                response.Result = accessor.GetSettledPodsByInvoiceID(request.InvoiceID);
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

        public Response<bool> AuditSettledPod(AuditSettledPodRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.SettledPodIDs == null || !request.SettledPodIDs.Any()
                || string.IsNullOrEmpty(request.Auditor) || request.AuditTime == null || string.IsNullOrEmpty(request.AuditTypeMessage))
            {
                ArgumentNullException ex = new ArgumentNullException("AuditSettledPod request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SettledAccessor accessor = new SettledAccessor();
                accessor.AuditSettledPod(request.SettledPodIDs, request.Auditor, request.AuditTime,request.AuditRemark,request.AuditType,request.AuditTypeMessage);
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

        public Response<bool> DeleteAllExtenFeeData(DeleteAllExtenFeeDataRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.SettledPodIDCollection == null || !request.SettledPodIDCollection.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("DeleteAllExtenFeeData request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SettledAccessor accessor = new SettledAccessor();
                accessor.DeleteAllExtenFeeData(request.SettledPodIDCollection);
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

        public Response<bool> DeleteManualSettledFee(DeleteManualSettledFeeRequest request)
        {
            Response<bool> response = new Response<bool>();
            if (request == null || request.SettledPodIDCollection == null || !request.SettledPodIDCollection.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("DeleteManualSettledFee request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SettledAccessor accessor = new SettledAccessor();
                accessor.DeleteManualSettledFee(request.SettledPodIDCollection);
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


        public Response<IEnumerable<SettledPod>> BatchUpdateSettledPodAmt(BatchUpdateSettledPodAmtRequest request)
        {
            Response<IEnumerable<SettledPod>> response = new Response<IEnumerable<SettledPod>>();

            if (request == null || request.SettledPods == null || !request.SettledPods.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("BatchUpdateSettledPodAmt request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                SettledAccessor accessor = new SettledAccessor();
                response.Result = accessor.BatchUpdateSettledPodAmt(request.SettledPods, request.SettleType, request.Updator);
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

    }
}
