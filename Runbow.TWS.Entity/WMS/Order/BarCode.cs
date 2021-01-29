using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS
{
    public class BarCodeInfo
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        [EntityPropertyExtension("BarCode", "BarCode")]
        public string BarCode { get; set; }
        [EntityPropertyExtension("Type", "Type")]
        public string Type { get; set; }
        [EntityPropertyExtension("OrderID", "OrderID")]
        public long OrderID { get; set; }
        [EntityPropertyExtension("OrderNumber", "OrderNumber")]
        public string OrderNumber { get; set; }
        [EntityPropertyExtension("DetailID", "DetailID")]
        public long DetailID { get; set; }
        [EntityPropertyExtension("Status", "Status")]
        public long Status { get; set; }
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID { get; set; }
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }
        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }
        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }
        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }
        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }
        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }
        [EntityPropertyExtension("Str4", "Str4")]
        public string Str4 { get; set; }
        [EntityPropertyExtension("Str5", "Str5")]
        public string Str5 { get; set; }
        [EntityPropertyExtension("BarCodeNumber", "BarCodeNumber")]
        public Int64 BarCodeNumber { get; set; }
        [EntityPropertyExtension("PackageNumber", "PackageNumber")]
        public string PackageNumber { get; set; }
        [EntityPropertyExtension("PackageID", "PackageID")]
        public Int64 PackageID { get; set; }
        [EntityPropertyExtension("PackageDetailID", "PackageDetailID")]
        public Int64 PackageDetailID { get; set; }
        [EntityPropertyExtension("Count", "Count")]
        public Int32 Count { get; set; }
        [EntityPropertyExtension("Result", "Result")]
        public string Result { get; set; }
    }
}
