using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.Replenishment;

namespace Runbow.TWS.MessageContracts.WMS.Inventory
{
    public class GetReplenishmentByConditionResponse
    {
        public IEnumerable<Replenishment> ASNCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
