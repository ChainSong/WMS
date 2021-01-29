using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts.WMS.PreOrders
{
    public class PreOrderResponse
    {

        public IEnumerable<PreOrder> PreO { get; set; }

        public PreOrder preOrder { get; set; }

        public PreOrderDetail preOrderDetail { get; set; }

        public IEnumerable<PreOrderSearchCondition> SearchCondition { get; set; }

        public IEnumerable<PreOrderDetail> PreOd { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
