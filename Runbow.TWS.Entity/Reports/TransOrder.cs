using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.Reports
{
    public class TransOrder
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("ShipperName", "ShipperName")]
        public string ShipperName { get; set; }

        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }

        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }

        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        [EntityPropertyExtension("ActualDeliveryDate", "ActualDeliveryDate")]
        public string ActualDeliveryDate { get; set; }

        [EntityPropertyExtension("GoodsNumber", "GoodsNumber")]
        public string GoodsNumber { get; set; }

        [EntityPropertyExtension("Weight", "Weight")]
        public string Weight { get; set; }

        [EntityPropertyExtension("Volume", "Volume")]
        public string Volume { get; set; }

    }
}
