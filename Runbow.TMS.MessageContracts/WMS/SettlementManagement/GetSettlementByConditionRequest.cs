using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.SettlementManagement;

namespace Runbow.TWS.MessageContracts 
{
    public class GetSettlementByConditionRequest
    {
        public Settlement Settlement { get; set; }
        public SettlementSearchCondition WLSearchCondition { get; set; }
        public WMS_HiltibjSettled HiltibjSettled { get; set; }
        public IEnumerable<SettlementDetail> Settlementdetails { get; set; }
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
