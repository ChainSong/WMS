using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
   public class WarehouseCheckDetail
    {
       [EntityPropertyExtension("UPC", "UPC")]
       public string UPC { get; set; }

       /// <summary>
       /// 单位
       /// </summary>
        [EntityPropertyExtension("Unit", "Unit")]
        public string Unit { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }

        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        [EntityPropertyExtension("CID", "CID")]
        public long CID { get; set; }

        /// <summary>
        /// 盘点单号
        /// </summary>
        [EntityPropertyExtension("CheckNumber", "CheckNumber")]
        public string CheckNumber { get; set; }
        /// <summary>
        /// 外部单号
        /// </summary>
        [EntityPropertyExtension("ExternNumber", "ExternNumber")]
        public string ExternNumber { get; set; }
        /// <summary>
        /// 客户ID
        /// </summary>
        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long CustomerID { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        /// <summary>
        /// 所属仓库
        /// </summary>
        [EntityPropertyExtension("Warehouse", "Warehouse")]
        public string Warehouse { get; set; }

        /// <summary>
        /// 库区
        /// </summary>
        [EntityPropertyExtension("Area", "Area")]
        public string Area { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        /// <summary>
        /// 品级
        /// </summary>
        [EntityPropertyExtension("GoodsType", "GoodsType")]
        public string GoodsType { get; set; }
        /// <summary>
        /// 托号
        /// </summary>
        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }

        
        /// <summary>
        /// 盘点数量
        /// </summary>
        [EntityPropertyExtension("CheckQty", "CheckQty")]
        public decimal CheckQty { get; set; }
        /// <summary>
        /// 实际盘点数量
        /// </summary>
        [EntityPropertyExtension("ActualQty", "ActualQty")]
        public long ActualQty { get; set; }
        /// <summary>
        /// 差异数量
        /// </summary>
        [EntityPropertyExtension("DifferQty", "DifferQty")]
        public string DifferQty { get; set; }
        
        
        /// <summary>
        /// 备注
        /// </summary>
        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }
        /// <summary>
        /// 是否存在差异
        /// </summary>
        [EntityPropertyExtension("IS_Difference", "IS_Difference")]
        public string IS_Difference { get; set; }
        /// <summary>
        /// 是否处理
        /// </summary>
        [EntityPropertyExtension("IS_Deal", "IS_Deal")]
        public string IS_Deal { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// article
        /// </summary>
        [EntityPropertyExtension("str10", "str10")]
        public string str10 { get; set; }
        /// <summary>
        /// size
        /// </summary>
        [EntityPropertyExtension("str9", "str9")]
        public string str9 { get; set; }
        /// <summary>
        /// BU
        /// </summary>
        [EntityPropertyExtension("str8", "str8")]
        public string str8 { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        [EntityPropertyExtension("BusinessType", "BusinessType")]
        public string BusinessType { get; set; }
    }
}
