using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetShippersByConditionResponse
    {
        public IEnumerable<Shipper> Shippers { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}