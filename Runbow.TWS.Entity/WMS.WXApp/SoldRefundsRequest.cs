using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 请求
    /// 获取售后列表（根据申请时间）
    /// 注意:status字段的说明，如果该字段不传，接口默认全部售后订单，非默认待同意申请的退货单查询不到。
    /// </summary>
    public class SoldRefundsRequest
    {
        /// <summary>
        /// 获取时间区间内退货订单的开始时间。格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime? start_created { get; set; }
        /// <summary>
        /// 获取时间区间内货订单的结束时间。格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime? end_created { get; set; }
        /// <summary>
        /// 交易状态（查看可选值）：WAIT_RETURN_BUYER_SEND_GOODS（待用户发货）、 WAIT_RETURN_SELLER_GOODS （用户已发货）、 TRADE_RETURNED_FINISHED（退款完成）、 TRADE_REFUND_REFUSED （拒绝退款） 默认查询所有交易状态的数据， 除了默认值外每次只能查询一种状态
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 页码。取值范围:大于零的整数; 默认值:1
        /// </summary>
        public Nullable<int> page_no { get; set; }
        /// <summary>
        /// 每页条数。取值范围:大于零的整数; 默认值:40;最大值:100
        /// </summary>
        public Nullable<int> page_size { get; set; }
    }
}
