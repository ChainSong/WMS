using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Order
{
    public class Order_Detail
    {
        [EntityPropertyExtension("Id", "Id")]
        public string Id { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        [EntityPropertyExtension("CreatorId", "CreatorId")]
        public string CreatorId { get; set; }

        [EntityPropertyExtension("CreatorRealName", "CreatorRealName")]
        public string CreatorRealName { get; set; }

        [EntityPropertyExtension("Deleted", "Deleted")]
        public int Deleted { get; set; }

        [EntityPropertyExtension("orderCode", "orderCode")]
        public string orderCode { get; set; }

        [EntityPropertyExtension("lineId", "lineId")]
        public string lineId { get; set; }

        [EntityPropertyExtension("platLineCode", "platLineCode")]
        public string platLineCode { get; set; }

        [EntityPropertyExtension("style_color", "style_color")]
        public string style_color { get; set; }

        [EntityPropertyExtension("size", "size")]
        public string size { get; set; }

        [EntityPropertyExtension("barCode", "barCode")]
        public string barCode { get; set; }

        [EntityPropertyExtension("quantity", "quantity")]
        public int quantity { get; set; }

        [EntityPropertyExtension("skuName", "skuName")]
        public string skuName { get; set; }

        [EntityPropertyExtension("totalPrice", "totalPrice")]
        public decimal? totalPrice { get; set; }

        [EntityPropertyExtension("unitPrice", "unitPrice")]
        public decimal? unitPrice { get; set; }

        [EntityPropertyExtension("remark", "remark")]
        public string remark { get; set; }

        [EntityPropertyExtension("extProps", "extProps")]
        public string extProps { get; set; }

    }
}
