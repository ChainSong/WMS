using Runbow.TWS.Entity.MonitoringReport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.MonitoringReport
{
    public class MonitoringResponse
    {
        public IEnumerable<tbl_wms_OrderHeader> OrderQuantity { get; set; }
        public IEnumerable<tbl_wms_OrderHeader> Efficiency { get; set; }
        public IEnumerable<tbl_wms_OrderHeader> TimelyRate { get; set; }
        public IEnumerable<tbl_wms_OrderHeader> WeeksOrders { get; set; }
        public IEnumerable<tbl_wms_OrderHeader> CarbonEmissions { get; set; } 
        public tbl_wms_OrderHeader GetElectric { get; set; }
    }
}
