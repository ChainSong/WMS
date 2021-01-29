using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts.WMS.PreOrders
{
    public class PreOrderRequest
    {
        public PreOrderSearchCondition SearchCondition { get; set; }

        public IEnumerable<PreOrderDetail> PreOd { get; set; }

        public IEnumerable<PreOrder> PreOrderList { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
