using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Report
{
    [Serializable]
    public class ReportSKUChange
    {

        /// <summary>
        /// 客户(货主)编号
        /// </summary>
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID { get; set; }

        [EntityPropertyExtension("SystemNumber", "SystemNumber")]
        public string SystemNumber { get; set; }

        [EntityPropertyExtension("ExternNumber", "ExternNumber")]
        public string ExternNumber { get; set; }

        [EntityPropertyExtension("Type", "Type")]
        public string Type { get; set; }

        [EntityPropertyExtension("OrderType", "OrderType")]
        public string OrderType { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("Warehouse", "Warehouse")]
        public string Warehouse { get; set; }

        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }

        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        [EntityPropertyExtension("FromArea", "FromArea")]
        public string FromArea { get; set; }

        [EntityPropertyExtension("ToArea", "ToArea")]
        public string ToArea { get; set; }

        [EntityPropertyExtension("FromLocation", "FromLocation")]
        public string FromLocation { get; set; }

        [EntityPropertyExtension("ToLocation", "ToLocation")]
        public string ToLocation { get; set; }

        [EntityPropertyExtension("FromGoodsType", "FromGoodsType")]
        public string FromGoodsType { get; set; }

        [EntityPropertyExtension("ToGoodsType", "ToGoodsType")]
        public string ToGoodsType { get; set; }

        [EntityPropertyExtension("FromQty", "FromQty")]
        public string FromQty { get; set; }

        [EntityPropertyExtension("ToQty", "ToQty")]
        public string ToQty { get; set; }

        [EntityPropertyExtension("Qty", "Qty")]
        public string Qty { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
    }
}
