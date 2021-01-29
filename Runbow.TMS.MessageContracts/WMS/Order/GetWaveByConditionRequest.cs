using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Order;

namespace Runbow.TWS.MessageContracts.WMS.Order
{
    public class GetWaveByConditionRequest
    {
        public WaveSearchCondition SearchCondition { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
