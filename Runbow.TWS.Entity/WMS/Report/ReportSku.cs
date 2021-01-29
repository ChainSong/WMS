using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 库存信息
    /// </summary>
    [Serializable]
    public class ReportSku
    {
        [EntityPropertyExtension("Unit", "Unit")]
        public string Unit { get; set; }

        [EntityPropertyExtension("Specifications", "Specifications")]
        public string Specifications { get; set; }

        [EntityPropertyExtension("BatchNumber", "BatchNumber")]
        public string BatchNumber { get; set; }

        [EntityPropertyExtension("BoxNumber", "BoxNumber")]
        public string BoxNumber { get; set; }

        [EntityPropertyExtension("Qty", "Qty")]
        public float Qty { get; set; }

        [EntityPropertyExtension("Types", "Types")]
        public string Types { get; set; }

        [EntityPropertyExtension("Dates", "Dates")]
        public DateTime? Dates { get; set; }

        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }

        [EntityPropertyExtension("CustomerID", "CustomerID")]
        public long? CustomerID { get; set; }
        

        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }

        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }
        /// <summary>
        /// 货主ID
        /// </summary>
        [EntityPropertyExtension("StorerID", "StorerID")]
        public long StorerID { get; set; }
        /// <summary>
        /// 货主名称
        /// </summary>
        [EntityPropertyExtension("StorerName", "StorerName")]
        public string StorerName { get; set; }
        /// <summary>
        /// 品名
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        /// <summary>
        /// 状态-0 不可用 1 可用
        /// </summary>
        [EntityPropertyExtension("Status", "Status")]
        public int Status { get; set; }
        /// <summary>
        /// 基本信息-货品种类
        /// </summary>
        [EntityPropertyExtension("GoodsName", "GoodsName")]
        public string GoodsName { get; set; }
        /// <summary>
        /// 基本信息-货品种类
        /// </summary>
        [EntityPropertyExtension("GoodsType", "GoodsType")]
        public int GoodsType { get; set; }
        /// <summary>
        /// 基本信息-SKU分类
        /// </summary>
        [EntityPropertyExtension("SKUClassification", "SKUClassification")]
        public string SKUClassification { get; set; }
        /// <summary>
        /// 基本信息-SKU分组
        /// </summary>
        [EntityPropertyExtension("SKUGroup", "SKUGroup")]
        public string SKUGroup { get; set; }
        /// <summary>
        /// 基本信息-制造商SKU
        /// </summary>
        [EntityPropertyExtension("ManufacturerSKU", "ManufacturerSKU")]
        public string ManufacturerSKU { get; set; }
        /// <summary>
        /// RetailSKU
        /// </summary>
        [EntityPropertyExtension("RetailSKU", "RetailSKU")]
        public string RetailSKU { get; set; }
        /// <summary>
        /// ReplaceSKU
        /// </summary>
        [EntityPropertyExtension("ReplaceSKU", "ReplaceSKU")]
        public string ReplaceSKU { get; set; }
        /// <summary>
        /// 基本信息-箱组
        /// </summary>
        [EntityPropertyExtension("BoxGroup", "BoxGroup")]
        public string BoxGroup { get; set; }
        /// <summary>
        /// 基本信息-包装
        /// </summary>
        [EntityPropertyExtension("Packing", "Packing")]
        public string Packing { get; set; }
        /// <summary>
        /// 基本信息-ABC分类
        /// </summary>
        [EntityPropertyExtension("Grade", "Grade")]
        public string Grade { get; set; }
        /// <summary>
        /// 基本信息-国家
        /// </summary>
        [EntityPropertyExtension("Country", "Country")]
        public string Country { get; set; }
        /// <summary>
        /// 基本信息-制造商
        /// </summary>
        [EntityPropertyExtension("Manufacturer", "Manufacturer")]
        public string Manufacturer { get; set; }
        /// <summary>
        /// 基本信息-危险代码
        /// </summary>
        [EntityPropertyExtension("DangerCode", "DangerCode")]
        public string DangerCode { get; set; }
        /// <summary>
        /// 包装信息-容积
        /// </summary>
        [EntityPropertyExtension("Volume", "Volume")]
        public string Volume { get; set; }
        /// <summary>
        /// 包装信息-标准容积
        /// </summary>
        [EntityPropertyExtension("StandardVolume", "StandardVolume")]
        public string StandardVolume { get; set; }
        /// <summary>
        /// 包装信息-毛重
        /// </summary>
        [EntityPropertyExtension("Weight", "Weight")]
        public string Weight { get; set; }
        /// <summary>
        /// 包装信息-标准毛重
        /// </summary>
        [EntityPropertyExtension("StandardWeight", "StandardWeight")]
        public string StandardWeight { get; set; }
        /// <summary>
        /// 包装信息-净重
        /// </summary>
        [EntityPropertyExtension("NetWeight", "NetWeight")]
        public string NetWeight { get; set; }
        /// <summary>
        /// 包装信息-标准净重
        /// </summary>
        [EntityPropertyExtension("StandardNetWeight", "StandardNetWeight")]
        public string StandardNetWeight { get; set; }
        /// <summary>
        /// 包装信息-单价
        /// </summary>
        [EntityPropertyExtension("Price", "Price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// 包装信息-实际单价
        /// </summary>
        [EntityPropertyExtension("ActualPrice", "ActualPrice")]
        public decimal? ActualPrice { get; set; }
        /// <summary>
        /// 成本
        /// </summary>
        [EntityPropertyExtension("Cost", "Cost")]
        public string Cost { get; set; }
        /// <summary>
        /// 包装信息-实际成本
        /// </summary>
        [EntityPropertyExtension("ActualCost", "ActualCost")]
        public string ActualCost { get; set; }
        /// <summary>
        /// 包装信息-标准订货成本
        /// </summary>
        [EntityPropertyExtension("StandardOrderingCost", "StandardOrderingCost")]
        public string StandardOrderingCost { get; set; }
        /// <summary>
        /// 包装信息-运输成本
        /// </summary>
        [EntityPropertyExtension("ShipmentCost", "ShipmentCost")]
        public string ShipmentCost { get; set; }
        /// <summary>
        /// 质检信息-质检地
        /// </summary>
        [EntityPropertyExtension("QcInSpectionLoc", "QcInSpectionLoc")]
        public string QcInSpectionLoc { get; set; }
        /// <summary>
        /// 质检信息-质检合格率
        /// </summary>
        [EntityPropertyExtension("QCPercentage", "QCPercentage")]
        public string QCPercentage { get; set; }
        /// <summary>
        /// 质检信息-质检Uom
        /// </summary>
        [EntityPropertyExtension("ReceiptQcUom", "ReceiptQcUom")]
        public string ReceiptQcUom { get; set; }
        /// <summary>
        /// 质检信息-质检是否合格
        /// </summary>
        [EntityPropertyExtension("IsQcEligible", "IsQcEligible")]
        public int IsQcEligible { get; set; }
        /// <summary>
        /// 策略信息-放货区域
        /// </summary>
        [EntityPropertyExtension("PutArea", "PutArea")]
        public string PutArea { get; set; }
        /// <summary>
        /// 策略信息-放货代码
        /// </summary>
        [EntityPropertyExtension("PutCode", "PutCode")]
        public string PutCode { get; set; }
        /// <summary>
        /// 策略信息-放置规则
        /// </summary>
        [EntityPropertyExtension("PutRule", "PutRule")]
        public string PutRule { get; set; }
        /// <summary>
        /// 策略信息-策略
        /// </summary>
        [EntityPropertyExtension("PutStrategy", "PutStrategy")]
        public string PutStrategy { get; set; }
        /// <summary>
        /// 策略信息-分配规则
        /// </summary>
        [EntityPropertyExtension("AllocateRule", "AllocateRule")]
        public string AllocateRule { get; set; }
        /// <summary>
        /// 策略-拣货代码
        /// </summary>
        [EntityPropertyExtension("PickedCode", "PickedCode")]
        public string PickedCode { get; set; }
        /// <summary>
        /// 货品信息-品名类别
        /// </summary>
        [EntityPropertyExtension("SKUType", "SKUType")]
        public string SKUType { get; set; }
        /// <summary>
        /// 货品信息-货品颜色
        /// </summary>
        [EntityPropertyExtension("Color", "Color")]
        public string Color { get; set; }
        /// <summary>
        /// 货品信息-货品尺寸
        /// </summary>
        [EntityPropertyExtension("Size", "Size")]
        public string Size { get; set; }


        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }
    }
}
