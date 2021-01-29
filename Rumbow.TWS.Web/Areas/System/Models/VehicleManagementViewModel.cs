using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class VehicleManagementViewModel
    {
        public IEnumerable<Vehicle> Vehicles { get; set; }

        public long ShipperID { get; set; }

        public bool IsEdit { get; set; }
    }
}