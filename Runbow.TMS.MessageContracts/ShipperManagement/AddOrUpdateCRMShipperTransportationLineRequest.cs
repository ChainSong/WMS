using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddOrUpdateCRMShipperTransportationLineRequest
    {
        public IEnumerable<CRMShipperTransportationLine> CRMShipperTransportationLineCollection { get; set; }
    }
}
