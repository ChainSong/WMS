using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.System
{
    public class UserWarehouseRequest
    {
        public int Target { get; set; }

        public long CustomerOrShipperID { get; set; }

        public long SegmentID { get; set; }

        public long ProjectID { get; set; }

        public long RelatedCustomerID { get; set; }

    }
}
