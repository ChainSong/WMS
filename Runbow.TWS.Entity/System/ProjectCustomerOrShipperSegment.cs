using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class ProjectCustomerOrShipperSegment
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        [EntityPropertyExtension("Target", "Target")]
        public int Target { get; set; }

        [EntityPropertyExtension("CustomerOrShipperID", "CustomerOrShipperID")]
        public long CustomerOrShipperID { get; set; }

        [EntityPropertyExtension("RelatedCustomerID", "RelatedCustomerID")]
        public long RelatedCustomerID { get; set; }

        [EntityPropertyExtension("State", "State")]
        public bool State { get; set; }

        [EntityPropertyExtension("SegmentID", "SegmentID")]
        public long SegmentID { get; set; }
    }
}
