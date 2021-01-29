using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Inventory
{
    public class ReplenishmentDetailSKUs
    {
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
    }
}
