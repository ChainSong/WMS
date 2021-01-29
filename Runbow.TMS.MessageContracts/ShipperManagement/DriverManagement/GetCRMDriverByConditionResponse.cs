using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.ShipperManagement.DriverManagement;

namespace Runbow.TWS.MessageContracts.ShipperManagement.DriverManagement
{
    public class GetCRMDriverByConditionResponse
    {
        public IEnumerable<CRMDriver> CRMDriverCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
