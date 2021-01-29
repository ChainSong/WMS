using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class WXCustomer
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("WeixinName", "WeixinName")]
        public string WeixinName { get; set; }

        [EntityPropertyExtension("RealName", "RealName")]
        public string RealName { get; set; }

        [EntityPropertyExtension("Phone", "Phone")]
        public string Phone { get; set; }

        [EntityPropertyExtension("unitName", "unitName")]
        public string UnitName { get; set; }

        [EntityPropertyExtension("Status", "Status")]
        public bool? Status { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }        
    }
}
