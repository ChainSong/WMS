using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity
{
    public class ProductStorer
    {
        [EntityPropertyExtension("ID", "ID")]
        public long? ID { get; set; }
        /// <summary>
        /// 货主ID
        /// </summary>
        [EntityPropertyExtension("StorerID", "StorerID")]
        public long? StorerID { get; set; }

        /// <summary>
        /// 货主ID
        /// </summary>
        [EntityPropertyExtension("CustomerName", "CustomerName")]
        public string CustomerName { get; set; }
        /// <summary>
        /// 品名
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        [EntityPropertyExtension("UPC", "UPC")]
        public string UPC { get; set; }       
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
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }

        [EntityPropertyExtension("Str1", "Str1")]
        public string Str1 { get; set; }
        [EntityPropertyExtension("Str2", "Str2")]
        public string Str2 { get; set; }
        [EntityPropertyExtension("Str3", "Str3")]
        public string Str3 { get; set; }

        [EntityPropertyExtension("Str4", "Str4")]
        public string Str4 { get; set; }
        [EntityPropertyExtension("Str5", "Str5")]
        public string Str5 { get; set; }
        [EntityPropertyExtension("Str6", "Str6")]
        public string Str6 { get; set; }

        [EntityPropertyExtension("Str7", "Str7")]
        public string Str7 { get; set; }
        [EntityPropertyExtension("Str8", "Str8")]
        public string Str8 { get; set; }
        [EntityPropertyExtension("Str9", "Str9")]
        public string Str9 { get; set; }

        [EntityPropertyExtension("Str10", "Str10")]
        public string Str10 { get; set; }
        [EntityPropertyExtension("Str11", "Str11")]
        public string Str11 { get; set; }
        [EntityPropertyExtension("Str12", "Str12")]
        public string Str12 { get; set; }

        [EntityPropertyExtension("Str13", "Str13")]
        public string Str13 { get; set; }
        [EntityPropertyExtension("Str14", "Str14")]
        public string Str14 { get; set; }
        [EntityPropertyExtension("Str15", "Str15")]
        public string Str15 { get; set; }

        [EntityPropertyExtension("Str16", "Str16")]
        public string Str16 { get; set; }
        [EntityPropertyExtension("Str17", "Str17")]
        public string Str17 { get; set; }
        [EntityPropertyExtension("Str18", "Str18")]
        public string Str18 { get; set; }

        [EntityPropertyExtension("Str19", "Str19")]
        public string Str19 { get; set; }
        [EntityPropertyExtension("Str20", "Str20")]
        public string Str20 { get; set; }
        [EntityPropertyExtension("DateTime1", "DateTime1")]
        public DateTime DateTime1 { get; set; }

        [EntityPropertyExtension("DateTime2", "DateTime2")]
        public DateTime DateTime2 { get; set; }
        [EntityPropertyExtension("DateTime3", "DateTime3")]
        public DateTime DateTime3 { get; set; }
        [EntityPropertyExtension("DateTime4", "DateTime4")]
        public DateTime DateTime4 { get; set; }
        [EntityPropertyExtension("DateTime5", "DateTime5")]
        public DateTime DateTime5 { get; set; }

        [EntityPropertyExtension("Bigint1", "Bigint1")]
        public long? Bigint1 { get; set; }


        [EntityPropertyExtension("Bingint2", "Bingint2")]
        public long? Bingint2 { get; set; }
        [EntityPropertyExtension("Bigint3", "Bigint3")]
        public long? Bigint3 { get; set; }

        [EntityPropertyExtension("Int1", "Int1")]
        public int? Int1 { get; set; }

        [EntityPropertyExtension("Int2", "Int2")]
        public int? Int2 { get; set; }
        [EntityPropertyExtension("Int3", "Int3")]
        public int? Int3 { get; set; }
        [EntityPropertyExtension("Bit1", "Bit1")]
        public Boolean? Bit1 { get; set; }
        [EntityPropertyExtension("Bit2", "Bit2")]
        public Boolean? Bit2 { get; set; }
        [EntityPropertyExtension("Bit3", "Bit3")]
        public Boolean? Bit3 { get; set; }


        /// <summary>
        /// 货品信息-BU
        /// </summary>
        [EntityPropertyExtension("Division", "Division")]
        public string Division { get; set; }


        /// <summary>
        /// 货品信息-GenderAge
        /// </summary>
        [EntityPropertyExtension("GenderAge", "GenderAge")]
        public string GenderAge { get; set; }

        /// <summary>
        /// 货品信息-SilHouette
        /// </summary>
        [EntityPropertyExtension("SilHouette", "SilHouette")]
        public string SilHouette { get; set; }

        /// <summary>
        /// 货品信息-CategoryText
        /// </summary>
        [EntityPropertyExtension("CategoryText", "CategoryText")]
        public string CategoryText { get; set; }

        /// <summary>
        /// 货品信息-Hanger
        /// </summary>
        [EntityPropertyExtension("Hanger", "Hanger")]
        public string Hanger { get; set; }

        /// <summary>
        /// 货品信息-GlblCatSumCode
        /// </summary>
        [EntityPropertyExtension("GlblCatSumCode", "GlblCatSumCode")]
        public string GlblCatSumCode { get; set; }

        /// <summary>
        /// 货品信息-GlblCatSum
        /// </summary>
        [EntityPropertyExtension("GlblCatSum", "GlblCatSum")]
        public string GlblCatSum { get; set; }

        /// <summary>
        /// 货品信息-SeasonCode
        /// </summary>
        [EntityPropertyExtension("SeasonCode", "SeasonCode")]
        public string SeasonCode { get; set; }

        /// <summary>
        /// 货品信息-seasonYear
        /// </summary>
        [EntityPropertyExtension("seasonYear", "seasonYear")]
        public string seasonYear { get; set; }

        /// <summary>
        /// 货品信息-CategoryCode
        /// </summary>
        [EntityPropertyExtension("CategoryCode", "CategoryCode")]
        public string CategoryCode { get; set; }

        /// <summary>
        /// 货品信息-SilHouette
        /// </summary>
        [EntityPropertyExtension("SubCategoryCode", "SubCategoryCode")]
        public string SubCategoryCode { get; set; }


    }
}
