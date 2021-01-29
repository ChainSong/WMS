using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System.Data;
using Runbow.TWS.MessageContracts.WMS.Template;

namespace Runbow.TWS.Biz.WMS
{
    public class TemplateManagementService : BaseService
    {

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetTemplateByConditionResponse> GetTemplateByCondition(GetTemplateByConditionRequest request)
        {
            Response<GetTemplateByConditionResponse> response = new Response<GetTemplateByConditionResponse>() { Result = new GetTemplateByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetTemplateByConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                TemplateManagementAccessor accessor = new TemplateManagementAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.TemplateCollection = accessor.GetTemplateByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                    response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                    response.Result.PageIndex = request.PageIndex;
                }
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


        public Response<GetTemplateByConditionResponse> GetTemplateDetailByCondition(GetTemplateByConditionRequest request)
        {
            Response<GetTemplateByConditionResponse> response = new Response<GetTemplateByConditionResponse>() { Result = new GetTemplateByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetTemplateDetailByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                TemplateManagementAccessor accessor = new TemplateManagementAccessor();
              
                    response.Result.TemplateCollection = accessor.GetTemplateDetailByCondition(request.SearchCondition);
                  
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


        public Response<string> EditTemplateDetail(GetTemplateByConditionRequest request)
        {
            Response<string> response = new Response<string>();
            string message = "";
            if (request == null || request.tableColumns == null || !request.tableColumns.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("EditTemplateDetail request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                message = new TemplateManagementAccessor().EditTemplateDetail(request);
                if (message == "更新成功"||message=="")
                {
                    response.Result = message;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Result = message;
                    response.IsSuccess = false;

                }
                return response;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Result = message + ex.Message;
            }

            return response;
        }

    }
}
