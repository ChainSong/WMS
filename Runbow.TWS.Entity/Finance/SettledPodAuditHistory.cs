using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class SettledPodAuditHistory
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("SettledPodID", "SettledPodID")]
        public long SettledPodID { get; set; }

        [EntityPropertyExtension("Auditor", "Auditor")]
        public string Auditor { get; set; }

        [EntityPropertyExtension("AuditTime", "AuditTime")]
        public DateTime AuditTime { get; set; }

        [EntityPropertyExtension("AuditRemark", "AuditRemark")]
        public string AuditRemark { get; set; }

        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }

        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }

        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime? DateTime1 { get; set; }

        [EntityPropertyExtension("Bit", "Bit")]
        public bool? Bit { get; set; }
    }
}
