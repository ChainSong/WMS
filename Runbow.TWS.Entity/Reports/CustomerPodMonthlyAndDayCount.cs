using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class CustomerPodMonthlyAndDayCount
    {
        [EntityPropertyExtension("Month", "Month")]
        public int Month { get; set; }

        [EntityPropertyExtension("Day", "Day")]
        public int Day { get; set; }

        [EntityPropertyExtension("PodCount", "PodCount")]
        public int PodCount { get; set; }
    }
}
