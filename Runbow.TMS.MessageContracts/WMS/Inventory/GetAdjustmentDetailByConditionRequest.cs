using Runbow.TWS.Entity.WMS.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Inventory
{
   public class GetAdjustmentDetailByConditionRequest
    {
        public AdjustmentDetailSearchCondition SearchCondition { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
