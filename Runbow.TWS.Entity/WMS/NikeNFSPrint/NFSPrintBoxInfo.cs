using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.NikeNFSPrint
{
   public class NFSPrintBoxInfo
    {
       /// <summary>
       /// 订单号
       /// </summary>
       [EntityPropertyExtension("OrderNumber", "OrderNumber")]
       public string OrderNumber { get; set; }
       /// <summary>
       /// 货主NIKE
       /// </summary>
       [EntityPropertyExtension("ShipFrom", "ShipFrom")]
       public string ShipFrom { get; set; }
       /// <summary>
       /// 外部单号
       /// </summary>
       [EntityPropertyExtension("ShipmentNo", "ShipmentNo")]
       public string ShipmentNo { get; set; }
       /// <summary>
       /// 发往门店
       /// </summary>
       [EntityPropertyExtension("ShipToCode", "ShipToCode")]
       public string ShipToCode { get; set; }
       /// <summary>
       /// 发往门店名称
       /// </summary>
       [EntityPropertyExtension("ShipToName", "ShipToName")]
       public string ShipToName { get; set; }
       /// <summary>
       /// 发货时间
       /// </summary>
       [EntityPropertyExtension("DeliveryDate", "DeliveryDate")]
       public DateTime? DeliveryDate { get; set; }
       /// <summary>
       /// 尺码
       /// </summary>
       [EntityPropertyExtension("Size", "Size")]
       public string Size { get; set; }
       /// <summary>
       /// 数量
       /// </summary>
       [EntityPropertyExtension("Quantity", "Quantity")]
       public int Quantity { get; set; }
       /// <summary>
       /// BU 类别
       /// </summary>
       [EntityPropertyExtension("Product", "Product")]
       public string Product { get; set; }
       /// <summary>
       /// 年龄描述
       /// </summary>
       [EntityPropertyExtension("Gender", "Gender")]
       public string Gender { get; set; }
       /// <summary>
       /// CategoryCode
       /// </summary>
       [EntityPropertyExtension("CategoryCode", "CategoryCode")]
       public string CategoryCode { get; set; }
       /// <summary>
       /// CategoryCode对应的意思
       /// </summary>
       [EntityPropertyExtension("Category", "Category")]
       public string Category { get; set; }
       /// <summary>
       /// MaterialDesc
       /// </summary>
       [EntityPropertyExtension("MaterialDesc", "MaterialDesc")]
       public string MaterialDesc { get; set; }
       /// <summary>
       /// 货号
       /// </summary>
       [EntityPropertyExtension("Material", "Material")]
       public string Material { get; set; }
       /// <summary>
       /// 包装单号
       /// </summary>
       [EntityPropertyExtension("PackageNumber", "PackageNumber")]
       public string PackageNumber { get; set; }
        /// <summary>
        /// 楼层
        /// </summary>
        [EntityPropertyExtension("str9", "str9")]
        public string str9 { get; set; }
    }
}
