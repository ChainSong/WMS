using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.WorkOrder
{
    public class WorkOrderProcess
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("WOID", "WOID")]
        public long WOID { get; set; }

        [EntityPropertyExtension("MessageContent", "MessageContent")]
        public string MessageContent { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }

        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime UpdateTime { get; set; }

        [EntityPropertyExtension("ParentID", "ParentID")]
        public long ParentID { get; set; }
    }
}
