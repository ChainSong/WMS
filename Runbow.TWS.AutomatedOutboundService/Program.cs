using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.WMS.PreOrders;

namespace Runbow.TWS.AutomatedOutboundService
{
    class Program
    {
        static void Main(string[] args)
        {
            //获取要分配的任务订单 12512
            PreOrderService service = new PreOrderService();
            PreOrderSearchCondition SearchCondition = new PreOrderSearchCondition();
            SearchCondition.ID = 12512;
            var getPreOrderByConditionResponse = new PreOrderService().GetPreOrderAndDetail(new PreOrderRequest() { SearchCondition = SearchCondition });
            //service.GetPreOrderAndDetail(
        }
    }
}
