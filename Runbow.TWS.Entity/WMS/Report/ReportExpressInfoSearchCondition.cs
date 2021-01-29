using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.Report;

namespace Runbow.TWS.Entity
{
    public class ReportExpressInfoSearchCondition : ReportExpressInfo
    { 
        public DateTime? StartReportDate { get; set; }
        public DateTime? EndReportDate { get; set; }
        public string CustomerIDs { get; set; }
    }
}
