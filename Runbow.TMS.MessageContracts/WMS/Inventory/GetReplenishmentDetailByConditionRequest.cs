using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.Inventory;

namespace Runbow.TWS.MessageContracts.WMS.Inventory
{
    public class GetReplenishmentDetailByConditionRequest
    {
        public ReplenishmentDetailSearchCondition SearchCondition { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
