using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Report
{
    public class ReportInventorySummarySearchCondition : ReportInventorySummary
    {

        public string CustomerIDs { get; set; }
    
        public int? IsLocationBy { get; set; }//是否根据库位分组汇总数据
    }
}
