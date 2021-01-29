using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 商品/SKU库存修改(提供按照全量或增量形式修改宝贝/SKU库存的功能)
    /// </summary>
    public class UpdateProductQuantityRequest
    {
        /// <summary>
        /// 商品数字ID 必须
        /// </summary>
        public int num_iid { get; set; }
        /// <summary>
        /// 规格唯一编号。如果不填默认修改商品所有库存，如果填上则修改该SKU的库存 可选
        /// </summary>
        public string sku_id { get; set; }
        /// <summary>
        /// 库存修改值，必选。当全量更新库存时，quantity必须为大于等于0的正整数；当增量更新库存时，quantity为整数，可小于等于0。若增量更新时传入的库存为负数，则负数与实际库存之和不能小于0。比如当前实际库存为1，传入增量更新quantity=-1，库存改为0
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 库存更新方式，可选。1为全量更新，2为增量更新。如果不填，默认为全量更新。
        /// </summary>
        public int? type { get; set; }
    }
}
