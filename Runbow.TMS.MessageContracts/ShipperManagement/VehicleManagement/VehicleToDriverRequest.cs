using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.ShipperManagement.VehicleManagement
{
    public class VehicleToDriverRequest
    {
        public long VID { get; set; }

        public long DID { get; set; }

        public string CreateUser { get; set; }
    }
}
