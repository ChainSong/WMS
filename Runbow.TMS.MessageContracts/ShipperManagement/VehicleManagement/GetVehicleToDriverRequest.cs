using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.ShipperManagement.DriverManagement;
using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;

namespace Runbow.TWS.MessageContracts.ShipperManagement.VehicleManagement
{
    public class GetVehicleToDriverRequest
    {
        public VehicleToDriver VehicleToDriver { get; set; }

        public CRMDriver Driver { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
