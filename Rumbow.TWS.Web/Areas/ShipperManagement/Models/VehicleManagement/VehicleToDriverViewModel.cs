using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity.ShipperManagement.DriverManagement;
using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;

namespace Runbow.TWS.Web.Areas.ShipperManagement.Models.VehicleManagement
{
    public class VehicleToDriverViewModel
    {
        public IEnumerable<VehicleToDriver> VehicleToDriver { get; set; }

        public IEnumerable<CRMVehicle> Vehicle { get; set; }

        public IEnumerable<CRMDriver> Driver { get; set; }



        public int PageIndex { get; set; }

        public int PageCount { get; set; }


        public long VID { get; set; }

        public long DID { get; set; }

        public string VehicleNo { get; set; }

        public string DriverName { get; set; }

        public string DriverPhone { get; set; }

        public string str1 { get; set; }

        public string str2 { get; set; }
    }
}