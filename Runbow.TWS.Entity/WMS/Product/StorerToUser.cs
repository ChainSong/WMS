using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Product
{
    public class StorerToUser
    {
        [EntityPropertyExtension("StorerID", "StorerID")]
        public long StorerID { get; set; }
        [EntityPropertyExtension("StorerName", "StorerName")]
        public string StorerName { get; set; }
        [EntityPropertyExtension("UserID", "UserID")]
        public long UserID { get; set; }
    }
}
