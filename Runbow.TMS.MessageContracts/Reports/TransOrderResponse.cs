using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.Reports;

namespace Runbow.TWS.MessageContracts.Reports
{
    public class TransOrderResponse
    {
        public IEnumerable<TransOrder> transOrder { get; set; }

        public int? PageCount { get; set; }

        public int? PageIndex { get; set; }
    }
}
