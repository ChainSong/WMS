using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class AMSUpload
    {//ID,FileName,FileType,ServerName,FilePath,ProjectID,ProjectName,OrderNo,Creator,CreateTime,Updator,UpdateTime,Status
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("FileName", "FileName")]
        public string FileName { get; set; }

        [EntityPropertyExtension("FileType", "FileType")]
        public string FileType { get; set; }

        [EntityPropertyExtension("ServerName", "ServerName")]
        public string ServerName { get; set; }

        [EntityPropertyExtension("FilePath", "FilePath")]
        public string FilePath { get; set; }

        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public long ProjectID { get; set; }

        [EntityPropertyExtension("ProjectName", "ProjectName")]
        public string ProjectName { get; set; }

        [EntityPropertyExtension("OrderNo", "OrderNo")]
        public string OrderNo { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }

        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime UpdateTime { get; set; }

        [EntityPropertyExtension("Status", "Status")]
        public bool? Status { get; set; }
    }
}
