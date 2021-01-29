using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.Reports
{
    public class WMSOrderStatusInfo
    {
        [EntityPropertyExtension("ID", "ID")]
        public int ID { get; set; }

        [EntityPropertyExtension("Project", "Project")]
        public string Project { get; set; }

        [EntityPropertyExtension("Num", "Num")]
        public float Num { get; set; }

        [EntityPropertyExtension("Allocated", "Allocated")]
        public float Allocated { get; set; }

        [EntityPropertyExtension("Picked", "Picked")]
        public float Picked { get; set; }

        [EntityPropertyExtension("Packaged", "Packaged")]
        public float Packaged { get; set; }

        [EntityPropertyExtension("RePicked", "RePicked")]
        public float RePicked { get; set; }

        [EntityPropertyExtension("Completed", "Completed")]
        public string Completed { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }

        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }

        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }

        [EntityPropertyExtension("float1", "float1")]
        public float float1 { get; set; }

        [EntityPropertyExtension("float2", "float2")]
        public float float2 { get; set; }


    }
}
