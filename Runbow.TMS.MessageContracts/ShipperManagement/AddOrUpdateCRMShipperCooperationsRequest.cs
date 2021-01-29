using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddOrUpdateCRMShipperCooperationsRequest
    {
        public IEnumerable<CRMShipperCooperation> CRMShipperCooperationCollection { get; set; }
    }
}
