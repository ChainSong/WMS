using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.MessageContracts.POD
{
    public class QueryBAFPriceRequest
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }
        [EntityPropertyExtension("BAFPrice", "BAFPrice")]
        public double BAFPrice { get; set; }
        [EntityPropertyExtension("BAFStartTime", "BAFStartTime")]
        public DateTime BAFStartTime { get; set; }
        [EntityPropertyExtension("BAFEndTime", "BAFEndTime")]
        public DateTime BAFEndTime { get; set; }
    }
}
