using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.POD
{
  
    /// <summary>
    /// 用户地址信息
    /// </summary>
    public class PodAddress
    {
        /// <summary>
        /// 省
        /// </summary>
        [EntityPropertyExtension("province", "province")]
        public string province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        [EntityPropertyExtension("city", "city")]
        public string city { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        [EntityPropertyExtension("district", "district")]
        public string district { get; set; }

        //[EntityPropertyExtension("street", "street")]
        //public string street { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [EntityPropertyExtension("detail", "detail")]
        public string detail { get; set; }
    }
}
