using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class DMS_Province
    {
        [EntityPropertyExtension("colCode", "colCode")]
        public string colCode { get; set; }
        [EntityPropertyExtension("colName", "colName")]
        public string colName { get; set; }
        [EntityPropertyExtension("colDesc", "colDesc")]
        public string colDesc { get; set; }
    }
}
