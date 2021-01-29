using Runbow.TWS.Entity.WMS.Instructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.IntelligentWarehouse
{
    public class PickUpGoodsManagementResponse
    {
        public IEnumerable<Instructions> instructions { get; set; }


        public int PageIndex { get; set; }

        public int PageCount { get; set; }
    }
}
