using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    [Serializable]
    public class PodReplyDocumentWithAttachment
    {
        [EntityPropertyExtension("PodID", "PodID")]
        public long PodID { get; set; }

        [EntityPropertyExtension("SystemNumber", "SystemNumber")]
        public string SystemNumber { get; set; }

        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }

        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        [EntityPropertyExtension("Replier", "Replier")]
        public string Replier { get; set; }

        [EntityPropertyExtension("ReplyTime", "ReplyTime")]
        public DateTime? ReplyTime { get; set; }

        [EntityPropertyExtension("AttachmentGroupID", "AttachmentGroupID")]
        public string AttachmentGroupID { get; set; }

        [EntityPropertyExtension("IsAudit", "IsAudit")]
        public bool? IsAudit { get; set; }

        [EntityPropertyExtension("AttachmentID", "AttachmentID")]
        public long AttachmentID { get; set; }

        [EntityPropertyExtension("DisplayName", "DisplayName")]
        public string DisplayName { get; set; }

        [EntityPropertyExtension("CreateDate", "CreateDate")]
        public DateTime? CreateDate { get; set; }

        [EntityPropertyExtension("Remark", "Remark")]
        public String Remark { get; set; }

        [EntityPropertyExtension("Url", "Url")]
        public string Url { get; set; }

        [EntityPropertyExtension("Warehouse", "Warehouse")]
        public string Warehouse { get; set; }
    }
}
