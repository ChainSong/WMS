using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.SettlementManagement
{
    public class Settlement
    {
        /// <summary>
        /// 序号
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }

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
        /// 仓库ID
        /// </summary>
        [EntityPropertyExtension("WarehouseID", "WarehouseID")]
        public long WarehouseID { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        [EntityPropertyExtension("WarehouseName", "WarehouseName")]
        public string WarehouseName { get; set; }
        /// <summary>
        /// 结算单号
        /// </summary>
        [EntityPropertyExtension("SettlementNumber", "SettlementNumber")]
        public string SettlementNumber { get; set; }
        /// <summary>
        /// 外部单号
        /// </summary>
        [EntityPropertyExtension("ExternNumber", "ExternNumber")]
        public string ExternNumber { get; set; }
        /// <summary>
        /// 结算类型 0应收1应付
        /// </summary>
        [EntityPropertyExtension("SettlementType", "SettlementType")]
        public int SettlementType { get; set; }
        /// <summary>
        /// 结算月份
        /// </summary>
        [EntityPropertyExtension("SettlementMonth", "SettlementMonth")]
        public string SettlementMonth { get; set; }
        /// <summary>
        /// 结算日
        /// </summary>
        [EntityPropertyExtension("SettlementDay", "SettlementDay")]
        public string SettlementDay { get; set; }
        /// <summary>
        /// 是否结算
        /// </summary>
        [EntityPropertyExtension("WhetherToSettle", "WhetherToSettle")]
        public int WhetherToSettle { get; set; }
        /// <summary>
        /// 仓租费率
        /// </summary>
        [EntityPropertyExtension("RentalUnitRate", "RentalUnitRate")]
        public double RentalUnitRate { get; set; }
        /// <summary>
        /// 项目费率
        /// </summary>
        [EntityPropertyExtension("FacilityItemsUnitRate", "FacilityItemsUnitRate")]
        public double FacilityItemsUnitRate { get; set; }
        /// <summary>
        /// 订单费率
        /// </summary>
        [EntityPropertyExtension("OBHandlingUnitRate", "OBHandlingUnitRate")]
        public double OBHandlingUnitRate { get; set; }
        /// <summary>
        /// 挂架费率
        /// </summary>
        [EntityPropertyExtension("GOHHandlingUnitRate", "GOHHandlingUnitRate")]
        public double GOHHandlingUnitRate { get; set; }
        /// <summary>
        /// 安全扣费率
        /// </summary>
        [EntityPropertyExtension("SecurityTagUnitRate", "SecurityTagUnitRate")]
        public double SecurityTagUnitRate { get; set; }
        /// <summary>
        /// 标准单位
        /// </summary>
        [EntityPropertyExtension("UOM", "UOM")]
        public string UOM { get; set; }
        /// <summary>
        /// 总件数
        /// </summary>
        [EntityPropertyExtension("Qty", "Qty")]
        public double Qty { get; set; }
        /// <summary>
        /// 挂架件数
        /// </summary>
        [EntityPropertyExtension("HangerQty", "HangerQty")]
        public double HangerQty { get; set; }
        /// <summary>
        /// 安全扣件数
        /// </summary>
        [EntityPropertyExtension("SafelockQty", "SafelockQty")]
        public double SafelockQty { get; set; }
        /// <summary>
        /// 仓租小计
        /// </summary>
        [EntityPropertyExtension("RentalSubTotal", "RentalSubTotal")]
        public double RentalSubTotal { get; set; }
        /// <summary>
        /// 项目小计
        /// </summary>
        [EntityPropertyExtension("FacilitySubTotal", "FacilitySubTotal")]
        public double FacilitySubTotal { get; set; }
        /// <summary>
        /// 订单小计
        /// </summary>
        [EntityPropertyExtension("OBHandlingSubTotal", "OBHandlingSubTotal")]
        public double OBHandlingSubTotal { get; set; }
        /// <summary>
        /// 挂架小计
        /// </summary>
        [EntityPropertyExtension("GOHHandlingSubTotal", "GOHHandlingSubTotal")]
        public double GOHHandlingSubTotal { get; set; }
        /// <summary>
        /// 安全扣小计
        /// </summary>
        [EntityPropertyExtension("SecurityTagSubTotal", "SecurityTagSubTotal")]
        public double SecurityTagSubTotal { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        [EntityPropertyExtension("TaxRate", "TaxRate")]
        public double TaxRate { get; set; }
        /// <summary>
        /// 仓租税额
        /// </summary>
        [EntityPropertyExtension("RentalTaxAmount", "RentalTaxAmount")]
        public double RentalTaxAmount { get; set; }
        /// <summary>
        /// 项目税额
        /// </summary>
        [EntityPropertyExtension("FacilityTaxAmount", "FacilityTaxAmount")]
        public double FacilityTaxAmount { get; set; }
        /// <summary>
        /// 订单税额
        /// </summary>
        [EntityPropertyExtension("OBHandlingTaxAmount", "OBHandlingTaxAmount")]
        public double OBHandlingTaxAmount { get; set; }
        /// <summary>
        /// 挂架税额
        /// </summary>
        [EntityPropertyExtension("GOHHandlingTaxAmount", "GOHHandlingTaxAmount")]
        public double GOHHandlingTaxAmount { get; set; }
        /// <summary>
        /// 安全扣税额
        /// </summary>
        [EntityPropertyExtension("SecurityTagTaxAmount", "SecurityTagTaxAmount")]
        public double SecurityTagTaxAmount { get; set; }
        /// <summary>
        /// 仓租总计费
        /// </summary>
        [EntityPropertyExtension("RentalTotalBilling", "RentalTotalBilling")]
        public double RentalTotalBilling { get; set; }
        /// <summary>
        /// 项目总计费
        /// </summary>
        [EntityPropertyExtension("FacilityTotalBilling", "FacilityTotalBilling")]
        public double FacilityTotalBilling { get; set; }
        /// <summary>
        /// 订单总计费
        /// </summary>
        [EntityPropertyExtension("OBHandlingTotalBilling", "OBHandlingTotalBilling")]
        public double OBHandlingTotalBilling { get; set; }
        /// <summary>
        /// 挂架总计费
        /// </summary>
        [EntityPropertyExtension("GOHHandlingTotalBilling", "GOHHandlingTotalBilling")]
        public double GOHHandlingTotalBilling { get; set; }
        /// <summary>
        /// 安全扣总计费
        /// </summary>
        [EntityPropertyExtension("SecurityTagTotalBilling", "SecurityTagTotalBilling")]
        public double SecurityTagTotalBilling { get; set; }
        /// <summary>
        /// 总计费
        /// </summary>
        [EntityPropertyExtension("TotalBilling", "TotalBilling")]
        public double TotalBilling { get; set; }
        /// <summary>
        /// HandlingCost
        /// </summary>
        [EntityPropertyExtension("HandlingCost", "HandlingCost")]
        public double HandlingCost { get; set; }
        /// <summary>
        /// VAS
        /// </summary>
        [EntityPropertyExtension("VAS", "VAS")]
        public double VAS { get; set; }
        /// <summary>
        /// 结算人
        /// </summary>
        [EntityPropertyExtension("Settler", "Settler")]
        public string Settler { get; set; }
        /// <summary>
        /// 结算时间
        /// </summary>
        [EntityPropertyExtension("SettlementTime", "SettlementTime")]
        public DateTime SettlementTime { get; set; }
        /// <summary>
        /// 取消结算人
        /// </summary>
        [EntityPropertyExtension("CancelSettler", "CancelSettler")]
        public string CancelSettler { get; set; }
        /// <summary>
        /// 取消结算时间
        /// </summary>
        [EntityPropertyExtension("CancelSettlementTime", "CancelSettlementTime")]
        public DateTime CancelSettlementTime { get; set; }
        /// <summary>
        /// 重新结算人
        /// </summary>
        [EntityPropertyExtension("ReClearingSettler", "ReClearingSettler")]
        public string ReClearingSettler { get; set; }
        /// <summary>
        /// 重新结算时间
        /// </summary>
        [EntityPropertyExtension("ReClearingTime", "ReClearingTime")]
        public DateTime ReClearingTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [EntityPropertyExtension("Creator", "Creator")]
        public string Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [EntityPropertyExtension("CreateTime", "CreateTime")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新人
        /// </summary>
        [EntityPropertyExtension("Updator", "Updator")]
        public string Updator { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [EntityPropertyExtension("UpdateTime", "UpdateTime")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [EntityPropertyExtension("Remark", "Remark")]
        public string Remark { get; set; }

        public string str1 { get; set; }
        public string str2 { get; set; }
        public string str3 { get; set; }
        public string str4 { get; set; }
        public string str5 { get; set; }
        public string str6 { get; set; }
        public string str7 { get; set; }
        public string str8 { get; set; }
        public string str9 { get; set; }
        public string str10 { get; set; }
        public string str11 { get; set; }
        public string str12 { get; set; }
        public string str13 { get; set; }
        public string str14 { get; set; }
        public string str15 { get; set; }
        public string str16 { get; set; }
        public string str17 { get; set; }
        public string str18 { get; set; }
        public string str19 { get; set; }
        public string str20 { get; set; }
        public DateTime DateTime1 { get; set; }
        public DateTime DateTime2 { get; set; }
        public DateTime DateTime3 { get; set; }
        public DateTime DateTime4 { get; set; }
        public DateTime DateTime5 { get; set; }
        public int Int1 { get; set; }
        public int Int2 { get; set; }
        public int Int3 { get; set; }
        public int Int4 { get; set; }
        public int Int5 { get; set; }
        /// <summary>
        /// 费用类型 新加
        /// </summary>
        [EntityPropertyExtension("CostCategory", "CostCategory")]
        public int CostCategory { get; set; }

    }
}
