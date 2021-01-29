using Runbow.TWS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 反馈
    /// 当前商家的订单列表（根据创建时间）
    /// 返回的数据结果是以订单的创建时间倒序排列的
    /// </summary>
    public class SoldTradesResponse
    {
        public SoldTrades trades_sold_get_response { get; set; }
    }

    public class SoldTrades
    {
        /// <summary>
        /// 搜索到的交易信息总数
        /// </summary>
        public int total_results { get; set; }
        /// <summary>
        /// 是否存在下一页
        /// </summary>
        public bool has_next { get; set; }
        /// <summary>
        /// 搜索到的交易信息列表，返回的Trade和Order中包含的字段信息
        /// </summary>
        public List<SoldTrade> trades { get; set; }
    }

    public class SoldTrade
    {
        /// <summary>
        /// 交易编号
        /// </summary>
        [EntityPropertyExtension("tid", "tid")]
        public string tid { get; set; }
        /// <summary>
        /// 买家备注
        /// </summary>
        [EntityPropertyExtension("buyer_memo", "buyer_memo")]
        public string buyer_memo { get; set; }
        /// <summary>
        /// 卖家备注
        /// </summary>
        [EntityPropertyExtension("seller_memo", "seller_memo")]
        public string seller_memo { get; set; }
        /// <summary>
        /// 卖家标记
        /// </summary>
        [EntityPropertyExtension("seller_flag", "seller_flag")]
        public string seller_flag { get; set; }
        /// <summary>
        /// 订单折扣
        /// </summary>
        [EntityPropertyExtension("discount_fee", "discount_fee")]
        public decimal discount_fee { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        [EntityPropertyExtension("status", "status")]
        public string status { get; set; }
        /// <summary>
        /// 关闭理由
        /// </summary>
        [EntityPropertyExtension("close_memo", "close_memo")]
        public string close_memo { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        [EntityPropertyExtension("created", "created")]
        public DateTime? created { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [EntityPropertyExtension("modified", "modified")]
        public DateTime? modified { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        [EntityPropertyExtension("pay_time", "pay_time")]
        public DateTime? pay_time { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        [EntityPropertyExtension("consign_time", "consign_time")]
        public DateTime? consign_time { get; set; }
        /// <summary>
        /// 订单完成时间
        /// </summary>
        [EntityPropertyExtension("end_time", "end_time")]
        public string end_time { get; set; }
        /// <summary>
        /// 下单人用户名
        /// </summary>
        [EntityPropertyExtension("buyer_uname", "buyer_uname")]
        public string buyer_uname { get; set; }
        /// <summary>
        /// 下单人邮箱
        /// </summary>
        [EntityPropertyExtension("buyer_email", "buyer_email")]
        public string buyer_email { get; set; }
        /// <summary>
        /// 买家昵称
        /// </summary>
        [EntityPropertyExtension("buyer_nick", "buyer_nick")]
        public string buyer_nick { get; set; }
        /// <summary>
        /// 下单人买家地区
        /// </summary>
        [EntityPropertyExtension("buyer_area", "buyer_area")]
        public string buyer_area { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        [EntityPropertyExtension("receiver_name", "receiver_name")]
        public string receiver_name { get; set; }
        /// <summary>
        /// 收货地址所在省
        /// </summary>
        [EntityPropertyExtension("receiver_state", "receiver_state")]
        public string receiver_state { get; set; }
        /// <summary>
        /// 收货地址所在市
        /// </summary>
        [EntityPropertyExtension("receiver_city", "receiver_city")]
        public string receiver_city { get; set; }
        /// <summary>
        /// 收货地址所在区
        /// </summary>
        [EntityPropertyExtension("receiver_district", "receiver_district")]
        public string receiver_district { get; set; }
        /// <summary>
        /// 收货地址所在的城镇街道
        /// </summary>
        [EntityPropertyExtension("receiver_town", "receiver_town")]
        public string receiver_town { get; set; }
        /// <summary>
        /// 收货详细地址
        /// </summary>
        [EntityPropertyExtension("receiver_address", "receiver_address")]
        public string receiver_address { get; set; }
        /// <summary>
        /// 收货地区邮编
        /// </summary>
        [EntityPropertyExtension("receiver_zip", "receiver_zip")]
        public string receiver_zip { get; set; }
        /// <summary>
        /// 收货联系人手机
        /// </summary>
        [EntityPropertyExtension("receiver_mobile", "receiver_mobile")]
        public string receiver_mobile { get; set; }
        /// <summary>
        /// 发票金额
        /// </summary>
        [EntityPropertyExtension("invoice_fee", "invoice_fee")]
        public decimal invoice_fee { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        [EntityPropertyExtension("invoice_title", "invoice_title")]
        public string invoice_title { get; set; }
        /// <summary>
        /// 实付金额。精确到2位小数;单位:元。如:200.07，表示:200元7分
        /// </summary>
        [EntityPropertyExtension("payment", "payment")]
        public string payment { get; set; }
        /// <summary>
        /// 门店编号，默认返回0
        /// </summary>
        [EntityPropertyExtension("storeId", "storeId")]
        public int? storeId { get; set; }
        /// <summary>
        /// 子订单列表
        /// </summary>
        public List<SoldTradeOrder> orders { get; set; }
    }

    public class SoldTradeOrder
    {
        /// <summary>
        /// 交易编号
        /// </summary>
        public string tid { get; set; }
        /// <summary>
        /// 商品规格号
        /// </summary>
        public string sku_id { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string num_id { get; set; }
        /// <summary>
        /// 商品货号
        /// </summary>
        public string outer_sku_id { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 规格属性名称
        /// </summary>
        public string sku_properties_name { get; set; }
        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int num { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string pic_path { get; set; }
        /// <summary>
        /// 商品状态
        /// </summary>
        public string refund_status { get; set; }
    }
}
