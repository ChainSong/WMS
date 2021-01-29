using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Order
{
    public class OrderShipmentSearchCondition : WMS_OrderShipment
    {
        public string ExternOrderNumber { get; set; }
        public string StartCreateTime { get; set; }
        public string EndCreateTime { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }


        //public string StartTime { get; set; }
        //public string EndTime { get; set; }

       
    }
}
