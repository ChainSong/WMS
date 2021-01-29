using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.Order;

namespace Runbow.TWS.MessageContracts.WMS.Order
{
    public class GetWaveByConditionResponse
    {
        public IEnumerable<WMS_Wave> WaveCollection { get; set; }

        public IEnumerable<WMS_WaveDetail> WaveDetailCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
