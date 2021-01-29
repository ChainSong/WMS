using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 请求
    /// 获取单笔退货的详细信息 
    /// </summary>
    public class RefundDetailRequest
    {
        /// <summary>
        /// 必须
        /// 交易编号
        /// </summary>
        public string tid { get; set; }
    }
}
