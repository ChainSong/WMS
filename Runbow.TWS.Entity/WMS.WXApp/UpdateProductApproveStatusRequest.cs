using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 修改商品销售状态 (上架， 下架， 入库)
    /// </summary>
    public class UpdateProductApproveStatusRequest
    {
        /// <summary>
        /// 商品数字ID 必须
        /// </summary>
        public int num_iid { get; set; }
        /// <summary>
        /// 商品状态（On_Sale）出售中/（Un_Sale）下架区/(In_Stock)仓库中,查看商品状态 必须
        /// </summary>
        public string approve_status { get; set; }
    }
}
