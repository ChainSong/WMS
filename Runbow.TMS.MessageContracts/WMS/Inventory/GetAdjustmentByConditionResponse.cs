using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Inventory
{
   public class GetAdjustmentByConditionResponse
    {
       public IEnumerable<Adjustment> ASNCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
