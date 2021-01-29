using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.WMS.Warehouse
{
   public class WarehouseAllocateRequest
    {
        public long CustomerID { get; set; }

        public long UserID { get; set; }

        public long WarehouseID { get; set; }

        public string  Creator { get; set; }
    }
}
