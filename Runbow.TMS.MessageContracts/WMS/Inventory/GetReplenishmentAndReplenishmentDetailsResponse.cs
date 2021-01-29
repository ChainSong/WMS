using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.Replenishment;

namespace Runbow.TWS.MessageContracts.WMS.Inventory
{
    public class GetReplenishmentAndReplenishmentDetailsResponse
    {
        public IEnumerable<Replenishment> ReplenishmentCollection { get; set; }

        public IEnumerable<ReplenishmentDetail> ReplenishmentDetailCollection { get; set; }
    }
}
