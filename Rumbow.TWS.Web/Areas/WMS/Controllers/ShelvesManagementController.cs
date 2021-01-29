using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using we = System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Runbow.TWS.Biz;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.Entity.WMS.Shelves;
using Runbow.TWS.MessageContracts.WMS.Shelves;
using Runbow.TWS.Web.Areas.WMS.Models.ShelvesManagement;
using Runbow.TWS.Web.Common;
using MyFile = System.IO.File;
using UtilConstants = Runbow.TWS.Common.Constants;
using Runbow.TWS.Common;
using System.Text;
using Runbow.TWS.MessageContracts;
using System.Text.RegularExpressions;
using Runbow.TWS.Web.Interface;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.Entity.WMS.NIKECE.InventorySnapshot;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class ShelvesManagementController : BaseController
    {
        //
        // GET: /WMS/ShelvesManagement/
        [HttpGet]
        public ActionResult Index(bool? hideActionButton, bool? showEditRelated, long customerID = 0)
        {
            //ChangeLocation(22, "A01", "37-01-A", "1");
            ShelvesModel sm = new ShelvesModel();
            ReceiptReceivingSearchCondition Condition = new ReceiptReceivingSearchCondition();
            Session["ShelvesIndexCondition"] = sm.Condition;
            Session["ShelvesIndexPageIndex"] = 0;
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            GetReceiptbyCondition gr = new GetReceiptbyCondition();
            //gr.StartStorageTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd 00:00"));
            //gr.EndStorageTime = DateTime.Now;
            gr.StartCreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd 00:00"));
            gr.EndCreateTime = DateTime.Now;
            gr.CustomerID = customerID;
            if (customerID == 0)
            {
                sm.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            else
            {
                sm.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                       .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            if (sm.WarehouseList.Count() == 1)
            {
                gr.WarehouseID = Convert.ToInt64(sm.WarehouseList.First().Value);
            }
            sm.Condition = gr;
            sm.SearchCondition = Condition;
            sm.HideActionButton = hideActionButton ?? false;
            sm.ShowEditRelated = showEditRelated ?? true;
            sm.StorerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
                                     .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            if (sm.WarehouseList.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in sm.WarehouseList)
                {
                    sb.Append("" + item.Value + ",");
                }
                if (sb.Length > 1)
                {
                    sm.Condition.WarehouseIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            // GetReceiptbyCondition rc = new GetReceiptbyCondition();
            if (customerID == 0)
            {
                StringBuilder sb = new StringBuilder();
                //sm.Customers = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
                //                     .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });

                sm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                foreach (var i in sm.Customers)
                {
                    sb.Append("'" + i.Value + "',");
                }
                if (sb.Length > 1)
                {
                    sm.Condition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }

            if (base.UserInfo.UserType == 0)
            {
                sm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID != 0)
                {
                    sm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        sm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }
            IEnumerable<SelectListItem> WarehouseList = null;
            if (customerID == 0)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });

            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }

            ViewBag.WarehouseList = WarehouseList;
            var WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == sm.SearchCondition.Warehouse && a.OperationType == 2).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });

            ViewBag.WorkStation = WorkStation;

            var response = new ShelvesManagementService().GetReceipt(new Runbow.TWS.MessageContracts.WMS.Shelves.GetReceiptByConditionRequest
            {
                Condition = sm.Condition,
                PageIndex = 0,
                PageSize = UtilConstants.PAGESIZE

            });
            sm.IsInnerUser = sm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            sm.ProjectRoleID = base.UserInfo.ProjectRoleID;
            if (base.UserInfo.UserType == 0)
            {
                sm.Condition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                if (customerIDs != null && customerIDs.Count() == 1)
                {
                    sm.Condition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                }
            }
            if (response.IsSuccess)
            {
                sm.receipt = response.Result.receipt;
                sm.PageIndex = response.Result.PageIndex;
                sm.PageCount = response.Result.PageCount;
            }

            this.GenQueryPodViewModel(sm);
            Session["ShelvesIndexCondition"] = sm.Condition;
            Session["ShelvesIndexPageIndex"] = 0;
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(sm);
        }

        private void GenQueryPodViewModel(ShelvesModel vm)
        {
            var Configs = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.Condition.CustomerID)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection;
            if (Configs.Where(t => t.Name == "WMS_Receipt").Count() == 0)
            {
                vm.Config = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Receipt");
            }
            else
            {
                vm.Config = Configs.First(t => t.Name == "WMS_Receipt");
            }
            //vm.Config = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID)).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Receipt");
            if (base.UserInfo.UserType == 2)
            {
                vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            }
            else if (base.UserInfo.UserType == 0)
            {
                vm.Customers = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            }
            else
            {
                vm.Customers = Enumerable.Empty<SelectListItem>();
            }
        }

        [HttpPost]
        public ActionResult Index(ShelvesModel sm, string Action, bool? hideActionButton, bool? showEditRelated, long? customerID, int? PageIndex)
        {
            if (sm.Condition != null)
            {
                Session["ShelvesIndexCondition"] = sm.Condition;
                Session["ShelvesIndexPageIndex"] = PageIndex;
            }
            else
            {
                if (Session["ShelvesIndexCondition"] == null)
                {
                    sm.Condition = new GetReceiptbyCondition();
                    PageIndex = 0;
                }
                else
                {
                    sm.Condition = Session["ShelvesIndexCondition"] as GetReceiptbyCondition;
                    PageIndex = Session["ShelvesIndexPageIndex"] == null ? 1 : (int)Session["ShelvesIndexPageIndex"];
                }
            }

            ReceiptReceivingSearchCondition Condition = new ReceiptReceivingSearchCondition();
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            sm.HideActionButton = hideActionButton ?? false;
            sm.ShowEditRelated = showEditRelated ?? true;
            sm.StorerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
                                     .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            if (sm.Condition.CustomerID == null)
            {
                sm.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            else
            {
                sm.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == sm.Condition.CustomerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                       .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }


            ViewBag.WarehouseList = sm.WarehouseList;

            sm.SearchCondition = Condition;
            var WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseID == sm.Condition.WarehouseID && a.OperationType == 2).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });

            ViewBag.WorkStation = WorkStation;
            Runbow.TWS.MessageContracts.WMS.Shelves.GetReceiptByConditionRequest Request = new Runbow.TWS.MessageContracts.WMS.Shelves.GetReceiptByConditionRequest();

            if (sm.WarehouseList.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in sm.WarehouseList)
                {
                    sb.Append("" + item.Value + ",");
                }
                if (sb.Length > 1)
                {
                    sm.Condition.WarehouseIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            if (sm.Condition.CustomerID == null)
            {
                StringBuilder sb = new StringBuilder();
                sm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                foreach (var i in sm.Customers)
                {
                    sb.Append("'" + i.Value + "',");
                }
                if (sb.Length > 1)
                {
                    sm.Condition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            if (Action == "查询" || Action == "Index")
            {
                Request.Condition = sm.Condition;
                Request.PageIndex = PageIndex ?? 0;
                Request.PageSize = UtilConstants.PAGESIZE;
            }
            if (Action == "导出已上架信息")
            {
                Request.Condition = sm.Condition;
                Request.PageIndex = 0;
                Request.PageSize = 0;
            }
            var response = new ShelvesManagementService().GetReceipt(Request);
            sm.IsInnerUser = sm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            sm.ProjectRoleID = base.UserInfo.ProjectRoleID;
            if (base.UserInfo.UserType == 0)
            {
                sm.Condition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                if (customerIDs != null && customerIDs.Count() == 1)
                {
                    sm.Condition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                }
            }
            if (response.IsSuccess)
            {
                if (Action == "导出已上架信息")
                {
                    var response_export = new ShelvesManagementService().GetReceiptExport(Request);
                    IEnumerable<Column> columnReceipt;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_CarryOutReceiptReceiving").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_CarryOutReceiptReceiving").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_CarryOutReceiptReceiving").ColumnCollection.Where(a => a.ForView == true);
                    }
                    ExportReceiptReceiving2(response_export.Result.receipt, columnReceipt);
                }
                sm.receipt = response.Result.receipt;
                sm.PageIndex = response.Result.PageIndex;
                sm.PageCount = response.Result.PageCount;
            }
            this.GenQueryPodViewModel(sm);
            return View(sm);
        }

        public JsonResult AddInstructions(string ids, string WorkStation, string WarehouseQueue, int Priority = 0)
        {

            var response = new ShelvesManagementService().AddInstructions(ids, WorkStation.Trim(), "2", Priority, UserInfo.Name);
            if (response.IsSuccess)
            {
                //try
                //{
                //    //int RandKey = new Random().Next(0, 11);
                //    foreach (var item in response.Result.instructionInfo)
                //    {
                //        RabbitSender rs = new RabbitSender(new RabbitSenderOption(new { ID = item.id, X = item.x, Y = item.y, ReleatedDetailID = item.releatedDetailID, WorkStation = item.workStation }, WarehouseQueue.Trim(), WarehouseQueue.Trim(), WarehouseQueue.Trim(), WarehouseQueue.Trim(), Convert.ToByte(Priority)));
                //        rs.Send();
                //    }
                //    var UpdateResults = new OrderManagementService().UpdateResults(ids, UserInfo.Name);
                //    if (UpdateResults)
                //    {
                //        return Json(new { Code = 1 });
                //    }
                //}
                //catch (Exception)
                //{
                //    return Json(new { Code = 0 });
                //}
                return Json(new { Code = 1 });
            }
            else
            {
                return Json(new { Code = -1, Message = response.SuccessMessage });
            }
            return Json(new { Code = 0 });
        }

        //没用了 bobo
        private void Export(IEnumerable<StoresByGetReceipt> crmShippers)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("入库单号", typeof(string));
            dt.Columns.Add("入库日期", typeof(DateTime));
            dt.Columns.Add("外部单号", typeof(string));
            dt.Columns.Add("客户", typeof(string));
            dt.Columns.Add("仓库", typeof(string));
            dt.Columns.Add("上架日期", typeof(DateTime));
            dt.Columns.Add("上架状态", typeof(string));
            dt.Columns.Add("SKU", typeof(string));
            dt.Columns.Add("UPC", typeof(string));
            dt.Columns.Add("行号", typeof(string));
            dt.Columns.Add("SKU行号", typeof(string));
            dt.Columns.Add("品名", typeof(string));
            dt.Columns.Add("品级", typeof(string));
            dt.Columns.Add("数量", typeof(int));
            dt.Columns.Add("库区", typeof(string));
            dt.Columns.Add("库位", typeof(string));
            dt.Columns.Add("批次", typeof(string));
            dt.Columns.Add("箱号", typeof(string));
            dt.Columns.Add("单位", typeof(string));
            dt.Columns.Add("Specifications", typeof(string));
            dt.Columns.Add("Remark", typeof(string));
            crmShippers.Each((i, s) =>
            {
                DataRow dr = dt.NewRow();
                dr[0] = s.ReceiptNumber;
                dr[1] = s.ReceiptDate;
                dr[2] = s.ExternReceiptNumber;
                dr[3] = s.CustomerName;
                dr[4] = s.WarehouseName;
                dr[5] = s.CreateTime;//s.CreateTime.HasValue ? s.CreateTime.Value.DateTimeToString() : "";
                dr[7] = s.SKU;
                dr[8] = s.UPC;
                dr[9] = s.LineNumber;
                dr[10] = s.SkuLineNumber;
                dr[11] = s.GoodsName;
                dr[12] = s.GoodsType;
                dr[13] = s.QtyReceived;
                dr[14] = s.Area;
                dr[15] = s.Location;
                dr[16] = s.BatchNumber;
                dr[17] = s.BoxNumber;
                dr[18] = s.Unit;
                dr[19] = s.Specifications;
                dr[20] = s.Remark;
                switch (s.Status)
                {
                    case 1:
                        dr[6] = "待上架 ";
                        break;
                    case 5:
                        dr[6] = "已上架 ";
                        break;
                    case 9:
                        dr[6] = "已入库 ";
                        break;
                    case -1:
                        dr[6] = "取消 ";
                        break;
                    default:
                        dr[6] = "";
                        break;
                }

                dt.Rows.Add(dr);
            });

            //ExportDataToExcelHelper.ExportDataSetToExcel(dt, "上架单" + DateTime.Now.ToString("yyyy-MM-dd"), null);
            EPPlusOperation.ExportByEPPlus(dt, "上架单" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        //导出选中上架单
        public void ExportReceiptReceving(string ids, long? CustomerID, string type)
        {
            if (type == "1")
            {
                var response = new ShelvesManagementService().GetShelvesByIDs(ids);//.GetShelvesByIDs(ids);
                IEnumerable<Column> columnReceipt;
                var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
                Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_CarryOutReceiptReceiving").Count() == 0)
                {
                    columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_CarryOutReceiptReceiving").ColumnCollection;
                }
                else
                {
                    columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_CarryOutReceiptReceiving").ColumnCollection.Where(a => a.ForView == true);
                }
                ExportReceiptReceiving2(response.Result.receipt, columnReceipt);
            }
            else
            {
            }
        }

        private void ExportReceiptReceiving2(IEnumerable<StoresByGetReceipt> response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<StoresByGetReceipt> receipts = response;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ProductLevel_" + base.UserInfo.ProjectName);


            }
            catch
            {
            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("GoodsTypes");


            }
            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {
                    if (receipt.DbColumnName == "GoodsType")
                    {
                        drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.WMS.Shelves.StoresByGetReceipt).GetProperty(receipt.DbColumnName).GetValue(s);
                        if (drReceipt[receipt.DisplayName].ToString() != "")
                        { }
                        else
                        {
                            //如果没有品级 则获取默认品级
                            drReceipt[receipt.DisplayName] = wms.FirstOrDefault().Name;
                        }
                    }
                    else
                    {
                        drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.WMS.Shelves.StoresByGetReceipt).GetProperty(receipt.DbColumnName).GetValue(s);
                    }
                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "上架单信息";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "已上架单信息" + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "已上架单信息" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        //导出上架单模板
        public void ExportReceiptReceiving()//GetReceiptDetailByConditionResponse response,
        {
            IEnumerable<Column> columnReceipt;

            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReceiptReceiving").Count() == 0)
            {
                columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving").ColumnCollection;
            }
            else
            {
                columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_ReceiptReceiving").ColumnCollection;
            }

            //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
            //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            //IEnumerable<Table> tables = module.Tables.TableCollection;
            //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_ReceiptReceiving").ColumnCollection;
            // ExportReceiptReceiving(columnReceipt);//getReceiptByConditionResponse.Result, 
            IEnumerable<ReceiptDetail> receipts = null;
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
                    if (receipt.DbColumnName == "Warehouse")
                    {
                        drReceipt[receipt.DisplayName] = "";
                    }
                    else if (receipt.DbColumnName == "Area")
                    {
                        drReceipt[receipt.DisplayName] = "";
                    }
                    else if (receipt.DbColumnName == "Location")
                    {
                        drReceipt[receipt.DisplayName] = "";
                    }
                    else if (receipt.DbColumnName == "Remark")
                    {
                        drReceipt[receipt.DisplayName] = "";
                    }
                    else if (receipt.DbColumnName == "GoodsType")
                    {
                        drReceipt[receipt.DisplayName] = "A品";
                    }
                    else
                    {
                        drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.ReceiptDetail).GetProperty(receipt.DbColumnName).GetValue(s);
                    }
                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "上架单信息";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "上架单模板" + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "上架单模板" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        [HttpGet]
        public ActionResult ReceiptReceivingDetail(long RID)
        {
            ShelvesModel sm = new ShelvesModel();
            GetShelvesByConditionRequest GetShelvesRequest = new GetShelvesByConditionRequest();
            ReceiptReceivingSearchCondition rs = new ReceiptReceivingSearchCondition();
            rs.RID = RID;
            GetShelvesRequest.SearchCondition = rs;
            ViewBag.Project = base.UserInfo.ProjectID;
            ViewBag.RID = RID;
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ProductLevel_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ProductLevel");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }

            ViewBag.selectList = st;
            // if (Status == 0)
            // {

            var response = new ShelvesManagementService().GetShelves(GetShelvesRequest);
            if (response.IsSuccess)
            {
                var WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == response.Result.storesByGetReceipt.WarehouseName && a.OperationType == 2).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });

                ViewBag.WorkStation = WorkStation;
                sm.storesByGetReceipt = response.Result.storesByGetReceipt;
                sm.Shelves = response.Result.Shelves;
            }
            // }
            //else if (Status == 1)
            //{
            //    var response = new ShelvesManagementService().GetReceiptReceiving(GetShelvesRequest);
            //    if (response.IsSuccess)
            //    {
            //        sm.storesByGetReceipt = response.Result.storesByGetReceipt;
            //        sm.Shelves = response.Result.Shelves;
            //    }
            //}
            return View(sm);

        }

        //public JsonResult asdasdadasd(long RID)
        //{
        //    ShelvesModel sm = new ShelvesModel();
        //    GetShelvesByConditionRequest GetShelvesRequest = new GetShelvesByConditionRequest();
        //    ReceiptReceivingSearchCondition rs = new ReceiptReceivingSearchCondition();
        //    rs.RID = RID;
        //    GetShelvesRequest.SearchCondition = rs;
        //    ViewBag.Project = base.UserInfo.ProjectID;
        //    // if (Status == 0)
        //    // {

        //    var response = new ShelvesManagementService().GetShelves(GetShelvesRequest);
        //    if (response.IsSuccess)
        //    {
        //        sm.storesByGetReceipt = response.Result.storesByGetReceipt;
        //        sm.Shelves = response.Result.Shelves;
        //    }
        //    // }
        //    //else if (Status == 1)
        //    //{
        //    //    var response = new ShelvesManagementService().GetReceiptReceiving(GetShelvesRequest);
        //    //    if (response.IsSuccess)
        //    //    {
        //    //        sm.storesByGetReceipt = response.Result.storesByGetReceipt;
        //    //        sm.Shelves = response.Result.Shelves;
        //    //    }
        //    //}
        //    return Json(new { data = sm.Shelves });
        //}
        /// <summary>
        /// 库区智能检索
        /// </summary>
        /// <param name="id">仓库ID</param>
        /// <param name="name">库区名字</param>
        /// <returns></returns>
        public JsonResult ChangeArea(long id, string name, string type)
        {
            // ApplicationConfigHelper.RefreshGetWarehouseAreaList(str);
            var Area = ApplicationConfigHelper.GetWarehouseAreaList(id);
            if (type == "keydown")
            {
                return Json(Area.Where(s => s.AreaName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.AreaName }), JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(Area.Where(s => s.AreaName == name).Select(t => new { Value = t.ID.ToString(), Text = t.AreaName }), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult selectList()
        {
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ProductLevel_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }

            if (wms.Count() <= 0 || wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ProductLevel");
            }
            return Json(wms);
        }

        /// <summary>
        /// 库位智能检索
        /// </summary>
        /// <param name="id">仓库ID</param>
        /// <param name="name">库区名字</param>
        /// <returns></returns>
        public JsonResult ChangeLocation(long id, string AreaName, string name, string type)
        {
            string WarehouseName = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(a => a.WarehouseID == id).Select(b => b.WarehouseName).FirstOrDefault();
            var Location = ApplicationConfigHelper.GetWarehouseLocationListByWarehouseName(WarehouseName);
            if (type == "keydown")
            {
                // ApplicationConfigHelper.RefreshGetWarehouseAreaList(str);
                return Json(Location.Where(s => s.AreaName.IndexOf(AreaName, StringComparison.OrdinalIgnoreCase) >= 0 && s.Location.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.Location }), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(Location.Where(s => s.AreaName.IndexOf(AreaName, StringComparison.OrdinalIgnoreCase) >= 0 && s.Location == name).Select(t => new { Value = t.ID.ToString(), Text = t.Location }), JsonRequestBehavior.AllowGet);
            }
        }

        public string StagingReceipt(string CustomerID, string Jaonstr)
        {
            bool IsSuccess = false;
            bool validation = true;
            StringBuilder result = new StringBuilder();
            try
            {
                var responses = JSONStringToList<ReceiptReceiving>(Jaonstr);

                #region 验证入库单是否取消
                IEnumerable<WMSConfig> wms = null;
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("OrderCancel_" + base.UserInfo.ProjectName);
                }
                catch
                {

                }
                if (wms == null)
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("OrderCancel");
                }
                var message = string.Empty;

                try
                {
                    DeliverConfirmService deliverConfirmService = new DeliverConfirmService();
                    message = deliverConfirmService.ValidOrderCancel(responses.FirstOrDefault().ExternReceiptNumber, Convert.ToInt64(CustomerID), wms.FirstOrDefault().Name, responses.FirstOrDefault().Warehouse, 3);
                    if (!string.IsNullOrEmpty(message))
                    {
                        return new { result = "已取消，无法上架！", IsSuccess = IsSuccess }.ToJsonString();
                    }
                }
                catch (Exception ex)
                {
                    return new { result = ex.Message, IsSuccess = IsSuccess }.ToJsonString();
                }
                #endregion

                List<ProductSearch> ListPs = new List<ProductSearch>();
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                responses.Each((i, data) =>
                {
                    ProductSearch ps = new ProductSearch();
                    ps.SKU = data.SKU;
                    ps.UPC = data.UPC;
                    ListPs.Add(ps);

                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "入库单管理";
                    operation.Operation = "入库单-上架";
                    operation.OrderType = "ReceiptReceiving";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = base.UserInfo.Name;
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = (int)base.UserInfo.ProjectID;
                    operation.ProjectName = base.UserInfo.ProjectName;
                    operation.CustomerID = Convert.ToInt32(data.CustomerID);
                    operation.CustomerName = data.CustomerName;
                    operation.WarehouseName = data.Warehouse;
                    operation.OrderID = data.RID.ToString();
                    operation.ExternOrderNumber = data.ExternReceiptNumber;
                    operation.OrderNumber = data.ReceiptNumber;
                    logs.Add(operation);
                });
                IEnumerable<ProductSearch> resualtProList = ApplicationConfigHelper.GetSearchProduct(Convert.ToInt64(CustomerID), ListPs, "UPC");

                //var ss = ApplicationConfigHelper.GetALLProductStorerList(CustomerID);
                //根据订单获取门店代码配置的库区
                List<string> list_number = new List<string>();
                list_number.Add(responses.FirstOrDefault().ReceiptNumber);
                IEnumerable<WMS_Customer> stores = new ProductService().Get_WMS_CustomerByNumbers(list_number);
                string area = "";
                string area2 = "";
                if (stores != null && stores.Any())
                {
                    area = stores.FirstOrDefault().UserDef2;
                    area2 = stores.FirstOrDefault().UserDef4;//对应的第二个库区
                }

                var list = from q in responses
                           group q by new { q.CustomerName, q.Warehouse, q.Area, q.Location } into n
                           select new
                           {
                               CustomerName = n.Key.CustomerName,
                               Warehouse = n.Key.Warehouse,
                               Area = n.Key.Area,
                               Location = n.Key.Location
                           };
                responses.Each((i, data) =>
                {
                    if (resualtProList.Where(c => (c.SKU == data.SKU && (c.UPC == data.UPC || string.IsNullOrEmpty(data.UPC)))).Select(m => m.GoodsName).FirstOrDefault() == null)
                    {
                        validation = false;
                        result.Append("").Append("第" + (i + 1).ToString() + "行，【" + data.SKU + "】 SKU或者UPC在该客户下不存在！");
                    }

                    //校验库存库区是否存在匹配 并且库区属于门店code中配置库区
                    if ((!string.IsNullOrEmpty(area) && area != data.Area) && (!string.IsNullOrEmpty(area2) && area2 != data.Area))//两个库区都不存在
                    {
                        validation = false;
                        result.Append("第" + (i + 1).ToString() + "行，库区【" + data.Area + "】与门店所在库区【" + area + "," + area2 + "】不匹配！");
                    }


                    if (validationWarehouse(data.CustomerName, data.Warehouse, data.Area, data.Location))
                    {
                        validation = false;
                        result.Append("第" + (i + 1).ToString() + "行，库区【" + data.Area + "】与库位【" + data.Location + "】不匹配！");
                    }
                });
                //NIKE 验证库区库位不匹配门店
                if (base.UserInfo.ProjectName.Equals("NIKE"))
                {
                    if (!validation)
                    {
                        return new { result = result.ToString(), IsSuccess = IsSuccess }.ToJsonString();
                        //return false;
                    }
                }

                var response = new ShelvesManagementService().InsertReceiptReceiving(new GetShelvesByConditionRequest()
                {
                    receiptReceiving = responses,
                    User = base.UserInfo.Name,
                });

                IsSuccess = response;
                if (IsSuccess)
                    new LogOperationService().AddLogOperation(logs);
            }
            catch (Exception ex)
            {
                //return IsSuccess;
                return new { result = ex.ToString(), IsSuccess = IsSuccess }.ToJsonString();
            }
            if (!validation)
                IsSuccess = false;
            if (validation && IsSuccess)
                result.Append("保存成功");
            //return IsSuccess;
            return new { result = result.ToString(), IsSuccess = IsSuccess }.ToJsonString();
        }

        /// <summary>
        /// 比瑞吉项目定制 入库即冻结（未检测状态）没用了 bobo
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string UpdateInventoryType(string receiptnumber, string action)
        {
            string response = "";
            try
            {
                response = new ShelvesManagementService().UpdateInventoryType(receiptnumber, action);
            }
            catch (Exception)
            {
                throw;
            }
            return response;

        }

        public bool AddshelvesAndInventory(string Jaonstr)
        {
            bool IsSuccess = false;
            try
            {
                var responses = JSONStringToList<ReceiptReceiving>(Jaonstr);
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();

                //根据订单获取门店代码配置的库区
                List<string> list_number = new List<string>();
                list_number.Add(responses.FirstOrDefault().ReceiptNumber);
                IEnumerable<WMS_Customer> stores = new ProductService().Get_WMS_CustomerByNumbers(list_number);
                string area = "";
                if (stores != null && stores.Any())
                {
                    area = stores.FirstOrDefault().UserDef2;
                }

                foreach (var item in responses)
                {
                    //验证库区库位
                    var LocationList = ApplicationConfigHelper.GetWarehouseLocationListByWarehouseName(item.Warehouse);
                    var AreaList = ApplicationConfigHelper.GetWarehouseAreaListByWarehouseName(item.Warehouse);
                    if (AreaList.Where(s => s.AreaName == item.Area).Count() == 0)
                        return false;
                    var aList = AreaList.Where(s => s.AreaName == item.Area);
                    var lList = LocationList.Where(s => s.Location == item.Location);
                    if (LocationList.Where(s => s.Location == item.Location).Count() == 0)
                        return false;
                    if (LocationList.Where(s => s.Location == item.Location && s.AreaName == item.Area).Count() == 0)
                        return false;

                    //校验库存库区是否存在匹配 并且库区属于门店code中配置库区
                    if (!string.IsNullOrEmpty(area) && area != item.Area)
                        return false;

                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "入库单管理";//上架管理
                    operation.Operation = "入库单-上架加入库存";//上架-加入库存
                    operation.OrderType = "ReceiptReceiving";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = base.UserInfo.Name;
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = (int)base.UserInfo.ProjectID;
                    operation.ProjectName = base.UserInfo.ProjectName;
                    operation.CustomerID = (int)item.CustomerID;
                    operation.CustomerName = item.CustomerName;
                    operation.OrderID = item.RID.ToString();
                    operation.OrderNumber = item.ReceiptNumber;
                    operation.ExternOrderNumber = item.ExternReceiptNumber;
                    logs.Add(operation);
                }
                var response = new ShelvesManagementService().AddshelvesAndInventory(new GetShelvesByConditionRequest()
                {
                    User = base.UserInfo.Name,
                    receiptReceiving = responses,
                });
                IsSuccess = response;
                if (IsSuccess)
                    new LogOperationService().AddLogOperation(logs);
            }
            catch (Exception)
            {
                throw;
            }
            return IsSuccess;
        }

        //加入库存
        [HttpPost]
        public string AddInventory(string id)
        {
            string response = "";
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            try
            {
                #region MyRegion
                IEnumerable<WMSConfig> wms = null;
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("OrderCancel_" + base.UserInfo.ProjectName);
                }
                catch
                {

                }
                if (wms == null)
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("OrderCancel");
                }
                #endregion

                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "入库单管理";
                operation.Operation = "入库单-上架加入库存";
                operation.OrderType = "ReceiptReceiving";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = id.ToString();
                logs.Add(operation);
                response = new ShelvesManagementService().AddInventory(id, base.UserInfo.Name, wms.FirstOrDefault().Name);
                if (response == "成功")
                {
                    new LogOperationService().AddLogOperation(logs);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        //Nike差异加入库存
        [HttpPost]
        public string AddInventoryWithFreeze(string id)
        {
            string response = "";
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            try
            {
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "入库单管理";
                operation.Operation = "入库单-上架加入库存";
                operation.OrderType = "ReceiptReceiving";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = id.ToString();
                logs.Add(operation);
                response = new ShelvesManagementService().AddInventoryWithFreeze(id, base.UserInfo.Name);
                if (response == "成功")
                {
                    new LogOperationService().AddLogOperation(logs);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }


        /// <summary>
        /// 验证上架数据是否和入库单一致
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckReceiptReceiving(string id)
        {
            List<ReceiptDetail> response;
            try
            {
                response = new ShelvesManagementService().CheckReceiving(id);

                if (response != null)
                {
                    //return response.ToJsonString();
                    return Json(new { Code = 1, data = response });
                }
                else
                {
                    return Json(new { Code = 0 });
                }
            }
            catch (Exception)
            {
                return Json(new { Code = 2 });
            }

        }

        [HttpPost]
        public string CheckRFReceiptReceiving(string id)
        {
            List<ReceiptDetail> response;
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = Int32.MaxValue;
            try
            {
                response = new ShelvesManagementService().CheckRFReceiving(id);

                if (response != null)
                {
                    //return response.ToJsonString();

                    return jsSerializer.Serialize(new { Code = 1, data = response });
                }
                else
                {
                    //return Json(new { Code = 0 });
                    return jsSerializer.Serialize(new { Code = 0, data = response });
                }
            }
            catch (Exception)
            {
                //return Json(new { Code = 2 });
                return jsSerializer.Serialize(new { Code = 2 });
            }

        }

        public void ExportDiffrent(string id)
        {
            List<ReceiptDetail> response;
            try
            {
                response = new ShelvesManagementService().CheckReceiving(id);
                //if (response.Count <= 0)
                //{
                //    return "";
                //}
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                dt.Columns.Add("入库单号");
                dt.Columns.Add("SKU");
                dt.Columns.Add("订单数量");
                dt.Columns.Add("上架数量");
                dt.Columns.Add("差异数量");
                foreach (var item in response)
                {
                    DataRow dr = dt.NewRow();
                    dr["入库单号"] = item.ReceiptNumber;
                    dr["SKU"] = item.SKU;
                    dr["订单数量"] = item.QtyExpected;
                    dr["上架数量"] = item.QtyReceived;
                    dr["差异数量"] = item.QtyReceived - item.QtyExpected;
                    dt.Rows.Add(dr);
                }
                ds.Tables.Add(dt);
                //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "上架差异信息" + DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
                EPPlusOperation.ExportDataSetByEPPlus(ds, "上架差异信息" + DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
            }
            catch (Exception ex)
            {
                //return ex.Message;
            }
        }

        [HttpPost]
        public string BackStatus(string ID, int ToStatus)
        {
            IList<Receipt> Receipts = new List<Receipt>();
            AddReceiptAndReceiptDetailRequest request = new AddReceiptAndReceiptDetailRequest();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            if (ID.Contains(","))
            {
                string[] strs = ID.Split(',');
                foreach (var str in strs)
                {
                    Receipts.Add(new Receipt
                    {
                        ID = str.ObjectToInt64(),
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        ReceiptDate = DateTime.Now,
                        CompleteDate = DateTime.Now
                    });
                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "入库单管理";
                    operation.Operation = "入库单-上架状态回退";
                    operation.OrderType = "ReceiptReceiving";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = base.UserInfo.Name;
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = (int)base.UserInfo.ProjectID;
                    operation.ProjectName = base.UserInfo.ProjectName;
                    operation.OrderID = ID.ToString();
                    operation.Remark = ToStatus.ToString();
                    logs.Add(operation);
                }
            }
            else
            {
                Receipts.Add(new Receipt
                {
                    ID = ID.ObjectToInt64(),
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now,
                    ReceiptDate = DateTime.Now,
                    CompleteDate = DateTime.Now
                });
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "入库单管理";
                operation.Operation = "入库单-上架状态回退";
                operation.OrderType = "ReceiptReceiving";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = ID.ToString();
                operation.Remark = ToStatus.ToString();
                logs.Add(operation);
            }
            request.Receipts = Receipts;
            var GetResponse = new ShelvesManagementService().ReceiptStatusBack(request, ToStatus);
            if (GetResponse.IsSuccess == true)
            {
                new LogOperationService().AddLogOperation(logs);
            }
            return GetResponse.Result;

        }

        public static List<T> JSONStringToList<T>(string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);
            return objs;
        }

        //private string NumList(int row_count)
        //{
        //    var linnumber = "";
        //    if (row_count < 10)
        //    {
        //        linnumber = "0000" + row_count;
        //    }
        //    if (100 > row_count && row_count >= 10)
        //    {
        //        linnumber = "000" + row_count;
        //    }
        //    if (1000 > row_count && row_count >= 100)
        //    {
        //        linnumber = "00" + row_count;
        //    }
        //    if (row_count >= 1000)
        //    {
        //        linnumber = "0" + row_count;
        //    }
        //    return linnumber;
        //}
        public void TransData(string CustomerID, long WareHouseID, ref DataSet transData, ref string message)
        {
            Object[] parameters = new Object[5];
            parameters[0] = "Receiving";
            parameters[1] = Convert.ToInt64(CustomerID);
            parameters[2] = base.UserInfo.ProjectID;
            parameters[3] = ApplicationConfigHelper.GetCacheInfo().First(p => p.UserName == base.UserInfo.Name).WarehouseID;//Bob
            parameters[4] = transData;

            string transDataInstanceName = transDataInstanceNameStr(Convert.ToInt64(CustomerID), WareHouseID);
            ITransData transDataInstance = Activator.CreateInstance(Type.GetType(transDataInstanceName), parameters) as ITransData;
            transData = transDataInstance.TransData(ref message);
        }

        private string transDataInstanceNameStr(long? CustomerID, long WareHouseID)
        {
            if (ApplicationConfigHelper.GetApplicationConfig().ProjectCollection.Where(p => p.Id == base.UserInfo.ProjectID.ToString()).Count() > 0)
            {
                var tranDataConfigCollection = ApplicationConfigHelper.GetApplicationConfig().ProjectCollection
                           .First(p => p.Id == base.UserInfo.ProjectID.ToString())
                           .TransDataConfigs.TransDataConfigCollection;

                string transDataInstanceName = "Runbow.TWS.Web.TransDataInstances.DefaultTransData";

                if (tranDataConfigCollection != null && tranDataConfigCollection.Any())
                {
                    var customerTransDataConfig = tranDataConfigCollection.FirstOrDefault(t => t.CustomerID == CustomerID);

                    if (customerTransDataConfig != null)
                    {
                        transDataInstanceName = string.IsNullOrEmpty(customerTransDataConfig.DefaultTransDataInstance) ? transDataInstanceName : customerTransDataConfig.DefaultTransDataInstance;

                        if (customerTransDataConfig.WarehouseConfigCollection != null && customerTransDataConfig.WarehouseConfigCollection.Any())
                        {
                            var finalTransDataConfig = customerTransDataConfig.WarehouseConfigCollection.FirstOrDefault(t => t.WarehouseID == WareHouseID);

                            if (finalTransDataConfig != null)
                            {
                                transDataInstanceName = string.IsNullOrEmpty(finalTransDataConfig.AllocateInstance) ? transDataInstanceName : finalTransDataConfig.TransDataInstances;
                            }
                        }
                    }
                }
                return transDataInstanceName;
            }
            else
            {
                return "Runbow.TWS.Web.TransDataInstances.DefaultTransData";
            }
        }

        /// <summary>
        /// 货品上架导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string ImputEcecl(string CustomerID, string Customer)
        {
            long cid = 0;
            long.TryParse(CustomerID, out cid);
            var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == cid && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            long warehouse_id = 0;
            long.TryParse(WarehouseList.Select(c => c.Value).FirstOrDefault(), out warehouse_id);
            var UnitAndSpecificationsList = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(base.UserInfo.ProjectID, cid, warehouse_id);
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    try
                    {
                        //DataSet ds = this.GetDataFromExcel(hpf);
                        DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                        string ErrorMessage = "";
                        TransData(CustomerID, 0, ref ds, ref ErrorMessage);
                        if (ErrorMessage != "")
                        {
                            return new { result = ErrorMessage, IsSuccess = false }.ToJsonString();
                        }
                        List<ProductSearch> ListPs = new List<ProductSearch>();

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ProductSearch ps = new ProductSearch();
                            ps.SKU = ds.Tables[0].Rows[i]["SKU"].ToString();
                            ps.UPC = ds.Tables[0].Rows[i]["UPC"].ToString();
                            ListPs.Add(ps);
                        }
                        IEnumerable<ProductSearch> resualtProList = ApplicationConfigHelper.GetSearchProduct(0, ListPs, "UPC");
                        
                        #region
                        for (int j = 0; j < ds.Tables.Count; j++)
                        {
                            if (ds != null && ds.Tables[j] != null)
                            {
                                IList<ReceiptReceiving> Info = new List<ReceiptReceiving>();
                                //var ss = ApplicationConfigHelper.GetALLProductStorerList(CustomerID);//修复验证sku是否存在
                                DataTable dtNumbers = ds.Tables[0].DefaultView.ToTable(true, "入库单号");
                                List<string> list_number = new List<string>();
                                foreach (DataRow row in dtNumbers.Rows)
                                {
                                    list_number.Add(row["入库单号"].ToString());
                                }
                                IEnumerable<WMS_Customer> stores = new ProductService().Get_WMS_CustomerByNumbers(list_number);
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    if (stores.Select(c => c.UserDef3 == ds.Tables[0].Rows[i]["入库单号"].ToString().Trim()) != null && stores.Select(c => c.UserDef3 == ds.Tables[0].Rows[i]["入库单号"].ToString().Trim()).Any())
                                    {
                                        if (stores.Where(c => c.UserDef3 == ds.Tables[0].Rows[i]["入库单号"].ToString().Trim()).First().UserDef2 != ds.Tables[0].Rows[i]["库区"].ToString().Trim()
                                            && stores.Where(m => m.UserDef3 == ds.Tables[0].Rows[i]["入库单号"].ToString().Trim()).First().UserDef4 != ds.Tables[0].Rows[i]["库区"].ToString().Trim())//判断门店对应的另外一个库区
                                        {
                                            return new { result = "第【" + i + "】行,门店代码：【" + stores.Where(c => c.UserDef3 == ds.Tables[0].Rows[i]["入库单号"].ToString().Trim()).First().StorerKey + "】和库区：" + ds.Tables[0].Rows[i]["库区"].ToString().Trim() + " 不匹配！", IsSuccess = false }.ToJsonString();
                                        }
                                    }
                                    if (!Regex.IsMatch(ds.Tables[0].Rows[i]["实际数量"].ToString(), @"^[-]?\d+[.]?\d*$"))
                                    {
                                        return new { result = "第【" + i + "】行,【" + ds.Tables[0].Rows[i]["实际数量"].ToString() + "】 不是数字！", IsSuccess = false }.ToJsonString();
                                    }
                                    if (resualtProList.Where(c => (c.SKU == ds.Tables[0].Rows[i]["SKU"].ToString() && (c.UPC == ds.Tables[0].Rows[i]["UPC"].ToString() || string.IsNullOrEmpty(ds.Tables[0].Rows[i]["UPC"].ToString())) && c.CustomerName == Customer)).Select(m => m.SKU).FirstOrDefault() == null)// || string.IsNullOrEmpty(c.UPC.ToString())
                                    {
                                        return new { result = "第【" + i + "】行,【" + ds.Tables[0].Rows[i]["SKU"].ToString() + "】 SKU或者UPC在该客户下不存在！", IsSuccess = false }.ToJsonString();
                                    }
                                    if (UnitAndSpecificationsList.Where(c => c.Unit == ds.Tables[0].Rows[i]["单位"].ToString().Trim()).Count() == 0)
                                    {
                                        return new { result = "第【" + i + "】行,【" + ds.Tables[0].Rows[i]["单位"].ToString() + "】 请填写有效单位！", IsSuccess = false }.ToJsonString();
                                    }
                                    Info.Add(new ReceiptReceiving()
                                    {
                                        ReceiptNumber = ds.Tables[0].Rows[i]["入库单号"].ToString().Trim(),
                                        ExternReceiptNumber = ds.Tables[0].Rows[i]["外部单号"].ToString().Trim(),
                                        LineNumber = ds.Tables[0].Rows[i]["收货单行号"].ToString().Trim(),
                                        SKU = ds.Tables[0].Rows[i]["SKU"].ToString().Trim(),
                                        UPC = ds.Tables[0].Rows[i]["UPC"].ToString().Trim(),
                                        SkuLineNumber = (i + 1).ToString().PadLeft(5, '0'),//ds.Tables[0].Rows[i]["SKU行号"].ToString().Trim(),
                                        GoodsName = ds.Tables[0].Rows[i]["货品名称"].ToString().Trim(),
                                        GoodsType = ds.Tables[0].Rows[i]["货品类型"].ToString().Trim(),
                                        QtyReceived = Convert.ToDouble(ds.Tables[0].Rows[i]["实际数量"]),
                                        Warehouse = ds.Tables[0].Rows[i]["仓库"].ToString().Trim(),
                                        Area = ds.Tables[0].Rows[i]["库区"].ToString().Trim(),
                                        Location = ds.Tables[0].Rows[i]["库位"].ToString().Trim(),
                                        BatchNumber = ds.Tables[0].Rows[i]["批次号"].ToString().Trim(),
                                        BoxNumber = ds.Tables[0].Rows[i]["托号"].ToString().Trim(),//YXDR加箱号
                                        Unit = ds.Tables[0].Rows[i]["单位"].ToString().Trim(),
                                        Specifications = ds.Tables[0].Rows[i]["规格"].ToString().Trim(),
                                        Remark = ds.Tables[0].Rows[i]["备注"].ToString().Trim(),
                                        DateTime1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["生产日期"].ToString()),
                                        CustomerName = Customer,// ds.Tables[0].Rows[i]["客户名称"].ToString().Trim(),
                                        str10 = ds.Tables[0].Rows[i]["箱内总数"].ToString().Trim(),//YXDR加箱内总件数
                                    });
                                }
                                var ReceiptReceivingInfo = from q in Info.AsParallel()
                                                           group q by new { q.ReceiptNumber, q.CustomerName, q.Warehouse, q.Area, q.Location, q.SKU, q.UPC, q.BatchNumber, q.GoodsType, q.BoxNumber, q.LineNumber, q.DateTime1 } into r
                                                           select new ReceiptReceiving
                                                           {
                                                               ReceiptNumber = r.Max(a => a.ReceiptNumber),
                                                               ExternReceiptNumber = r.Max(a => a.ExternReceiptNumber),
                                                               LineNumber = r.Max(a => a.LineNumber),
                                                               SkuLineNumber = r.Max(a => a.SkuLineNumber),
                                                               SKU = r.Max(a => a.SKU),
                                                               UPC = r.Max(a => a.UPC),
                                                               GoodsName = r.Max(a => a.GoodsName),
                                                               GoodsType = r.Max(a => a.GoodsType),
                                                               QtyReceived = r.Sum(a => a.QtyReceived),
                                                               Warehouse = r.Max(a => a.Warehouse),
                                                               Area = r.Max(a => a.Area),
                                                               Location = r.Max(a => a.Location),
                                                               BatchNumber = r.Max(a => a.BatchNumber),
                                                               BoxNumber = r.Max(a => a.BoxNumber),
                                                               Unit = r.Max(a => a.Unit),
                                                               Specifications = r.Max(a => a.Specifications),
                                                               Remark = r.Max(a => a.Remark),
                                                               CustomerName = r.Max(a => a.CustomerName),
                                                               str10 = r.Max(a => a.str10),
                                                               DateTime1 = r.Max(a => a.DateTime1),
                                                           };
                                var IsNumber = ReceiptReceivingInfo.AsParallel().Where(q => !Regex.IsMatch(q.QtyReceived.ToString(), @"^[-]?\d+[.]?\d*$")).ToList();
                                if (IsNumber.Count > 0)
                                {
                                    return new { result = "实际数量列存在非数字的值,请检查！", IsSuccess = false }.ToJsonString();
                                }
                                #region 验证订单是否取消
                                IEnumerable<WMSConfig> wms = null;
                                try
                                {
                                    wms = ApplicationConfigHelper.GetWMS_Config("OrderCancel_" + base.UserInfo.ProjectName);
                                }
                                catch
                                {}
                                if (wms == null)
                                {
                                    wms = ApplicationConfigHelper.GetWMS_Config("OrderCancel");
                                }
                                StringBuilder sbb = new StringBuilder();
                                ReceiptReceivingInfo.Select(a => a.ExternReceiptNumber).Distinct().AsEnumerable().Each((i, item) =>
                                       {
                                           var message = new DeliverConfirmService().ValidOrderCancel(item, Convert.ToInt64(CustomerID), wms.FirstOrDefault().Name, "", 3);
                                           if (!string.IsNullOrEmpty(message))
                                           {
                                               sbb.Append("外部单号【" + item + "】已取消，无法上架！");
                                           }
                                       });
                                if (sbb != null && sbb.Length > 0)
                                {
                                    return new { result = sbb.ToString(), IsSuccess = false }.ToJsonString();
                                }
                                #endregion
                                var list = from q in ReceiptReceivingInfo
                                           group q by new { q.CustomerName, q.Warehouse, q.Area, q.Location } into n
                                           select new
                                           {
                                               CustomerName = n.Key.CustomerName,
                                               Warehouse = n.Key.Warehouse,
                                               Area = n.Key.Area,
                                               Location = n.Key.Location
                                           };
                                foreach (var item in list)
                                {
                                    if (validationWarehouse(item.CustomerName, item.Warehouse, item.Area, item.Location))
                                    {
                                        StringBuilder sbs = new StringBuilder();//修复提示信息BUG
                                        return new { result = sbs.Append("请检查库区库位 " + item.Area + "|" + item.Location + "").ToString(), IsSuccess = false }.ToJsonString();
                                    }
                                }
                                var response = new ShelvesManagementService().InsertReceiptReceivingExecl(new GetShelvesByConditionRequest()
                                {
                                    User = base.UserInfo.Name,
                                    receiptReceiving = ReceiptReceivingInfo,
                                });
                                if (response.IsSuccess)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    if (response.Result.receiptReceiving.Count() == 0)
                                    {
                                        return new { result = sb.Append("上架失败！").ToString(), IsSuccess = false }.ToJsonString();
                                    }
                                    //失败记录
                                    if (response.Result.receiptReceiving.Where(c => c.Remark != "成功" && c.Remark != "已入库").Count() > 0)
                                    {
                                        sb.Append("失败记录：");
                                        foreach (var item in response.Result.receiptReceiving.Where(c => c.Remark != "成功" && c.Remark != "已入库"))
                                        {
                                            sb.Append(item.ReceiptNumber + item.Remark + "</br>");
                                        }
                                    }
                                    //成功记录
                                    if (response.Result.receiptReceiving.Where(c => c.Remark == "成功").Count() > 0)
                                    {
                                        sb.Append("成功记录:");
                                        List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                                        foreach (var item in response.Result.receiptReceiving.Where(c => c.Remark == "成功"))
                                        {
                                            sb.Append(item.ReceiptNumber + "</br>");
                                            #region log
                                            WMS_Log_Operation operation = new WMS_Log_Operation();
                                            operation.MenuName = "入库单管理";
                                            operation.Operation = "入库单-上架信息导入";
                                            operation.OrderType = "ReceiptReceiving";
                                            operation.Controller = Request.RawUrl;
                                            operation.Creator = base.UserInfo.Name;
                                            operation.CreateTime = DateTime.Now;
                                            operation.ProjectID = (int)base.UserInfo.ProjectID;
                                            operation.ProjectName = base.UserInfo.ProjectName;
                                            operation.CustomerID = Convert.ToInt32(CustomerID);
                                            operation.CustomerName = Customer;
                                            operation.WarehouseName = item.Warehouse;
                                            operation.OrderNumber = item.ReceiptNumber;
                                            operation.ExternOrderNumber = item.ExternReceiptNumber;
                                            logs.Add(operation);
                                            #endregion
                                        }
                                        new LogOperationService().AddLogOperation(logs);
                                    }
                                    //已入库记录
                                    if (response.Result.receiptReceiving.Where(c => c.Remark == "已入库").Count() > 0)
                                    {
                                        sb.Append("已入库记录:");
                                        foreach (var item in response.Result.receiptReceiving.Where(c => c.Remark == "已入库"))
                                        {
                                            sb.Append(item.ReceiptNumber + "</br>");
                                        }
                                    }
                                    return new { result = sb.ToString(), IsSuccess = true }.ToJsonString();
                                }
                            }
                        }
                        #endregion
                    }
                    catch (Exception e)
                    {
                        return new { result = e.Message, IsSuccess = false }.ToJsonString();
                    }
                    return new { result = "excel内容有误！", IsSuccess = false }.ToJsonString();
                }
                return new { result = "文件内容为空！", IsSuccess = false }.ToJsonString();
            }
            return new { result = "请选择文件！", IsSuccess = false }.ToJsonString();
        }

        /// <summary>
        /// 验证Warehousename
        /// </summary>
        /// <param name="CustomerName"></param>
        /// <param name="Warehouse"></param>
        /// <param name="Area"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        private bool validationWarehouse(string CustomerName, string Warehouse, string Area, string Location)
        {
            var users = ApplicationConfigHelper.GetWarehouseLocationListByWarehouseName(Warehouse).Where(a => a.CustomerName == CustomerName && a.WarehouseName == Warehouse && a.AreaName == Area && a.Location == Location);//0, Warehouse
            //  var list = users.Where(a => a.CustomerName == CustomerName && a.WarehouseName == Warehouse && a.AreaName == Area && a.Location == Location);
            if (users.Count() == 0)
            {
                return true;
            }
            return false;
        }

        private DataSet GetDataFromExcel(HttpPostedFileBase hpf)
        {
            string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH;
            string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, base.UserInfo.ProjectID.ToString(), Runbow.TWS.Common.Constants.TEMPFOLDER);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            string fileName = base.UserInfo.ID.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(hpf.FileName);
            string fullPath = Path.Combine(targetPath, fileName);
            hpf.SaveAs(fullPath);
            hpf.InputStream.Close();
            Runbow.TWS.Common.ExcelHelper excelHelper = new Runbow.TWS.Common.ExcelHelper(fullPath);
            DataSet ds = excelHelper.GetAllDataFromAllSheets();
            excelHelper.Dispose();
            MyFile.Delete(fullPath);
            return ds;
        }

        private ActionResult ExportDataTableToExcel(DataTable dt, string FileName)
        {
            var sbHtml = new StringBuilder();
            sbHtml.Append("<style>td{mso-number-format:\"\\@\";}</style>");
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", dt.Columns[i].ColumnName);
            }

            sbHtml.Append("</tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sbHtml.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i][j].ToString());
                }
                sbHtml.Append("</tr>");
            }

            sbHtml.Append("</table>");
            Response.Charset = "UTF-8";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.AppendHeader("content-disposition", "attachment;filename=" + FileName);
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sbHtml.ToString());
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        public ActionResult ReceiptReceivingInfo(long RID)
        {

            ShelvesModel sm = new ShelvesModel();
            GetShelvesByConditionRequest GetShelvesRequest = new GetShelvesByConditionRequest();
            ReceiptReceivingSearchCondition rs = new ReceiptReceivingSearchCondition();
            rs.RID = RID;
            GetShelvesRequest.SearchCondition = rs;
            // if (Status == 0)
            // {

            var response = new ShelvesManagementService().GetShelves(GetShelvesRequest);
            if (response.IsSuccess)
            {
                sm.storesByGetReceipt = response.Result.storesByGetReceipt;
                sm.Shelves = response.Result.Shelves;
            }
            return View(sm);
        }

    }
}
