using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.POD
{
    public class BAFPriceInfo
    {

        [EntityPropertyExtension("ID", "ID")]
        public int  ID { get; set; }
        [EntityPropertyExtension("ProjectID", "ProjectID")]
        public int ProjectID { get; set; }
        [EntityPropertyExtension("ProjectName", "ProjectName")]
        public string ProjectName { get; set; }
        [EntityPropertyExtension("TragetID", "TragetID")]
        public int TragetID { get; set; }
        [EntityPropertyExtension("TragetName", "TragetName")]
        public string TragetName { get; set; }
        [EntityPropertyExtension("BAFPrice", "BAFPrice")]
        public decimal BAFPrice { get; set; }
        [EntityPropertyExtension("BAFStartTime", "BAFStartTime")]
        public DateTime BAFStartTime { get; set; }
        [EntityPropertyExtension("BAFEndTime", "BAFEndTime")]
        public DateTime BAFEndTime { get; set; }
        [EntityPropertyExtension("Start", "Start")]
        public int Start { get; set; }
        [EntityPropertyExtension("str1", "str1")]
        public int str1 { get; set; }

    }
}
