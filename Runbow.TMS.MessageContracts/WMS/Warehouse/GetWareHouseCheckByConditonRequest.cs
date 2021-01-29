using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Warehouse;

namespace Runbow.TWS.MessageContracts 
{
    public class GetWarehouseCheckByConditonRequest
    {
        public WarehouseCheck Warehousecheck { get; set; }
        public WarehouseCheckSearchCondition WLSearchCondition { get; set; }
        public IEnumerable<WarehouseCheckDetail> Warehousecheckdetails { get; set; }
        public string[] Roles;
        public string[] ActualQTY;

        public long CustomerID { get; set; }

        public long WarehouseID { get; set; }
        public long AreaID { get; set; }
        public long Type { get; set; } 
        public long Types { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
