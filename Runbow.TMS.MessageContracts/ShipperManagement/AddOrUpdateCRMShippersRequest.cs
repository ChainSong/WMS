using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddOrUpdateCRMShippersRequest
    {
        public IEnumerable<CRMShipper> CRMShipperCollection { get; set; }
    }
}