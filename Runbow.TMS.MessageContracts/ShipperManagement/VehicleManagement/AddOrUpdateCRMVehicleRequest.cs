using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.ShipperManagement.VehicleManagement
{
    public class AddOrUpdateCRMVehicleRequest
    {
        public IEnumerable<CRMVehicle> CRMVehicleCollection { get; set; }
    }
}
