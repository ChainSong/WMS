using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.ForecastWarehouse
{
    public class UserCode
    {
        [EntityPropertyExtension("ID", "ID")]
        public Int64 ID { get; set; }

        [EntityPropertyExtension("User", "User")]
        public string User { get; set; }

        [EntityPropertyExtension("StoreCode", "StoreCode")]
        public string StoreCode { get; set; }

        [EntityPropertyExtension("StoreName", "StoreName")]
        public string StoreName { get; set; }

        [EntityPropertyExtension("City", "City")]
        public string City { get; set; }

        [EntityPropertyExtension("Brand", "Brand")]
        public string Brand { get; set; }

        [EntityPropertyExtension("CreatTime", "CreatTime")]
        public DateTime CreatTime { get; set; }

    }
}
