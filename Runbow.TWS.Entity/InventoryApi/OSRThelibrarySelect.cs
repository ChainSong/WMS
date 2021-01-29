using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class OSRThelibrarySelect
    {
       [EntityPropertyExtension("TransOrderNO", "TransOrderNO")]
       public string TransOrderNO { get; set; }

        [EntityPropertyExtension("OrderKey", "OrderKey")]
        public string OrderKey { get; set; }

        [EntityPropertyExtension("DeliveryStore", "DeliveryStore")]
        public string DeliveryStore { get; set; }

        [EntityPropertyExtension("ShiptoCode", "ShiptoCode")]
        public string ShiptoCode { get; set; }

        [EntityPropertyExtension("ShippedDate", "ShippedDate")]
        public string ShippedDate { get; set; }

        [EntityPropertyExtension("ETA", "ETA")]
        public string ETA { get; set; }

        //[EntityPropertyExtension("ATA", "ATA")]
        //public string ATA { get; set; }

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

        [EntityPropertyExtension("Category", "Category")]
        public string Category { get; set; }

        [EntityPropertyExtension("SilHouette", "SilHouette")]
        public string SilHouette { get; set; }


        //[EntityPropertyExtension("Season", "Season")]
        //public string Season { get; set; }

        //[EntityPropertyExtension("LongMaterial", "LongMaterial")]
        //public string LongMaterial { get; set; }

       

       

       
        //APP、FWL
    }
}
