using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetReceiptDetailByConditionRequest
    {
        public ReceiptDetailSearchCondition SearchCondition { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
