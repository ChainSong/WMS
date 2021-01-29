using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.Warehouse
{
    /// <summary>
    /// 用户操作库位
    /// </summary>
    public class WMS_User_Area_Mapping
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("UserID", "UserID")]
        public long UserID { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }

        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }

        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }

        [EntityPropertyExtension("AreaID", "AreaID")]
        public long? AreaID { get; set; }

        [EntityPropertyExtension("AreaName", "AreaName")]
        public string AreaName { get; set; }

        [EntityPropertyExtension("Status", "Status")]
        public string Status { get; set; }

        [EntityPropertyExtension("StorerKey", "StorerKey")]
        public string StorerKey { get; set; }

        [EntityPropertyExtension("Company", "Company")]
        public string Company { get; set; }

        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }

        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }
        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }
        [EntityPropertyExtension("Str4", "Str4")]
        public string Str4 { get; set; }
        [EntityPropertyExtension("Str5", "Str5")]
        public string Str5 { get; set; }

        [EntityPropertyExtension("Int1", "Int1")]
        public long Int1 { get; set; }
        [EntityPropertyExtension("Int2", "Int2")]
        public long Int2 { get; set; }
        [EntityPropertyExtension("Int3", "Int3")]
        public long Int3 { get; set; }
        [EntityPropertyExtension("Int4", "Int4")]
        public long Int4 { get; set; }
        [EntityPropertyExtension("Int5", "Int5")]
        public long Int5 { get; set; }

        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime DateTime1 { get; set; }
    }
}
