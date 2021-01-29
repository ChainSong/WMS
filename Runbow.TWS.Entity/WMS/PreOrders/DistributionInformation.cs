using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.PreOrders
{
    public class DistributionInformation
    {
        [EntityPropertyExtension("POID", "POID")]
        public long POID { get; set; }

        [EntityPropertyExtension("ExternOrderNumber", "ExternOrderNumber")]
        public string ExternOrderNumber { get; set; }

        [EntityPropertyExtension("Customer", "Customer")]
        public string Customer { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }

        [EntityPropertyExtension("Note", "Note")]
        public string Note { get; set; }

        [EntityPropertyExtension("Type", "Type")]
        public string Type { get; set; }

        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        [EntityPropertyExtension("QTY", "QTY")]
        public string QTY { get; set; }

        [EntityPropertyExtension("Article", "Article")]
        public string Article { get; set; }

        [EntityPropertyExtension("Size", "Size")]
        public string Size { get; set; }

        [EntityPropertyExtension("BU", "BU")]
        public string BU { get; set; }

        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }

        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }
    }
}
