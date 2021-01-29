using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.InventoryApi;

namespace Runbow.TWS.MessageContracts.Inventory
{
   public class OSRThelibraryResponses
    {
       public IEnumerable<OSRThelibraryHeader> Header { get; set; }
       public IEnumerable<OSRThelibraryDetailed> Detailed { get; set; }
       public IEnumerable<OSRThelibrarySelect> Export { get; set; }
       public int RowCount { get; set; }
    }
}
