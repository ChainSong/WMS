using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runbow.TWS.Entity.WMS.WXApp
{
    /// <summary>
    /// 请求
    /// 获取当前商家的商品列表
    /// 获取当前商家的商品信息，可根据商品时间段或商品状态进行条件筛选
    /// </summary>
    public class SoldProductsRequest
    {
        /// <summary>
        /// 起始的修改时间
        /// </summary>
        public DateTime? start_modified { get; set; }
        /// <summary>
        /// 结束的修改时间
        /// </summary>
        public DateTime? end_modified { get; set; }
        /// <summary>
        /// 商品状态On_Sale(出售中)/(Un_Sale)下架区/(In_Stock)仓库中 默认查询所有状态的数据，除了默认值外每次只能查询一种状态
        /// </summary>
        public string approve_status { get; set; }
        /// <summary>
        /// 搜索关键字，可搜索商品的名称
        /// </summary>
        public string q { get; set; }
        /// <summary>
        /// 排序方式。
        /// 格式为column:asc/desc 
        /// column可选值:display_sequence（默认顺序） create_time(创建时间),sold_quantity（商品销量）;
        /// 默认商品排序编号升序(diplay_sequence值越小在前)。
        /// 如按照商品排序编号升序排序方式为display_sequence:asc
        /// </summary>
        public string order_by { get; set; }
        /// <summary>
        /// 页码。取值范围:大于零的整数; 默认值:1
        /// </summary>
        public Nullable<int> page_no { get; set; }
        /// <summary>
        /// 每页条数。取值范围:大于零的整数; 默认值:40;最大值:100
        /// </summary>
        public Nullable<int> page_size { get; set; }
    }
}
