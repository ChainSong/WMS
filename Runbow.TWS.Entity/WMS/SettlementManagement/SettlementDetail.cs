using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.WMS.SettlementManagement
{
    public class SettlementDetail
    {
        /// <summary>
        /// 序号
        /// </summary>
        [EntityPropertyExtension("ID", "ID")]
        public long ID { get; set; }
        /// <summary>
        /// 主表序号
        /// </summary>
        [EntityPropertyExtension("WSID", "WSID")]
        public long WSID { get; set; }
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
        /// 结算行号
        /// </summary>
        [EntityPropertyExtension("LineNumber", "LineNumber")]
        public string LineNumber { get; set; }



        /// <summary>
        /// 日期
        /// </summary>
        [EntityPropertyExtension("OrderDate", "OrderDate")]
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// 运输类型
        /// </summary>
        [EntityPropertyExtension("TransportatioType", "TransportatioType")]
        public string TransportatioType { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        [EntityPropertyExtension("OrderNumber", "OrderNumber")]
        public string OrderNumber { get; set; }
        /// <summary>
        /// 发货门店代码
        /// </summary>
        [EntityPropertyExtension("DeliveryStoreCode", "DeliveryStoreCode")]
        public string DeliveryStoreCode { get; set; }
        /// <summary>
        /// 发货门店名称
        /// </summary>
        [EntityPropertyExtension("DeliveryStoreName", "DeliveryStoreName")]
        public string DeliveryStoreName { get; set; }
        /// <summary>
        /// 收货门店代码
        /// </summary>
        [EntityPropertyExtension("ReceivingStoreCode", "ReceivingStoreCode")]
        public string ReceivingStoreCode { get; set; }
        /// <summary>
        /// 收货门店名称
        /// </summary>
        [EntityPropertyExtension("ReceivingStoreName", "ReceivingStoreName")]
        public string ReceivingStoreName { get; set; }
        /// <summary>
        /// 箱数
        /// </summary>
        [EntityPropertyExtension("BoxQty", "BoxQty")]
        public double BoxQty { get; set; }
        /// <summary>
        /// 件数
        /// </summary>
        [EntityPropertyExtension("Qty", "Qty")]
        public double Qty { get; set; }
        /// <summary>
        /// 防盗扣数量
        /// </summary>
        [EntityPropertyExtension("SafelockQty", "SafelockQty")]
        public double SafelockQty { get; set; }
        /// <summary>
        /// 衣架数量
        /// </summary>
        [EntityPropertyExtension("HangerQty", "HangerQty")]
        public double HangerQty { get; set; }



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
        public DateTime? DateTime1 { get; set; }
        public DateTime? DateTime2 { get; set; }
        public DateTime? DateTime3 { get; set; }
        public DateTime? DateTime4 { get; set; }
        public DateTime? DateTime5 { get; set; }
        public int? Int1 { get; set; }
        public int? Int2 { get; set; }
        public int? Int3 { get; set; }
        public int? Int4 { get; set; }
        public int? Int5 { get; set; }

        /// <summary>
        /// 仓库名称 新加
        /// </summary>
        [EntityPropertyExtension("Warehouse", "Warehouse")]
        public string Warehouse { get; set; }
        /// <summary>
        /// 出库日期 新加
        /// </summary>
        [EntityPropertyExtension("CompleteDate", "CompleteDate")]
        public DateTime CompleteDate { get; set; }
        /// <summary>
        /// 外部单号
        /// </summary>
        [EntityPropertyExtension("ExternOrderNumber", "ExternOrderNumber")]
        public string ExternOrderNumber { get; set; }
        /// <summary>
        /// 收货门店 新加
        /// </summary>
        [EntityPropertyExtension("Company", "Company")]
        public string Company { get; set; }
        /// <summary>
        /// SKU 新加
        /// </summary>
        [EntityPropertyExtension("SKU", "SKU")]
        public string SKU { get; set; }
        /// <summary>
        /// 箱数 新加
        /// </summary>
        [EntityPropertyExtension("TotalBox", "TotalBox")]
        public int TotalBox { get; set; }
        /// <summary>
        /// 安全扣 新加
        /// </summary>
        [EntityPropertyExtension("Safelock", "Safelock")]
        public string Safelock { get; set; }
        /// <summary>
        /// 衣架 新加
        /// </summary>
        [EntityPropertyExtension("Hanger", "Hanger")]
        public string Hanger { get; set; }
        /// <summary>
        /// 费用类型 新加
        /// </summary>
        [EntityPropertyExtension("CostCategory", "CostCategory")]
        public int CostCategory { get; set; }

    }
}
