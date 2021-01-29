using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class SetPodShipperManuallyRequest : SetPodShipperRequest
    {
        public long ShipperID { get; set; }

        public string ShipperName { get; set; }
    }
}
