using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class GetPodAndPodReplyDocumentByConditionRequest
    {
        public IEnumerable<string> CustomerOrderNumbers { get; set; }

        public long? ShipperID { get; set; }

        public DateTime? StartActualArrivalDate { get; set; }

        public DateTime? EndActualArrivalDate { get; set; }

        public int MinPodState { get; set; }
    }
}
