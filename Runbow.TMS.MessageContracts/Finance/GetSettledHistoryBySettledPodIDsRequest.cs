using System.Collections.Generic;

namespace Runbow.TWS.MessageContracts
{
    public class GetSettledHistoryBySettledPodIDsRequest
    {
        public IEnumerable<long> SettledPodIDs { get; set; }
    }
}