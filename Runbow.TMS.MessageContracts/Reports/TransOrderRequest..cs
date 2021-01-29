using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.Reports
{
    public class TransOrderRequest
    {

        public string ID { get; set; }

        public string Customers { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        //public string Consignee { get; set; }

        public string startCityTreeName { get; set; }

        public string endCityTreeName { get; set; }

        public string ShipperName { get; set; }

        public string Time { get; set; }

        public int? PageIndex { get; set; }

        public int? PageSize { get; set; }
        
    }
}
