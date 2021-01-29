using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.ShipperManagement.VehicleManagement
{
    public class GetCRMVehicleByConditionResponse
    {
        public IEnumerable<CRMVehicle> CRMVehicleCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
