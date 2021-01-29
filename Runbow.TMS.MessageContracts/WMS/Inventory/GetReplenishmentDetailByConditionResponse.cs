using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.Replenishment;

namespace Runbow.TWS.MessageContracts.WMS.Inventory
{
    public class GetReplenishmentDetailByConditionResponse
    {
        public IEnumerable<ReplenishmentDetail> ReplenishmentDetailCollection { get; set; }
        public Replenishment Replenishment { get; set; }
        public IEnumerable<Replenishment> ReplenishmentCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
