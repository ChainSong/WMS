using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.MessageContracts
{
    public class JCAPiResponse
    {
        /// <summary>
        /// 回传状态
        /// </summary>
        public string flag { set; get; }
        /// <summary>
        /// 响应码
        /// </summary>
        public string code { set; get; }
        /// <summary>
        /// 响应信息
        /// </summary>
        public string message { set; get; }
        /// <summary>
        /// 单号
        /// </summary>
        public string relatednumber { get; set; }
    }
}
