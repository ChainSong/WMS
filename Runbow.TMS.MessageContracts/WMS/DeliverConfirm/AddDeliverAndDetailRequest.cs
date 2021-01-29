using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.DeliveryConfirm;

namespace Runbow.TWS.MessageContracts.WMS.DeliverConfirm
{
    public class AddDeliverAndDetailRequest
    {
        public IEnumerable<DeliverHeader> DeliverHeaderConnection { get; set; }
        public IEnumerable<DeliverDetail> DeliverDetailConnection { get; set; }
    }
}
