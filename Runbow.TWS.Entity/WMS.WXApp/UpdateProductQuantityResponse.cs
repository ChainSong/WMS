using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    public class UpdateProductQuantityResponse
    {
        /// <summary>
        /// 返回商品的详细字段信息
        /// </summary>
        public items item { get; set; }
    }
    public class items {
        /// <summary>
        /// 商品分类编号
        /// </summary>
        public int cid { get; set; }
        /// <summary>
        /// 商品类目名称
        /// </summary>
        public string cat_name { get; set; }
        /// <summary>
        /// 品牌编号
        /// </summary>
        public int brand_id { get; set; }
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string brand_name { get; set; }
        /// <summary>
        /// 商品类型
        /// </summary>
        public int type_id { get; set; }
        /// <summary>
        /// 商品类型名称
        /// </summary>
        public string type_name { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int num_iid { get; set; }
        /// <summary>
        /// 商品货号
        /// </summary>
        public string outer_id { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 商品图片主图，存在多图就以数组形式返回，中间用逗号隔开
        /// </summary>
        public List<string> pic_url { get; set; }
        /// <summary>
        /// pc端的商品详情
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 移动端的商品详情
        /// </summary>
        public string wap_desc { get; set; }
        /// <summary>
        /// 商品创建时间
        /// </summary>
        public DateTime list_time { get; set; }
        /// <summary>
        /// 商品修改时间
        /// </summary>
        public DateTime modified { get; set; }
        /// <summary>
        /// 商品排序号
        /// </summary>
        public int display_sequence { get; set; }
        /// <summary>
        /// 商品状态
        /// </summary>
        public string approve_status { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public int sold_quantity { get; set; }
        /// <summary>
        /// 商品扩展属性，(注：属性名称中的冒号':'被转换为：'#cln#'; 分号';'被转换为：'#scln#' )
        /// </summary>
        public int props_name { get; set; }
        /// <summary>
        /// 商品库存
        /// </summary>
        public int sub_stock { get; set; }
        /// <summary>
        /// 商品其他规格型号
        /// </summary>
        public Skus skus { get; set; }
        
    }
    public class Skus {
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
        public int sku_properties_name { get; set; }
    }
}
