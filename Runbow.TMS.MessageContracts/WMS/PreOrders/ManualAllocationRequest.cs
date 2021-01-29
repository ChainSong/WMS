using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts.WMS.PreOrders
{
    public class ManualAllocationRequest
    {
        public IEnumerable<PreOrderDetail> PodRequest { get; set; }

        public long ID { get; set; }

        public string CustomerId { get; set; }

        public string Creator { get; set; }

        public string Criterion { get; set; }

        public string SqlProc { get; set; }
    }
}
