using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using Runbow.TWS.Dao;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.POD;
using Runbow.TWS.MessageContracts.POD.Hilti;
using System.Data;

namespace Runbow.TWS.Biz
{
    public class HiltiService: BaseService
    {
        public Response<IEnumerable<PodAll>> GetPodAndPodReplyDocumentByCondition(GetPodAndPodReplyDocumentByConditionRequest request)
        {   
            Response<IEnumerable<PodAll>> response = new Response<IEnumerable<PodAll>>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodAndPodReplyDocumentByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
                response.Result = accessor.GetPodAndPodReplyDocumentByCondition(request.CustomerOrderNumbers, request.ShipperID, request.StartActualArrivalDate, request.EndActualArrivalDate,request.MinPodState);
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



        public Response<GetPodTrackReportExportRequest> GetPodTrackReport(GetPodTrackReportExportRequest request)
        {
            Response<GetPodTrackReportExportRequest> response = new Response<GetPodTrackReportExportRequest>() { Result=new GetPodTrackReportExportRequest()};

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodTrackReportExport request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
            return response;
        }

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
                int Rowcount;
                double SumGrossWeight;
                double  SumNetWeight;
                response.Result.XLDTrackReport = accessor.GetPodTrackReport(request.SqlWhere, request.PageIndex, request.PageSize,request.ReportName, out Rowcount,out SumGrossWeight,out SumNetWeight);
                response.Result.PageIndex = request.PageIndex;
                response.Result.PageCount = Rowcount % request.PageSize == 0 ? Rowcount / request.PageSize : Rowcount / request.PageSize + 1;
                response.Result.SumPoll = Rowcount;
                response.Result.SumGrossWeight = SumGrossWeight;
                response.Result.SumNetWeight = SumNetWeight;
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



        public Response<DataTable> GetPodTrackReportExport(GetPodTrackReportExportRequest request)
        {
            Response<DataTable> response = new Response<DataTable>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetPodTrackReportExport request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
               
                response.Result = accessor.GetPodTrackReportExport(request.SqlWhere,request.ReportName);
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


        public Response<IEnumerable<ServicePeriod>> GetServicePeriod()
        {
            Response<IEnumerable<ServicePeriod>> response = new Response<IEnumerable<ServicePeriod>>();
            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
                response.Result = accessor.GetServicePeriod();
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

        public Response<ServicePeriod> GetServicePeriodByCondition(GetServicePeriodByConditionRequest request)
        {
            Response<ServicePeriod> response = new Response<ServicePeriod>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetServicePeriodByCondition request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
                response.Result = accessor.GetServicePeriodByCondition(request.ProjectID,request.CustomerID,request.StartCityID,request.EndCityID);
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





        public Response<UpdatePodInfoUpdateRequest> PodInfoUpdateByTable(UpdatePodInfoUpdateRequest request)
        {
            Response<UpdatePodInfoUpdateRequest> response = new Response<UpdatePodInfoUpdateRequest>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("PodInfoUpdateByTable request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
                response.Result = accessor.PodInfoUpdateByTable(request);
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

        public Response<UpdateOrderNoInfoUpdateRequest> OrderNoInfoUpdateByTable(UpdateOrderNoInfoUpdateRequest request)
        {


            Response<UpdateOrderNoInfoUpdateRequest> response = new Response<UpdateOrderNoInfoUpdateRequest>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("OrderNoInfoUpdateByTable request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
                response.Result = accessor.OrderNoInfoUpdateByTable(new UpdateOrderNoInfoUpdateRequest() { OrderNoinfo = request.OrderNoinfo });
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

        public Response<DataTable> GetPrintHandoverDetailedListDetail(PrintHandoverDetailedListDetailRequest request)
        {
            Response<DataTable> response = new Response<DataTable>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("OrderNoInfoUpdateByTable request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
                response.Result = accessor.GetPrintHandoverDetailedListDetail(request.SqlWhere);
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






        public Response<bool> PODColumnsInfoUpdateByTable(UpdatePodColumnInfoRequest request)
        {
            Response<bool> response = new Response<bool>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("OrderNoInfoUpdateByTable request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
                response.Result = accessor.PODColumnsInfoUpdateByTable(request.PodColumnInfo);
                if (response.Result)
                {
                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
                }
                
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




        public Response<AddOrUpdateServicePeriodRequest> AddOrUpdateServicePeriod(AddOrUpdateServicePeriodRequest request)
        {

            Response<AddOrUpdateServicePeriodRequest> response = new Response<AddOrUpdateServicePeriodRequest>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddOrUpdateServicePeriod request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
                response.Result = accessor.AddOrUpdateServicePeriod(new AddOrUpdateServicePeriodRequest() { EndCity=request.EndCity, EndCityID=request.EndCityID, Period=request.Period });
                
                response.IsSuccess = response.Result.ErrorValue.ObjectToBoolean();
               
                
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




        public Response<DataTable> CheckShipperDistributionDate(PrintHandoverDetailedListDetailRequest request)
        {
            Response<DataTable> response = new Response<DataTable>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("CheckShipperDistributionDate request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();

                response.Result = accessor.CheckShipperDistributionDate(request.ShipperName,request.BeginDateTime,request.EndDateTime);
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






        public Response<int> AddOrUpdateSellInfo(AddOrUpdateServicePeriodRequest request)
        {

            Response<int> response = new Response<int>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("AddOrUpdateSellInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
                response.Result = accessor.AddOrUpdateSellInfo(request.SellName,request.SellPhone);

                


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






        public Response<int> GetServicePeriodInfo(AddOrUpdateServicePeriodRequest request)
        {

            Response<int> response = new Response<int>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetServicePeriodInfo request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
                response.Result = accessor.GetServicePeriodInfo(request.EndCity);




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





        public Response<string> GetHiltiDriverPhone(AddOrUpdateServicePeriodRequest request)
        {

            Response<string> response = new Response<string>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("GetHiltiDriverPhone request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
                response.Result = accessor.GetHiltiDriverPhone(request.SellName);




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
        /// 把给定的datatable转换成数据库表对应的datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tableName"></param>
        /// <param name="colNames"></param>
        /// <param name="colTypes"></param>
        /// <returns></returns>
        private DataTable Convert2MyDt(DataTable dt,string tableName,string []colNames,Type []colTypes)
        {
            DataTable tableNew = new DataTable(tableName);
            int colLen = colNames.Length;
            DataColumn[] dcs = new DataColumn[colLen];
            for (int i = 0; i < colLen; i++)
            {
                DataColumn dc = new DataColumn(colNames[i]);
                dc.DataType = colTypes[i];
                tableNew.Columns.Add(dc);
            }
            tableNew.AcceptChanges();

            for (int j = 0; j < dt.Rows.Count; j++)
            {
                IList<object> valList = new List<object>();
                for (int k = 0; k < colLen; k++)
                {
                    object tempValue = dt.Rows[j][k];
                    valList.Add(tempValue);
                }
                bool flag =true;
                foreach (var item in valList)
                {
                    if (string.IsNullOrWhiteSpace(item.ToString()))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    tableNew.Rows.Add(valList.ToArray());
                }
            }
            return tableNew;
        }



        public Response<bool> CreditUpdateByTable(CreditUpdateRequest request)
        {
            Response<bool> response = new Response<bool>();

            if (request == null)
            {
                ArgumentNullException ex = new ArgumentNullException("CreditUpdateRequest request");
                LogError(ex);
                response.ErrorCode = ErrorCode.Argument;
                response.Exception = ex;
                return response;
            }
            request.OrderNoinfo = Convert2MyDt(request.OrderNoinfo, "CreditUpdate", new string[] { "CustomerCode", "CreditCode" }, new Type[] { typeof(string), typeof(string) });

            if (request.OrderNoinfo == null || request.OrderNoinfo.Rows == null || request.OrderNoinfo.Rows.Count < 1)
            {
                response.IsSuccess = false;
                response.ErrorCode = ErrorCode.DataEffective;
                response.SuccessMessage = "无效的表数据，请检查数据！";
                return response;
            }

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
                if (!accessor.DeleteAllPod())//清空HiltiCustomerCreditMapping表
                {
                    Exception ex = new Exception("清空HiltiCustomerCreditMapping表失败");
                    LogError(ex);
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
                    response.Exception = ex;
                    return response;
                }

                bool res = accessor.BulkCopy(request.OrderNoinfo);
                if (res)
                {
                    response.SuccessMessage = "上传更新成功!";
                    response.IsSuccess = true;  
                }
                
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

        public Response<bool> UpdatePOD(CreditUpdateRequest request)
        {
            Response<bool> response = new Response<bool>();

            try
            {
                HiltiAccessor accessor = new HiltiAccessor();
                if (!accessor.UpdateCode())//清空HiltiCustomerCreditMapping表
                {
                    Exception ex = new Exception("更新失败");
                    LogError(ex);
                    response.IsSuccess = false;
                    response.ErrorCode = ErrorCode.Technical;
                    response.Exception = ex;
                    return response;
                }

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
    }
}
