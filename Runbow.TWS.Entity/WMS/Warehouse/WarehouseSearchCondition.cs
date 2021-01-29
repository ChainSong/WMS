using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class WarehouseSearchCondition
    {
        public long ID { get; set; }
        public long ID2 { get; set; }
        public long? CustomerID { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseStatus { get; set; }
        public string WarehouseType { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ProvinceCity { get; set; }
        public string SearchType { get; set; }
        public long AreaID { get; set; }
        public long ProjectID { get; set; }
        public long UserID { get; set; }
    }
}
