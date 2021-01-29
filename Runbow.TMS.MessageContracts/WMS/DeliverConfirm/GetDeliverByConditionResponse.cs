using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.DeliveryConfirm;

namespace Runbow.TWS.MessageContracts.WMS.DeliverConfirm
{
    public class GetDeliverByConditionResponse
    {
        public IEnumerable<DeliverHeader> DeliverHeaderConnection { get; set; }
        public IEnumerable<DeliverDetail> DeliverDetailConnection { get; set; }//交接单明细
        public IEnumerable<DeliverDetail> DeliverExpressNoConnection { get; set; }//快递单列表
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
    }
}
