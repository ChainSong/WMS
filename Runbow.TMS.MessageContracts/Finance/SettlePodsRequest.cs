using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class SettlePodsRequest
    {
        public IEnumerable<SettledPod> SettledPods { get; set; }

        public int SettledType { get; set; }
    }
}
