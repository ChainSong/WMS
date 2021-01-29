using System.Collections.Generic;

namespace Runbow.TWS.MessageContracts
{
    public class GetPodInfoRequest
    {
        public long PodID { get; set; }

        public IEnumerable<long> PodIDs { get; set; }
    }
}