using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WorkOrder
{
    [Serializable]
    public class WorkOrderSearchCondition:WorkOrder
    {
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
    }
}
