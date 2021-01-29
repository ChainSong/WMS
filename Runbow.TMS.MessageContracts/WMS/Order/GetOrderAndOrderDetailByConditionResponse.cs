using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.NIKECE.ShipRequest;
namespace Runbow.TWS.MessageContracts
{
    public class GetOrderAndOrderDetailByConditionResponse
    {
        public IEnumerable<OrderDetailInfo> OrderDetailCollection { get; set; }

        public OrderInfo order { get; set; }

        public IEnumerable<OrderInfo> OrderCollection { get; set; }

        public IEnumerable<PreOrder> PreOrderCollection { get; set; }

        public IEnumerable<PreOrderDetail> PreOrderDetailCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }

        public long UPCSum { get; set; }

        public long OrderSum { get; set; }

        public IEnumerable<WMS_ShipRequestHeader> ShipRequestHeaderCollection { get; set; }
        public IEnumerable<WMS_ShipRequestDetail> ShipRequestDetailCollection { get; set; }
    }
}
