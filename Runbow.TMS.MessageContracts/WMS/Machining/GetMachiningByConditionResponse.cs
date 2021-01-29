using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts.WMS.Machining
{
    public class GetMachiningByConditionResponse
    {
        public IEnumerable<WMS_MachiningHeaderAndDetail> MachiningCollection { get; set; }

        public IEnumerable<Inventorys> InventoryCollection { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }
    }
}
