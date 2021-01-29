using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class OSRThelibraryHeader
    {
       [EntityPropertyExtension("ID", "ID")]
       public int ID { get; set; }

       [EntityPropertyExtension("TransOrderNO", "TransOrderNO")]
       public string TransOrderNO { get; set; }

       [EntityPropertyExtension("OrderKey", "OrderKey")]
       public string OrderKey { get; set; }

       [EntityPropertyExtension("ShipToCompany", "ShipToCompany")]
       public string ShipToCompany { get; set; }

       [EntityPropertyExtension("ShipToKey", "ShipToKey")]
       public string ShipToKey { get; set; }

       [EntityPropertyExtension("DeliverDate", "DeliverDate")]
       public DateTime DeliverDate { get; set; }

       [EntityPropertyExtension("QtyTotal", "QtyTotal")]
       public int QtyTotal { get; set; }

       //ATA
       [EntityPropertyExtension("Date1", "Date1")]
       public DateTime? Date1 { get; set; }

       [EntityPropertyExtension("ETA", "ETA")]
       public DateTime? ETA { get; set; }

       [EntityPropertyExtension("PE", "PE")]
       public string PE { get; set; }

       [EntityPropertyExtension("Season", "Season")]
       public string Season { get; set; }
    }
}
