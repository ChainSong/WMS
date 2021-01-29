using Runbow.TWS.Entity.ShipperManagement.VehicleManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.ShipperManagement.VehicleManagement
{
    public class GetCRMVehicleByConditionRequest
    {
        public CRMVehicleSearchCondition SearchCondition { get; set; }

        public CRMVehicle CreateFiles { get; set; }

        public CRMVehicle Vehicle { get; set; }

        public string ShipperID { get; set; }

        public string keyword { get; set; }

        public string vehicleNo { get; set; }

        public string StatUpLoadTime { get; set; }

        public string EndUpLoadTime { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
