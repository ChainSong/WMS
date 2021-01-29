using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class BatchUpdateSettledPodAmtRequest
    {
        public IEnumerable<SettledPod> SettledPods { get; set; }

        public int SettleType { get; set; }

        public string Updator { get; set; }
    }
}
