using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Report
{
    public class WorkingSearchCondition : WMS_WorkingStatis
    {
        public string CustomerIDs { get; set; }
        public DateTime? BeginCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
    }
}
