using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.UnitAndSpecifications
{
    public class UnitAndSpecificationsInfo
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public int CustomerID { get; set; }

        [EntityPropertyExtension("CustomerIDs", "CustomerIDs")]
        public string CustomerIDs { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("Unit", "Unit")]
        public string Unit { get; set; }

        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }
        
    }
}
