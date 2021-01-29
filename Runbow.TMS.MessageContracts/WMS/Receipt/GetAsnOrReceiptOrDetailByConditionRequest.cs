using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetAsnOrReceiptOrDetailByConditionRequest
    {
        public ASNDetailSearchCondition SearchCondition { get; set; }
        public ASNSearchCondition ASNSearchCondition { get; set; }
        public ReceiptSearchCondition ReceiptSearchCondition { get; set; }
        public ReceiptDetailSearchCondition ReceiptDetailSearchCondition { get; set; }
        public int PageIndex { get; set; }

        public int PageSize { get; set; } 

    }
}
