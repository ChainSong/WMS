using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class SegmentDetail
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("SegmentID", "SegmentID")]
        public long SegmentID { get; set; }

        [EntityPropertyExtension("StartVal", "StartVal")]
        public float StartVal { get; set; }

        [EntityPropertyExtension("EndVal", "EndVal")]
        public float EndVal { get; set; }

        [EntityPropertyExtension("Description", "Description")]
        public string Description { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }

        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }

        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }
    }
}