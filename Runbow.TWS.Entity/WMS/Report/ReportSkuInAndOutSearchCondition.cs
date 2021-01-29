using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class ReportSkuInAndOutSearchCondition:ReportSkuInAndOut
    {
        public DateTime? BeginCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
    }
}
