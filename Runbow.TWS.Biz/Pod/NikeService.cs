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
namespace Runbow.TWS.Biz.POD
{
    public class NikeService : BaseService
    {
        /// <summary>
        /// 获取Nike导出报表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<DataTable> GetNikeReportExprot(GetNikeReportExportRequest request) 
        {
            Response<DataTable> response = new Response<DataTable>() { Result = new GetNikeReportExportRequest().NikeReport };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetNikeReportExport request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                NikeAccessor Accessor = new NikeAccessor();

                response.Result = Accessor.GetNikeReportExprot(request.SqlWhere,request.ReportName);
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









        public Response<GetNikeReportExportRequest> GetNikeReportQutry(GetNikeReportExportRequest request)
        {
            Response<GetNikeReportExportRequest> response = new Response<GetNikeReportExportRequest>() { Result = new GetNikeReportExportRequest() };

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetNikeReportQutry request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                NikeAccessor accessor = new NikeAccessor();
                int Rowcount;
                response.Result.NikeReport = accessor.GetNikeReportQuery(request.SqlWhere,request.ReportName, request.PageIndex, request.PageSize, out Rowcount);
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

        public Response<IEnumerable<PodAll>> GetNikeExportPodAllByCondition(GetNikeExportPodAllByConditionRequest request)
        {
            Response<IEnumerable<PodAll>> response = new Response<IEnumerable<PodAll>>();
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetNikeExportPodAllByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                NikeAccessor accessor = new NikeAccessor();
                response.Result = accessor.GetNikeExportPodAllByCondition(request.Condition);
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


        public Response<UpdateNikePodAndGetTheDifferenceResponse> UpdateNikePodAndGetTheDifference(UpdateNikePodAndGetTheDifferenceRequest request)
        {
            Response<UpdateNikePodAndGetTheDifferenceResponse> response = new Response<UpdateNikePodAndGetTheDifferenceResponse>() { Result = new UpdateNikePodAndGetTheDifferenceResponse() };

            if (request == null || request.Pods == null || !request.Pods.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateNikePodAndGetTheDifference request Pods");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                NikeAccessor accessor = new NikeAccessor();

                response.Result = accessor.UpdateNikePodAndGetTheDifference(request.Pods);

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
        /// 箱数件数同步
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<UpdateNikePodBGAndGetTheDifferenceResponse> UpdateNikePodBGAndGetTheDifference(UpdateNikePodBGAndGetTheDifferenceRequest request)
        {
            Response<UpdateNikePodBGAndGetTheDifferenceResponse> response = new Response<UpdateNikePodBGAndGetTheDifferenceResponse>() { Result = new UpdateNikePodBGAndGetTheDifferenceResponse() };

            if (request == null || request.Pods == null || !request.Pods.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("UpdateNikePodAndGetTheDifference request Pods");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                NikeAccessor accessor = new NikeAccessor();

                response.Result = accessor.UpdateNikePodBGAndGetTheDifference(request.Pods);

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
        public Response<NikePODForBSResponses> GetNikePOD(NikePODForBSRequest request)
        {
            Response<NikePODForBSResponses> response = new Response<NikePODForBSResponses>() { Result = new NikePODForBSResponses() };
            try
            {
                NikeAccessor accessor = new NikeAccessor();
                int Rowcount = 0;
                response.Result.PodCollection = accessor.GetNikePOD(request.Condition, request.PageIndex, request.PageSize, out Rowcount);//
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

        public string AddNikePodForBS(NikePODForBSRequest request)
        {
            Response<NikePODForBSResponses> response = new Response<NikePODForBSResponses>() { Result = new NikePODForBSResponses() };
            string str = "";
            try
            {
                NikeAccessor accessor = new NikeAccessor();
                int RowCount = 0;
                str = accessor.AddNikePodForBS(request.PodCollection, request.UserName, request.ShipperName);//, request.PageIndex, request.PageSize,out RowCount
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }
            return str;
        }

        public string CancelNikePodForBS(NikePODForBSRequest request)
        {
            Response<NikePODForBSResponses> response = new Response<NikePODForBSResponses>() { Result = new NikePODForBSResponses() };
            string str = "";
            try
            {
                NikeAccessor accessor = new NikeAccessor();
                int RowCount = 0;
                str = accessor.CancelNikePodForBS(request.PodCollection);//, request.PageIndex, request.PageSize,out RowCount

                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Exception = ex;
            }
            return str;
        }
    }
}
