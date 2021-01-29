using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.System
{
    public class CacheInfo
    {
        [EntityPropertyExtension("OperationType", "OperationType")]
        public int OperationType { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public int ProjectID { get; set; }

        [EntityPropertyExtension("UserID", "UserID")]
        public int UserID { get; set; }

        [EntityPropertyExtension("UserName", "UserName")]
        public string UserName { get; set; }

        [EntityPropertyExtension("UserType", "UserType")]
        public int UserType { get; set; }

        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public int WarehouseID { get; set; }

        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }

        [EntityPropertyExtension("WarehouseStatus", "WarehouseStatus")]
        public int WarehouseStatus { get; set; }

        [EntityPropertyExtension("WarehouseType", "WarehouseType")]
        public int WarehouseType { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID { get; set; }

        [EntityPropertyExtension("CustomerCode", "CustomerCode")]
        public string CustomerCode { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("CustomerState", "CustomerState")]
        public int CustomerState { get; set; }

        [EntityPropertyExtension("StoreType", "StoreType")]
        public string StoreType { get; set; }

        [EntityPropertyExtension("OperationAreaID", "OperationAreaID")]
        public int OperationAreaID { get; set; }

        [EntityPropertyExtension("Operation", "Operation")]
        public string Operation { get; set; }


    }
}

