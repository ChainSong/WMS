using Runbow.TWS.Dao.MonitoringReport;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.MonitoringReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Biz.MonitoringReport
{
    public class MonitoringReportService : BaseService
    {
        public Response<MonitoringResponse> GetMonitoringReport()
        {
            Response<MonitoringResponse> response = new Response<MonitoringResponse>() { Result = new MonitoringResponse() };
            try
            {
                response.Result = new MonitoringReportAccessor().GetMonitoringReport();//request.ShipperId, 
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                //LogError(ex);
                response.IsSuccess = false;
                //response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }
        public Response<MonitoringResponse> GetElectricMeter(string Date)
        {
            Response<MonitoringResponse> response = new Response<MonitoringResponse>() { Result = new MonitoringResponse() };
            try
            {
                response.Result = new MonitoringReportAccessor().GetElectricMeter(Date);//request.ShipperId, 
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                //LogError(ex);
                response.IsSuccess = false;
                //response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }
        public Response<MonitoringResponse> AddElectricMeter(string date, string Office, string NFS, string Digital, string Inline)
        {
            Response<MonitoringResponse> response = new Response<MonitoringResponse>() { Result = new MonitoringResponse() };
            try
            {
                response.IsSuccess = new MonitoringReportAccessor().AddElectricMeter(date, Office, NFS, Digital, Inline);//request.ShipperId, 
                
            }
            catch (Exception ex)
            {
                //LogError(ex);
                response.IsSuccess = false;
                //response.ErrorCode = ErrorCode.Technical;
            }
            return response;
        }
    }
}
