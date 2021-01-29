using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Order
{
    public class InventoryOfOutboundRequest
    {
        public long? CustomerId { get; set; }

        public string Area { get; set; }

        public string Location { get; set; }

        public string SKU { get; set; }

        public string UPC { get; set; }

        public string GoodsType { get; set; }

        public string CustomerIds { get; set; }

        public string Warehouse { get; set; }

        public string Ids { get; set; }
        
    }
}
