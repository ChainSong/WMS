using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.ShipperManagement;
using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;

namespace Runbow.TWS.Web.Areas.ShipperManagement.Models
{
    public class ShipperToVehicleViewModel
    {
        public ShipperToVehicle ShipperToVehicle { get; set; }

        //public IEnumerable<ShipperToVehicle> ShipperToVehicleCollection { get; set; }

        public IEnumerable<CRMShipper> Shipper { get; set; }

        public IEnumerable<CRMVehicle> Vehicle { get; set; }

        public CRMVehicle CRMVehicle { get; set; }

        public string ids { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        //0 add 1 view
        public int ViewType { get; set; }


        public long SID { get; set; }

        public long VID { get; set; }


        public string ShipperName { get; set; }

        public string VehicleNo { get; set; }

        public string Remark { get; set; }

        public string str1 { get; set; }

        public string str2 { get; set; }
    }
}