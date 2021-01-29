using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class ProjectUserCustomer : ProjectUserRole
    {
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }

        [EntityPropertyExtension("Code", "Code")]
        public string Code { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }
    }
}