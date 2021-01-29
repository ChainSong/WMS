using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class OSRStorageSelect
    {
       [EntityPropertyExtension("TransOrderNO", "TransOrderNO")]
       public string TransOrderNO { get; set; }

       [EntityPropertyExtension("ReceiptKey", "ReceiptKey")]
       public string ReceiptKey { get; set; }

       [EntityPropertyExtension("ETA", "ETA")]
       public string ETA { get; set; }

       [EntityPropertyExtension("ActualReceivingTime", "ActualReceivingTime")]
       public string ActualReceivingTime { get; set; }

       [EntityPropertyExtension("PutawayTime", "PutawayTime")]
       public string PutawayTime { get; set; }

       [EntityPropertyExtension("ProcessingTime", "ProcessingTime")]
       public int ProcessingTime { get; set; }

       [EntityPropertyExtension("DeliveryStore", "DeliveryStore")]
       public string DeliveryStore { get; set; }

       [EntityPropertyExtension("STATUS", "STATUS")]
       public string STATUS { get; set; }

       [EntityPropertyExtension("PE", "PE")]
       public string PE { get; set; }

       [EntityPropertyExtension("SKU", "SKU")]
       public string SKU { get; set; }

       [EntityPropertyExtension("UPC", "UPC")]
       public string UPC { get; set; }

       [EntityPropertyExtension("Size", "Size")]
       public string Size { get; set; }

       [EntityPropertyExtension("Qty", "Qty")]
       public string Qty { get; set; }

       [EntityPropertyExtension("Category", "Category")]
       public string Category { get; set; }

       [EntityPropertyExtension("SilHouette", "SilHouette")]
       public string SilHouette { get; set; }

    

    }
}
