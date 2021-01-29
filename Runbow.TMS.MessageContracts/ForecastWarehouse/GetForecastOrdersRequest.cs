using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
  public   class GetForecastOrdersRequest
    {
        public IEnumerable<ForecastOrders> IEnumerabletForecastOrders { get; set; }
        public ForecastOrders ForecastOrders { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public long? ID { get; set; } 
    }
}
