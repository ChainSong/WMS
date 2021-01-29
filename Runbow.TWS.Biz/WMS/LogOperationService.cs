using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.LogOperation;

namespace Runbow.TWS.Biz.WMS
{
    public class LogOperationService : BaseService
    {
        public Response<string> AddLogOperation(IEnumerable<WMS_Log_Operation> LogOperations)
        {
            Response<string> response = new Response<string>();
            string message = "";
            if (LogOperations == null || !LogOperations.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddLogOperation request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {

                message = new LogOperationAccessor().AddLogOperation(LogOperations);
                if (message.Contains("添加成功"))
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
        public Response<string> AddLogOperationRF(IEnumerable<WMS_Log_OperationRF> LogOperations)
        {
            Response<string> response = new Response<string>();
            string message = "";
            if (LogOperations == null || !LogOperations.Any())
            {
                ArgumentNullException ex = new ArgumentNullException("AddLogOperation request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {

                message = new LogOperationAccessor().AddLogOperationRF(LogOperations);
                if (message.Contains("添加成功"))
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
        /// <summary>
        /// 一箱完成后更新日志表箱状态为1
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Response<string> UpdateLogOperationPackageStatusRF(string userName, string BoxNumber)
        {
            Response<string> response = new Response<string>();
            string message = "";
            try
            {

                message = new LogOperationAccessor().UpdateLogOperationPackageStatusRF(userName, BoxNumber);
                if (message.Contains("更新成功"))
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
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetLogOperationByConditionResponse> GetLogOperationByCondition(GetLogOperationByConditionRequest request)
        {
            Response<GetLogOperationByConditionResponse> response = new Response<GetLogOperationByConditionResponse>() { Result = new GetLogOperationByConditionResponse() };

            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetLogOperationByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                LogOperationAccessor accessor = new LogOperationAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.LogOperationCollection = accessor.GetLogOperationByCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
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

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetLogOperationByConditionResponse> GetLogOperationRFByCondition(GetLogOperationByConditionRequest request)
        {
            Response<GetLogOperationByConditionResponse> response = new Response<GetLogOperationByConditionResponse>() { Result = new GetLogOperationByConditionResponse() };

            if (request == null || request.RFSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetLogOperationRFByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                LogOperationAccessor accessor = new LogOperationAccessor();
                int RowCount;
                if (request.PageSize > 0)
                {
                    response.Result.LogOperationRFCollection = accessor.GetLogOperationRFByCondition(request.RFSearchCondition, request.PageIndex, request.PageSize, out RowCount);
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
        public Response<GetLogOperationByConditionResponse> ExportLogOperationRFByCondition(GetLogOperationByConditionRequest request)
        {
            Response<GetLogOperationByConditionResponse> response = new Response<GetLogOperationByConditionResponse>() { Result = new GetLogOperationByConditionResponse() };

            if (request == null || request.RFSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("ExportLogOperationRFByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                LogOperationAccessor accessor = new LogOperationAccessor();
                response.Result.LogOperationRFCollection = accessor.ExportLogOperationRFByCondition(request.RFSearchCondition);
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


        /// <summary>
        /// cord中间表订单日志
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        public bool AddNikeCordOrderLog(List<NikeCrodOrderLog> logs)
        {
            try
            {
                return new LogOperationAccessor().AddNikeCordOrderLog(logs);
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// 退货仓sftp日志 多条
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        public bool AddNikeReturnSFTPLogs(List<WMS_NikeReturnSFTP_Log> logs)
        {
            try
            {
                return new LogOperationAccessor().AddNikeReturnSFTPLogs(logs);
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// 退货仓sftp日志单条
        /// </summary>
        /// <param name="logs"></param>
        /// <returns></returns>
        public bool AddNikeReturnSFTPLog(WMS_NikeReturnSFTP_Log log)
        {
            try
            {
                List<WMS_NikeReturnSFTP_Log> logs = new List<WMS_NikeReturnSFTP_Log>();
                logs.Add(log);
                return new LogOperationAccessor().AddNikeReturnSFTPLogs(logs);
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }
}
