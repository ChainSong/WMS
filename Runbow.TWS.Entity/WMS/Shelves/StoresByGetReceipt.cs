using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Shelves
{
    public class StoresByGetReceipt
    {

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public string CustomerID { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("ID", "ID")]
        public string ID { get; set; }

        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }

        [EntityPropertyExtension("ReceiptNumber", "ReceiptNumber")]
        public string ReceiptNumber { get; set; }

        [EntityPropertyExtension("ExternReceiptNumber", "ExternReceiptNumber")]
        public string ExternReceiptNumber { get; set; }

        [EntityPropertyExtension("ASNID", "ASNID")]
        public string ASNID { get; set; }

        [EntityPropertyExtension("ASNNumber", "ASNNumber")]
        public string ASNNumber { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }

        [EntityPropertyExtension("ReceiptDate", "ReceiptDate")]
        public DateTime? ReceiptDate { get; set; }

        [EntityPropertyExtension("Status", "Status")]
        public int Status { get; set; }

        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }

        [EntityPropertyExtension("LineNumber", "LineNumber")]
        public string LineNumber { get; set; }

        [EntityPropertyExtension("SkuLineNumber", "SkuLineNumber")]
        public string SkuLineNumber { get; set; }

        [EntityPropertyExtension("GoodsName", "GoodsName")]
        public string GoodsName { get; set; }

        [EntityPropertyExtension("GoodsType", "GoodsType")]
        public string GoodsType { get; set; }

        [EntityPropertyExtension("QtyReceived", "QtyReceived")]
        public int QtyReceived { get; set; }

        [EntityPropertyExtension("Area", "Area")]
        public string Area { get; set; }

        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }

        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }

        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        [EntityPropertyExtension("Unit", "Unit")]
        public string Unit { get; set; }

        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }

        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }
        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public string UpdateTime { get; set; }
        [EntityPropertyExtension("ReceiptTypeName", "ReceiptTypeName")]
        public string ReceiptTypeName { get; set; }
        [EntityPropertyExtension("ReceiptType", "ReceiptType")]
        public string ReceiptType { get; set; }
        [EntityPropertyExtension("ReceiptStatusName", "ReceiptStatusName")]
        public string ReceiptStatusName { get; set; }
        [EntityPropertyExtension("QtyExpected", "QtyExpected")]
        public string QtyExpected { get; set; }
        [EntityPropertyExtension("CompleteDate", "CompleteDate")]
        public string CompleteDate { get; set; }
        [EntityPropertyExtension("str15", "str15")]
        public string str15 { get; set; }

        //门店代码
        [EntityPropertyExtension("str3", "str3")]
        public string str3 { get; set; }

        /// <summary>
        /// 总件数
        /// </summary>
        [EntityPropertyExtension("str13", "str13")]
        public string str13 { get; set; }

    }
}
