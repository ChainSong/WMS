using Runbow.TWS.Web.Areas.WMS.Models.Inventory;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity.WMS.Inventory;
using UtilConstants = Runbow.TWS.Common.Constants;
using Runbow.TWS.MessageContracts.WMS.Inventory;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Runbow.TWS.Common;
using MyFile = System.IO.File;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Script.Serialization;
using Runbow.TWS.MessageContracts;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using SysIO = System.IO;
using we = System.Web;
using System.Data.SqlClient;
using System.Collections;
using Runbow.TWS.Web.Areas.WMS.Models.InventoryManagement;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity.WMS.Replenishment;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.Entity.WMS;
using System.Net;
using Runbow.TWS.MessageContracts.WMS.JCApi;
using Runbow.TWS.Entity.WMS.Product;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class InventoryManagementController : BaseController
    {
        public string TestIndex()
        {
            return "123";
        }
        public ActionResult PrintReplenishmentYFBLD(string ID)
        {
            ReplenishmentViewModel vm = new ReplenishmentViewModel();
            var response = new ReplenishmentManagementService().PrintReplishmentYFBLD(ID);


            if (response.IsSuccess)
            {
                vm.ReplenishmentCollection = response.Result.ReplenishmentCollection;
                vm.ReplenishmentDetailCollection = response.Result.ReplenishmentDetailCollection;
            }
            ViewBag.rsid = ID;
            return View(vm);
        }
        [HttpPost]
        public JsonResult ComplateReplenishment(string ID)
        {
            int RSID = 0;
            int.TryParse(ID, out RSID);
            if (new ReplenishmentManagementService().Complate(RSID).IsSuccess)
            {
                return Json(new { Message = "完成成功", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = "完成失败！", IsSuccess = false });
            }
        }
        [HttpPost]
        public JsonResult CancelReplenishment(string ID)
        {
            int RSID = 0;
            int.TryParse(ID, out RSID);
            if (new ReplenishmentManagementService().Cancel(RSID).IsSuccess)
            {
                return Json(new { Message = "取消成功", IsSuccess = true });
            }
            else
            {
                return Json(new { Message = "取消失败！", IsSuccess = false });
            }
        }
        [HttpPost]
        public JsonResult GenerateReplenishment(string CustomerID, string CustomerName, string WarehouseID, string WarehouseName, string SKU, string Remark, string Qty)
        {
            int IQty = 0;
            Int32.TryParse(Qty, out IQty);
            List<ReplenishmentDetailSKUs> list = new List<ReplenishmentDetailSKUs>();

            foreach (var item in SKU.Split(new char[] { '\n' }))
            {
                ReplenishmentDetailSKUs s = new ReplenishmentDetailSKUs();
                s.SKU = item;
                list.Add(s);
            }
            IEnumerable<ReplenishmentDetailSKUs> skus = list;

            if (IQty > 0)
            {
                var getReplenishmentByConditionResponse = new ReplenishmentManagementService().GenerateReplenishmentByNumber(list, base.UserInfo.ProjectID.ToString(), CustomerID, CustomerName, WarehouseID, WarehouseName, Remark, base.UserInfo.Name, IQty);

                if (getReplenishmentByConditionResponse.IsSuccess)
                {
                    return Json(new { Message = "操作成功", IsSuccess = true, ID = getReplenishmentByConditionResponse.Result.replenishment.ID });
                }
                else
                {
                    return Json(new { Message = "操作失败！", IsSuccess = false });
                }
            }
            else
            {
                var getReplenishmentByConditionResponse = new ReplenishmentManagementService().GenerateReplenishment(list, base.UserInfo.ProjectID.ToString(), CustomerID, CustomerName, WarehouseID, WarehouseName, Remark, base.UserInfo.Name);

                if (getReplenishmentByConditionResponse.IsSuccess)
                {
                    return Json(new { Message = "操作成功", IsSuccess = true, ID = getReplenishmentByConditionResponse.Result.replenishment.ID });
                }
                else
                {
                    return Json(new { Message = "操作失败！", IsSuccess = false });
                }
            }

        }
        [HttpGet]
        public ActionResult AddorEditorViewReplenishment(int ID, long? customerID, int ViewType = 0)
        {
            ReplenishmentViewModel vm = new ReplenishmentViewModel();

            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.ReplenishmentCondition = new ReplenishmentSearchCondition();
            if (customerID != null && customerID != 0)
            {


                var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

                ViewBag.WarehouseList = WarehouseList;
                //ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == customerID)
                //                  .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                //ViewBag.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
                //                    .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            }
            else
            {
                //ViewBag.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
                //                     .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

            }


            //vm.ReplenishmentCondition.CreateTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getReplenishmentByConditionResponse = new ReplenishmentManagementService().GetReplenishmentInfos(new GetReplenishmentByConditionRequest() { ID = ID });


            if (getReplenishmentByConditionResponse.IsSuccess)
            {
                vm = new ReplenishmentViewModel()
                {
                    ReplenishmentAndReplenishmentDetails = getReplenishmentByConditionResponse.Result
                };
            }
            if (ViewType == 1)
            {
                vm.ReplenishmentAndReplenishmentDetails = new ReplenishmentAndReplenishmentDetail();
                vm.ReplenishmentAndReplenishmentDetails.replenishment = new Replenishment();
                //vm.ReplenishmentAndReplenishmentDetails.replenishment.WarehouseID = getReplenishmentByConditionResponse.Result.replenishment.WarehouseID;
            }
            if (ViewType == 2)
            {
                var WarehouseID = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.ReplenishmentAndReplenishmentDetails.replenishment.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct().Where(n => n.WarehouseName == vm.ReplenishmentAndReplenishmentDetails.replenishment.WarehouseName)
                                   .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString() }).FirstOrDefault().Value;
                var UnitAndSpecificationsList = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(base.UserInfo.ProjectID, vm.ReplenishmentAndReplenishmentDetails.replenishment.CustomerID, WarehouseID.ObjectToInt64());
                var UnitList = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Unit }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
                var SpecificationsList = UnitAndSpecificationsList.Select(t => new { Code = t.Specifications, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });

                ViewBag.skulist = vm.ReplenishmentAndReplenishmentDetails.replenishmentDetails.Select(m => new SelectListItem() { Value = m.SKU + "|" + m.GoodsName, Text = m.SKU + "|" + m.GoodsType });
                ViewBag.upclist = vm.ReplenishmentAndReplenishmentDetails.replenishmentDetails.Select(m => new SelectListItem() { Value = m.UPC, Text = m.UPC });
                ViewBag.batchnumberlist = vm.ReplenishmentAndReplenishmentDetails.replenishmentDetails.Select(m => new SelectListItem() { Value = m.BatchNumber, Text = m.BatchNumber });
                ViewBag.boxnumberlist = vm.ReplenishmentAndReplenishmentDetails.replenishmentDetails.Select(m => new SelectListItem() { Value = m.BoxNumber, Text = m.BoxNumber });
                //ViewBag.Unitlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Unit });
                //ViewBag.Specificationslist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.
                ViewBag.Unitlist = UnitList;
                //vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Unit });
                ViewBag.Specificationslist = SpecificationsList;
            }
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            if (customerID != null)
            {
                if (base.UserInfo.UserType == 0)
                {
                    vm.ReplenishmentAndReplenishmentDetails.replenishment.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        vm.ReplenishmentAndReplenishmentDetails.replenishment.CustomerID = customerID;
                    }
                    else
                    {
                        var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                        if (customerIDs != null && customerIDs.Count() == 1)
                        {
                            vm.ReplenishmentAndReplenishmentDetails.replenishment.CustomerID = customerIDs.First();
                        }
                    }
                }
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
            vm.GoodsTypes = wms.Select(c => new SelectListItem() { Value = c.Name, Text = c.Name }); ;

            vm.ViewType = ViewType;
            //vm.ReplenishmentAndReplenishmentDetails.replenishment.CreateTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));

            return View(vm);
        }
        [HttpGet]
        public ActionResult Replenishment(int? PageIndex, long? customerID, long? warehouseID, long? warehosueAreaID)
        {
            Session["ReplenishmentConditionModel"] = null;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            ReplenishmentViewModel vm = new ReplenishmentViewModel();

            vm.ReplenishmentCondition = new ReplenishmentSearchCondition();
            vm.ReplenishmentCondition.StartCreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
            vm.ReplenishmentCondition.EndCreateTime = DateTime.Now.ToString("yyyy-MM-dd").ObjectToNullableDateTime();
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            if (base.UserInfo.UserType == 0)
            {
                vm.ReplenishmentCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.ReplenishmentCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.ReplenishmentCondition.CustomerID = customerIDs.First();
                    }
                }
            }
            vm.ReplenishmentCondition.WarehouseID = warehouseID;
            //vm.ReplenishmentCondition.str19 = warehouseAreaID.ToString();
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.ReplenishmentCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName }).Distinct();
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.ReplenishmentCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            if (WarehouseList.Count() == 1)
            {
                vm.ReplenishmentCondition.Warehouse = WarehouseList.Select(a => a.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;
            ////if (!string.IsNullOrEmpty(vm.AdjustmentCondition.Warehouse))
            ////{
            ////    ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == Int64.Parse(vm.AdjustmentCondition.Warehouse)).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            ////}
            //ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehouseID).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            ////ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehouseID).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });

            ////if (vm.AdjustmentCondition.CustomerID != null)
            ////{
            ////    ViewBag.LocationList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Location(vm.AdjustmentCondition.CustomerID.Value).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Location });
            ////}
            ////else
            ////{
            ////    ViewBag.LocationList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Location().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Location });
            ////}
            if (WarehouseList.Count() == 1)
            {
                vm.ReplenishmentCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            }
            ViewBag.WarehouseList = WarehouseList;

            GetReplenishmentByConditionRequest request = new GetReplenishmentByConditionRequest();
            request.SearchCondition = vm.ReplenishmentCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            var response = new ReplenishmentManagementService().GetReplenishmentByCondition(request);
            if (response.IsSuccess)
            {
                vm.ReplenishmentCollection = response.Result.ReplenishmentCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            Session["ReplenishmentConditionModel"] = vm.ReplenishmentCondition;
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            //return View(vm);
            return View(vm);
        }
        [HttpPost]
        public ActionResult Replenishment(ReplenishmentViewModel vm, int? PageIndex, string Action)
        {
            if (vm.ReplenishmentCondition == null && Session["ReplenishmentConditionModel"] != null)
            {
                vm.ReplenishmentCondition = (ReplenishmentSearchCondition)Session["ReplenishmentConditionModel"];
            }
            else if (vm.ReplenishmentCondition == null && Session["ReplenishmentConditionModel"] == null)
            {
                Session["ReplenishmentConditionModel"] = null;
                Session["ReplenishmentConditionModel"] = vm.ReplenishmentCondition;
            }
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.ReplenishmentCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.ReplenishmentCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            ViewBag.CustomerList = CustomerList;
            ViewBag.WarehouseList = WarehouseList;
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });

            if (!string.IsNullOrEmpty(vm.ReplenishmentCondition.Warehouse))
            {
                ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == Int64.Parse(vm.ReplenishmentCondition.Warehouse)).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            }

            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getReplenishmentByConditionRequest = new GetReplenishmentByConditionRequest();

            getReplenishmentByConditionRequest.SearchCondition = vm.ReplenishmentCondition;
            getReplenishmentByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getReplenishmentByConditionRequest.PageIndex = PageIndex ?? 0;

            var getReplenishmentByConditionResponse = new ReplenishmentManagementService().GetReplenishmentByCondition(getReplenishmentByConditionRequest);

            if (getReplenishmentByConditionResponse.IsSuccess)
            {
                vm.ReplenishmentCollection = getReplenishmentByConditionResponse.Result.ReplenishmentCollection;
                vm.PageIndex = getReplenishmentByConditionResponse.Result.PageIndex;
                vm.PageCount = getReplenishmentByConditionResponse.Result.PageCount;
                //IEnumerable<Column> columnadjustment;
                //IEnumerable<Column> columnadjustmentdetail;
                //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.ReplenishmentCondition.CustomerID).ProjectCollection.First();
                //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                //if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Replenishment").Count() == 0)
                //{
                //    columnadjustment = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Replenishment").ColumnCollection;
                //}
                //else
                //{
                //    columnadjustment = module.Tables.TableCollection.First(t => t.Name == "WMS_Replenishment").ColumnCollection;
                //}
                //if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_ReplenishmentDetail").Count() == 0)
                //{
                //    columnadjustmentdetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_ReplenishmentDetail").ColumnCollection;
                //}
                //else
                //{
                //    columnadjustmentdetail = module.Tables.TableCollection.First(t => t.Name == "WMS_ReplenishmentDetail").ColumnCollection;
                //}
                //if (vm.ReplenishmentCondition.CustomerID == 0)
                //{
                //    columnadjustment = columnadjustment.Where(c => (c.IsKey == true && c.ForView == true && c.DbColumnName != "CustomerID" && c.DbColumnName != "ID"));
                //    columnadjustmentdetail = columnadjustmentdetail.Where(c => (c.IsKey == true && c.ForView == true && c.DbColumnName != "CustomerID" && c.DbColumnName != "ID" && c.DbColumnName != "AID" && c.DbColumnName != "IsHold"));
                //}
                if (Action == "导出")
                {
                    //Export(getAdjustByConditionResponse.Result, columnadjustment.Where(c => (c.IsKey == true && c.ForView == true && c.DbColumnName != "CustomerID" && c.DbColumnName != "ID")), columnadjustmentdetail.Where(c => (c.IsKey == true && c.ForView == true && c.DbColumnName != "CustomerID" && c.DbColumnName != "ID" && c.DbColumnName != "AID")));
                }
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }
        [HttpGet]
        public ActionResult Index(int? PageIndex, long? customerID, long? warehouseID, long? warehouseAreaID)
        {

            //GetLocationList("37-01-A", "22", 34);
            Session["AdjustmentConditionModel"] = null;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            IndexViewModel vm = new IndexViewModel();
            vm.AdjustmentCondition = new AdjustmentSearchCondition();
            vm.AdjustmentCondition.StartAdjustmentDate = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
            vm.AdjustmentCondition.EndAdjustmentDate = DateTime.Now.ToString("yyyy-MM-dd").ObjectToNullableDateTime();
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            if (base.UserInfo.UserType == 0)
            {
                vm.AdjustmentCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.AdjustmentCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.AdjustmentCondition.CustomerID = customerIDs.First();
                    }
                    else
                    {
                        vm.AdjustmentCondition.CustomerID = 0;
                    }
                }
            }
            //vm.AdjustmentCondition.Warehouse = warehouseID.ToString();
            vm.AdjustmentCondition.str19 = warehouseAreaID.ToString();
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.AdjustmentCondition.CustomerID == 0)
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
                    vm.AdjustmentCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.AdjustmentCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.AdjustmentCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

            }
            if (warehouseID != null)
            {
                vm.AdjustmentCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == warehouseID.ToString()).First().Value + "'";
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
                    vm.AdjustmentCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }
            ViewBag.WarehouseList = WarehouseList;
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.AdjustmentCondition.Warehouse = WarehouseList.Select(a => a.Value).FirstOrDefault();
            //}
            //if (!string.IsNullOrEmpty(vm.AdjustmentCondition.Warehouse))
            //{
            //    ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == Int64.Parse(vm.AdjustmentCondition.Warehouse)).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            //}
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehouseID).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            //ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehouseID).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });

            //if (vm.AdjustmentCondition.CustomerID != null)
            //{
            //    ViewBag.LocationList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Location(vm.AdjustmentCondition.CustomerID.Value).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Location });
            //}
            //else
            //{
            //    ViewBag.LocationList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Location().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Location });
            //}
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.AdjustmentCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault();
            //}
            ViewBag.WarehouseList = WarehouseList;
            GetAdjustmentByConditionRequest getadjustByConditionRequest = new GetAdjustmentByConditionRequest();
            getadjustByConditionRequest.SearchCondition = vm.AdjustmentCondition;
            getadjustByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getadjustByConditionRequest.PageIndex = PageIndex ?? 0;
            var getadjustByConditionResponse = new AdjustmentManagementService().GetAdjustmentByCondition(getadjustByConditionRequest);
            if (getadjustByConditionResponse.IsSuccess)
            {
                vm.AdjustmentCollection = getadjustByConditionResponse.Result.AdjustmentCollection;
                vm.PageIndex = getadjustByConditionResponse.Result.PageIndex;
                vm.PageCount = getadjustByConditionResponse.Result.PageCount;
            }
            Session["AdjustmentConditionModel"] = vm.AdjustmentCondition;
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }
        [HttpPost]
        public ActionResult Index(IndexViewModel vm, int? PageIndex, string Action)
        {
            if (vm.AdjustmentCondition == null && Session["AdjustmentConditionModel"] != null)
            {
                vm.AdjustmentCondition = (AdjustmentSearchCondition)Session["AdjustmentConditionModel"];
            }
            else if (vm.AdjustmentCondition == null && Session["AdjustmentConditionModel"] == null)
            {
                Session["AdjustmentConditionModel"] = null;
                Session["AdjustmentConditionModel"] = vm.AdjustmentCondition;
            }
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            IEnumerable<SelectListItem> WarehouseList = null;

            if (vm.AdjustmentCondition.CustomerID == null)
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
                    vm.AdjustmentCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.AdjustmentCondition.CustomerIDs = "0";


                }
                vm.AdjustmentCondition.CustomerID = 0;

            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.AdjustmentCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            ViewBag.CustomerList = CustomerList;
            ViewBag.WarehouseList = WarehouseList;
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            if (!string.IsNullOrEmpty(vm.AdjustmentCondition.Warehouse))
            {
                ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == Int64.Parse(vm.AdjustmentCondition.Warehouse.ToString())).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            }
            if (vm.AdjustmentCondition.Warehouse != null)
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
                    vm.AdjustmentCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }
            //if (!string.IsNullOrEmpty(vm.AdjustmentCondition.Warehouse))
            //{
            //    ViewBag.LocationList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Location().Where(c => c.WarehouseID == Int64.Parse(vm.AdjustmentCondition.Warehouse)).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Location });
            //} if (!string.IsNullOrEmpty(vm.AdjustmentCondition.str19))
            //{
            //    ViewBag.LocationList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Location().Where(c => c.AreaID == Int64.Parse(vm.AdjustmentCondition.str19)).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Location });
            //}


            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getAdjustmentByConditionRequest = new GetAdjustmentByConditionRequest();

            getAdjustmentByConditionRequest.SearchCondition = vm.AdjustmentCondition;
            getAdjustmentByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getAdjustmentByConditionRequest.PageIndex = PageIndex ?? 0;

            //var getAdjustByConditionResponse = new AdjustmentManagementService().GetAdjustmentByCondition(getAdjustmentByConditionRequest);

            var getAdjustByConditionResponse = new Response<GetAdjustmentDetailByConditionResponse>();
            //导出与查询分离
            if (Action == "导出")
            {
                getAdjustByConditionResponse = new AdjustmentManagementService().ExportAdjustmentByCondition(getAdjustmentByConditionRequest);
            }
            else
            {
                getAdjustByConditionResponse = new AdjustmentManagementService().GetAdjustmentByCondition(getAdjustmentByConditionRequest);
            }

            if (getAdjustByConditionResponse.IsSuccess)
            {
                vm.AdjustmentCollection = getAdjustByConditionResponse.Result.AdjustmentCollection;
                vm.PageIndex = getAdjustByConditionResponse.Result.PageIndex;
                vm.PageCount = getAdjustByConditionResponse.Result.PageCount;
                if (Action == "导出")
                {
                    IEnumerable<Column> columnadjustment;
                    IEnumerable<Column> columnadjustmentdetail;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.AdjustmentCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Adjustment").Count() == 0)
                    {
                        columnadjustment = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Adjustment").ColumnCollection;
                    }
                    else
                    {
                        columnadjustment = module.Tables.TableCollection.First(t => t.Name == "WMS_Adjustment").ColumnCollection;
                    }
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_AdjustmentDetail").Count() == 0)
                    {
                        columnadjustmentdetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_AdjustmentDetail").ColumnCollection;
                    }
                    else
                    {
                        columnadjustmentdetail = module.Tables.TableCollection.First(t => t.Name == "WMS_AdjustmentDetail").ColumnCollection;
                    }
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.AdjustmentCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Column> columnadjustment = module.Tables.TableCollection.First(t => t.Name == "WMS_Adjustment").ColumnCollection;
                    //IEnumerable<Column> columnadjustmentdetail = module.Tables.TableCollection.First(t => t.Name == "WMS_AdjustmentDetail").ColumnCollection;
                    if (vm.AdjustmentCondition.CustomerID == 0)
                    {
                        columnadjustment = columnadjustment.Where(c => (c.IsKey == true && c.ForView == true && c.DbColumnName != "CustomerID" && c.DbColumnName != "ID"));
                        columnadjustmentdetail = columnadjustmentdetail.Where(c => (c.IsKey == true && c.ForView == true && c.DbColumnName != "CustomerID" && c.DbColumnName != "ID" && c.DbColumnName != "AID" && c.DbColumnName != "IsHold"));
                    }
                    Export(getAdjustByConditionResponse.Result, columnadjustment.Where(c => (c.IsKey == true && c.ForView == true && c.DbColumnName != "CustomerID" && c.DbColumnName != "ID")), columnadjustmentdetail.Where(c => (c.IsKey == true && c.ForView == true && c.DbColumnName != "CustomerID" && c.DbColumnName != "ID" && c.DbColumnName != "AID")));
                }
                //if (Action == "导出")
                //{
                //    Export(getAdjustByConditionResponse.Result, columnadjustment.Where(c => (c.IsKey == true && c.ForView == true && c.DbColumnName != "CustomerID" && c.DbColumnName != "ID")), columnadjustmentdetail.Where(c => (c.IsKey == true && c.ForView == true && c.DbColumnName != "CustomerID" && c.DbColumnName != "ID" && c.DbColumnName != "AID")));
                //}
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }
        //导出
        private void Export(GetAdjustmentDetailByConditionResponse response, IEnumerable<Column> columnAdjust, IEnumerable<Column> columnAdjustDetail)
        {
            IEnumerable<Adjustment> adjustments = response.AdjustmentCollection;
            IEnumerable<AdjustmentDetail> adjustmentDetail = response.AdjustmentDetailCollection;
            DataSet ds = new DataSet();
            DataTable dtAdjustment = new DataTable();
            DataTable dtAdjustmentDetail = new DataTable();
            foreach (var adjust in columnAdjust)
            {
                dtAdjustment.Columns.Add(adjust.DisplayName, typeof(string));
            }
            foreach (var adjustDetailDetail in columnAdjustDetail)
            {
                dtAdjustmentDetail.Columns.Add(adjustDetailDetail.DisplayName, typeof(string));
            }
            adjustments.Each((i, s) =>
            {
                DataRow dradjustment = dtAdjustment.NewRow();
                foreach (var adjustment in columnAdjust)
                {
                    dradjustment[adjustment.DisplayName] = typeof(Runbow.TWS.Entity.Adjustment).GetProperty(adjustment.DbColumnName).GetValue(s);
                }
                dtAdjustment.Rows.Add(dradjustment);
            });
            adjustmentDetail.Each((i, s) =>
            {
                DataRow dradjustmentdetail = dtAdjustmentDetail.NewRow();
                foreach (var adjustmentdetail in columnAdjustDetail)
                {
                    dradjustmentdetail[adjustmentdetail.DisplayName] = typeof(Runbow.TWS.Entity.AdjustmentDetail).GetProperty(adjustmentdetail.DbColumnName).GetValue(s);
                }
                dtAdjustmentDetail.Rows.Add(dradjustmentdetail);
            });

            for (int i = 0; i < dtAdjustment.Rows.Count; i++)
            {
                if ("0" == dtAdjustment.Rows[i]["是否是冻结单"].ToString())
                {
                    dtAdjustment.Rows[i]["是否是冻结单"] = "否";
                }
                if ("1" == dtAdjustment.Rows[i]["是否是冻结单"].ToString())
                {
                    dtAdjustment.Rows[i]["是否是冻结单"] = "是";
                }
                if ("1" == dtAdjustment.Rows[i]["调整单状态"].ToString())
                {
                    dtAdjustment.Rows[i]["调整单状态"] = "新增";
                }
                if ("9" == dtAdjustment.Rows[i]["调整单状态"].ToString())
                {
                    dtAdjustment.Rows[i]["调整单状态"] = "已完成";
                }
            }
            dtAdjustment.TableName = "库存调整单主信息";
            dtAdjustmentDetail.TableName = "库存调整单明细信息";
            ds.Tables.Add(dtAdjustment);
            ds.Tables.Add(dtAdjustmentDetail);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "调整单" + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "调整单" + DateTime.Now.ToString("yyyy-MM-dd"));
        }
        /// <summary>
        /// 解冻
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string Unfreeze(int ID)
        {
            if (new AdjustmentManagementService().Unfreeze(ID).IsSuccess)
            {
                return Json(new { Message = "解冻成功", IsSuccess = true }).ToString();
            }
            else
            {
                return Json(new { Message = "解冻失败！", IsSuccess = false }).ToString();
            }
        }
        /// <summary>
        /// 单条取消
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public string Cancel(int ID)
        {
            //ApplicationConfigHelper.RefreshASNInfo();
            if (new AdjustmentManagementService().Cancel(ID).IsSuccess)
            {
                return Json(new { Message = "取消成功", IsSuccess = true }).ToString();
            }
            else
            {
                return Json(new { Message = "取消失败！", IsSuccess = false }).ToString();
            }
        }
        /// <summary>
        /// 单条完成操作
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string Complet(int ID, string type)
        {
            Response<bool> resp = new AdjustmentManagementService().Complets(ID, type);
            if (resp.IsSuccess)
            {
                #region 操作日志
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "库存变更管理";
                operation.Operation = "库存变更-完成";
                operation.OrderType = type;
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = ID.ToString();
                operation.Remark = "操作成功";
                logs.Add(operation);
                new LogOperationService().AddLogOperation(logs);
                #endregion
                return "操作成功";
            }
            else
            {
                #region 操作日志 失败了还加个蛋 日志
                //List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                //WMS_Log_Operation operation = new WMS_Log_Operation();
                //operation.MenuName = "库存变更管理";
                //operation.Operation = "库存变更-完成";
                //operation.OrderType = type;
                //operation.Controller = Request.RawUrl;
                //operation.Creator = base.UserInfo.Name;
                //operation.CreateTime = DateTime.Now;
                //operation.ProjectID = (int)base.UserInfo.ProjectID;
                //operation.ProjectName = base.UserInfo.ProjectName;
                //operation.OrderID = ID.ToString();
                //operation.Remark = "操作失败:" + resp.SuccessMessage;
                //logs.Add(operation);
                //new LogOperationService().AddLogOperation(logs);
                #endregion
                return "操作失败:" + resp.SuccessMessage;
            }
        }
        /// <summary>
        /// 批量完成操作
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        [HttpPost]
        public string PLComplet(string IDs, string type)
        {
            Response<bool> resp = new AdjustmentManagementService().PLComplet(IDs, type);
            if (resp.IsSuccess)
            {
                #region 操作日志
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "库存变更管理";
                operation.Operation = "库存变更-完成";
                operation.OrderType = type;
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = IDs.ToString();
                operation.Remark = "操作成功";
                logs.Add(operation);
                new LogOperationService().AddLogOperation(logs);
                #endregion
                return "操作成功";
            }
            else
            {
                #region 操作日志
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "库存变更管理";
                operation.Operation = "库存变更-完成";
                operation.OrderType = type;
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = IDs.ToString();
                operation.Remark = "操作失败:" + resp.SuccessMessage;
                logs.Add(operation);
                new LogOperationService().AddLogOperation(logs);
                #endregion
                return "操作失败:" + resp.SuccessMessage;
            }
        }
        /// <summary>
        /// 批量取消
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        [HttpPost]
        public string Cancels(string IDs)
        {
            //ApplicationConfigHelper.RefreshASNInfo();
            if (new AdjustmentManagementService().Cancels(IDs))
            {
                return Json(new { Message = "取消成功", IsSuccess = true }).ToString();
            }
            else
            {
                return Json(new { Message = "取消失败！", IsSuccess = false }).ToString();
            }
        }
        //阿克苏库存拆箱
        [HttpGet]
        public ActionResult Unboxing_akzo(string IDS)
        {
            IndexViewModel vm = new IndexViewModel();
            InventoryManagementService s = new InventoryManagementService();
            var response = s.GetInventoryByIDS(IDS.Substring(0, IDS.Length - 1));
            ViewBag.IDS = IDS.Substring(0, IDS.Length - 1);
            if (response.IsSuccess)
            {
                vm.InventoryCollection = response.Result.InventoryCollection;
            }
            return View(vm);
        }
        [HttpPost]
        public string Unboxing_akzo(string IDS, float UnboxingQty, string ToSKUJson)
        {
            var responseJson = jsonlist<PreOrderDetail>(ToSKUJson);
            responseJson.Each((i, r) =>
            {
                r.LineNumber = returnlinenumber(i + 1);
            });
            InventoryManagementService s = new InventoryManagementService();
            var response = s.Unboxing_akzo(IDS.Substring(0, IDS.Length - 1), UnboxingQty, base.UserInfo.Name, responseJson);
            return response;
        }
        private string returnlinenumber(int row_count)
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
        // 0查看 1新增 2编辑
        [HttpGet]
        public ActionResult AddorEditorViewAdjust(int ID, long? customerID, int ViewType = 0, string AdjustType = null, string Adjustlocation = null,
            string AdjustSku = null, string AdjustUPC = null, string Adjustflag = null, string AdjustBatchNumber = null, string AdjustBoxNumber = null,
            string WarehouseName = null, string AdjustUnit = null, string AdjustSpecifications = null
            , string GoodsName = null, string Qty = null, string GoodsType = null)
        {
            IndexViewModel vm = new IndexViewModel();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.AdjustmentCondition = new AdjustmentSearchCondition();

            //IEnumerable<WMSConfig> wmsCompany;
            //if (customerID == 103)
            //{
            //    wmsCompany = new ConfigService().GetWMSCustomerConfig_Return(customerID).Result;
            //}
            //else
            //{
            //    wmsCompany = new ConfigService().GetWMSCustomerConfig().Result;
            //}
            //List<SelectListItem> stCompanty = new List<SelectListItem>();
            //foreach (WMSConfig w in wmsCompany)
            //{
            //    stCompanty.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            //}
            //vm.CompanyCodeList = stCompanty;
            vm.CompanyCodeList = null;

            if (customerID != null && customerID != 0)
            {
                var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = WarehouseList;
                //ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == customerID)
                //                  .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                //ViewBag.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
                //                    .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            }
            else
            {
                //ViewBag.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
                //                     .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            vm.AdjustmentCondition.AdjustmentTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getAdjustmentByConditionResponse = new AdjustmentManagementService().GetAdjustmentInfos(new GetAdjustmentByConditionRequest() { ID = ID });
            if (getAdjustmentByConditionResponse.IsSuccess)
            {
                vm = new IndexViewModel()
                {
                    AdjustmentAndAdjustmentDetails = getAdjustmentByConditionResponse.Result
                };
            }
            if (ViewType == 1)
            {
                vm.AdjustmentAndAdjustmentDetails = new AdjustmentAndAdjustmentDetail();
                vm.AdjustmentAndAdjustmentDetails.adjustment = new Adjustment();
                vm.AdjustmentAndAdjustmentDetails.adjustment.Warehouse = WarehouseName;
            }
            if (ViewType == 2)
            {
                var WarehouseID = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct().Where(n => n.WarehouseName == vm.AdjustmentAndAdjustmentDetails.adjustment.Warehouse)
                                   .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString() }).FirstOrDefault().Value;
                var UnitAndSpecificationsList = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(base.UserInfo.ProjectID, vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID, WarehouseID.ObjectToInt64());
                var UnitList = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Unit }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
                var SpecificationsList = UnitAndSpecificationsList.Select(t => new { Code = t.Specifications, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });

                ViewBag.skulist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.SKU + "|" + m.ToGoodsType, Text = m.SKU + "|" + m.ToGoodsType });
                ViewBag.upclist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.UPC, Text = m.UPC });
                ViewBag.batchnumberlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.BatchNumber, Text = m.BatchNumber });
                ViewBag.boxnumberlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.BoxNumber, Text = m.BoxNumber });
                //ViewBag.Unitlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Unit });
                //ViewBag.Specificationslist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Specifications });
                ViewBag.togoodstypelist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.ToGoodsType, Text = m.ToGoodsType });
                ViewBag.Unitlist = UnitList;
                //vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Unit });
                ViewBag.Specificationslist = SpecificationsList;
            }
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            if (customerID != null)
            {
                if (base.UserInfo.UserType == 0)
                {
                    vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = customerID;
                    }
                    else
                    {
                        var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                        if (customerIDs != null && customerIDs.Count() == 1)
                        {
                            vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = customerIDs.First();
                        }
                    }
                }
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
            vm.GoodsTypes = wms.Select(c => new SelectListItem() { Value = c.Name, Text = c.Name });

            ViewData["Adjusttype"] = AdjustType;
            ViewData["Adjustlocation"] = Adjustlocation;
            ViewData["AdjustSku"] = AdjustSku;
            ViewData["AdjustUPC"] = AdjustUPC;

            ViewData["Adjustflag"] = Adjustflag;
            ViewData["AdjustBatchNumber"] = AdjustBatchNumber;
            ViewData["AdjustBoxNumber"] = AdjustBoxNumber;
            ViewData["AdjustUnit"] = AdjustUnit;
            ViewData["AdjustSpecifications"] = AdjustSpecifications;
            ViewData["GoodsName"] = GoodsName;
            ViewData["Qty"] = Qty;
            ViewData["GoodsType"] = GoodsType;
            vm.ViewType = ViewType;
            vm.AdjustmentAndAdjustmentDetails.adjustment.AdjustmentTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            return View(vm);
        }

        //阿克苏专用
        [HttpGet]
        public ActionResult AddorEditorViewAdjust_akzo(int ID, long? customerID, int ViewType = 0, string AdjustType = null, string Adjustlocation = null,
            string AdjustSku = null, string AdjustUPC = null, string Adjustflag = null, string AdjustBatchNumber = null, string AdjustBoxNumber = null,
            string WarehouseName = null, string AdjustUnit = null, string AdjustSpecifications = null
            , string GoodsName = null, string Qty = null, string GoodsType = null)
        {
            IndexViewModel vm = new IndexViewModel();

            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.AdjustmentCondition = new AdjustmentSearchCondition();
            if (customerID != null && customerID != 0)
            {


                var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

                ViewBag.WarehouseList = WarehouseList;
                //ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == customerID)
                //                  .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                //ViewBag.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
                //                    .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            }
            else
            {
                //ViewBag.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
                //                     .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

            }


            vm.AdjustmentCondition.AdjustmentTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getAdjustmentByConditionResponse = new AdjustmentManagementService().GetAdjustmentInfos(new GetAdjustmentByConditionRequest() { ID = ID });


            if (getAdjustmentByConditionResponse.IsSuccess)
            {
                vm = new IndexViewModel()
                {
                    AdjustmentAndAdjustmentDetails = getAdjustmentByConditionResponse.Result
                };
            }
            if (ViewType == 1)
            {
                vm.AdjustmentAndAdjustmentDetails = new AdjustmentAndAdjustmentDetail();
                vm.AdjustmentAndAdjustmentDetails.adjustment = new Adjustment();
                vm.AdjustmentAndAdjustmentDetails.adjustment.Warehouse = WarehouseName;
            }
            if (ViewType == 2)
            {
                var WarehouseID = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct().Where(n => n.WarehouseName == vm.AdjustmentAndAdjustmentDetails.adjustment.Warehouse)
                                   .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString() }).FirstOrDefault().Value;
                var UnitAndSpecificationsList = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(base.UserInfo.ProjectID, vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID, WarehouseID.ObjectToInt64());
                var UnitList = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Unit }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
                var SpecificationsList = UnitAndSpecificationsList.Select(t => new { Code = t.Specifications, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });

                ViewBag.skulist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.SKU + "|" + m.ToGoodsType, Text = m.SKU + "|" + m.ToGoodsType });
                ViewBag.upclist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.UPC, Text = m.UPC });
                ViewBag.batchnumberlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.BatchNumber, Text = m.BatchNumber });
                ViewBag.boxnumberlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.BoxNumber, Text = m.BoxNumber });
                //ViewBag.Unitlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Unit });
                //ViewBag.Specificationslist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Specifications });
                ViewBag.togoodstypelist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.ToGoodsType, Text = m.ToGoodsType });
                ViewBag.Unitlist = UnitList;
                //vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Unit });
                ViewBag.Specificationslist = SpecificationsList;
            }
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            if (customerID != null)
            {
                if (base.UserInfo.UserType == 0)
                {
                    vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = customerID;
                    }
                    else
                    {
                        var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                        if (customerIDs != null && customerIDs.Count() == 1)
                        {
                            vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = customerIDs.First();
                        }
                    }
                }
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
            vm.GoodsTypes = wms.Select(c => new SelectListItem() { Value = c.Name, Text = c.Name });
            ViewData["Adjusttype"] = AdjustType;
            ViewData["Adjustlocation"] = Adjustlocation;
            ViewData["AdjustSku"] = AdjustSku;
            ViewData["AdjustUPC"] = AdjustUPC;

            ViewData["Adjustflag"] = Adjustflag;
            ViewData["AdjustBatchNumber"] = AdjustBatchNumber;
            ViewData["AdjustBoxNumber"] = AdjustBoxNumber;
            ViewData["AdjustUnit"] = AdjustUnit;
            ViewData["AdjustSpecifications"] = AdjustSpecifications;
            ViewData["GoodsName"] = GoodsName;
            ViewData["Qty"] = Qty;
            ViewData["GoodsType"] = GoodsType;
            vm.ViewType = ViewType;
            vm.AdjustmentAndAdjustmentDetails.adjustment.AdjustmentTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            return View(vm);
        }
        //自动检索库位
        [HttpPost]
        public ActionResult GetLocationList(string location, string warehouseid, int areaid)
        {
            string WarehouseName = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(a => a.WarehouseID.ToString() == warehouseid).Select(b => b.WarehouseName).FirstOrDefault();
            if (areaid == 0)
            {
                var inventory = ApplicationConfigHelper.GetWarehouseLocationListByWarehouseName(WarehouseName).Where(a => a.Location.Contains(location) && a.WarehouseID == warehouseid.ObjectToInt64());
                return Json(inventory.Where(s => s.Location.IndexOf(location, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID, Text = t.AreaName + "|" + t.Location.Trim() }).Distinct(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var inventory = ApplicationConfigHelper.GetWarehouseLocationListByWarehouseName(WarehouseName).Where(a => a.Location.Contains(location) && a.WarehouseID == warehouseid.ObjectToInt64() && a.AreaID == areaid);
                return Json(inventory.Where(s => s.Location.IndexOf(location, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID, Text = t.Location }), JsonRequestBehavior.AllowGet);
            }
        }
        //自动检索库位  回车
        public ActionResult GetLocationListAkzo(string location, string warehouseid, int areaid)
        {
            WarehouseService service = new WarehouseService();
            var list = service.GetWarehouseLocationListByLocation(warehouseid, location).Result;
            if (list == null || !list.Any())
            {
                new List<LocationInfo>();
            }
            return Json(list.Where(s => s.Location.IndexOf(location, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID, Text = t.AreaName + "|" + t.Location.Trim() }).Distinct(), JsonRequestBehavior.AllowGet);
        }
        //自动检索sku
        [HttpPost]
        public ActionResult GetALLSKU(string sku, string CustomerID)
        {
            var skulist = ApplicationConfigHelper.GetALLProductStorerList(CustomerID).Where(a => a.SKU.Contains(sku));
            return Json(skulist.Select(t => new { Value = t.SKU, Text = t.SKU }), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //根据库位获取sku 数量 货品等级 货品描述等
        public ActionResult GetInventoryskuList(string CustomerID, string location, string sku, string upc, string goodstype, string batchnumber, string boxnumber, string warehouse, string Unit, string Specifications, string StoreCode)
        {
            //sku = sku;
            goodstype = goodstype == null ? "" : goodstype;
            location = location == null ? "" : location.Trim();
            batchnumber = batchnumber == null ? "" : batchnumber;
            boxnumber = boxnumber == null ? "" : boxnumber;
            Unit = Unit == null ? "" : Unit;
            Specifications = Specifications == null ? "" : Specifications;
            AdjustmentManagementService service = new AdjustmentManagementService();
            var inventory = service.GetInventoryLocationList(location.Substring(location.IndexOf('|') + 1), warehouse, CustomerID, StoreCode).Result;
            //if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && (string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(inventory.Where(c => c.BoxNumber.Trim() == boxnumber.Trim() && c.SKU.Trim() == sku.Trim() && c.BatchNumber.Trim() == batchnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}

            //var aaaaa = inventory.Where(
            //        w => (w.SKU == sku || sku == "")
            //            && (w.UPC == upc || upc == "")
            //             && (w.GoodsType == goodstype || goodstype == "")
            //                 && (w.BatchNumber == batchnumber || batchnumber == "")
            //                     && (w.BoxNumber == boxnumber || boxnumber == "")
            //                       && (w.Warehouse == warehouse || warehouse == "")
            //                         && (w.Unit == Unit || Unit == "")
            //                           && (w.Specifications == Specifications || Specifications == "")
            //    ).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, UPC = t.UPC, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct();
            return Json(inventory.Where(
                    w => (w.SKU == sku || sku == "")
                        && (w.UPC == upc || upc == "")
                         && (w.GoodsType == goodstype || goodstype == "")
                             && (w.BatchNumber == batchnumber || batchnumber == "")
                                 && (w.BoxNumber == boxnumber || boxnumber == "")
                                   && (w.Warehouse == warehouse || warehouse == "")
                                     && (w.Unit == Unit || Unit == "")
                                       && (w.Specifications == Specifications || Specifications == "")
                ).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, UPC = t.UPC, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.SKU == sku.Trim() && c.GoodsType.Trim() == goodstype.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.SKU == sku.Trim() && c.BatchNumber == batchnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.SKU == sku.Trim() && c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim() && c.SKU.Trim() == sku.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.SKU == sku.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim() && c.SKU == sku.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}



            //if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.Unit.Trim() == Unit.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Unit.Trim() == Unit.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Unit.Trim() == Unit.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.SKU == sku.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Unit.Trim() == Unit.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.SKU == sku.Trim() && c.BatchNumber == batchnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Unit.Trim() == Unit.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.SKU == sku.Trim() && c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Unit.Trim() == Unit.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Unit.Trim() == Unit.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim() && c.SKU.Trim() == sku.Trim() && c.Unit.Trim() == Unit.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim() && c.Unit.Trim() == Unit.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim() && c.Unit.Trim() == Unit.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.Unit.Trim() == Unit.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.SKU == sku.Trim() && c.Unit.Trim() == Unit.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim() && c.SKU == sku.Trim() && c.Unit.Trim() == Unit.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim() && c.SKU == sku.Trim() && c.BatchNumber == batchnumber.Trim() && c.GoodsType == goodstype.Trim() && c.Unit.Trim() == Unit.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}


            //if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.SKU == sku.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.SKU == sku.Trim() && c.BatchNumber == batchnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.SKU == sku.Trim() && c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim() && c.SKU.Trim() == sku.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.SKU == sku.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim() && c.SKU == sku.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}



            //if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.SKU == sku.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.SKU == sku.Trim() && c.BatchNumber == batchnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.SKU == sku.Trim() && c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim() && c.SKU.Trim() == sku.Trim() && c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim() && c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim() && c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.SKU == sku.Trim() && c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}
            //else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim() && c.SKU == sku.Trim() && c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}


            //else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype) && (!string.IsNullOrEmpty(Unit) && !string.IsNullOrEmpty(Specifications)))
            //{
            //    return Json(inventory.Where(c => c.BoxNumber.Trim() == boxnumber.Trim() && c.SKU.Trim() == sku.Trim() && c.BatchNumber.Trim() == batchnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim() && c.Unit.Trim() == Unit.Trim() && c.Specifications.Trim() == Specifications.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
            //}

            //else
            //{
            //    return Json(inventory.Where(s => s.SKU.Trim() == sku.Trim() && s.GoodsType.Trim() == goodstype.Trim() && s.BatchNumber == batchnumber.Trim() && s.BoxNumber == boxnumber.Trim()).Select(t => new { GoodsType = t.GoodsType, GoodsName = t.GoodsName, qty = t.Qty, SKU = t.SKU, BatchNumber = t.BatchNumber, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }), JsonRequestBehavior.AllowGet);
            //}
        }
        //public ActionResult GetInventoryskuListByChange(string location, string sku, string goodstype, string batchnumber, string boxnumber, string warehouse, string Specifications)
        //{
        //    sku = sku == "请选择" ? "" : sku;
        //    goodstype = goodstype == "请选择" ? "" : goodstype;
        //    batchnumber = batchnumber == "请选择" ? "" : batchnumber;
        //    boxnumber = boxnumber == "请选择" ? "" : boxnumber;
        //    Specifications = Specifications == "请选择" ? "" : Specifications;
        //    AdjustmentManagementService service = new AdjustmentManagementService();
        //    var inventory = service.GetInventoryLocationList(location.Substring(location.IndexOf('|') + 1), warehouse, customerid).Result;

        //    if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype))
        //    {
        //        return Json(inventory.GroupBy(c => new { c.Location }).Select(t => new { qty = t.Sum(m => m.Qty) }).Distinct(), JsonRequestBehavior.AllowGet);
        //    }
        //    else if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype))
        //    {
        //        return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim()).GroupBy(c => new { c.BoxNumber, c.GoodsType }).Select(t => new { qty = t.Sum(m => m.Qty) }).Distinct(), JsonRequestBehavior.AllowGet);
        //    }
        //    else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype))
        //    {
        //        return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim()).GroupBy(c => new { c.BatchNumber, c.GoodsType }).Select(t => new { qty = t.Sum(m => m.Qty) }).Distinct(), JsonRequestBehavior.AllowGet);
        //    }
        //    else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype))
        //    {
        //        return Json(inventory.Where(c => c.SKU == sku.Trim() && c.GoodsType.Trim() == goodstype.Trim()).GroupBy(c => new { c.SKU, c.GoodsName, c.GoodsType }).Select(t => new { qty = t.Sum(m => m.Qty), SKU = t.Key.SKU, GoodsName = t.Key.GoodsName, GoodsType = t.Key.GoodsType }).Distinct(), JsonRequestBehavior.AllowGet);
        //    }
        //    else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype))
        //    {
        //        return Json(inventory.Where(c => c.SKU == sku.Trim() && c.BatchNumber == batchnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim()).GroupBy(c => new { c.SKU, c.GoodsName, c.BatchNumber, c.GoodsType }).Select(t => new { qty = t.Sum(m => m.Qty), SKU = t.Key.SKU, BatchNumber = t.Key.BatchNumber, GoodsName = t.Key.GoodsName, GoodsType = t.Key.GoodsType }).Distinct(), JsonRequestBehavior.AllowGet);
        //    }
        //    else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype))
        //    {
        //        return Json(inventory.Where(c => c.SKU == sku.Trim() && c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim()).GroupBy(c => new { c.SKU, c.GoodsName, c.BoxNumber, c.GoodsType }).Select(t => new { qty = t.Sum(m => m.Qty), SKU = t.Key.SKU, BoxNumber = t.Key.BoxNumber, GoodsName = t.Key.GoodsName, GoodsType = t.Key.GoodsType }).Distinct(), JsonRequestBehavior.AllowGet);
        //    }
        //    else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && !string.IsNullOrEmpty(goodstype))
        //    {
        //        return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim() && c.GoodsType.Trim() == goodstype.Trim()).GroupBy(c => new { c.BatchNumber, c.BoxNumber, c.GoodsType }).Select(t => new { qty = t.Sum(m => m.Qty), BatchNumber = t.Key.BatchNumber, GoodsType = t.Key.GoodsType }).Distinct(), JsonRequestBehavior.AllowGet);
        //    }
        //    else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype))
        //    {
        //        return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim() && c.SKU.Trim() == sku.Trim()).GroupBy(c => new { c.SKU, c.GoodsName, c.BatchNumber, c.BoxNumber }).Select(t => new { qty = t.Sum(m => m.Qty), SKU = t.Key.SKU, BoxNumber = t.Key.BoxNumber, BatchNumber = t.Key.BatchNumber, GoodsName = t.Key.GoodsName }).Distinct(), JsonRequestBehavior.AllowGet);
        //    }
        //    else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype))
        //    {
        //        return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.BoxNumber == boxnumber.Trim()).GroupBy(c => new { c.BatchNumber, c.BoxNumber }).Select(t => new { qty = t.Sum(m => m.Qty), BoxNumber = t.Key.BoxNumber, BatchNumber = t.Key.BatchNumber }).Distinct(), JsonRequestBehavior.AllowGet);
        //    }
        //    else if (string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype))
        //    {
        //        return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim()).GroupBy(c => new { c.BoxNumber }).Select(t => new { qty = t.Sum(m => m.Qty), BoxNumber = t.Key.BoxNumber }).Distinct(), JsonRequestBehavior.AllowGet);
        //    }
        //    else if (string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype))
        //    {
        //        return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim()).GroupBy(c => new { c.BatchNumber }).Select(t => new { qty = t.Sum(m => m.Qty), BatchNumber = t.Key.BatchNumber }).Distinct(), JsonRequestBehavior.AllowGet);
        //    }
        //    else if (!string.IsNullOrEmpty(sku) && !string.IsNullOrEmpty(batchnumber) && string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype))
        //    {
        //        return Json(inventory.Where(c => c.BatchNumber == batchnumber.Trim() && c.SKU == sku.Trim()).GroupBy(c => new { c.SKU, c.GoodsName, c.BatchNumber }).Select(t => new { qty = t.Sum(m => m.Qty), SKU = t.Key.SKU, GoodsName = t.Key.GoodsName, BatchNumber = t.Key.BatchNumber }).Distinct(), JsonRequestBehavior.AllowGet);
        //    }
        //    else if (!string.IsNullOrEmpty(sku) && string.IsNullOrEmpty(batchnumber) && !string.IsNullOrEmpty(boxnumber) && string.IsNullOrEmpty(goodstype))
        //    {
        //        return Json(inventory.Where(c => c.BoxNumber == boxnumber.Trim() && c.SKU == sku.Trim()).GroupBy(c => new { c.SKU, c.GoodsName, c.BoxNumber }).Select(t => new { qty = t.Sum(m => m.Qty), SKU = t.Key.SKU, GoodsName = t.Key.GoodsName, BoxNumber = t.Key.BoxNumber }).Distinct(), JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(inventory.Where(s => s.SKU.Trim() == sku.Trim() && s.GoodsType.Trim() == goodstype.Trim() && s.BatchNumber == batchnumber.Trim() && s.BoxNumber == boxnumber.Trim()).GroupBy(c => new { c.SKU, c.GoodsName, c.BatchNumber, c.BoxNumber, c.GoodsType }).Select(t => new { qty = t.Sum(m => m.Qty), SKU = t.Key.SKU, GoodsName = t.Key.GoodsName, BoxNumber = t.Key.BoxNumber, BatchNumber = t.Key.BatchNumber, GoodsType = t.Key.GoodsType }), JsonRequestBehavior.AllowGet);
        //    }
        //}
        public static List<T> jsonlist<T>(string str)
        {
            return JsonConvert.DeserializeObject<List<T>>(str);
        }

        public ActionResult InventoryWarning()
        {
            IndexViewModel vm = new IndexViewModel();
            //var inventory = service.GetInventoryLocationList(location.Substring(location.IndexOf('|') + 1), warehouse, CustomerID).Result;
            //return new AdjustmentManagementService().CheckAdjustData(request);
            var respone = new InventoryManagementService().GetInventoryWarning();
            if (respone.IsSuccess)
            {
                vm.InventoryCollection = respone.Result.InventoryCollection;
            }

            return View(vm);
        }

        /// <summary>
        /// 验证保存的调整数据是否合法
        /// </summary>
        /// <param name="AdjustmentDetails"></param>
        /// <returns></returns>
        public string CheckAdjustData(AddAdjustmentandAdjustmentDetailRequest request)
        {
            //var inventory = service.GetInventoryLocationList(location.Substring(location.IndexOf('|') + 1), warehouse, CustomerID).Result;
            return new AdjustmentManagementService().CheckAdjustData(request);
        }
        /// <summary>
        /// 暂存
        /// </summary>
        /// <param name="JsonTable"></param>
        /// <param name="ASNNumber"></param>
        /// <param name="ASNID"></param>
        /// <param name="ExternadjustmentNumber"></param>
        /// <param name="CustomerName"></param>
        /// <param name="CustomerID"></param>
        /// <param name="adjustmenttype"></param>
        /// <param name="adjustmentDate"></param>
        /// <param name="JsonField"></param>
        /// <param name="WarehouseID"></param>
        /// <param name="WarehouseName"></param>
        /// <returns></returns>
        [HttpPost]
        public string AddAdjustAndAdjustDetail(string JsonTable, long CustomerID, string CustomerName, string AdjustmentType, DateTime adjustmenttime, string AdjustmentReason, string JsonField, long WarehouseID, String WarehouseName, string AdID, string AdjustmentRemark, string StoreCode)
        {
            var responseJsonFieldsets = jsonlist<Adjustment>(JsonField);
            AddAdjustmentandAdjustmentDetailRequest request = new AddAdjustmentandAdjustmentDetailRequest();
            IList<AdjustmentDetail> AdjustmentDetails = jsonlist<AdjustmentDetail>(JsonTable);
            #region akzo项目 调整单品级不能相同 移动单库位不能相同
            if (base.UserInfo.ProjectID.ToString() == "15")
            {
                foreach (var item in AdjustmentDetails)
                {
                    if (AdjustmentType == "库存数量调整单")
                    {
                        if (item.FromGoodsType != item.ToGoodsType)
                        {
                            return "库存调整单品级不能不相同！";
                        }
                        if (item.FromLocation != item.ToLocation)
                        {
                            return "库存调整单库位不能不相同！";
                        }
                    }
                    else if (AdjustmentType == "库存移动单")
                    {
                        if (item.FromLocation == item.ToLocation)
                        {
                            return "库存移动单库位不能相同";
                        }
                        if (item.FromGoodsType != item.ToGoodsType)
                        {
                            return "库存移动单品级不能不相同";
                        }
                    }
                    else if (AdjustmentType == "库存品级调整单")
                    {
                        if (item.FromLocation != item.ToLocation)
                        {
                            return "库存品级调整单库位不能不相同";
                        }
                        if (item.FromGoodsType == item.ToGoodsType)
                        {
                            return "库存品级调整单品级不能相同";
                        }
                    }
                }
            }
            #endregion

            responseJsonFieldsets.Each((i, adjust) =>
            {
                adjust.CustomerID = CustomerID;
                adjust.CustomerName = CustomerName;
                adjust.Warehouse = WarehouseName;
                adjust.AdjustmentType = AdjustmentType;
                adjust.AdjustmentReason = AdjustmentReason;
                adjust.AdjustmentTime = DateTime.Parse(adjustmenttime.DateTimeToString());
                adjust.CreateTime = DateTime.Now;
                adjust.Creator = base.UserInfo.Name.ToString();
                adjust.IsHold = AdjustmentType == "库存冻结单" ? 1 : 0;
                adjust.Status = 1;
                adjust.Remark = AdjustmentRemark;
                adjust.str3 = StoreCode;
            });
            string datetime = DateTime.Now.ToString("yyyyMMddHHmmss");
            string str = "";
            try
            {
                AdjustmentDetails.Each((i, adjustDetail) =>
                {
                    str = datetime + ReturnLineNumber(i + 1);
                    adjustDetail.CustomerID = CustomerID;
                    adjustDetail.CustomerName = CustomerName;
                    adjustDetail.FromLot = adjustDetail.FromLot;
                    adjustDetail.ToLot = adjustDetail.ToLot;
                    adjustDetail.FromWarehouse = WarehouseName;
                    adjustDetail.ToWarehouse = WarehouseName;
                    adjustDetail.FromArea = adjustDetail.FromLocation.Substring(0, adjustDetail.FromLocation.IndexOf('|'));
                    adjustDetail.ToArea = adjustDetail.ToLocation.Substring(0, adjustDetail.ToLocation.IndexOf('|'));
                    adjustDetail.FromQty = adjustDetail.FromQty;
                    adjustDetail.ToQty = adjustDetail.ToQty;
                    adjustDetail.FromLocation = adjustDetail.FromLocation.Substring(adjustDetail.FromLocation.IndexOf('|') + 1);
                    adjustDetail.ToLocation = adjustDetail.ToLocation.Substring(adjustDetail.ToLocation.IndexOf('|') + 1);
                    adjustDetail.FromLot = adjustDetail.FromLot;
                    adjustDetail.FromGoodsType = adjustDetail.FromGoodsType;
                    adjustDetail.ToGoodsType = adjustDetail.ToGoodsType;
                    adjustDetail.GoodsName = adjustDetail.GoodsName;
                    adjustDetail.IsHold = AdjustmentType == "库存冻结单" ? 1 : 0;
                    adjustDetail.AdjustmentReason = adjustDetail.AdjustmentReason;
                    adjustDetail.SKU = adjustDetail.SKU.Trim().Substring(0, adjustDetail.SKU.IndexOf('|'));
                    adjustDetail.UPC = adjustDetail.UPC.Trim();
                    adjustDetail.CreateTime = DateTime.Now;
                    adjustDetail.Creator = base.UserInfo.Name.ToString();
                    adjustDetail.BatchNumber = adjustDetail.BatchNumber == "请选择" ? null : adjustDetail.BatchNumber;
                    adjustDetail.BoxNumber = adjustDetail.BoxNumber == "请选择" ? null : adjustDetail.BoxNumber;
                    adjustDetail.BoxNumberNew = str;
                });
            }
            catch (Exception)
            {

                throw;
            }
            request.adjustment = responseJsonFieldsets;
            request.adjustmentDetails = AdjustmentDetails;
            request.AdID = AdID == null ? "0" : AdID;

            string resString = CheckAdjustData(request);
            var response = new Response<string>();


            if (resString != "")
            {
                response.IsSuccess = false;
                response.Result = resString;
                return response.Result;
            }
            response = new AdjustmentManagementService().AddAdjustmentANDAdjustmentDetail(request);
            if (response.IsSuccess)
            {
                #region 操作日志
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "库存变更管理";
                operation.Operation = AdjustmentType + "-新增";
                operation.OrderType = "InventoryChange";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = response.Result.Replace("添加成功", "");
                logs.Add(operation);
                new LogOperationService().AddLogOperation(logs);
                #endregion
            }
            return response.Result;
        }
        [HttpPost]
        //更新暂存信息
        public string UpdateAdjustAndAdjustDetail(string JsonTable, long CustomerID, string CustomerName, string AdjustmentType, DateTime adjustmenttime, string AdjustmentReason, string JsonField, long WarehouseID, String WarehouseName, int ID, string adjustmentnumber, string AdjustmentRemark, string StoreCode)
        {
            var responseJsonFieldsets = jsonlist<Adjustment>(JsonField);
            AddAdjustmentandAdjustmentDetailRequest request = new AddAdjustmentandAdjustmentDetailRequest();
            IList<AdjustmentDetail> AdjustmentDetails = jsonlist<AdjustmentDetail>(JsonTable);


            responseJsonFieldsets.Each((i, adjust) =>
            {
                adjust.ID = ID;
                adjust.CustomerID = CustomerID;
                adjust.CustomerName = CustomerName;
                adjust.Warehouse = WarehouseName;
                adjust.AdjustmentType = AdjustmentType;
                adjust.AdjustmentReason = AdjustmentReason;
                adjust.AdjustmentTime = adjustmenttime;
                adjust.CreateTime = DateTime.Now;
                adjust.UpdateTime = DateTime.Now;
                adjust.Updator = base.UserInfo.Name.ToString();
                adjust.Creator = base.UserInfo.Name.ToString();
                adjust.Status = 1;
                adjust.Remark = AdjustmentRemark;
                adjust.str3 = StoreCode;
            });

            AdjustmentDetails.Each((i, adjustDetail) =>
            {
                adjustDetail.AID = ID;
                adjustDetail.AdjustmentNumber = adjustmentnumber;
                adjustDetail.CustomerID = CustomerID;
                adjustDetail.CustomerName = CustomerName;
                adjustDetail.FromLot = adjustDetail.FromLot;
                adjustDetail.ToLot = adjustDetail.ToLot;
                adjustDetail.FromWarehouse = WarehouseName;
                adjustDetail.ToWarehouse = WarehouseName;
                adjustDetail.FromArea = adjustDetail.FromLocation.Substring(0, adjustDetail.FromLocation.IndexOf('|'));
                adjustDetail.ToArea = adjustDetail.ToLocation.Substring(0, adjustDetail.ToLocation.IndexOf('|'));
                adjustDetail.FromQty = adjustDetail.FromQty;
                adjustDetail.ToQty = adjustDetail.ToQty;
                //adjustDetail.ToQty = adjustDetail.ToQty;
                adjustDetail.FromLocation = adjustDetail.FromLocation.Substring(adjustDetail.FromLocation.IndexOf('|') + 1);
                adjustDetail.ToLocation = adjustDetail.ToLocation.Substring(adjustDetail.ToLocation.IndexOf('|') + 1);
                adjustDetail.FromLot = adjustDetail.FromLot;
                adjustDetail.FromGoodsType = adjustDetail.FromGoodsType;
                adjustDetail.ToGoodsType = adjustDetail.ToGoodsType;
                adjustDetail.GoodsName = adjustDetail.GoodsName;
                adjustDetail.IsHold = AdjustmentType == "库存冻结单" ? 1 : 0;
                adjustDetail.AdjustmentReason = adjustDetail.AdjustmentReason;
                adjustDetail.SKU = adjustDetail.SKU.Trim().Substring(0, adjustDetail.SKU.IndexOf('|'));
                adjustDetail.UpdateTime = DateTime.Now;
                adjustDetail.Updator = base.UserInfo.Name.ToString();
                adjustDetail.CreateTime = DateTime.Now;
                adjustDetail.Creator = base.UserInfo.Name.ToString();
                adjustDetail.SKU = adjustDetail.SKU == "请选择" ? null : adjustDetail.SKU;
                adjustDetail.BatchNumber = adjustDetail.BatchNumber == "请选择" ? null : adjustDetail.BatchNumber;
                adjustDetail.BoxNumber = adjustDetail.BoxNumber == "请选择" ? null : adjustDetail.BoxNumber;

            });
            request.adjustment = responseJsonFieldsets;
            request.adjustmentDetails = AdjustmentDetails;
            string resString = CheckAdjustData(request);
            var response = new Response<string>();

            if (resString != "")
            {
                response.IsSuccess = false;
                response.Result = resString;
                return response.Result;
            }
            response = new AdjustmentManagementService().UpdateAdjustmentANDAdjustmentDetail(request);
            if (response.IsSuccess)
            {
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "库存变更管理";
                operation.Operation = AdjustmentType + "-修改";
                operation.OrderType = "InventoryChange";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = ID.ToString();
                logs.Add(operation);
                new LogOperationService().AddLogOperation(logs);
            }
            return response.Result;
        }
        /// <summary>
        /// 提交操作
        /// </summary>
        /// <param name="JsonTable"></param>
        /// <param name="CustomerName"></param>
        /// <param name="CustomerID"></param>
        /// <param name="warehouse"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateAndInsertInventory(string JsonTable, string CustomerName, string warehouse, string Inventorytype, int aid)
        {
            AddAdjustmentandAdjustmentDetailRequest request = new AddAdjustmentandAdjustmentDetailRequest();

            IList<AdjustmentDetail> adjustmentdetail = jsonlist<AdjustmentDetail>(JsonTable);

            adjustmentdetail.Each((i, adjustmentdetails) =>
            {
                adjustmentdetails.AID = aid;
                adjustmentdetails.CustomerName = CustomerName;
                adjustmentdetails.ToWarehouse = warehouse;
                adjustmentdetails.ToArea = adjustmentdetails.ToLocation.Substring(0, adjustmentdetails.ToLocation.IndexOf('|'));
                adjustmentdetails.SKU = adjustmentdetails.SKU.Substring(0, adjustmentdetails.SKU.IndexOf('|'));
                adjustmentdetails.ToQty = adjustmentdetails.ToQty;
                //adjustmentdetails.ToQty = adjustmentdetails.ToQty;
                adjustmentdetails.FromWarehouse = warehouse;
                adjustmentdetails.FromArea = adjustmentdetails.FromLocation.Substring(0, adjustmentdetails.FromLocation.IndexOf('|'));
                adjustmentdetails.FromLocation = adjustmentdetails.FromLocation.Substring(adjustmentdetails.FromLocation.IndexOf('|') + 1);
                adjustmentdetails.ToLocation = adjustmentdetails.ToLocation.Substring(adjustmentdetails.ToLocation.IndexOf('|') + 1);
                adjustmentdetails.FromGoodsType = adjustmentdetails.FromGoodsType;
                adjustmentdetails.ToGoodsType = adjustmentdetails.ToGoodsType;
                adjustmentdetails.CreateTime = DateTime.Now;
                adjustmentdetails.Creator = base.UserInfo.Name.ToString();
                adjustmentdetails.BatchNumber = adjustmentdetails.BatchNumber == "请选择" ? null : adjustmentdetails.BatchNumber;
                adjustmentdetails.BoxNumber = adjustmentdetails.BoxNumber == "请选择" ? null : adjustmentdetails.BoxNumber;
            });
            request.adjustmentDetails = adjustmentdetail;

            var response = new AdjustmentManagementService().UpdateAndInsertInventory(request, Inventorytype);
            return response.Result;
        }
        //检查导入数据
        private string CheckImportsData(DataSet ds, long customerid)
        {
            string errormsg = "";
            ArrayList skulocation = new ArrayList();
            string storecode = ds.Tables[0].Rows[0]["门店代码"].ToString();
            string inventorytype = ds.Tables[0].Rows[0]["调整类型"].ToString();
            List<ProductSearch> ListPs = new List<ProductSearch>();
            ProductSearch ps = new ProductSearch();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ps.SKU = ds.Tables[0].Rows[i]["SKU"].ToString();
                ListPs.Add(ps);
                var resualtProList = ApplicationConfigHelper.GetSearchProduct(customerid.ToString().ObjectToInt64(), ListPs, "UPC");
                if (resualtProList.Where(c => (c.SKU == ds.Tables[0].Rows[i]["SKU"].ToString() && c.CustomerID == customerid.ToString())).Select(m => m.CustomerID).FirstOrDefault() == null)
                {
                    errormsg = "SKU： " + ds.Tables[0].Rows[i]["SKU"].ToString() + "     在系统中不存在";
                    return errormsg;
                }

                //if (ds.Tables[0].Rows[i]["门店代码"].ToString().Trim() == "")
                //{
                //    errormsg = "门店代码不可为空";
                //    return errormsg;
                //}
                if (!skulocation.Contains(ds.Tables[0].Rows[i]["SKU"].ToString() + ds.Tables[0].Rows[i]["原库位"].ToString() + ds.Tables[0].Rows[i]["新库位"].ToString()))
                {
                    skulocation.Add(ds.Tables[0].Rows[i]["SKU"].ToString() + ds.Tables[0].Rows[i]["原库位"].ToString() + ds.Tables[0].Rows[i]["新库位"].ToString());
                }
                else
                {
                    errormsg = "同一个 SKU+原库位+新库位 只能出现一次！ SKU:" + ds.Tables[0].Rows[i]["SKU"].ToString() + " 原库位:" + ds.Tables[0].Rows[i]["原库位"].ToString() + " 新库位:" + ds.Tables[0].Rows[i]["新库位"].ToString();
                    return errormsg;
                }
                if (inventorytype != ds.Tables[0].Rows[i]["调整类型"].ToString())
                {
                    errormsg = "每次只能导入一种调整类型！";
                    return errormsg;
                }
                //if (storecode != ds.Tables[0].Rows[i]["门店代码"].ToString())
                //{
                //    errormsg = "每次只能导入一家门店！";
                //    return errormsg;
                //}
                if (inventorytype == "库存冻结单")
                {
                    if (ds.Tables[0].Rows[i]["调整数量"].ToString().Trim() == "0")
                    {
                        errormsg = "冻结数量不能为0";
                        return errormsg;
                    }
                }
                if (inventorytype == "库存移动单")
                {
                    if (ds.Tables[0].Rows[i]["调整数量"].ToString().Trim() == "0")
                    {
                        errormsg = "移动数量不能为0";
                        return errormsg;
                    }
                    if (ds.Tables[0].Rows[i]["原库位"].ToString().Trim() == ds.Tables[0].Rows[i]["新库位"].ToString().Trim())
                    {
                        errormsg = "移动单原库位和新库位不能一样";
                        return errormsg;
                    }
                    if (ds.Tables[0].Rows[i]["原等级"].ToString().Trim() != ds.Tables[0].Rows[i]["调整等级"].ToString().Trim())
                    {
                        errormsg = "移动单原等级和调整等级必须一致";
                        return errormsg;
                    }
                }
                ///新增
                if (inventorytype == "库存调整单")
                {
                    if (ds.Tables[0].Rows[i]["调整备注"].ToString().Trim() == "" || ds.Tables[0].Rows[i]["调整备注"].ToString().Trim() == "选填")
                    {
                        errormsg = "调整备注不可为空";
                        return errormsg;
                    }
                }
            }
            //nike需要验证调整单的reasoncode是否填写
            #region  页面读取那个需要显示reason
            IEnumerable<WMS_Config_Type> ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            if (inventorytype == "库存调整单" && ctype != null && ctype.Any())
            {
                if (ctype.Where(m => m.Type == "CustomerID" && m.Code == "NIKENFS" && m.Name == customerid.ToString()) != null && ctype.Where(m => m.Type == "CustomerID" && m.Code == "NIKENFS" && m.Name == customerid.ToString()).Any())
                {
                    IEnumerable<WMSConfig> reasonTypeConfig = ApplicationConfigHelper.GetWMS_Config("ADJReasonType");
                    if (reasonTypeConfig != null && reasonTypeConfig.Any())
                    {
                        //验证reasoncode调整原因是否按照约定填写
                        List<string> reasonList = (from d in ds.Tables[0].AsEnumerable()
                                                   select d.Field<string>("调整原因").ToString().Trim()
                                                 ).ToList();
                        if (reasonList.Distinct().Count() > 1)
                        {
                            return errormsg = "您填写的调整原因不一致，请检查！";
                        }
                        if (reasonTypeConfig.Where(m => m.Code == reasonList[0].ToString()).Count() <= 0)
                        {
                            return errormsg = "您填写的调整原因：" + reasonList[0].ToString() + " 在系统配置中不存在，请检查！";
                        }
                    }
                }
            }
            #endregion
            if (inventorytype != "库存冻结单" && inventorytype != "库存调整单" && inventorytype != "库存移动单" && inventorytype != "库存品级调整单")
            {
                errormsg = "调整类型必须是 库存冻结单、 库存调整单、 库存移动单、库存品级调整单的一种！";
                return errormsg;
            }
            return "";
        }
        //批量导入
        public string Imports(long customerid, string customername, string warehousename)
        {
            AdjustmentManagementService service = new AdjustmentManagementService();

            IndexViewModel vm = new IndexViewModel();
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    try
                    {
                        IList<Adjustment> adjustment = new List<Adjustment>();
                        IList<AdjustmentDetail> adjustmentdetail = new List<AdjustmentDetail>();
                        //DataSet ds = this.GetDataFromExcel(hpf);
                        DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                        string res = CheckImportsData(ds, customerid);
                        if (res != "")
                        {
                            return new { result = "<h3><font color='#FF0000'>批量导入失败！" + res + "</font></h3>", IsSuccess = true }.ToJsonString();
                        }
                        string inventorytype = "";
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                inventorytype = ds.Tables[0].Rows[0]["调整类型"].ToString();
                                adjustment.Add(new Adjustment()
                                {
                                    CustomerID = customerid,
                                    CustomerName = customername,
                                    Warehouse = warehousename,
                                    AdjustmentType = inventorytype,
                                    AdjustmentReason = ds.Tables[0].Rows[0]["调整原因"].ToString().Trim(),
                                    Remark = ds.Tables[0].Rows[0]["调整备注"].ToString().Trim(),
                                    AdjustmentTime = DateTime.Now,
                                    CreateTime = DateTime.Now,
                                    Creator = base.UserInfo.Name.ToString(),
                                    IsHold = inventorytype == "库存冻结单" ? 1 : 0,
                                    Status = 1,
                                    str3 = ds.Tables[0].Rows[0]["门店代码"].ToString()
                                });
                            }
                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            {


                                adjustmentdetail.Add(new AdjustmentDetail()
                                {
                                    CustomerID = customerid,
                                    CustomerName = customername,
                                    FromWarehouse = warehousename,
                                    ToWarehouse = warehousename,
                                    FromArea = ds.Tables[0].Rows[k]["原库区"].ToString().Trim(),
                                    ToArea = ds.Tables[0].Rows[k]["新库区"].ToString().Trim(),
                                    Unit = ds.Tables[0].Rows[k]["单位"].ToString().Trim(),
                                    UPC = ds.Tables[0].Rows[k]["UPC"].ToString().Trim(),
                                    FromLocation = ds.Tables[0].Rows[k]["原库位"].ToString().Trim(),
                                    ToLocation = ds.Tables[0].Rows[k]["新库位"].ToString().Trim(),
                                    BatchNumber = ds.Tables[0].Columns.IndexOf("批次") == -1 ? "" : ds.Tables[0].Rows[k]["批次"].ToString().Trim(),
                                    BoxNumber = ds.Tables[0].Columns.IndexOf("托号") == -1 ? "" : ds.Tables[0].Rows[k]["托号"].ToString().Trim(),
                                    FromQty = service.GetInventoryLocationList(ds.Tables[0].Rows[k]["原库位"].ToString().Trim().Substring(ds.Tables[0].Rows[k]["原库位"].ToString().Trim().IndexOf('|') + 1),
                                        warehousename, customerid.ToString(), ds.Tables[0].Rows[k]["门店代码"].ToString().Trim()).Result.Where(c => c.SKU == ds.Tables[0].Rows[k]["SKU"].ToString().Trim() && c.GoodsType == ds.Tables[0].Rows[k]["原等级"].ToString().Trim()
                                            && c.Unit == ds.Tables[0].Rows[k]["单位"].ToString().Trim() && c.BatchNumber == (ds.Tables[0].Columns.IndexOf("批次") == -1 ? "" :
                                            ds.Tables[0].Rows[k]["批次"].ToString().Trim()) && c.BoxNumber == (ds.Tables[0].Columns.IndexOf("托号") == -1 ? ""
                                            : ds.Tables[0].Rows[k]["托号"].ToString().Trim())).Select(b => b.Qty).Sum(),
                                    // FromQty = service.GetInventoryLocationList(ds.Tables[0].Rows[k]["原库位"].ToString().Trim().Substring(ds.Tables[0].Rows[k]["原库位"].ToString().Trim().IndexOf('|') + 1),
                                    //warehousename, customerid.ToString()).Result.Where(c => c.SKU == ds.Tables[0].Rows[k]["SKU"].ToString().Trim() && c.GoodsType == ds.Tables[0].Rows[k]["原等级"].ToString().Trim()
                                    //    && c.Unit == ds.Tables[0].Rows[k]["单位"].ToString().Trim() && c.BatchNumber == (ds.Tables[0].Columns.IndexOf("批次") == -1 ? null :
                                    //    ds.Tables[0].Rows[k]["批次"].ToString().Trim()) && c.BoxNumber == (ds.Tables[0].Columns.IndexOf("托号") == -1 ? null
                                    //    : ds.Tables[0].Rows[k]["托号"].ToString().Trim())).Select(b => b.Qty).FirstOrDefault(),

                                    ToQty = int.Parse(ds.Tables[0].Rows[k]["调整数量"].ToString().Trim()),
                                    FromGoodsType = ds.Tables[0].Rows[k]["原等级"].ToString().Trim(),
                                    ToGoodsType = ds.Tables[0].Rows[k]["调整等级"].ToString().Trim(),
                                    GoodsName = ds.Tables[0].Rows[k]["产品名称"].ToString().Trim(),
                                    AdjustmentReason = ds.Tables[0].Rows[k]["调整备注"].ToString().Trim(),
                                    SKU = ds.Tables[0].Rows[k]["SKU"].ToString().Trim(),
                                    IsHold = inventorytype == "库存冻结单" ? 1 : 0,
                                    CreateTime = DateTime.Now,
                                    Creator = base.UserInfo.Name.ToString(),
                                });
                            }

                        }

                        adjustment.Each((i, adjust) =>
                        {
                            adjust.CustomerID = adjust.CustomerID;
                            adjust.CustomerName = adjust.CustomerName;
                            adjust.Warehouse = adjust.Warehouse;
                            adjust.AdjustmentType = adjust.AdjustmentType;
                            adjust.AdjustmentReason = adjust.AdjustmentReason;
                            adjust.AdjustmentTime = adjust.AdjustmentTime;
                            adjust.CreateTime = DateTime.Now;
                            adjust.UpdateTime = DateTime.Now;
                            adjust.Creator = base.UserInfo.Name.ToString();
                            adjust.IsHold = adjust.AdjustmentType == "库存冻结单" ? 1 : 0;
                            adjust.Status = 1;
                        });
                        adjustmentdetail.Each((i, adjustDetail) =>
                        {
                            adjustDetail.CustomerID = adjustDetail.CustomerID;
                            adjustDetail.CustomerName = adjustDetail.CustomerName;
                            adjustDetail.FromLot = adjustDetail.FromLot;
                            adjustDetail.ToLot = adjustDetail.ToLot;
                            adjustDetail.FromWarehouse = adjustDetail.FromWarehouse;
                            adjustDetail.ToWarehouse = adjustDetail.ToWarehouse;
                            adjustDetail.FromArea = adjustDetail.FromArea;
                            adjustDetail.ToArea = adjustDetail.ToArea;
                            adjustDetail.FromQty = adjustDetail.FromQty;
                            adjustDetail.ToQty = adjustDetail.ToQty;
                            adjustDetail.FromLocation = adjustDetail.FromLocation;
                            adjustDetail.ToLocation = adjustDetail.ToLocation;
                            adjustDetail.FromGoodsType = adjustDetail.FromGoodsType;
                            adjustDetail.ToGoodsType = adjustDetail.ToGoodsType;
                            adjustDetail.GoodsName = adjustDetail.GoodsName;
                            adjustDetail.IsHold = adjustDetail.IsHold;
                            adjustDetail.Unit = adjustDetail.Unit;
                            adjustDetail.AdjustmentReason = adjustDetail.AdjustmentReason;
                            adjustDetail.SKU = adjustDetail.SKU.Trim();
                            adjustDetail.CreateTime = DateTime.Now;
                            adjustDetail.Creator = base.UserInfo.Name.ToString();
                        });
                        AddAdjustmentandAdjustmentDetailRequest request = new AddAdjustmentandAdjustmentDetailRequest();
                        request.adjustment = adjustment;

                        request.adjustmentDetails = adjustmentdetail;

                        request.AdID = "0";

                        var response = new AdjustmentManagementService().AddAdjustmentANDAdjustmentDetail(request);
                        if (response.IsSuccess)
                        {
                            return new { result = "<h3><font color='#00dd00'>批量导入成功！</font></h3>", IsSuccess = true }.ToJsonString();
                        }
                        else
                        {
                            return new { result = "<h3><font color='#FF0000'>批量导入失败！</font></h3>" + response.Result.ToString(), IsSuccess = false }.ToJsonString();
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                }
            }
            return new { result = "<h3>失败</h3>", IsSuccess = false }.ToJsonString();
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
        [HttpGet]
        public ActionResult InventorySearch(int? PageIndex, long? warehouseID, long customerID = 0)
        {

            IndexViewModel vm = new IndexViewModel();

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
            vm.InventorySearchCondition = new InventorySearchCondition();
            vm.InventorySearchCondition.InventoryType = 1;
            vm.InventorySearchCondition.Warehouse = warehouseID.ToString();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
            //                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;
            //ApplicationConfigHelper.GetApplicationCustomers()
            //                      .Select(c => new SelectListItem() { Value = c.Name, Text = c.Name });
            vm.InventorySearchCondition.CustomerID = customerID;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.InventorySearchCondition.CustomerID == 0)
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
                    vm.InventorySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.InventorySearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

            }

            ViewBag.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetWarehouseList()
            //                      .Select(c => new SelectListItem() { Value = c.WarehouseName, Text = c.WarehouseName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehouseID).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            //ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct()
            //                      .Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            //var Areas = ApplicationConfigHelper.GetWarehouseLocationList().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            var Areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            ViewBag.Areas = Areas;
            if (warehouseID != null)
            {
                vm.InventorySearchCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == warehouseID.ToString()).First().Text + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("'" + i.Text + "',");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }
            if (base.UserInfo.UserType == 0)
            {
                vm.InventorySearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
            {
                vm.InventorySearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID != 0)
                {
                    vm.InventorySearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.InventorySearchCondition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                    }
                }
            }
            vm.InventorySearchCondition.str20 = "产品";
            ViewBag.AreaLists = Areas.Where(c => c.Value == vm.InventorySearchCondition.Warehouse).Select(c => new SelectListItem() { Value = c.Text.ToString(), Text = c.Text }); ;
            #region   //获取门店列表            
            try
            {
                IEnumerable<WMS_Customer> WMSCustomerList = ApplicationConfigHelper.GetAllCustomerByID(CustomerListID.FirstOrDefault());
                List<SelectListItem> storerlist = new List<SelectListItem>();
                foreach (var item in WMSCustomerList)
                {
                    storerlist.Add(new SelectListItem() { Value = item.StorerKey, Text = item.StorerKey });
                }
                vm.StorerList = storerlist;
            }
            catch
            {
            }
            #endregion

            var response = new InventoryManagementService().GetInventoryBySearchCondition(new GetInventoryBySearchConditionRequest()
            {
                InventorySearchCondition = vm.InventorySearchCondition,
                PageSize = UtilConstants.PAGESIZE,
                PageIndex = PageIndex ?? 0
            });
            if (response.IsSuccess)
            {
                vm.InventoryCollection = response.Result.InventoryCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            if (WarehouseList.Count() == 1)
            {
                vm.InventorySearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault().ToString();
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }

        [HttpGet]
        public ActionResult InventorySumSearch(int? PageIndex, long? warehouseID, long? customerID)
        {
            IndexViewModel vm = new IndexViewModel();
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
            vm.InventorySearchCondition = new InventorySearchCondition();
            vm.InventorySearchCondition.InventoryType = 1;
            //客户仓库
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            IEnumerable<SelectListItem> WarehouseList = null;
            if (base.UserInfo.UserType == 0)
            {
                vm.InventorySearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.InventorySearchCondition.CustomerID = (long)customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.InventorySearchCondition.CustomerID = customerIDs.First();
                    }
                    else
                    {
                        vm.InventorySearchCondition.CustomerID = 0;
                    }
                }
            }
            if (vm.InventorySearchCondition.CustomerID == 0)
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
                    vm.InventorySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.InventorySearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
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
                    vm.InventorySearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            ViewBag.WarehouseList = WarehouseList;
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehouseID).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            var Areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            ViewBag.Areas = Areas;

            ViewBag.AreaLists = Areas.Where(c => c.Value == vm.InventorySearchCondition.Warehouse).Select(c => new SelectListItem() { Value = c.Text.ToString(), Text = c.Text });


            if (WarehouseList.Count() == 1)
            {
                vm.InventorySearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault().ToString();
            }

            return View(vm);
        }

        [HttpGet]
        public ActionResult InventorySearchGroup(int? PageIndex, long? warehouseID, long customerID = 0)
        {

            IndexViewModel vm = new IndexViewModel();

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
            vm.InventorySearchCondition = new InventorySearchCondition();
            vm.InventorySearchCondition.InventoryType = 1;
            vm.InventorySearchCondition.Warehouse = warehouseID.ToString();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
            //                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;
            //ApplicationConfigHelper.GetApplicationCustomers()
            //                      .Select(c => new SelectListItem() { Value = c.Name, Text = c.Name });
            vm.InventorySearchCondition.CustomerID = customerID;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.InventorySearchCondition.CustomerID == 0)
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
                    vm.InventorySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.InventorySearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

            }

            ViewBag.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetWarehouseList()
            //                      .Select(c => new SelectListItem() { Value = c.WarehouseName, Text = c.WarehouseName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehouseID).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            //ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct()
            //                      .Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            //var Areas = ApplicationConfigHelper.GetWarehouseLocationList().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            var Areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            ViewBag.Areas = Areas;
            if (warehouseID != null)
            {
                vm.InventorySearchCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == warehouseID.ToString()).First().Text + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("'" + i.Text + "',");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }
            if (base.UserInfo.UserType == 0)
            {
                vm.InventorySearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
            {
                vm.InventorySearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID != 0)
                {
                    vm.InventorySearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.InventorySearchCondition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                    }
                }
            }
            ViewBag.AreaLists = Areas.Where(c => c.Value == vm.InventorySearchCondition.Warehouse).Select(c => new SelectListItem() { Value = c.Text.ToString(), Text = c.Text }); ;
            var response = new InventoryManagementService().GetInventoryBySearchConditionGroup(new GetInventoryBySearchConditionRequest()
            {
                InventorySearchCondition = vm.InventorySearchCondition,
                PageSize = UtilConstants.PAGESIZE,
                PageIndex = PageIndex ?? 0
            });
            if (response.IsSuccess)
            {
                vm.InventoryCollection = response.Result.InventoryCollection;
                vm.InventoryCollection2 = response.Result.InventoryCollection2;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            if (WarehouseList.Count() == 1)
            {
                vm.InventorySearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault().ToString();
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }

        [HttpGet]
        public ActionResult InventorySearchGroup2(int? PageIndex, long? warehouseID, long customerID = 0)
        {

            IndexViewModel vm = new IndexViewModel();

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
            vm.InventorySearchCondition = new InventorySearchCondition();
            vm.InventorySearchCondition.InventoryType = 1;
            vm.InventorySearchCondition.Warehouse = warehouseID.ToString();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
            //                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;
            //ApplicationConfigHelper.GetApplicationCustomers()
            //                      .Select(c => new SelectListItem() { Value = c.Name, Text = c.Name });
            vm.InventorySearchCondition.CustomerID = customerID;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.InventorySearchCondition.CustomerID == 0)
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
                    vm.InventorySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.InventorySearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

            }

            ViewBag.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetWarehouseList()
            //                      .Select(c => new SelectListItem() { Value = c.WarehouseName, Text = c.WarehouseName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehouseID).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            //ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct()
            //                      .Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            //var Areas = ApplicationConfigHelper.GetWarehouseLocationList().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            var Areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            ViewBag.Areas = Areas;
            if (warehouseID != null)
            {
                vm.InventorySearchCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == warehouseID.ToString()).First().Text + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("'" + i.Text + "',");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }
            if (base.UserInfo.UserType == 0)
            {
                vm.InventorySearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
            {
                vm.InventorySearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID != 0)
                {
                    vm.InventorySearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.InventorySearchCondition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                    }
                }
            }
            ViewBag.AreaLists = Areas.Where(c => c.Value == vm.InventorySearchCondition.Warehouse).Select(c => new SelectListItem() { Value = c.Text.ToString(), Text = c.Text }); ;
            var response = new InventoryManagementService().GetInventoryBySearchConditionGroup(new GetInventoryBySearchConditionRequest()
            {
                InventorySearchCondition = vm.InventorySearchCondition,
                PageSize = UtilConstants.PAGESIZE,
                PageIndex = PageIndex ?? 0
            });
            if (response.IsSuccess)
            {
                vm.InventoryCollection = response.Result.InventoryCollection;
                vm.InventoryCollection2 = response.Result.InventoryCollection2;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            if (WarehouseList.Count() == 1)
            {
                vm.InventorySearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault().ToString();
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }

        [HttpGet]
        public ActionResult InventorySnapshoot(long? warehouseID, int? PageIndex, long customerID = 0)
        {
            IndexViewModel vm = new IndexViewModel();
            vm.InventorySearchCondition = new InventorySearchCondition();

            vm.InventorySearchCondition.Warehouse = warehouseID.ToString();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
            //                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;
            //ApplicationConfigHelper.GetApplicationCustomers()
            //                      .Select(c => new SelectListItem() { Value = c.Name, Text = c.Name });
            vm.InventorySearchCondition.CustomerID = customerID;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.InventorySearchCondition.CustomerID == 0)
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
                    vm.InventorySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.InventorySearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

            }

            ViewBag.WarehouseList = WarehouseList;

            if (warehouseID != null)
            {
                vm.InventorySearchCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == warehouseID.ToString()).First().Text + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("'" + i.Text + "',");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }
            if (base.UserInfo.UserType == 0)
            {
                vm.InventorySearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
            {
                vm.InventorySearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID != 0)
                {
                    vm.InventorySearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.InventorySearchCondition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                    }
                }
            }
            //var response = new InventoryManagementService().GetInventoryBySearchCondition(new GetInventoryBySearchConditionRequest()
            //{
            //    InventorySearchCondition = vm.InventorySearchCondition,
            //    PageSize = UtilConstants.PAGESIZE,
            //    PageIndex = PageIndex ?? 0
            //});
            //if (response.IsSuccess)
            //{
            //    vm.InventoryCollection = response.Result.InventoryCollection;
            //    vm.PageIndex = response.Result.PageIndex;
            //    vm.PageCount = response.Result.PageCount;
            //}
            if (WarehouseList.Count() == 1)
            {
                vm.InventorySearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault().ToString();
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;

            return View(vm);
        }
        /// <summary>
        /// 库存快照查询与导出
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="Action"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InventorySnapshoot(IndexViewModel vm, string Action, int? PageIndex)
        {

            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            ViewBag.CustomerList = CustomerList;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.InventorySearchCondition.CustomerID == 0)
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
                    vm.InventorySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.InventorySearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            if (WarehouseList.Count() == 1)
            {
                vm.InventorySearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault().ToString();
            }
            ViewBag.WarehouseList = WarehouseList;

            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct()
                                    .Select(m => new SelectListItem() { Value = m.AreaName, Text = m.AreaName });

            var Areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            ViewBag.Areas = Areas;
            var request = new GetInventoryBySearchConditionRequest();
            request.InventorySearchCondition = vm.InventorySearchCondition;

            if (vm.InventorySearchCondition.Warehouse != null)
            {
                request.InventorySearchCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == vm.InventorySearchCondition.Warehouse).First().Text + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    sb.Append("'" + i.Text + "',");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            request.PageIndex = PageIndex ?? 0;
            request.PageSize = UtilConstants.PAGESIZE;
            var ss = WarehouseList.Where(m => m.Text == vm.InventorySearchCondition.Warehouse.Replace("'", "")).Select(n => n.Value).FirstOrDefault();
            ViewBag.AreaLists = Areas.Where(c => c.Value == ss).Select(c => new SelectListItem() { Value = c.Text.ToString(), Text = c.Text });

            Response<GetInventoryBySearchConditionResponse> response = new Response<GetInventoryBySearchConditionResponse>();

            if (Action == "导出")
            {
                response = new InventoryManagementService().ExportInventorySnapshootBySearchCondition(request);
            }
            else
            {
                response = new InventoryManagementService().GetInventorySnapshootBySearchCondition(request);
            }

            if (response.IsSuccess)
            {
                vm.InventorySnapCollection = response.Result.InventorySnapCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
                if (Action == "导出")
                {
                    IEnumerable<Column> columnInventorySnap;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.InventorySearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_InventorySnapshoot").Count() == 0)
                    {
                        columnInventorySnap = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Inventory").ColumnCollection;
                    }
                    else
                    {
                        columnInventorySnap = module.Tables.TableCollection.First(t => t.Name == "WMS_InventorySnapshoot").ColumnCollection;
                    }
                    ExportInventorySnap(response.Result, columnInventorySnap);
                }

            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }

        /// <summary>
        /// 库存快照报表
        /// </summary>
        /// <param name="response"></param>
        /// <param name="columnReceipt"></param>
        private void ExportInventorySnap(GetInventoryBySearchConditionResponse response, IEnumerable<Column> columnInventorySnap)
        {
            IEnumerable<InventorySnapshoot> receipts = response.InventorySnapCollection;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnInventorySnap)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }

            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnInventorySnap)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.WMS.Inventory.InventorySnapshoot).GetProperty(receipt.DbColumnName).GetValue(s);

                }
                dtReceipt.Rows.Add(drReceipt);
            });

            dtReceipt.TableName = "库存快照报表信息";
            ds.Tables.Add(dtReceipt);
            ExportDataToExcelHelper.ExportExcel(ds.Tables[0], "库存快照报表信息" + DateTime.Now.ToString("yyyy-MM-dd"), "库存快照报表");

        }

        [HttpPost]
        public ActionResult InventorySearch(IndexViewModel vm, string Action, int? PageIndex)
        {
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
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
            //                       .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;
            //ApplicationConfigHelper.GetApplicationCustomers()
            //.Select(m => new SelectListItem() { Value = m.Name, Text = m.Name });
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.InventorySearchCondition.CustomerID == 0)
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
                    vm.InventorySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.InventorySearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            if (WarehouseList.Count() == 1)
            {
                vm.InventorySearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault().ToString();
            }
            ViewBag.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetWarehouseList()
            //                    .Select(m => new SelectListItem() { Value = m.WarehouseName, Text = m.WarehouseName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct()
                                    .Select(m => new SelectListItem() { Value = m.AreaName, Text = m.AreaName });
            //var Areas = ApplicationConfigHelper.GetWarehouseLocationList().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            var Areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            ViewBag.Areas = Areas;
            var request = new GetInventoryBySearchConditionRequest();
            request.InventorySearchCondition = vm.InventorySearchCondition;

            if (vm.InventorySearchCondition.Warehouse != null)
            {
                request.InventorySearchCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == vm.InventorySearchCondition.Warehouse).First().Text + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    sb.Append("'" + i.Text + "',");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            #region   //获取门店列表            
            try
            {
                IEnumerable<WMS_Customer> WMSCustomerList = ApplicationConfigHelper.GetAllCustomerByID(CustomerListID.FirstOrDefault());
                List<SelectListItem> storerlist = new List<SelectListItem>();
                foreach (var item in WMSCustomerList)
                {
                    storerlist.Add(new SelectListItem() { Value = item.StorerKey, Text = item.StorerKey });
                }
                vm.StorerList = storerlist;
            }
            catch
            {
            }
            #endregion
            request.PageIndex = PageIndex ?? 0;
            request.PageSize = UtilConstants.PAGESIZE;
            var ss = WarehouseList.Where(m => m.Text == vm.InventorySearchCondition.Warehouse.Replace("'", "")).Select(n => n.Value).FirstOrDefault();
            ViewBag.AreaLists = Areas.Where(c => c.Value == ss).Select(c => new SelectListItem() { Value = c.Text.ToString(), Text = c.Text });
            if (Action == "查询" || Action == "InventorySearch")
            {
                var response = new InventoryManagementService().GetInventoryBySearchCondition(request);
                if (response.IsSuccess)
                {
                    vm.InventoryCollection = response.Result.InventoryCollection;
                    vm.PageIndex = response.Result.PageIndex;
                    vm.PageCount = response.Result.PageCount;
                }
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }

        [HttpPost]
        public ActionResult InventorySearchGroup(IndexViewModel vm, string Action, int? PageIndex)
        {
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
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
            //                       .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;
            //ApplicationConfigHelper.GetApplicationCustomers()
            //.Select(m => new SelectListItem() { Value = m.Name, Text = m.Name });
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.InventorySearchCondition.CustomerID == 0)
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
                    vm.InventorySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.InventorySearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            if (WarehouseList.Count() == 1)
            {
                vm.InventorySearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault().ToString();
            }
            ViewBag.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetWarehouseList()
            //                    .Select(m => new SelectListItem() { Value = m.WarehouseName, Text = m.WarehouseName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct()
                                    .Select(m => new SelectListItem() { Value = m.AreaName, Text = m.AreaName });
            //var Areas = ApplicationConfigHelper.GetWarehouseLocationList().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            var Areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            ViewBag.Areas = Areas;
            var request = new GetInventoryBySearchConditionRequest();
            request.InventorySearchCondition = vm.InventorySearchCondition;

            if (vm.InventorySearchCondition.Warehouse != null)
            {
                request.InventorySearchCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == vm.InventorySearchCondition.Warehouse).First().Text + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    sb.Append("'" + i.Text + "',");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            request.PageIndex = PageIndex ?? 0;
            request.PageSize = UtilConstants.PAGESIZE;
            var ss = WarehouseList.Where(m => m.Text == vm.InventorySearchCondition.Warehouse.Replace("'", "")).Select(n => n.Value).FirstOrDefault();
            ViewBag.AreaLists = Areas.Where(c => c.Value == ss).Select(c => new SelectListItem() { Value = c.Text.ToString(), Text = c.Text });
            if (Action == "查询" || Action == "InventorySearchGroup")
            {
                var response = new InventoryManagementService().GetInventoryBySearchConditionGroup(request);
                if (response.IsSuccess)
                {
                    vm.InventoryCollection = response.Result.InventoryCollection;
                    vm.InventoryCollection2 = response.Result.InventoryCollection2;
                    vm.PageIndex = response.Result.PageIndex;
                    vm.PageCount = response.Result.PageCount;
                }
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }
        [HttpGet]
        public ActionResult GetInventoryView(string Warehouse, string CustomerID, string location = null, string SKU = null)
        {
            IndexViewModel vm = new IndexViewModel();
            Response<GetInventoryBySearchConditionResponse> response = null;
            if (SKU == null || SKU == "")
            {
                response = new InventoryManagementService().GetInventoryByLocation(Warehouse, CustomerID, location);
            }
            if (location == null || location == "")
            {
                response = new InventoryManagementService().GetInventoryBySKU(Warehouse, CustomerID, SKU);
            }

            if (response.IsSuccess)
            {
                vm.InventoryCollection = response.Result.InventoryCollection;
                vm.ReceiptCollection = response.Result.ReceiptCollection;
                vm.OrderCollection = response.Result.OrderCollection;
                vm.AdjustmentCollection = response.Result.AdjustCollection;
            }
            return View(vm);
        }
        [HttpGet]
        public ActionResult GetInventoryViewBySKU(string Warehouse, string CustomerID, string SKU)
        {
            IndexViewModel vm = new IndexViewModel();
            var response = new InventoryManagementService().GetInventoryBySKU(Warehouse, CustomerID, SKU);
            if (response.IsSuccess)
            {
                vm.InventoryCollection = response.Result.InventoryCollection;
                vm.ReceiptCollection = response.Result.ReceiptCollection;
                vm.OrderCollection = response.Result.OrderCollection;
                vm.AdjustmentCollection = response.Result.AdjustCollection;
            }
            return View(vm);
        }
        [HttpPost]
        public string ChangeCustomer(long ID)
        {
            string js = string.Empty;
            var list = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == ID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            js = jsonSerializer.Serialize(list);
            return js;
        }
        [HttpPost]
        public JsonResult GetPrintByAdjust(string AdjustNumber)
        {
            var response = new InventoryManagementService().GetPrintByAdjust(AdjustNumber);
            if (response.IsSuccess)
            {
                if (response.Result.AdjustDetailCollection.Count() > 0)
                {
                    deleteTmpFiles(Server.MapPath("~/TotalImage/"));

                    response.Result.AdjustDetailCollection.Each((a, b) =>
                    {
                        b.StringDateTime1 = b.DateTime1.DateTimeToString("yyyy-MM-dd");
                        b.StringDateTime2 = b.DateTime2.DateTimeToString("yyyy-MM-dd");
                        string strGUID = "Inventory" + Guid.NewGuid().ToString();
                        b.PictureStr = GetDimensionalCode("[{'GoodsName':" + b.GoodsName + ", 'SKU':" + b.SKU + ",'ProductionDate':" + b.DateTime1.DateTimeToString("yyyy-MM-dd") + ", 'ExpirationDate':" + b.DateTime2.DateTimeToString("yyyy-MM-dd") + ", 'BatchNumber':" + b.BatchNumber + ", 'Manufacturer':" + b.Manufacturer + ",'BoxNumber':" + b.BoxNumber + ", 'QtyExpected':" + b.ToQty + ",'NetWeight':''," + "'GrossWeight':'' }]", strGUID + ".jpg");
                        b.PageIndex = "page" + (a + 1);
                    });
                    return Json(new { ErrorCode = "1", Response = response.Result.AdjustDetailCollection });
                }
                else
                {
                    return Json(new { ErrorCode = "0" });
                }


            }
            return Json(new { ErrorCode = "0" });
        }
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
            //if (SysIO.Directory.GetDirectories(strPath).Length > 0)
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
                    if (va.Contains("Inventory"))
                    {
                        SysIO.File.Delete(va);
                    }
                }
            }
        }
        public ActionResult GetInventoryRecord(int? PageIndex, long? warehouseID, long customerID = 0)
        {
            IndexViewModel vm = new IndexViewModel();
            vm.InventorySearchCondition = new InventorySearchCondition();
            vm.InventorySearchCondition.InventoryType = 1;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
            //                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;
            //ApplicationConfigHelper.GetApplicationCustomers()
            //                      .Select(c => new SelectListItem() { Value = c.Name, Text = c.Name });
            vm.InventorySearchCondition.CustomerID = customerID;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.InventorySearchCondition.CustomerID == 0)
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
                    vm.InventorySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.InventorySearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

            }
            ViewBag.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetWarehouseList()
            //                      .Select(c => new SelectListItem() { Value = c.WarehouseName, Text = c.WarehouseName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            if (warehouseID != null)
            {
                vm.InventorySearchCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == warehouseID.ToString()).First().Text + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    sb.Append("'" + i.Text + "',");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }
            if (base.UserInfo.UserType == 0)
            {
                vm.InventorySearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
            {
                vm.InventorySearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID != 0)
                {
                    vm.InventorySearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.InventorySearchCondition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                    }
                }
            }
            var response = new InventoryManagementService().GetInventoryRecord(new GetInventoryBySearchConditionRequest()
            {
                InventorySearchCondition = vm.InventorySearchCondition,
                PageSize = UtilConstants.PAGESIZE,
                PageIndex = PageIndex ?? 0
            });
            if (response.IsSuccess)
            {
                if (response.Result.InventoryCollection != null)
                {
                    response.Result.InventoryCollection.Each((a, b) =>
                    {
                        b.Price = (double)ApplicationConfigHelper.GetALLProductStorerList(customerID.ToString()).Where(c => c.SKU == b.SKU && c.CustomerName == b.CustomerName).Select(c => c.Price).FirstOrDefault();
                        b.Total = (b.Price * b.Qty);
                    });
                }
                vm.InventoryCollection = response.Result.InventoryCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            return View(vm);
        }
        [HttpPost]
        public ActionResult GetInventoryRecord(IndexViewModel vm, string Action, int? PageIndex)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
            //                       .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;
            //ApplicationConfigHelper.GetApplicationCustomers()
            //.Select(m => new SelectListItem() { Value = m.Name, Text = m.Name });
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.InventorySearchCondition.CustomerID == 0)
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
                    vm.InventorySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.InventorySearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            ViewBag.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetWarehouseList()
            //                    .Select(m => new SelectListItem() { Value = m.WarehouseName, Text = m.WarehouseName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct()
                                    .Select(m => new SelectListItem() { Value = m.AreaName, Text = m.AreaName });

            var request = new GetInventoryBySearchConditionRequest();
            request.InventorySearchCondition = vm.InventorySearchCondition;
            if (vm.InventorySearchCondition.Warehouse != null)
            {
                request.InventorySearchCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == vm.InventorySearchCondition.Warehouse).First().Text + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    sb.Append("'" + i.Text + "',");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            request.PageIndex = PageIndex ?? 0;
            request.PageSize = UtilConstants.PAGESIZE;
            if (Action == "查询")
            {
                var response = new InventoryManagementService().GetInventoryRecord(request);
                if (response.IsSuccess)
                {
                    if (response.Result.InventoryCollection != null)
                    {
                        response.Result.InventoryCollection.Each((a, b) =>
                        {
                            b.Price = (double)ApplicationConfigHelper.GetALLProductStorerList(vm.InventorySearchCondition.CustomerID.ToString()).Where(c => c.SKU == b.SKU && c.CustomerName == b.CustomerName).Select(c => c.Price).FirstOrDefault();
                            b.Total = (b.Price * b.Qty);
                        });
                    }
                    vm.InventoryCollection = response.Result.InventoryCollection;
                    vm.PageIndex = response.Result.PageIndex;
                    vm.PageCount = response.Result.PageCount;
                }
            }
            return View(vm);
        }
        [HttpGet]
        public ActionResult InventoryRemaining()
        {
            IndexViewModel vm = new IndexViewModel();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            InventorySearchCondition condition = new InventorySearchCondition();
            condition.CreateTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            condition.DateTime1 = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            if (condition.CustomerID == 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in CustomerList)
                {
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    condition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    condition.CustomerIDs = "0";
                }
            }
            var response = new InventoryManagementService().InventoryRemaining(condition);
            if (response.IsSuccess)
            {
                //if (response.Result.InventoryCollection != null)
                //{
                //    response.Result.InventoryCollection.Each((a, b) =>
                //       {
                //           b.Price = (decimal)ApplicationConfigHelper.GetALLProductStorerList().Where(c => c.SKU == b.SKU && c.CustomerName == b.CustomerName).Select(c => c.Price).FirstOrDefault();
                //           b.Total = (b.Price * b.Qty);
                //       });
                //}
                vm.InventorySearchCondition = condition;
                vm.directAddInventory = response.Result.directAddInventory;
                //vm.PageIndex = response.Result.PageIndex;
                //vm.PageCount = response.Result.PageCount;
            }

            return View(vm);
        }
        public ActionResult InventorydDtails(string CustomerId, string ProduceType, DateTime? Date)
        {
            IndexViewModel vm = new IndexViewModel();
            var response = new InventoryManagementService().InventorydDtails(CustomerId, ProduceType, Date);
            if (response.IsSuccess)
            {
                //if (response.Result.InventoryCollection != null)
                //{
                //    response.Result.InventoryCollection.Each((a, b) =>
                //    {
                //        b.Price = (decimal)ApplicationConfigHelper.GetALLProductStorerList().Where(c => c.SKU == b.SKU && c.CustomerName == b.CustomerName).Select(c => c.Price).FirstOrDefault();
                //        b.Total = (b.Price * b.Qty);
                //    });
                //}
                vm.directAddInventory = response.Result.directAddInventory;
            }
            return View(vm);
        }
        [HttpPost]
        public ActionResult InventoryRemaining(IndexViewModel vm, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            if (vm.InventorySearchCondition.CustomerID == 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in CustomerList)
                {
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySearchCondition.CustomerIDs = "0";
                }
            }
            if (Action == "导出汇总表")
            {
                vm.InventorySearchCondition.StateDate = vm.InventorySearchCondition.CreateTime.Value.AddDays(-1);
                var responses = new InventoryManagementService().TotalReport(vm.InventorySearchCondition);
                if (responses.IsSuccess)
                {
                    ExportBaiXingPodExceptionToExcel(responses.Result.directAddInventory, responses.Result.Total, responses.Result.daily, vm.InventorySearchCondition.StateDate.Value.ToString("yyyy-MM-dd"));
                }
                return new EmptyResult();
            }
            if (Action == "导出日报表")
            {
                vm.InventorySearchCondition.StateDate = vm.InventorySearchCondition.CreateTime.Value.AddDays(-1);
                var responses = new InventoryManagementService().dailyReport(vm.InventorySearchCondition);
                if (responses.IsSuccess)
                {
                    dailyExportBaiXingPodExceptionToExcel(responses.Result.directAddInventory, responses.Result.Total, responses.Result.daily, vm.InventorySearchCondition.StateDate.Value.ToString("yyyy-MM-dd"));
                }
                return new EmptyResult();
            }

            if (Action == "产品进销存明细表")
            {
                vm.InventorySearchCondition.StateDate = vm.InventorySearchCondition.CreateTime.Value.AddDays(-1);
                var responses = new InventoryManagementService().detailReport(vm.InventorySearchCondition);
                if (responses.IsSuccess)
                {
                    detailExportBaiXingPodExceptionToExcel(responses.Result.directAddInventory, responses.Result.Total, responses.Result.detail, vm.InventorySearchCondition.StateDate.Value.ToString());
                }
                return new EmptyResult();
            }
            var response = new InventoryManagementService().InventoryRemaining(vm.InventorySearchCondition);
            if (response.IsSuccess)
            {
                //if (response.Result.InventoryCollection != null)
                //{
                //    response.Result.InventoryCollection.Each((a, b) =>
                //    {
                //        b.Price = (decimal)ApplicationConfigHelper.GetALLProductStorerList().Where(c => c.SKU == b.SKU && c.CustomerName == b.CustomerName).Select(c => c.Price).FirstOrDefault();
                //        b.Total = (b.Price * b.Qty);
                //    });
                //}
                vm.directAddInventory = response.Result.directAddInventory;
                //vm.PageIndex = response.Result.PageIndex;
                //vm.PageCount = response.Result.PageCount;
            }
            return View(vm);

        }
        private string Getchanpinleibie(string name)
        {

            switch (name)
            {
                case "上海曲阳轮胎有限公司":
                    name = "轮胎";
                    break;
                case "上海策园实业有限公司":
                    name = "轮胎";
                    break;
                case "博联":
                    name = "dell服务器";
                    break;
                case "海润光伏":
                    name = "太阳能晶片";
                    break;
            }

            return name;
        }
        private void ExportBaiXingPodExceptionToExcel(IEnumerable<DirectAddInventory> directAddInventory, IEnumerable<DirectAddInventory> Total, IEnumerable<DirectAddInventory> daily, string datatime)
        {
            #region 表头
            DataTable dtPod = new DataTable();
            dtPod.Columns.Add("日期", typeof(string));
            dtPod.Columns.Add("项目", typeof(string));
            dtPod.Columns.Add("单位", typeof(string));
            dtPod.Columns.Add("数量", typeof(string));
            dtPod.Columns.Add("单价", typeof(string));
            dtPod.Columns.Add("金额", typeof(string));
            dtPod.Columns.Add("数量a", typeof(string));
            dtPod.Columns.Add("单价a", typeof(string));
            dtPod.Columns.Add("金额a", typeof(string));
            dtPod.Columns.Add("数量b", typeof(string));
            dtPod.Columns.Add("单价b", typeof(string));
            dtPod.Columns.Add("金额b", typeof(string));
            dtPod.Columns.Add("数量c", typeof(string));
            dtPod.Columns.Add("单价c", typeof(string));
            dtPod.Columns.Add("金额c", typeof(string));
            dtPod.Columns.Add("市场单价", typeof(string));
            dtPod.Columns.Add("市场金额", typeof(string));
            #endregion
            var data = directAddInventory.GroupBy(a => new { a.Customer, a.CustomerID, a.InventoryType, a.CreateTime, a.SCPrice })
                .Select(a => new
                {
                    a.Key.CreateTime,
                    a.Key.Customer,
                    a.Key.InventoryType,
                    Quantity = a.Sum(b => b.Quantity),
                    TotalPrice = a.Sum(b => b.TotalPrice),
                    Price = a.Sum(b => b.Price),
                    GuidePrice = a.Sum(b => b.GuidePrice),
                    SCPrice = a.Sum(b => b.SCPrice)
                }).ToList();
            #region old
            int dailyQty = 0;
            decimal dailyPrice = 0;
            int totalQty = 0;
            decimal totalPrice = 0;
            var totalSCPrice = 0;
            foreach (var item in data.GroupBy(a => a.Customer).Select(a => a.Key).ToList())
            {

                // data.Where(a => a.Customer == item && a.InventoryType == 1 && a.CreateTime.Value.ToString("yyyy-MM-dd") == datatime).Select(a => a.Quantity).FirstOrDefault();
                var CountChu = 0;
                var CountRu = daily.Where(a => a.Customer == item).Select(a => a.Qty).FirstOrDefault();
                // data.Where(a => a.Customer == item && a.InventoryType == 2 && a.CreateTime.Value.ToString("yyyy-MM-dd") == datatime).Select(a => a.Quantity).FirstOrDefault();
                var MoneryChu = 0;
                // data.Where(a => a.Customer == item && a.InventoryType == 1 && a.CreateTime.Value.ToString("yyyy-MM-dd") == datatime).Select(a => a.TotalPrice).FirstOrDefault();
                var MoneryRu = daily.Where(a => a.Customer == item).Select(a => a.TotalPrice).FirstOrDefault();

                var Count1Chu = data.Where(a => a.Customer == item && a.InventoryType == 1).Select(a => a.Quantity).FirstOrDefault();
                var Count1Ru = data.Where(a => a.Customer == item && a.InventoryType == 2).Select(a => a.Quantity).FirstOrDefault();
                var Monery1Chu = data.Where(a => a.Customer == item && a.InventoryType == 1).Select(a => a.TotalPrice).FirstOrDefault();
                var Monery1Ru = data.Where(a => a.Customer == item && a.InventoryType == 2).Select(a => a.TotalPrice).FirstOrDefault();

                var SCPrices = Total.Where(a => a.Customer == item).Select(a => a.SCPrice).FirstOrDefault();
                var QimojiecunQty = Total.Where(a => a.Customer == item).Select(a => a.Qty).FirstOrDefault();
                var QimojiecunMonery = Total.Where(a => a.Customer == item).Select(a => a.TotalPrice).FirstOrDefault();
                DataRow dr = dtPod.NewRow();
                dailyQty += CountRu;
                dailyPrice += MoneryRu;
                totalQty += Count1Ru + CountRu - Count1Chu;
                totalPrice += MoneryRu + Monery1Ru - Monery1Chu;
                totalSCPrice += Convert.ToInt32(SCPrices);
                dr["日期"] = data.Where(a => a.Customer == item).Select(a => a.CreateTime).FirstOrDefault();
                dr["项目"] = item;
                dr["单位"] = "个";
                dr["数量"] = CountRu;
                dr["单价"] = CountRu != 0 ? Convert.ToInt32(MoneryRu / CountRu) : 0;
                dr["金额"] = Convert.ToInt32(MoneryRu);
                dr["数量a"] = Count1Ru;
                dr["单价a"] = Count1Ru != 0 ? Convert.ToInt32(Monery1Ru / Count1Ru) : 0;
                dr["金额a"] = Convert.ToInt32(Monery1Ru);
                dr["数量b"] = Count1Chu;
                dr["单价b"] = Count1Chu != 0 ? Convert.ToInt32(Monery1Chu / Count1Chu) : 0;
                dr["金额b"] = Convert.ToInt32(Monery1Chu);
                dr["数量c"] = Count1Ru + CountRu - Count1Chu;// QimojiecunQty;
                dr["单价c"] = (Count1Ru + CountRu - Count1Chu) != 0 ? Convert.ToInt32((MoneryRu + Monery1Ru - Monery1Chu) / (Count1Ru + CountRu - Count1Chu)) : 0;// QimojiecunQty != 0 ? Convert.ToInt32(QimojiecunMonery / QimojiecunQty) : 0;
                dr["金额c"] = Convert.ToInt32(MoneryRu + Monery1Ru - Monery1Chu);//Convert.ToInt32(QimojiecunMonery);
                dr["市场单价"] = "";// (Count1Ru + CountRu - Count1Chu) != 0 ? Math.Abs(Convert.ToInt32(SCPrices / (Count1Ru + CountRu - Count1Chu))) : 0;
                dr["市场金额"] = "";// Convert.ToInt32(SCPrices);
                dtPod.Rows.Add(dr);
            }
            #endregion
            DataRow zheji = dtPod.NewRow();
            zheji["日期"] = "合计";
            zheji["项目"] = "";
            zheji["单位"] = "个";
            zheji["数量"] = dailyQty;
            zheji["单价"] = "";
            zheji["金额"] = Convert.ToInt32(dailyPrice);
            zheji["数量a"] = directAddInventory.Where(a => a.InventoryType == 2).Sum(a => a.Quantity);
            zheji["单价a"] = "";
            zheji["金额a"] = Convert.ToInt32(directAddInventory.Where(a => a.InventoryType == 2).Sum(a => a.TotalPrice));
            zheji["数量b"] = directAddInventory.Where(a => a.InventoryType == 1).Sum(a => a.Quantity);
            zheji["单价b"] = "";
            zheji["金额b"] = Convert.ToInt32(directAddInventory.Where(a => a.InventoryType == 1).Sum(a => a.TotalPrice));
            zheji["数量c"] = totalQty;//Count1Ru + CountRu - CountChu - Count1Chu;
            zheji["单价c"] = "";
            zheji["金额c"] = Convert.ToInt32(totalPrice);//MoneryRu + Monery1Ru - Monery1Chu - MoneryChu;
            zheji["市场单价"] = "";
            zheji["市场金额"] = "";// totalSCPrice;
            dtPod.Rows.Add(zheji);

            this.ExeclTptalReport(dtPod, "Exception.xls");
            //ExportDataToExcelHelper.ExportDataSetToExcel(dtPod, "ExportBaiXingPodExceptions", "");
            //return new EmptyResult();
        }
        private void dailyExportBaiXingPodExceptionToExcel(IEnumerable<DirectAddInventory> directAddInventory, IEnumerable<DirectAddInventory> Total, IEnumerable<DirectAddInventory> daily, string datatime)
        {
            #region 表头
            DataTable dtPod = new DataTable();
            dtPod.Columns.Add("日期", typeof(string));
            dtPod.Columns.Add("项目名称", typeof(string));
            dtPod.Columns.Add("产品类别", typeof(string));
            dtPod.Columns.Add("品牌", typeof(string));
            dtPod.Columns.Add("型号(编码)", typeof(string));
            dtPod.Columns.Add("单位", typeof(string));
            dtPod.Columns.Add("数量", typeof(string));
            dtPod.Columns.Add("单价", typeof(string));
            dtPod.Columns.Add("金额", typeof(string));
            dtPod.Columns.Add("数量a", typeof(string));
            dtPod.Columns.Add("单价a", typeof(string));
            dtPod.Columns.Add("金额a", typeof(string));
            dtPod.Columns.Add("数量b", typeof(string));
            dtPod.Columns.Add("单价b", typeof(string));
            dtPod.Columns.Add("金额b", typeof(string));
            dtPod.Columns.Add("数量c", typeof(string));
            dtPod.Columns.Add("单价c", typeof(string));
            dtPod.Columns.Add("金额c", typeof(string));
            dtPod.Columns.Add("市场单价", typeof(string));
            dtPod.Columns.Add("市场金额", typeof(string));
            #endregion
            var data = directAddInventory.GroupBy(a => new { a.Customer, a.CustomerID, a.InventoryType, a.CreateTime, a.Specifications, a.Brand, a.goodsName, a.SCPrice })
                .Select(a => new
                {
                    a.Key.CreateTime,
                    a.Key.Customer,
                    a.Key.CustomerID,
                    a.Key.InventoryType,
                    a.Key.Brand,
                    a.Key.Specifications,
                    a.Key.goodsName,
                    Quantity = a.Sum(b => b.Quantity),
                    TotalPrice = a.Sum(b => b.TotalPrice),
                    Price = a.Sum(b => b.Price),
                    GuidePrice = a.Sum(b => b.GuidePrice),
                    SCPrice = a.Sum(b => b.SCPrice)
                }).ToList();
            #region old
            int dailyQty = 0;
            decimal dailyPrice = 0;
            int totalQty = 0;
            decimal totalPrice = 0;
            var totalSCPrice = 0;
            foreach (var item in data.GroupBy(a => new { a.Customer, a.CustomerID, a.Brand, a.Specifications, a.goodsName, a.SCPrice }).Select(a => a.Key).ToList())
            {

                var CountChu = 0;
                var CountRu = daily.Where(a => a.Customer == item.Customer && a.Specifications == item.Specifications).Select(a => a.Qty).FirstOrDefault();

                var MoneryChu = 0;

                var MoneryRu = daily.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications).Select(a => a.TotalPrice).FirstOrDefault();


                var Count1Chu = data.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications && a.InventoryType == 1).Select(a => a.Quantity).FirstOrDefault();
                var Count1Ru = data.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications && a.InventoryType == 2).Select(a => a.Quantity).FirstOrDefault();
                var Monery1Chu = data.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications && a.InventoryType == 1).Select(a => a.TotalPrice).FirstOrDefault();
                var Monery1Ru = data.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications && a.InventoryType == 2).Select(a => a.TotalPrice).FirstOrDefault();
                var QimojiecunQty = Total.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications).Select(a => a.Qty).FirstOrDefault();
                var QimojiecunMonery = Total.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications).Select(a => a.TotalPrice).FirstOrDefault();
                var SCPrices = Total.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications).Select(a => a.SCPrice).FirstOrDefault();

                DataRow dr = dtPod.NewRow();
                dailyQty += CountRu;
                dailyPrice += MoneryRu;
                totalQty += Count1Ru + CountRu - Count1Chu;
                totalPrice += MoneryRu + Monery1Ru - Monery1Chu;
                totalSCPrice += Convert.ToInt32(SCPrices);
                dr["日期"] = data.Where(a => a.Customer == item.Customer).Select(a => a.CreateTime).FirstOrDefault();
                dr["项目名称"] = item.Customer;
                dr["产品类别"] = Getchanpinleibie(item.Customer);
                dr["品牌"] = item.Brand;
                dr["型号(编码)"] = ApplicationConfigHelper.GetProductStorerList(item.CustomerID).Where(a => a.SKU.Trim() == item.Specifications.Trim()).Select(a => a.GoodsName).FirstOrDefault(); //item.goodsName;
                dr["单位"] = "个";
                dr["数量"] = CountRu;
                dr["单价"] = CountRu != 0 ? Math.Abs(Convert.ToInt32(MoneryRu / CountRu)) : 0;
                dr["金额"] = Convert.ToInt32(MoneryRu);
                dr["数量a"] = Count1Ru;
                dr["单价a"] = Count1Ru != 0 ? Math.Abs(Convert.ToInt32(Monery1Ru / Count1Ru)) : 0;
                dr["金额a"] = Convert.ToInt32(Monery1Ru);
                dr["数量b"] = Count1Chu;
                dr["单价b"] = Count1Chu != 0 ? Math.Abs(Convert.ToInt32(Monery1Chu / Count1Chu)) : 0;
                dr["金额b"] = Convert.ToInt32(Monery1Chu);
                dr["数量c"] = Count1Ru + CountRu - Count1Chu;//QimojiecunQty;
                dr["单价c"] = (Count1Ru + CountRu - Count1Chu) != 0 ? Math.Abs(Convert.ToInt32((MoneryRu + Monery1Ru - Monery1Chu) / (Count1Ru + CountRu - Count1Chu))) : 0;// QimojiecunQty != 0 ? Convert.ToInt32(QimojiecunMonery / QimojiecunQty) : 0; 
                dr["金额c"] = Convert.ToInt32(MoneryRu + Monery1Ru - Monery1Chu);// MoneryRu + Monery1Ru - Monery1Chu - MoneryChu;
                dr["市场单价"] = "";// (Count1Ru + CountRu - Count1Chu) != 0 ? Math.Abs(Convert.ToInt32(SCPrices / (Count1Ru + CountRu - Count1Chu))) : 0; 
                dr["市场金额"] = "";// Convert.ToInt32(SCPrices);
                dtPod.Rows.Add(dr);
            }
            DataRow zheji = dtPod.NewRow();
            zheji["日期"] = "合计";
            zheji["项目名称"] = "";
            zheji["产品类别"] = "";
            zheji["品牌"] = "";
            zheji["型号(编码)"] = "";
            zheji["单位"] = "个";
            zheji["数量"] = dailyQty;
            zheji["单价"] = "";
            zheji["金额"] = Convert.ToInt32(dailyPrice);
            zheji["数量a"] = directAddInventory.Where(a => a.InventoryType == 2).Sum(a => a.Quantity);
            zheji["单价a"] = "";
            zheji["金额a"] = Convert.ToInt32(directAddInventory.Where(a => a.InventoryType == 2).Sum(a => a.TotalPrice));
            zheji["数量b"] = directAddInventory.Where(a => a.InventoryType == 1).Sum(a => a.Quantity);
            zheji["单价b"] = "";
            zheji["金额b"] = Convert.ToInt32(directAddInventory.Where(a => a.InventoryType == 1).Sum(a => a.TotalPrice));
            zheji["数量c"] = totalQty;// Count1Ru + CountRu - CountChu - Count1Chu;
            zheji["单价c"] = "";
            zheji["金额c"] = Convert.ToInt32(totalPrice);// MoneryRu + Monery1Ru - Monery1Chu - MoneryChu;
            zheji["市场单价"] = "";
            zheji["市场金额"] = "";// totalSCPrice;
            dtPod.Rows.Add(zheji);



            #endregion
            this.dailyExeclTptalReport(dtPod, "Exception.xls");

        }
        private void detailExportBaiXingPodExceptionToExcel(IEnumerable<DirectAddInventory> directAddInventory, IEnumerable<DirectAddInventory> Total, IEnumerable<DirectAddInventory> detail, string datatime)
        {
            #region 表头
            DataTable dtPod = new DataTable();
            dtPod.Columns.Add("日期", typeof(string));
            dtPod.Columns.Add("项目名称", typeof(string));
            dtPod.Columns.Add("产品类别", typeof(string));
            dtPod.Columns.Add("品牌", typeof(string));
            dtPod.Columns.Add("型号(编码)", typeof(string));
            dtPod.Columns.Add("出入库单号", typeof(string));
            dtPod.Columns.Add("摘要", typeof(string));
            dtPod.Columns.Add("单位", typeof(string));
            dtPod.Columns.Add("数量", typeof(string));
            dtPod.Columns.Add("单价", typeof(string));
            dtPod.Columns.Add("金额", typeof(string));
            dtPod.Columns.Add("数量a", typeof(string));
            dtPod.Columns.Add("单价a", typeof(string));
            dtPod.Columns.Add("金额a", typeof(string));
            dtPod.Columns.Add("数量b", typeof(string));
            dtPod.Columns.Add("单价b", typeof(string));
            dtPod.Columns.Add("金额b", typeof(string));
            dtPod.Columns.Add("数量c", typeof(string));
            dtPod.Columns.Add("单价c", typeof(string));
            dtPod.Columns.Add("金额c", typeof(string));
            dtPod.Columns.Add("市场单价", typeof(string));
            dtPod.Columns.Add("市场金额", typeof(string));
            #endregion
            var data = directAddInventory.GroupBy(a => new { a.Customer, a.CustomerID, a.InventoryType, a.CreateTime, a.Specifications, a.Brand, a.goodsName, a.BatchNumber })
                .Select(a => new
                {
                    a.Key.CreateTime,
                    a.Key.Customer,
                    a.Key.CustomerID,
                    a.Key.InventoryType,
                    a.Key.Brand,
                    a.Key.Specifications,
                    a.Key.goodsName,
                    a.Key.BatchNumber,
                    Quantity = a.Sum(b => b.Quantity),
                    TotalPrice = a.Sum(b => b.TotalPrice),
                    Price = a.Sum(b => b.Price),
                    GuidePrice = a.Sum(b => b.GuidePrice),
                    SCPrice = a.Sum(b => b.SCPrice)
                }).ToList();
            #region old
            int detailQty = 0;
            decimal detailPrice = 0;
            int totalQty = 0;
            decimal totalPrice = 0;
            var totalSCPrice = 0;
            foreach (var item in data.GroupBy(a => new { a.Customer, a.CustomerID, a.Brand, a.Specifications, a.BatchNumber, a.goodsName }).Select(a => a.Key).ToList())
            {

                var CountChu = 0;
                //var CountRu = daily.Where(a => a.Customer == item.Customer  && a.Specifications == item.Specifications).Select(a => a.Qty).FirstOrDefault();

                var CountRu = detail.Where(a => a.Customer == item.Customer && a.Specifications == item.Specifications).Select(a => a.Qty).FirstOrDefault();
                var MoneryChu = 0;
                var MoneryRu = detail.Where(a => a.Customer == item.Customer && a.Specifications == item.Specifications).Select(a => a.TotalPrice).FirstOrDefault();
                var Count1Chu = data.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications && a.InventoryType == 1).Select(a => a.Quantity).FirstOrDefault();
                var Count1Ru = data.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications && a.InventoryType == 2).Select(a => a.Quantity).FirstOrDefault();
                var Monery1Chu = data.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications && a.InventoryType == 1).Select(a => a.TotalPrice).FirstOrDefault();
                var Monery1Ru = data.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications && a.InventoryType == 2).Select(a => a.TotalPrice).FirstOrDefault();
                var QimojiecunQty = Total.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications).Select(a => a.Qty).FirstOrDefault();
                var QimojiecunMonery = Total.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications).Sum(a => a.TotalPrice);//Select(a => a.TotalPrice).FirstOrDefault();
                var SCPrices = Total.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.Specifications == item.Specifications).Select(a => a.SCPrice).FirstOrDefault();
                DataRow dr = dtPod.NewRow();
                detailQty += CountRu;
                detailPrice += MoneryRu;
                totalQty += Count1Ru + CountRu - Count1Chu;
                totalPrice += MoneryRu + Monery1Ru - Monery1Chu;
                totalSCPrice += Convert.ToInt32(SCPrices);
                dr["日期"] = data.Where(a => a.Customer == item.Customer).Select(a => a.CreateTime).FirstOrDefault();
                dr["项目名称"] = item.Customer;
                dr["产品类别"] = Getchanpinleibie(item.Customer);
                dr["品牌"] = item.Brand; //data.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.goodsName == item.Specifications&&a.InventoryId==item.InventoryId).Select(a => a.Brand).FirstOrDefault();
                dr["型号(编码)"] = ApplicationConfigHelper.GetProductStorerList(item.CustomerID).Where(a => a.SKU.Trim() == item.Specifications.Trim()).Select(a => a.GoodsName).FirstOrDefault();
                //data.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.goodsName == item.Specifications && a.InventoryId == item.InventoryId).Select(a => a.goodsName).FirstOrDefault();
                dr["出入库单号"] = item.BatchNumber;
                //data.Where(a => a.Customer == item.Customer && a.Brand == item.Brand && a.goodsName == item.Specifications && a.InventoryId == item.InventoryId).Select(a => a.InventoryId).FirstOrDefault();
                dr["摘要"] = "";
                dr["单位"] = "个";
                dr["数量"] = CountRu;
                dr["单价"] = CountRu != 0 ? Math.Abs(Convert.ToInt32(MoneryRu / CountRu)) : 0;
                dr["金额"] = Convert.ToInt32(MoneryRu);
                dr["数量a"] = Count1Ru;
                dr["单价a"] = Count1Ru != 0 ? Math.Abs(Convert.ToInt32(Monery1Ru / Count1Ru)) : 0;
                dr["金额a"] = Convert.ToInt32(Monery1Ru);
                dr["数量b"] = Count1Chu;
                dr["单价b"] = Count1Chu != 0 ? Math.Abs(Convert.ToInt32(Monery1Chu / Count1Chu)) : 0;
                dr["金额b"] = Convert.ToInt32(Monery1Chu);
                dr["数量c"] = Count1Ru + CountRu - Count1Chu;// Count1Ru + CountRu - CountChu - Count1Chu;
                dr["单价c"] = (Count1Ru + CountRu - Count1Chu) != 0 ? Math.Abs(Convert.ToInt32((MoneryRu + Monery1Ru - Monery1Chu) / (Count1Ru + CountRu - Count1Chu))) : 0;
                dr["金额c"] = Convert.ToInt32(MoneryRu + Monery1Ru - Monery1Chu);// MoneryRu + Monery1Ru - Monery1Chu - MoneryChu;
                dr["市场单价"] = "";//(Count1Ru + CountRu - Count1Chu) != 0 ? Math.Abs(Convert.ToInt32(SCPrices / (Count1Ru + CountRu - Count1Chu))) : 0;
                dr["市场金额"] = "";// Convert.ToInt32(SCPrices);
                dtPod.Rows.Add(dr);
            }

            DataRow zheji = dtPod.NewRow();
            zheji["日期"] = "合计";
            zheji["项目名称"] = "";
            zheji["产品类别"] = "";
            zheji["品牌"] = "";
            zheji["型号(编码)"] = "";
            zheji["出入库单号"] = "";
            zheji["摘要"] = "";
            zheji["单位"] = "个";
            zheji["数量"] = detailQty;// detail.Sum(a => a.Qty);
            zheji["单价"] = "";
            zheji["金额"] = Convert.ToInt32(detailPrice);// detail.Sum(a => a.TotalPrice);
            zheji["数量a"] = directAddInventory.Where(a => a.InventoryType == 2).Sum(a => a.Quantity);
            zheji["单价a"] = "";
            zheji["金额a"] = Convert.ToInt32(directAddInventory.Where(a => a.InventoryType == 2).Sum(a => a.TotalPrice));
            zheji["数量b"] = directAddInventory.Where(a => a.InventoryType == 1).Sum(a => a.Quantity);
            zheji["单价b"] = "";
            zheji["金额b"] = Convert.ToInt32(directAddInventory.Where(a => a.InventoryType == 1).Sum(a => a.TotalPrice));
            zheji["数量c"] = totalQty;// Count1Ru + CountRu - CountChu - Count1Chu;
            zheji["单价c"] = "";
            zheji["金额c"] = Convert.ToInt32(totalPrice);// Total.Sum(a => a.TotalPrice);// MoneryRu + Monery1Ru - Monery1Chu - MoneryChu;
            zheji["市场单价"] = "";
            zheji["市场金额"] = "";// totalSCPrice;
            dtPod.Rows.Add(zheji);
            #endregion
            this.detailExeclTptalReport(dtPod, "Exception.xls");

        }
        public ActionResult InventoryCompare()
        {
            IndexViewModel vm = new IndexViewModel();
            //客户下拉列表
            var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            ViewBag.WarehouseList = WarehouseList;
            return View();
        }
        public string DirectAddInventoryImports(string CustomerID, DateTime? Data)
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    if (ds != null && ds.Tables[0] != null)
                    {
                        IList<DirectAddInventory> Info = new List<DirectAddInventory>();
                        var aa = ApplicationConfigHelper.GetALLProductStorerList(CustomerID);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            //   int StatusTable = ds.Tables[0].Rows[i]["状态"].ToString().Trim() == "可用" ? 1 : 0;
                            int TypeTable = 0;
                            //IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_Get_Type");
                            //var dasda = wms.Where(q => q.Name == ds.Tables[0].Rows[i]["货品种类"].ToString().Trim()).Select(q => q.Code).ToList().FirstOrDefault();
                            //TypeTable = Convert.ToInt32(wms.Where(q => q.Name == ds.Tables[0].Rows[i]["货品种类"].ToString().Trim()).Select(q => q.Code).ToList().FirstOrDefault());
                            //int IsQcEligibleTable = ds.Tables[0].Rows[i]["质检是否合格"].ToString().Trim() == "否" ? 0 : 1;
                            try
                            {
                                decimal Price = aa.Where(a => a.SKU == ds.Tables[0].Rows[i]["规格"].ToString().Trim()).Select(a => a.Price).FirstOrDefault().Value;
                                Info.Add(new DirectAddInventory()
                                {

                                    Customer = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(a => a.CustomerID == long.Parse(CustomerID)).Select(a => a.CustomerName).FirstOrDefault(),
                                    InventoryId = string.Concat("Runbow", DateTime.Now.ToString("yyyyMMddHHmmssff"), (10000 + i).ToString().Substring(1)),
                                    CustomerID = long.Parse(CustomerID),// ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(a => a.CustomerName == ds.Tables[0].Rows[i]["客户"].ToString().Trim()).Select(a=>a.CustomerID).FirstOrDefault(),
                                    Brand = ds.Tables[0].Rows[i]["品牌"].ToString().Trim(),
                                    Specifications = ds.Tables[0].Rows[i]["规格"].ToString().Trim(),
                                    Price = Price,
                                    GuidePrice = decimal.Parse(ds.Tables[0].Rows[i]["单价"].ToString().Trim()),
                                    Quantity = int.Parse(ds.Tables[0].Rows[i]["操作数量"].ToString().Trim()),
                                    // TotalPrice = (decimal.Parse(ds.Tables[0].Rows[i]["单价"].ToString().Trim()) * int.Parse(ds.Tables[0].Rows[i]["操作数量"].ToString().Trim())),
                                    // Creator = base.UserInfo.Name,

                                    InventoryType = ds.Tables[0].Rows[i]["操作类型"].ToString().Trim() == "出库" ? 1 : 2,
                                    BatchNumber = ds.Tables[0].Rows[i]["操作批次号"].ToString().Trim()
                                });
                            }
                            catch (Exception e)
                            {
                                return new { result = "规格：" + ds.Tables[0].Rows[i]["规格"].ToString().Trim() + "附近，数据格式不对", IsSuccess = false }.ToString();
                            }

                        }
                        var List = Info.GroupBy(a => new { a.Brand, a.Specifications, a.InventoryType, a.BatchNumber, a.CustomerID, a.Customer }).Select(a => new DirectAddInventory()
                        {
                            Customer = a.Key.Customer,
                            InventoryId = a.Max(b => b.InventoryId),
                            CustomerID = a.Key.CustomerID,
                            Brand = a.Key.Brand,
                            Specifications = a.Key.Specifications,
                            Price = a.Max(b => b.Price),
                            GuidePrice = a.Max(b => b.GuidePrice),
                            Quantity = a.Sum(b => b.Quantity),
                            TotalPrice = a.Max(b => b.GuidePrice) * a.Sum(b => b.Quantity),
                            Creator = base.UserInfo.Name,
                            InventoryType = a.Key.InventoryType,
                            BatchNumber = a.Key.BatchNumber,
                            CreateTime = Data ?? DateTime.Now
                        }).ToList();
                        var response = new InventoryManagementService().DirectAddInventoryImports(new GetInventoryBySearchConditionRequest() { directAddInventory = List });

                        if (response == "成功")
                        {
                            //ApplicationConfigHelper.RefreshGetProductStorerList();
                            return new { result = "导入成功！", IsSuccess = true }.ToJsonString();
                        }
                    }
                    return new { result = "<h3>导入失败!</h3><br/>excel内容有误！", IsSuccess = false }.ToString();
                }
                return new { result = "文件内容为空", IsSuccess = false }.ToString();
            }
            return new { result = "请选择文件", IsSuccess = false }.ToString();
        }
        public JsonResult DelDirectAddInventory(string Id)
        {
            var response = new InventoryManagementService().DelDirectAddInventory(Id);
            if (response)
            {
                return Json(new { Code = 1 });
            }
            return Json(new { Code = 0 });
        }
        public void ExeclTptalReport(DataTable dt, string FileName)
        {
            //var response = new InventoryManagementService().SupplyChainReport();
            var sbHtml = new StringBuilder();
            sbHtml.Append("<style>td{mso-number-format:\"\\@\";}</style>");
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr><td style='font-size: 25px;text-align:center' colspan=17>供应链项目库存汇总表</td></tr>");
            sbHtml.Append("<tr><td rowspan=2>日期</td><td rowspan=2>项目</td><td rowspan=2>单位</td><td colspan=3>期初库存</td><td colspan=3>本期入库</td><td colspan=3>本期出库</td><td colspan=3>期末结存</td><td colspan=2>备注</td></tr>");
            sbHtml.Append(" <tr> <td  >数量</td> <td  >单价</td> <td  >金额</td> <td  >数量</td> <td >单价</td> <td >金额</td> <td  >数量</td> <td >单价</td> <td >金额</td> <td  >数量</td> <td >单价</td> <td >金额</td> <td >市场单价</td> <td >市场金额</td> </tr>");

            //sbHtml.Append("<tr>");
            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", dt.Columns[i].ColumnName);
            //}

            //sbHtml.Append("</tr>");

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
            HttpResponse Response;
            Response = we.HttpContext.Current.Response;
            Response.Charset = "UTF-8";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.AppendHeader("content-disposition", "attachment;filename=" + FileName);
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sbHtml.ToString());
            Response.Flush();
            Response.End();
        }
        public void dailyExeclTptalReport(DataTable dt, string FileName)
        {
            var response = new InventoryManagementService().SupplyChainReport();
            var sbHtml = new StringBuilder();
            sbHtml.Append("<style>td{mso-number-format:\"\\@\";}</style>");
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr><td style='font-size: 25px;text-align:center' colspan=20>项目库存日报表</td></tr>");
            sbHtml.Append("<tr><td rowspan=2>日期</td><td rowspan=2>项目名称</td><td rowspan=2>产品类别</td><td rowspan=2>品牌</td><td rowspan=2>型号(编码)</td><td rowspan=2>单位</td><td colspan=3>期初库存</td><td colspan=3>本期入库</td><td colspan=3>本期出库</td><td colspan=3>期末结存</td><td colspan=2>备注</td></tr>");
            sbHtml.Append(" <tr> <td>数量</td> <td>单价</td> <td>金额</td> <td>数量</td> <td >单价</td> <td>金额</td> <td>数量</td> <td>单价</td> <td>金额</td> <td>数量</td> <td>单价</td> <td >金额</td> <td>市场单价</td> <td>市场金额</td> </tr>");

            //sbHtml.Append("<tr>");
            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", dt.Columns[i].ColumnName);
            //}

            //sbHtml.Append("</tr>");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sbHtml.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i][j].ToString());
                }
                sbHtml.Append("</tr>");
            }
            //  sbHtml.Append("<tr>  <td>合计 </td> <td> </td>  <td> </td>  <td>数量 </td>  <td> </td>  <td> 金额</td>  <td> 数量</td>  <td> </td> <td> 金额 </td>   <td> 数量</td>   <td> </td>  <td> 金额</td>  <td> 数量</td>  <td>  </td>  <td>金额 </td>  <td>市场单价 </td> <td>市场金额 </td> </tr>");
            sbHtml.Append("</table>");
            HttpResponse Response;
            Response = we.HttpContext.Current.Response;
            Response.Charset = "UTF-8";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.AppendHeader("content-disposition", "attachment;filename=" + FileName);
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sbHtml.ToString());
            Response.Flush();
            Response.End();


        }
        public void detailExeclTptalReport(DataTable dt, string FileName)
        {
            //var response = new InventoryManagementService().SupplyChainReport();
            var sbHtml = new StringBuilder();
            sbHtml.Append("<style>td{mso-number-format:\"\\@\";}</style>");
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr><td style='font-size: 25px;text-align:center' colspan=22>产品进销存明细表</td></tr>");
            sbHtml.Append("<tr><td rowspan=2>日期</td><td rowspan=2>项目</td><td rowspan=2>产品类别</td><td rowspan=2>品牌</td><td rowspan=2>型号(编码)</td><td rowspan=2>出入库单号</td><td rowspan=2>摘要</td><td rowspan=2>单位</td><td colspan=3>期初库存</td><td colspan=3>本期入库</td><td colspan=3>本期出库</td><td colspan=3>期末结存</td><td colspan=2>备注</td></tr>");
            sbHtml.Append(" <tr> <td  >数量</td> <td  >单价</td> <td  >金额</td> <td  >数量</td> <td >单价</td> <td >金额</td> <td  >数量</td> <td >单价</td> <td >金额</td> <td  >数量</td> <td >单价</td> <td >金额</td> <td >市场单价</td> <td >市场金额</td> </tr>");

            //sbHtml.Append("<tr>");
            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", dt.Columns[i].ColumnName);
            //}

            //sbHtml.Append("</tr>");

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
            HttpResponse Response;
            Response = we.HttpContext.Current.Response;
            Response.Charset = "UTF-8";
            Response.HeaderEncoding = Encoding.UTF8;
            Response.AppendHeader("content-disposition", "attachment;filename=" + FileName);
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/ms-excel";
            Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sbHtml.ToString());
            Response.Flush();
            Response.End();


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
            IList<ReceiptDetail> Inventorys = new List<ReceiptDetail>();
            if (ID.Contains(","))
            {
                string[] strs = ID.Split(',');
                foreach (var str in strs)
                {
                    Inventorys.Add(new ReceiptDetail
                    {
                        ID = str.ObjectToInt32()

                    });

                }
            }
            else
            {
                Inventorys.Add(new ReceiptDetail
                {
                    ID = ID.ObjectToInt32()
                });

            }
            var response = new ReportManagementService().GetPrintLabelInventorySearch(Inventorys);
            if (response.IsSuccess)
            {
                deleteTmpFilesInventorySearch(Server.MapPath("~/TotalImage/"));

                response.Result.receiptPrint.Each((a, b) =>
            {
                b.StringDateTime1 = b.DateTime1.DateTimeToString("yyyy-MM-dd");
                b.StringDateTime2 = b.DateTime2.DateTimeToString("yyyy-MM-dd");
                string strGUID = "InventorySearch" + Guid.NewGuid().ToString();
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
        private void deleteTmpFilesInventorySearch(string strPath)
        {
            //删除这个目录下的所有子目录
            //if (SysIO.Directory.GetDirectories(strPath).Length > 0)
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
                    if (va.Contains("InventorySearch"))
                    {
                        SysIO.File.Delete(va);
                    }
                }
            }
        }
        [HttpGet]
        public string GetLocationMax(string Location, long warehouseid)
        {
            Location = Location.Substring(Location.IndexOf('|') + 1, Location.Length - Location.IndexOf('|') - 1);
            string val = new ReportManagementService().GetLocationMax(Location, warehouseid);
            return val;
        }
        [HttpPost]
        public string InventoryCompare(string CustomerName, long CustomerID, string Type)
        {
            string message;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    if (ds == null)
                    {
                        return new { result = "<h3><font color='#FF0000'>Excel format is incorrect</font></h3>", IsSuccess = false }.ToJsonString();
                    }
                    var response = new InventoryManagementService().InventoryCompare(ds, base.UserInfo, out message);

                    if (response.IsSuccess && message == "成功")
                    {
                        //if (Type == "importexcel")
                        //{
                        ExportInventoryCompare(response.Result.InventoryCompareCollection);
                        //return new { result = "<h3><font color='#FF0000'>Export successful!</font></h3>", IsSuccess = true }.ToJsonString();
                        //}
                        //else
                        //{
                        //    ExportInventoryCompare(response.Result.InventoryCompareCollection);
                        //    return new { result = "Export Success!", IsSuccess = true }.ToJsonString();
                        //}
                    }
                    else
                    {
                        return new { result = "<h3><font color='#FF0000'>Import failed!</font></h3>" + response.Result.ToString(), IsSuccess = false }.ToJsonString();
                    }

                }
            }
            return new { result = "<h3><font color='#FF0000'>Import failed!</font></h3>", IsSuccess = false }.ToJsonString();
        }
        public void ExportInventoryCompare(IEnumerable<InventoryCompare> IC)
        {
            IEnumerable<Column> columnReceipt;
            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerID).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_InventoryCompare").Count() == 0)
            {
                columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrder").ColumnCollection;
            }
            else
            {
                columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_InventoryCompare").ColumnCollection;
            }

            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }
            ds.Tables.Add(dtReceipt);

            IC.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {
                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.InventoryCompare).GetProperty(receipt.DbColumnName).GetValue(s);
                }
                dtReceipt.Rows.Add(drReceipt);
            });
            ds.Tables[0].TableName = "Sheet1";
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "InventoryCompare " + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "InventoryCompare " + DateTime.Now.ToString("yyyy-MM-dd"));
        }
        public void ReportExportExecl(long? ID)
        {
            long CustomerID = ID ?? 0;
            if (CustomerID == 79)  //YXDRBJ模板
            {
                //ExportDemoYXDRBJ();
                return;
            }
            IEnumerable<Column> columnReceipt;
            //IEnumerable<Column> columnReceiptDetail;
            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerID).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_InventoryAdjustment").Count() == 0)
            {
                columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_InventoryAdjustment").ColumnCollection;
            }
            else
            {
                columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_InventoryAdjustment").ColumnCollection;
            }
            //if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_PreOrderDetail").Count() == 0)
            //{
            //    columnReceiptDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
            //}
            //else
            //{
            //    columnReceiptDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
            //}
            if (CustomerID == 0)
            {
                columnReceipt = columnReceipt.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                //columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
            }
            else
            {
                //var notKeyColumns1 = columnReceipt.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID));
                //var notKeyColumns2 = columnReceiptDetail.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID));
                //columnReceipt = columnReceipt.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true && c.DbColumnName != "ReceiptType" && c.DbColumnName != "Status")).Union(notKeyColumns1.Where(c => c.IsShowInList));
                //columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true)).Union(notKeyColumns2.Where(c => c.IsShowInList));
            }
            ExportInventoryDemo(columnReceipt, "Demo");//receipts, PreOd,
        }
        private void ExportInventoryDemo(IEnumerable<Column> columnReceipt, string type)
        {
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            //DataTable dtReceiptDetail = new DataTable();
            if (type == "Demo")
            {
                foreach (var receipt in columnReceipt)
                {
                    dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
                }
                //foreach (var receiptDetail in columnReceiptDetail)
                //{
                //    if (receiptDetail.DisplayName != "客户" && receiptDetail.DisplayName != "预出库单号" && receiptDetail.DisplayName != "仓库编号" && receiptDetail.DisplayName != "产品名称" && receiptDetail.DisplayName != "行号" && receiptDetail.DisplayName != "可用数量" && receiptDetail.DisplayName != "已分配数量")
                //    {
                //        dtReceiptDetail.Columns.Add(receiptDetail.DisplayName, typeof(string));
                //    }
                //}
            }
            else
            {
                foreach (var receipt in columnReceipt)
                {
                    dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
                }
            }

            string ProjectName = base.UserInfo.ProjectName.ToString();

            if (ProjectName == "NIKEReturn")
            {
                DataRow dr1 = dtReceipt.NewRow();
                dr1["调整类型"] = "库存调整单";
                dr1["原库区"] = "A01";
                dr1["新库区"] = "A01";
                dr1["原库位"] = "01-01-A";
                dr1["新库位"] = "01-01-A";
                dr1["SKU"] = "需填";
                dr1["单位"] = "当前项目的单位，如无单位则不填";
                //dr1["调整数量"] = "调整后数量（需填）";
                dr1["调整数量"] = "调整数量（需填 调增：10，调减-10）";
                dr1["原等级"] = "需填";
                dr1["调整等级"] = "需填";
                dr1["调整原因"] = "ADJ001";
                dr1["调整备注"] = "选填";
                dr1["门店代码"] = "QQQQQQ";
                dtReceipt.Rows.Add(dr1);

                DataRow dr2 = dtReceipt.NewRow();
                dr2["调整类型"] = "库存移动单";
                dr2["原库区"] = "A01";
                dr2["新库区"] = "A01";
                dr2["原库位"] = "01-01-A";
                dr2["新库位"] = "01-01-A";
                dr2["SKU"] = "需填";
                dr2["单位"] = "当前项目的单位，如无单位则不填";
                //dr1["调整数量"] = "调整后数量（需填）";
                dr2["调整数量"] = "需移动数量";
                dr2["原等级"] = "需填";
                dr2["调整等级"] = "需填";
                dr2["调整原因"] = "选填";
                dr2["调整备注"] = "选填";
                dr2["门店代码"] = "QQQQQQ";
                dtReceipt.Rows.Add(dr2);

                DataRow dr3 = dtReceipt.NewRow();
                dr3["调整类型"] = "库存冻结单";
                dr3["原库区"] = "A01";
                dr3["新库区"] = "A01";
                dr3["原库位"] = "01-01-A";
                dr3["新库位"] = "01-01-A";
                dr3["SKU"] = "需填";
                dr3["单位"] = "当前项目的单位，如无单位则不填";
                //dr1["调整数量"] = "调整后数量（需填）";
                dr3["调整数量"] = "需冻结数量";
                dr3["原等级"] = "需填";
                dr3["调整等级"] = "需填";
                dr3["调整原因"] = "选填";
                dr3["调整备注"] = "选填";
                dr3["门店代码"] = "QQQQQQ";
                dtReceipt.Rows.Add(dr3);

                DataRow dr4 = dtReceipt.NewRow();
                dr4["调整类型"] = "库存品级调整单";
                dr4["原库区"] = "A01";
                dr4["新库区"] = "A01";
                dr4["原库位"] = "01-01-A";
                dr4["新库位"] = "01-01-A";
                dr4["SKU"] = "需填";
                dr4["单位"] = "当前项目的单位，如无单位则不填";
                //dr1["调整数量"] = "调整后数量（需填）";
                dr4["调整数量"] = "需变更数量";
                dr4["原等级"] = "需填";
                dr4["调整等级"] = "需填";
                dr4["调整原因"] = "选填";
                dr4["调整备注"] = "选填";
                dr4["门店代码"] = "QQQQQQ";
                dtReceipt.Rows.Add(dr4);

                //DataRow dr11 = dtReceiptDetail.NewRow();
                //dr11["外部单号"] = "Total001";
                //dr11["仓库"] = "道达尔天津仓";
                //dr11["SKU"] = "1005031";
                //dr11["产品等级"] = "A品";
                //dr11["期望数量"] = 10;
                //dtReceiptDetail.Rows.Add(dr11);

                dtReceipt.TableName = "Sheet1";
                //dtReceiptDetail.TableName = "预出库单明细信息";

                ds.Tables.Add(dtReceipt);
                //ds.Tables.Add(dtReceiptDetail);
            }
            else
            {
                DataRow dr1 = dtReceipt.NewRow();
                dr1["调整类型"] = "库存调整单";
                dr1["原库区"] = "A01";
                dr1["新库区"] = "A01";
                dr1["原库位"] = "01-01-A";
                dr1["新库位"] = "01-01-A";
                dr1["SKU"] = "需填";
                dr1["单位"] = "当前项目的单位，如无单位则不填";
                //dr1["调整数量"] = "调整后数量（需填）";
                dr1["调整数量"] = "调整数量（需填 调增：10，调减-10）";
                dr1["原等级"] = "需填";
                dr1["调整等级"] = "需填";
                dr1["调整原因"] = "ADJ001";
                dr1["调整备注"] = "选填";
                dtReceipt.Rows.Add(dr1);

                DataRow dr2 = dtReceipt.NewRow();
                dr2["调整类型"] = "库存移动单";
                dr2["原库区"] = "A01";
                dr2["新库区"] = "A01";
                dr2["原库位"] = "01-01-A";
                dr2["新库位"] = "01-01-A";
                dr2["SKU"] = "需填";
                dr2["单位"] = "当前项目的单位，如无单位则不填";
                //dr1["调整数量"] = "调整后数量（需填）";
                dr2["调整数量"] = "需移动数量";
                dr2["原等级"] = "需填";
                dr2["调整等级"] = "需填";
                dr2["调整原因"] = "选填";
                dr2["调整备注"] = "选填";
                dtReceipt.Rows.Add(dr2);

                DataRow dr3 = dtReceipt.NewRow();
                dr3["调整类型"] = "库存冻结单";
                dr3["原库区"] = "A01";
                dr3["新库区"] = "A01";
                dr3["原库位"] = "01-01-A";
                dr3["新库位"] = "01-01-A";
                dr3["SKU"] = "需填";
                dr3["单位"] = "当前项目的单位，如无单位则不填";
                //dr1["调整数量"] = "调整后数量（需填）";
                dr3["调整数量"] = "需冻结数量";
                dr3["原等级"] = "需填";
                dr3["调整等级"] = "需填";
                dr3["调整原因"] = "选填";
                dr3["调整备注"] = "选填";
                dtReceipt.Rows.Add(dr3);

                DataRow dr4 = dtReceipt.NewRow();
                dr4["调整类型"] = "库存品级调整单";
                dr4["原库区"] = "A01";
                dr4["新库区"] = "A01";
                dr4["原库位"] = "01-01-A";
                dr4["新库位"] = "01-01-A";
                dr4["SKU"] = "需填";
                dr4["单位"] = "当前项目的单位，如无单位则不填";
                //dr1["调整数量"] = "调整后数量（需填）";
                dr4["调整数量"] = "需变更数量";
                dr4["原等级"] = "需填";
                dr4["调整等级"] = "需填";
                dr4["调整原因"] = "选填";
                dr4["调整备注"] = "选填";
                dtReceipt.Rows.Add(dr4);

                //DataRow dr11 = dtReceiptDetail.NewRow();
                //dr11["外部单号"] = "Total001";
                //dr11["仓库"] = "道达尔天津仓";
                //dr11["SKU"] = "1005031";
                //dr11["产品等级"] = "A品";
                //dr11["期望数量"] = 10;
                //dtReceiptDetail.Rows.Add(dr11);

                dtReceipt.TableName = "Sheet1";
                //dtReceiptDetail.TableName = "预出库单明细信息";

                ds.Tables.Add(dtReceipt);
                //ds.Tables.Add(dtReceiptDetail);
            }
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "InventoryCompareDemo " + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "InventoryCompareDemo " + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// 库存调整
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="customerID"></param>
        /// <param name="ViewType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AdjustAddorEditorView(int ID, long? customerID, int ViewType = 0)
        {
            IndexViewModel vm = new IndexViewModel();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.AdjustmentCondition = new AdjustmentSearchCondition();
            if (customerID != null && customerID != 0)
            {
                var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = WarehouseList;
            }
            else
            {
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            vm.AdjustmentCondition.AdjustmentTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getAdjustmentByConditionResponse = new AdjustmentManagementService().GetAdjustmentInfos(new GetAdjustmentByConditionRequest() { ID = ID });
            if (getAdjustmentByConditionResponse.IsSuccess)
            {
                vm = new IndexViewModel()
                {
                    AdjustmentAndAdjustmentDetails = getAdjustmentByConditionResponse.Result
                };
            }
            if (ViewType == 1)
            {
                vm.AdjustmentAndAdjustmentDetails = new AdjustmentAndAdjustmentDetail();
                vm.AdjustmentAndAdjustmentDetails.adjustment = new Adjustment();
                //vm.AdjustmentAndAdjustmentDetails.adjustment.Warehouse = WarehouseName;
            }
            if (ViewType == 2)
            {
                var WarehouseID = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct().Where(n => n.WarehouseName == vm.AdjustmentAndAdjustmentDetails.adjustment.Warehouse)
                                   .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString() }).FirstOrDefault().Value;
                var UnitAndSpecificationsList = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(base.UserInfo.ProjectID, vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID, WarehouseID.ObjectToInt64());
                var UnitList = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Unit }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
                var SpecificationsList = UnitAndSpecificationsList.Select(t => new { Code = t.Specifications, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });

                ViewBag.skulist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.SKU + "|" + m.ToGoodsType, Text = m.SKU + "|" + m.ToGoodsType });
                ViewBag.upclist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.UPC, Text = m.UPC });
                ViewBag.batchnumberlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.BatchNumber, Text = m.BatchNumber });
                ViewBag.boxnumberlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.BoxNumber, Text = m.BoxNumber });
                //ViewBag.Unitlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Unit });
                //ViewBag.Specificationslist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Specifications });
                ViewBag.togoodstypelist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.ToGoodsType, Text = m.ToGoodsType });
                ViewBag.Unitlist = UnitList;
                //vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Unit });
                ViewBag.Specificationslist = SpecificationsList;
            }
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            if (customerID != null)
            {
                if (base.UserInfo.UserType == 0)
                {
                    vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = customerID;
                    }
                    else
                    {
                        var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                        if (customerIDs != null && customerIDs.Count() == 1)
                        {
                            vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = customerIDs.First();
                        }
                    }
                }
            }
            //IEnumerable<WMSConfig> wmsCompany;
            //if (customerID==103)
            //{
            //    wmsCompany = new ConfigService().GetWMSCustomerConfig_Return(customerID).Result;
            //}
            //else
            //{
            //    wmsCompany = new ConfigService().GetWMSCustomerConfig().Result;
            //}
            //List<SelectListItem> stCompanty = new List<SelectListItem>();
            //foreach (WMSConfig w in wmsCompany)
            //{
            //    stCompanty.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            //}
            vm.CompanyCodeList = null;
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
            vm.GoodsTypes = wms.Select(c => new SelectListItem() { Value = c.Name, Text = c.Name });
            vm.ViewType = ViewType;
            vm.AdjustmentAndAdjustmentDetails.adjustment.AdjustmentTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            #region  页面读取那个需要显示reason
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);
        }
        /// <summary>
        /// 移库单
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="customerID"></param>
        /// <param name="ViewType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MoveAddorEditorView(int ID, long? customerID, int ViewType = 0)
        {
            IndexViewModel vm = new IndexViewModel();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.AdjustmentCondition = new AdjustmentSearchCondition();
            if (customerID != null && customerID != 0)
            {
                var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = WarehouseList;
            }
            else
            {
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            vm.AdjustmentCondition.AdjustmentTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getAdjustmentByConditionResponse = new AdjustmentManagementService().GetAdjustmentInfos(new GetAdjustmentByConditionRequest() { ID = ID });
            if (getAdjustmentByConditionResponse.IsSuccess)
            {
                vm = new IndexViewModel()
                {
                    AdjustmentAndAdjustmentDetails = getAdjustmentByConditionResponse.Result
                };
            }
            if (ViewType == 1)
            {
                vm.AdjustmentAndAdjustmentDetails = new AdjustmentAndAdjustmentDetail();
                vm.AdjustmentAndAdjustmentDetails.adjustment = new Adjustment();
                //vm.AdjustmentAndAdjustmentDetails.adjustment.Warehouse = WarehouseName;
                if (customerID != null)
                {
                    if (base.UserInfo.UserType == 0)
                    {
                        vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = base.UserInfo.CustomerOrShipperID;
                    }
                    else if (base.UserInfo.UserType == 2)
                    {
                        if (customerID.HasValue)
                        {
                            vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = customerID;
                        }
                        else
                        {
                            var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                            if (customerIDs != null && customerIDs.Count() == 1)
                            {
                                vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = customerIDs.First();
                            }
                        }
                    }
                }
            }
            if (ViewType == 2)
            {
                var WarehouseID = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct().Where(n => n.WarehouseName == vm.AdjustmentAndAdjustmentDetails.adjustment.Warehouse)
                                   .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString() }).FirstOrDefault().Value;
                var UnitAndSpecificationsList = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(base.UserInfo.ProjectID, vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID, WarehouseID.ObjectToInt64());
                var UnitList = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Unit }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
                var SpecificationsList = UnitAndSpecificationsList.Select(t => new { Code = t.Specifications, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });

                ViewBag.skulist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.SKU + "|" + m.ToGoodsType, Text = m.SKU + "|" + m.ToGoodsType });
                ViewBag.upclist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.UPC, Text = m.UPC });
                ViewBag.batchnumberlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.BatchNumber, Text = m.BatchNumber });
                ViewBag.boxnumberlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.BoxNumber, Text = m.BoxNumber });
                //ViewBag.Unitlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Unit });
                //ViewBag.Specificationslist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Specifications });
                ViewBag.togoodstypelist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.ToGoodsType, Text = m.ToGoodsType });
                ViewBag.Unitlist = UnitList;
                //vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Unit });
                ViewBag.Specificationslist = SpecificationsList;
            }
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;

            //IEnumerable<WMSConfig> wmsCompany;
            //if (customerID == 103)
            //{
            //    wmsCompany = new ConfigService().GetWMSCustomerConfig_Return(customerID).Result;
            //}
            //else
            //{
            //    wmsCompany = new ConfigService().GetWMSCustomerConfig().Result;
            //}
            //List<SelectListItem> stCompanty = new List<SelectListItem>();
            //foreach (WMSConfig w in wmsCompany)
            //{
            //    stCompanty.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            //}
            vm.CompanyCodeList = null;

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
            vm.GoodsTypes = wms.Select(c => new SelectListItem() { Value = c.Name, Text = c.Name });
            vm.ViewType = ViewType;
            vm.AdjustmentAndAdjustmentDetails.adjustment.AdjustmentTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            return View(vm);
        }
        /// <summary>
        /// 冻结单
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="customerID"></param>
        /// <param name="ViewType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FrozenAddorEditorView(int ID, long? customerID, int ViewType = 0)
        {
            IndexViewModel vm = new IndexViewModel();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.AdjustmentCondition = new AdjustmentSearchCondition();
            if (customerID != null && customerID != 0)
            {
                var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = WarehouseList;
            }
            else
            {
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            vm.AdjustmentCondition.AdjustmentTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getAdjustmentByConditionResponse = new AdjustmentManagementService().GetAdjustmentInfos(new GetAdjustmentByConditionRequest() { ID = ID });
            if (getAdjustmentByConditionResponse.IsSuccess)
            {
                vm = new IndexViewModel()
                {
                    AdjustmentAndAdjustmentDetails = getAdjustmentByConditionResponse.Result
                };
            }
            if (ViewType == 1)
            {
                vm.AdjustmentAndAdjustmentDetails = new AdjustmentAndAdjustmentDetail();
                vm.AdjustmentAndAdjustmentDetails.adjustment = new Adjustment();
                //vm.AdjustmentAndAdjustmentDetails.adjustment.Warehouse = WarehouseName;
            }
            if (ViewType == 2)
            {
                var WarehouseID = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct().Where(n => n.WarehouseName == vm.AdjustmentAndAdjustmentDetails.adjustment.Warehouse)
                                   .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString() }).FirstOrDefault().Value;
                var UnitAndSpecificationsList = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(base.UserInfo.ProjectID, vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID, WarehouseID.ObjectToInt64());
                var UnitList = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Unit }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
                var SpecificationsList = UnitAndSpecificationsList.Select(t => new { Code = t.Specifications, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });

                ViewBag.skulist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.SKU + "|" + m.ToGoodsType, Text = m.SKU + "|" + m.ToGoodsType });
                ViewBag.upclist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.UPC, Text = m.UPC });
                ViewBag.batchnumberlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.BatchNumber, Text = m.BatchNumber });
                ViewBag.boxnumberlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.BoxNumber, Text = m.BoxNumber });
                //ViewBag.Unitlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Unit });
                //ViewBag.Specificationslist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Specifications });
                ViewBag.togoodstypelist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.ToGoodsType, Text = m.ToGoodsType });
                ViewBag.Unitlist = UnitList;
                //vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Unit });
                ViewBag.Specificationslist = SpecificationsList;
            }
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            if (customerID != null)
            {
                if (base.UserInfo.UserType == 0)
                {
                    vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = customerID;
                    }
                    else
                    {
                        var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                        if (customerIDs != null && customerIDs.Count() == 1)
                        {
                            vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = customerIDs.First();
                        }
                    }
                }
            }
            //IEnumerable<WMSConfig> wmsCompany;
            //if (customerID == 103)
            //{
            //    wmsCompany = new ConfigService().GetWMSCustomerConfig_Return(customerID).Result;
            //}
            //else
            //{
            //    wmsCompany = new ConfigService().GetWMSCustomerConfig().Result;
            //}
            //List<SelectListItem> stCompanty = new List<SelectListItem>();
            //foreach (WMSConfig w in wmsCompany)
            //{
            //    stCompanty.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            //}
            vm.CompanyCodeList = null;

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
            vm.GoodsTypes = wms.Select(c => new SelectListItem() { Value = c.Name, Text = c.Name });
            vm.ViewType = ViewType;
            vm.AdjustmentAndAdjustmentDetails.adjustment.AdjustmentTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            return View(vm);
        }
        /// <summary>
        /// 品级调整单
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="customerID"></param>
        /// <param name="ViewType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AdjustGoodsTypeAddorEditorView(int ID, long? customerID, int ViewType = 0)
        {
            IndexViewModel vm = new IndexViewModel();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.AdjustmentCondition = new AdjustmentSearchCondition();
            if (customerID != null && customerID != 0)
            {
                var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = WarehouseList;
            }
            else
            {
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            vm.AdjustmentCondition.AdjustmentTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getAdjustmentByConditionResponse = new AdjustmentManagementService().GetAdjustmentInfos(new GetAdjustmentByConditionRequest() { ID = ID });
            if (getAdjustmentByConditionResponse.IsSuccess)
            {
                vm = new IndexViewModel()
                {
                    AdjustmentAndAdjustmentDetails = getAdjustmentByConditionResponse.Result
                };
            }
            if (ViewType == 1)
            {
                vm.AdjustmentAndAdjustmentDetails = new AdjustmentAndAdjustmentDetail();
                vm.AdjustmentAndAdjustmentDetails.adjustment = new Adjustment();
                //vm.AdjustmentAndAdjustmentDetails.adjustment.Warehouse = WarehouseName;
            }
            if (ViewType == 2)
            {
                var WarehouseID = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID.ToString(), WarehouseName = t.WarehouseName }).Distinct().Where(n => n.WarehouseName == vm.AdjustmentAndAdjustmentDetails.adjustment.Warehouse)
                                   .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString() }).FirstOrDefault().Value;
                var UnitAndSpecificationsList = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(base.UserInfo.ProjectID, vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID, WarehouseID.ObjectToInt64());
                var UnitList = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Unit }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
                var SpecificationsList = UnitAndSpecificationsList.Select(t => new { Code = t.Specifications, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });

                ViewBag.skulist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.SKU + "|" + m.ToGoodsType, Text = m.SKU + "|" + m.ToGoodsType });
                ViewBag.upclist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.UPC, Text = m.UPC });
                ViewBag.batchnumberlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.BatchNumber, Text = m.BatchNumber });
                ViewBag.boxnumberlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.BoxNumber, Text = m.BoxNumber });
                //ViewBag.Unitlist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Unit });
                //ViewBag.Specificationslist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Specifications });
                ViewBag.togoodstypelist = vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.ToGoodsType, Text = m.ToGoodsType });
                ViewBag.Unitlist = UnitList;
                //vm.AdjustmentAndAdjustmentDetails.adjustmentDetails.Select(m => new SelectListItem() { Value = m.Unit });
                ViewBag.Specificationslist = SpecificationsList;
            }
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            if (customerID != null)
            {
                if (base.UserInfo.UserType == 0)
                {
                    vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = customerID;
                    }
                    else
                    {
                        var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                        if (customerIDs != null && customerIDs.Count() == 1)
                        {
                            vm.AdjustmentAndAdjustmentDetails.adjustment.CustomerID = customerIDs.First();
                        }
                    }
                }
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
            vm.GoodsTypes = wms.Select(c => new SelectListItem() { Value = c.Name, Text = c.Name });
            vm.ViewType = ViewType;
            vm.AdjustmentAndAdjustmentDetails.adjustment.AdjustmentTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
            return View(vm);
        }
        /// <summary>
        /// 修改库存批次
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="warehouseID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InventoryBatchChange(int? PageIndex, long? warehouseID, long customerID = 0)
        {

            IndexViewModel vm = new IndexViewModel();

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
            vm.InventorySearchCondition = new InventorySearchCondition();
            vm.InventorySearchCondition.InventoryType = 1;
            vm.InventorySearchCondition.Warehouse = warehouseID.ToString();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
            //                        .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;
            //ApplicationConfigHelper.GetApplicationCustomers()
            //                      .Select(c => new SelectListItem() { Value = c.Name, Text = c.Name });
            vm.InventorySearchCondition.CustomerID = customerID;

            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.InventorySearchCondition.CustomerID == 0)
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
                    vm.InventorySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.InventorySearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

            }

            ViewBag.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetWarehouseList()
            //                      .Select(c => new SelectListItem() { Value = c.WarehouseName, Text = c.WarehouseName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehouseID).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            //ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct()
            //                      .Select(c => new SelectListItem() { Value = c.AreaName, Text = c.AreaName });
            //var Areas = ApplicationConfigHelper.GetWarehouseLocationList().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            var Areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            ViewBag.Areas = Areas;
            if (warehouseID != null)
            {
                vm.InventorySearchCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == warehouseID.ToString()).First().Text + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    //sb.Append("'" + i.Value + "',");
                    sb.Append("'" + i.Text + "',");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }
            if (base.UserInfo.UserType == 0)
            {
                vm.InventorySearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
            {
                vm.InventorySearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID != 0)
                {
                    vm.InventorySearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.InventorySearchCondition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                    }
                }
            }
            ViewBag.AreaLists = Areas.Where(c => c.Value == vm.InventorySearchCondition.Warehouse).Select(c => new SelectListItem() { Value = c.Text.ToString(), Text = c.Text }); ;
            //var response = new InventoryManagementService().GetInventoryBySearchCondition(new GetInventoryBySearchConditionRequest()
            //{
            //    InventorySearchCondition = vm.InventorySearchCondition,
            //    PageSize = UtilConstants.PAGESIZE,
            //    PageIndex = PageIndex ?? 0
            //});
            //if (response.IsSuccess)
            //{
            //    vm.InventoryCollection = response.Result.InventoryCollection;
            //    vm.PageIndex = response.Result.PageIndex;
            //    vm.PageCount = response.Result.PageCount;
            //}
            if (WarehouseList.Count() == 1)
            {
                vm.InventorySearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault().ToString();
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }
        /// <summary>
        ///  修改库存批次
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="Action"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult InventoryBatchChange(IndexViewModel vm, string Action, int? PageIndex)
        {
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
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
            //                       .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;
            //ApplicationConfigHelper.GetApplicationCustomers()
            //.Select(m => new SelectListItem() { Value = m.Name, Text = m.Name });
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.InventorySearchCondition.CustomerID == 0)
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
                    vm.InventorySearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.InventorySearchCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.InventorySearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            if (WarehouseList.Count() == 1)
            {
                vm.InventorySearchCondition.Warehouse = WarehouseList.Select(c => c.Value).FirstOrDefault().ToString();
            }
            ViewBag.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetWarehouseList()
            //                    .Select(m => new SelectListItem() { Value = m.WarehouseName, Text = m.WarehouseName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(a => new { a.AreaName }).Distinct()
                                    .Select(m => new SelectListItem() { Value = m.AreaName, Text = m.AreaName });
            //var Areas = ApplicationConfigHelper.GetWarehouseLocationList().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            var Areas = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            ViewBag.Areas = Areas;
            var request = new GetInventoryBySearchConditionRequest();
            request.InventorySearchCondition = vm.InventorySearchCondition;

            if (vm.InventorySearchCondition.Warehouse != null)
            {
                request.InventorySearchCondition.Warehouse = "'" + WarehouseList.Where(a => a.Value == vm.InventorySearchCondition.Warehouse).First().Text + "'";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in WarehouseList)
                {
                    sb.Append("'" + i.Text + "',");
                }
                if (sb.Length > 1)
                {
                    vm.InventorySearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            request.PageIndex = PageIndex ?? 0;
            request.PageSize = UtilConstants.PAGESIZE;
            var ss = WarehouseList.Where(m => m.Text == vm.InventorySearchCondition.Warehouse.Replace("'", "")).Select(n => n.Value).FirstOrDefault();
            ViewBag.AreaLists = Areas.Where(c => c.Value == ss).Select(c => new SelectListItem() { Value = c.Text.ToString(), Text = c.Text });
            if (Action == "查询" || Action == "InventoryBatchChange")
            {
                var response = new InventoryManagementService().GetInventoryBySearchCondition(request);
                if (response.IsSuccess)
                {
                    vm.InventoryCollection = response.Result.InventoryCollection;
                    vm.PageIndex = response.Result.PageIndex;
                    vm.PageCount = response.Result.PageCount;
                }
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }
        /// <summary>
        /// 修改库存批次
        /// </summary>
        /// <param name="inventorys"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateInventChange(string inventorys)
        {
            var responseinventorys = jsonlist<Inventorys>(inventorys);
            IList<Inventorys> invents = new List<Inventorys>();
            AddInventroyRequest request = new AddInventroyRequest();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            if (responseinventorys == null || responseinventorys.Count() <= 0)
            {
                return "错误：没有需要修改的库存,请检查";
            }

            responseinventorys.Each((i, invent) =>
            {
                Inventorys inventory = new Inventorys()
                {
                    CustomerID = invent.CustomerID,
                    CustomerName = invent.CustomerName,
                    Warehouse = invent.Warehouse,
                    Area = invent.Area,
                    Location = invent.Location,
                    SKU = invent.SKU,
                    GoodsName = invent.GoodsName,
                    GoodsType = invent.GoodsType,
                    InventoryType = invent.InventoryType,
                    Qty = invent.Qty,
                    str1 = invent.BatchNumber,//原批次      
                    str2 = invent.Unit,//单位
                    str3 = invent.str3,//新的批次
                    str4 = invent.str4 == "" ? null : invent.str4,//新的生产日期                   
                    CreateTime = DateTime.Now,
                    Creator = UserInfo.Name
                };
                invents.Add(inventory);
                WMS_Log_Operation operation = new WMS_Log_Operation();

                operation.MenuName = "库存管理";
                operation.Operation = "库存批次修改";
                operation.OrderType = "InventoryBatchChange";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.CustomerID = (int)invent.CustomerID;
                operation.CustomerName = invent.CustomerName;
                operation.WarehouseName = invent.Warehouse;
                operation.OrderID = "";
                operation.Remark = invent.IDS;//修改的库存ID
                operation.Str1 = invent.BatchNumber;//原批次
                operation.Str2 = invent.str5;//原生产日期
                operation.Str3 = invent.str3;//新批次
                operation.Str4 = invent.str4;//新生产日期
                logs.Add(operation);
            });
            request.inventorys = invents;
            var respone = new InventoryManagementService().UpdateInventoryBatch(request);
            if (respone.IsSuccess)
            {
                new LogOperationService().AddLogOperation(logs);//写日志
            }

            return respone.Result;
        }
        /// <summary>
        /// 盘点单确认推送鲸仓
        /// </summary>
        /// <param name="IDS"></param>
        /// <returns></returns>
        public JsonResult submitAndSend(string AdjustmentNumber)
        {
            IList<JCAPiResponse> listResponses = new List<JCAPiResponse>();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            try
            {
                string url = UtilConstants.JCSendAPIAddress + "CheckSendAgain?outbizcode=" + AdjustmentNumber;
                string res = this.HTTPGet(url);
                listResponses = jsonlist<JCAPiResponse>(res);
                foreach (var item in listResponses)
                {
                    logs.Add(new WMS_Log_Operation()
                    {
                        MenuName = "库存变更管理",
                        Operation = "库存调整单-回传鲸仓",
                        OrderType = "CheckSendAgain",
                        Controller = Request.RawUrl,
                        Creator = base.UserInfo.Name,
                        CreateTime = DateTime.Now,
                        ProjectID = (int)base.UserInfo.ProjectID,
                        ProjectName = base.UserInfo.ProjectName,
                        OrderNumber = item.relatednumber,
                        Str1 = AdjustmentNumber,//请求报文
                        Str2 = item.message   //返回结果
                    });
                }
                new LogOperationService().AddLogOperation(logs);
            }
            catch (Exception ex)
            {
                logs.Add(new WMS_Log_Operation()
                {
                    MenuName = "库存变更管理",
                    Operation = "库存调整单-回传鲸仓",
                    OrderType = "CheckSendAgain",
                    Controller = Request.RawUrl,
                    Creator = base.UserInfo.Name,
                    CreateTime = DateTime.Now,
                    ProjectID = (int)base.UserInfo.ProjectID,
                    ProjectName = base.UserInfo.ProjectName,
                    OrderNumber = "",
                    Str1 = AdjustmentNumber,
                    Str2 = ex.Message
                });
                new LogOperationService().AddLogOperation(logs);
            }
            return Json(new { Result = listResponses });
        }
        /// <summary>
        /// POST提交
        /// </summary>
        private string HTTPGet(string url)
        {
            HttpWebRequest requests = (HttpWebRequest)WebRequest.Create(url);

            //GET请求
            requests.Method = "GET";
            requests.ReadWriteTimeout = 5000;
            requests.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)requests.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            //返回内容
            string retString = myStreamReader.ReadToEnd();
            return retString;
        }

    }
}
