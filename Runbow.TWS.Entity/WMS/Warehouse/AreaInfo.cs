using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 区域信息
    /// </summary>
    public class AreaInfo
    {

        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }

        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }

        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }

        /// <summary>
        /// 库区名称
        /// </summary>
        [EntityPropertyExtension("AreaName", "AreaName")]
        public string AreaName { get; set; }


        /// <summary>
        /// 状态
        /// </summary>
        [EntityPropertyExtension("Status", "Status")]
        public string Status { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [EntityPropertyExtension("Type", "Type")]
        public int Type { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 库位List
        /// </summary>
        public IEnumerable<LocationInfo> Locations { get; set; }

        /// <summary>
        /// 库区类别
        /// </summary>
        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }
    }
}
