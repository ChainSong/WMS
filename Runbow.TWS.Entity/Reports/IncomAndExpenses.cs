using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class IncomAndExpenses
    {
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("Year", "Year")]
        public string Year { get; set; }

        [EntityPropertyExtension("Month", "Month")]
        public int Month { get; set; }

        [EntityPropertyExtension("Type", "Type")]
        public int Type { get; set; }

        [EntityPropertyExtension("Fee", "Fee")]
        public decimal Fee { get; set; }
    }
}