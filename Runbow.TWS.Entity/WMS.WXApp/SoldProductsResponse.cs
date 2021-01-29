using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 反馈
    /// 获取当前商家的商品列表
    /// 获取当前商家的商品信息，可根据商品时间段或商品状态进行条件筛选
    /// </summary>
    public class SoldProductsResponse
    {
        public SoldProducts products_get_response { get; set; }
    }

    public class SoldProducts
    {
        /// <summary>
        /// 总条数
        /// </summary>
        public int total_results { get; set; }
        /// <summary>
        /// 返回商品具体的字段信息
        /// </summary>
        public List<SoldProduct> items { get; set; }
    }
    public class SoldProduct
    {
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
        /// 商品类型
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
        /// 商品创建时间
        /// </summary>
        public DateTime? list_time { get; set; }
        /// <summary>
        /// 商品修改时间
        /// </summary>
        public DateTime? modified { get; set; }
        /// <summary>
        /// 商品状态
        /// </summary>
        public string approve_status { get; set; }
        /// <summary>
        /// 销售数量
        /// </summary>
        public int sold_quantity { get; set; }
        /// <summary>
        /// 商品库存
        /// </summary>
        public int num { get; set; }
        /// <summary>
        /// 商品一口价
        /// </summary>
        public decimal price { get; set; }
    } 
}
