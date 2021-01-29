using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.PreOrders
{
    public class PreOrderDistributionResult
    {
        public string ExternOrderNumber { get; set; }

        public int Status { get; set; }

        public string SKU { get; set; }

        public string OriginalQty { get; set; }

        public string AllocatedQty { get; set; }

    }
}
