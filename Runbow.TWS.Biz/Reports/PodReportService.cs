using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.MobileWeb;
using Runbow.TWS.Entity.Reports;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.Reports;

namespace Runbow.TWS.Biz
{
    public class PodReportService : BaseService
    {
        public Response<QueryPodAndInvoiceAndReceiveOrPayOrdersResponse> QueryPodAndInvoiceAndReceiveOrPayOrders(QueryPodAndInvoiceAndReceiveOrPayOrdersRequest request)
        {
            Response<QueryPodAndInvoiceAndReceiveOrPayOrdersResponse> response = new Response<QueryPodAndInvoiceAndReceiveOrPayOrdersResponse>() { Result = new QueryPodAndInvoiceAndReceiveOrPayOrdersResponse() };
            if (request == null || request.SearchCondition == null)
            {
                ArgumentNullException ex = new ArgumentNullException("QueryPodAndInvoiceAndReceiveOrPayOrders request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodReportAccessor accessor = new PodReportAccessor();
                int rowCount;
                response.Result.PodInvoiceReceiveOrPayOrders = new PodInvoiceReceiveOrPayOrders();
                response.Result.PodInvoiceReceiveOrPayOrders.PodCollection = new PodAccessor().QueryPod(request.SearchCondition, request.ProjectID, request.PageIndex, request.PageSize, out rowCount).ToList();

                if (response.Result.PodInvoiceReceiveOrPayOrders.PodCollection != null && response.Result.PodInvoiceReceiveOrPayOrders.PodCollection.Any())
                {
                    response.Result.PodInvoiceReceiveOrPayOrders.SettledPodCollection = new SettledAccessor().GetSettledPodByPodIDsWithNoType(response.Result.PodInvoiceReceiveOrPayOrders.PodCollection.Select(p => p.ID));
                }

                if (response.Result.PodInvoiceReceiveOrPayOrders.SettledPodCollection != null && response.Result.PodInvoiceReceiveOrPayOrders.SettledPodCollection.Any())
                {
                    response.Result.PodInvoiceReceiveOrPayOrders.InvoiceCollection = new InvoiceAccessor().GetInvoicesByIDs(response.Result.PodInvoiceReceiveOrPayOrders.SettledPodCollection.Where(p=>p.InvoiceID.HasValue).Select(p=>p.InvoiceID.Value));
                }

                if (response.Result.PodInvoiceReceiveOrPayOrders.InvoiceCollection != null && response.Result.PodInvoiceReceiveOrPayOrders.InvoiceCollection.Any())
                {
                    response.Result.PodInvoiceReceiveOrPayOrders.ReceiveOrPayOrderCollection = new ReceiveOrPayOrdersAccessor().GetReceiveOrPayOrdersByInvoiceIDs(response.Result.PodInvoiceReceiveOrPayOrders.InvoiceCollection.Select(i => i.ID));
                }

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

        public Response<IEnumerable<CustomerPodMonthlyAndDayCount>> QueryCustomerMonthlyAndDailyPodCount(QueryCustomerMonthlyAndDailyPodCountRequest request)
        {
            Response<IEnumerable<CustomerPodMonthlyAndDayCount>> response = new Response<IEnumerable<CustomerPodMonthlyAndDayCount>>();
            if (request == null || string.IsNullOrEmpty(request.Year) || request.CustomerID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("QueryCustomerMonthlyAndDailyPodCount request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodReportAccessor accessor = new PodReportAccessor();
                DateTime startTime = DateTime.Parse(request.Year + "-01-01 00:00:00");
                DateTime endTime = DateTime.Parse(request.Year + "-12-31 23:59:59");
                response.Result = accessor.QueryCustomerMonthlyAndDailyPodCount(startTime, endTime, request.CustomerID);
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

        public Response<IEnumerable<CustomerPodYearMonthCount>> QueryCustomerPodYearMonthCountByTimeRange(QueryCustomerPodYearMonthCountByTimeRangeRequest request)
        {
            Response<IEnumerable<CustomerPodYearMonthCount>> response = new Response<IEnumerable<CustomerPodYearMonthCount>>();
            if (request == null || string.IsNullOrEmpty(request.Year) || request.ShipperID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("QueryCustomerPodYearMonthCountByTimeRange request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodReportAccessor accessor = new PodReportAccessor();
                DateTime startTime = DateTime.Parse(request.Year + "-01-01 00:00:00");
                DateTime endTime = DateTime.Parse((request.Year.ObjectToInt32() + 1).ToString() + "-01-01 00:00:00");

                response.Result = accessor.QueryCustomerPodYearMonthCountByTimeRange(startTime, endTime, request.ShipperID);
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
        public Response<TransOrderResponse> QueryTransOrderRange(TransOrderRequest request)
        {
            Response<TransOrderResponse> response = new Response<TransOrderResponse>() { Result = new TransOrderResponse() };
            
            try
            {
                PodReportAccessor accessor = new PodReportAccessor();
                int RowCount;
                response.Result.transOrder = accessor.QueryTransOrderRange(request);
                //response.Result.PageIndex = 1;
                //response.Result.transOrder = accessor.QueryTransOrderRange(request, out RowCount);
                //response.Result.PageIndex
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

        public Response<BTBMoneySummaryReportResponse> BTBMoneySummaryReport()
        {
            Response<BTBMoneySummaryReportResponse> response = new Response<BTBMoneySummaryReportResponse>() { Result = new BTBMoneySummaryReportResponse() };
            try
            {
                PodReportAccessor accessor = new PodReportAccessor();
                int RowCount;
                response.Result.Response = accessor.BTBMoneySummaryReport();
                //response.Result.PageIndex = 1;
                //response.Result.transOrder = accessor.QueryTransOrderRange(request, out RowCount);
                //response.Result.PageIndex
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


        public Response<TransOrderResponse> QueryTransOrderRanges(TransOrderRequest request)
        {
            Response<TransOrderResponse> response = new Response<TransOrderResponse>() { Result = new TransOrderResponse() };

            try
            {
                PodReportAccessor accessor = new PodReportAccessor();
                int RowCount;
                response.Result.transOrder = accessor.QueryTransOrderRanges(request,out RowCount);
                response.Result.PageCount = RowCount % request.PageSize == 0 ? RowCount / request.PageSize : RowCount / request.PageSize + 1;
                response.Result.PageIndex = request.PageIndex;
                response.IsSuccess = true;
                //response.Result.PageIndex = 1;
                //response.Result.transOrder = accessor.QueryTransOrderRange(request, out RowCount);
                //response.Result.PageIndex
                //response.IsSuccess = true;
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

        public Response<IEnumerable<PodRegionCount>> QueryCustomerPodCountByRegionAndTime(QueryCustomerPodCountByRegionAndTimeRequest request)
        {
            Response<IEnumerable<PodRegionCount>> response = new Response<IEnumerable<PodRegionCount>>();
            if (request == null || string.IsNullOrEmpty(request.Year))
            {
                ArgumentNullException ex = new ArgumentNullException("QueryCustomerPodCountByRegionAndTime request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodReportAccessor accessor = new PodReportAccessor();
                DateTime startTime;
                DateTime endTime;
                if (request.Month == 0)
                {
                    startTime = DateTime.Parse(request.Year + "-01-01 00:00:00");
                    endTime = DateTime.Parse((request.Year.ObjectToInt32() + 1).ToString() + "-01-01 00:00:00");
                }
                else
                {
                    string startMonth = string.Empty;
                    string endMonth = string.Empty;
                    string startYear = string.Empty;
                    string endYear = string.Empty;
                    if(request.Month < 10)
                    {
                        startYear = request.Year;
                        startMonth = "0" + request.Month.ToString();
                        endYear = request.Year;
                        if(request.Month == 9)
                        {
                            endMonth = (request.Month + 1).ToString();
                        }
                        else
                        {
                            endMonth = "0" + (request.Month + 1).ToString();
                        }
                    }
                    else
                    {
                        startYear = request.Year;
                        if(request.Month == 12)
                        {
                            startMonth = request.Month.ToString();
                            endMonth = "01";
                            endYear = (request.Year.ObjectToInt32() + 1).ToString();
                        }
                        else
                        {
                            startMonth = request.Month.ToString();
                            endMonth = (request.Month + 1).ToString();
                            endYear = request.Year;
                        }
                    }

                    startTime = DateTime.Parse(startYear + "-" + startMonth + "-01 00:00:00");
                    endTime = DateTime.Parse(endYear + "-" + endMonth + "-01 00:00:00"); 
                }
                response.Result = accessor.QueryCustomerPodCountByRegionAndTime(startTime, endTime, request.CustomerID);
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

        public Response<IEnumerable<CustomerPodYearCount>> QueryCustomerYearPodCount(QueryCustomerYearPodCountRequest request)
        {
            Response<IEnumerable<CustomerPodYearCount>> response = new Response<IEnumerable<CustomerPodYearCount>>();
            if (request == null || string.IsNullOrEmpty(request.Year))
            {
                ArgumentNullException ex = new ArgumentNullException("QueryCustomerYearPodCount request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodReportAccessor accessor = new PodReportAccessor();
                DateTime startTime = DateTime.Parse(request.Year + "-01-01 00:00:00");
                DateTime endTime = DateTime.Parse(request.Year + "-12-31 23:59:59");
                response.Result = accessor.QueryCustomerYearPodCount(startTime, endTime);
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

        public Response<IEnumerable<IncomAndExpenses>> QueryIncomAndExpenses(QueryIncomAndExpensesRequest request)
        {
            Response<IEnumerable<IncomAndExpenses>> response = new Response<IEnumerable<IncomAndExpenses>>();
            if (request == null || string.IsNullOrEmpty(request.Year) || request.CustomerID == 0)
            {
                ArgumentNullException ex = new ArgumentNullException("QueryIncomAndExpenses request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodReportAccessor accessor = new PodReportAccessor();
                DateTime startTime = DateTime.Parse(request.Year + "-01-01 00:00:00");
                DateTime endTime = DateTime.Parse((request.Year.ObjectToInt32() + 1) + "-01-01 00:00:00");
                response.Result = accessor.QueryIncomAndExpenses(startTime, endTime, request.CustomerID);
                if (response.Result != null)
                {
                    response.Result.Each((i, k) =>
                    {
                        k.CustomerID = request.CustomerID;
                        k.Year = request.Year;
                    });
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

        public Response<IEnumerable<ShipperCost>> QueryShipperCost(QueryShipperCostRequest request)
        {
            Response<IEnumerable<ShipperCost>> response = new Response<IEnumerable<ShipperCost>>();
            if (request == null || string.IsNullOrEmpty(request.Year) || request.ShipperID== 0)
            {
                ArgumentNullException ex = new ArgumentNullException("QueryShipperCost request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                PodReportAccessor accessor = new PodReportAccessor();
                DateTime startTime = DateTime.Parse(request.Year + "-01-01 00:00:00");
                DateTime endTime = DateTime.Parse((request.Year.ObjectToInt32() + 1) + "-01-01 00:00:00");
                response.Result = accessor.QueryShipperCost(startTime, endTime, request.ShipperID);
                if (response.Result != null)
                {
                    response.Result.Each((i, k) =>
                    {
                        k.Year = request.Year;
                    });
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

        public Response<ShowCaseHotMapResponse> ShowCaseHotMap(ShowCaseHotMapRequest Request )
        {
            Response<ShowCaseHotMapResponse> response = new Response<ShowCaseHotMapResponse>() { Result = new ShowCaseHotMapResponse() };
            try
            {
                PodReportAccessor accessor = new PodReportAccessor();
                response.Result.showCaseHotMap = accessor.ShowCaseHotMap(Request);
          
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
