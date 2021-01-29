using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.POD.Distribution;

namespace Runbow.TWS.MessageContracts.POD.Distribution
{
    public class SettlePodsDistributionRequest
    {
        public IEnumerable<SettlePodDistribution> SettledPodsDistribution { get; set; }
    }
}
