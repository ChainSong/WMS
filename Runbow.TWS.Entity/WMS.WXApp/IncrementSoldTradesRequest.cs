using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 请求
    /// 查询订单的增量交易数据（根据修改时间） 
    /// 一次请求只能查询时间跨度为一天的增量交易记录，即end_modified - start_modified小于等于1天。
    /// </summary>
    public class IncrementSoldTradesRequest
    {
        /// <summary>
        /// 必须
        /// 查询修改开始时间(修改时间跨度不能大于一天)。格式:yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime start_modified { get; set; }
        /// <summary>
        /// 必须
        /// 查询修改结束时间，必须大于修改开始时间(修改时间跨度不能大于一天)，格式:yyyy-MM-dd HH:mm:ss。建议使用30分钟以内的时间跨度，能大大提高响应速度和成功率
        /// </summary>
        public DateTime end_modified { get; set; }
        /// <summary>
        /// 交易状态（查看可选值）：WAIT_BUYER_PAY（等待买家付款）、 WAIT_SELLER_SEND_GOODS （等待商家发货）、 WAIT_BUYER_CONFIRM_GOODS（等待买家确认收货）、TRADE_CLOSED （交易关闭）、TRADE_FINISHED（交易成功） 默认查询所有交易状态的数据，除了默认值外每次只能查询一种状态
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
        /// 每页条数。取值范围:大于零的整数; 默认值:40;最大值:100（建议使用40~50，可以提高成功率，减少超时数量）
        /// </summary>
        public int? page_size { get; set; }
    }
}
