using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class GetShipperAllInfoRequest
    {
        public long ShipperID { get; set; }

        public long? ProjectID { get; set; }

        public long? RelatedCustomerID { get; set; }
    }
}
