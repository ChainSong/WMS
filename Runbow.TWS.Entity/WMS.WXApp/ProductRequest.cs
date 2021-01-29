using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 请求
    /// 获取指定商品的详细信息
    /// </summary>
    public class ProductRequest
    {
        /// <summary>
        /// 必须
        /// 商品数字ID
        /// </summary>
        public int num_iid { get; set; }
    }
}
