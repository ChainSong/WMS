using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 请求
    /// 当前商家的订单列表（根据创建时间）
    /// </summary>
    public class SoldTradesRequest
    {
        /// <summary>
        /// 获取时间区间内订单的开始时间。格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime? start_created { get; set; }
        /// <summary>
        /// 获取时间区间内订单的结束时间。格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime? end_created { get; set; }
        /// <summary>
        /// 交易状态（查看可选值）：WAIT_BUYER_PAY（等待买家付款）、 WAIT_SELLER_SEND_GOODS （等待商家发货）、 WAIT_BUYER_CONFIRM_GOODS（等待买家确认收货）、 TRADE_CLOSED （交易关闭）、TRADE_FINISHED（交易成功） 默认查询所有交易状态的数据， 除了默认值外每次只能查询一种状态
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 买家帐号
        /// </summary>
        public string buyer_uname { get; set; }
        /// <summary>
        /// 页码。取值范围:大于零的整数; 默认值:1
        /// </summary>
        public int? page_no { get; set; }
        /// <summary>
        /// 每页条数。取值范围:大于零的整数; 默认值:40;最大值:100
        /// </summary>
        public int? page_size { get; set; }

    }
}
