using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class SampleStockSelect
    {
        //需求中没有此列
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        [EntityPropertyExtension("Size", "Size")]
        public string Size { get; set; }

        [EntityPropertyExtension("Gender", "Gender")]
        public string Gender { get; set; }

        [EntityPropertyExtension("Category", "Category")]
        public string Category { get; set; }

        [EntityPropertyExtension("Silhouette", "Silhouette")]
        public string Silhouette { get; set; }

        [EntityPropertyExtension("SKUDesc", "SKUDesc")]
        public string SKUDesc { get; set; }

        [EntityPropertyExtension("PE", "PE")]
        public string PE { get; set; }

        [EntityPropertyExtension("Season", "Season")]
        public string Season { get; set; }


        [EntityPropertyExtension("LotCategory02", "LotCategory02")]
        public string LotCategory02 { get; set; }

        [EntityPropertyExtension("Qty", "Qty")]
        public int Qty { get; set; }

        [EntityPropertyExtension("FactoryCode", "FactoryCode")]
        public string FactoryCode { get; set; }

        [EntityPropertyExtension("ModelName", "ModelName")]
        public string ModelName { get; set; }

        [EntityPropertyExtension("FOB", "FOB")]
        public string FOB { get; set; }

        [EntityPropertyExtension("RetailPrice", "RetailPrice")]
        public string RetailPrice { get; set; }

        [EntityPropertyExtension("keepIntight", "keepIntight")]
        public string keepIntight { get; set; }
        //Factory Code\Model name\FOB\Retail Price\Keep in tight导入
    }
}
