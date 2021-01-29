using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class WinQueryPodRequest
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public WeiQueryPod WeiQueryPods { get; set; }
    }
}
