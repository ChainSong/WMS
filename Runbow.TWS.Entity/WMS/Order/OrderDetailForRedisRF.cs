using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    [Serializable]
    public class OrderDetailForRedisRF:OrderDetailInfo
    {
        [EntityPropertyExtension("QtyPicked", "QtyPicked")]
        public decimal? QtyPicked { get; set; }

        [EntityPropertyExtension("BoxNum", "BoxNum")]
        public string BoxNum { get; set; } //箱型
    }
}
