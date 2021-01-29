using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class SampleThelibrarySelect
    {
        [EntityPropertyExtension("OrderDate", "OrderDate")]
        public string OrderDate { get; set; }

        [EntityPropertyExtension("ShippedDate", "ShippedDate")]
        public string ShippedDate { get; set; }

        [EntityPropertyExtension("Requester", "Requester")]
        public string Requester { get; set; }

        [EntityPropertyExtension("OrderName", "OrderName")]
        public string OrderName { get; set; }

        [EntityPropertyExtension("CartonNo", "CartonNo")]
        public string CartonNo { get; set; }

        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        [EntityPropertyExtension("Size", "Size")]
        public string Size { get; set; }

        [EntityPropertyExtension("Gender", "Gender")]
        public string Gender { get; set; }

        [EntityPropertyExtension("Category", "Category")]
        public string Category { get; set; }

        [EntityPropertyExtension("SKUDesc", "SKUDesc")]
        public string SKUDesc { get; set; }

        [EntityPropertyExtension("SilhouetteDesc", "SilhouetteDesc")]
        public string SilhouetteDesc { get; set; }

        [EntityPropertyExtension("PE", "PE")]
        public string PE { get; set; }

        [EntityPropertyExtension("Season", "Season")]
        public string Season { get; set; }

        [EntityPropertyExtension("QTY", "QTY")]
        public int QTY { get; set; }

        [EntityPropertyExtension("FactoryCode", "FactoryCode")]
        public string FactoryCode { get; set; }

        [EntityPropertyExtension("Remarks", "Remarks")]
        public string Remarks { get; set; }

        [EntityPropertyExtension("FOB", "FOB")]
        public int FOB { get; set; }

        [EntityPropertyExtension("RetailPrice", "RetailPrice")]
        public int RetailPrice { get; set; }

        
    }
}
