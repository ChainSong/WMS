using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Warehouse
{
  public   class WarehouseAllocate
    {
        [EntityPropertyExtension("UserID", "UserID")]
        public long UserID { get; set; }

        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }

        public bool Checked { get; set; }
    }
}
