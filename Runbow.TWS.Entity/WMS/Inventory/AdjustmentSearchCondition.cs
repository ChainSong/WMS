using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Inventory
{
    [Serializable]
    public class AdjustmentSearchCondition : Adjustment
    {
        public DateTime? StartAdjustmentDate  { get; set; }
        public DateTime? EndAdjustmentDate { get; set; }
        public string CustomerIDs { get; set; }
    }
}
