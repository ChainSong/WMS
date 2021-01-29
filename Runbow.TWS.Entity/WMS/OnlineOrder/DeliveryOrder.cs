using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.OnlineOrder
{
    public class DeliveryOrder
    {
        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }
        [EntityPropertyExtension("Size", "Size")]
        public string Size { get; set; }
        [EntityPropertyExtension("Season", "Season")]
        public string Season { get; set; }
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        [EntityPropertyExtension("Gender", "Gender")]
        public string Gender { get; set; }
        [EntityPropertyExtension("Category", "Category")]
        public string Category { get; set; }
        [EntityPropertyExtension("ModelName", "ModelName")]
        public string ModelName { get; set; }
        [EntityPropertyExtension("Silhouette", "Silhouette")]
        public string Silhouette { get; set; }
        [EntityPropertyExtension("PE", "PE")]
        public string PE { get; set; }
        [EntityPropertyExtension("QTY", "QTY")]
        public int QTY { get; set; }
        [EntityPropertyExtension("RequireQTY", "RequireQTY")]
        public int RequireQTY { get; set; }
        [EntityPropertyExtension("ReceivedQTY", "ReceivedQTY")]
        public int ReceivedQTY { get; set; }
        [EntityPropertyExtension("BuyerPO", "BuyerPO")]
        public string BuyerPO { get; set; }
        [EntityPropertyExtension("Date1", "Date1")]
        public DateTime Date1 { get; set; }
        [EntityPropertyExtension("Date2", "Date2")]
        public DateTime Date2 { get; set; }
    }
}
