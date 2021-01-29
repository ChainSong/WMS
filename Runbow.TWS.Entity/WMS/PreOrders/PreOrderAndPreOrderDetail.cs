using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.PreOrders
{
    public class PreOrderAndPreOrderDetail
    {
        public  PreOrderSearchCondition SearchCondition { get; set; }

        public IEnumerable<PreOrderDetail> PreOd { get; set; }

        public PreOrder PreO { get; set; }

        public IEnumerable<PreOrder> PreOrderList { get; set; }

        public IEnumerable<PreOrderDetail> PreOrderDetailList { get; set; }

        public IEnumerable<DistributionInformation> DisInfo { get; set; }

        public IEnumerable<OrderInfo> OrderInfo { get; set; }

        public IEnumerable<Inventorys> Inventorys { get; set; }
    }
}
