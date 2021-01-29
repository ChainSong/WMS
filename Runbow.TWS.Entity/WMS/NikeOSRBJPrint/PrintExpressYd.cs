using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.NikeOSRBJPrint
{
    public class PrintExpressYd
    {
        [EntityPropertyExtension("OrderNumber", "OrderNumber")]
        public string OrderNumber { get; set; }
        [EntityPropertyExtension("ExternOrderNumber", "ExternOrderNumber")]
        public string ExternOrderNumber { get; set; }
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }
        [EntityPropertyExtension("ExpressNumber", "ExpressNumber")]
        public string ExpressNumber { get; set; }
        [EntityPropertyExtension("PackageNumber", "PackageNumber")]
        public string PackageNumber { get; set; }
        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }
        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }
        [EntityPropertyExtension("ReceiverName", "ReceiverName")]
        public string ReceiverName { get; set; }
        [EntityPropertyExtension("ReceiverMobile", "ReceiverMobile")]
        public string ReceiverMobile { get; set; }
        [EntityPropertyExtension("ReceiverAddress", "ReceiverAddress")]
        public string ReceiverAddress { get; set; }
        [EntityPropertyExtension("SenderName", "SenderName")]
        public string SenderName { get; set; }
        [EntityPropertyExtension("SenderMobile", "SenderMobile")]
        public string SenderMobile { get; set; }
        [EntityPropertyExtension("SenderAddress", "SenderAddress")]
        public string SenderAddress { get; set; }
        [EntityPropertyExtension("position", "position")]
        public string position { get; set; }
        [EntityPropertyExtension("position_no", "position_no")]
        public string position_no { get; set; }
        [EntityPropertyExtension("mailNo", "mailNo")]
        public string mailNo { get; set; }
        [EntityPropertyExtension("four_code", "four_code")]
        public string four_code { get; set; }
        [EntityPropertyExtension("package_wdjc", "package_wdjc")]
        public string package_wdjc { get; set; }
        [EntityPropertyExtension("cus_area1", "cus_area1")]
        public string cus_area1 { get; set; }
    }
}
