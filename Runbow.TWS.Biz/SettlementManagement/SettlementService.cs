using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using System.Data;
using Runbow.TWS.Entity.WMS.SettlementManagement;

namespace Runbow.TWS.Biz
{
    public class SettlementService : BaseService
    {
        public Response<GetSettlementByConditionResponse> GetSettlement(GetSettlementByConditionRequest request)
        {
            Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>() { Result = new GetSettlementByConditionResponse() };

            if (request.WLSearchCondition == null || request.WLSearchCondition.CustomerID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSettlement request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                int RowCount;
                response.Result.SettlementCollection = new SettlementAccessor().GetSettlement(request.WLSearchCondition, out RowCount);
                response.Result.PageCount = RowCount;
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

        public Response<GetSettlementByConditionResponse> GetSettlementNew(GetSettlementByConditionRequest request)
        {
            Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>() { Result = new GetSettlementByConditionResponse() };

            if (request.WLSearchCondition == null || request.WLSearchCondition.CustomerID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSettlement request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                int RowCount;
                response.Result.SettlementDetailCollection = new SettlementAccessor().GetSettlementNew(request.WLSearchCondition, out RowCount);
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = RowCount % request.WLSearchCondition.PageSize == 0 ? (RowCount / request.WLSearchCondition.PageSize) : (RowCount / request.WLSearchCondition.PageSize) + 1;
                response.Result.RowCount = RowCount;
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

        public Response<GetSettlementByConditionResponse> GetSettlementSave(GetSettlementByConditionRequest request)
        {
            Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>() { Result = new GetSettlementByConditionResponse() };
            string News = string.Empty;
            try
            {
                News = new SettlementAccessor().GetSettlementSave(request.WLSearchCondition);
                response.IsSuccess = true;
                response.Result.Message = News;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Result.Message = "操作失败(" + ex.Message + ")";
            }

            return response;
        }

        public Response<GetSettlementByConditionResponse> GetSettlementPreview(GetSettlementByConditionRequest request)
        {
            Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>() { Result = new GetSettlementByConditionResponse() };
            string News = string.Empty;
            try
            {
                response.Result = new SettlementAccessor().GetSettlementPreview(request.WLSearchCondition);
                response.IsSuccess = true;
                response.Result.Message = "提交成功";
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
                response.Result.Message = "提交失败";
            }

            return response;
        }

        public Response<GetSettlementByConditionResponse> GetSettlementList(GetSettlementByConditionRequest request)
        {
            Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>();

            try
            {
                int RowCount;
                response.Result = new SettlementAccessor().GetSettlementList(request.WLSearchCondition, out RowCount);
                response.Result.PageCount = RowCount % request.WLSearchCondition.PageSize == 0 ? RowCount / request.WLSearchCondition.PageSize : RowCount / request.WLSearchCondition.PageSize + 1;
                response.Result.PageIndex = request.WLSearchCondition.PageIndex;
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

        public Response<GetSettlementByConditionResponse> GetSettlementListPay(GetSettlementByConditionRequest request)
        {
            Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>();

            try
            {
                int RowCount;
                response.Result = new SettlementAccessor().GetSettlementListPay(request.WLSearchCondition, out RowCount);
                response.Result.PageCount = RowCount % request.WLSearchCondition.PageSize == 0 ? RowCount / request.WLSearchCondition.PageSize : RowCount / request.WLSearchCondition.PageSize + 1;
                response.Result.PageIndex = request.WLSearchCondition.PageIndex;
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

        public Response<GetSettlementByConditionResponse> GetSettlementBySettlementNumber(GetSettlementByConditionRequest request)
        {
            Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>();

            try
            {
                response.Result = new SettlementAccessor().GetSettlementBySettlementNumber(request.WLSearchCondition);
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

        public Response<GetSettlementByConditionResponse> ExportSettlementBySettlementNumber(GetSettlementByConditionRequest request)
        {
            Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>();

            try
            {
                response.Result = new SettlementAccessor().ExportSettlementBySettlementNumber(request.WLSearchCondition);
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

        public string SaveSettlementBySettlementNumber(GetSettlementByConditionRequest request)
        {
            string Message = string.Empty;
            try
            {
                Message = new SettlementAccessor().SaveSettlementBySettlementNumber(request.WLSearchCondition);
            }
            catch (Exception ex)
            {
                return "更新错误";
            }
            return Message;

        }

        public string GetSettlementDelete(GetSettlementByConditionRequest request)
        {
            string Message = string.Empty;
            try
            {
                Message = new SettlementAccessor().GetSettlementDelete(request.WLSearchCondition);
            }
            catch (Exception ex)
            {
                return "更新错误";
            }
            return Message;

        }

        public string GetSettlementDone(GetSettlementByConditionRequest request)
        {
            string Message = string.Empty;
            try
            {
                Message = new SettlementAccessor().GetSettlementDone(request.WLSearchCondition);
            }
            catch (Exception ex)
            {
                return "更新错误" + ex.Message;
            }
            return Message;

        }

        //批量导出
        public DataSet SummaryExportSettlementBySettlementNumber_b(GetSettlementByConditionRequest request)
        {
            DataSet response = new DataSet();

            try
            {
                response = new SettlementAccessor().SummaryExportSettlementBySettlementNumber_b(request.WLSearchCondition);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return response;
        }


        //导出
        public DataSet ExportSettlementBySettlementNumber_b(GetSettlementByConditionRequest request)
        {
            DataSet response = new DataSet();

            try
            {
                response = new SettlementAccessor().ExportSettlementBySettlementNumber_b(request.WLSearchCondition);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return response;
        }

        public Response<GetSettlementByConditionResponse> GetPrintSettlementBySettlementNumber(string Settlementnumber)
        {
            Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>() { Result = new GetSettlementByConditionResponse() };

            try
            {
                response.Result = new SettlementAccessor().GetPrintSettlementBySettlementNumber(Settlementnumber);
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

        public Response<GetSettlementByConditionResponse> GetPrintSettlementBySettlementNumberNike(string Settlementnumber)
        {
            Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>() { Result = new GetSettlementByConditionResponse() };

            try
            {
                response.Result = new SettlementAccessor().GetPrintSettlementBySettlementNumberNike(Settlementnumber);
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
        /// 20190315
        /// </summary>
        /// <param name="cb"></param>
        /// <returns></returns>
        public Response<bool> AddInfoHiltiNewTab(WMS_HiltibjSettled cb)
        {
            Response<bool> response = new Response<bool>();
            try
            {
                new SettlementAccessor().AddInfoHiltiNewTab(cb);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;

            }
            return response;
        }


        public Response<IEnumerablerResult> Count(string creatdate,int CustomerID)
        {
            Response<IEnumerablerResult> response = new Response<IEnumerablerResult>() { Result = new IEnumerablerResult() };

            try
            {
                response.Result = new SettlementAccessor().Count(creatdate, CustomerID);
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

        public Response<GetSettlementByConditionResponse> GetHiltiList(GetSettlementByConditionRequest request, DateTime? StartSettlementdate, DateTime? EndSettlementdate, string DateTime1,int Cid)
        {
            Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>();

            try
            {
                int RowCount;
                response.Result = new SettlementAccessor().GetHiltiList(request.HiltibjSettled, StartSettlementdate, EndSettlementdate, DateTime1, Cid, out RowCount);
                response.Result.PageCount = RowCount % request.HiltibjSettled.PageSize == 0 ? RowCount / request.HiltibjSettled.PageSize : RowCount / request.HiltibjSettled.PageSize + 1;
                response.Result.PageIndex = request.HiltibjSettled.PageIndex;
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
        //删除
        public int DeleteHiltibjSettled(string ID)
        {
            int a = 0;
            try
            {
                a = new SettlementAccessor().DeleteHiltibjSettled(ID);
            }
            catch (Exception ex)
            {

                throw;
            }

            return a;
        }

        //导出
        public DataTable ExportSettlement(GetSettlementByConditionRequest request, DateTime? StartSettlementdate, DateTime? EndSettlementdate, string DateTime1)
        {
            DataTable dt = new DataTable();
            try
            {
                int RowCount;
                dt = new SettlementAccessor().ExportSettlement(request.HiltibjSettled, StartSettlementdate, EndSettlementdate, DateTime1, out RowCount);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return dt;
        }
    }
}
