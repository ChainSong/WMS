using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class ManualSettledPodRequest
    {
        public IEnumerable<long> PodIDs { get; set; }

        public long ShipperID { get; set; }

        public string ShipperName { get; set; }

        public IEnumerable<SettledPod> SettledPodCOllection { get; set; }

    }
}
