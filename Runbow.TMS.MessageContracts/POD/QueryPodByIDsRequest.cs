using System.Collections.Generic;

namespace Runbow.TWS.MessageContracts
{
    public class QueryPodByIDsRequest
    {
        public IEnumerable<long> PodIDs { get; set; }
    }
}