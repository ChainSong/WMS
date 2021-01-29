using Runbow.TWS.Entity.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Order
{
    public class OutboundInstructionsResponse
    {
        public IEnumerable<InstructionInfo> instructionInfo { get; set; }
    }
}
