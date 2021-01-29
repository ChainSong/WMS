using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.OnlineOrder
{
    public class DeliveryOrderSearchCondition
    {
        /// <summary>
        /// 勾选时作为唯一条件
        /// </summary>
        public string UPC { get; set; }
        /// <summary>
        /// 季节
        /// </summary>
        public string Season { get; set; }
        /// <summary>
        /// 货码=SKU 系统里面是Article+Corol
        /// </summary>
        public string SKU { get; set; }
        /// <summary>
        /// 材料说明
        /// </summary>
        public string ModelName { get; set; }
        /// <summary>
        /// 尺码
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 类别(APP\EQP\FWL\FWR)
        /// </summary>
        public string PE { get; set; }
        /// <summary>
        /// 种类
        /// </summary>
        public string Category { get; set; }
    }
}
