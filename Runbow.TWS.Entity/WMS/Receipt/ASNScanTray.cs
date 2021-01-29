using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Receipt
{

    /// <summary>
    ///  WMS_ASN_ScanTray    
    /// </summary>
    public class ASNScanTray
    {
        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

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
        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long? WarehouseID { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }

        /// <summary>
        /// 托盘    
        /// </summary>
        [EntityPropertyExtension("TrayNumber", "TrayNumber")]
        public string TrayNumber { get; set; }

        /// <summary>
        /// 箱号    
        /// </summary>
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        /// <summary>
        /// 库位    
        /// </summary>
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }

        /// <summary>
        /// 1:预检台,箱号绑定托盘号;2:暂存库位,托盘绑定库位;3:质检台,托盘绑定质检台库位;4:装箱,新箱号绑定新托盘;5:暂存库位2,新托盘绑定新库位;6:打印库位标签操作台,托盘绑定操作台    
        /// </summary>
        [EntityPropertyExtension("Status", "Status")]
        public int? Status { get; set; }

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
        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Str4", "Str4")]
        public string Str4 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Str5", "Str5")]
        public string Str5 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Str6", "Str6")]
        public string Str6 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Str7", "Str7")]
        public string Str7 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Str8", "Str8")]
        public string Str8 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Str9", "Str9")]
        public string Str9 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Str10", "Str10")]
        public string Str10 { get; set; }

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

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Int4", "Int4")]
        public int? Int4 { get; set; }

        /// <summary>
        ///     
        /// </summary>
        [EntityPropertyExtension("Int5", "Int5")]
        public int? Int5 { get; set; }

    }
}
