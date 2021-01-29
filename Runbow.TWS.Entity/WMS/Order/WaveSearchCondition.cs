using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Order
{
    public class WaveSearchCondition:WMS_Wave
    {
        public string OrderNumber { get; set; }
        public string ExternOrderNumber { get; set; }
        public string PreOrderNumber { get; set; }
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
        public DateTime? StartCompleteTime { get; set; }
        public DateTime? EndCompleteTime { get; set; }
        public string CustomerIDs { get; set; }
    }
}
