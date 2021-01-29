using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 反馈
    /// 获取指定商品的详细信息
    /// </summary>
    public class ProductResponse
    {
        public Products product_get_response { get; set; }
    }

    public class Products
    {
        public Product item { get; set; }
    }

    public class Product:SoldProduct
    {
        /// <summary>
        /// pc端的商品详情
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 移动端的商品详情
        /// </summary>
        public string wap_desc { get; set; }
        /// <summary>
        /// 商品排序号
        /// </summary>
        public int display_sequence { get; set; }
        /// <summary>
        /// 商品扩展属性，(注：属性名称中的冒号':'被转换为：'#cln#'; 分号';'被转换为：'#scln#' )
        /// </summary>
        public string props_name { get; set; }
        /// <summary>
        /// 商品库存
        /// </summary>
        public int sub_stock { get; set; }
        /// <summary>
        /// 商品其他规格型号
        /// </summary>
        public List<ProductSku> skus { get; set; }
    }
    public class ProductSku
    {
        /// <summary>
        /// 商品编号
        /// </summary>
        public int num_iid { get; set; }
        /// <summary>
        /// 商品规格号
        /// </summary>
        public string sku_id { get; set; }
        /// <summary>
        /// 商品货号
        /// </summary>
        public string outer_sku_id { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        /// 商品单价
        /// </summary>
        public decimal price { get; set; }
        /// <summary>
        /// 商品规格名称
        /// </summary>
        public string sku_properties_name { get; set; }
    }
}
