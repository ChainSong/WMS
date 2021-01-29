using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class WinQueryPodResponses
    {
        public string Id { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
        public IEnumerable<WeiQueryPod> WeiQueryPods { get; set; }
    }
}
