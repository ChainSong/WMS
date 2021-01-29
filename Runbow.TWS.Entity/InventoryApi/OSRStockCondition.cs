using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.InventoryApi
{
   public class OSRStockCondition
    {
        public string SKU { get; set; }

        public string Season { get; set; }

        public string Category { get; set; }

        public string Article { get; set; }

        public string UPC { get; set; }

        public string PE { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        
       
    }
}
