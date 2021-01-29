using System;
using Runbow.TWS.Common;


namespace Runbow.TWS.Entity.POD
{
    /// <summary>
    /// 全毅快递请求数据
    /// </summary>
    public class PodTrack_QY
    {
        /// <summary>
        /// 路由节点信息编号，每一个 id 代表一条不同的路由节点信息
        /// </summary>
        [EntityPropertyExtension("id", "id")]
        public int id { get; set; }

        /// <summary>
        /// 顺丰运单号
        /// </summary>
        [EntityPropertyExtension("mailno", "mailno")]
        public string mailno { get; set; }

        /// <summary>
        /// 客户订单号
        /// </summary>
        [EntityPropertyExtension("orderid", "orderid")]
        public string orderid { get; set; }

        /// <summary>
        /// 路由节点产生的时间
        /// </summary>
        [EntityPropertyExtension("acceptTime", "acceptTime")]
        public string acceptTime { get; set; }

        /// <summary>
        /// 路由节点发生的城市
        /// </summary>
        [EntityPropertyExtension("acceptAddress", "acceptAddress")]
        public string acceptAddress { get; set; }

        /// <summary>
        /// 路由节点具体描述
        /// </summary>
        [EntityPropertyExtension("remark", "remark")]
        public string remark { get; set; }

        /// <summary>
        /// 路由节点操作码
        /// </summary>
        [EntityPropertyExtension("opCode", "opCode")]
        public string opCode { get; set; }

        /// <summary>
        /// 快递公司
        /// </summary>
        [EntityPropertyExtension("courier", "courier")]
        public string courier { get; set; }

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
        
    }
}
