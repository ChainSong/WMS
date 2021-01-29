using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Warehouse;

namespace Runbow.TWS.MessageContracts
{
    public class GetWarehouseCheckByConditionResponse
    {
        public WarehouseCheck WarehouseCheck { get; set; }
        public IEnumerable<WarehouseCheck> WarehouseCheckCollection { get; set; }
        public IEnumerable<WarehouseCheckDetail> WarehouseCheckDetailCollection { get; set; }

        public int PageCount { get; set; }

        public string Message { get; set; }

        public int PageIndex { get; set; }
    }
}
