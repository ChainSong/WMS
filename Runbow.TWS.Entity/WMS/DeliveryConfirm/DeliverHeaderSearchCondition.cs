using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.DeliveryConfirm
{
    public class DeliverHeaderSearchCondition : DeliverHeader
    {

        public DateTime? StartCreateTime { get; set; }
        public DateTime? EndCreateTime { get; set; }

        public DateTime? StartUpdateTime { get; set; }
        public DateTime? EndUpdateTime { get; set; }
        public string CustomerIDs;

    }
}
