using Runbow.TWS.Entity;
using Runbow.TWS.Entity.MonitoringReport;
using Runbow.TWS.MessageContracts.MonitoringReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;
namespace Runbow.TWS.Dao.MonitoringReport
{
    public class MonitoringReportAccessor : BaseAccessor
    {
        public MonitoringResponse GetMonitoringReport()
        {
            MonitoringResponse re = new MonitoringResponse();
            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@SqlWhere", DbType.String, "", ParameterDirection.Input)
   
            };
            DataSet ds = this.ExecuteDataSet("Proc_MonitoringReport", dbParams);
            re.OrderQuantity = ds.Tables[0].ConvertToEntityCollection<tbl_wms_OrderHeader>();
            //re.Efficiency = ds.Tables[1].ConvertToEntityCollection<tbl_wms_OrderHeader>();
            re.TimelyRate = ds.Tables[1].ConvertToEntityCollection<tbl_wms_OrderHeader>();
            re.WeeksOrders = ds.Tables[2].ConvertToEntityCollection<tbl_wms_OrderHeader>();
            re.CarbonEmissions = ds.Tables[3].ConvertToEntityCollection<tbl_wms_OrderHeader>();
            re.GetElectric = ds.Tables[4].ConvertToEntity<tbl_wms_OrderHeader>();
            return re;
        }
        public MonitoringResponse GetElectricMeter(string Date)
        {
            string Datetime = Convert.ToDateTime(Date).ToString("yyyy-MM-dd");
            MonitoringResponse re = new MonitoringResponse();
            DbParam[] dbParams = new DbParam[]
            {
                new DbParam("@SqlWhere", DbType.String, Datetime, ParameterDirection.Input)
            };
            DataTable dt = this.ExecuteDataTable("Proc_GetElectricMeter", dbParams);
            re.GetElectric = dt.ConvertToEntity<tbl_wms_OrderHeader>();
            return re;
        }

        public bool AddElectricMeter(string Date, string Office, string NFS, string Digital, string Inline)
        {
            MonitoringResponse re = new MonitoringResponse();
            DbParam[] dbParams = new DbParam[]
            {
               
                 new DbParam("@Date", DbType.String, Date, ParameterDirection.Input),
                 new DbParam("@Office", DbType.Decimal, Office, ParameterDirection.Input),
                 new DbParam("@NFS", DbType.Decimal, NFS, ParameterDirection.Input),
                 new DbParam("@Digital", DbType.Decimal, Digital, ParameterDirection.Input),
                 new DbParam("@Inline", DbType.Decimal, Inline, ParameterDirection.Input),
                
            };
            DataTable dt = this.ExecuteDataTable("Proc_AddElectricMeter", dbParams);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

    }
}
