using Runbow.TWS.Entity.WMS.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Report
{
    public class GetWorkingStatisRequest
    {
        public WorkingSearchCondition WorkingSearchCondition { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
