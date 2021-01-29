using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Inventory
{
    public class DirectAddInventory
    {
        [EntityPropertyExtension("Id", "Id")]
        public long? Id { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID { get; set; }

        [EntityPropertyExtension("Customer", "Customer")]
        public string Customer { get; set; }

        [EntityPropertyExtension("Brand", "Brand")]
        public string Brand { get; set; }

        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }

        [EntityPropertyExtension("Price", "Price")]
        public decimal Price { get; set; }

        [EntityPropertyExtension("Quantity", "Quantity")]
        public int Quantity { get; set; }

        [EntityPropertyExtension("TotalPrice", "TotalPrice")]
        public decimal TotalPrice { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }



        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }

        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }

        [EntityPropertyExtension("Str4", "Str4")]
        public string Str4 { get; set; }

        [EntityPropertyExtension("InventoryType", "InventoryType")]
        public int InventoryType { get; set; }

        [EntityPropertyExtension("InventoryId", "InventoryId")]
        public string InventoryId { get; set; }

        [EntityPropertyExtension("GuidePrice", "GuidePrice")]
        public decimal GuidePrice { get; set; }

        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }

        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1 { get; set; }

        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime? DateTime2 { get; set; }

        [EntityPropertyExtension("SKUName", "SKUName")]
        public string SKUName { get; set; }


        [EntityPropertyExtension("SCPrice", "SCPrice")]
        public decimal? SCPrice { get; set; }

        [EntityPropertyExtension("goodsName", "goodsName")]
        public string goodsName { get; set; }

        [EntityPropertyExtension("Qty", "Qty")]
        public int Qty { get; set; }

        [EntityPropertyExtension("Quantity1", "Quantity1")]
        public int Quantity1 { get; set; }

        [EntityPropertyExtension("Quantity2", "Quantity2")]
        public int Quantity2 { get; set; }

        [EntityPropertyExtension("TotalPrice1", "TotalPrice1")]
        public decimal TotalPrice1 { get; set; }
        [EntityPropertyExtension("TotalPrice2", "TotalPrice2")]
        public decimal TotalPrice2 { get; set; }
    }
}
