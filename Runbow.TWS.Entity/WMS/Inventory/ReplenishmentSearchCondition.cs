using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Inventory
{
    [Serializable]
    public class ReplenishmentSearchCondition : Runbow.TWS.Entity.WMS.Replenishment.Replenishment
    {
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
        public string Warehouse { get; set; }
        public string CustomerIDs { get; set; }
    }
}
