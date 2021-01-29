using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Order
{
    public class OutboundInstructionsRequest
    {
        public string IDs { get; set; }
        public string UserName { get; set; }
        public int ReleatedType { get; set; }
        public string WorkStatio { get; set; }
    }
}
