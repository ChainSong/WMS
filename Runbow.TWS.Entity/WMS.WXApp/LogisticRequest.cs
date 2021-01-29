using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 订单发货
    /// </summary>
    public class LogisticRequest
    {
        /// <summary>
        /// 交易编号 必须
        /// </summary>
        public string tid { get; set; }
        /// <summary>
        /// 物流公司名称 必须
        /// </summary>
        public string company_name { get; set; }
        /// <summary>
        /// 运单号 必须
        /// </summary>
        public string out_sid { get; set; }
    }
}
