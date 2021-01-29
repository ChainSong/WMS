using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 反馈
    /// 获取单笔退货的详细信息 
    /// </summary>
    public class RefundDetailResponse
    {
        public RefundDetails refund_get_response { get; set; }
    }

    public class RefundDetails
    {
        /// <summary>
        /// 搜索到的交易信息列表息
        /// </summary>
        public RefundDetail refund { get; set; }
    }
    public class RefundDetail : SoldRefund
    {
        /// <summary>
        /// 物流公司
        /// </summary>
        public string expressCompanyName { get; set; }
        /// <summary>
        /// 物流编号
        /// </summary>
        public string expressCompanyAbb { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string adminShipTo { get; set; }
    }
}
