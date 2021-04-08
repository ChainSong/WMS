using System;
using Runbow.TWS.Common;


namespace Runbow.TWS.Entity
{
    /// <summary>
    /// 顺丰下单返回明细
    /// </summary>
    public class WMS_SFDetail
    {
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [EntityPropertyExtension("OID", "OID")]
        public long? OID { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [EntityPropertyExtension("OrderNumber", "OrderNumber")]
        public string OrderNumber { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        [EntityPropertyExtension("waybillNo", "waybillNo")]
        public string waybillNo { get; set; }

        /// <summary>
        /// 原寄地中转场
        /// </summary>
        [EntityPropertyExtension("sourceTransferCode", "sourceTransferCode")]
        public string sourceTransferCode { get; set; }

        /// <summary>
        /// 原寄地城市代码
        /// </summary>
        [EntityPropertyExtension("sourceCityCode", "sourceCityCode")]
        public string sourceCityCode { get; set; }

        /// <summary>
        /// 原寄地网点代码
        /// </summary>
        [EntityPropertyExtension("sourceDeptCode", "sourceDeptCode")]
        public string sourceDeptCode { get; set; }

        /// <summary>
        /// 原寄地单元区域
        /// </summary>
        [EntityPropertyExtension("sourceTeamCode", "sourceTeamCode")]
        public string sourceTeamCode { get; set; }

        /// <summary>
        /// 目的地城市代码（eg:755)
        /// </summary>
        [EntityPropertyExtension("destCityCode", "destCityCode")]
        public string destCityCode { get; set; }

        /// <summary>
        /// 目的地网点代码（eg:755AQ)
        /// </summary>
        [EntityPropertyExtension("destDeptCode", "destDeptCode")]
        public string destDeptCode { get; set; }

        /// <summary>
        /// 目的地网点代码映射码
        /// </summary>
        [EntityPropertyExtension("destDeptCodeMapping", "destDeptCodeMapping")]
        public string destDeptCodeMapping { get; set; }

        /// <summary>
        /// 目的地单元区域（eg:001)
        /// </summary>
        [EntityPropertyExtension("destTeamCode", "destTeamCode")]
        public string destTeamCode { get; set; }

        /// <summary>
        /// 目的地单元区域映射码
        /// </summary>
        [EntityPropertyExtension("destTeamCodeMapping", "destTeamCodeMapping")]
        public string destTeamCodeMapping { get; set; }

        /// <summary>
        /// 目的地中转场
        /// </summary>
        [EntityPropertyExtension("destTransferCode", "destTransferCode")]
        public string destTransferCode { get; set; }

        /// <summary>
        /// 打单时的路由标签信息
        /// </summary>
        [EntityPropertyExtension("destRouteLabel", "destRouteLabel")]
        public string destRouteLabel { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [EntityPropertyExtension("proName", "proName")]
        public string proName { get; set; }

        /// <summary>
        /// 快件内容,如:C816、SP601
        /// </summary>
        [EntityPropertyExtension("cargoTypeCode", "cargoTypeCode")]
        public string cargoTypeCode { get; set; }

        /// <summary>
        /// 时效代码, 如:T4
        /// </summary>
        [EntityPropertyExtension("limitTypeCode", "limitTypeCode")]
        public string limitTypeCode { get; set; }

        /// <summary>
        /// 产品类型,如:B1
        /// </summary>
        [EntityPropertyExtension("expressTypeCode", "expressTypeCode")]
        public string expressTypeCode { get; set; }

        /// <summary>
        /// 入港映射码（eg:S10)
        /// </summary>
        [EntityPropertyExtension("codingMapping", "codingMapping")]
        public string codingMapping { get; set; }

        /// <summary>
        /// 出港映射码
        /// </summary>
        [EntityPropertyExtension("codingMappingOut", "codingMappingOut")]
        public string codingMappingOut { get; set; }

        /// <summary>
        /// XB标志(0:不需要打印XB,1:需要打印XB)
        /// </summary>
        [EntityPropertyExtension("xbFlag", "xbFlag")]
        public string xbFlag { get; set; }

        /// <summary>
        /// 打印标志
        /// </summary>
        [EntityPropertyExtension("printFlag", "printFlag")]
        public string printFlag { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        [EntityPropertyExtension("twoDimensionCode", "twoDimensionCode")]
        public string twoDimensionCode { get; set; }

        /// <summary>
        /// 时效类型,值为二维码中的K4
        /// </summary>
        [EntityPropertyExtension("proCode", "proCode")]
        public string proCode { get; set; }

        /// <summary>
        /// 打印图标
        /// </summary>
        [EntityPropertyExtension("printIcon", "printIcon")]
        public string printIcon { get; set; }

        /// <summary>
        /// AB标
        /// </summary>
        [EntityPropertyExtension("abFlag", "abFlag")]
        public string abFlag { get; set; }

        /// <summary>
        /// 目的地口岸代码
        /// </summary>
        [EntityPropertyExtension("destPortCode", "destPortCode")]
        public string destPortCode { get; set; }

        /// <summary>
        /// 目的国别(国别代码如:JP)
        /// </summary>
        [EntityPropertyExtension("destCountry", "destCountry")]
        public string destCountry { get; set; }

        /// <summary>
        /// 目的地邮编
        /// </summary>
        [EntityPropertyExtension("destPostCode", "destPostCode")]
        public string destPostCode { get; set; }

        /// <summary>
        /// 总价值(保留两位小数, 数字类型, 可补位)
        /// </summary>
        [EntityPropertyExtension("goodsValueTotal", "goodsValueTotal")]
        public string goodsValueTotal { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        [EntityPropertyExtension("currencySymbol", "currencySymbol")]
        public string currencySymbol { get; set; }

        /// <summary>
        /// 件数
        /// </summary>
        [EntityPropertyExtension("goodsNumber", "goodsNumber")]
        public string goodsNumber { get; set; }

        /// <summary>
        /// 根据k1-k6生成的校验码
        /// </summary>
        [EntityPropertyExtension("checkCode", "checkCode")]
        public string checkCode { get; set; }

        
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime? CreateTime { get; set; }
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
        [EntityPropertyExtension("str6", "str6")]
        public string str6 { get; set; }
        [EntityPropertyExtension("str7", "str7")]
        public string str7 { get; set; }
        [EntityPropertyExtension("str8", "str8")]
        public string str8 { get; set; }
        [EntityPropertyExtension("str9", "str9")]
        public string str9 { get; set; }
        [EntityPropertyExtension("str10", "str10")]
        public string str10 { get; set; }
    }
}
