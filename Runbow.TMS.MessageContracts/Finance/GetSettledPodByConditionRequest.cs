using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class GetSettledPodByConditionRequest
    {
        public IEnumerable<string> CustomerOrderNumberCollection { get; set; }

        public int SettledType { get; set; }

        public long CustomerID { get; set; }

        public long? ShipperID { get; set; }
    }
}
