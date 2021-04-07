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
    public class AMSUploadService : BaseService
    {
        /// <summary>
        /// 获取2月内已上传回单
        /// </summary>
        public Response<IEnumerable<AMSUpload>> GetAMSUpload(AddAMSUploadRequest request)
        {
            StringBuilder names = new StringBuilder();
            StringBuilder project = new StringBuilder();
            if (request.amsUpload != null)
            {
                foreach (AMSUpload a in request.amsUpload)
                {
                    names.Append("'" + a.FileName + "',");
                    if (!project.ToString().Contains(a.ProjectName))
                    {
                        project.Append("'" + a.ProjectName + "'");
                    }
                }
            }
            else
            {
                names.Append("0");
            }


            Response<IEnumerable<AMSUpload>> response = new Response<IEnumerable<AMSUpload>>();
            try
            {
                response.Result = new AMSUploadAccessor().GetAMSUpload(names.ToString() == "0" ? "0" : names.ToString().TrimEnd(','), project.ToString());
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
        /// 获取2月内现有回单
        /// </summary>
        public Response<IEnumerable<AMSUpload>> GetAMSUploadOrPODInfo()
        {
            Response<IEnumerable<AMSUpload>> response = new Response<IEnumerable<AMSUpload>>();
            try
            {
                response.Result = new AMSUploadAccessor().GetAMSUploadOrPODInfo();
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
        /// 获取已上传附件运单记录
        /// </summary>
        public Response<IEnumerable<Attachment>> GetAttachmentOrAMS()
        {
            Response<IEnumerable<Attachment>> response = new Response<IEnumerable<Attachment>>();
            try
            {
                response.Result = new AMSUploadAccessor().GetAttachmentOrAMS();
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
        /// 手动同步回单
        /// </summary>
        public void InsertAMSUpload()
        {
            try
            {
                new AMSUploadAccessor().InsertAMSUpload();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }


        /// <summary>
        /// 验证通过修改验证状态
        /// </summary>
        public void UpdateAMSUploadStatus(AddAMSUploadRequest request)
        {
            try
            {
                new AMSUploadAccessor().UpdateAMSUploadStatus(request.Ids);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        /// <summary>
        /// 新增回单
        /// </summary>
        public Response<IEnumerable<AMSUpload>> AddAMSUpload(AddAMSUploadRequest request)
        {
            Response<IEnumerable<AMSUpload>> response = new Response<IEnumerable<AMSUpload>>();

            if (request == null || request.amsUpload == null || !request.amsUpload.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddAMSUpload request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                AMSUploadAccessor accessor = new AMSUploadAccessor();
                response.Result = accessor.AddAttachments(request.amsUpload);
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

        public Response<QueryAMSUploadResponses> QueryAMSUpload(QueryAMSUploadRequests request)
        {
            Response<QueryAMSUploadResponses> response = new Response<QueryAMSUploadResponses>() { Result = new QueryAMSUploadResponses() };
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("QueryAMSUpload request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                AMSUploadAccessor accessor = new AMSUploadAccessor();
                int RowCount;
                response.Result.AMSUploadCollection = accessor.GetQueryAttachments(request.SearchCondition, request.Customers, request.PageIndex, request.PageSize, out RowCount);
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
        /// 订单号的查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GenBoxNumberResponse> QueryGenBoxNumber(GenBoxNumberRequest request)
        {
            Response<GenBoxNumberResponse> response = new Response<GenBoxNumberResponse> { Result = new GenBoxNumberResponse() };
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("QueryAMSUpload request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                AMSUploadAccessor accessor = new AMSUploadAccessor();
                response.Result.AMSUploadCollection = accessor.GetBoxNumberAttachments(request.SearchCondition, request.Customers);
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

        public Response<GenBoxNumberResponse> AddGenBoxNumber(GenBoxNumberRequest request)
        {
            Response<GenBoxNumberResponse> response = new Response<GenBoxNumberResponse> { Result = new GenBoxNumberResponse() };
            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddPackingLists request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                AMSUploadAccessor accessor = new AMSUploadAccessor();
                response.Result.AMSUploadCollection = accessor.AddAMSController(request.Check.Substring(0, request.Check.Length - 1));
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


        public Response<AMSUpload> GetAttachmentByID(GetAMSUPLOADByIDRequest request)
        {
            Response<AMSUpload> response = new Response<AMSUpload>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetAttachmentByID request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                AMSUploadAccessor accessor = new AMSUploadAccessor();
                response.Result = accessor.GetAmsUploadByID(request.ID);
                if (response.Result.ID == 0)
                {
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Argument;
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


        /// <summary>
        /// 成品订单查询
        /// </summary>
        public Response<QueryAMSUploadResponses> GetWMS_PackageByCondition(QueryAMSUploadRequests request)
        {
            Response<QueryAMSUploadResponses> response = new Response<QueryAMSUploadResponses>() { Result = new QueryAMSUploadResponses() };
            if (request == null || request.WMS_PackageSearch == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetWMS_PackageByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                AMSUploadAccessor accessor = new AMSUploadAccessor();
                int RowCount;
                response.Result.WMS_PackageCollection = accessor.GetWMS_PackageByCondition(request.WMS_PackageSearch, request.Customers, request.PageIndex, request.PageSize, out RowCount);
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

    }
}
