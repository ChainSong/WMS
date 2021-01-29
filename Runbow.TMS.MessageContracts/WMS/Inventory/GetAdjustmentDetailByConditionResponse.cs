using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Inventory
{
   public class GetAdjustmentDetailByConditionResponse
    {
       public IEnumerable<AdjustmentDetail> AdjustmentDetailCollection { get; set; }
       public Adjustment Adjustment { get; set; }
        public IEnumerable<Adjustment> AdjustmentCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
