using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Report;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.Report;

namespace Runbow.TWS.Biz.WMS
{
    public class ReportManagementService : BaseService
    {
        public Response<GetSkuBySearchConditionResponse> GetSkuBySearchCondition(GetSkuBySearchConditionRequest request)
        {
            Response<GetSkuBySearchConditionResponse> response = new Response<GetSkuBySearchConditionResponse>() { Result = new GetSkuBySearchConditionResponse() };

            if (request == null && request.SkuSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSkuBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.SkuCollection = accessor.GetSkuBySearchCondition(request.SkuSearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
        public Response<GetSkuBySearchConditionResponse> ExportSkuBySearchCondition(GetSkuBySearchConditionRequest request)
        {
            Response<GetSkuBySearchConditionResponse> response = new Response<GetSkuBySearchConditionResponse>() { Result = new GetSkuBySearchConditionResponse() };

            if (request == null && request.SkuSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSkuBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.SkuCollection = accessor.ExportSkuBySearchCondition(request.SkuSearchCondition);
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


        public Response<GetReportTableBySearchConditionResponse> GetResponTableBySearchCondition(GetReportTableBySearchConditionRequest request)
        {
            Response<GetReportTableBySearchConditionResponse> response = new Response<GetReportTableBySearchConditionResponse>() { Result = new GetReportTableBySearchConditionResponse() };

            if (request == null && request.TableSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReportTableBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.ReprotTableInCollection = accessor.GetReprotTableBySearchCondition(request.TableSearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;

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
        /// 查询出库单报表
        /// </summary>
        /// <param name="request">查询条件</param>
        /// <param name="proc">存储过程变成可配置wms_config</param>
        /// <returns></returns>
        public Response<GetReportTableBySearchConditionResponse> GetResponTableBySearchCondition(GetReportTableBySearchConditionRequest request, string Proc)
        {
            Response<GetReportTableBySearchConditionResponse> response = new Response<GetReportTableBySearchConditionResponse>() { Result = new GetReportTableBySearchConditionResponse() };

            if (request == null && request.TableSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReportTableBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.ReprotTableInCollection = accessor.GetReprotTableBySearchCondition(request.TableSearchCondition, request.PageIndex, request.PageSize, out RowCount, Proc);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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

        public Response<GetReportTableBySearchConditionResponse> ExportResponTableBySearchCondition(GetReportTableBySearchConditionRequest request)
        {
            Response<GetReportTableBySearchConditionResponse> response = new Response<GetReportTableBySearchConditionResponse>() { Result = new GetReportTableBySearchConditionResponse() };

            if (request == null && request.TableSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReportTableBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.ReprotTableInCollection = accessor.ExportReprotTableBySearchCondition(request.TableSearchCondition);
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
        /// 导出出库单报表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Proc">存储过程可配置</param>
        /// <returns></returns>
        public Response<GetReportTableBySearchConditionResponse> ExportResponTableBySearchCondition(GetReportTableBySearchConditionRequest request, string Proc)
        {
            Response<GetReportTableBySearchConditionResponse> response = new Response<GetReportTableBySearchConditionResponse>() { Result = new GetReportTableBySearchConditionResponse() };

            if (request == null && request.TableSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReportTableBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.ReprotTableInCollection = accessor.ExportReprotTableBySearchCondition(request.TableSearchCondition, Proc);
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
        /// 快递单报表导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetReportExpressBySearchConditionResponse> ExportResponExpressBySearchCondition(GetReportExpressBySearchConditionRequest request)
        {
            Response<GetReportExpressBySearchConditionResponse> response = new Response<GetReportExpressBySearchConditionResponse>() { Result = new GetReportExpressBySearchConditionResponse() };

            if (request == null && request.ExpressSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReportTableBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.ReprotExpressInfoCollection = accessor.ExportReprotExpressBySearchCondition(request.ExpressSearchCondition);
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
        /// 查询快递单报表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetReportExpressBySearchConditionResponse> GetResponExpressBySearchCondition(GetReportExpressBySearchConditionRequest request)
        {
            Response<GetReportExpressBySearchConditionResponse> response = new Response<GetReportExpressBySearchConditionResponse>() { Result = new GetReportExpressBySearchConditionResponse() };

            if (request == null && request.ExpressSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReportTableBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.ReprotExpressInfoCollection = accessor.GetReprotExpressBySearchCondition(request.ExpressSearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;

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
        /// 查询门店
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetInventoryBySearchConditionResponse> GetWms_CustomerByCustomreID(GetInventoryBySearchConditionRequest request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            if (request == null && request.InventorySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInventoryBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.WMS_CustomerCollection = accessor.GetWMS_CustomerByCustomerID(request.InventorySearchCondition);

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
        /// 获取耐克需要发送的报表
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public Response<GetInventoryBySearchConditionResponse> GetNikeReport(string StoreID, string CustomerID, string start_CompleteDate, string end_CompleteDate)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result = accessor.GetNikeReport(StoreID, CustomerID, start_CompleteDate, end_CompleteDate);
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
        /// 获取耐克需要发送的邮件里的内容
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public Response<GetInventoryBySearchConditionResponse> GetNikeReportEmail(string StoreID, string CustomerID, string start_CompleteDate, string end_CompleteDate)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result = accessor.GetNikeReportEmail(StoreID, CustomerID, start_CompleteDate, end_CompleteDate);
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
        /// 获取耐克需要发送的邮件里的内容-EpackList
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public Response<GetInventoryBySearchConditionResponse> GetEpackListEmail(string StoreID, string CustomerID, string start_CompleteDate, string end_CompleteDate)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result = accessor.GetEpackListEmail(StoreID, CustomerID, start_CompleteDate, end_CompleteDate);
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
        /// 获取NIke每日EpackList
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public Response<GetInventoryBySearchConditionResponse> GetNikeEpackListReport(string StoreID, string CustomerID, string DriverName, string DriverTel, string CarNo, string ExpectTime, string start_CompleteDate, string end_CompleteDate)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result = accessor.GetNikeEpackListReport(StoreID, CustomerID, DriverName, DriverTel, CarNo, ExpectTime, start_CompleteDate, end_CompleteDate);
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

        //库存报表
        public Response<GetInventoryBySearchConditionResponse> GetInventoryBySearchCondition(GetInventoryBySearchConditionRequest request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            if (request == null && request.InventorySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInventoryBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.InventoryCollection = accessor.GetInventoryBySearchCondition(request.InventorySearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
        /// 库存汇总查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetInventoryBySearchConditionResponse> GetInventorysummaryBySearchCondition(GetInventoryBySearchConditionRequest request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            if (request == null && request.InventorySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInventoryBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.InventorySummaryCollection = accessor.GetInventorySummaryBySearchCondition(request.InventorysummarySearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
        /// 库存汇总查询导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetInventoryBySearchConditionResponse> ExportInventorySummaryBySearchCondition(GetInventoryBySearchConditionRequest request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            if (request == null && request.InventorySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("ExportInventoryBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.InventorySummaryCollection = accessor.ExportInventorySummaryBySearchCondition(request.InventorysummarySearchCondition);
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

        public Response<GetInventoryBySearchConditionResponse> ExportInventoryBySearchCondition(GetInventoryBySearchConditionRequest request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            if (request == null && request.InventorySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("ExportInventoryBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.InventoryCollection = accessor.ExportInventoryBySearchCondition(request.InventorySearchCondition);
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

        public DataTable ExportInventoryBySearchCondition2(GetInventoryBySearchConditionRequest request)
        {
            DataTable dt = new DataTable();
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            if (request == null && request.InventorySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("ExportInventoryBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return null;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                dt = accessor.ExportInventoryBySearchCondition2(request.InventorySearchCondition);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }

            return dt;

        }

        public Response<GetSkuInAndOutBySearchConditionResponse> GetSkuInAndOutBySearchCondition(GetSkuInAndOutBySearchConditionRequest request)
        {
            Response<GetSkuInAndOutBySearchConditionResponse> response = new Response<GetSkuInAndOutBySearchConditionResponse>() { Result = new GetSkuInAndOutBySearchConditionResponse() };

            if (request == null && request.SkuInAndOutSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSkuInAndOutBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.SkuInAndOutCollection = accessor.GetSkuInAndOutBySearchCondition(request.SkuInAndOutSearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
        public Response<GetSkuInAndOutBySearchConditionResponse> ExportSkuInAndOutBySearchCondition(GetSkuInAndOutBySearchConditionRequest request)
        {
            Response<GetSkuInAndOutBySearchConditionResponse> response = new Response<GetSkuInAndOutBySearchConditionResponse>() { Result = new GetSkuInAndOutBySearchConditionResponse() };

            if (request == null && request.SkuInAndOutSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("ExportSkuInAndOutBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.SkuInAndOutCollection = accessor.ExportSkuInAndOutBySearchCondition(request.SkuInAndOutSearchCondition);
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
        /// SKU变动报表查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetSKUChangeBySearchConditionResponse> GetSKUChangeBySearchCondition(GetSKUChangeBySearchConditionRequest request)
        {
            Response<GetSKUChangeBySearchConditionResponse> response = new Response<GetSKUChangeBySearchConditionResponse>() { Result = new GetSKUChangeBySearchConditionResponse() };

            if (request == null && request.SKUChangeSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetSKUChangeBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.SkuChangeCollection = accessor.GetSKUChangeBySearchCondition(request.SKUChangeSearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
        /// SKU变动报表导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetSKUChangeBySearchConditionResponse> ExportSKUChangeBySearchCondition(GetSKUChangeBySearchConditionRequest request)
        {
            Response<GetSKUChangeBySearchConditionResponse> response = new Response<GetSKUChangeBySearchConditionResponse>() { Result = new GetSKUChangeBySearchConditionResponse() };

            if (request == null && request.SKUChangeSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("ExportSKUChangeBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.SkuChangeCollection = accessor.ExportSKUChangeBySearchCondition(request.SKUChangeSearchCondition);
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



        public Response<GetInventoryChangeBySearchConditionResponse> GetInventoryChangeBySearchCondition(GetInventoryChangeBySearchConditionRequest request)
        {
            Response<GetInventoryChangeBySearchConditionResponse> response = new Response<GetInventoryChangeBySearchConditionResponse>() { Result = new GetInventoryChangeBySearchConditionResponse() };

            if (request == null && request.InventoryChangeSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInventoryChangeBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.InventoryChangeCollection = accessor.GetInventoryChangeBySearchCondition(request.InventoryChangeSearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
        public Response<GetInventoryChangeBySearchConditionResponse> ExportInventoryChangeBySearchCondition(GetInventoryChangeBySearchConditionRequest request)
        {
            Response<GetInventoryChangeBySearchConditionResponse> response = new Response<GetInventoryChangeBySearchConditionResponse>() { Result = new GetInventoryChangeBySearchConditionResponse() };

            if (request == null && request.InventoryChangeSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("ExportInventoryChangeBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.InventoryChangeCollection = accessor.ExportInventoryChangeBySearchCondition(request.InventoryChangeSearchCondition);
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

        public Response<GetWarehouseStorageDensityBySearchConditionResponse> GetWarehouseStorageDensityBySearchCondition(GetWarehouseStorageDensityBySearchConditionRequest request)
        {
            Response<GetWarehouseStorageDensityBySearchConditionResponse> response = new Response<GetWarehouseStorageDensityBySearchConditionResponse>() { Result = new GetWarehouseStorageDensityBySearchConditionResponse() };

            if (request == null && request.WarehouseStorageDensitySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetWarehouseStorageDensityBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.WarehouseStorageDensityCollection = accessor.GetWarehouseStorageDensityBySearchCondition(request.WarehouseStorageDensitySearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
        public Response<GetWarehouseStorageDensityBySearchConditionResponse> ExportWarehouseStorageDensityBySearchCondition(GetWarehouseStorageDensityBySearchConditionRequest request)
        {
            Response<GetWarehouseStorageDensityBySearchConditionResponse> response = new Response<GetWarehouseStorageDensityBySearchConditionResponse>() { Result = new GetWarehouseStorageDensityBySearchConditionResponse() };

            if (request == null && request.WarehouseStorageDensitySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("ExportWarehouseStorageDensityBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.WarehouseStorageDensityCollection = accessor.ExportWarehouseStorageDensityBySearchCondition(request.WarehouseStorageDensitySearchCondition);
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

        public Response<GetInventoryBySearchConditionResponse> PrintUpdateReceipt(long ID, IList<ReceiptDetail> request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };
            //string message = string.Empty;
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.receiptPrint = accessor.PrintUpdateReceipt(ID, request);
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

        public Response<GetInventoryBySearchConditionResponse> GetPrintLabel(IList<ReceiptDetail> request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };
            //string message = string.Empty;
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.receiptPrint = accessor.GetPrintLabel(request);
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
        public Response<GetInventoryBySearchConditionResponse> GetPrintLabelInventorySearch(IList<ReceiptDetail> request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };
            //string message = string.Empty;
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.receiptPrint = accessor.GetPrintLabelInventorySearch(request);
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
        /// NIke每日报表邮件发送成功之后插入表中
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public string InsertReportSendEmail(string StoreID, string CustomerID)
        {
            string message = string.Empty;
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                message = accessor.InsertReportSendEmail(StoreID, CustomerID);

            }
            catch (Exception ex)
            {
                LogError(ex);
                message = ex.Message;
            }
            return message;
        }
        /// <summary>
        /// NIke EpackList邮件发送成功之后插入表中
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public string InsertEpackListSendEmail(string StoreID, string CustomerID, string start_CompleteDate, string end_CompleteDate)
        {
            string message = string.Empty;
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                message = accessor.InsertEpackListSendEmail(StoreID, CustomerID, start_CompleteDate, end_CompleteDate);

            }
            catch (Exception ex)
            {
                LogError(ex);
                message = ex.Message;
            }
            return message;
        }
        public Response<GetInventoryBySearchConditionResponse> PrintUpdateNumber(long ID, string BoxNumber, long EverySum)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };
            //string message = string.Empty;
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.receiptPrint = accessor.PrintUpdateNumber(ID, BoxNumber, EverySum);
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
            //string message = string.Empty;
            //try
            //{
            //    ReportManagementAccessor accessor = new ReportManagementAccessor();
            //    message = accessor.PrintUpdateNumber(ID, BoxNumber, EverySum);

            //}
            //catch (Exception ex)
            //{
            //    message = ex.Message;
            //}

            //return message;
        }

        public string GetLocationMax(string Location, long warehouseid)
        {
            string val = "";
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                val = accessor.GetLocationMax(Location, warehouseid);


            }
            catch (Exception ex)
            {
                LogError(ex);

            }

            return val;
        }
        /// <summary>
        /// 出库单差异报表查询
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Proc"></param>
        /// <returns></returns>
        public Response<GetReportTableBySearchConditionResponse> GetResponTableDifferenceBySearch(GetReportTableBySearchConditionRequest request, string Proc)
        {
            Response<GetReportTableBySearchConditionResponse> response = new Response<GetReportTableBySearchConditionResponse>() { Result = new GetReportTableBySearchConditionResponse() };

            if (request == null && request.TableSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReportTableBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.ReprotTableInCollection = accessor.GetReprotTableDifferenceBySearch(request.TableSearchCondition, request.PageIndex, request.PageSize, out RowCount, Proc);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
        /// 导出出库单差异报表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Proc">存储过程可配置</param>
        /// <returns></returns>
        public Response<GetReportTableBySearchConditionResponse> ExportResponTableDifferenceBySearch(GetReportTableBySearchConditionRequest request, string Proc)
        {
            Response<GetReportTableBySearchConditionResponse> response = new Response<GetReportTableBySearchConditionResponse>() { Result = new GetReportTableBySearchConditionResponse() };

            if (request == null && request.TableSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReportTableBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.ReprotTableInCollection = accessor.ExportReprotTableDifferenceBySearch(request.TableSearchCondition, Proc);
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
        /// 入库单差异报表查询
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Proc"></param>
        /// <returns></returns>
        public Response<GetReceiptDetailByConditionResponse> GetReportReceiptDiffBySearch(GetReceiptByConditionRequest request, string Proc)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };

            if (request == null && request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReportTableBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.ReceiptDetailCollection3 = accessor.GetReportReceiptDiffBySearch(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount, Proc);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
        /// 导出入库单差异报表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Proc">存储过程可配置</param>
        /// <returns></returns>
        public Response<GetReceiptDetailByConditionResponse> ExportReportReceiptDifferBySearch(GetReceiptByConditionRequest request, string Proc)
        {
            Response<GetReceiptDetailByConditionResponse> response = new Response<GetReceiptDetailByConditionResponse>() { Result = new GetReceiptDetailByConditionResponse() };

            if (request == null && request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReportTableBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.ReceiptDetailCollection3 = accessor.ExportReportReceiptDifferBySearch(request.SearchCondition, Proc);
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
        /// 门店库存查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetInventoryBySearchConditionResponse> GetInventoryStorerBySearchCondition(GetInventoryBySearchConditionRequest request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };
            if (request == null && request.InventorySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInventoryStorerBySearchCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.InventoryCollection = accessor.GetInventoryStorerBySearchCondition(request.InventorySearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
        /// 门店库存导出查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetInventoryBySearchConditionResponse> ExportInventoryStorerBySearchCondition(GetInventoryBySearchConditionRequest request)
        {
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>() { Result = new GetInventoryBySearchConditionResponse() };

            if (request == null && request.InventorySearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("ExportInventoryStorerBySearchCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.InventoryCollection = accessor.ExportInventoryStorerBySearchCondition(request.InventorySearchCondition);
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
        /// 工时统计查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetWorkingStatisResponse> GetWorkingStatisBySearchCondition(GetWorkingStatisRequest request)
        {
            Response<GetWorkingStatisResponse> response = new Response<GetWorkingStatisResponse>() { Result = new GetWorkingStatisResponse() };
            if (request == null && request.WorkingSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInventoryStorerBySearchCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.WorkingStatisCollection = accessor.GetWorkingStatisBySearchCondition(request.WorkingSearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
        /// 新增工时统计
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CreateWorkingStatis(WorkingSearchCondition searchCondition, out string msg)
        {
            msg = "";
            try
            {
                return new ReportManagementAccessor().CreateWorkingStatis(searchCondition);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }

        }

        /// <summary>
        /// 更新工时统计
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateWorkingStatis(WorkingSearchCondition searchCondition, out string msg)
        {
            msg = "";
            try
            {
                return new ReportManagementAccessor().UpdateWorkingStatis(searchCondition);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return false;
            }

        }

        /// <summary>
        /// 获取进程列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <param name="rowcounts"></param>
        /// <returns></returns>
        public IEnumerable<WMS_ProcessTracking> GetProcessTrackingList(ProcessTrackingSearchCondition request, out string msg, out int rowcounts)
        {
            msg = "";
            rowcounts = 0;
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                return accessor.GetProcessTrackingList(request, out rowcounts);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                rowcounts = 0;
                return null;
            }
        }
        /// <summary>
        /// 导出进程列表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <param name="rowcounts"></param>
        /// <returns></returns>
        public IEnumerable<WMS_ProcessTracking> ExportProcessTracking(ProcessTrackingSearchCondition request, out string msg)
        {
            msg = "";
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                return accessor.ExportProcessTracking(request);
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                return null;
            }
        }

        /// <summary>
        /// RSO质检报告查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetInspectionReportBySearchConditionResponse> GetInspectionReportBySearchCondition(GetInspectionReportBySearchConditionRequest request)
        {
            Response<GetInspectionReportBySearchConditionResponse> response = new Response<GetInspectionReportBySearchConditionResponse>() { Result = new GetInspectionReportBySearchConditionResponse() };
            if (request == null && request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetInspectionReportBySearchConditionRequest request");
                LogError(ex);
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Argument;
                return response;
            }
            try
            {
                int RowCount;
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.InspectionReportCollection = accessor.GetInspectionReportBySearchCondition(request.SearchCondition, request.PageIndex, request.PageSize, out RowCount);
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.IsSuccess = true;
            }
            catch(Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }

        /// <summary>
        /// RSO质检报告导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<GetInspectionReportBySearchConditionResponse> ExportInspectionReportBySearchCondition(GetInspectionReportBySearchConditionRequest request)
        {
            Response<GetInspectionReportBySearchConditionResponse> response = new Response<GetInspectionReportBySearchConditionResponse>() { Result = new GetInspectionReportBySearchConditionResponse() };
            if(request==null &&request.SearchCondition==null)
            {
                ArgumentNullException ex = new ArgumentNullException("ExportInspectionReportBySearchCondition request");
                LogError(ex);
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Argument;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.InspectionReportCollection = accessor.ExportInspectionReportBySearchCondition(request.SearchCondition);
                response.IsSuccess = true;
            }
            catch(Exception ex)
            {
                LogError(ex);
                response.Exception = ex;
                response.ErrorCode = ErrorCode.Technical;
                response.IsSuccess = false;
            }
            return response;
        }


        /// <summary>
        /// 查询出库耗材报表
        /// </summary>
        /// <param name="request">查询条件</param>
        /// <param name="proc">存储过程变成可配置wms_config</param>
        /// <returns></returns>
        public Response<GetReportTableBySearchConditionResponse> GetOrderConsumable(GetReportTableBySearchConditionRequest request, string Proc)
        {
            Response<GetReportTableBySearchConditionResponse> response = new Response<GetReportTableBySearchConditionResponse>() { Result = new GetReportTableBySearchConditionResponse() };

            if (request == null && request.TableSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReportTableBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                int RowCount;
                response.Result.ReprotTableInCollection = accessor.GetOrderConsumable(request.TableSearchCondition, request.PageIndex, request.PageSize, out RowCount, Proc);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
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
        /// 导出出库耗材报表
        /// </summary>
        /// <param name="request"></param>
        /// <param name="Proc">存储过程可配置</param>
        /// <returns></returns>
        public Response<GetReportTableBySearchConditionResponse> ExportGetOrderConsumable(GetReportTableBySearchConditionRequest request, string Proc)
        {
            Response<GetReportTableBySearchConditionResponse> response = new Response<GetReportTableBySearchConditionResponse>() { Result = new GetReportTableBySearchConditionResponse() };

            if (request == null && request.TableSearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetReportTableBySearchConditionRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            try
            {
                ReportManagementAccessor accessor = new ReportManagementAccessor();
                response.Result.ReprotTableInCollection = accessor.ExportGetOrderConsumable(request.TableSearchCondition, Proc);
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




    }
}
