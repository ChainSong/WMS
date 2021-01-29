using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class ProjectCustomer
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }

        [EntityPropertyExtension("IsDefault", "IsDefault")]
        public bool IsDefault { get; set; }

        [EntityPropertyExtension("Code", "Code")]
        public string Code { get; set; }

        [EntityPropertyExtension("Name", "Name")]
        public string Name { get; set; }
    }
}