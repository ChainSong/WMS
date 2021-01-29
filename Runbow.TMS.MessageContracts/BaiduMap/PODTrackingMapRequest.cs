using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.BaiduMap
{
    public class PODTrackingMapRequest
    {
        public int Customer { get; set; }

        public string Customers { get; set; }

        public string CarNo { get; set; }

        public string PODID { get; set; }

        public string Type { get; set; }

        public string Customerordernumber { get; set; }

        public string ID { get; set; }

        public string EndCustomer { get; set; }

        public string Destination { get; set; }

        public DateTime? start_DeliveryDate { get; set; }

        public DateTime? end_DeliveryDate { get; set; }

        public DateTime? start_PlanArrive { get; set; }

        public DateTime? end_PlanArrive { get; set; }
    }
}
