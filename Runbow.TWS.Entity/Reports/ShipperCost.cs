using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class ShipperCost
    {
        [EntityPropertyExtension("ShipperID", "ShipperID")]
        public long ShipperID { get; set; }

        [EntityPropertyExtension("ShipperName", "ShipperName")]
        public string ShipperName { get; set; }

        [EntityPropertyExtension("Year", "Year")]
        public string Year { get; set; }

        [EntityPropertyExtension("Month", "Month")]
        public int Month { get; set; }

        [EntityPropertyExtension("Fee", "Fee")]
        public decimal Fee { get; set; }

        [EntityPropertyExtension("RelatedCustomerID", "RelatedCustomerID")]
        public long RelatedCustomerID { get; set; }

        [EntityPropertyExtension("RelatedCustomerName", "RelatedCustomerName")]
        public string RelatedCustomerName { get; set; }
    }
}