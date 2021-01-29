using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WebApi
{
    public class WXPODRequest
    {
        
            public string Customer { get; set; }

            public string PodType { get; set; }

            public string ShipperType { get; set; }

            public string txtStart { get; set; }

            public string txtEnd { get; set; }

            public string datetimes { get; set; }

            public string st { get; set; }

            //public string CustomerOrderNumber { get; set; }

        
        
    }
}
