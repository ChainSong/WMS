using Runbow.TWS.Entity.WMS.Receipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.ASNs
{
    public class GetASNNewBoxLabelByConditionRequest
    {//NIKE退货仓-
        public ASNNewBoxLabelSearchCondition SearchCondition { get; set; }

        public Int32 ID { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
