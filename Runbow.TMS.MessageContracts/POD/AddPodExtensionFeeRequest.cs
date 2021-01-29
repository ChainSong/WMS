using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddPodExtensionFeeRequest
    {
        public IEnumerable<long> PodIDs { get; set; }

        public IEnumerable<Pod> PodCollection { get; set; }

        public IEnumerable<SettledPod> SettledPodCOllection { get; set; }

        public int Type { get; set; }
    }
}
