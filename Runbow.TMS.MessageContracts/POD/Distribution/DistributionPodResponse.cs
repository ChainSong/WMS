using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class DistributionPodResponse
    {
        public IEnumerable<PodToDistribution> PodCollections { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}