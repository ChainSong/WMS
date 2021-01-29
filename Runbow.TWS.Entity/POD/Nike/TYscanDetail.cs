using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class TYscanDetail
    {
        [EntityPropertyExtension("PODID", "PODID")]
        public long PODID { get; set; }

        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        [EntityPropertyExtension("LabelNo", "LabelNo")]
        public string LabelNo { get; set; }

        [EntityPropertyExtension("SHTor", "SHTor")]
        public string SHTor { get; set; }

        [EntityPropertyExtension("SHTime", "SHTime")]
        public string SHTime { get; set; }

        [EntityPropertyExtension("FHTor", "FHTor")]
        public string FHTor { get; set; }

        [EntityPropertyExtension("FHTime", "FHTime")]
        public string FHTime { get; set; }
    }
}
