using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddOrUpdateCRMShipperTerminalInfoRequest
    {
        public IEnumerable<CRMShipperTerminalInfo> CRMShipperTerminalInfoCollection { get; set; }
    }
}
