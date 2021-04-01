using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ss = System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.WMS.Report;
using Runbow.TWS.Web.Areas.WMS.Models.ReceiptManagement;
using Runbow.TWS.Web.Areas.WMS.Models.ReportManagement;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Common;
using UtilConstants = Runbow.TWS.Common.Constants;
using Runbow.TWS.MessageContracts;
using System.Text;
using ThoughtWorks.QRCode.Codec;
using System.Drawing;
using SysIO = System.IO;
using Runbow.TWS.Biz;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
using Runbow.TWS.Entity.WMS.Report;
using Runbow.TWS.Entity.WMS;
using DataTable = System.Data.DataTable;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Runbow.TWS.Entity.WMS.NikeNFSPrint;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.Entity.WMS.Warehouse;
using System.Threading;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class ReportManagementController : BaseController
    {
        /// <summary>
        /// 获取每日报表邮件表格中的数据
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public JsonResult GetReportSendEmail(string StoreID, string CustomerID, string start_CompleteDate, string end_CompleteDate)
        {
            List<List<ReportInventory>> lists = new List<List<ReportInventory>>();
            List<WMS_Customer> lists2 = new List<WMS_Customer>();
            try
            {
                var response = new ReportManagementService().GetNikeReportEmail(StoreID, CustomerID, start_CompleteDate, end_CompleteDate);
                if (response.IsSuccess)
                {
                    lists.Add(response.Result.InventoryTongjiEmailCollection.ToList());
                    lists.Add(response.Result.ReceiptEmailCollection.ToList());
                    lists.Add(response.Result.ReceiptBackEmailCollection.ToList());
                    lists.Add(response.Result.BuHuoEmailCollection.ToList());
                    lists.Add(response.Result.DiaoRuEmailCollection.ToList());
                    lists.Add(response.Result.DiaoChuEmailCollection.ToList());
                    lists.Add(response.Result.AdjustmentEmailCollection.ToList());
                    lists.Add(response.Result.AdjustmentAddEmailCollection.ToList());
                    lists.Add(response.Result.AdjustmentFrezzEmailCollection.ToList());
                    lists.Add(response.Result.MenDianZhiFaEmailCollection.ToList());
                    lists.Add(response.Result.o2oEmailCollection.ToList());
                    lists2 = response.Result.WMS_CustomerCollection.ToList();
                }
                return Json(new { code = 1, Result = lists, Result2 = lists2 });
            }
            catch (Exception ex)
            {
                return Json(new { code = 0, Result = lists, Result2 = lists2 });
            }

        }
        /// <summary>
        /// 获取EpackList邮件表格中的数据
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public JsonResult GetEpackListSendEmail(string StoreID, string CustomerID, string start_CompleteDate, string end_CompleteDate)
        {
            List<List<ReportInventory>> lists = new List<List<ReportInventory>>();
            List<WMS_Customer> lists2 = new List<WMS_Customer>();
            try
            {
                var response = new ReportManagementService().GetEpackListEmail(StoreID, CustomerID, start_CompleteDate, end_CompleteDate);
                if (response.IsSuccess)
                {
                    lists.Add(response.Result.InventoryCollection.ToList());
                    lists2 = response.Result.WMS_CustomerCollection.ToList();
                }
                return Json(new { code = 1, Result = lists, Result2 = lists2 });
            }
            catch (Exception ex)
            {
                return Json(new { code = 0, Result = lists, Result2 = lists2 });
            }

        }

        public ActionResult NikeReportSendEmail(long? customerID, long? warehouseID, long? warehouseAreaID, int? PageIndex)
        {
            InventoryViewModel vm = new InventoryViewModel();
            GetInventoryBySearchConditionRequest request = new GetInventoryBySearchConditionRequest();
            request.InventorySearchCondition = new ReportInventorySearchCondition();
            vm.SearchCondition = new ReportInventorySearchCondition();

            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID).FirstOrDefault();
            request.InventorySearchCondition.CustomerID = Convert.ToInt64(CustomerListID);
            var response = new ReportManagementService().GetWms_CustomerByCustomreID(request);
            if (response.IsSuccess)
            {
                vm.WMS_CustomerCollection = response.Result.WMS_CustomerCollection;
            }
            return View(vm);
        }
        /// <summary>
        /// 发送每日报表
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInputAttribute(false)]
        public string ReportSendEmail(string StoreID, string CustomerID, string ContentHtml, string start_CompleteDate, string end_CompleteDate)
        {
            var response = new ReportManagementService().GetNikeReport(StoreID, CustomerID, start_CompleteDate, end_CompleteDate);

            string StoreKey = "";
            string msg = "";
            try
            {
                if (response.IsSuccess)
                {
                    StoreKey = response.Result.WMS_CustomerCollection.Select(c => c.UserDef10).FirstOrDefault().Replace(" ", "");
                    msg = OrderReportExcel(response.Result.OrderReportCollection, StoreKey);
                    msg = DayInventoryReport(response.Result.InventoryCollection, StoreKey);
                    msg = ReceiptBackReport(response.Result.ReceiptBackReportCollection, StoreKey);
                    msg = ReceiptReport(response.Result.ReceiptReportCollection, StoreKey);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            if (msg == "")
            {
                try
                {
                    string filePath = Server.MapPath("\\" + "NikeReportSendEmail\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\"); // 文件路径
                    List<string> FilePath = new List<string>();
                    string Subject = StoreKey + "-CSC Daily Reports_" + DateTime.Now.ToString("yyyy-MM-dd") + "---" + StoreID;
                    string Body = ContentHtml;
                    FilePath.Add(filePath + StoreKey + "-CSC_出货日报表_" + DateTime.Now.ToString("yyyyMMdd") + "_" + StoreKey + ".xlsx");
                    FilePath.Add(filePath + StoreKey + "-CSC_每日库存_" + DateTime.Now.ToString("yyyyMMdd") + "_" + StoreKey + ".xlsx");
                    FilePath.Add(filePath + StoreKey + "-CSC_每日退货收货日报表_" + DateTime.Now.ToString("yyyyMMdd") + "_" + StoreKey + ".xlsx");
                    FilePath.Add(filePath + StoreKey + "-CSC_收货日报表_" + DateTime.Now.ToString("yyyyMMdd") + "_" + StoreKey + ".xlsx");
                    msg = EmailHelper.Send(Subject, Body, FilePath, response.Result.EmailConfig.MailToConfig, response.Result.EmailConfig.CCEmailConfig);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
                if (msg == "")
                {
                    msg = new ReportManagementService().InsertReportSendEmail(StoreID, CustomerID);
                }

            }

            return msg;
        }
        /// <summary>
        /// 发送EpackList
        /// </summary>
        /// <param name="StoreID"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInputAttribute(false)]
        public string EpackListReportSendEmail(string StoreID, string CustomerID, string ContentHtml, string DriverName, string DriverTel, string CarNo, string ExpectTime, string start_CompleteDate, string end_CompleteDate)
        {
            var response = new ReportManagementService().GetNikeEpackListReport(StoreID, CustomerID, DriverName, DriverTel, CarNo, ExpectTime, start_CompleteDate, end_CompleteDate);
            string StoreKey = "";
            string msg = "";
            try
            {
                if (response.IsSuccess)
                {
                    StoreKey = response.Result.WMS_CustomerCollection.Select(c => c.UserDef10).FirstOrDefault().Replace(" ", "");
                    msg = EpacklistReport(response.Result.NikePackageReportCollcetion);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            if (msg == "")
            {
                try
                {
                    string filePath = Server.MapPath("\\" + "NikeReportSendEmail\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\"); // 文件路径
                    List<string> FilePath = new List<string>();
                    string Subject = StoreKey + "-CSC Daily epacklist_" + DateTime.Now.ToString("yyyy-MM-dd") + "---" + StoreID;
                    string Body = ContentHtml;
                    FilePath.Add(filePath + "Epacklist_CRD" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx");
                    msg = EmailHelper.Send(Subject, Body, FilePath, response.Result.EmailConfig.MailToConfig, response.Result.EmailConfig.CCEmailConfig);
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
                if (msg == "")
                {
                    msg = new ReportManagementService().InsertEpackListSendEmail(StoreID, CustomerID, start_CompleteDate, end_CompleteDate);
                }
            }
            return msg;
        }
        /// <summary>
        /// 装箱清单报表
        /// </summary>
        /// <param name="NikePackageReportCollcetion"></param>
        /// <param name="StoreKey"></param>
        public string EpacklistReport(IEnumerable<NFSPrintBoxInfo> NikePackageReportCollcetion)
        {
            string msg = "";
            try
            {
                DataSet ds = new DataSet();
                foreach (var item2 in NikePackageReportCollcetion.GroupBy(c => new { c.OrderNumber, c.ShipmentNo, c.ShipToName })
                    .Select(a => new NFSPrintBoxInfo { OrderNumber = a.Key.OrderNumber, ShipmentNo = a.Key.ShipmentNo, ShipToName = a.Key.ShipToName, }))
                {
                    DataTable dt = new DataTable();
                    DataColumn dc = new DataColumn();

                    dc = dt.Columns.Add("PackageKey", typeof(string));
                    dc = dt.Columns.Add("BIN", typeof(string));
                    dc = dt.Columns.Add("Material", typeof(string));
                    dc = dt.Columns.Add("Size", typeof(string));
                    dc = dt.Columns.Add("Product", typeof(string));
                    dc = dt.Columns.Add("Quantity", typeof(string));
                    dc = dt.Columns.Add("Gender", typeof(string));
                    dc = dt.Columns.Add("Category", typeof(string));
                    dc = dt.Columns.Add("Description", typeof(string));

                    DataTable dto = new DataTable();
                    DataColumn dco = new DataColumn();
                    dco = dto.Columns.Add("PackageKey", typeof(string));
                    dco = dto.Columns.Add("BIN", typeof(string));
                    dco = dto.Columns.Add("Material", typeof(string));
                    dco = dto.Columns.Add("Size", typeof(string));
                    dco = dto.Columns.Add("Product", typeof(string));
                    dco = dto.Columns.Add("Quantity", typeof(string));
                    dco = dto.Columns.Add("Gender", typeof(string));
                    dco = dto.Columns.Add("Category", typeof(string));
                    dco = dto.Columns.Add("Description", typeof(string));

                    DataRow dro = dto.NewRow();
                    dro["PackageKey"] = item2.OrderNumber;
                    dro["BIN"] = item2.ShipmentNo;
                    dro["Material"] = item2.ShipToName;
                    dto.Rows.Add(dro);
                    dt.ImportRow(dro);

                    foreach (var item3 in NikePackageReportCollcetion.Where(c => c.OrderNumber == item2.OrderNumber).GroupBy(c => c.PackageNumber).Select(a => new NFSPrintBoxInfo { PackageNumber = a.Key }))
                    {
                        foreach (var item4 in NikePackageReportCollcetion.Where(c => c.OrderNumber == item2.OrderNumber && c.PackageNumber == item3.PackageNumber))
                        {
                            DataRow dr = dt.NewRow();
                            dr["PackageKey"] = item4.PackageNumber;
                            dr["BIN"] = "";
                            dr["Material"] = item4.Material;
                            dr["Size"] = item4.Size;
                            dr["Product"] = item4.Product;
                            dr["Quantity"] = item4.Quantity;
                            dr["Gender"] = item4.Gender;
                            dr["Category"] = item4.Category;
                            dr["Description"] = item4.MaterialDesc;
                            dt.Rows.Add(dr);
                        }
                        DataRow dr2 = dt.NewRow();
                        dr2["PackageKey"] = "页合计:";
                        dr2["BIN"] = NikePackageReportCollcetion.Where(c => c.OrderNumber == item2.OrderNumber && c.PackageNumber == item3.PackageNumber).Sum(c => c.Quantity);
                        dr2["Material"] = "";
                        dr2["Size"] = "";
                        dr2["Product"] = "";
                        dr2["Quantity"] = "";
                        dr2["Gender"] = "";
                        dr2["Category"] = "";
                        dr2["Description"] = "";
                        dt.Rows.Add(dr2);
                    }
                    DataRow dr3 = dt.NewRow();
                    dr3["PackageKey"] = "总合计：";
                    dr3["BIN"] = NikePackageReportCollcetion.Where(c => c.OrderNumber == item2.OrderNumber).Sum(c => c.Quantity);
                    DataRow dr4 = dt.NewRow();
                    dr4["PackageKey"] = "总箱数：";
                    dr4["BIN"] = NikePackageReportCollcetion.Where(c => c.OrderNumber == item2.OrderNumber).GroupBy(a => a.PackageNumber).Count();
                    dt.Rows.Add(dr3);
                    dt.Rows.Add(dr4);
                    dt.TableName = item2.OrderNumber;
                    ds.Tables.Add(dt);
                }

                EpacklistExcelFileToFolder(ds, "Epacklist_CRD" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx");
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        /// <summary>
        /// 将两个列不同的DataTable合并成一个新的DataTable
        /// </summary>
        /// <param name="dt1">表1</param>
        /// <param name="dt2">表2</param>
        /// <param name="DTName">合并后新的表名</param>
        /// <returns></returns>
        private DataTable UniteDataTable(DataTable dt1, DataTable dt2, string DTName)
        {
            DataTable dt3 = dt1.Clone();
            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                dt3.Columns.Add(dt2.Columns[i].ColumnName);
            }
            object[] obj = new object[dt3.Columns.Count];

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                dt1.Rows[i].ItemArray.CopyTo(obj, 0);
                dt3.Rows.Add(obj);
            }

            if (dt1.Rows.Count >= dt2.Rows.Count)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        dt3.Rows[i][j + dt1.Columns.Count] = dt2.Rows[i][j].ToString();
                    }
                }
            }
            else
            {
                DataRow dr3;
                for (int i = 0; i < dt2.Rows.Count - dt1.Rows.Count; i++)
                {
                    dr3 = dt3.NewRow();
                    dt3.Rows.Add(dr3);
                }
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        dt3.Rows[i][j + dt1.Columns.Count] = dt2.Rows[i][j].ToString();
                    }
                }
            }
            dt3.TableName = DTName; //设置DT的名字
            return dt3;
        }

        /// <summary>
        /// 日出货报表
        /// </summary>    
        public string OrderReportExcel(IEnumerable<ReprotTableIn> OrderReportCollection, string StoreKey)
        {
            string msg = "";
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataColumn dc = new DataColumn();
                DataTable dt2 = new DataTable();
                DataColumn dc2 = new DataColumn();
                dc = dt.Columns.Add("序号", typeof(string));
                dc = dt.Columns.Add("订单类型", typeof(string));
                dc = dt.Columns.Add("订单号", typeof(string));
                dc = dt.Columns.Add("SKU", typeof(string));
                dc = dt.Columns.Add("Article", typeof(string));
                dc = dt.Columns.Add("SIZE", typeof(string));
                dc = dt.Columns.Add("实出数量", typeof(string));
                dc = dt.Columns.Add("货物类型", typeof(string));
                dc = dt.Columns.Add("安全扣", typeof(string));
                dc = dt.Columns.Add("衣架", typeof(string));
                dc = dt.Columns.Add("实际出货日期", typeof(string));
                dc = dt.Columns.Add("门店代码", typeof(string));
                dc = dt.Columns.Add("门店名称", typeof(string));

                dc2 = dt2.Columns.Add("门店代码", typeof(string));
                dc2 = dt2.Columns.Add("门店名称", typeof(string));
                dc2 = dt2.Columns.Add("实出数量", typeof(string));
                int index = 1;
                foreach (var item in OrderReportCollection)
                {
                    DataRow dr = dt.NewRow();
                    dr["序号"] = index;
                    dr["订单类型"] = item.OrderType;
                    dr["订单号"] = item.ExternOrderNumber;
                    dr["SKU"] = item.SKU;
                    dr["Article"] = item.Article;
                    dr["SIZE"] = item.Size;
                    dr["实出数量"] = item.Qty;
                    dr["货物类型"] = item.GoodsType;
                    dr["安全扣"] = item.SafeLock;
                    dr["衣架"] = item.Hanger;
                    dr["实际出货日期"] = item.CompleteDate;
                    dr["门店代码"] = item.str4;
                    dr["门店名称"] = item.Company;
                    dt.Rows.Add(dr);
                    index++;
                }

                DataRow dr2 = dt2.NewRow();
                dr2["门店代码"] = OrderReportCollection.Select(c => c.str4).FirstOrDefault();
                dr2["门店名称"] = StoreKey;
                dr2["实出数量"] = OrderReportCollection.Sum(c => c.Qty);
                dt2.Rows.Add(dr2);
                dt.TableName = "门店补货";
                dt2.TableName = "汇总";
                ds.Tables.Add(dt);
                ds.Tables.Add(dt2);
                DayOrderReportExcelFileToFolder(ds, StoreKey + "-CSC_出货日报表_" + DateTime.Now.ToString("yyyyMMdd") + "_" + StoreKey + ".xlsx", StoreKey);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        /// <summary>
        /// 每日库存报表
        /// </summary>
        /// <param name="InventoryCollection"></param>
        /// <param name="StoreKey"></param>
        public string DayInventoryReport(IEnumerable<ReportInventory> InventoryCollection, string StoreKey)
        {
            string msg = "";
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataColumn dc = new DataColumn();
                dc = dt.Columns.Add("序号", typeof(string));
                dc = dt.Columns.Add("SKU", typeof(string));
                dc = dt.Columns.Add("Article", typeof(string));
                dc = dt.Columns.Add("尺码", typeof(string));
                dc = dt.Columns.Add("BU", typeof(string));
                dc = dt.Columns.Add("库存数量", typeof(string));
                dc = dt.Columns.Add("可用数量", typeof(string));
                dc = dt.Columns.Add("Gender", typeof(string));
                dc = dt.Columns.Add("Category", typeof(string));
                int index = 1;
                foreach (var item in InventoryCollection)
                {
                    DataRow dr = dt.NewRow();
                    dr["序号"] = index;
                    dr["SKU"] = item.SKU;
                    dr["Article"] = item.Article;
                    dr["尺码"] = item.Size;
                    dr["BU"] = item.BU;
                    dr["库存数量"] = item.Qty;
                    dr["可用数量"] = item.InventoryQty;
                    dr["Gender"] = item.Gender;
                    dr["Category"] = item.Category;
                    dt.Rows.Add(dr);
                    index++;
                }
                dt.TableName = "正常库存";
                ds.Tables.Add(dt);
                DayInventoryReportExcelFileToFolder(ds, StoreKey + "-CSC_每日库存_" + DateTime.Now.ToString("yyyyMMdd") + "_" + StoreKey + ".xlsx");
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }



        [HttpGet]   //来货预检差异报表
        public ActionResult FinancialStatements(long? customerID, long? warehouseID, int? PageIndex)
        {
            IndexViewModel vm = new IndexViewModel();
            vm.ProjectRoleID = base.UserInfo.ProjectID;
            vm.SearchCondition = new ReceiptSearchCondition();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.SearchCondition.StartCompleteDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString()).AddDays(-10);
            vm.SearchCondition.EndCompleteDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.SearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();

            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            GetReceiptByConditionRequest request = new GetReceiptByConditionRequest();
            request.SearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            return View(vm);
        }

        [HttpPost]   //来货预检差异报表
        public ActionResult FinancialStatements(IndexViewModel vm, int? PageIndex, string Action)
        {
            vm.ProjectRoleID = base.UserInfo.ProjectID;

            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.ReceiptTypes = st;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            ViewBag.CustomerList = CustomerList;
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                StringBuilder sb = new StringBuilder();

                foreach (var i in CustomerListID)
                {
                    sb.Append("" + i + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.SearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.SearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            vm.SearchCondition.UserType = base.UserInfo.UserType;

            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getReceiptByConditionRequest = new GetReceiptByConditionRequest();

            getReceiptByConditionRequest.SearchCondition = vm.SearchCondition;
            getReceiptByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getReceiptByConditionRequest.PageIndex = PageIndex ?? 0;

            Response<GetReceiptDetailByConditionResponse> getReceiptByConditionResponse = new Response<GetReceiptDetailByConditionResponse>();

            if (Action == "导出")
            {
                getReceiptByConditionResponse = new ReceiptManagementService().GetFinancialStatementsExport(getReceiptByConditionRequest);
            }
            else
            {
                getReceiptByConditionResponse = new ReceiptManagementService().GetFinancialStatements(getReceiptByConditionRequest);
            }
            if (getReceiptByConditionResponse.IsSuccess)
            {

                vm.ReceiptDetailCollection2 = getReceiptByConditionResponse.Result.ReceiptDetailCollection3;
                vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
                vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
                if (Action == "导出")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<style>td{mso-number-format:\"\\@\";}</style>");
                    sb.Append("<table border='1' cellspacing='0' cellpadding='0'>");
                    sb.Append("<tr><td>行号</td><td>预入库单号</td><td>外部单号</td><td>入库日期</td><td>箱号</td><td>产品编码</td><td>期望数量</td><td>扫描数量</td><td>差异数量</td><td>门店代码</td></tr>");
                    int i = 0;
                    foreach (var item in vm.ReceiptDetailCollection2)
                    {
                        sb.Append("<tr><td>" + i++ + "</td><td>" + item.ASNNumber.ToString() + "</td><td>" + item.ExternReceiptNumber.ToString() + "</td>");
                        sb.Append("<td>" + item.ReceiptDate.ToString() + "</td><td>" + item.str2.ToString() + "</td><td>" + item.SKU.ToString() + "</td><td>" + item.QtyExpected.ToString() + "</td><td>" + item.QtyReceived.ToString() + "</td><td>" + (item.QtyReceived - item.QtyExpected).ToString() + "</td><td>" + item.str3.ToString() + "</td></tr>");
                    }
                    sb.Append("</table>");
                    ss.HttpResponse Response;
                    Response = ss.HttpContext.Current.Response;
                    Response.Charset = "UTF-8";
                    Response.HeaderEncoding = Encoding.UTF8;
                    Response.AppendHeader("content-disposition", "attachment;filename=\"" + ss.HttpUtility.UrlEncode("来货预检扫描差异_" + DateTime.Now.ToShortDateString(), Encoding.UTF8) + ".xls\"");
                    Response.ContentEncoding = Encoding.UTF8;
                    Response.ContentType = "application/ms-excel";
                    Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sb.ToString());
                    Response.Flush();
                    Response.End();


                }
                vm.ProjectRoleID = base.UserInfo.ProjectID;
            }
            this.GenQueryReceiptViewModel(vm);
            return View(vm);
        }


        /// <summary>
        /// 每日退货收货日报表
        /// </summary>
        /// <param name="ReceiptBackReportCollection"></param>
        /// <param name="StoreKey"></param>
        public string ReceiptBackReport(IEnumerable<ReportReceiptReport> ReceiptBackReportCollection, string StoreKey)
        {
            string msg = "";
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataColumn dc = new DataColumn();
                DataTable dt2 = new DataTable();
                DataColumn dc2 = new DataColumn();
                dc = dt.Columns.Add("序号", typeof(string));
                dc = dt.Columns.Add("SKU", typeof(string));
                dc = dt.Columns.Add("Article", typeof(string));
                dc = dt.Columns.Add("尺码", typeof(string));
                dc = dt.Columns.Add("预计数量", typeof(string));
                dc = dt.Columns.Add("实收数量", typeof(string));
                dc = dt.Columns.Add("货物类型", typeof(string));
                dc = dt.Columns.Add("到货日期", typeof(string));

                dc2 = dt2.Columns.Add("门店代码", typeof(string));
                dc2 = dt2.Columns.Add("门店名称", typeof(string));
                dc2 = dt2.Columns.Add("实出数量", typeof(string));
                dc2 = dt2.Columns.Add("备注", typeof(string));
                int index = 1;
                foreach (var item in ReceiptBackReportCollection)
                {
                    DataRow dr = dt.NewRow();
                    dr["序号"] = index;
                    dr["SKU"] = item.SKU;
                    dr["Article"] = item.Article;
                    dr["尺码"] = item.Size;
                    dr["预计数量"] = item.QtyExpected;
                    dr["实收数量"] = item.QtyReceived;
                    dr["货物类型"] = item.GoodsType;
                    dr["到货日期"] = item.CompleteDate == null ? "" : item.CompleteDate;
                    dt.Rows.Add(dr);
                    index++;
                }
                DataRow dr2 = dt2.NewRow();
                dr2["门店代码"] = ReceiptBackReportCollection.Select(c => c.str3).FirstOrDefault();
                dr2["门店名称"] = StoreKey;
                dr2["实出数量"] = ReceiptBackReportCollection.Sum(c => c.QtyReceived);
                dr2["备注"] = "A品";
                dt2.Rows.Add(dr2);
                dt.TableName = "收货报表";
                dt2.TableName = "汇总";
                ds.Tables.Add(dt);
                ds.Tables.Add(dt2);
                DayReceiptReportExcelFileToFolder(ds, StoreKey + "-CSC_每日退货收货日报表_" + DateTime.Now.ToString("yyyyMMdd") + "_" + StoreKey + ".xlsx", StoreKey);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        /// <summary>
        /// 收货日报表
        /// </summary>
        /// <param name="ReceiptReportCollection"></param>
        /// <param name="StoreKey"></param>
        public string ReceiptReport(IEnumerable<ReportReceiptReport> ReceiptReportCollection, string StoreKey)
        {
            string msg = "";
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataColumn dc = new DataColumn();
                DataTable dt2 = new DataTable();
                DataColumn dc2 = new DataColumn();
                dc = dt.Columns.Add("序号", typeof(string));
                dc = dt.Columns.Add("SKU", typeof(string));
                dc = dt.Columns.Add("Article", typeof(string));
                dc = dt.Columns.Add("尺码", typeof(string));
                dc = dt.Columns.Add("预计数量", typeof(string));
                dc = dt.Columns.Add("实收数量", typeof(string));
                dc = dt.Columns.Add("货物类型", typeof(string));
                dc = dt.Columns.Add("到货日期", typeof(string));

                dc2 = dt2.Columns.Add("门店代码", typeof(string));
                dc2 = dt2.Columns.Add("门店名称", typeof(string));
                dc2 = dt2.Columns.Add("实出数量", typeof(string));
                dc2 = dt2.Columns.Add("备注", typeof(string));
                int index = 1;
                foreach (var item in ReceiptReportCollection)
                {
                    DataRow dr = dt.NewRow();
                    dr["序号"] = index;
                    dr["SKU"] = item.SKU;
                    dr["Article"] = item.Article;
                    dr["尺码"] = item.Size;
                    dr["预计数量"] = item.QtyExpected;
                    dr["实收数量"] = item.QtyReceived;
                    dr["货物类型"] = item.GoodsType;
                    dr["到货日期"] = item.CompleteDate;
                    dt.Rows.Add(dr);
                    index++;
                }
                DataRow dr2 = dt2.NewRow();
                dr2["门店代码"] = ReceiptReportCollection.Select(c => c.str3).FirstOrDefault();
                dr2["门店名称"] = StoreKey;
                dr2["实出数量"] = ReceiptReportCollection.Sum(c => c.QtyReceived);
                dr2["备注"] = "A品";
                dt2.Rows.Add(dr2);
                dt.TableName = "收货报表";
                dt2.TableName = "汇总";
                ds.Tables.Add(dt);
                ds.Tables.Add(dt2);
                DayReceiptReportExcelFileToFolder(ds, StoreKey + "-CSC_收货日报表_" + DateTime.Now.ToString("yyyyMMdd") + "_" + StoreKey + ".xlsx", StoreKey);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
        /// <summary>
        /// nike装箱清单报表
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="fileName"></param>
        /// <param name="OrderNumber"></param>
        /// <param name="ExternOrdernumber"></param>
        /// <param name="company"></param>
        public void EpacklistExcelFileToFolder(DataSet ds, string fileName)
        {
            try
            {
                string filePath = Server.MapPath("\\" + "NikeReportSendEmail\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\" + fileName); // 文件路径

                // 1.检测是否存在文件夹，若不存在就建立个文件夹
                string DirectoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(DirectoryName))
                {
                    Directory.CreateDirectory(DirectoryName);
                }

                ExportDataToExcelHelper.ExportEpacklistExcelToURL(ds, filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 每日出货报表
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="fileName"></param>
        public void DayOrderReportExcelFileToFolder(DataSet ds, string fileName, string StoreKey)
        {
            try
            {
                string filePath = Server.MapPath("\\" + "NikeReportSendEmail\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\" + fileName); // 文件路径

                // 1.检测是否存在文件夹，若不存在就建立个文件夹
                string DirectoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(DirectoryName))
                {
                    Directory.CreateDirectory(DirectoryName);
                }

                ExportDataToExcelHelper.ExportDayOrderReportExcelToURL(ds, filePath, StoreKey);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 每日库存报表
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="fileName"></param>
        public void DayInventoryReportExcelFileToFolder(DataSet ds, string fileName)
        {
            try
            {
                string filePath = Server.MapPath("\\" + "NikeReportSendEmail\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\" + fileName); // 文件路径

                // 1.检测是否存在文件夹，若不存在就建立个文件夹
                string DirectoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(DirectoryName))
                {
                    Directory.CreateDirectory(DirectoryName);
                }

                ExportDataToExcelHelper.ExportDayInventoryExcelToURL(ds, filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Nike每日收货及退货报表
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="fileName"></param>
        public void DayReceiptReportExcelFileToFolder(DataSet ds, string fileName, string StoreKey)
        {
            try
            {
                string filePath = Server.MapPath("\\" + "NikeReportSendEmail\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\" + fileName); // 文件路径

                // 1.检测是否存在文件夹，若不存在就建立个文件夹
                string DirectoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(DirectoryName))
                {
                    Directory.CreateDirectory(DirectoryName);
                }

                ExportDataToExcelHelper.ExportDayReceiptReportExcelToURL(ds, filePath, StoreKey);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 生成的EXCEL存到文件夹下
        /// </summary>
        public void ExcelFileToFolder(DataSet ds, string fileName)
        {
            try
            {
                string filePath = Server.MapPath("\\" + "NikeReportSendEmail\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\" + fileName); // 文件路径

                // 1.检测是否存在文件夹，若不存在就建立个文件夹
                string DirectoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(DirectoryName))
                {
                    Directory.CreateDirectory(DirectoryName);
                }

                ExportDataToExcelHelper.ExportExcelToURL(ds, filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public ActionResult InventoryStorer(long? customerID, long? warehouseID, long? warehouseAreaID, int? PageIndex)
        {
            InventoryViewModel vm = new InventoryViewModel();

            vm.ProjectRoleID = base.UserInfo.ProjectID;
            vm.SearchCondition = new ReportInventorySearchCondition();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                    else
                    {
                        vm.SearchCondition.CustomerID = 0;
                    }
                }
            }

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == 0)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            if (WarehouseList != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            ViewBag.WarehouseList = WarehouseList;

            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.SearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();
            var areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).
                Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });

            //获取这个用户可以看到的库区
            var userarea = ApplicationConfigHelper.GetAllUser_Area().Where(m => m.UserID == base.UserInfo.ID);
            IEnumerable<SelectListItem> arealist = null;
            IEnumerable<WMS_User_Area_Mapping> areamaplist = null;
            //普通门店
            if (userarea != null && userarea.Any())
            {
                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem()
                {
                    Value = areas.Where(m => m.Value == userarea.Select(n => n.AreaName).FirstOrDefault()).FirstOrDefault().Value,
                    Text = userarea.Select(m => m.Company).FirstOrDefault()
                });
                arealist = list;
                //arealist = areas.Where(m => m.Value == userarea.Select(n => n.AreaName).FirstOrDefault());
                //vm.SearchCondition.str1 = arealist.Select(m => m.Text).FirstOrDefault();
                vm.SearchCondition.Area = arealist.Select(m => m.Value).FirstOrDefault();
            }
            //门店管理员
            else
            {
                areamaplist = ApplicationConfigHelper.GetAllUser_Area().Where(m => m.WarehouseID == s);
                List<SelectListItem> list = new List<SelectListItem>();
                IEnumerable<SelectListItem> list2 = areas.Where(m => areamaplist.Select(n => n.AreaName).Distinct().Contains(m.Value));
                foreach (var item in list2)
                {
                    list.Add(new SelectListItem()
                    {
                        Value = item.Value,
                        Text = areamaplist.Where(m => m.AreaName == item.Value).FirstOrDefault().Company
                    });
                }
                arealist = list;
                //vm.SearchCondition.str20 = new JavaScriptSerializer().Serialize(areamaplist);
            }
            //ViewBag.AreaMapList = areamaplist;
            ViewBag.AreaList = arealist;
            GetInventoryBySearchConditionRequest request = new GetInventoryBySearchConditionRequest();
            request.InventorySearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            if (arealist != null)
            {
                var response = new ReportManagementService().GetInventoryStorerBySearchCondition(request);
                if (response.IsSuccess)
                {
                    vm.ReportInventoryCollection = response.Result.InventoryCollection;
                    vm.PageIndex = response.Result.PageIndex;
                    vm.PageCount = response.Result.PageCount;
                }
            }
            return View(vm);
        }

        /// <summary>
        /// 门店库存查询
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InventoryStorer(InventoryViewModel vm, int? PageIndex, string Action)
        {
            vm.ProjectRoleID = base.UserInfo.ProjectID;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            if (vm.SearchCondition.Warehouse != null)
            {
                //vm.AdjustmentCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == vm.AdjustmentCondition.Warehouse.ToString()).First().Value + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            ViewBag.WarehouseList = WarehouseList;

            var areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID)).Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            var userarea = ApplicationConfigHelper.GetAllUser_Area().Where(m => m.UserID == base.UserInfo.ID);//获取这个用户可以看到的库区
            IEnumerable<SelectListItem> arealist = null;
            IEnumerable<WMS_User_Area_Mapping> areamaplist = null;
            if (userarea != null && userarea.Any())
            {
                List<SelectListItem> list = new List<SelectListItem>();
                list.Add(new SelectListItem()
                {
                    Value = areas.Where(m => m.Value == userarea.Select(n => n.AreaName).FirstOrDefault()).FirstOrDefault().Value,
                    Text = userarea.Select(m => m.Company).FirstOrDefault()
                });
                arealist = list;
                //arealist = areas.Where(m => m.Value == userarea.Select(n => n.AreaName).FirstOrDefault());
            }
            else
            {
                areamaplist = ApplicationConfigHelper.GetAllUser_Area().Where(m => m.WarehouseID == vm.SearchCondition.Warehouse.ObjectToInt64());
                List<SelectListItem> list = new List<SelectListItem>();
                IEnumerable<SelectListItem> list2 = areas.Where(m => areamaplist.Select(n => n.AreaName).Distinct().Contains(m.Value));
                foreach (var item in list2)
                {
                    list.Add(new SelectListItem()
                    {
                        Value = item.Value,
                        Text = areamaplist.Where(m => m.AreaName == item.Value).FirstOrDefault().Company
                    });
                }
                arealist = list;

                //arealist = areas.Where(m => areamaplist.Select(n => n.AreaName).Distinct().Contains(m.Value));
            }

            ViewBag.AreaMapList = areamaplist;
            ViewBag.AreaList = arealist;

            GetInventoryBySearchConditionRequest request = new GetInventoryBySearchConditionRequest();
            request.InventorySearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>();
            if (Action == "导出")
            {
                response = new ReportManagementService().ExportInventoryStorerBySearchCondition(request);
            }
            else
            {
                response = new ReportManagementService().GetInventoryStorerBySearchCondition(request);
            }
            if (response.IsSuccess)
            {
                vm.ReportInventoryCollection = response.Result.InventoryCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {
                    ExportStorerInventory(response.Result);
                    //IEnumerable<Column> columnReceipt;
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Inventory").Count() == 0)
                    //{
                    //    columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    //}
                    //else
                    //{
                    //    columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    //}                    
                    //ExportInventory(response.Result, columnReceipt);                   
                }
            }
            return View(vm);
        }

        /// <summary>
        /// 门店库存报表导出方法
        /// </summary>
        private void ExportStorerInventory(GetInventoryBySearchConditionResponse response)
        {
            IEnumerable<ReportInventory> inventorys = response.InventoryCollection;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("序号", typeof(string));
            dt.Columns.Add("SKU", typeof(string));
            dt.Columns.Add("Article", typeof(string));
            dt.Columns.Add("尺码", typeof(string));
            dt.Columns.Add("BU", typeof(string));
            dt.Columns.Add("库存数量", typeof(string));
            dt.Columns.Add("可用数量", typeof(string));
            dt.Columns.Add("Gender", typeof(string));
            dt.Columns.Add("Category", typeof(string));

            dt.Columns.Add("sku-size", typeof(string));
            dt.Columns.Add("TTL库存", typeof(string));
            dt.Columns.Add("店铺库存", typeof(string));
            dt.Columns.Add("SILO", typeof(string));
            dt.Columns.Add("HD", typeof(string));
            dt.Columns.Add("QS", typeof(string));
            dt.Columns.Add("OMD", typeof(string));
            dt.Columns.Add("Desc", typeof(string));
            dt.Columns.Add("MSRP", typeof(string));
            dt.Columns.Add("店铺货品反馈", typeof(string));
            dt.Columns.Add("REMARK", typeof(string));

            inventorys.Each((i, inven) =>
            {
                DataRow row = dt.NewRow();

                row["序号"] = (i + 1).ToString();
                row["SKU"] = inven.SKU;
                row["Article"] = inven.Article;
                row["尺码"] = inven.Size;
                row["BU"] = inven.BU;
                row["库存数量"] = inven.Qty;
                row["可用数量"] = inven.InventoryQty;
                row["Gender"] = inven.Gender;
                row["Category"] = inven.str4;
                row["sku-size"] = "";
                row["TTL库存"] = "";
                row["店铺库存"] = "";
                row["SILO"] = "";
                row["HD"] = "";
                row["QS"] = "";
                row["OMD"] = "";
                row["Desc"] = "";
                row["MSRP"] = "";
                row["店铺货品反馈"] = "";
                row["REMARK"] = "";

                dt.Rows.Add(row);
            });
            dt.TableName = "正常库存";
            ds.Tables.Add(dt);
            ExportDataToExcelHelper.ExportExcel(ds.Tables[0], "门店实时库存" + DateTime.Now.ToString("yyyy-MM-ddHHmmss"), "正常库存");
        }


        /// <summary>
        /// 工时统计
        /// </summary>
        /// <returns></returns>
        public ActionResult WorkingStatis(long? customerID, long? warehouseID, int? PageIndex)
        {
            WorkingViewModel vm = new WorkingViewModel();
            vm.ProjectRoleID = base.UserInfo.ProjectID;
            vm.SearchCondition = new WorkingSearchCondition();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            vm.SearchCondition.BeginCreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd HH:mm:ss"));
            vm.SearchCondition.EndCreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ObjectToNullableDateTime();

            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                    else
                    {
                        vm.SearchCondition.CustomerID = 0;
                    }
                }
            }

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == 0)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            if (WarehouseList != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            ViewBag.WarehouseList = WarehouseList;
            GetWorkingStatisRequest request = new GetWorkingStatisRequest();
            request.WorkingSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;

            var response = new ReportManagementService().GetWorkingStatisBySearchCondition(request);
            if (response.IsSuccess)
            {
                vm.WorkingCollection = response.Result.WorkingStatisCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            return View(vm);
        }

        /// <summary>
        ///工时统计post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WorkingStatis(WorkingViewModel vm, int? PageIndex, string Action)
        {
            vm.ProjectRoleID = base.UserInfo.ProjectID;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            if (vm.SearchCondition.WarehouseName != null)
            {
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            ViewBag.WarehouseList = WarehouseList;

            GetWorkingStatisRequest request = new GetWorkingStatisRequest();
            request.WorkingSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            Response<GetWorkingStatisResponse> response = new Response<GetWorkingStatisResponse>();

            if (Action == "导出")
            {
                response = new ReportManagementService().GetWorkingStatisBySearchCondition(request);
            }
            else
            {
                response = new ReportManagementService().GetWorkingStatisBySearchCondition(request);
            }
            if (response.IsSuccess)
            {
                vm.WorkingCollection = response.Result.WorkingStatisCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {
                    //ExportStorerInventory(response.Result);
                }
            }
            return View(vm);
        }

        [HttpPost]
        public JsonResult CreateWorkingStatis(string requestData)
        {
            int code = 401;
            string msg = string.Empty;
            try
            {
                WorkingSearchCondition workingSearch = new WorkingSearchCondition();
                workingSearch = new JavaScriptSerializer().Deserialize<WorkingSearchCondition>(requestData);
                if (workingSearch != null)
                {
                    if (workingSearch.CustomerID == 0 || workingSearch.CustomerID == null || workingSearch.WarehouseID == 0 || workingSearch.StatisDate == "")
                    {
                        msg = "传入的数据有误！";
                        code = 402;
                    }
                    else
                    {
                        //插入数据库
                        workingSearch.StatisDate = Convert.ToDateTime(workingSearch.StatisDate).ToString("yyyy-MM-dd");
                        workingSearch.WorkingNumber = Guid.NewGuid().ToString("N").ToUpper();//弄一个唯一Guid
                        workingSearch.Creator = base.UserInfo.Name;
                        bool result = new ReportManagementService().CreateWorkingStatis(workingSearch, out msg);
                        if (result && msg == "")
                        {
                            code = 0;
                        }
                        else
                        {
                            code = 402;
                        }
                    }

                }
                else
                {
                    msg = "传入的数据有误！";
                    code = 402;
                }
            }
            catch (Exception ex)
            {
                code = 402;
                msg = ex.Message.ToJsonString();
            }
            return Json(new { code = code, msg = msg });
        }

        [HttpPost]
        public JsonResult UpdateWorkingStatis(string requestData)
        {
            int code = 401;
            string msg = string.Empty;
            try
            {
                WorkingSearchCondition workingSearch = new WorkingSearchCondition();
                workingSearch = new JavaScriptSerializer().Deserialize<WorkingSearchCondition>(requestData);
                if (workingSearch != null)
                {
                    if (workingSearch.ID == 0 || workingSearch.WorkHour == 0 || workingSearch.PersonNumber == 0)
                    {
                        msg = "传入的数据有误！";
                        code = 402;
                    }
                    else
                    {
                        //插入数据库                        
                        workingSearch.Creator = base.UserInfo.Name;
                        bool result = new ReportManagementService().UpdateWorkingStatis(workingSearch, out msg);
                        if (result && msg == "")
                        {
                            code = 0;
                        }
                        else
                        {
                            code = 402;
                        }
                    }

                }
                else
                {
                    msg = "传入的数据有误！";
                    code = 402;
                }
            }
            catch (Exception ex)
            {
                code = 402;
                msg = ex.Message.ToJsonString();
            }
            return Json(new { code = code, msg = msg });
        }


        [HttpGet]
        public ActionResult Inventory(long? customerID, long? warehouseID, long? warehouseAreaID, int? PageIndex)
        {
            InventoryViewModel vm = new InventoryViewModel();
            #region 获取品级下拉框
            IEnumerable<WMSConfig> goodsTypes = null;
            try
            {
                goodsTypes = ApplicationConfigHelper.GetWMS_Config("ProductLevel_" + base.UserInfo.ProjectName);
            }
            catch
            {
            }
            if (goodsTypes == null)
            {
                goodsTypes = ApplicationConfigHelper.GetWMS_Config("GoodsTypes");

            }
            List<SelectListItem> st_goodsType = new List<SelectListItem>();
            foreach (WMSConfig w in goodsTypes)
            {
                st_goodsType.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.GoodsTypes = st_goodsType;
            #endregion
            vm.ProjectRoleID = base.UserInfo.ProjectID;
            vm.SearchCondition = new ReportInventorySearchCondition();
            //vm.SearchCondition.StartCreateTime = Convert.ToDateTime(DateTime.Now.ToString()).AddDays(-10);
            //vm.SearchCondition.EndCreateTime = Convert.ToDateTime(DateTime.Now.ToString());
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                    else
                    {
                        vm.SearchCondition.CustomerID = 0;
                    }
                }
            }
            IEnumerable<Column> columnReceipt;
            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Inventory").Count() == 0)
            {
                columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
            }
            else
            {
                columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
            }
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == 0)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.SearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            //}
            if (WarehouseList != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            ViewBag.WarehouseList = WarehouseList;
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("InventoryTypes_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("InventoryTypes");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            vm.InventoryTypes = st;
            GetInventoryBySearchConditionRequest request = new GetInventoryBySearchConditionRequest();
            request.InventorySearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            #region 查询方法
            //var response = new ReportManagementService().GetInventoryBySearchCondition(request);
            //if (response.IsSuccess)
            //{
            //    vm.ReportInventoryCollection = response.Result.InventoryCollection;
            //    vm.PageIndex = response.Result.PageIndex;
            //    vm.PageCount = response.Result.PageCount;
            //}
            #endregion
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        [HttpPost]   //库存
        public ActionResult Inventory(InventoryViewModel vm, int? PageIndex, string Action)
        {
            vm.ProjectRoleID = base.UserInfo.ProjectID;
            #region 获取品级下拉框
            IEnumerable<WMSConfig> goodsTypes = null;
            try
            {
                goodsTypes = ApplicationConfigHelper.GetWMS_Config("ProductLevel_" + base.UserInfo.ProjectName);
            }
            catch
            {
            }
            if (goodsTypes == null)
            {
                goodsTypes = ApplicationConfigHelper.GetWMS_Config("GoodsTypes");

            }
            List<SelectListItem> st_goodsType = new List<SelectListItem>();
            foreach (WMSConfig w in goodsTypes)
            {
                st_goodsType.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.GoodsTypes = st_goodsType;

            //vm.SearchCondition.StartCreateTime = Convert.ToDateTime(DateTime.Now.ToString()).AddDays(-10);
            //vm.SearchCondition.EndCreateTime = Convert.ToDateTime(DateTime.Now.ToString());
            #endregion
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            //ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID)).Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            //}
            if (vm.SearchCondition.Warehouse != null)
            {
                //vm.AdjustmentCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == vm.AdjustmentCondition.Warehouse.ToString()).First().Value + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }
            IEnumerable<Column> columnReceipt;
            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Inventory").Count() == 0)
            {
                columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
            }
            else
            {
                columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
            }
            ViewBag.WarehouseList = WarehouseList;
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("InventoryTypes_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("InventoryTypes");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            vm.InventoryTypes = st;
            GetInventoryBySearchConditionRequest request = new GetInventoryBySearchConditionRequest();
            request.InventorySearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>();
            DataTable dt2 = new DataTable();
            if (Action == "导出" || Action == "快速导出")
            {
                response = new ReportManagementService().ExportInventoryBySearchCondition(request);
                //dt2 = new ReportManagementService().ExportInventoryBySearchCondition2(request);
                //response.IsSuccess = true;
            }
            else
            {
                response = new ReportManagementService().GetInventoryBySearchCondition(request);
            }
            if (response.IsSuccess)
            {
                vm.ReportInventoryCollection = response.Result.InventoryCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {
                    //IEnumerable<Column> columnReceipt;
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Inventory").Count() == 0)
                    //{
                    //    columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    //}
                    //else
                    //{
                    //    columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    //}
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    ExportInventory(response.Result, columnReceipt);
                    //ExportInventory2(dt2, columnReceipt);
                }
                else if (Action == "快速导出")
                {
                    //IEnumerable<Column> columnReceipt;
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Inventory").Count() == 0)
                    //{
                    //    columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    //}
                    //else
                    //{
                    //    columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    //}
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    FastExportInventory(response.Result, columnReceipt);
                    //ExportInventory2(dt2, columnReceipt);
                    return Redirect("/WMS/ReportManagement/Inventory");
                }
            }
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        /// <summary>
        /// 库存汇总报表  爱库存专用 （呵呵呵）
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InventorySummary(int? PageIndex, long? customerID, int? IslocationBy = 0)
        {
            //Session["SearchConditionModel"] = null;

            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            InventoryViewModel vm = new InventoryViewModel();

            vm.InventorySummarySearchCondition = new ReportInventorySummarySearchCondition();
            vm.InventorySummarySearchCondition.IsLocationBy = IslocationBy;//是否根据库位分组查询
            vm.ProjectRoleID = base.UserInfo.ProjectID;

            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;

            GetInventoryBySearchConditionRequest request = new GetInventoryBySearchConditionRequest();

            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.InventorySummarySearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.InventorySummarySearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.InventorySummarySearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                StringBuilder sb = new StringBuilder();

                foreach (var i in CustomerListID)
                {
                    sb.Append("" + i + ",");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySummarySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySummarySearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.InventorySummarySearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.InventorySummarySearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID)
                .FirstOrDefault();

            //ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.InventorySummarySearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });

            if (WarehouseList != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySummarySearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            ViewBag.WarehouseList = WarehouseList;

            if (IslocationBy != null)
            {
                vm.InventorySummarySearchCondition.IsLocationBy = (int)IslocationBy;
            }
            request.InventorysummarySearchCondition = vm.InventorySummarySearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }

        /// <summary>
        /// 库存汇总查询，爱库存用  (post)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InventorySummary(InventoryViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.ProjectRoleID = base.UserInfo.ProjectID;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.InventorySummarySearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                StringBuilder sb = new StringBuilder();

                foreach (var i in CustomerListID)
                {
                    sb.Append("" + i + ",");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySummarySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySummarySearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.InventorySummarySearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.InventorySummarySearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();
            //ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.InventorySummarySearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (vm.InventorySummarySearchCondition.WarehouseID != 0)
            {
                //vm.AdjustmentCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == vm.AdjustmentCondition.Warehouse.ToString()).First().Value + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySummarySearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }

            ViewBag.WarehouseList = WarehouseList;

            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var request = new GetInventoryBySearchConditionRequest();

            request.InventorysummarySearchCondition = vm.InventorySummarySearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>();
            if (Action == "导出")
            {
                response = new ReportManagementService().ExportInventorySummaryBySearchCondition(request);
            }
            else
            {
                response = new ReportManagementService().GetInventorysummaryBySearchCondition(request);
            }
            if (response.IsSuccess)
            {
                vm.ReportInventorySummaryCollection = response.Result.InventorySummaryCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {
                    IEnumerable<Column> columnReceipt;

                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.InventorySummarySearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_InventorySummary").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_InventorySummary").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_InventorySummary").ColumnCollection;
                    }
                    ExportInventorySummary(response.Result, columnReceipt);
                }
            }

            return View(vm);
        }

        /// <summary>
        /// 导出库存汇总报表 爱库存
        /// </summary>
        /// <param name="response"></param>
        /// <param name="columnReceipt"></param>
        private void ExportInventorySummary(GetInventoryBySearchConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReportInventorySummary> receipts = response.InventorySummaryCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReportInventorySummary).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "库存汇总报表信息";
            ds.Tables.Add(dtReceipt);
            ExportDataToExcelHelper.ExportExcel(ds.Tables[0], "库存汇总报表信息" + DateTime.Now.ToString("yyyy-MM-dd"), "库存汇总报表");

        }

        [HttpGet]
        public ActionResult ReceiptReport(int? PageIndex, long? customerID)
        {
            //Session["SearchConditionModel"] = null;

            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            IndexViewModel vm = new IndexViewModel();
            List<WMSConfig> wms = new List<WMSConfig>();
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName).ToList();
                wms.AddRange(ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName + "_FG").ToList());
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType").ToList();
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.ReceiptTypes = st;

            vm.SearchCondition = new ReceiptSearchCondition();
            vm.ProjectRoleID = base.UserInfo.ProjectID;
            vm.SearchCondition.StartReceiptDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString()).AddDays(-10);
            vm.SearchCondition.EndReceiptDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            GetReceiptByConditionRequest getReceiptByConditionRequest = new GetReceiptByConditionRequest();
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                StringBuilder sb = new StringBuilder();

                foreach (var i in CustomerListID)
                {
                    sb.Append("" + i + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.SearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.SearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();
            //ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            ViewBag.WarehouseList = WarehouseList;
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            }
            getReceiptByConditionRequest.SearchCondition = vm.SearchCondition;
            getReceiptByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getReceiptByConditionRequest.PageIndex = PageIndex ?? 0;
            #region 查询方法
            //var getReceiptByConditionResponse = new ReceiptManagementService().GetReceiptForRPTByCondition(getReceiptByConditionRequest);
            //if (getReceiptByConditionResponse.IsSuccess)
            //{
            //    vm.ReceiptCollection = getReceiptByConditionResponse.Result.ReceiptCollection;
            //    vm.ReceiptDetailCollection2 = getReceiptByConditionResponse.Result.ReceiptDetailCollection3;
            //    vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
            //    vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
            //}
            #endregion
            //Session["SearchConditionModel"] = vm.SearchCondition;
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            this.GenQueryReceiptViewModel(vm);
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        [HttpPost]   //入库报表
        public ActionResult ReceiptReport(IndexViewModel vm, int? PageIndex, string Action)
        {
            vm.ProjectRoleID = base.UserInfo.ProjectID;
            //if (vm.SearchCondition == null && Session["SearchConditionModel"] != null)
            //{
            //    vm.SearchCondition = (ReceiptSearchCondition)Session["SearchConditionModel"];
            //}
            //else if (vm.SearchCondition == null && Session["SearchConditionModel"] == null)
            //{
            //    vm.SearchCondition = new ReceiptSearchCondition();
            //    vm.SearchCondition.StartReceiptDate = DateTime.Now.AddDays(-10);
            //    vm.SearchCondition.EndReceiptDate = DateTime.Now;
            //}
            //else
            //{
            //    Session["SearchConditionModel"] = null;
            //    Session["SearchConditionModel"] = vm.SearchCondition;
            //}
            List<WMSConfig> wms = new List<WMSConfig>();
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName).ToList();
                wms.AddRange(ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName + "_FG").ToList());
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType").ToList();
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.ReceiptTypes = st;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            ViewBag.CustomerList = CustomerList;
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                StringBuilder sb = new StringBuilder();

                foreach (var i in CustomerListID)
                {
                    sb.Append("" + i + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.SearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.SearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();
            //ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            ViewBag.WarehouseList = WarehouseList;
            vm.SearchCondition.UserType = base.UserInfo.UserType;

            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getReceiptByConditionRequest = new GetReceiptByConditionRequest();

            getReceiptByConditionRequest.SearchCondition = vm.SearchCondition;
            getReceiptByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getReceiptByConditionRequest.PageIndex = PageIndex ?? 0;

            Response<GetReceiptDetailByConditionResponse> getReceiptByConditionResponse = new Response<GetReceiptDetailByConditionResponse>();

            if (Action == "导出" || Action == "快速导出")
            {
                getReceiptByConditionResponse = new ReceiptManagementService().ExportReceiptForRPTByCondition(getReceiptByConditionRequest);
            }
            else
            {
                getReceiptByConditionResponse = new ReceiptManagementService().GetReceiptForRPTByCondition(getReceiptByConditionRequest);
            }
            if (getReceiptByConditionResponse.IsSuccess)
            {
                vm.ReceiptCollection = getReceiptByConditionResponse.Result.ReceiptCollection;
                vm.ReceiptDetailCollection2 = getReceiptByConditionResponse.Result.ReceiptDetailCollection3;
                vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
                vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
                if (Action == "导出")
                {

                    IEnumerable<Column> columnReceipt;

                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptReport").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ResponTable").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReport").ColumnCollection.Where(c => c.ForView == true);
                    }

                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    ExportReceiptReport(getReceiptByConditionResponse.Result, columnReceipt);

                }
                else if (Action == "快速导出")
                {
                    IEnumerable<Column> columnReceipt;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptReport").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReport").ColumnCollection;
                    }
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    FastExportReceiptReport(getReceiptByConditionResponse.Result, columnReceipt);
                    //ExportInventory2(dt2, columnReceipt);
                    return Redirect("/WMS/ReportManagement/ReceiptReport");
                }
            }
            if (getReceiptByConditionResponse.IsSuccess)
            {
                vm.ReceiptCollection = getReceiptByConditionResponse.Result.ReceiptCollection;
                vm.ReceiptDetailCollection2 = getReceiptByConditionResponse.Result.ReceiptDetailCollection3;
                vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
                vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
                vm.ProjectRoleID = base.UserInfo.ProjectID;

            }


            this.GenQueryReceiptViewModel(vm);
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        private void GenQueryReceiptViewModel(IndexViewModel vm)
        {
            var Configs = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection;
            if (Configs.Where(t => t.Name == "WMS_ReceiptReport").Count() == 0)
            {
                Configs = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection;
            }
            vm.Config = Configs.First(t => t.Name == "WMS_ReceiptReport");

            if (base.UserInfo.UserType == 2)
            {
                vm.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            }
            else if (base.UserInfo.UserType == 0)
            {
                vm.CustomerList = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            }
            else
            {
                vm.CustomerList = Enumerable.Empty<SelectListItem>();
            }
        }

        private void ExportReceiptReport(GetReceiptDetailByConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReportReceiptReport> receipts = response.ReceiptDetailCollection3;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReportReceiptReport).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "入库报表";
            ds.Tables.Add(dtReceipt);
            ExportDataToExcelHelper.ExportExcel(ds.Tables[0], "入库报表信息" + DateTime.Now.ToString("yyyy-MM-dd"), "入库报表");

        }

        private void FastExportReceiptReport(GetReceiptDetailByConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReportReceiptReport> receipts = response.ReceiptDetailCollection3;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReportReceiptReport).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "入库报表";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportExcel(ds.Tables[0], "入库报表信息" + DateTime.Now.ToString("yyyy-MM-dd"), "入库报表");
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "入库报表" + DateTime.Now.ToString("yyyy-MM-dd"));
            ExportByEPPlus(ds.Tables[0], "入库报表信息");
        }

        private void ExportReceiptReport(IEnumerable<ReportReceiptReport> response)
        {
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            dtReceipt.Columns.Add("出库单行号");
            dtReceipt.Columns.Add("产品编码");
            dtReceipt.Columns.Add("描述");
            dtReceipt.Columns.Add("Article");
            dtReceipt.Columns.Add("尺码");
            dtReceipt.Columns.Add("预计数量");
            dtReceipt.Columns.Add("实际数量");
            dtReceipt.Columns.Add("差异");
            dtReceipt.Columns.Add("系统收货单号");
            foreach (var item in response)
            {
                DataRow dr = dtReceipt.NewRow();
                dr["出库单行号"] = item.LineNumber;
                dr["产品编码"] = item.SKU;
                dr["描述"] = item.GoodsName;
                dr["Article"] = item.Article;
                dr["尺码"] = item.Size;
                dr["预计数量"] = item.QtyExpected;
                dr["实际数量"] = item.QtyReceived;
                dr["差异"] = item.Qty;
                dr["系统收货单号"] = item.ReceiptNumber;
                dtReceipt.Rows.Add(dr);
            }

            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "入库报表" + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "入库报表" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// Bob 查询 SKU进出库
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="warehouseID"></param>
        /// <param name="warehouseAreaID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SkuInAndOut(long? customerID, long? warehouseID, long? warehouseAreaID, int? PageIndex)
        {
            SkuInAndOutViewModel vm = new SkuInAndOutViewModel();
            vm.SearchCondition = new ReportSkuInAndOutSearchCondition();
            vm.SearchCondition.BeginCreateTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString()).AddDays(-10);
            vm.SearchCondition.EndCreateTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                    else
                    {
                        vm.SearchCondition.CustomerID = 0;
                    }
                }
            }

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == 0)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            //}
            if (WarehouseList != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            ViewBag.WarehouseList = WarehouseList;
            GetSkuInAndOutBySearchConditionRequest request = new GetSkuInAndOutBySearchConditionRequest();
            request.SkuInAndOutSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            //var response = new ReportManagementService().GetSkuInAndOutBySearchCondition(request);

            //if (response.IsSuccess)
            //{
            //    vm.ReportSkuInAndOutCollection = response.Result.SkuInAndOutCollection;
            //    vm.PageIndex = response.Result.PageIndex;
            //    vm.PageCount = response.Result.PageCount;

            //}
            return View(vm);
        }

        /// <summary>
        /// Bob 条件查询 SKU进出库日志报表
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="PageIndex"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SkuInAndOut(SkuInAndOutViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            //}
            if (vm.SearchCondition.Warehouse != null)
            {

            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }
            ViewBag.WarehouseList = WarehouseList;



            ViewBag.WarehouseList = WarehouseList;
            GetSkuInAndOutBySearchConditionRequest request = new GetSkuInAndOutBySearchConditionRequest();
            request.SkuInAndOutSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            Response<GetSkuInAndOutBySearchConditionResponse> response = new Response<GetSkuInAndOutBySearchConditionResponse>();
            if (Action == "导出")
            {
                response = new ReportManagementService().ExportSkuInAndOutBySearchCondition(request);
            }
            else
            {
                response = new ReportManagementService().GetSkuInAndOutBySearchCondition(request);
            }
            if (response.IsSuccess)
            {
                vm.ReportSkuInAndOutCollection = response.Result.SkuInAndOutCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {

                    IEnumerable<Column> columnReceipt;

                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_SkuInAndOut").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_SkuInAndOut").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_SkuInAndOut").ColumnCollection;
                    }
                    //是否导出批次列
                    if (vm.SearchCondition.CustomerID != 88)
                    {
                        columnReceipt = columnReceipt.Where(m => m.DbColumnName != "BatchNumber");
                    }
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    ExportSkuInAndOut(response.Result, columnReceipt);

                }
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult ReportTable(long? customerID, long? warehouseID, long? warehouseAreaID, int? PageIndex)
        {
            ReportTableViewModel vm = new ReportTableViewModel();
            List<WMSConfig> wms = new List<WMSConfig>();
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName).ToList();
                wms.AddRange(ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName + "_FG").ToList());
            }
            catch (Exception e)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderType").ToList();
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.OrderType = st;

            vm.ProjectRoleID = base.UserInfo.ProjectID;
            vm.SearchCondition = new ReportTableSearchCondition();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            //完成日期默认值  
            vm.SearchCondition.StartCompleteDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString()).AddDays(-10);
            vm.SearchCondition.EndCompleteDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.SearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();
            //ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            GetReportTableBySearchConditionRequest request = new GetReportTableBySearchConditionRequest();
            request.TableSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            //   var response = new WMS_CustomerService().GetCustomerByConditon(new GetWMS_CustomerByConditionRequest() { StorerKey = string.IsNullOrEmpty(vm.StorerKey) ? "" : vm.StorerKey, CustomerID = string.IsNullOrEmpty(vm.CustomerID.ToString()) ? "" : vm.CustomerID.ToString(), Contact1 = string.IsNullOrEmpty(vm.Contact1) ? "" : vm.Contact1, PhoneNum = string.IsNullOrEmpty(vm.PhoneNum1) ? "" : vm.PhoneNum1, Company = string.IsNullOrEmpty(vm.Company) ? "" : vm.Company, PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 });
            #region 查询方法
            //var response = new ReportManagementService().GetResponTableBySearchCondition(request);
            //if (response.IsSuccess)
            //{
            //vm.ReportTableChangeCollection = response.Result.ReprotTableInCollection;
            //vm.PageIndex = response.Result.PageIndex;
            //vm.PageCount = response.Result.PageCount;
            //vm.ProjectRoleID = base.UserInfo.ProjectID;
            //}
            #endregion
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        /// </summary>
        /// <param name="vm"></param>
        /// <param name="PageIndex"></param>
        /// <param name="Action"></param>
        /// <returns></returns>

        //出库报表
        [HttpPost]
        public ActionResult ReportTable(ReportTableViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.SearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();
            //ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;

            List<WMSConfig> wmss = new List<WMSConfig>();
            try
            {
                wmss = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName).ToList();
                wmss.AddRange(ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName + "_FG").ToList());
            }
            catch (Exception e)
            {
            }

            if (wmss == null)
            {
                wmss = ApplicationConfigHelper.GetWMS_Config("OrderType").ToList();
            }

            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wmss)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.OrderType = st;

            GetReportTableBySearchConditionRequest request = new GetReportTableBySearchConditionRequest();
            request.TableSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;



            Response<GetReportTableBySearchConditionResponse> response = new Response<GetReportTableBySearchConditionResponse>();

            if (Action == "导出" || Action == "快速导出")
            {
                //出库报表做成可配置
                IEnumerable<WMSConfig> wms = null;
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("ExportReportTableBySearch_" + base.UserInfo.ProjectName);
                }
                catch
                {
                }
                if (wms == null)
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("ExportReportTableBySearch");
                }
                response = new ReportManagementService().ExportResponTableBySearchCondition(request, wms.FirstOrDefault().Name);
            }
            else
            {
                //出库报表做成可配置
                IEnumerable<WMSConfig> wms = null;
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("GetReportTableBySearch_" + base.UserInfo.ProjectName);
                }
                catch
                {
                }
                if (wms == null)
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("GetReportTableBySearch");
                }
                response = new ReportManagementService().GetResponTableBySearchCondition(request, wms.FirstOrDefault().Name);
            }
            if (response.IsSuccess)
            {
                vm.ReportTableChangeCollection = response.Result.ReprotTableInCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {

                    IEnumerable<Column> columnReceipt;

                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ResponTable").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ResponTable").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_ResponTable").ColumnCollection;
                    }

                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    ExportReportTable(response.Result, columnReceipt);

                }
                else if (Action == "快速导出")
                {
                    IEnumerable<Column> columnReceipt;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ResponTable").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_ResponTable").ColumnCollection;
                    }
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    FastExportReportTable(response.Result, columnReceipt);
                    //ExportInventory2(dt2, columnReceipt);
                    return Redirect("/WMS/ReportManagement/ReportTable");
                }
            }
            if (response.IsSuccess)
            {
                vm.ReportTableChangeCollection = response.Result.ReprotTableInCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                vm.ProjectRoleID = base.UserInfo.ProjectID;

            }
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        [HttpGet]   //出库单差异报表
        public ActionResult DiffReportTable(long? customerID, long? warehouseID, long? warehouseAreaID, int? PageIndex)
        {
            ReportTableViewModel vm = new ReportTableViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.OrderType = st;

            vm.ProjectRoleID = base.UserInfo.ProjectID;
            vm.SearchCondition = new ReportTableSearchCondition();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.SearchCondition.StartCompleteDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString()).AddDays(-10);
            vm.SearchCondition.EndCompleteDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.SearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            GetReportTableBySearchConditionRequest request = new GetReportTableBySearchConditionRequest();
            request.TableSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        [HttpGet]   //来货预检差异报表
        public ActionResult AsnScanDiffReport(long? customerID, long? warehouseID, int? PageIndex)
        {
            IndexViewModel vm = new IndexViewModel();
            vm.ProjectRoleID = base.UserInfo.ProjectID;
            vm.SearchCondition = new ReceiptSearchCondition();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.SearchCondition.StartCompleteDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString()).AddDays(-10);
            vm.SearchCondition.EndCompleteDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.SearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();

            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            GetReceiptByConditionRequest request = new GetReceiptByConditionRequest();
            request.SearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            return View(vm);
        }

        [HttpPost]   //来货预检差异报表
        public ActionResult AsnScanDiffReport(IndexViewModel vm, int? PageIndex, string Action)
        {
            vm.ProjectRoleID = base.UserInfo.ProjectID;

            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ASNType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.ReceiptTypes = st;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            ViewBag.CustomerList = CustomerList;
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                StringBuilder sb = new StringBuilder();

                foreach (var i in CustomerListID)
                {
                    sb.Append("" + i + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.SearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.SearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            vm.SearchCondition.UserType = base.UserInfo.UserType;

            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getReceiptByConditionRequest = new GetReceiptByConditionRequest();

            getReceiptByConditionRequest.SearchCondition = vm.SearchCondition;
            getReceiptByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getReceiptByConditionRequest.PageIndex = PageIndex ?? 0;

            Response<GetReceiptDetailByConditionResponse> getReceiptByConditionResponse = new Response<GetReceiptDetailByConditionResponse>();

            if (Action == "导出")
            {
                getReceiptByConditionResponse = new ReceiptManagementService().ExportAsnScanDiffForRPTByCondition(getReceiptByConditionRequest);
            }
            else
            {
                getReceiptByConditionResponse = new ReceiptManagementService().GetAsnScanDiffForRPTByCondition(getReceiptByConditionRequest);
            }
            if (getReceiptByConditionResponse.IsSuccess)
            {

                vm.ReceiptDetailCollection2 = getReceiptByConditionResponse.Result.ReceiptDetailCollection3;
                vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
                vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
                if (Action == "导出")
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<style>td{mso-number-format:\"\\@\";}</style>");
                    sb.Append("<table border='1' cellspacing='0' cellpadding='0'>");
                    sb.Append("<tr><td>行号</td><td>预入库单号</td><td>外部单号</td><td>入库日期</td><td>箱号</td><td>产品编码</td><td>期望数量</td><td>扫描数量</td><td>差异数量</td><td>门店代码</td></tr>");
                    int i = 0;
                    foreach (var item in vm.ReceiptDetailCollection2)
                    {
                        sb.Append("<tr><td>" + i++ + "</td><td>" + item.ASNNumber.ToString() + "</td><td>" + item.ExternReceiptNumber.ToString() + "</td>");
                        sb.Append("<td>" + item.ReceiptDate.ToString() + "</td><td>" + item.str2.ToString() + "</td><td>" + item.SKU.ToString() + "</td><td>" + item.QtyExpected.ToString() + "</td><td>" + item.QtyReceived.ToString() + "</td><td>" + (item.QtyReceived - item.QtyExpected).ToString() + "</td><td>" + item.str3.ToString() + "</td></tr>");
                    }
                    sb.Append("</table>");
                    ss.HttpResponse Response;
                    Response = ss.HttpContext.Current.Response;
                    Response.Charset = "UTF-8";
                    Response.HeaderEncoding = Encoding.UTF8;
                    Response.AppendHeader("content-disposition", "attachment;filename=\"" + ss.HttpUtility.UrlEncode("来货预检扫描差异_" + DateTime.Now.ToShortDateString(), Encoding.UTF8) + ".xls\"");
                    Response.ContentEncoding = Encoding.UTF8;
                    Response.ContentType = "application/ms-excel";
                    Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sb.ToString());
                    Response.Flush();
                    Response.End();


                }
                vm.ProjectRoleID = base.UserInfo.ProjectID;
            }
            this.GenQueryReceiptViewModel(vm);
            return View(vm);
        }


        [HttpPost]    //出库单差异报表
        public ActionResult DiffReportTable(ReportTableViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.SearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;

            IEnumerable<WMSConfig> wmss = null;
            try
            {
                wmss = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }

            if (wmss == null)
            {
                wmss = ApplicationConfigHelper.GetWMS_Config("OrderType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wmss)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.OrderType = st;

            GetReportTableBySearchConditionRequest request = new GetReportTableBySearchConditionRequest();
            request.TableSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;

            Response<GetReportTableBySearchConditionResponse> response = new Response<GetReportTableBySearchConditionResponse>();

            if (Action == "导出" || Action == "快速导出")
            {
                //出库报表做成可配置
                IEnumerable<WMSConfig> wms = null;
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("ExportDiffReportTableBySearch_" + base.UserInfo.ProjectName);
                }
                catch
                {
                }
                if (wms == null)
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("ExportDiffReportTableBySearch");
                }
                response = new ReportManagementService().ExportResponTableBySearchCondition(request, wms.FirstOrDefault().Name);
            }
            else
            {
                //出库报表做成可配置
                IEnumerable<WMSConfig> wms = null;
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("GetDiffReportTableByCondition_" + base.UserInfo.ProjectName);
                }
                catch
                {
                }
                if (wms == null)
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("GetDiffReportTableByCondition");
                }
                response = new ReportManagementService().GetResponTableBySearchCondition(request, wms.FirstOrDefault().Name);
            }
            if (response.IsSuccess)
            {
                vm.ReportTableChangeCollection = response.Result.ReprotTableInCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {
                    IEnumerable<Column> columnReceipt;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_DiffResponTable").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ResponTable").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_DiffResponTable").ColumnCollection;
                    }
                    ExportReportTableDiffer(response.Result, columnReceipt);
                }
                else if (Action == "快速导出")
                {
                    IEnumerable<Column> columnReceipt;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_DiffResponTable").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_DiffResponTable").ColumnCollection;
                    }
                    FastExportDiffReportTable(response.Result, columnReceipt);
                    return Redirect("/WMS/ReportManagement/ReportTable");
                }
            }
            if (response.IsSuccess)
            {
                vm.ReportTableChangeCollection = response.Result.ReprotTableInCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                vm.ProjectRoleID = base.UserInfo.ProjectID;

            }
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        private void ExportReportTable(GetReportTableBySearchConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReprotTableIn> receipts = response.ReprotTableInCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReprotTableIn).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "出库报表";
            ds.Tables.Add(dtReceipt);
            ExportDataToExcelHelper.ExportExcel(ds.Tables[0], "出库报表信息" + DateTime.Now.ToString("yyyy-MM-dd"), "出库报表");
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "出库报表" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        private void FastExportReportTable(GetReportTableBySearchConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReprotTableIn> receipts = response.ReprotTableInCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReprotTableIn).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "出库报表";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportExcel(ds.Tables[0], "出库报表信息" + DateTime.Now.ToString("yyyy-MM-dd"), "出库报表");
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "出库报表" + DateTime.Now.ToString("yyyy-MM-dd"));
            ExportByEPPlus(ds.Tables[0], "出库报表信息");
        }

        private void FastExportDiffReportTable(GetReportTableBySearchConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReprotTableIn> receipts = response.ReprotTableInCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReprotTableIn).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "出库差异报表";
            ds.Tables.Add(dtReceipt);
            ExportByEPPlus(ds.Tables[0], "出库差异报表信息");
        }

        //public IEnumerable<ReprotTableIn> ExportReprotTableBySearchCondition(ReportTableSearchCondition SearchCondition)
        //{
        //    string sqlWhere = this.GenGetReportTableIn(SearchCondition);
        //    DbParam[] dbParams = new DbParam[]{
        //        new DbParam("@Where", DbType.String, sqlWhere, ParameterDirection.Input)
        //    };
        //    DataTable dt = this.ExecuteDataTable("Proc_WMS_ExportReportTableBySearchCondition_Report", dbParams);
        //    return dt.ConvertToEntityCollection<ReprotTableIn>();
        //}

        /// <summary>
        /// Bob 查询 库存变更报表
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="warehouseID"></param>
        /// <param name="warehouseAreaID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult InventoryChange(long? customerID, long? warehouseID, long? warehouseAreaID, int? PageIndex)
        {
            InventoryChangeViewModel vm = new InventoryChangeViewModel();
            vm.SearchCondition = new ReportInventoryChangeSearchCondition();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                    else
                    {
                        vm.SearchCondition.CustomerID = 0;
                    }
                }
            }

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == 0)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            //}
            if (warehouseID != null)
            {
                vm.SearchCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == warehouseID.ToString()).First().Value + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }
            ViewBag.WarehouseList = WarehouseList;
            GetInventoryChangeBySearchConditionRequest request = new GetInventoryChangeBySearchConditionRequest();
            request.InventoryChangeSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            //var response = new ReportManagementService().GetInventoryChangeBySearchCondition(request);
            //if (response.IsSuccess)
            //{
            //vm.ReportInventoryChangeCollection = response.Result.InventoryChangeCollection;
            //    vm.PageIndex = response.Result.PageIndex;
            //    vm.PageCount = response.Result.PageCount;

            //}
            return View(vm);
        }

        /// <summary>
        /// Bob 条件查询 库存变更报表
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="PageIndex"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InventoryChange(InventoryChangeViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            //}
            if (vm.SearchCondition.Warehouse != null)
            {
                //vm.AdjustmentCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == vm.AdjustmentCondition.Warehouse.ToString()).First().Value + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            ViewBag.WarehouseList = WarehouseList;
            GetInventoryChangeBySearchConditionRequest request = new GetInventoryChangeBySearchConditionRequest();
            request.InventoryChangeSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            Response<GetInventoryChangeBySearchConditionResponse> response = new Response<GetInventoryChangeBySearchConditionResponse>();
            if (Action == "导出")
            {
                response = new ReportManagementService().ExportInventoryChangeBySearchCondition(request);
            }
            else
            {
                response = new ReportManagementService().GetInventoryChangeBySearchCondition(request);
            }
            if (response.IsSuccess)
            {
                vm.ReportInventoryChangeCollection = response.Result.InventoryChangeCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {

                    IEnumerable<Column> columnReceipt;

                    if (vm.SearchCondition.CustomerID == 101 || vm.SearchCondition.CustomerID == 103)
                    {
                        var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                        Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                        if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_InventoryChange_SH").Count() == 0)
                        {
                            columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_InventoryChange_SH").ColumnCollection;
                        }
                        else
                        {
                            columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_InventoryChange_SH").ColumnCollection;
                        }

                    }
                    else
                    {
                        var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                        Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                        if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_InventoryChange").Count() == 0)
                        {
                            columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_InventoryChange").ColumnCollection;
                        }
                        else
                        {
                            columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_InventoryChange").ColumnCollection;
                        }

                        //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                        //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                        //IEnumerable<Table> tables = module.Tables.TableCollection;
                        //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_Inventory").ColumnCollection;

                    }
                    ExportInventoryChange(response.Result, columnReceipt, request.InventoryChangeSearchCondition.InventoryChangeTypes);
                }
            }
            return View(vm);
        }

        /// <summary>
        /// Bob 查询 仓储密度报表
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="warehouseID"></param>
        /// <param name="warehouseAreaID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WarehouseStorageDensity(long? customerID, long? warehouseID, long? warehouseAreaID, int? PageIndex)
        {
            WarehouseStorageDensityViewModel vm = new WarehouseStorageDensityViewModel();
            vm.SearchCondition = new ReportWarehouseStorageDensitySearchCondition();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }

            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            GetWarehouseStorageDensityBySearchConditionRequest request = new GetWarehouseStorageDensityBySearchConditionRequest();
            request.WarehouseStorageDensitySearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            //var response = new ReportManagementService().GetWarehouseStorageDensityBySearchCondition(request);
            //if (response.IsSuccess)
            //{
            //    vm.ReportWarehouseStorageDensityCollection = response.Result.WarehouseStorageDensityCollection;
            //    vm.PageIndex = response.Result.PageIndex;
            //    vm.PageCount = response.Result.PageCount;

            //}
            return View(vm);
        }

        /// <summary>
        /// Bob 条件查询 仓储密度报表
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="PageIndex"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WarehouseStorageDensity(WarehouseStorageDensityViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }

            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            GetWarehouseStorageDensityBySearchConditionRequest request = new GetWarehouseStorageDensityBySearchConditionRequest();
            request.WarehouseStorageDensitySearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            Response<GetWarehouseStorageDensityBySearchConditionResponse> response = new Response<GetWarehouseStorageDensityBySearchConditionResponse>();
            if (Action == "导出")
            {
                response = new ReportManagementService().ExportWarehouseStorageDensityBySearchCondition(request);
            }
            else
            {
                response = new ReportManagementService().GetWarehouseStorageDensityBySearchCondition(request);
            }
            if (response.IsSuccess)
            {
                vm.ReportWarehouseStorageDensityCollection = response.Result.WarehouseStorageDensityCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {

                    IEnumerable<Column> columnReceipt;

                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_WarehouseStorageDensity").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_WarehouseStorageDensity").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_WarehouseStorageDensity").ColumnCollection;
                    }

                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    ExportWarehouseStorageDensity(response.Result, columnReceipt);

                }
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult Sku(long? customerID, long? warehouseID, long? warehouseAreaID, int? PageIndex)
        {
            SkuViewModel vm = new SkuViewModel();
            vm.SearchCondition = new ReportSkuSearchCondition();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }

            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            GetSkuBySearchConditionRequest request = new GetSkuBySearchConditionRequest();
            request.SkuSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            var response = new ReportManagementService().GetSkuBySearchCondition(request);
            if (response.IsSuccess)
            {
                vm.ReportSkuCollection = response.Result.SkuCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;

            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Sku(SkuViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;


            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }

            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            GetSkuBySearchConditionRequest request = new GetSkuBySearchConditionRequest();
            request.SkuSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            Response<GetSkuBySearchConditionResponse> response = new Response<GetSkuBySearchConditionResponse>();
            if (Action == "导出")
            {
                response = new ReportManagementService().ExportSkuBySearchCondition(request);
            }
            else
            {
                response = new ReportManagementService().GetSkuBySearchCondition(request);
            }
            if (response.IsSuccess)
            {
                vm.ReportSkuCollection = response.Result.SkuCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {

                    IEnumerable<Column> columnReceipt;

                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_SkuNormal").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_SkuNormal").ColumnCollection;
                    }
                    ExportSku(response.Result, columnReceipt);

                }
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult Staff(long? customerID)
        {
            StaffViewModel vm = new StaffViewModel();
            vm.SearchCondition = new ReportStaffSearchCondition();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }

            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            return View(vm);
        }

        [HttpPost]
        public ActionResult Staff(StaffViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;


            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            return View(vm);
        }

        [HttpPost]
        public string ChangeWarehouse(string str)
        {
            string js = string.Empty;
            long st1 = 0;
            var s = ApplicationConfigHelper.GetCacheInfo().Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct().Where(c => c.WarehouseName == str);
            if (s != null)
            {
                st1 = s.Select(c => c.WarehouseID).FirstOrDefault();
            }

            if (st1 != 0)
            {
                //ApplicationConfigHelper.RefreshGetWarehouseAreaList(str);
                IEnumerable<AreaInfo> list = ApplicationConfigHelper.GetWarehouseAreaList(st1);

                List<SelectListItem> st = new List<SelectListItem>();
                foreach (AreaInfo warehouse in list)
                {
                    if (!st.Contains(new SelectListItem() { Value = warehouse.ID.ToString(), Text = warehouse.AreaName }))
                        st.Add(new SelectListItem() { Value = warehouse.ID.ToString(), Text = warehouse.AreaName });
                }

                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                js = jsonSerializer.Serialize(list);

            }
            return js;
        }

        public void ExportExcel(DataTable dt, string strFileName, string strSheetName)
        {
            XSSFWorkbook book = new XSSFWorkbook();
            ISheet sheet = book.CreateSheet(strSheetName);

            IRow headerrow = sheet.CreateRow(0);
            ICellStyle style = book.CreateCellStyle();
            style.Alignment = HorizontalAlignment.CENTER;
            style.VerticalAlignment = VerticalAlignment.CENTER;

            XSSFRow dataRow = (XSSFRow)sheet.CreateRow(0);
            string strColumns = "";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strColumns += dt.Columns[i].ColumnName + ",";
            }
            strColumns = strColumns.Substring(0, strColumns.Length - 1);
            string[] strArry = strColumns.Split(',');
            for (int i = 0; i < strArry.Length; i++)
            {
                dataRow.CreateCell(i).SetCellValue(strArry[i]);
                dataRow.GetCell(i).CellStyle = style;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataRow = (XSSFRow)sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string ValueType = "";
                    string Value = "";
                    if (dt.Rows[i][j].ToString() != null)
                    {
                        ValueType = dt.Rows[i][j].GetType().ToString();
                        Value = dt.Rows[i][j].ToString();
                    }
                    switch (ValueType)
                    {
                        case "System.String"://字符串类型
                            dataRow.CreateCell(j).SetCellValue(Value);
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(Value, out dateV);
                            dataRow.CreateCell(j).SetCellValue(dateV);
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(Value, out boolV);
                            dataRow.CreateCell(j).SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(Value, out intV);
                            dataRow.CreateCell(j).SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(Value, out doubV);
                            dataRow.CreateCell(j).SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            dataRow.CreateCell(j).SetCellValue("");
                            break;
                        default:
                            dataRow.CreateCell(j).SetCellValue("");
                            break;
                    }
                    dataRow.GetCell(j).CellStyle = style;
                    //设置宽度
                    sheet.SetColumnWidth(j, (Value.Length + 10) * 256);
                }
            }
            MemoryStream ms = new MemoryStream();
            book.Write(ms);
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xlsx", HttpUtility.UrlEncode(strFileName, Encoding.UTF8)));
            Response.BinaryWrite(ms.ToArray());
            Response.End();
            book = null;
            ms.Close();
            ms.Dispose();
            //HSSFWorkbook book = new HSSFWorkbook();
            //ISheet sheet = book.CreateSheet(strSheetName);

            //IRow headerrow = sheet.CreateRow(0);
            //ICellStyle style = book.CreateCellStyle();
            //style.Alignment = HorizontalAlignment.Center;
            //style.VerticalAlignment = VerticalAlignment.Center;

            //HSSFRow dataRow = (HSSFRow)sheet.CreateRow(0);
            //string strColumns = "";
            //for (int i = 0; i <dt.Columns.Count; i++)
            //{
            //    strColumns += dt.Columns[i].ColumnName + ",";
            //}
            //strColumns = strColumns.Substring(0, strColumns.Length - 1);
            //string[] strArry = strColumns.Split(',');
            //for (int i = 0; i <strArry.Length; i++)
            //{
            //    dataRow.CreateCell(i).SetCellValue(strArry[i]);
            //    dataRow.GetCell(i).CellStyle = style;
            //}
            //for (int i = 0; i <dt.Rows.Count; i++)
            //{
            //    dataRow = (HSSFRow)sheet.CreateRow(i + 1);
            //    for (int j = 0; j <dt.Columns.Count; j++)
            //    {
            //        string ValueType = "";
            //        string Value = "";
            //        if (dt.Rows[i][j].ToString() != null)
            //        {
            //            ValueType = dt.Rows[i][j].GetType().ToString();
            //            Value = dt.Rows[i][j].ToString();
            //        }
            //        switch (ValueType)
            //        {
            //            case "System.String"://字符串类型
            //                dataRow.CreateCell(j).SetCellValue(Value);
            //                break;
            //            case "System.DateTime"://日期类型
            //                DateTime dateV;
            //                DateTime.TryParse(Value, out dateV);
            //                dataRow.CreateCell(j).SetCellValue(dateV);
            //                break;
            //            case "System.Boolean"://布尔型
            //                bool boolV = false;
            //                bool.TryParse(Value, out boolV);
            //                dataRow.CreateCell(j).SetCellValue(boolV);
            //                break;
            //            case "System.Int16"://整型
            //            case "System.Int32":
            //            case "System.Int64":
            //            case "System.Byte":
            //                int intV = 0;
            //                int.TryParse(Value, out intV);
            //                dataRow.CreateCell(j).SetCellValue(intV);
            //                break;
            //            case "System.Decimal"://浮点型
            //            case "System.Double":
            //                double doubV = 0;
            //                double.TryParse(Value, out doubV);
            //                dataRow.CreateCell(j).SetCellValue(doubV);
            //                break;
            //            case "System.DBNull"://空值处理
            //                dataRow.CreateCell(j).SetCellValue("");
            //                break;
            //            default:
            //                dataRow.CreateCell(j).SetCellValue("");
            //                break;
            //        }
            //        dataRow.GetCell(j).CellStyle = style;
            //        //设置宽度
            //        sheet.SetColumnWidth(j, (Value.Length + 10) * 256);
            //    }
            //}
            //MemoryStream ms = new MemoryStream();
            //book.Write(ms);
            //Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.xls", HttpUtility.UrlEncode(strFileName, Encoding.UTF8)));
            //Response.BinaryWrite(ms.ToArray());
            //Response.End();
            //book = null;
            //ms.Close();
            //ms.Dispose();
        }

        private void ExportSku(GetSkuBySearchConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReportSku> receipts = response.SkuCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReportSku).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "产品编码报表信息";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "SKU报表信息" + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "产品编码报表信息" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        private void ExportInventory(GetInventoryBySearchConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReportInventory> receipts = response.InventoryCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReportInventory).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "库存报表信息";
            ds.Tables.Add(dtReceipt);
            ExportDataToExcelHelper.ExportExcel(ds.Tables[0], "库存报表信息" + DateTime.Now.ToString("yyyy-MM-dd"), "库存报表");
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "库存报表信息" + DateTime.Now.ToString("yyyy-MM-dd"));
            //DataTableToExcel(ds.Tables[0], @"D:\", "库存报表信息" + DateTime.Now.ToString("yyyy-MM-dd").ToString());
        }

        /// <summary>
        /// 快速导出
        /// </summary>
        /// <param name="response"></param>
        /// <param name="columnReceipt"></param>
        private void FastExportInventory(GetInventoryBySearchConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReportInventory> receipts = response.InventoryCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }
            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {
                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReportInventory).GetProperty(receipt.DbColumnName).GetValue(s);
                }
                dtReceipt.Rows.Add(drReceipt);
            });
            dtReceipt.TableName = "库存报表信息";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportExcel(ds.Tables[0], "库存报表信息" + DateTime.Now.ToString("yyyy-MM-dd"), "库存报表");
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "库存报表信息" + DateTime.Now.ToString("yyyy-MM-dd"));
            ExportByEPPlus(ds.Tables[0], "库存报表信息");
        }

        private void ExportInventory2(DataTable dtReceipt, IEnumerable<Column> columnReceipt)
        {
            //DataTable dt = new DataTable();
            //dt = dtReceipt;
            //IEnumerable<ReportInventory> receipts = response.InventoryCollection;
            //DataSet ds = new DataSet();
            //DataTable dtReceipt = new DataTable();
            //foreach (var receipt in columnReceipt)
            //{
            //    dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            //}

            //receipts.Each((i, s) =>
            //{
            //    DataRow drReceipt = dtReceipt.NewRow();
            //    foreach (var receipt in columnReceipt)
            //    {

            //        drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReportInventory).GetProperty(receipt.DbColumnName).GetValue(s);

            //    }
            //    dtReceipt.Rows.Add(drReceipt);
            //});
            DataSet ds = new DataSet();
            dtReceipt.TableName = "库存报表信息";
            ds.Tables.Add(dtReceipt.Copy());
            ExportDataToExcelHelper.ExportExcel(ds.Tables[0], "库存报表信息" + DateTime.Now.ToString("yyyy-MM-dd"), "库存报表");
            // ExportDataToExcelHelper.ExportDataSetToExcel(ds, "库存报表信息" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        private void ExportSkuInAndOut(GetSkuInAndOutBySearchConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReportSkuInAndOut> receipts = response.SkuInAndOutCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReportSkuInAndOut).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "产品编码进出库日志报表信息";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "SKU进出库日志报表信息" + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "产品编码进出库日志报表信息" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        private void ExportSkuChange(GetSKUChangeBySearchConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReportSKUChange> receipts = response.SkuChangeCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.WMS.Report.ReportSKUChange).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "产品编码变更报表信息";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "SKU变更报表信息" + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "产品编码变更报表信息" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        private void ExportInventoryChange(GetInventoryChangeBySearchConditionResponse response, IEnumerable<Column> columnReceipt, String InventoryChangeTypes)
        {
            IEnumerable<ReportInventoryChange> receipts = response.InventoryChangeCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReportInventoryChange).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });
            //response.InventoryChangeCollection.Select(m => m.AdjustmentType.ToString());
            if (string.IsNullOrEmpty(InventoryChangeTypes))
            {
                dtReceipt.TableName = "库存变更报表";//看情况
                ds.Tables.Add(dtReceipt);
                //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "库存变更报表" + DateTime.Now.ToString("yyyy-MM-dd"));
                EPPlusOperation.ExportDataSetByEPPlus(ds, "库存变更报表" + DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else if (InventoryChangeTypes.Equals("库存调整单"))//1
            {
                dtReceipt.TableName = "库存调整报表";//看情况
                ds.Tables.Add(dtReceipt);
                //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "库存调整报表" + DateTime.Now.ToString("yyyy-MM-dd"));
                EPPlusOperation.ExportDataSetByEPPlus(ds, "库存调整报表" + DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else if (InventoryChangeTypes.Equals("库存冻结单"))//2
            {
                dtReceipt.TableName = "库存冻结报表";//看情况
                ds.Tables.Add(dtReceipt);
                //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "库存冻结报表" + DateTime.Now.ToString("yyyy-MM-dd"));
                EPPlusOperation.ExportDataSetByEPPlus(ds, "库存冻结报表" + DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else if (InventoryChangeTypes.Equals("库存移动单"))//3
            {
                dtReceipt.TableName = "库存移库报表";//看情况
                ds.Tables.Add(dtReceipt);
                //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "库存移库报表" + DateTime.Now.ToString("yyyy-MM-dd"));
                EPPlusOperation.ExportDataSetByEPPlus(ds, "库存移库报表" + DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else if (InventoryChangeTypes.Equals("库存品级调整单"))//3
            {
                dtReceipt.TableName = "库存品级调整报表";//看情况
                ds.Tables.Add(dtReceipt);
                //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "库存品级调整报表" + DateTime.Now.ToString("yyyy-MM-dd"));
                EPPlusOperation.ExportDataSetByEPPlus(ds, "库存品级调整报表" + DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else
            {
                Response.ContentType = "text/html";
                string script = "<script>alert('无法导出当前类型报表！')</script>";

                Response.Write(script);
                Response.Write("<script>document.location=document.location;</script>");
            }

        }

        private void ExportWarehouseStorageDensity(GetWarehouseStorageDensityBySearchConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReportWarehouseStorageDensity> receipts = response.WarehouseStorageDensityCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReportWarehouseStorageDensity).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });
            dtReceipt.TableName = "仓储密度报表";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "仓储密度报表" + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "仓储密度报表" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        [HttpPost]
        public JsonResult PrintUpdateReceipt(long ID, string SKU, long AllSum, long EverySum)
        {
            long i = 0;
            long lastSum = 0;
            var skuAndbatch = SKU.Split('/');
            string SKUS = skuAndbatch[0];
            string batchs = skuAndbatch[1];
            IList<ReceiptDetail> ReceiptDetail = new List<ReceiptDetail>();
            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            int c = 1;
            string str = "";
            if (AllSum > EverySum)
            {
                i = AllSum / EverySum;
                lastSum = AllSum % EverySum;
                for (int j = 0; j < i; j++)
                {
                    str = datetime + ReturnLineNumber(c);
                    c++;
                    ReceiptDetail.Add(
                        new ReceiptDetail()
                        {
                            RID = ID.ObjectToInt32(),
                            SKU = SKUS,
                            QtyExpected = EverySum.ObjectToInt32(),
                            BoxNumber = str,
                            BatchNumber = batchs
                        });
                }
                if (lastSum != 0)
                {
                    str = datetime + ReturnLineNumber(c);
                    ReceiptDetail.Add(new ReceiptDetail() { RID = ID.ObjectToInt32(), SKU = SKUS, QtyExpected = lastSum.ObjectToInt32(), BoxNumber = str, BatchNumber = batchs });
                }
            }

            else
            {
                i = 1;
                lastSum = EverySum;
                str = datetime + ReturnLineNumber(c);
                ReceiptDetail.Add(new ReceiptDetail() { RID = ID.ObjectToInt32(), SKU = SKUS, QtyExpected = lastSum.ObjectToInt32(), BoxNumber = str, BatchNumber = batchs });
            }
            var response = new ReportManagementService().PrintUpdateReceipt(ID, ReceiptDetail);
            if (response.IsSuccess)
            {
                return Json(new { ErrorCode = "1", Response = response.Result.receiptPrint });
            }
            else
            {
                return Json(new { ErrorCode = "0" });
            }

        }

        public string ReturnLineNumber(int row_count)
        {
            var linnumber = "";
            if (row_count < 10)
            {
                linnumber = "0000" + row_count;
            }
            if (100 > row_count && row_count >= 10)
            {
                linnumber = "000" + row_count;
            }
            if (1000 > row_count && row_count >= 100)
            {
                linnumber = "00" + row_count;
            }
            if (row_count >= 1000)
            {
                linnumber = "0" + row_count;
            }
            return linnumber;
        }

        [HttpPost]
        public JsonResult GetPrintLabel(string ID)
        {
            IList<ReceiptDetail> Receipts = new List<ReceiptDetail>();
            if (ID.Contains(","))
            {
                string[] strs = ID.Split(',');
                foreach (var str in strs)
                {
                    Receipts.Add(new ReceiptDetail
                    {
                        ID = str.ObjectToInt64()

                    });

                }
            }
            else
            {
                Receipts.Add(new ReceiptDetail
                {
                    ID = ID.ObjectToInt64()
                });

            }
            var response = new ReportManagementService().GetPrintLabel(Receipts);
            if (response.IsSuccess)
            {
                deleteTmpFiles(Server.MapPath("~/TotalImage/"));

                response.Result.receiptPrint.Each((a, b) =>
                {
                    b.StringDateTime1 = b.DateTime1.DateTimeToString("yyyy-MM-dd");
                    b.StringDateTime2 = b.DateTime2.DateTimeToString("yyyy-MM-dd");
                    string strGUID = "Label" + Guid.NewGuid().ToString();
                    b.PictureStr = GetDimensionalCode("[{'GoodsName':" + b.GoodsName + ", 'SKU':" + b.SKU + ",'ProductionDate':" + b.DateTime1.DateTimeToString("yyyy-MM-dd") + ", 'ExpirationDate':" + b.DateTime2.DateTimeToString("yyyy-MM-dd") + ", 'BatchNumber':" + b.BatchNumber + ", 'Manufacturer':" + b.Manufacturer + ",'BoxNumber':" + b.BoxNumber + ", 'QtyExpected':" + b.QtyExpected + ",'NetWeight':''," + "'GrossWeight':'' }]", strGUID + ".jpg");
                    b.PageIndex = "page" + (a + 1);

                });
                return Json(new { ErrorCode = "1", Response = response.Result.receiptPrint });
            }
            else
            {
                return Json(new { ErrorCode = "0" });
            }
        }

        [HttpPost]
        public JsonResult PrintUpdateNumber(long ID, string BoxNumber, long EverySum)
        {
            var response = new ReportManagementService().PrintUpdateNumber(ID, BoxNumber, EverySum);
            if (response.IsSuccess)
            {
                return Json(new { ErrorCode = "1", Response = response.Result.receiptPrint });
            }
            else
            {
                return Json(new { ErrorCode = "0", Message = response });
            }

        }

        //private string create_two(string nr)
        //{
        //    Bitmap bt;
        //    string enCodeString = nr;
        //    QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
        //    qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
        //    qrCodeEncoder.QRCodeScale = 4;//大小(值越大生成的二维码图片像素越高)      
        //    qrCodeEncoder.QRCodeVersion = 0;//版本(注意：设置为0主要是防止编码的字符串太长时发生错误) 
        //    qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
        //     bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
        //    return bt.ToString();
        //}
        private string GetDimensionalCode(string link, string filename)
        {
            Bitmap bmp = null;

            try
            {
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeScale = 4;
                //int version = Convert.ToInt16(cboVersion.Text);
                qrCodeEncoder.QRCodeVersion = 0;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                bmp = qrCodeEncoder.Encode(link, Encoding.UTF8);
                Image ima = bmp;
                string paths = Server.MapPath("~/TotalImage/");
                ima.Save(paths + filename);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Invalid version !");
            }

            return filename;
        }

        private void deleteTmpFiles(string strPath)
        {
            //删除这个目录下的所有子目录
            //if (SysIO.Directory.GetDirectories(strPath).Length> 0)
            //{
            //    foreach (string var in SysIO.Directory.GetDirectories(strPath))
            //    {
            //        //DeleteDirectory(var);
            //        SysIO.Directory.Delete(var, true);
            //        //DeleteDirectory(var);
            //    }
            //}
            //删除这个目录下的所有文件
            if (SysIO.Directory.GetFiles(strPath).Length > 0)
            {
                foreach (string va in SysIO.Directory.GetFiles(strPath))
                {
                    if (va.Contains("Label"))
                    {
                        SysIO.File.Delete(va);
                    }
                }
            }
        }

        /// <summary>
        /// 快递单报表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReportExpressInfo(long? customerID, long? warehouseID, long? warehouseAreaID, int? PageIndex)
        {
            ReportExpressInfoViewModel vm = new ReportExpressInfoViewModel();//model
            vm.ProjectRoleID = base.UserInfo.ProjectID;//projdectid
            vm.SearchCondition = new ReportExpressInfoSearchCondition();//报表查询条件
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.SearchCondition.StartReportDate = Convert.ToDateTime(DateTime.Now.ToString()).AddDays(-10);
            vm.SearchCondition.EndReportDate = Convert.ToDateTime(DateTime.Now.ToString());

            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);//用户可访问的customerID
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }

                }
            }
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            GetReportExpressBySearchConditionRequest request = new GetReportExpressBySearchConditionRequest();//查询条件和页索引
            request.ExpressSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            //首次加载页面暂不查询
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        /// <summary>
        /// 快递单报表查询与导出
        /// </summary>
        /// <param name="response"></param>
        /// <param name="columnReceipt"></param>
        public ActionResult ReportExpressInfo(ReportExpressInfoViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }

            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;


            GetReportExpressBySearchConditionRequest request = new GetReportExpressBySearchConditionRequest();
            request.ExpressSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;

            Response<GetReportExpressBySearchConditionResponse> response = new Response<GetReportExpressBySearchConditionResponse>();
            if (Action == "导出")
            {
                response = new ReportManagementService().ExportResponExpressBySearchCondition(request);
            }
            else
            {
                response = new ReportManagementService().GetResponExpressBySearchCondition(request);
            }
            if (response.IsSuccess)
            {
                vm.ReportExpressChangeCollection = response.Result.ReprotExpressInfoCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                vm.ProjectRoleID = base.UserInfo.ProjectID;
                if (Action == "导出")
                {
                    IEnumerable<Column> columnReceipt;//配置列
                    //获取配置表字段
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module moudle = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ResponExpress").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ResponExpress").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = moudle.Tables.TableCollection.First(t => t.Name == "WMS_ResponExpress").ColumnCollection;
                    }

                    ExportReportExpress(response.Result, columnReceipt);
                }
            }
            else
            {
                vm.ReportExpressChangeCollection = response.Result.ReprotExpressInfoCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                vm.ProjectRoleID = base.UserInfo.ProjectID;

            }
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        private void ExportReportExpress(GetReportExpressBySearchConditionResponse response, IEnumerable<Column> columnExpress)
        {
            IEnumerable<ReportExpressInfo> express = response.ReprotExpressInfoCollection;
            DataSet ds = new DataSet();
            DataTable dtExpress = new DataTable();
            foreach (var receipt in columnExpress)
            {
                dtExpress.Columns.Add(receipt.DisplayName, typeof(string));
            }

            express.Each((i, s) =>
            {
                DataRow drexpress = dtExpress.NewRow();
                foreach (var expre in columnExpress)
                {
                    drexpress[expre.DisplayName] = typeof(Runbow.TWS.Entity.WMS.Report.ReportExpressInfo).GetProperty(expre.DbColumnName).GetValue(s);
                }
                dtExpress.Rows.Add(drexpress);
            });

            dtExpress.TableName = "快递单报表";
            ds.Tables.Add(dtExpress);
            ExportDataToExcelHelper.ExportExcel(ds.Tables[0], "快递单报表信息" + DateTime.Now.ToString("yyyy-MM-dd"), "快递单报表");

        }

        /// <summary>
        /// 出库单差异报表
        ///  </summary>
        /// <param name="customerID"></param>
        /// <param name="warehouseID"></param>
        /// <param name="warehouseAreaID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReportTableDifference(long? customerID, long? warehouseID, long? warehouseAreaID, int? PageIndex)
        {
            //用的是出库单报表的model
            ReportTableViewModel vm = new ReportTableViewModel();
            vm.ProjectRoleID = base.UserInfo.ProjectID;
            vm.SearchCondition = new ReportTableSearchCondition();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.SearchCondition.StartReportDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString()).AddDays(-10);
            vm.SearchCondition.EndReportDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }

            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            GetReportTableBySearchConditionRequest requset = new GetReportTableBySearchConditionRequest();
            requset.TableSearchCondition = vm.SearchCondition;
            requset.PageSize = UtilConstants.PAGESIZE;
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        [HttpPost]
        public ActionResult ReportTableDifference(ReportTableViewModel vm, int? PageIndex, string Action)
        {
            vm.ProjectRoleID = base.UserInfo.ProjectID;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }

            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;

            GetReportTableBySearchConditionRequest request = new GetReportTableBySearchConditionRequest();
            request.TableSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;

            Response<GetReportTableBySearchConditionResponse> response = new Response<GetReportTableBySearchConditionResponse>();

            if (Action == "导出")
            {
                IEnumerable<WMSConfig> wms = null;
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("ExportReportTableDifferenceBySearch_" + base.UserInfo.ProjectName);
                }
                catch
                {
                }
                if (wms == null)
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("ExportReportTableDifferenceBySearch");
                }
                response = new ReportManagementService().ExportResponTableDifferenceBySearch(request, wms.FirstOrDefault().Name);
            }
            else
            {

                IEnumerable<WMSConfig> wms = null;
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("GetReportTableDifferenceBySearch_" + base.UserInfo.ProjectName);
                }
                catch
                {
                }
                if (wms == null)
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("GetReportTableDifferenceBySearch");
                }
                response = new ReportManagementService().GetResponTableDifferenceBySearch(request, wms.FirstOrDefault().Name);
            }

            if (response.IsSuccess)
            {
                vm.ReportTableChangeCollection = response.Result.ReprotTableInCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {

                    IEnumerable<Column> columnReceipt;

                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ResponTableDiffer").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ResponTable").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_ResponTableDiffer").ColumnCollection;
                    }
                    ExportReportTableDiffer(response.Result, columnReceipt);

                }
            }
            if (response.IsSuccess)
            {
                vm.ReportTableChangeCollection = response.Result.ReprotTableInCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                vm.ProjectRoleID = base.UserInfo.ProjectID;

            }
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        /// <summary>
        /// 出库单差异报表
        /// </summary>
        /// <param name="response"></param>
        /// <param name="columnReceipt"></param>
        private void ExportReportTableDiffer(GetReportTableBySearchConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReprotTableIn> receipts = response.ReprotTableInCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReprotTableIn).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "出库单差异报表";
            ds.Tables.Add(dtReceipt);
            ExportDataToExcelHelper.ExportExcel(ds.Tables[0], "出库单差异报表信息" + DateTime.Now.ToString("yyyy-MM-dd"), "出库差异报表");

        }

        private void ExportReportReceiptDiffer(GetReceiptDetailByConditionResponse response, IEnumerable<Column> columnsreceiptdiffer)
        {
            IEnumerable<ReportReceiptReport> receiptdiffer = response.ReceiptDetailCollection3;

            DataSet ds = new DataSet();
            DataTable dtreceiptdiffer = new DataTable();

            foreach (var receipt in columnsreceiptdiffer)
            {
                dtreceiptdiffer.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receiptdiffer.Each((a, b) =>
            {
                DataRow dr = dtreceiptdiffer.NewRow();
                foreach (var item in columnsreceiptdiffer)
                {
                    dr[item.DisplayName] = typeof(Runbow.TWS.Entity.ReportReceiptReport).GetProperty(item.DbColumnName).GetValue(b);

                }
                dtreceiptdiffer.Rows.Add(dr);
            });
            dtreceiptdiffer.TableName = "入库差异报表";
            ds.Tables.Add(dtreceiptdiffer);
            ExportDataToExcelHelper.ExportExcel(ds.Tables[0], "入库单差异报表信息" + DateTime.Now.ToString("yyyy-MM-dd"), "入库差异报表");
        }

        /// <summary>
        /// 入库单差异报表
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReportReceiptDifference(int? PageIndex, long? customerID)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            IndexViewModel vm = new IndexViewModel();
            vm.SearchCondition = new ReceiptSearchCondition();
            vm.SearchCondition.StartCompleteDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString()).AddDays(-10);

            vm.SearchCondition.EndCompleteDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            GetReceiptByConditionRequest getReceiptByConditionRequest = new GetReceiptByConditionRequest();
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                StringBuilder sb = new StringBuilder();

                foreach (var i in CustomerListID)
                {
                    sb.Append("" + i + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.SearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            ViewBag.WarehouseList = WarehouseList;
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            }
            getReceiptByConditionRequest.SearchCondition = vm.SearchCondition;
            getReceiptByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getReceiptByConditionRequest.PageIndex = PageIndex ?? 0;

            ViewBag.ProjectName = base.UserInfo.ProjectName;

            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        /// <summary>
        /// 入库差异报表查询与导出
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="PageIndex"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReportReceiptDifference(IndexViewModel vm, int? PageIndex, string Action)
        {
            vm.ProjectRoleID = base.UserInfo.ProjectID;

            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            ViewBag.CustomerList = CustomerList;//客户列表
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                StringBuilder sb = new StringBuilder();

                foreach (var i in CustomerListID)
                {
                    sb.Append("" + i + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.SearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            ViewBag.WarehouseList = WarehouseList;//仓库列表

            //查询条件中给仓库赋值
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(m => m.Text).FirstOrDefault();
            }

            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getReceiptByConditionRequest = new GetReceiptByConditionRequest();

            getReceiptByConditionRequest.SearchCondition = vm.SearchCondition;
            getReceiptByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getReceiptByConditionRequest.PageIndex = PageIndex ?? 0;

            Response<GetReceiptDetailByConditionResponse> getReceiptByConditionResponse = new Response<GetReceiptDetailByConditionResponse>();

            if (Action == "导出")
            {
                IEnumerable<WMSConfig> wms = null;
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("ExportReportReceiptDifferBySearch_" + base.UserInfo.ProjectName);
                }
                catch
                {

                }
                if (wms == null)
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("ExportReportReceiptDifferBySearch");
                }

                getReceiptByConditionResponse = new ReportManagementService().ExportReportReceiptDifferBySearch(getReceiptByConditionRequest, wms.FirstOrDefault().Name);
            }
            else
            {
                IEnumerable<WMSConfig> wms = null;
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("GetReportReceiptDifferBySearch_" + base.UserInfo.ProjectName);
                }
                catch
                {

                }
                if (wms == null)
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("GetReportReceiptDifferBySearch");
                }

                getReceiptByConditionResponse = new ReportManagementService().GetReportReceiptDiffBySearch(getReceiptByConditionRequest, wms.FirstOrDefault().Name);

            }

            if (getReceiptByConditionResponse.IsSuccess)
            {
                vm.ReceiptDetailCollection2 = getReceiptByConditionResponse.Result.ReceiptDetailCollection3;
                vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
                vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
                if (Action == "导出")
                {
                    IEnumerable<Column> columnReceipt;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptReportDiffer").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReportDiffer").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReportDiffer").ColumnCollection;
                    }
                    ExportReportReceiptDiffer(getReceiptByConditionResponse.Result, columnReceipt);

                }
            }

            if (getReceiptByConditionResponse.IsSuccess)
            {
                vm.ReceiptDetailCollection2 = getReceiptByConditionResponse.Result.ReceiptDetailCollection3;
                vm.PageIndex = getReceiptByConditionResponse.Result.PageIndex;
                vm.PageCount = getReceiptByConditionResponse.Result.PageCount;
                vm.ProjectRoleID = base.UserInfo.ProjectID;

            }
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion

            return View(vm);
        }

        /// <summary>
        /// SKU变动报表（出入库，调整，移库）
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public ActionResult SKUChangeReport(int? PageIndex, long? customerID)
        {

            SKUChangeViewModel vm = new SKUChangeViewModel();
            vm.ProjectRoleID = base.UserInfo.ProjectID;
            vm.SearchCondition = new ReportSKUChangeSearchCondition();
            vm.SearchCondition.StartCreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd HH:mm:ss"));
            vm.SearchCondition.EndCreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ObjectToNullableDateTime();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                    else
                    {
                        vm.SearchCondition.CustomerID = 0;
                    }
                }
            }
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == 0)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            if (WarehouseList != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            ViewBag.WarehouseList = WarehouseList;
            GetSKUChangeBySearchConditionRequest request = new GetSKUChangeBySearchConditionRequest();
            request.SKUChangeSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            //加载时不查询
            return View(vm);

        }

        /// <summary>
        /// SKU变动报表
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="PageIndex"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SKUChangeReport(SKUChangeViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            if (vm.SearchCondition.Warehouse != null)
            {

            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }
            ViewBag.WarehouseList = WarehouseList;
            GetSKUChangeBySearchConditionRequest request = new GetSKUChangeBySearchConditionRequest();
            request.SKUChangeSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            Response<GetSKUChangeBySearchConditionResponse> response = new Response<GetSKUChangeBySearchConditionResponse>();
            if (Action == "导出")
            {
                response = new ReportManagementService().ExportSKUChangeBySearchCondition(request);
            }
            else
            {

                response = new ReportManagementService().GetSKUChangeBySearchCondition(request);
            }
            if (response.IsSuccess)
            {
                vm.ReportSKUChangeCollection = response.Result.SkuChangeCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {
                    IEnumerable<Column> columnReceipt;

                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_SkuChangeReport").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_SkuChangeReport").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_SkuChangeReport").ColumnCollection;
                    }
                    ExportSkuChange(response.Result, columnReceipt);
                }
            }

            return View(vm);
        }


        /// <summary>
        /// 使用EPPlus导出Excel(xlsx)
        /// </summary>
        /// <param name="sourceTable">数据源</param>
        /// <param name="strFileName">xlsx文件名(不含后缀名)</param>
        public static void ExportByEPPlus(DataTable sourceTable, string strFileName)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                string sheetName = string.IsNullOrEmpty(sourceTable.TableName) ? "Sheet1" : sourceTable.TableName;
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(sourceTable, true);

                //Format the row
                ExcelBorderStyle borderStyle = ExcelBorderStyle.Thin;
                Color borderColor = Color.FromArgb(155, 155, 155);

                using (ExcelRange rng = ws.Cells[1, 1, sourceTable.Rows.Count + 1, sourceTable.Columns.Count])
                {
                    rng.Style.Font.Name = "宋体";
                    rng.Style.Font.Size = 10;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));

                    rng.Style.Border.Top.Style = borderStyle;
                    rng.Style.Border.Top.Color.SetColor(borderColor);

                    rng.Style.Border.Bottom.Style = borderStyle;
                    rng.Style.Border.Bottom.Color.SetColor(borderColor);

                    rng.Style.Border.Right.Style = borderStyle;
                    rng.Style.Border.Right.Color.SetColor(borderColor);
                }

                //Format the header row
                using (ExcelRange rng = ws.Cells[1, 1, 1, sourceTable.Columns.Count])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(234, 241, 246));  //Set color to dark blue
                    rng.Style.Font.Color.SetColor(Color.FromArgb(51, 51, 51));
                }

                //Write it back to the client
                ss.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                ss.HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment;  filename={0}.xlsx", HttpUtility.UrlEncode(strFileName, Encoding.UTF8)));
                ss.HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;

                ss.HttpContext.Current.Response.BinaryWrite(pck.GetAsByteArray());
                ss.HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 出入库进程报表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ProcessReport(long? customerID)
        {
            return View();
        }

        /// <summary>
        /// 获取下拉绑定的数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetProcessWhere(long? customerID)
        {
            try
            {
                ProcessReportModel vm = new ProcessReportModel();
                //获取下拉
                vm.SearchCondition = new ProcessTrackingSearchCondition();
                vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                vm.SearchCondition.EndCreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
                var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
                var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                vm.CustomerList = CustomerList;
                if (base.UserInfo.UserType == 0)
                {
                    vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        vm.SearchCondition.CustomerID = customerID;
                    }
                    else
                    {
                        var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                        if (customerIDs != null && customerIDs.Count() == 1)
                        {
                            vm.SearchCondition.CustomerID = customerIDs.First();
                        }
                        else
                        {
                            vm.SearchCondition.CustomerID = 0;
                        }
                    }
                }
                //获取门店下拉
                if (vm.SearchCondition.CustomerID.HasValue)
                {
                    IEnumerable<WMS_Customer> wmscustomers = new WMS_CustomerService().GetWMSCustomerByCustomerID(vm.SearchCondition.CustomerID.ObjectToInt64());
                    if (wmscustomers != null && wmscustomers.Any())
                    {
                        IEnumerable<SelectListItem> wmscustomerList = wmscustomers.Select(m => new SelectListItem { Value = m.StorerKey, Text = m.Company }).ToList();
                        vm.StoresList = wmscustomerList;
                    }
                }

                return Json(new
                {
                    data = vm,
                    code = 0
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    data = "",
                    code = 402
                });
            }
        }

        /// <summary>
        /// 获取进程列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetProcessTrackingList(RequestModel request)
        {
            ResponseModel res = new ResponseModel();
            string msg = string.Empty;
            res.code = 401;
            ProcessTrackingSearchCondition searchCondition = new ProcessTrackingSearchCondition();

            try
            {
                searchCondition = new JavaScriptSerializer().Deserialize<ProcessTrackingSearchCondition>(request.requestData);
            }
            catch (Exception ex)
            {
                msg = "查询条件有误";
                res.code = 402;
                return new JavaScriptSerializer().Serialize(res);
            }
            try
            {
                //查询数据
                if (searchCondition.CustomerID != null && searchCondition.CustomerID > 0)
                {
                    searchCondition.PageIndex = request.page > 0 ? request.page - 1 : 0;
                    searchCondition.PageSize = request.limit > 0 ? request.limit : 30;
                    IEnumerable<WMS_ProcessTracking> list;
                    int rowcounts = 0;
                    list = new ReportManagementService().GetProcessTrackingList(searchCondition, out msg, out rowcounts);
                    if (list != null && list.Any() && msg == "")
                    {
                        res.code = 0;
                        res.count = rowcounts;
                        res.data = list;
                    }
                    else
                    {
                        res.code = 402;
                        res.count = rowcounts;
                        msg = "未查询到数据：" + msg;
                    }
                }
                else
                {
                    msg = "获取数据失败";
                    res.code = 402;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message.ToString();
                res.code = 402;
            }
            res.msg = msg;
            return new JavaScriptSerializer().Serialize(res);
        }

        /// <summary>
        /// 导出进程报表
        /// </summary>        
        public void ExportProcessTracking(string Where)
        {
            string msg = string.Empty;
            ProcessTrackingSearchCondition searchCondition = new ProcessTrackingSearchCondition();
            try
            {
                searchCondition = new JavaScriptSerializer().Deserialize<ProcessTrackingSearchCondition>(Where);
                IEnumerable<WMS_ProcessTracking> list = new ReportManagementService().ExportProcessTracking(searchCondition, out msg);

                if (list != null && list.Any() && string.IsNullOrEmpty(msg))
                {
                    //导出                   
                    ExportProcessTrackingToExcel(list);
                    //return Redirect("/WMS/ReportManagement/ExportProcessTracking");
                }
                else
                {
                    //return Redirect("/WMS/ReportManagement/ExportProcessTracking");
                }
            }
            catch (Exception ex)
            {
                //return Redirect("/WMS/ReportManagement/ExportProcessTracking");
            }

        }

        /// <summary>
        /// 导出到excel
        /// </summary>
        /// <param name="list"></param>
        public void ExportProcessTrackingToExcel(IEnumerable<WMS_ProcessTracking> list)
        {
            DataTable dt = new DataTable();
            dt.TableName = "进程报表";
            dt.Columns.Add("客户", typeof(string));
            dt.Columns.Add("门店代码", typeof(string));
            dt.Columns.Add("门店名称", typeof(string));
            dt.Columns.Add("类型", typeof(string));
            dt.Columns.Add("待处理数量", typeof(string));
            dt.Columns.Add("未包装数量", typeof(string));
            dt.Columns.Add("已完成数量", typeof(string));
            dt.Columns.Add("订单总量", typeof(string));
            dt.Columns.Add("完成率", typeof(string));
            dt.Columns.Add("开始时间", typeof(string));
            dt.Columns.Add("结束时间", typeof(string));
            dt.Columns.Add("创建时间", typeof(string));

            foreach (var item in list)
            {
                DataRow row = dt.NewRow();
                row["客户"] = item.CustomerName;
                row["门店代码"] = item.StorerKey;
                row["门店名称"] = item.Company;
                row["类型"] = item.Type == 1 ? "出库" : "入库";
                row["待处理数量"] = item.Qty1;
                row["未包装数量"] = item.Qty2;
                row["已完成数量"] = item.Qty3;
                row["订单总量"] = item.Qty4;
                row["完成率"] = item.Proportion;
                row["开始时间"] = item.StartTime.ToString();
                row["结束时间"] = item.EndTime.ToString();
                row["创建时间"] = item.CreateTime.ToString();
                dt.Rows.Add(row);
            }
            //ExportDataToExcelHelper.ExportExcel(dt, "进程报表" + DateTime.Now.ToString("yyyyMMddHHmmss"), "进程报表");
            ExportByEPPlus(dt, "进程报表" + DateTime.Now.ToString("yyyyMMddHHmmss"));
        }


        /// <summary>
        /// RSO质检报告
        /// </summary>
        /// <returns></returns>
        public ActionResult InspectionReport()
        {
            InspectionReportViewModel vm = new InspectionReportViewModel();
            vm.SearchCondition = new InspectionReportSearchCondition();

            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            vm.CustomerList = CustomerList;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            vm.WarehouseList = WarehouseList;

            return View(vm);
        }
        /// <summary>
        /// RSO质检报告查询
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InspectionReport(InspectionReportViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            vm.CustomerList = CustomerList;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            vm.WarehouseList = WarehouseList;

            ReportManagementService service = new ReportManagementService();
            GetInspectionReportBySearchConditionRequest request = new GetInspectionReportBySearchConditionRequest()
            {
                SearchCondition = vm.SearchCondition,
                PageIndex = PageIndex ?? 0,
                PageSize = UtilConstants.PAGESIZE
            };
            Response<GetInspectionReportBySearchConditionResponse> response = new Response<GetInspectionReportBySearchConditionResponse>();
            if (Action == "导出")
            {
                response = service.ExportInspectionReportBySearchCondition(request);
            }
            else
            {
                response = service.GetInspectionReportBySearchCondition(request);
            }
            if (response.IsSuccess)
            {
                vm.InspectionReportCollection = response.Result.InspectionReportCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {
                    ExportInspection(response.Result.InspectionReportCollection);
                }
            }
            return View(vm);
        }

        public void ExportInspection(IEnumerable<InspectionReport> inspectionReports)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("ExternReceiptkey", typeof(string));
            dt.Columns.Add("DUSR02", typeof(string));
            dt.Columns.Add("SKU", typeof(string));
            dt.Columns.Add("QtyExpected", typeof(string));
            dt.Columns.Add("BeforeReceivedQty", typeof(string));

            inspectionReports.Each((i, ip) =>
            {
                DataRow dr = dt.NewRow();
                dr[0] = ip.ExternReceiptNumber;
                dr[1] = ip.GoodsType;
                dr[2] = ip.SKU;
                dr[3] = ip.QtyExpected;
                dr[4] = ip.QtyReceived;

                dt.Rows.Add(dr);
            });
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("ExternReceiptkey", typeof(string));
            dt1.Columns.Add("DUSR02", typeof(string));
            dt1.Columns.Add("SKU", typeof(string));
            dt1.Columns.Add("QtyExpected", typeof(string));
            dt1.Columns.Add("BeforeReceivedQty", typeof(string));
            dt1.Columns.Add("BoxNum", typeof(string));

            inspectionReports.Each((i, ip) =>
            {
                DataRow dr = dt1.NewRow();
                dr[0] = ip.ExternReceiptNumber;
                dr[1] = ip.GoodsType;
                dr[2] = ip.SKU;
                dr[3] = ip.QtyExpected;
                dr[4] = ip.QtyReceived;
                dr[5] = ip.str2;

                dt1.Rows.Add(dr);
            });

            ds.Tables.Add(dt);
            ds.Tables.Add(dt1);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "RSO质检报告" + DateTime.Now.ToString("yyyy-MM-ddHHmmss"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "RSO质检报告" + DateTime.Now.ToString("yyyy-MM-ddHHmmss"));
        }


        /// <summary>
        /// 出库耗材导出
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="warehouseID"></param>
        /// <param name="warehouseAreaID"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OrderConsumable(long? customerID, long? warehouseID, long? warehouseAreaID, int? PageIndex)
        {
            ReportTableViewModel vm = new ReportTableViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.OrderType = st;

            vm.ProjectRoleID = base.UserInfo.ProjectID;
            vm.SearchCondition = new ReportTableSearchCondition();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.SearchCondition.StartCompleteDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString()).AddDays(-10);
            vm.SearchCondition.EndCompleteDate = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.SearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();
            //ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            GetReportTableBySearchConditionRequest request = new GetReportTableBySearchConditionRequest();
            request.TableSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            //   var response = new WMS_CustomerService().GetCustomerByConditon(new GetWMS_CustomerByConditionRequest() { StorerKey = string.IsNullOrEmpty(vm.StorerKey) ? "" : vm.StorerKey, CustomerID = string.IsNullOrEmpty(vm.CustomerID.ToString()) ? "" : vm.CustomerID.ToString(), Contact1 = string.IsNullOrEmpty(vm.Contact1) ? "" : vm.Contact1, PhoneNum = string.IsNullOrEmpty(vm.PhoneNum1) ? "" : vm.PhoneNum1, Company = string.IsNullOrEmpty(vm.Company) ? "" : vm.Company, PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0 });
            #region 查询方法
            //var response = new ReportManagementService().GetResponTableBySearchCondition(request);
            //if (response.IsSuccess)
            //{
            //vm.ReportTableChangeCollection = response.Result.ReprotTableInCollection;
            //vm.PageIndex = response.Result.PageIndex;
            //vm.PageCount = response.Result.PageCount;
            //vm.ProjectRoleID = base.UserInfo.ProjectID;
            //}
            #endregion
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        /// <summary>
        /// 出库耗材导出
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="PageIndex"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderConsumable(ReportTableViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }
            var s = ApplicationConfigHelper.GetCacheInfo().Where(a => a.CustomerID == vm.SearchCondition.CustomerID && a.UserID == base.UserInfo.ID).Select(a => a.WarehouseID).FirstOrDefault();
            //ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.WarehouseID == s)).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;

            IEnumerable<WMSConfig> wmss = null;
            try
            {
                wmss = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }

            if (wmss == null)
            {
                wmss = ApplicationConfigHelper.GetWMS_Config("OrderType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wmss)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.OrderType = st;

            GetReportTableBySearchConditionRequest request = new GetReportTableBySearchConditionRequest();
            request.TableSearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;

            Response<GetReportTableBySearchConditionResponse> response = new Response<GetReportTableBySearchConditionResponse>();

            //if (Action == "导出" || Action == "快速导出")
            if (Action == "导出")
            {
                //出库耗材导出报表做成可配置
                IEnumerable<WMSConfig> wms = null;
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("ExportGetOrderConsumable_" + base.UserInfo.ProjectName);
                }
                catch
                {
                }
                if (wms == null)
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("ExportGetOrderConsumable");
                }
                response = new ReportManagementService().ExportGetOrderConsumable(request, wms.FirstOrDefault().Name);
            }
            else
            {
                //出库耗材导出报表做成可配置
                IEnumerable<WMSConfig> wms = null;
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("GetOrderConsumable_" + base.UserInfo.ProjectName);
                }
                catch
                {
                }
                if (wms == null)
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("GetOrderConsumable");
                }
                response = new ReportManagementService().GetOrderConsumable(request, wms.FirstOrDefault().Name);
            }
            if (response.IsSuccess)
            {
                vm.ReportTableChangeCollection = response.Result.ReprotTableInCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {

                    IEnumerable<Column> columnReceipt;

                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_OrderConsumable").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_OrderConsumable").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_OrderConsumable").ColumnCollection;
                    }

                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    ExportReportTable1(response.Result, columnReceipt);
                    return Redirect("/WMS/ReportManagement/OrderConsumable");
                }
            }
            if (response.IsSuccess)
            {
                vm.ReportTableChangeCollection = response.Result.ReprotTableInCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                vm.ProjectRoleID = base.UserInfo.ProjectID;

            }
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }

        /// <summary>
        /// 出库耗材报表导出生成excel文件
        /// </summary>
        /// <param name="response"></param>
        /// <param name="columnReceipt"></param>
        private void ExportReportTable1(GetReportTableBySearchConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<ReprotTableIn> receipts = response.ReprotTableInCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReprotTableIn).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "出库耗材报表";
            ds.Tables.Add(dtReceipt);
            ExportDataToExcelHelper.ExportExcel(ds.Tables[0], "出库耗材报表信息" + DateTime.Now.ToString("yyyy-MM-dd"), "出库耗材报表");
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "出库报表" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

    }
}
