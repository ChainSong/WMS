using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class ProjectCustomerWarehouse : ProjectUserRole
    {
        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }
        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }
       
    }
}