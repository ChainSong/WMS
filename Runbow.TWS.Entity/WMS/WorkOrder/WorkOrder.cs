using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.WorkOrder
{
    public class WorkOrder
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("WorkOrderNumber", "WorkOrderNumber")]
        public string WorkOrderNumber { get; set; }

        [EntityPropertyExtension("WorkOrderType", "WorkOrderType")]
        public long WorkOrderType { get; set; }

        [EntityPropertyExtension("TypeName", "TypeName")]
        public string TypeName { get; set; }

        [EntityPropertyExtension("Title", "Title")]
        public string Title { get; set; }

        [EntityPropertyExtension("OrderContent", "OrderContent")]
        public string OrderContent { get; set; }

        [EntityPropertyExtension("Status", "Status")]
        public long Status { get; set; }

        [EntityPropertyExtension("StatusName", "StatusName")]
        public string StatusName { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime UpdateTime { get; set; }

        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }

        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        [EntityPropertyExtension("UserID", "UserID")]
        public long UserID { get; set; }
    }
}
