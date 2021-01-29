using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.DeliveryConfirm;

namespace Runbow.TWS.MessageContracts.WMS.DeliverConfirm
{
    public class GetDeliverByConditionRequest
    {
        public DeliverHeaderSearchCondition SearchCondition { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

    }
}
