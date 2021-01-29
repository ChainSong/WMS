using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class CustomerPodYearCount
    {
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("PodCount", "PodCount")]
        public int PodCount { get; set; }

        [EntityPropertyExtension("Year", "Year")]
        public string Year { get; set; }
    }
}
