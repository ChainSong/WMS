using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.POD
{
    /// <summary>
    /// 快递路由信息
    /// </summary>
    public class PodRaw
    {
        /// <summary>
        /// 路由节点信息编号，每一个 id 代表一条不同的路由节点信息
        /// </summary>
        [EntityPropertyExtension("id", "id")]
        public int id { get; set; }

        /// <summary>
        /// 快递运单号
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

        ///// <summary>
        ///// 用户验签
        ///// </summary>
        //public LoginUser userInfo { get; set; }
    }
}
