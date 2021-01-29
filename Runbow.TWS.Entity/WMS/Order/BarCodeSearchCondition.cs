using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Order
{
    public class BarCodeSearchCondition:BarCodeInfo
    {
        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }
        public string CustomerIDs { get; set; }
    }
}
