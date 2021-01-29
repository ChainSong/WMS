using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Inventory
{
    public class AddInventroyRequest
    {
        public IEnumerable<Inventorys> inventorys { get; set; }
    }
}
