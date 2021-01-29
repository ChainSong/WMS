using Runbow.TWS.Entity.WMS.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Order
{
    public class OrderShipmentResponse
    {
        public IEnumerable<WMS_OrderShipment> shipments { get; set; }
        public IEnumerable<WMS_OrderShipmentAndDetail> shipmentAndDetails { get; set; }//这个里面会包含一个带DN单列表的list
        public IEnumerable<WMS_OrderShipmentDetail> shipmentDetails { get; set; }
    }
}
