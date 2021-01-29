using System.Collections.Generic;

namespace Runbow.TWS.MessageContracts
{
    public class GetPodForecastInfoCollectionRequest
    {
        public IEnumerable<long> PodIDs { get; set; }

        public long CustomerID { get; set; }

        public long ProjectID { get; set; }
    }
}