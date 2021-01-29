using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Product
{
    public class ProductSearch
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public string CustomerID { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        /// <summary>
        /// UPC
        /// </summary>
        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [EntityPropertyExtension("GoodsName", "GoodsName")]
        public string GoodsName { get; set; }

        /// <summary>
        /// 价格Str2
        /// </summary>
        [EntityPropertyExtension("Price", "Price")]
        public string Price { get; set; }

        /// <summary>
        /// 安全扣Str3
        /// </summary>
        [EntityPropertyExtension("SafeLock", "SafeLock")]
        public string SafeLock { get; set; }

        /// <summary>
        /// 是否羽绒服Str4
        /// </summary>
        [EntityPropertyExtension("DownCoat", "DownCoat")]
        public string DownCoat { get; set; }

        /// <summary>
        /// 是否过期
        /// </summary>
        [EntityPropertyExtension("Str8", "Str8")]
        public string Str8 { get; set; }

        /// <summary>
        /// 尺寸
        /// </summary>
        [EntityPropertyExtension("Str9", "Str9")]
        public string Str9 { get; set; }

        /// <summary>
        /// 款号
        /// </summary>
        [EntityPropertyExtension("Str10", "Str10")]
        public string Str10 { get; set; }

        /// <summary>
        /// 10 APP 20 40 FTW 30 EQP
        /// </summary>
        [EntityPropertyExtension("Division", "Division")]
        public string Division { get; set; }

        /// <summary>
        /// 18 MENS 22 WOMENS
        /// </summary>
        [EntityPropertyExtension("GenderAge", "GenderAge")]
        public string GenderAge { get; set; }

        /// <summary>
        /// 衣架
        /// </summary>
        [EntityPropertyExtension("Hanger", "Hanger")]
        public string Hanger { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [EntityPropertyExtension("LongMaterial", "LongMaterial")]
        public string LongMaterial { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        [EntityPropertyExtension("categoryDes", "categoryDes")]
        public string categoryDes { get; set; }
    }
}
