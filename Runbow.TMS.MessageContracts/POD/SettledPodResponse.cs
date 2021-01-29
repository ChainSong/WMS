using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class SettledPodResponse
    {
        public IEnumerable<GroupedPods> GroupedPods { get; set; }

        public IEnumerable<long> PodIDs { get; set; }

        public int SettledType { get; set; }
    }
}
