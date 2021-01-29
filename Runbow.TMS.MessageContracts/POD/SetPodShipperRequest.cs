using System.Collections.Generic;

namespace Runbow.TWS.MessageContracts
{
    public class SetPodShipperRequest
    {
        public IEnumerable<long> IDs { get; set; }

        public long ProjectID { get; set; }
    }
}