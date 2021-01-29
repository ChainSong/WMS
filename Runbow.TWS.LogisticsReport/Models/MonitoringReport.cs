using Runbow.TWS.Entity.MonitoringReport;
using Runbow.TWS.MessageContracts.MonitoringReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.LogisticsReport.Models
{
    public class MonitoringReport
    {
        public MonitoringResponse MyProperty { get; set; }

        public tbl_wms_OrderHeader ElectricMeter { get; set; }
    }
}