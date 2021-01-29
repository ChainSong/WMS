using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.ShipperManagement;
using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;

namespace Runbow.TWS.MessageContracts.ShipperManagement
{
    public class GetShipperToVehicleRequest
    {
        public ShipperToVehicle ShipperToVehicle { get; set; }

        public CRMVehicle Vehicle { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
