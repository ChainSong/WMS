using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class OSRStockSelect
    {

       [EntityPropertyExtension("PE", "PE")]
       public string PE { get; set; }

        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        [EntityPropertyExtension("Size", "Size")]
        public string Size { get; set; }

        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }

        [EntityPropertyExtension("Qty", "Qty")]
        public int Qty { get; set; }

        [EntityPropertyExtension("QtyAllocated", "QtyAllocated")]
        public int QtyAllocated { get; set; }

        [EntityPropertyExtension("QtyPicked", "QtyPicked")]
        public int QtyPicked { get; set; }

        [EntityPropertyExtension("QtyAvailable", "QtyAvailable")]
        public int QtyAvailable { get; set; }

        [EntityPropertyExtension("Category", "Category")]
        public string Category { get; set; }

        [EntityPropertyExtension("SilHouette", "SilHouette")]
        public string SilHouette { get; set; }

        [EntityPropertyExtension("PutawayTime", "PutawayTime")]
        public string PutawayTime { get; set; }

        [EntityPropertyExtension("Season", "Season")]
        public string Season { get; set; }

        [EntityPropertyExtension("InventoryAge", "InventoryAge")]
        public string InventoryAge { get; set; }

        


       
    }
}
