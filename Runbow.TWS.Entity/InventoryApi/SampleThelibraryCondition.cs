using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class SampleThelibraryCondition
    {
       public string Category { get; set; }
       public string SKU { get; set; }
       public string PE { get; set; }
       public string Requester { get; set; }
       public DateTime? OrderBeginTime { get; set; }
       public DateTime? OrderEndTime { get; set; }
       public DateTime? DeliverBeginTime { get; set; }
       public DateTime? DeliverEndTime { get; set; }
       public string Type { get; set; }
       public int PageIndex { get; set; }
       public int PageSize { get; set; }
    }
}
