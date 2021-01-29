using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetTransportationLinesByConditionResponse
    {
        public IEnumerable<TransportationLine> TransportationLines { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}