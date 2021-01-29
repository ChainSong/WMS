using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.InventoryApi;

namespace Runbow.TWS.MessageContracts.Inventory
{
   public class OSRJobResponses
    {
       public IEnumerable<OSRJobSelect> OSRJob { get; set; }
       public int RowCount { get; set; }
    }
}
