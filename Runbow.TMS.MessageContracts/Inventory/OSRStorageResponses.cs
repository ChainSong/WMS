using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.InventoryApi;

namespace Runbow.TWS.MessageContracts.Inventory
{
   public class OSRStorageResponses
    {
       public IEnumerable<OSRStorageHeader> Header { get; set; }
       public IEnumerable<OSRStorageDetailed> Detailed { get; set; }
       public IEnumerable<OSRStorageSelect> Export { get; set; }
       public int RowCount { get; set; }
    }
}
