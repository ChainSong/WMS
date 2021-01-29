using Newtonsoft.Json;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using UtilConstants = Runbow.TWS.Common.Constants;
using SW = System.Web;
using Runbow.TWS.Web.Areas.WMS.Models.SettlementManagement;
using Runbow.TWS.Entity.WMS.SettlementManagement;
using Runbow.TWS.Biz;
using System.Web.Script.Serialization;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using System.ComponentModel;
using System.IO;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class SettlementManagementController : BaseController
    {
        #region  仓储结算 2018-12-19
        //应收结算
        [HttpGet]
        public ActionResult Settlement(long? customerID, long? warehouseID)
        {
            var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                               .Select(c => c.CustomerID);
            SettlementListViewModel wa = new SettlementListViewModel();
            wa.SearchCondition = new SettlementSearchCondition();
            // wa.SearchCondition.Checkdate = DateTime.Now;
            // wa.SearchCondition.CheckNumber = "PD" + DateTime.Now.ToString("yyyyMMddHHmmss"); 
            //  wa.SearchCondition.str5="";
            if (base.UserInfo.UserType == 0)
            {
                wa.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    wa.SearchCondition.CustomerID = long.Parse( customerID.ToString());
                }
                else
                {
                    // var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        wa.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }
            // wa.SearchCondition.Warehouse = warehouseID.ToString();
            // var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            var CustomerList = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            // var WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == Int64.Parse(CustomerList.Select(a => a.Value).FirstOrDefault())).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && customerIDs.Contains(c.CustomerID.Value) && c.CustomerID == Int64.Parse(CustomerList.Select(a => a.Value).FirstOrDefault()))
                                                 .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            var AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == 0).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName }).Distinct()
                                                 .Select(c => new SelectListItem() { Value = c.Value, Text = c.Text });
            if (WarehouseList.Count() > 0)
            {
                AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == Int64.Parse(WarehouseList.Select(a => a.Value).FirstOrDefault()) & c.CustomerID == wa.SearchCondition.CustomerID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName }).Distinct()
                    .Select(c => new SelectListItem() { Value = c.Value, Text = c.Text });
            }
            //ViewBag.AreaLists = ApplicationConfigHelper.GetWarehouseLocationList().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            //var Areas = ApplicationConfigHelper.GetWarehouseLocationList().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            var Areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            ViewBag.Areas = Areas;
            ViewBag.CustomerList = CustomerList;
            ViewBag.WarehouseList = WarehouseList;
            ViewBag.AreaList = AreaList;
            wa.SearchCondition.StartSettlementdate = DateTime.Now.AddDays(-10);
            wa.SearchCondition.EndSettlementdate = DateTime.Now;
            
            ViewBag.AreaLists = AreaList;
            if (WarehouseList.Count() == 1)
            {
                wa.SearchCondition.WarehouseID =long.Parse( WarehouseList.Select(c => c.Value).FirstOrDefault());
            }
            var Request = new GetSettlementByConditionRequest();
            Request.WLSearchCondition = wa.SearchCondition;
            Request.WLSearchCondition.PageSize = UtilConstants.PAGESIZE;//
            Request.WLSearchCondition.PageIndex = 0;
            var Requests = new SettlementService().GetSettlementList(Request);
            if (Requests.IsSuccess)
            {
                wa.SettlementCollection = Requests.Result.SettlementCollection;
                wa.PageIndex = Requests.Result.PageIndex;
                wa.PageCount = Requests.Result.PageCount;
            }
            return View(wa);
        }
        [HttpPost]
        public ActionResult Settlement(SettlementListViewModel wc, int? PageIndex, long? customerID, long? warehouseID, string Action)
        {
            var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                             .Select(c => c.CustomerID);
            if (wc.SearchCondition.CustomerID.ToString() != null)
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == wc.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == Int64.Parse(wc.SearchCondition.CustomerID.ToString())).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
            if (wc.SearchCondition.WarehouseID.ToString() != null)
            {
                ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == wc.SearchCondition.WarehouseID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName, Selected = true });
            }
            else
            {
                ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == 0).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            }
            ViewBag.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            //ViewBag.AreaLists = ApplicationConfigHelper.GetWarehouseLocationList().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && customerIDs.Contains(c.CustomerID.Value) && c.CustomerID == wc.SearchCondition.CustomerID)
                                                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            var Areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == Int64.Parse(WarehouseList.Select(a => a.Value).FirstOrDefault()) & c.CustomerID == wc.SearchCondition.CustomerID).Select(t => new { ID = t.ID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            //if (wc.SearchCondition.Warehouse != null)
            //{
            //    ViewBag.AreaLists = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseName == wc.SearchCondition.Warehouse.ToString()).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName, Selected = true });
            //}
            //else
            //{
            //    ViewBag.AreaLists = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == 0).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            //}
            ViewBag.Areas = Areas;
            ViewBag.AreaLists = Areas;
            if (WarehouseList.Count() == 1)
            {
                wc.SearchCondition.WarehouseID = long.Parse(WarehouseList.Select(c => c.Value).FirstOrDefault());
            }
            var Request = new GetSettlementByConditionRequest();
            Request.WLSearchCondition = wc.SearchCondition;
            Request.WLSearchCondition.PageSize = UtilConstants.PAGESIZE;//
            Request.WLSearchCondition.PageIndex = PageIndex ?? 0;
            var Requests = new SettlementService().GetSettlementList(Request);
            if (Requests.IsSuccess)
            {
                wc.SettlementCollection = Requests.Result.SettlementCollection;
                wc.PageIndex = Requests.Result.PageIndex;
                wc.PageCount = Requests.Result.PageCount;
            }
            //if (Action == "导出差异")
            //{
            //    //  Export(getAdjustByConditionResponse.Result, columnReceipt, columnReceiptDetail);
            //}
            return View(wc);
        }

        //应付结算
        [HttpGet]
        public ActionResult PayableSettlement(long? customerID, long? warehouseID)
        {
            var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                               .Select(c => c.CustomerID);
            SettlementListViewModel wa = new SettlementListViewModel();
            wa.SearchCondition = new SettlementSearchCondition();
            // wa.SearchCondition.Checkdate = DateTime.Now;
            // wa.SearchCondition.CheckNumber = "PD" + DateTime.Now.ToString("yyyyMMddHHmmss"); 
            //  wa.SearchCondition.str5="";
            if (base.UserInfo.UserType == 0)
            {
                wa.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    wa.SearchCondition.CustomerID = long.Parse(customerID.ToString());
                }
                else
                {
                    // var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        wa.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }
            // wa.SearchCondition.Warehouse = warehouseID.ToString();
            // var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            var CustomerList = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            // var WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == Int64.Parse(CustomerList.Select(a => a.Value).FirstOrDefault())).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && customerIDs.Contains(c.CustomerID.Value) && c.CustomerID == Int64.Parse(CustomerList.Select(a => a.Value).FirstOrDefault()))
                                                 .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            var AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == 0).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName }).Distinct()
                                                 .Select(c => new SelectListItem() { Value = c.Value, Text = c.Text });
            if (WarehouseList.Count() > 0)
            {
                AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == Int64.Parse(WarehouseList.Select(a => a.Value).FirstOrDefault()) & c.CustomerID == wa.SearchCondition.CustomerID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName }).Distinct()
                    .Select(c => new SelectListItem() { Value = c.Value, Text = c.Text });
            }
            //ViewBag.AreaLists = ApplicationConfigHelper.GetWarehouseLocationList().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            //var Areas = ApplicationConfigHelper.GetWarehouseLocationList().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            var Areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            ViewBag.Areas = Areas;
            ViewBag.CustomerList = CustomerList;
            ViewBag.WarehouseList = WarehouseList;
            ViewBag.AreaList = AreaList;
            wa.SearchCondition.StartSettlementdate = DateTime.Now.AddDays(-10);
            wa.SearchCondition.EndSettlementdate = DateTime.Now;

            ViewBag.AreaLists = AreaList;
            if (WarehouseList.Count() == 1)
            {
                wa.SearchCondition.WarehouseID = long.Parse(WarehouseList.Select(c => c.Value).FirstOrDefault());
            }
            var Request = new GetSettlementByConditionRequest();
            Request.WLSearchCondition = wa.SearchCondition;
            Request.WLSearchCondition.PageSize = UtilConstants.PAGESIZE;//
            Request.WLSearchCondition.PageIndex = 0;
            var Requests = new SettlementService().GetSettlementListPay(Request);
            if (Requests.IsSuccess)
            {
                wa.SettlementCollection = Requests.Result.SettlementCollection;
                wa.PageIndex = Requests.Result.PageIndex;
                wa.PageCount = Requests.Result.PageCount;
            }
            return View(wa);
        }
        [HttpPost]
        public ActionResult PayableSettlement(SettlementListViewModel wc, int? PageIndex, long? customerID, long? warehouseID, string Action)
        {
            var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                             .Select(c => c.CustomerID);
            if (wc.SearchCondition.CustomerID.ToString() != null)
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == wc.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == Int64.Parse(wc.SearchCondition.CustomerID.ToString())).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
            if (wc.SearchCondition.WarehouseID.ToString() != null)
            {
                ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == wc.SearchCondition.WarehouseID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName, Selected = true });
            }
            else
            {
                ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == 0).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            }
            ViewBag.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            //ViewBag.AreaLists = ApplicationConfigHelper.GetWarehouseLocationList().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && customerIDs.Contains(c.CustomerID.Value) && c.CustomerID == wc.SearchCondition.CustomerID)
                                                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            var Areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == Int64.Parse(WarehouseList.Select(a => a.Value).FirstOrDefault()) & c.CustomerID == wc.SearchCondition.CustomerID).Select(t => new { ID = t.ID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            //if (wc.SearchCondition.Warehouse != null)
            //{
            //    ViewBag.AreaLists = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseName == wc.SearchCondition.Warehouse.ToString()).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName, Selected = true });
            //}
            //else
            //{
            //    ViewBag.AreaLists = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == 0).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            //}
            ViewBag.Areas = Areas;
            ViewBag.AreaLists = Areas;
            if (WarehouseList.Count() == 1)
            {
                wc.SearchCondition.WarehouseID = long.Parse(WarehouseList.Select(c => c.Value).FirstOrDefault());
            }
            var Request = new GetSettlementByConditionRequest();
            Request.WLSearchCondition = wc.SearchCondition;
            Request.WLSearchCondition.PageSize = UtilConstants.PAGESIZE;//
            Request.WLSearchCondition.PageIndex = PageIndex ?? 0;
            var Requests = new SettlementService().GetSettlementListPay(Request);
            if (Requests.IsSuccess)
            {
                wc.SettlementCollection = Requests.Result.SettlementCollection;
                wc.PageIndex = Requests.Result.PageIndex;
                wc.PageCount = Requests.Result.PageCount;
            }
            //if (Action == "导出差异")
            //{
            //    //  Export(getAdjustByConditionResponse.Result, columnReceipt, columnReceiptDetail);
            //}
            return View(wc);
        }

        /// <summary>
        /// 1 编辑
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="warehouseID"></param>
        /// <param name="ViewType"></param>
        /// <param name="CheckNumber"></param>
        /// <returns></returns>
        //[HttpGet]
        //public ActionResult WareHouseCheckEdit(long? customerID, long? warehouseID, int ViewType = 0, string CheckNumber = "", string flag = null)
        //{
        //    WarehouseCheckModel wa = new WarehouseCheckModel();
        //    wa.ViewType = ViewType;
        //    //if (wa.ViewType == 1)
        //    {
        //        //根据CheckNumber 获取盘点单信息
        //        WarehouseCheckModel getwc = new WarehouseCheckModel();
        //        Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>();
        //        GetWarehouseCheckByConditonRequest request = new GetWarehouseCheckByConditonRequest();
        //        WarehouseCheckSearchCondition wc = new WarehouseCheckSearchCondition();
        //        wc.CheckNumber = CheckNumber;
        //        wc.ViewType = ViewType;
        //        request.WLSearchCondition = wc;
        //        request.WLSearchCondition.CheckNumber = CheckNumber;
        //        if (flag == "导出差异")
        //        {
        //            IEnumerable<Column> columnReceipt;
        //            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
        //            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
        //            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_CheckDetail").Count() == 0)
        //            {
        //                columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_CheckDetail").ColumnCollection;
        //            }
        //            else
        //            {
        //                columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_CheckDetail").ColumnCollection;
        //            }
        //            //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, customerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
        //            //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
        //            //IEnumerable<Table> tables = module.Tables.TableCollection;
        //            //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_CheckDetail").ColumnCollection;
        //            var responses = new WarehouseService().ExportWarehouseCheckByCheckNumber(request);
        //            ExportCheckDetail(responses.Result, columnReceipt);
        //            return null;
        //        }
        //        else
        //        {
        //            var responses = new WarehouseService().GetWarehouseCheckByCheckNumber(request);
        //            wa.SearchCondition = wc;
        //            wa.WarehouseCheckDetailCollection = responses.Result.WarehouseCheckDetailCollection;
        //            wa.WarehouseCheckCollection = responses.Result.WarehouseCheckCollection;
        //        }
        //    }
        //    return View(wa);
        //}

        private void ExportSettlementDetail(GetSettlementByConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<SettlementDetail> receipts = response.SettlementDetailCollection;
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
                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.WMS.SettlementManagement.SettlementDetail).GetProperty(receipt.DbColumnName).GetValue(s);
                }
                dtReceipt.Rows.Add(drReceipt);
            });
            dtReceipt.TableName = "结算信息";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "结算信息" + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "结算信息" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// 1 编辑
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="warehouseID"></param>
        /// <param name="ViewType"></param>
        /// <param name="CheckNumber"></param>
        /// <returns></returns>
        //[HttpPost]
        //public ActionResult WareHouseCheckEdit(WarehouseCheckModel vm, string[] SKU, string[] ActualQTY, string Action)
        //{
        //    WarehouseCheckModel wa = new WarehouseCheckModel();
        //    //根据CheckNumber 获取盘点单信息
        //    WarehouseCheckModel getwc = new WarehouseCheckModel();
        //    Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>();
        //    GetWarehouseCheckByConditonRequest request = new GetWarehouseCheckByConditonRequest();
        //    vm.SearchCondition.ActulQtyargs = ActualQTY;
        //    request.WLSearchCondition = vm.SearchCondition;
        //    string Message = new WarehouseService().SaveWarehouseCheckByCheckNumber(request);
        //    return RedirectToAction("WareHouseCheckDetail");
        //}

        //应收结算新增
        [HttpGet]
        public ActionResult SettlementDetail(long? customerID, long? warehouseID, string externNumber = "", int ViewType = 0, string CheckNumber = "")
        {
            SettlementListViewModel wa = new SettlementListViewModel();
            wa.ViewType = ViewType;
            if (ViewType == 0)
            {
                wa.SearchCondition = new SettlementSearchCondition();
                //wa.SearchCondition.ExternNumber = externNumber;
                //wa.SearchCondition.Checkdate = DateTime.Now;
                wa.SearchCondition.SettlementNumber = "JS" + DateTime.Now.ToString("yyyyMMddHHmmss");
                wa.SearchCondition.StartCompleteDate = DateTime.Now.AddDays(-1);
                wa.SearchCondition.EndCompleteDate = DateTime.Now.AddDays(-1);
                //wa.SearchCondition.ExternNumber = base.UserInfo.Name + DateTime.Now.ToString("MMddHHmmss");
                //wa.SearchCondition.str5 = "";
                if (base.UserInfo.UserType == 0)
                {
                    wa.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        wa.SearchCondition.CustomerID = long.Parse( customerID.ToString());
                    }
                    else
                    {
                        var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                        if (customerIDs != null && customerIDs.Count() == 1)
                        {
                            wa.SearchCondition.CustomerID = customerIDs.First();
                        }
                    }
                }
                wa.SearchCondition.WarehouseID = long.Parse( warehouseID.ToString());
                var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                ViewBag.CustomerList = CustomerList;
                if (customerID != null)
                {
                    //ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == wa.SearchCondition.CustomerID).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
                    ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                }
                else
                {
                    ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == Int64.Parse(CustomerList.Select(a => a.Value).FirstOrDefault())).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
                }
                //ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID ==).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
                int warehou = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID)).Select(s => s.WarehouseID).FirstOrDefault();
                ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehou & c.CustomerID == wa.SearchCondition.CustomerID).Select(t => new { ID = t.ID.ToString(), AreaName = t.AreaName }).Distinct()
                    .Select(c => new SelectListItem() { Value = c.ID, Text = c.AreaName });
                //ViewBag.Completeddate = DateTime.Now.ToString("yyyy-MM-dd");
                //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                //var WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == Int64.Parse(CustomerList.Select(a => a.Value).FirstOrDefault())).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
                //var AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == Int64.Parse(WarehouseList.Select(a => a.Value).FirstOrDefault())).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
                //if (WarehouseList.Count() == 1)
                //{
                //    wa.SearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
                //}
                //ViewBag.CustomerList = CustomerList;
                //ViewBag.WarehouseList = WarehouseList;
                //ViewBag.AreaList = AreaList;
            }
            #region viewtype=1
            else if (ViewType == 1)
            {
                //根本checknumber获取明细信息
                wa.SearchCondition = new SettlementSearchCondition();
                //wa.SearchCondition.Settlementdate = DateTime.Now;
                // wa.SearchCondition.CheckNumber = "PD" + DateTime.Now.ToString("yyyyMMddHHmmss");
                //wa.SearchCondition.str5 = "";
                if (base.UserInfo.UserType == 0)
                {
                    wa.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        wa.SearchCondition.CustomerID = long.Parse( customerID.ToString());
                    }
                    else
                    {
                        var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                        if (customerIDs != null && customerIDs.Count() == 1)
                        {
                            wa.SearchCondition.CustomerID = customerIDs.First();
                        }
                    }
                }
                wa.SearchCondition.WarehouseID = long.Parse( warehouseID.ToString());
                ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == wa.SearchCondition.CustomerID).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
                //ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID ==).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
                ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehouseID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
                ViewBag.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            }
            #endregion
            return View(wa);
        }
        [HttpPost]
        public ActionResult SettlementDetail(SettlementListViewModel vm, int? PageIndex, string[] roles, string[] ActualQTY, string Action)
        {
            SettlementSearchCondition wc = new SettlementSearchCondition();
            #region 执行条件判断
            //string ISOK = string.Empty;  //判断是否存在错误信息
            //if (string.IsNullOrEmpty(vm.SearchCondition.ExternNumber))
            //{
            //    ISOK += "外部盘点单号为空 |";
            //}
            //if (string.IsNullOrEmpty(vm.SearchCondition.Checkdate.ToString()))
            //{
            //    vm.SearchCondition.Checkdate = DateTime.Now;
            //    //ISOK += "盘点日期为空  |";
            //}
            //else
            //{
            //    try
            //    {
            //        DateTime.Parse(vm.SearchCondition.Checkdate.ToString());
            //    }
            //    catch { ISOK += "盘点日期格式错误  |"; }
            //}
            //if (string.IsNullOrEmpty(vm.SearchCondition.Warehouse))
            //{
            //    ISOK += "盘点仓库为空  |";
            //}
            //if (vm.SearchCondition.Type != 1)
            //{
            //    if (string.IsNullOrEmpty(vm.SearchCondition.str1) && string.IsNullOrEmpty(vm.SearchCondition.str2))
            //    {
            //        if (vm.SearchCondition.Type == 2)
            //            ISOK += "起始库位不能为空  |";
            //        if (vm.SearchCondition.Type == 3)
            //            ISOK += "起始SKU不能为空  |";
            //        if (vm.SearchCondition.Type == 4)
            //            ISOK += "SKU上下线数量不能为空  |";
            //        if (vm.SearchCondition.Type == 5)
            //            ISOK += "开始结束时间不能为空  |";
            //    }
            //}
            //if (vm.SearchCondition.Type == 5)
            //{
            //    string businessType = "";
                //if (roles.Length == 0 || roles == null)
                //{
                //    ISOK += "业务类型至少勾选一项  |";
                //}
                //else
                //{
                //for (int i = 0; i < roles.Length; i++)
                //{
                //    businessType += roles[i] + ",";
                    //for (int j = 0; j < vm.Remark.Count(); j++)
                    //{
                    //    if (vm.Remark.First(a => a.Value == roles[i]).Text !="" )
                    //    {
                    //        vm.Remark.First(a => a.Value == roles[i]).Selected = true;
                    //    }
                    //}
                //}
                //}
                //wc.str5 = businessType.Substring(0, businessType.Length - 1);
            //}
            //判断查询行数是否变化
            //var responsese = new WarehouseService().GetWarehouseCheck(new GetWarehouseCheckByConditonRequest() { WLSearchCondition = vm.SearchCondition });
            //if (responsese.Result.PageCount != ActualQTY.Length)
            //{
            //    ISOK += "差异行数已更新,请重新查询  |";
            //}
            #endregion
            string aaa = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Select(m => m.WarehouseID).FirstOrDefault().ToString();
            var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;
            var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            ViewBag.WarehouseList = WarehouseList;
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == long.Parse(aaa))
                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            if (Action == "查询" || Action == "SettlementDetail")//翻页
            {
                SettlementListViewModel viewm = new SettlementListViewModel();
                wc.CustomerID = Int64.Parse(vm.SearchCondition.CustomerID.ToString());
                wc.WarehouseID = vm.SearchCondition.WarehouseID;
                wc.CompleteDate = vm.SearchCondition.CompleteDate;
                wc.StartCompleteDate = vm.SearchCondition.StartCompleteDate;
                wc.EndCompleteDate = vm.SearchCondition.EndCompleteDate;
                wc.Type = int.Parse(vm.SearchCondition.Type.ToString());
                IEnumerable<WMSConfig> Classifications;
                try
                {
                    Classifications = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("SettlementType");
                    wc.Type_description = Classifications.Where(m => m.Code == wc.Type.ToString()).Select(a => a.Name).FirstOrDefault();
                }
                catch (Exception Ex)
                {
                }
                wc.PageIndex = PageIndex.Value;
                wc.PageSize = UtilConstants.PAGESIZE;

                vm.ViewType = 1;
                vm.SearchCondition = wc;
                var response = new SettlementService().GetSettlementNew(new GetSettlementByConditionRequest() { WLSearchCondition = wc });
                if (response.IsSuccess)
                {
                    if (response.Result != null)
                    {
                        vm.SettlementDetailCollection = response.Result.SettlementDetailCollection;
                        vm.PageCount = response.Result.PageCount;
                        //vm.RowIndex = (PageIndex.Value + 1) * UtilConstants.PAGESIZE;
                        vm.RowCount = response.Result.RowCount;
                        return View(vm);
                    }
                }
                return View();
            }
            else
            {
                //while (vm.SearchCondition.StartCompleteDate < vm.SearchCondition.EndCompleteDate)
                //{
                //    vm.SearchCondition.SettlementNumber = "JS" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + DateTime.Parse(vm.SearchCondition.StartCompleteDate.ToString()).ToOADate().ToString();
                //    vm.SearchCondition.StartCompleteDate = DateTime.Parse(vm.SearchCondition.StartCompleteDate.ToString()).AddDays(1);
                //}
                Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>();
                GetSettlementByConditionRequest request = new GetSettlementByConditionRequest();
                vm.SearchCondition.Oprer = base.UserInfo.Name;
                vm.SearchCondition.CustomerName = CustomerList.Where(c => c.Value == vm.SearchCondition.CustomerID.ToString()).Select(s=>s.Text).FirstOrDefault();
                vm.SearchCondition.WarehouseName = WarehouseList.Where(c => c.Value == vm.SearchCondition.WarehouseID.ToString()).Select(s => s.Text).FirstOrDefault();
                vm.SearchCondition.SettlementNumber = "JS" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                request.WLSearchCondition = vm.SearchCondition;
                response = new SettlementService().GetSettlementSave(request);
                if (response.Result.Message.Contains("操作失败"))
                {
                    return RedirectToAction("Settlement");
                }
                return RedirectToAction("Settlement");
            }
        }

        //应付结算新增
        [HttpGet]
        public ActionResult PaySettlementDetail(long? customerID, long? warehouseID, string externNumber = "", int ViewType = 0, string CheckNumber = "")
        {
            SettlementListViewModel wa = new SettlementListViewModel();
            wa.ViewType = ViewType;
            if (ViewType == 0)
            {
                wa.SearchCondition = new SettlementSearchCondition();
                //wa.SearchCondition.ExternNumber = externNumber;
                //wa.SearchCondition.Checkdate = DateTime.Now;
                wa.SearchCondition.SettlementNumber = "JS" + DateTime.Now.ToString("yyyyMMddHHmmss");
                wa.SearchCondition.StartSettlementdate = DateTime.Now.AddDays(-1);
                wa.SearchCondition.EndSettlementdate = DateTime.Now.AddDays(-1);
                //wa.SearchCondition.ExternNumber = base.UserInfo.Name + DateTime.Now.ToString("MMddHHmmss");
                //wa.SearchCondition.str5 = "";
                if (base.UserInfo.UserType == 0)
                {
                    wa.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        wa.SearchCondition.CustomerID = long.Parse(customerID.ToString());
                    }
                    else
                    {
                        var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                        if (customerIDs != null && customerIDs.Count() == 1)
                        {
                            wa.SearchCondition.CustomerID = customerIDs.First();
                        }
                    }
                }
                wa.SearchCondition.WarehouseID = long.Parse(warehouseID.ToString());
                var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                ViewBag.CustomerList = CustomerList;
                if (customerID != null)
                {
                    //ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == wa.SearchCondition.CustomerID).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
                    ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                }
                else
                {
                    ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == Int64.Parse(CustomerList.Select(a => a.Value).FirstOrDefault())).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
                }
                //ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID ==).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
                int warehou = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID)).Select(s => s.WarehouseID).FirstOrDefault();
                ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehou & c.CustomerID == wa.SearchCondition.CustomerID).Select(t => new { ID = t.ID.ToString(), AreaName = t.AreaName }).Distinct()
                    .Select(c => new SelectListItem() { Value = c.ID, Text = c.AreaName });
                //ViewBag.Completeddate = DateTime.Now.ToString("yyyy-MM-dd");
                //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                //var WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == Int64.Parse(CustomerList.Select(a => a.Value).FirstOrDefault())).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
                //var AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == Int64.Parse(WarehouseList.Select(a => a.Value).FirstOrDefault())).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
                //if (WarehouseList.Count() == 1)
                //{
                //}
                //ViewBag.CustomerList = CustomerList;
                //ViewBag.WarehouseList = WarehouseList;
                //ViewBag.AreaList = AreaList;
            }
            #region viewtype=1
            else if (ViewType == 1)
            {
                //根本checknumber获取明细信息
                wa.SearchCondition = new SettlementSearchCondition();
                //wa.SearchCondition.Settlementdate = DateTime.Now;
                // wa.SearchCondition.CheckNumber = "PD" + DateTime.Now.ToString("yyyyMMddHHmmss");
                //wa.SearchCondition.str5 = "";
                if (base.UserInfo.UserType == 0)
                {
                    wa.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        wa.SearchCondition.CustomerID = long.Parse(customerID.ToString());
                    }
                    else
                    {
                        var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                        if (customerIDs != null && customerIDs.Count() == 1)
                        {
                            wa.SearchCondition.CustomerID = customerIDs.First();
                        }
                    }
                }
                wa.SearchCondition.WarehouseID = long.Parse(warehouseID.ToString());
                ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == wa.SearchCondition.CustomerID).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
                //ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID ==).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
                ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehouseID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
                ViewBag.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            }
            #endregion
            return View(wa);
        }
        [HttpPost]
        public ActionResult PaySettlementDetail(SettlementListViewModel vm, int? PageIndex, string[] roles, string[] ActualQTY, string Action)
        {
            SettlementSearchCondition wc = new SettlementSearchCondition();
            #region 执行条件判断
            //string ISOK = string.Empty;  //判断是否存在错误信息
            //if (string.IsNullOrEmpty(vm.SearchCondition.ExternNumber))
            //{
            //    ISOK += "外部盘点单号为空 |";
            //}
            //if (string.IsNullOrEmpty(vm.SearchCondition.Checkdate.ToString()))
            //{
            //    vm.SearchCondition.Checkdate = DateTime.Now;
            //    //ISOK += "盘点日期为空  |";
            //}
            //else
            //{
            //    try
            //    {
            //        DateTime.Parse(vm.SearchCondition.Checkdate.ToString());
            //    }
            //    catch { ISOK += "盘点日期格式错误  |"; }
            //}
            //if (string.IsNullOrEmpty(vm.SearchCondition.Warehouse))
            //{
            //    ISOK += "盘点仓库为空  |";
            //}
            //if (vm.SearchCondition.Type != 1)
            //{
            //    if (string.IsNullOrEmpty(vm.SearchCondition.str1) && string.IsNullOrEmpty(vm.SearchCondition.str2))
            //    {
            //        if (vm.SearchCondition.Type == 2)
            //            ISOK += "起始库位不能为空  |";
            //        if (vm.SearchCondition.Type == 3)
            //            ISOK += "起始SKU不能为空  |";
            //        if (vm.SearchCondition.Type == 4)
            //            ISOK += "SKU上下线数量不能为空  |";
            //        if (vm.SearchCondition.Type == 5)
            //            ISOK += "开始结束时间不能为空  |";
            //    }
            //}
            //if (vm.SearchCondition.Type == 5)
            //{
            //    string businessType = "";
            //if (roles.Length == 0 || roles == null)
            //{
            //    ISOK += "业务类型至少勾选一项  |";
            //}
            //else
            //{
            //for (int i = 0; i < roles.Length; i++)
            //{
            //    businessType += roles[i] + ",";
            //for (int j = 0; j < vm.Remark.Count(); j++)
            //{
            //    if (vm.Remark.First(a => a.Value == roles[i]).Text !="" )
            //    {
            //        vm.Remark.First(a => a.Value == roles[i]).Selected = true;
            //    }
            //}
            //}
            //}
            //wc.str5 = businessType.Substring(0, businessType.Length - 1);
            //}
            //判断查询行数是否变化
            //var responsese = new WarehouseService().GetWarehouseCheck(new GetWarehouseCheckByConditonRequest() { WLSearchCondition = vm.SearchCondition });
            //if (responsese.Result.PageCount != ActualQTY.Length)
            //{
            //    ISOK += "差异行数已更新,请重新查询  |";
            //}
            #endregion
            string aaa = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Select(m => m.WarehouseID).FirstOrDefault().ToString();
            var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;
            var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            ViewBag.WarehouseList = WarehouseList;
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == long.Parse(aaa))
                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            if (Action == "查询" || Action == "SettlementDetail")//翻页
            {
                SettlementListViewModel viewm = new SettlementListViewModel();
                wc.CustomerID = Int64.Parse(vm.SearchCondition.CustomerID.ToString());
                wc.WarehouseID = vm.SearchCondition.WarehouseID;
                wc.CompleteDate = vm.SearchCondition.CompleteDate;
                wc.StartCompleteDate = vm.SearchCondition.StartCompleteDate;
                wc.EndCompleteDate = vm.SearchCondition.EndCompleteDate;
                wc.Type = int.Parse(vm.SearchCondition.Type.ToString());
                IEnumerable<WMSConfig> Classifications;
                try
                {
                    Classifications = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("SettlementType");
                    wc.Type_description = Classifications.Where(m => m.Code == wc.Type.ToString()).Select(a => a.Name).FirstOrDefault();
                }
                catch (Exception Ex)
                {
                }
                wc.PageIndex = PageIndex.Value;
                wc.PageSize = UtilConstants.PAGESIZE;

                vm.ViewType = 1;
                vm.SearchCondition = wc;
                var response = new SettlementService().GetSettlementNew(new GetSettlementByConditionRequest() { WLSearchCondition = wc });
                if (response.IsSuccess)
                {
                    if (response.Result != null)
                    {
                        vm.SettlementDetailCollection = response.Result.SettlementDetailCollection;
                        vm.PageCount = response.Result.PageCount;
                        //vm.RowIndex = (PageIndex.Value + 1) * UtilConstants.PAGESIZE;
                        vm.RowCount = response.Result.RowCount;
                        return View(vm);
                    }
                }
                return View();
            }
            else
            {
                Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>();
                GetSettlementByConditionRequest request = new GetSettlementByConditionRequest();
                vm.SearchCondition.Oprer = base.UserInfo.Name;
                vm.SearchCondition.CustomerName = CustomerList.Where(c => c.Value == vm.SearchCondition.CustomerID.ToString()).Select(s => s.Text).FirstOrDefault();
                vm.SearchCondition.WarehouseName = WarehouseList.Where(c => c.Value == vm.SearchCondition.WarehouseID.ToString()).Select(s => s.Text).FirstOrDefault();
                vm.SearchCondition.SettlementNumber = "JS" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                request.WLSearchCondition = vm.SearchCondition;
                response = new SettlementService().GetSettlementSave(request);
                return RedirectToAction("Settlement");
            }
        }

        //应收结算预览
        public ActionResult SettlementDetailPreview(long CustomerID, long WarehouseID, string StartCompleteDate, string EndCompleteDate)
        {
            SettlementListViewModel vm = new SettlementListViewModel();
            Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>();
            GetSettlementByConditionRequest request = new GetSettlementByConditionRequest();
            request.WLSearchCondition = new SettlementSearchCondition();
            request.WLSearchCondition.CustomerID = CustomerID;
            request.WLSearchCondition.WarehouseID = WarehouseID;
            request.WLSearchCondition.StartCompleteDate =DateTime.Parse( StartCompleteDate);
            request.WLSearchCondition.EndCompleteDate = DateTime.Parse(EndCompleteDate);
            response = new SettlementService().GetSettlementPreview(request);
            if (response.IsSuccess)
            {
                if (response.Result != null)
                {
                    vm.SettlementCollection = response.Result.SettlementCollection;
                }
            }
            return View(vm);
            //return Json(vm.SettlementCollection, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 保存查询明细 
        /// </summary>
        /// <param name="SettlementNumber"></param>
        /// <param name="ExtrnNumber"></param>
        /// <param name="SettlementDate"></param>
        /// <param name="Customer"></param>
        /// <param name="Warehouse"></param>
        /// <param name="Area"></param>
        /// <param name="Type"></param>
        /// <param name="str1">查询条件1</param>
        /// <param name="str2">查询条件2</param>
        /// <param name="Roles">业务类型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveSettlementInfo(string SettlementNumber, string ExternNumber, string CompleteDate, string Customer, string Warehouse, string Area, string Type_Des, string Type, string str1, string str2, string Roles, string ActualQty)
        {
            string ISOK = string.Empty;  //判断是否存在错误信息
            #region 执行条件判断
            if (string.IsNullOrEmpty(ExternNumber))
            {
                ISOK += "外部单号为空 |";
            }
            if (string.IsNullOrEmpty(CompleteDate))
            {
                ISOK += "出库日期为空  |";
            }
            else
            {
                try
                {
                    DateTime.Parse(CompleteDate);
                }
                catch { ISOK += "出库日期格式错误  |"; }
            }
            if (string.IsNullOrEmpty(Warehouse))
            {
                ISOK += "结算仓库为空  |";
            }
            if (string.IsNullOrEmpty(Area))
            {
                ISOK += "结算区域为空  |";
            }
            if (Type != "1")
            {
                if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
                {
                    if (Type == "2")
                        ISOK += "起始库位不能为空  |";
                    if (Type == "3")
                        ISOK += "起始SKU不能为空  |";
                    if (Type == "4")
                        ISOK += "SKU上下线数量不能为空  |";
                    if (Type == "5")
                        ISOK += "开始结束时间不能为空  |";
                }
            }
            if (Type == "5" && (Roles.Length == 0 || Roles == null))
            {
                ISOK += "业务类型至少勾选一项  |";
            }
            if (ISOK == string.Empty)
            {
                SettlementListViewModel wc = new SettlementListViewModel();

                //判断查询行数是否变化
                var responsese = new SettlementService().GetSettlement(new GetSettlementByConditionRequest() { WLSearchCondition = wc.SearchCondition });
                //if (responsese.Result.PageCount != ActualQty.Split(',').Length)
                //{
                //    ISOK += "差异行数已更新,请重新查询  |";
                //}
            }
            #endregion
            if (ISOK == string.Empty)
            {
                SettlementListViewModel vm = new SettlementListViewModel();
                //vm.SearchCondition.str1 = str1;
                //vm.SearchCondition.str2 = str2;
                //vm.SearchCondition.str3 = Roles;
                //vm.SearchCondition.str4 = ActualQty;
                vm.SearchCondition.SettlementNumber = SettlementNumber;
                vm.SearchCondition.ExternNumber = ExternNumber;
                vm.SearchCondition.CompleteDate = DateTime.Parse(CompleteDate);
                vm.SearchCondition.CustomerID = Int64.Parse(Customer);
                vm.SearchCondition.WarehouseID = long.Parse( Warehouse);
                //vm.SearchCondition.Area = Area;
                vm.SearchCondition.Type = int.Parse(Type);
                vm.SearchCondition.Type_description = Type_Des;
                vm.SearchCondition.Oprer = base.UserInfo.Name;
                //执行保存操作
                Response<GetSettlementByConditionResponse> response = new Response<GetSettlementByConditionResponse>();
                GetSettlementByConditionRequest request = new GetSettlementByConditionRequest();
                request.WLSearchCondition = vm.SearchCondition;
                response = new SettlementService().GetSettlementSave(request);
            }
            else
            {
                throw new Exception("获取盘点信息失败!");
            }
            return Json("");
        }

        //应收结算查看
        [HttpGet]
        public ActionResult SettlementLook(long CustomerID, long WarehouseID, string SettlementNumber)
        {
            var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            SettlementListViewModel vm = new SettlementListViewModel();
            SettlementSearchCondition wc = new SettlementSearchCondition();
            wc.CustomerID = CustomerID;
            wc.CustomerName = CustomerList.Where(c => c.Value == CustomerID.ToString()).Select(s => s.Text).FirstOrDefault();
            wc.WarehouseID = WarehouseID;
            wc.WarehouseName = WarehouseList.Where(c => c.Value == WarehouseID.ToString()).Select(s => s.Text).FirstOrDefault();
            wc.SettlementNumber = SettlementNumber;
            vm.SearchCondition = wc;
            var response = new SettlementService().GetSettlementBySettlementNumber(new GetSettlementByConditionRequest() { WLSearchCondition = wc });
            if (response.IsSuccess)
            {
                if (response.Result != null)
                {
                    vm.SettlementDetailCollection = response.Result.SettlementDetailCollection;
                    vm.PageCount = response.Result.PageCount;
                    return View(vm);
                }
            }
            return View();
        }

        //ajax方式查询盘点数据 
        [HttpPost]
        public JsonResult GetSettlementInfo(string customerID, string warehouseID, string AreaID, string Type, string Types, string Condition1, string Condition2)
        {
            if (customerID != null && warehouseID != null && AreaID != null && Type != null && Types != null)
            {
                SettlementSearchCondition wc = new SettlementSearchCondition();
                wc.CustomerID = Int64.Parse(customerID);
                wc.WarehouseID = long.Parse( warehouseID);
                //wc.Area = AreaID;
                wc.Type = int.Parse(Type);
                wc.Type_description = Types;
                //wc.str1 = Condition1;
                //wc.str2 = Condition2;
                var response = new SettlementService().GetSettlement(new GetSettlementByConditionRequest() { WLSearchCondition = wc });
                if (response.IsSuccess)
                {
                    if (response.Result != null)
                    {
                        var resultData = Json(response.Result.SettlementCollection);
                        return resultData;
                    }
                    else
                    {
                        return Json("");
                    }
                }
            }
            throw new Exception("获取盘点信息失败!");
        }

        //public ActionResult GetWarehouseCheckInfo(string customerID, string warehouseID, string AreaID, string Type, string Types, string Condition1, string Condition2)
        //{
        //    if (customerID != null && warehouseID != null && AreaID != null && Type != null && Types != null)
        //    {
        //        WarehouseCheckSearchCondition wc = new WarehouseCheckSearchCondition();
        //        wc.CustomerID = Int64.Parse(customerID);
        //        wc.Warehouse = warehouseID;
        //        wc.Area = AreaID;
        //        wc.Type = int.Parse(Type);
        //        wc.Type_description = Types;
        //        wc.str1 = Condition1;
        //        wc.str2 = Condition2;
        //        var response = new WarehouseService().GetWarehouseCheck(new GetWarehouseCheckByConditonRequest() { WLSearchCondition = wc });
        //        WarehouseCheckModel vm = new WarehouseCheckModel();
        //        vm.WarehouseCheckDetailCollection = response.Result.WarehouseCheckDetailCollection;
        //    }
        //    return View();
        //}
        //ajax方式删除盘点单 

        //应收结算删除
        [HttpPost]
        public JsonResult SettlementDelete(long customerID, long warehouseID, string SettlementNumber)
        {
            SettlementSearchCondition wc = new SettlementSearchCondition();
            wc.CustomerID = customerID;
            wc.WarehouseID = warehouseID;
            wc.SettlementNumber = SettlementNumber;
            var response = new SettlementService().GetSettlementDelete(new GetSettlementByConditionRequest() { WLSearchCondition = wc });
            return Json(response);
        }

        //ajax方式完成 应收结算确认
        [HttpPost]
        [ValidateInput(false)]
        public string GetSettlementDone(string SettlementNumber, string jsonString)
        {
            //var responseJsonTable = jsonlist<SettlementDetail>(jsonString);
            SettlementSearchCondition wc = new SettlementSearchCondition();
            wc.SettlementNumber = SettlementNumber;
            var response = new SettlementService().GetSettlementDone(new GetSettlementByConditionRequest() { WLSearchCondition = wc });
            //return Json(response);
            return response;
        }

        public static List<T> jsonlist<T>(string str)
        {
            return JsonConvert.DeserializeObject<List<T>>(str);
        }

        //[HttpGet]
        //public ActionResult PrintWareHouseCheck(string checknumber)
        //{
        //    WarehouseCheckModel vm = new WarehouseCheckModel();
        //    var response = new WarehouseService().GetPrintWareHouseCheckByCheckNumber(checknumber);
        //    if (response.IsSuccess)
        //    {
        //        vm.WarehouseCheck = response.Result.WarehouseCheck;
        //        vm.WarehouseCheckDetailCollection = response.Result.WarehouseCheckDetailCollection;
        //    }
        //    ViewBag.CheckNumber = vm.WarehouseCheck.CheckNumber;
        //    ViewBag.ExternNumber = vm.WarehouseCheck.ExternNumber;
        //    ViewBag.CreateTime = vm.WarehouseCheck.CreateTime;
        //    return View(vm);
        //}
        //[HttpGet]
        //public ActionResult PrintWareHouseCheckNike(string checknumber)
        //{
        //    WarehouseCheckModel vm = new WarehouseCheckModel();
        //    var response = new WarehouseService().GetPrintWareHouseCheckByCheckNumberNike(checknumber);
        //    if (response.IsSuccess)
        //    {
        //        vm.WarehouseCheck = response.Result.WarehouseCheck;
        //        vm.WarehouseCheckDetailCollection = response.Result.WarehouseCheckDetailCollection;
        //    }
        //    ViewBag.CheckNumber = vm.WarehouseCheck.CheckNumber;
        //    ViewBag.ExternNumber = vm.WarehouseCheck.ExternNumber;
        //    ViewBag.CreateTime = vm.WarehouseCheck.CreateTime;
        //    return View(vm);
        //}

        //应收结算批量汇总导出
        public void SummaryExportSettlement(long CustomerID, long WarehouseID, string SettlementNumberList)
        {
            DataSet ds = new DataSet();
            var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            SettlementListViewModel vm = new SettlementListViewModel();
            SettlementSearchCondition wc = new SettlementSearchCondition();
            wc.CustomerID = CustomerID;
            wc.CustomerName = CustomerList.Where(c => c.Value == CustomerID.ToString()).Select(s => s.Text).FirstOrDefault();
            wc.WarehouseID = WarehouseID;
            wc.WarehouseName = WarehouseList.Where(c => c.Value == WarehouseID.ToString()).Select(s => s.Text).FirstOrDefault();
            wc.SettlementNumber = SettlementNumberList;
            vm.SearchCondition = wc;
            ds = new SettlementService().SummaryExportSettlementBySettlementNumber_b(new GetSettlementByConditionRequest() { WLSearchCondition = wc });

            ExportDataToExcelHelper.ExportExcel(ds, "结算汇总报表 " + DateTime.Now.ToString("yyyy-MM-dd"), "Summary", "OB Detail");
        }

        //应收结算批量单条导出
        public void ExportSettlement(long CustomerID, long WarehouseID, string SettlementNumber)
        {
            DataSet ds = new DataSet();
            var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            SettlementListViewModel vm = new SettlementListViewModel();
            SettlementSearchCondition wc = new SettlementSearchCondition();
            wc.CustomerID = CustomerID;
            wc.CustomerName = CustomerList.Where(c => c.Value == CustomerID.ToString()).Select(s => s.Text).FirstOrDefault();
            wc.WarehouseID = WarehouseID;
            wc.WarehouseName = WarehouseList.Where(c => c.Value == WarehouseID.ToString()).Select(s => s.Text).FirstOrDefault();
            wc.SettlementNumber = SettlementNumber;
            vm.SearchCondition = wc;
            ds = new SettlementService().ExportSettlementBySettlementNumber_b(new GetSettlementByConditionRequest() { WLSearchCondition = wc });
            
            ExportDataToExcelHelper.ExportExcel(ds, "结算汇总报表 " + DateTime.Now.ToString("yyyy-MM-dd"), "Summary", "OB Detail");
        }

        //仓库变更
        public string ChangeWarehouse(long str)
        {
            string js = string.Empty;
            if (str != 0)
            {
                //ApplicationConfigHelper.RefreshGetWarehouseAreaList(str);
                IEnumerable<AreaInfo> list = ApplicationConfigHelper.GetWarehouseAreaList(str);
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

        //客户变更
        public string ChangeCustomer(long str)
        {
            string js = string.Empty;
            if (str != 0)
            {
                //ApplicationConfigHelper.RefreshGetWarehouseListByCustomerID(str);
                IEnumerable<WarehouseInfo> list = ApplicationConfigHelper.GetWarehouseListByCustomer(str);
                List<SelectListItem> st = new List<SelectListItem>();
                foreach (WarehouseInfo warehouse in list)
                {
                    st.Add(new SelectListItem() { Value = warehouse.ID.ToString(), Text = warehouse.WarehouseName });
                }
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                js = jsonSerializer.Serialize(list);
            }
            return js;
        }
        #endregion

        #region 北京喜利得
        //查询结算信息
        [HttpGet]
        public ActionResult SelHilti(HiltiModel wa, string message, string message2)
        {
            if (message == "新增成功")
            {
                ViewBag.message = "操作成功";
            }
            else if (message2 == "1")
            {
                ViewBag.message2 = "1";
            }


            wa.SearchCondition = new WMS_HiltibjSettled();
            var Request = new GetSettlementByConditionRequest();
            Request.HiltibjSettled = wa.SearchCondition;
            Request.HiltibjSettled.PageSize = UtilConstants.PAGESIZE;//
            Request.HiltibjSettled.PageIndex = 0;
            int Cid = 50;
            var Requests = new SettlementService().GetHiltiList(Request, wa.StartSettlementdate, wa.EndSettlementdate, wa.DateTime1,Cid);
            if (Requests.IsSuccess)
            {
                wa.SettlementCollection = Requests.Result.HilSettlementCollection;
                wa.PageIndex = Requests.Result.PageIndex;
                wa.PageCount = Requests.Result.PageCount;
            }
            return View(wa);
        }

        [HttpPost]
        public ActionResult SelHilti(HiltiModel wc, int? PageIndex, long? customerID, long? warehouseID, string Action)
        {
            var Request = new GetSettlementByConditionRequest();
            Request.HiltibjSettled = wc.SearchCondition;
            Request.HiltibjSettled.PageSize = UtilConstants.PAGESIZE;//
            Request.HiltibjSettled.PageIndex = PageIndex ?? 0;
            int Cid = 50;
            var Requests = new SettlementService().GetHiltiList(Request, wc.StartSettlementdate, wc.EndSettlementdate, wc.DateTime1, Cid);


            if (Action == "导出")
            {

                DataTable dt = new SettlementService().ExportSettlement(Request, wc.StartSettlementdate, wc.EndSettlementdate, wc.DateTime1);

                if (dt != null && dt.Rows.Count > 0)
                {

                    dt = this.ExportSettlementSummaryTable(dt, Cid);
                    ExportDataToExcelHelper.ExportDataSetToExcel(dt, "喜利得结算信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", "");
                    return new EmptyResult();
                }
            }



            if (Requests.IsSuccess)
            {
                wc.SettlementCollection = Requests.Result.HilSettlementCollection;
                wc.PageIndex = Requests.Result.PageIndex;
                wc.PageCount = Requests.Result.PageCount;
            }
            return View(wc);
        }
        //导出
        private DataTable ExportSettlementSummaryTable(DataTable settledPods,int Cid)
        {
            DataTable dt = new DataTable();
            if (Cid == 50)
            {
                dt.Columns.Add("仓库", typeof(string));
                dt.Columns.Add("结算单号", typeof(string));
                dt.Columns.Add("结算日期", typeof(string));
                dt.Columns.Add("入库件数", typeof(string));
                dt.Columns.Add("出库件数", typeof(string));
                dt.Columns.Add("入库费用(单价：6)", typeof(string));
                dt.Columns.Add("出库费用(单价：7)", typeof(string));
                dt.Columns.Add("加班费(单价：17.5)", typeof(string));
                dt.Columns.Add("周末加班费(单价:24)", typeof(string));
                dt.Columns.Add("中文标签费(费率：0.2)", typeof(string));
                dt.Columns.Add("防火标签费(费率：1)", typeof(string));
                dt.Columns.Add("贴保修卡费(费率：2)", typeof(string));
                dt.Columns.Add("纸箱包装费(费率：9.6)", typeof(string));
                dt.Columns.Add("槽钢包装材料费(费率：0.6)", typeof(string));
                dt.Columns.Add("产品合格证费(费率：0.5)", typeof(string));
                dt.Columns.Add("其他(费率：1)", typeof(string));
                dt.Columns.Add("包干费", typeof(string));
                dt.Columns.Add("包干平均费用", typeof(string));
                dt.Columns.Add("操作总费用", typeof(string));
                dt.Columns.Add("服务总费用", typeof(string));
                foreach (DataRow item in settledPods.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["仓库"] = item["WarehouseName"].ToString();
                    dr["结算单号"] = item["SettlementNumber"].ToString();
                    dr["结算日期"] = item["CreateTime"].ToString();
                    dr["入库件数"] = item["Number1"].ToString();
                    dr["出库件数"] = item["Number2"].ToString();
                    dr["入库费用(单价：6)"] = item["OperationCost1"].ObjectToDecimal().ToString("0.00");
                    dr["出库费用(单价：7)"] = item["OperationCost2"].ObjectToDecimal().ToString("0.00");
                    dr["加班费(单价：17.5)"] = item["OperationCost3"].ObjectToDecimal().ToString("0.00");
                    dr["周末加班费(单价:24)"] = item["OperationCost4"].ObjectToDecimal().ToString("0.00");
                    dr["中文标签费(费率：0.2)"] = item["Cost1"].ObjectToDecimal().ToString("0.00");
                    dr["防火标签费(费率：1)"] = item["Cost2"].ObjectToDecimal().ToString("0.00");
                    dr["贴保修卡费(费率：2)"] = item["Cost3"].ObjectToDecimal().ToString("0.00");

                    dr["纸箱包装费(费率：9.6)"] = item["Cost4"].ObjectToDecimal().ToString("0.00");
                    dr["槽钢包装材料费(费率：0.6)"] = item["Cost5"].ObjectToDecimal().ToString("0.00");
                    dr["产品合格证费(费率：0.5)"] = item["Cost6"].ObjectToDecimal().ToString("0.00");
                    dr["其他(费率：1)"] = item["Cost7"].ObjectToDecimal().ToString("0.00");
                    dr["包干费"] = item["OutsourcingTotalSum"].ObjectToDecimal().ToString("0.00");
                    dr["包干平均费用"] = item["OutsourcingAveragecost"].ObjectToDecimal().ToString("0.00"); ;
                    dr["操作总费用"] = item["OperationTotal"].ObjectToDecimal().ToString("0.00");
                    dr["服务总费用"] = item["TotalCost"].ObjectToDecimal().ToString("0.00"); ;
                    dt.Rows.Add(dr);
                }

                return dt;
            }
            else
            {
                dt.Columns.Add("仓库", typeof(string));
                dt.Columns.Add("结算单号", typeof(string));
                dt.Columns.Add("结算日期", typeof(string));
                dt.Columns.Add("仓租收入(单价/天:￥1)", typeof(string));
                dt.Columns.Add("入库管理费(单价/立方:￥12)", typeof(string));
                dt.Columns.Add("出库管理费(单价/立方:￥12)", typeof(string));
                dt.Columns.Add("电商出库拣货费(单价/单:￥3)", typeof(string));
                dt.Columns.Add("门店出库拣货费(单价/单:￥20)", typeof(string));
                dt.Columns.Add("门店出库理货费(单价/单:￥18)", typeof(string)); 
               
                foreach (DataRow item in settledPods.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["仓库"] = item["WarehouseName"].ToString();
                    dr["结算单号"] = item["SettlementNumber"].ToString();
                    dr["结算日期"] = item["CreateTime"].ToString();
                    dr["仓租收入(单价/天:￥1)"] = item["OperationCost2"].ToString();
                    dr["入库管理费(单价/立方:￥12)"] = item["OperationCost4"].ToString();
                    dr["出库管理费(单价/立方:￥12)"] = item["Cost2"].ToString();
                    dr["电商出库拣货费(单价/单:￥3)"] = item["Cost5"].ObjectToDecimal().ToString("0.00");
                    dr["门店出库拣货费(单价/单:￥20)"] = item["Cost6"].ObjectToDecimal().ToString("0.00");
                    dr["门店出库理货费(单价/单:￥18)"] = item["Cost4"].ObjectToDecimal().ToString("0.00");
                   
                    dt.Rows.Add(dr);
                }

                return dt;
            }
        }
        //将结算信息添加
        [HttpGet]
        public ActionResult HiltiAddSettlement(string date, HiltiModel vm)
        {
            SettlementService service = new SettlementService();
            int CustomerID = 96;
            var response = new SettlementService().Count(date, CustomerID);
            foreach (var item in response.Result.A)
            {
                vm.Count1 = item.count1;
                vm.SumCount1 = item.count1 * 6;
            }
            foreach (var item in response.Result.B)
            {
                vm.Count2 = item.count2;
                vm.SumCount2 = item.count2 * 7;
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult HiltiAddSettlement(HiltiModel vm)
        {
            if (vm.Count1 == 0 && vm.Count2 == 0)
            {
                return RedirectToAction("SelHilti", new { message2 = "1" });
            }
            WMS_HiltibjSettled c = new WMS_HiltibjSettled
            {
                WarehouseID = 50,
                WarehouseName = "喜利得北京仓",
                SettlementNumber = "Hi" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                OutsourcingTotalSum = vm.inpTotal,
                OutsourcingAveragecost = vm.inpAvg,
                Number1 = vm.Count1,
                Number2 = vm.Count2,
                Number3 = vm.number3,
                Number4 = vm.number4,
                OperationCost1 = vm.SumCount1,
                OperationCost2 = vm.SumCount2,
                OperationCost3 = vm.opCost3,
                OperationCost4 = vm.opCost4,
                OperationTotal = vm.SumCount1 + vm.SumCount2 + vm.opCost3 + vm.opCost4,
                Num1 = vm.inNumber1,
                Num2 = vm.inNumber2,
                Num3 = vm.inNumber3,
                Num4 = vm.inNumber4,
                Num5 = vm.inNumber5,
                Num6 = vm.inNumber6,
                Num7 = vm.inNumber7,
                Cost1 = vm.inCost1,
                Cost2 = vm.inCost2,
                Cost3 = vm.inCost3,
                Cost4 = vm.inCost4,
                Cost5 = vm.inCost5,
                Cost6 = vm.inCost6,
                Cost7 = vm.inCost7,
                TotalCost = vm.inCost1 + vm.inCost2 + vm.inCost3 + vm.inCost4 + vm.inCost5 + vm.inCost6 + vm.inCost7

            };


            var response = new SettlementService().AddInfoHiltiNewTab(c);
            if (response.IsSuccess)
            {
                return RedirectToAction("SelHilti", new { message = "新增成功" });
            }

            return Error("添加失败，请联系IT");


        }
        //删除
        [HttpPost]
        public JsonResult Delete(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return Json(new { ReturnVal = -1, Message = "传入数据有误，请联系IT" });
            }

            int response = new SettlementService().DeleteHiltibjSettled(ids);
            if (response > 0)
            {

                return Json(new { ReturnVal = 1, Message = "删除成功" });
            }

            return Json(new { ReturnVal = -2, Message = "删除失败，请联系IT" });
        }
        #endregion

        #region HABA
        [HttpGet]
        public ActionResult Settlementpage(string date, HabaModel vm)
        {
            SettlementService service = new SettlementService();
            int CustomerID = 98;
            var response = new SettlementService().Count(date, CustomerID);
            foreach (var item in response.Result.C)
            {
                vm.vo1 = item.vo1;     //获取当天收入体积之和
                vm.vo1Cost = item.vo1 * 12;//入库管理费
            }
            foreach (var item in response.Result.D)
            {
                //电商出库总单数：sum1，经销商出库总单数：sum2，经销商出库订单体积之和：sum3
                vm.sum3 = item.sum3;     //门店出库管理费，最低收费40
                decimal? dec=item.sum3 * 12;
                if (dec<40)
                {
                    dec = 40;
                }
                vm.sumCost1 = dec;
               // vm.sum3 = item.sum3;     //门店出库理货费
                vm.sumCost2 = item.sum3 * 18;
                vm.sum1 = item.sum1;     //电商出库拣货费
                vm.sumCost3 = item.sum1 * 3;
                vm.sum2 = item.sum2;     //门店出库拣货费
                vm.sumCost4 = item.sum2 * 20;
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult Settlementpage(HabaModel vm)
        {
          
            WMS_HiltibjSettled c = new WMS_HiltibjSettled
            {
                WarehouseID = 53,
                WarehouseName = "HABA上海仓",
                SettlementNumber = "Ha" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                OperationCost2 = vm.CzSr,     //仓库收入
                OperationCost4 = vm.vo1Cost,                 //入库管理费
                Cost2 = vm.sumCost1,                //门店出库管理费
                Cost4 = vm.sumCost2,                //门店出库理货费
                Cost5 = vm.sumCost3,                //电商出货拣货费
                Cost6 = vm.sumCost4,                 //门店出库拣货费

                TotalCost = vm.CzSr + vm.vo1Cost + vm.sumCost1 + vm.sumCost2 + 
                vm.sumCost3 + vm.sumCost4

            };


            var response = new SettlementService().AddInfoHiltiNewTab(c);
            if (response.IsSuccess)
            {
                return RedirectToAction("SelHaba", new { message = "新增成功" });
            }

            return Error("添加失败，请联系IT");

        }


        //查询结算信息
        [HttpGet]
        public ActionResult SelHaba(HabaModel wa, string message, string message2)
        {
            if (message == "新增成功")
            {
                ViewBag.message = "操作成功";
            }
            else if (message2 == "1")
            {
                ViewBag.message2 = "1";
            }


            wa.SearchCondition = new WMS_HiltibjSettled();

            //wa.SearchCondition.StartSettlementdate = DateTime.Now.AddDays(-10);
            //wa.SearchCondition.EndSettlementdate = DateTime.Now;
            var Request = new GetSettlementByConditionRequest();
            Request.HiltibjSettled = wa.SearchCondition;
            Request.HiltibjSettled.PageSize = UtilConstants.PAGESIZE;//
            Request.HiltibjSettled.PageIndex = 0;
            int Cid = 53;
            var Requests = new SettlementService().GetHiltiList(Request, wa.StartSettlementdate, wa.EndSettlementdate, wa.DateTime1, Cid);
            if (Requests.IsSuccess)
            {
                wa.SettlementCollection = Requests.Result.HilSettlementCollection;
                wa.PageIndex = Requests.Result.PageIndex;
                wa.PageCount = Requests.Result.PageCount;
            }
            return View(wa);
        }

        [HttpPost]
        public ActionResult SelHaba(HabaModel wc, int? PageIndex, long? customerID, long? warehouseID, string Action)
        {
            var Request = new GetSettlementByConditionRequest();
            Request.HiltibjSettled = wc.SearchCondition;
            Request.HiltibjSettled.PageSize = UtilConstants.PAGESIZE;//
            Request.HiltibjSettled.PageIndex = PageIndex ?? 0;
            int Cid = 53;
            var Requests = new SettlementService().GetHiltiList(Request, wc.StartSettlementdate, wc.EndSettlementdate, wc.DateTime1, Cid);


            if (Action == "导出")
            {

                DataTable dt = new SettlementService().ExportSettlement(Request, wc.StartSettlementdate, wc.EndSettlementdate, wc.DateTime1);

                if (dt != null && dt.Rows.Count > 0)
                {

                    dt = this.ExportSettlementSummaryTable(dt, Cid);
                    ExportDataToExcelHelper.ExportDataSetToExcel(dt, "Haba结算信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", "");
                    return new EmptyResult();
                }
            }



            if (Requests.IsSuccess)
            {
                wc.SettlementCollection = Requests.Result.HilSettlementCollection;
                wc.PageIndex = Requests.Result.PageIndex;
                wc.PageCount = Requests.Result.PageCount;
            }
            return View(wc);
        }
      
        #endregion
    }
}