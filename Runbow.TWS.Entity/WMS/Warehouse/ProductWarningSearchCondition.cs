using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class ProductWarningSearchCondition
    {
        public long CustomerID { get; set; }
        public long ProductID { get; set; }       
        public long WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string ProductName { get; set; }
        public decimal MinNumber { get; set; }
        public decimal MaxNumber { get; set; }
    }
}
