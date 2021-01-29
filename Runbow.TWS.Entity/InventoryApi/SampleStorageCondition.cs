using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class SampleStorageCondition
    {
       public string SKU { get; set; }

       public string Category { get; set; }

       public string PE { get; set; }

       public DateTime? ReceiveBeginDate { get; set; }

       public DateTime? ReceiveEndDate { get; set; }

       public int PageIndex { get; set; }

       public int PageSize { get; set; }
    }
}
