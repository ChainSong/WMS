using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Order
{
    
    public class WMS_OrderShipmentAndDetail:WMS_OrderShipment
    {      

        public IEnumerable<WMS_OrderShipmentDetail> OrderList { get; set; }
    }
}
