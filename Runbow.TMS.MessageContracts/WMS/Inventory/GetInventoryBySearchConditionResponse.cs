using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Inventory;

namespace Runbow.TWS.MessageContracts.WMS.Inventory
{
   public class GetInventoryBySearchConditionResponse
   {
       public IEnumerable<Inventorys> InventoryCollection { get; set; }
       public IEnumerable<Inventorys> InventoryCollection2 { get; set; }

       public IEnumerable<Receipt> ReceiptCollection { get; set; }

       public IEnumerable<OrderInfo> OrderCollection { get; set; }

       public IEnumerable<Adjustment> AdjustCollection { get; set; }

       public IEnumerable<AdjustmentDetail> AdjustDetailCollection { get; set; }

       public IEnumerable<DirectAddInventory> directAddInventory { get; set; }

       public IEnumerable<DirectAddInventory>  Total { get; set; }

       public IEnumerable<DirectAddInventory> daily { get; set; }

       public IEnumerable<DirectAddInventory> detail { get; set; }

       public IEnumerable<InventoryCompare> InventoryCompareCollection { get; set; }

       public int PageCount { get; set; }

       public int PageIndex { get; set; }
       public IEnumerable<InventorySnapshoot> InventorySnapCollection { get;set;}//库存快照
    }
}
