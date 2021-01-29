using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.InventoryApi
{
    public class SampleStockCondition
    {
        public string SKU { get; set; }

        public string Size { get; set; }

        public string Gender { get; set; }

        public string PE { get; set; }

        public string Season { get; set; }

        public string ModelName { get; set; }

        public string Silhouette { get; set; }

        public string FOBBegin { get; set; }

        public string FOBEnd { get; set; }

        public string RetailPriceBegin { get; set; }

        public string RetailPriceEnd { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
