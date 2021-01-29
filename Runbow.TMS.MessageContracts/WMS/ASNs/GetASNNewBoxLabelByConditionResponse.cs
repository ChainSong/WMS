using Runbow.TWS.Entity.WMS.Receipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.ASNs
{
    public class GetASNNewBoxLabelByConditionResponse
    {//NIKE退货仓-
        public ASNNewBoxLabel asnBox { get; set; }
        public IEnumerable<ASNNewBoxLabel> asnBoxCollection { get; set; }
        public int PageCount { get; set; }
        public int PageIndex { get; set; }
    }
}
