using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Print;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Order
{
    public class OrderAndDetailModel
    {
        public IEnumerable<OrderInfo> orderInfos { get; set; }//订单头
        public IEnumerable<OrderDetailInfo> orderDetailInfos { get; set; } //订单明细

        public IEnumerable<PrintHeader> printHeaders { get; set; }//波次头信息

    }
}
