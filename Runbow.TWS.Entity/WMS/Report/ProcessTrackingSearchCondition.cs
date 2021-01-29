using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Report
{
    public class ProcessTrackingSearchCondition : WMS_ProcessTracking
    {        
        public string StartCreateTime { get; set; }
        public string EndCreateTime { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

    }
}
