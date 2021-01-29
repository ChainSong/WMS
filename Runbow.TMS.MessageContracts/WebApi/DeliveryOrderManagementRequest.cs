using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.OnlineOrder;

namespace Runbow.TWS.MessageContracts.WebApi
{
    public class DeliveryOrderManagementResponse
    {
        public DeliveryOrder DeliveryOrderinfo { get; set; }

        public IEnumerable<DeliveryOrder> EnumerableDeliveryOrder { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }
    }
}
