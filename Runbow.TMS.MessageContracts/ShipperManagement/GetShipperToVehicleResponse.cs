using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.ShipperManagement;
using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;

namespace Runbow.TWS.MessageContracts.ShipperManagement
{
    public class GetShipperToVehicleResponse
    {
        public IEnumerable<ShipperToVehicle> ShipperToVehicleCollection { get; set; }

        public IEnumerable<CRMVehicle> Vehicle { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
