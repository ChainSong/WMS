using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class ProjectCustomersOrShippers
    {
        [EntityPropertyExtension("ProjectShipperOrCustomerID", "ProjectShipperOrCustomerID")]
        public long ProjectShipperOrCustomerID { get; set; }

        [EntityPropertyExtension("Target", "Target")]
        public int Target { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        [EntityPropertyExtension("ProjectName", "ProjectName")]
        public string ProjectName { get; set; }

        [EntityPropertyExtension("CustomerOrShipperID", "CustomerOrShipperID")]
        public long CustomerOrShipperID { get; set; }

        [EntityPropertyExtension("CustomerOrShipperName", "CustomerOrShipperName")]
        public string CustomerOrShipperName { get; set; }

        [EntityPropertyExtension("IsDefault", "IsDefault")]
        public bool IsDefault { get; set; }

        [EntityPropertyExtension("State", "State")]
        public bool State { get; set; }

        [EntityPropertyExtension("SegmentID", "SegmentID")]
        public long? SegmentID { get; set; }
    }
}