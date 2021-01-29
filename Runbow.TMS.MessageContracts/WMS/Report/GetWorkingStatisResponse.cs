using Runbow.TWS.Entity.WMS.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Report
{
    public class GetWorkingStatisResponse
    {
        public IEnumerable<WMS_WorkingStatis> WorkingStatisCollection { get; set; }
        public IEnumerable<WMS_WorkingStatisDetail> WorkingStatisDetailCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
