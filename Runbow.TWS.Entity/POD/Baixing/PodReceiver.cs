using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.POD
{
    
    public class PodReceiver
    {
        /// <summary>
        /// 用户地址信息
        /// </summary>
        public PodAddress address { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [EntityPropertyExtension("contact", "contact")]
        public string contact { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [EntityPropertyExtension("tel", "tel")]
        public string tel { get; set; }
    }
}
