using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts.POD
{
    /// <summary>
    /// 百姓网项目请求数据
    /// </summary>
    public class GetBaiXingRequest
    {
        /// <summary>
        /// 百姓订单号
        /// </summary>
        public string CustomerOrderNumber { get; set; }
    }
}
