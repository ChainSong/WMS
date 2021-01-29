using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    public class WMS_Task
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }
        [EntityPropertyExtension("TaskID", "TaskID")]
        public long TaskID { get; set; }
        [EntityPropertyExtension("TaskStatus", "TaskStatus")]
        public int? TaskStatus { get; set; }
        [EntityPropertyExtension("TaskNumber", "TaskNumber")]
        public string TaskNumber { get; set; }
        [EntityPropertyExtension("TaskType", "TaskType")]
        public string TaskType { get; set; }
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        
    }
}
