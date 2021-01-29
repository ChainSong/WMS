using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.WorkOrder
{
    public class WorkOrderFile
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("WOID", "WOID")]
        public long WOID { get; set; }

        [EntityPropertyExtension("FilePath", "FilePath")]
        public string FilePath { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("FileType", "FileType")]
        public long FileType { get; set; }

        [EntityPropertyExtension("FileTypeName", "FileTypeName")]
        public string FileTypeName { get; set; }

        [EntityPropertyExtension("ProcessID", "ProcessID")]
        public long ProcessID { get; set; }
    }
}
