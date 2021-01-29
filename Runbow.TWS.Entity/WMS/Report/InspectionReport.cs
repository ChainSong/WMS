using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Report
{

    /// <summary>
    ///  表WMS_ASNDetail_ScanABC
    /// </summary>
    public class InspectionReport
    {
        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("ASNID", "ASNID")]
        public long? ASNID { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("ASNNumber", "ASNNumber")]
        public string ASNNumber { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("ExternReceiptNumber", "ExternReceiptNumber")]
        public string ExternReceiptNumber { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("LineNumber", "LineNumber")]
        public string LineNumber { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("QtyExpected", "QtyExpected")]
        public decimal? QtyExpected { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("QtyReceived", "QtyReceived")]
        public decimal? QtyReceived { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("QtyReceipt", "QtyReceipt")]
        public decimal? QtyReceipt { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("GoodsType", "GoodsType")]
        public string GoodsType { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("GoodsName", "GoodsName")]
        public string GoodsName { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("WareHouseID", "WareHouseID")]
        public long? WareHouseID { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Area", "Area")]
        public string Area { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Unit", "Unit")]
        public string Unit { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str1", "str1")]
        public string str1 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str2", "str2")]
        public string str2 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str3", "str3")]
        public string str3 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str4", "str4")]
        public string str4 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str5", "str5")]
        public string str5 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str6", "str6")]
        public string str6 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str7", "str7")]
        public string str7 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str8", "str8")]
        public string str8 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str9", "str9")]
        public string str9 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str10", "str10")]
        public string str10 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str11", "str11")]
        public string str11 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str12", "str12")]
        public string str12 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str13", "str13")]
        public string str13 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str14", "str14")]
        public string str14 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str15", "str15")]
        public string str15 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str16", "str16")]
        public string str16 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str17", "str17")]
        public string str17 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str18", "str18")]
        public string str18 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str19", "str19")]
        public string str19 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("str20", "str20")]
        public string str20 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime? DateTime2 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("DateTime3", "DateTime3")]
        public DateTime? DateTime3 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("DateTime4", "DateTime4")]
        public DateTime? DateTime4 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("DateTime5", "DateTime5")]
        public DateTime? DateTime5 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Int1", "Int1")]
        public int? Int1 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Int2", "Int2")]
        public int? Int2 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Int3", "Int3")]
        public int? Int3 { get; set; }
    }
}
