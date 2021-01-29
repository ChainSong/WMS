using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Order
{
    /// <summary>
    /// 拣货信息
    /// </summary>
    public class OrderPickDetail
    {
        [EntityPropertyExtension("OID", "OID")]
        public long OID { get; set; }

        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        [EntityPropertyExtension("QTY", "QTY")]
        public decimal QTY { get; set; }

        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }
        [EntityPropertyExtension("Int1", "Int1")]
        public int Int1 { get; set; }
    }
}
