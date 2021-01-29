using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.NIKECE.JWT
{
    public class JWTHeader
    {
        /// <summary>
        /// 协议类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 签名算法
        /// </summary>
        public string alg { get; set; }
    }
}
