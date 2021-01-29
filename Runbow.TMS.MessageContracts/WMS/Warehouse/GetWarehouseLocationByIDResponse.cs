using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class GetWarehouseLocationByIDResponse
    {

        /// <summary>
        /// 仓库
        /// </summary>
        public WarehouseInfo Warehouse { get; set; }


        /// <summary>
        /// 库区
        /// </summary>
        public AreaInfo Area { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public LocationInfo Location { get; set; }


        public GoodsShelfInfo GoodsShelf { get; set; }
    }
}
