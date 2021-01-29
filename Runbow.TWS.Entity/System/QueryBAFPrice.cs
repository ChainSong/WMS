using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class QueryBAFPrice
    {
        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public int ProjectID { get; set; }
        [EntityPropertyExtension("BAFPrice", "BAFPrice")]
        public decimal BAFPrice { get; set; }
        [EntityPropertyExtension("BAFStartTime", "BAFStartTime")]
        public DateTime BAFStartTime { get; set; }
        [EntityPropertyExtension("BAFEndTime", "BAFEndTime")]
        public DateTime BAFEndTime { get; set; }
    }
}
