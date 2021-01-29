using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.Warehouse
{
    public class WarehouseCheck
    {

        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }

        [EntityPropertyExtension("Unit", "Unit")]
        public string Unit { get; set; }

        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }

        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

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
        /// 盘点日期
        /// </summary>
        [EntityPropertyExtension("Checkdate", "Checkdate")]
        public string Checkdate { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [EntityPropertyExtension("Area", "Area")]
        public string Area { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [EntityPropertyExtension("Location", "Location")]
        public string Location { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }


        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }

        /// <summary>
        /// GoodsType
        /// </summary>
        [EntityPropertyExtension("GoodsType", "GoodsType")]
        public string GoodsType { get; set; }

        /// <summary>
        /// Qty
        /// </summary>
        [EntityPropertyExtension("Qty", "Qty")]
        public string Qty { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [EntityPropertyExtension("Type", "Type")]
        public string Type { get; set; }

        /// <summary>
        /// 省份城市
        /// </summary>
        [EntityPropertyExtension("Type_Description", "Type_Description")]
        public string Type_Description { get; set; }
        /// <summary>
        /// 邮政编号
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
        public string CreateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public string UpdateTime { get; set; }

        


        [EntityPropertyExtension("str1", "str1")]
        public string str1 { get; set; }

        [EntityPropertyExtension("str2", "str2")]
        public string str2 { get; set; }

        [EntityPropertyExtension("str3", "str3")]
        public string str3 { get; set; }

        [EntityPropertyExtension("str4", "str4")]
        public string str4 { get; set; }

        [EntityPropertyExtension("str5", "str5")]
        public string str5 { get; set; }

    }
}
