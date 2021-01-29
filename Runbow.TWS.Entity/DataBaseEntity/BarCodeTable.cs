using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.DataBaseEntity
{
    public class BarCodeTable
    {
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        [EntityPropertyExtension("Type", "Type")]
        public string Type { get; set; }
        [EntityPropertyExtension("OrderID", "OrderID")]
        public Int32 OrderID { get; set; }
        [EntityPropertyExtension("OrderNumber", "OrderNumber")]
        public string OrderNumber { get; set; }
        [EntityPropertyExtension("DetailID", "DetailID")]
        public Int32 DetailID { get; set; }
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public Int32 CustomerID { get; set; }
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }
        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public Int32 WarehouseID { get; set; }
        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }
        [EntityPropertyExtension("Count", "Count")]
        public Int32 Count { get; set; }
    }
}
