using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    public class LogisticResponse
    {
        /// <summary>
        /// 发货订单结果
        /// </summary>
        public Shiping shiping { get; set; }
    }
    public class Shiping {
        /// <summary>
        /// 发货成功与否 true
        /// </summary>
        public bool is_success { get; set; }
    }
}
