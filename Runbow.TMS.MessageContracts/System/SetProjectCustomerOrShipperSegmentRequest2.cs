using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.System
{
    public class SetProjectCustomerOrShipperSegmentRequest2
    {
        public int Target { get; set; }

        public long CustomerOrShipperID { get; set; }

        public string SegmentID { get; set; }

        public long ProjectID { get; set; }

        public string RelatedCustomerID { get; set; }
    }
}
