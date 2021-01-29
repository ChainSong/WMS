using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 反馈
    /// 获取售后列表（根据申请时间）
    /// 返回的数据结果是以申请的时间倒序排列的
    /// </summary>
    public class SoldRefundsResponse
    {
        public SoldRefunds refunds_get_response { get; set; }
    }

    public class SoldRefunds
    {
        /// <summary>
        /// 搜索到的总数
        /// </summary>
        public int total_results { get; set; }
        /// <summary>
        /// 搜索到的信息列表
        /// </summary>
        public List<SoldRefund> refunds { get; set; }
    }

    public class SoldRefund
    {
        /// <summary>
        /// 退货ID
        /// </summary>
        public int returnId { get; set; }
        /// <summary>
        /// 交易编号
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 买家用户名
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 申请退货时间
        /// </summary>
        public DateTime? applyForTime { get; set; }
        /// <summary>
        /// 管理员备注
        /// </summary>
        public string adminRemark { get; set; }
        /// <summary>
        /// 用户备注
        /// </summary>
        public string userRemark { get; set; }
        /// <summary>
        /// 退货状态
        /// </summary>
        public string handleStatus { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string orderStatus { get; set; }
        /// <summary>
        /// 退货数量
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// SKU
        /// </summary>
        public string skuId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string productName { get; set; }
        /// <summary>
        /// 退货理由
        /// </summary>
        public string returnReason { get; set; }
        public string userCredentials { get; set; }
        /// <summary>
        /// 邮寄地址
        /// </summary>
        public string adminShipAddress { get; set; }
        /// <summary>
        /// 退货金额
        /// </summary>
        public decimal refundAmount { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? agreedOrRefusedTime { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? finishTime { get; set; }
        /// <summary>
        /// 用户快递编号
        /// </summary>
        public string shipOrderNumber { get; set; }
        /// <summary>
        /// 用户发货时间
        /// </summary>
        public DateTime? userSendGoodsTime { get; set; }
        public DateTime? handleTime { get; set; }
        /// <summary>
        /// 退货商品编码
        /// </summary>
        public string outerSkuId { get; set; }
    }
}
