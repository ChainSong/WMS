﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;

namespace Runbow.TWS.MessageContracts.ShipperManagement.VehicleManagement
{
    public class VehicleMappingDriverRequest
    {
        public IEnumerable<CRMDriverName> driver { get; set; }

        public string VehicleNo { get; set; }

        public string UserName { get; set; }

        public string DriverName { get; set; }

        public string DriverPhone { get; set; }

       
    }
    
}
