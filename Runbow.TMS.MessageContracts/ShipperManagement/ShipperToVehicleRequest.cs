using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.ShipperManagement
{
    public class ShipperToVehicleRequest
    {
        public long SID { get; set; }

        public long VID { get; set; }

        public string CreateUser { get; set; }
    }
}
