using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.SettlementManagement;

namespace Runbow.TWS.MessageContracts
{
    public class GetSettlementByConditionResponse
    {
        public Settlement Settlement { get; set; }
        public IEnumerable<Settlement> SettlementCollection { get; set; }
        public IEnumerable<SettlementDetail> SettlementDetailCollection { get; set; }
        public IEnumerable<WMS_HiltibjSettled> HilSettlementCollection { get; set; }
        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public string Message { get; set; }

    }
}
