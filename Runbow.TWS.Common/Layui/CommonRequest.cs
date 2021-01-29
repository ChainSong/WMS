using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Common.Layui
{
    public class CommonRequest
    {
        /// <summary>
        /// 标记属性(新增||修改...)
        /// </summary>
        public string IsSign { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 每页显示的条数
        /// </summary>
        public int limit { get; set; }

        /// <summary>
        /// 扩展参数
        /// </summary>
        public long anyID { get; set; }

        /// <summary>
        /// 对象字符串
        /// </summary>
        public string requestData { get; set; }

        /// <summary>
        /// 对象字符串
        /// </summary>
        public string requestData2 { get; set; }

        /// <summary>
        /// 对象字符串
        /// </summary>
        public string requestData3 { get; set; }

        /// <summary>
        /// 对象字符串
        /// </summary>
        public string requestData4 { get; set; }

        /// <summary>
        /// 对象字符串
        /// </summary>
        public string requestData5 { get; set; }
    }
}
