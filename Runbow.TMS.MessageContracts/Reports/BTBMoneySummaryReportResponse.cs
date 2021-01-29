using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.MobileWeb;

namespace Runbow.TWS.MessageContracts.Reports
{
    public class BTBMoneySummaryReportResponse
    {
        public IEnumerable<BTBMoneySummaryReport> Response { get; set; }
    }
}
