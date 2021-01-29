using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.ShipperManagement.DriverManagement;

namespace Runbow.TWS.MessageContracts.ShipperManagement.DriverManagement
{
    public class AddOrUpdateCRMDriverRequest
    {
        public IEnumerable<CRMDriver> CRMDriverCollection { get; set; }
    }
}
