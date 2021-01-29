using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    /// <summary>
    /// 根据ID获取仓库的所有信息
    /// </summary>
    public class GetWarehouseByIDResponse
    {
        /// <summary>
        /// 仓库信息
        /// </summary>
        public WarehouseInfo Warehouse { get; set; }
        /// <summary>
        /// 库区List
        /// </summary>
        public IEnumerable<AreaInfo> Areas { get; set; }
        /// <summary>
        /// 库位List
        /// </summary>
        public IEnumerable<LocationInfo> Locations { get; set; }
    }
}
