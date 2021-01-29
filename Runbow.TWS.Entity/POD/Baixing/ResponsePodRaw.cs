using System;
using Runbow.TWS.Common;
using System.Collections.Generic;

namespace Runbow.TWS.Entity.POD
{
    /// <summary>
    /// 返回快递路由信息
    /// </summary>
    public class ResponsePodRaw
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        [EntityPropertyExtension("statusCode", "statusCode")]
        public string statuscode { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        [EntityPropertyExtension("Message", "Message")]
        public string message { get; set; }

        public List<PodRaw> podRaw { get; set; }
    }
}
