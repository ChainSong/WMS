using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public  class CustomerPodYearMonthCount : CustomerPodYearCount
    {
        [EntityPropertyExtension("Month", "Month")]
        public int Month { get; set; }
    }
}
