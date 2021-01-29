using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class QueryCustomerPodYearMonthCountByTimeRangeRequest
    {
        public long ShipperID { get; set; }

        public string Year { get; set; }
    }
}
