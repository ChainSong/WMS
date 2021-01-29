using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.MobilePOD
{
    public class PodStatusTrack
    {
        [EntityPropertyExtension("PodID", "PodID")]
        public string PodID { get; set; }
        [EntityPropertyExtension("SystemNumber", "SystemNumber")]
        public string SystemNumber { get; set; }
        [EntityPropertyExtension("CustomerOrderNumber", "CustomerOrderNumber")]
        public string CustomerOrderNumber { get; set; }
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public string CreateTime { get; set; }
        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }
        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }
        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }
        [EntityPropertyExtension("Str5", "Str5")]
        public string Str5 { get; set; }
        [EntityPropertyExtension("Str6", "Str6")]
        public string Str6 { get; set; }
        [EntityPropertyExtension("Str7", "Str7")]
        public string Str7 { get; set; }
        [EntityPropertyExtension("Str8", "Str8")]
        public string Str8 { get; set; }
        [EntityPropertyExtension("Str9", "Str9")]
        public string Str9 { get; set; }
        [EntityPropertyExtension("Str10", "Str10")]
        public string Str10 { get; set; }
        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public string DateTime1 { get; set; }
        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public string DateTime2 { get; set; }
        [EntityPropertyExtension("DateTime3", "DateTime3")]
        public string DateTime3 { get; set; }

    }
}
