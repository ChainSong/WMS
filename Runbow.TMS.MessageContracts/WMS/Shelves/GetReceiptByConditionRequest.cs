using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.Shelves;

namespace Runbow.TWS.MessageContracts.WMS.Shelves
{
    public  class GetReceiptByConditionRequest
    {
        public GetReceiptbyCondition Condition { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
