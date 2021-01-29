using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.POD
{


    /// <summary>
    /// 百姓网推送主订单信息
    /// </summary>
  
    public class PodOrder
    {
        ///// <summary>
        ///// 信息编号
        ///// </summary>
        //[EntityPropertyExtension("ID", "ID")]
        //public long ID { get; set; }

        ///// <summary>
        ///// 系统单号
        ///// </summary>
        //[EntityPropertyExtension("SystemNumber", "SystemNumber")]
        //public string SystemNumber { get; set; }

        /// <summary>
        /// 百姓订单号
        /// </summary>
        [EntityPropertyExtension("order_id", "order_id")]
        public string order_id { get; set; }

        /// <summary>
        /// 商品品名
        /// </summary>
        [EntityPropertyExtension("commodity", "commodity")]
        public string commodity { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        [EntityPropertyExtension("weight", "weight")]
        public string weight { get; set; }

        /// <summary>
        /// 体积
        /// </summary>
        [EntityPropertyExtension("volume", "volume")]
        public string volume { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        [EntityPropertyExtension("price", "price")]
        public string price { get; set; }

        /// <summary>
        /// 投保
        /// </summary>
        [EntityPropertyExtension("insurance", "insurance")]
        public string insurance { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        [EntityPropertyExtension("memo", "memo")]
        public string memo { get; set; }

        /// <summary>
        /// 快递运单号
        /// </summary>
        [EntityPropertyExtension("bill_id", "bill_id")]
        public string bill_id { get; set; }

        /// <summary>
        /// 快递公司
        /// </summary>
        [EntityPropertyExtension("courier", "courier")]
        public string courier { get; set; }

        /// <summary>
        /// 预估快递费用
        /// </summary>
        [EntityPropertyExtension("fee", "fee")]
        public string fee { get; set; }

        /// <summary>
        /// 数据来源
        /// </summary>
        /// [EntityPropertyExtension("sourceApp", "sourceApp")]
        public string sourceApp { get; set; }

        /// <summary>
        /// 发货人
        /// </summary>

        public PodSender sender { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public PodReceiver receiver { get; set; }

        ///// <summary>
        ///// 用户验签
        ///// </summary>
        //public LoginUser userInfo { get; set; }
    }
}
