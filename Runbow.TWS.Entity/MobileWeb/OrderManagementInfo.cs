using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.MobilePOD
{
    public class OrderManagementInfo
    {

        [EntityPropertyExtension("ID", "ID")]
        public string ID { get; set; }
        [EntityPropertyExtension("SystemNumber", "SystemNumber")]
        public string SystemNumber { get; set; }
        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }
        [EntityPropertyExtension("ShipperName", "ShipperName")]
        public string ShipperName { get; set; }
        [EntityPropertyExtension("ActualDeliveryDate", "ActualDeliveryDate")]
        public string ActualDeliveryDate { get; set; }
        [EntityPropertyExtension("StartCityName", "StartCityName")]
        public string StartCityName { get; set; }
        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }
        [EntityPropertyExtension("Weight", "Weight")]
        public string Weight { get; set; }
        [EntityPropertyExtension("GoodsNumber", "GoodsNumber")]
        public string GoodsNumber { get; set; }
        [EntityPropertyExtension("Volume", "Volume")]
        public string Volume { get; set; }
        [EntityPropertyExtension("ticketkey", "ticketkey")]
        public string ticketkey { get; set; }

        
    }
}
