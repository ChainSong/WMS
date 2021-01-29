using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class GetServicePeriodByConditionRequest
    {
        public long ProjectID { get; set; }

        public long CustomerID { get; set; }

        public long StartCityID { get; set; }

        public long EndCityID { get; set; }
    }
}
