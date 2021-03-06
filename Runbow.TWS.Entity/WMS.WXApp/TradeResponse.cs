﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 反馈
    /// 获取单笔交易的详细信息 
    /// </summary>
    public class TradeResponse
    {
        public Trades trade_get_response { get; set; }
    }

    public class Trades
    {
        public Trade trade { get; set; }
    }

    public class Trade
    {
        /// <summary>
        /// 交易编号
        /// </summary>
        public string tid { get; set; }
        /// <summary>
        /// 买家备注
        /// </summary>
        public string buyer_memo { get; set; }
        /// <summary>
        /// 卖家备注
        /// </summary>
        public string seller_memo { get; set; }
        /// <summary>
        /// 卖家标记
        /// </summary>
        public string seller_flag { get; set; }
        /// <summary>
        /// 订单折扣
        /// </summary>
        public decimal discount_fee { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 关闭理由
        /// </summary>
        public string close_memo { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime created { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime modified { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime pay_time { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime consign_time { get; set; }
        /// <summary>
        /// 订单完成时间
        /// </summary>
        public string end_time { get; set; }
        /// <summary>
        /// 下单人用户名
        /// </summary>
        public string buyer_uname { get; set; }
        /// <summary>
        /// 下单人邮箱
        /// </summary>
        public string buyer_email { get; set; }
        /// <summary>
        /// 买家昵称
        /// </summary>
        public string buyer_nick { get; set; }
        /// <summary>
        /// 下单人买家地区
        /// </summary>
        public string buyer_area { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string receiver_name { get; set; }
        /// <summary>
        /// 收货地址所在省
        /// </summary>
        public string receiver_state { get; set; }
        /// <summary>
        /// 收货地址所在市
        /// </summary>
        public string receiver_city { get; set; }
        /// <summary>
        /// 收货地址所在区
        /// </summary>
        public string receiver_district { get; set; }
        /// <summary>
        /// 收货地址所在的城镇街道
        /// </summary>
        public string receiver_town { get; set; }
        /// <summary>
        /// 收货详细地址
        /// </summary>
        public string receiver_address { get; set; }
        /// <summary>
        /// 收货地区邮编
        /// </summary>
        public string receiver_zip { get; set; }
        /// <summary>
        /// 收货联系人手机
        /// </summary>
        public string receiver_mobile { get; set; }
        /// <summary>
        /// 发票金额
        /// </summary>
        public decimal invoice_fee { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string invoice_title { get; set; }
        /// <summary>
        /// 实付金额。精确到2位小数;单位:元。如:200.07，表示:200元7分
        /// </summary>
        public string payment { get; set; }

        public List<TradeOrder> orders { get; set; }
    }

    public class TradeOrder
    {
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
