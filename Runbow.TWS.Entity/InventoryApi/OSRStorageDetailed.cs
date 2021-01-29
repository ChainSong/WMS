using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class OSRStorageDetailed
    {
       [EntityPropertyExtension("TransOrderNO", "TransOrderNO")]
       public string TransOrderNO { get; set; }

       [EntityPropertyExtension("ReceiptKey", "ReceiptKey")]
       public string ReceiptKey { get; set; }

       [EntityPropertyExtension("LocationCategory", "LocationCategory")]
       public string LocationCategory { get; set; }

       [EntityPropertyExtension("SKU", "SKU")]
       public string SKU { get; set; }

        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }

        [EntityPropertyExtension("Qty", "Qty")]
        public int Qty { get; set; }

        [EntityPropertyExtension("SKUCategory05", "SKUCategory05")]
        public string SKUCategory05 { get; set; }

        [EntityPropertyExtension("CategoryMeaning", "CategoryMeaning")]
        public string CategoryMeaning { get; set; }

        [EntityPropertyExtension("SilHouetteMeaning", "SilHouetteMeaning")]
        public string SilHouetteMeaning { get; set; }

        //[EntityPropertyExtension("LongMaterial", "LongMaterial")]
        //public string LongMaterial { get; set; }
    }
}
