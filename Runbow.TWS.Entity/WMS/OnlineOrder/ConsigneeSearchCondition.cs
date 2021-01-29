using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.OnlineOrder
{
    public class ConsigneeSearchCondition
    {
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserAddress { get; set; }
        public string UserEmail { get; set; }
        public int UserType { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
