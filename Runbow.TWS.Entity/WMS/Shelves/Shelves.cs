using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Shelves
{
    [Serializable]
    public class Shelves
    {
        [EntityPropertyExtension("LocationMax", "LocationMax")]
        public string LocationMax { get; set; }

        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }

        [EntityPropertyExtension("Unit", "Unit")]
        public string Unit { get; set; }

        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }

        [EntityPropertyExtension("LineNumber", "LineNumber")]
        public string LineNumber { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public string CustomerID { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("SkuLineNumber", "SkuLineNumber")]
        public string SkuLineNumber { get; set; }

        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        [EntityPropertyExtension("ReceiptDate", "ReceiptDate")]
        public DateTime? ReceiptDate { get; set; }

        [EntityPropertyExtension("GoodsName", "GoodsName")]
        public string GoodsName { get; set; }

        [EntityPropertyExtension("GoodsType", "GoodsType")]
        public string GoodsType { get; set; }

        [EntityPropertyExtension("QtyExpected", "QtyExpected")]
        public decimal QtyExpected { get; set; }

        [EntityPropertyExtension("QtyReceived", "QtyReceived")]
        public decimal QtyReceived { get; set; }

        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }

        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public string WarehouseID { get; set; }

        [EntityPropertyExtension("ReceiptNumber", "ReceiptNumber")]
        public string ReceiptNumber { get; set; }

        [EntityPropertyExtension("ExternReceiptNumber", "ExternReceiptNumber")]
        public string ExternReceiptNumber { get; set; }

        [EntityPropertyExtension("Area", "Area")]
        public string Area { get; set; }

        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }

        [EntityPropertyExtension("ASNID", "ASNID")]
        public string ASNID { get; set; }

        [EntityPropertyExtension("ASNNumber", "ASNNumber")]
        public string ASNNumber { get; set; }

        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }

        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        [EntityPropertyExtension("RDID", "RDID")]
        public string RDID { get; set; }

        [EntityPropertyExtension("RID", "RID")]
        public string RID { get; set; }

        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1 { get; set; }
    }
}
