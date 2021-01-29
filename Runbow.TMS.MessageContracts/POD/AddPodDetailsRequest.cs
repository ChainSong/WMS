using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddPodDetailsRequest
    {
        public IEnumerable<PodDetail> PodDetails { get; set; }

        public long CustomerID { get; set; }
    }
}