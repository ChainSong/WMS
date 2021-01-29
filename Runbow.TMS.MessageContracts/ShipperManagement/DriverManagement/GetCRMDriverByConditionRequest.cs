using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.ShipperManagement.DriverManagement;

namespace Runbow.TWS.MessageContracts.ShipperManagement.DriverManagement
{
    public class GetCRMDriverByConditionRequest
    {
        public CRMDriverSearchCondition SearchCondition { get; set; }

        public CRMDriver AddDriver { get; set; }

        public CRMDriver Driver { get; set; }

        public string driverName { get; set; }

        public string driverphone { get; set; }

        public string keyword { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
