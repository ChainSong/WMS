using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.Reports
{
    public class ShowCaseHotMap
    {
        [EntityPropertyExtension("PODID", "PODID")]
        public int PODID { get; set; }
        [EntityPropertyExtension("SystemNumber", "SystemNumber")]
        public string SystemNumber { get; set; }
        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }
        [EntityPropertyExtension("EndCityName", "EndCityName")]
        public string EndCityName { get; set; }
        [EntityPropertyExtension("ShippingAddress", "ShippingAddress")]
        public string ShippingAddress { get; set; }
        [EntityPropertyExtension("EndCitylat", "EndCitylat")]
        public string EndCitylat { get; set; }
        [EntityPropertyExtension("EndCitylng", "EndCitylng")]
        public string EndCitylng { get; set; }
        [EntityPropertyExtension("ShippingAddresslat", "ShippingAddresslat")]
        public string ShippingAddresslat { get; set; }
        [EntityPropertyExtension("ShippingAddresslng", "ShippingAddresslng")]
        public string ShippingAddresslng { get; set; }
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public  float BoxNumber { get; set; }
        [EntityPropertyExtension("GoodsNumber", "GoodsNumber")]
        public float GoodsNumber { get; set; }
        [EntityPropertyExtension("ActualDeliveryDate", "ActualDeliveryDate")]
        public string ActualDeliveryDate { get; set; }
        [EntityPropertyExtension("Count", "Count")]
        public int Count { get; set; }
        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }
        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }
    }
}
