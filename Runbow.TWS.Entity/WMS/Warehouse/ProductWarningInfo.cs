using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class ProductWarningInfo
    {
       
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }

        [EntityPropertyExtension("ProductID", "ProductID")]
        public long ProductID { get; set; }

        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }

        [EntityPropertyExtension("ProductName", "ProductName")]
        public string ProductName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("Status", "Status")]
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }

        [EntityPropertyExtension("MinNumber", "MinNumber")]
        public decimal MinNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [EntityPropertyExtension("MaxNumber", "MaxNumber")]
        public decimal MaxNumber { get; set; }

      
    }
}
