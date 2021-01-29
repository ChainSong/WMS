using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.InventoryApi;

namespace Runbow.TWS.MessageContracts.Inventory
{
   public class OSRStockResponses
    {
       public IEnumerable<OSRStockSelect> OSRsk { get; set; }
       public int RowCount { get; set; }
       public int Total { get; set; }
    }
}
