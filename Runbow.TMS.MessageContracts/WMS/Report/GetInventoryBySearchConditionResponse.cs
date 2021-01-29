using Runbow.TWS.Entity;
using System.Collections.Generic;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.Entity.WMS.NikeNFSPrint;

namespace Runbow.TWS.MessageContracts.WMS.Report
{
    public class GetInventoryBySearchConditionResponse
    {
        /// <summary>
        /// 装箱清单
        /// </summary>
        public IEnumerable<NFSPrintBoxInfo> NikePackageReportCollcetion { get; set; }
        /// <summary>
        /// 出货日报表
        /// </summary>
        public IEnumerable<ReprotTableIn> OrderReportCollection { get; set; }
        /// <summary>
        /// 每日库存报表
        /// </summary>
        public IEnumerable<ReportInventory> InventoryCollection { get; set; }
        /// <summary>
        /// 每日退货收货报表
        /// </summary>
        public IEnumerable<ReportReceiptReport> ReceiptBackReportCollection { get; set; }
        /// <summary>
        /// 每日收货报表
        /// </summary>
        public IEnumerable<ReportReceiptReport> ReceiptReportCollection { get; set; }
        /// <summary>
        /// 库存统计
        /// </summary>
        public IEnumerable<ReportInventory> InventoryTongjiEmailCollection { get; set; }
        /// <summary>
        /// 收货数量
        /// </summary>
        public IEnumerable<ReportInventory> ReceiptEmailCollection { get; set; }
        /// <summary>
        /// 退货入库数量
        /// </summary>
        public IEnumerable<ReportInventory> ReceiptBackEmailCollection { get; set; }
        /// <summary>
        /// 门店补货数量
        /// </summary>
        public IEnumerable<ReportInventory> BuHuoEmailCollection { get; set; }
        /// <summary>
        /// 调入数量
        /// </summary>
        public IEnumerable<ReportInventory> DiaoRuEmailCollection { get; set; }
        /// <summary>
        /// 调出数量
        /// </summary>
        public IEnumerable<ReportInventory> DiaoChuEmailCollection { get; set; }
        /// <summary>
        /// 索赔调减
        /// </summary>
        public IEnumerable<ReportInventory> AdjustmentEmailCollection { get; set; }

        /// <summary>
        /// 索赔调增
        /// </summary>
        public IEnumerable<ReportInventory> AdjustmentAddEmailCollection { get; set; }
        /// <summary>
        /// 冻结
        /// </summary>
        public IEnumerable<ReportInventory> AdjustmentFrezzEmailCollection { get; set; }
        /// <summary>
        /// 门店直发
        /// </summary>
        public IEnumerable<ReportInventory> MenDianZhiFaEmailCollection { get; set; }
        /// <summary>
        /// o2o
        /// </summary>
        public IEnumerable<ReportInventory> o2oEmailCollection { get; set; }
        public IEnumerable<ReportInventorySummary> InventorySummaryCollection { get; set; }
        public IEnumerable<WMS_Customer> WMS_CustomerCollection { get; set; }
        public WMS_Customer EmailConfig { get; set; }
        public int PageCount { get; set; }

        public int PageIndex { get; set; }

        public IEnumerable<Entity.WMS.Receipt.ReceiptPrint> receiptPrint { get; set; }
    }
}
