using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class GetSettledPodByPodIDsRequest
    {
        public IEnumerable<long> PodIDs { get; set; }

        public int SettledType { get; set; }
    }
}
