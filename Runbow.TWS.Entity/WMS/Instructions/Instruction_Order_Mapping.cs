using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Instructions
{
    public class Instruction_Order_Mapping
    {
        [EntityPropertyExtension("WorkStationID", "WorkStationID")]
        public long? WorkStationID { get; set; }
        [EntityPropertyExtension("RePickWallID", "RePickWallID")]
        public long? RePickWallID { get; set; }
        [EntityPropertyExtension("InstructionsID", "InstructionsID")]
        public long? InstructionsID { get; set; }
        [EntityPropertyExtension("OrderID", "OrderID")]
        public long? OrderID { get; set; }
        [EntityPropertyExtension("OrderNumber", "OrderNumber")]
        public string OrderNumber { get; set; }
        [EntityPropertyExtension("ReleatedDetailID", "ReleatedDetailID")]
        public long? ReleatedDetailID { get; set; }
    }
}
