using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Shelves;

namespace Runbow.TWS.MessageContracts.WMS.Shelves
{
    public class GetReceiptByConditionResponse
    {
        public IEnumerable<StoresByGetReceipt> receipt { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
