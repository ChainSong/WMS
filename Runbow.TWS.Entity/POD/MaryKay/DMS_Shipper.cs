using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class DMS_Shipper
    {
        [EntityPropertyExtension("ID", "ID")]
        public int ID { get; set; }
        [EntityPropertyExtension("colCode", "colCode")]
        public string colCode { get; set; }
        [EntityPropertyExtension("colName", "colName")]
        public string colName { get; set; }
        [EntityPropertyExtension("colEnglishName", "colEnglishName")]
        public string colEnglishName { get; set; }
    }
}
