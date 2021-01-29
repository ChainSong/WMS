using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Entity.WMS.PreOrders;
using Runbow.TWS.MessageContracts.WMS.PreOrders;
using Runbow.TWS.Web.Areas.WMS.Models.PreOrders;
using Runbow.TWS.Web.Common;
using MyFile = System.IO.File;
using UtilConstants = Runbow.TWS.Common.Constants;
using Runbow.TWS.Common;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IO;
using Runbow.TWS.Web.Interface;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.MessageContracts;
using System.Threading.Tasks;
using Runbow.TWS.Biz;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class PreOrderController : BaseController
    {

        [HttpGet]
        public ActionResult Index(bool? hideActionButton, bool? showEditRelated, long? customerID, bool? ShowSubmit)
        {
            PreOrderViewModel vm = new PreOrderViewModel();
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
            vm.OrderTypes = st;

            //取消单需要选择原因 20200326  没意思
            IEnumerable<WMSConfig> cancelList = null;
            try
            {
                cancelList = ApplicationConfigHelper.GetWMS_Config("OrderCancelReason_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            { }
            if (cancelList == null)
            {
                cancelList = ApplicationConfigHelper.GetWMS_Config("OrderCancelReason");
            }

            List<SelectListItem> cancelreasonlist = new List<SelectListItem>();
            if (cancelList != null)
            {
                cancelList.ToList().ForEach((item) =>
                {
                    cancelreasonlist.Add(new SelectListItem()
                    {
                        Value = item.Code,
                        Text = item.Name
                    });
                });
            }
            vm.OrderCancelReasonList = cancelreasonlist;

            vm.SearchCondition = new PreOrderSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                .Select(t => t.CustomerID);
            ViewBag.ShowSubmit = ShowSubmit;

            if (customerID == null)
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                StringBuilder sb = new StringBuilder();
                vm.CustomerNames = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                foreach (var i in vm.CustomerNames)
                {
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.SearchCondition.CustomerIDs = "0";
                }
                vm.SearchCondition.CustomerID = Convert.ToInt64(vm.CustomerNames.First().Value);
                vm.SearchCondition.CustomerName = vm.CustomerNames.First().Text;
                //vm.SearchCondition.WarehouseId = long.Parse(vm.Warehouses.Select(c => c.Value).FirstOrDefault().ToString());
                //vm.SearchCondition.Warehouse = vm.Warehouses.Select(c => c.Text).FirstOrDefault().ToString();
            }
            else
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;

            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    var CustomerNames = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                    vm.SearchCondition.CustomerID = customerID;
                    vm.SearchCondition.CustomerName = CustomerNames.Where(c=>c.Value==customerID.ToString()).FirstOrDefault().Text;
                }
                else
                {
                    if (vm.CustomerNames != null)
                    {
                        vm.SearchCondition.CustomerID = Convert.ToInt64(vm.CustomerNames.First().Value);
                        vm.SearchCondition.CustomerName = vm.CustomerNames.First().Text;
                    }
                }
            }

            vm.SearchCondition.CreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd HH:mm:ss"));
            vm.SearchCondition.EndCreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ObjectToNullableDateTime();

            var response = new PreOrderService().GetPreOrder(new PreOrderRequest
            {
                SearchCondition = vm.SearchCondition,
                PageIndex = 0,
                PageSize = UtilConstants.PAGESIZE
            });

            if (response.IsSuccess)
            {
                vm.SearchConditionResponse = response.Result.SearchCondition;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }

            this.GenQueryPodViewModel(vm);
            
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }
        [HttpPost]
        public ActionResult CustomAllocation(PreOrderViewModel vm, int PageIndex, string Action)
        {
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
            vm.OrderTypes = st;
            //PreOrderViewModel vm = new PreOrderViewModel();
            //vm.SearchCondition = new PreOrderSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            //vm.HideActionButton = hideActionButton ??  false;
            //vm.ShowEditRelated = showEditRelated ?? true;
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            vm.CustomerNames = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            if (vm.SearchCondition.CustomerID == null)
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            else
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                       .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                       .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            if (vm.SearchCondition.Warehouse == null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var i in vm.Warehouses)
                {
                    sb.Append("'" + i.Value + "',");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            else
            {
                vm.SearchCondition.Warehouse = "'" + vm.SearchCondition.Warehouse + "'";
            }
            //StringBuilder sb = new StringBuilder();
            if (vm.SearchCondition.CustomerID == 0)
            {
                StringBuilder sb = new StringBuilder();
                vm.CustomerNames = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.CustomerID == vm.SearchCondition.CustomerID && t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                foreach (var i in vm.CustomerNames)
                {
                    sb.Append("'" + i.Value + "',");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }


            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }

            var response = new PreOrderService().GetPreOrder(new PreOrderRequest
            {
                SearchCondition = vm.SearchCondition,
                PageIndex = PageIndex,
                PageSize = UtilConstants.PAGESIZE

            });
            if (Action == "导出")
            {
                var responses = new PreOrderService().GetPreOrderExecl(new PreOrderRequest
                {
                    SearchCondition = vm.SearchCondition,
                    PageIndex = PageIndex,
                    PageSize = UtilConstants.PAGESIZE

                });
                ExportExecl(vm, responses.Result.SearchCondition, responses.Result.PreOd);
                //return ExportDataToExcelHelper.ExportDataTableToExcel(podAlls);

            }
            if (response.IsSuccess)
            {
                vm.SearchConditionResponse = response.Result.SearchCondition;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }

            this.GenQueryPodViewModel(vm);
            if (vm.Warehouses.Count() == 1)
            {
                vm.SearchCondition.Warehouse = vm.Warehouses.Select(c => c.Text).FirstOrDefault().ToString();
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }
        public ActionResult CustomAllocation(bool? hideActionButton, bool? showEditRelated, long? customerID, bool? ShowSubmit)
        {
            PreOrderViewModel vm = new PreOrderViewModel();
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
            vm.OrderTypes = st;
            vm.SearchCondition = new PreOrderSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            //vm.HideActionButton = hideActionButton ??  false;
            //vm.ShowEditRelated = showEditRelated ?? true;
            ViewBag.ShowSubmit = ShowSubmit;
            if (customerID == null)
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            else
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            vm.CustomerNames = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            if (vm.Warehouses != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var i in vm.Warehouses)
                {
                    sb.Append("'" + i.Value + "',");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            if (customerID == null)
            {
                StringBuilder sb = new StringBuilder();
                vm.CustomerNames = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                foreach (var i in vm.CustomerNames)
                {
                    sb.Append("" + i.Value + ",");
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

            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
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
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                    }
                }
            }
            vm.SearchCondition.CreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
            vm.SearchCondition.EndCreateTime = DateTime.Now.ToString("yyyy-MM-dd").ObjectToNullableDateTime();
            var response = new PreOrderService().GetPreOrder(new PreOrderRequest
            {
                SearchCondition = vm.SearchCondition,
                PageIndex = 0,
                PageSize = UtilConstants.PAGESIZE

            });
            if (response.IsSuccess)
            {
                vm.SearchConditionResponse = response.Result.SearchCondition;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }

            this.GenQueryPodViewModel(vm);
            if (vm.Warehouses.Count() == 1)
            {
                vm.SearchCondition.Warehouse = vm.Warehouses.Select(c => c.Text).FirstOrDefault().ToString();
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }
        [HttpPost]
        public ActionResult Index(PreOrderViewModel vm, int PageIndex, string Action)
        {
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
            vm.OrderTypes = st;


            //取消单需要选择原因 20200326  没意思
            IEnumerable<WMSConfig> cancelList = null;
            try
            {
                cancelList = ApplicationConfigHelper.GetWMS_Config("OrderCancelReason_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {

            }
            if (cancelList == null)
            {
                cancelList = ApplicationConfigHelper.GetWMS_Config("OrderCancelReason");
            }
            List<SelectListItem> cancelreasonlist = new List<SelectListItem>();
            if (cancelList != null)
            {
                cancelList.ToList().ForEach((item) =>
                {
                    cancelreasonlist.Add(new SelectListItem()
                    {
                        Value = item.Code,
                        Text = item.Name
                    });
                });
            }
            vm.OrderCancelReasonList = cancelreasonlist;

            //PreOrderViewModel vm = new PreOrderViewModel();
            //vm.SearchCondition = new PreOrderSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            //vm.HideActionButton = hideActionButton ??  false;
            //vm.ShowEditRelated = showEditRelated ?? true;
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            vm.CustomerNames = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            if (vm.SearchCondition.CustomerID == null)
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            else
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                       .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                       .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            if (vm.SearchCondition.WarehouseId == null)
            {
                foreach (var i in vm.Warehouses)
                {
                    if (i.Value == vm.SearchCondition.Warehouse)
                    {
                        vm.SearchCondition.WarehouseId = long.Parse(i.Value);
                        vm.SearchCondition.Warehouse = i.Text;
                    }
                }
            }

            if (vm.SearchCondition.CustomerID == null)
            {
                foreach (var i in vm.CustomerNames)
                {
                    if (i.Value == vm.SearchCondition.CustomerName)
                    {
                        vm.SearchCondition.CustomerID = long.Parse(i.Value);
                        vm.SearchCondition.CustomerName = i.Text;
                    }
                }
            }

            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }

            var response = new PreOrderService().GetPreOrder(new PreOrderRequest
            {
                SearchCondition = vm.SearchCondition,
                PageIndex = PageIndex,
                PageSize = UtilConstants.PAGESIZE

            });
            if (Action == "导出")
            {
                var responses = new PreOrderService().GetPreOrderExecl(new PreOrderRequest
                {
                    SearchCondition = vm.SearchCondition,
                    PageIndex = PageIndex,
                    PageSize = UtilConstants.PAGESIZE

                });
                ExportExecl(vm, responses.Result.SearchCondition, responses.Result.PreOd);
                //return ExportDataToExcelHelper.ExportDataTableToExcel(podAlls);

            }
            if (response.IsSuccess)
            {
                vm.SearchConditionResponse = response.Result.SearchCondition;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }

            this.GenQueryPodViewModel(vm);
            if (vm.Warehouses.Count() == 1)
            {
                vm.SearchCondition.Warehouse = vm.Warehouses.Select(c => c.Text).FirstOrDefault().ToString();
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }

        /// <summary>
        /// 勾选导出
        /// </summary>
        /// <param name="IDS"></param>
        /// <param name="CustomerID"></param>
        public void ExportPreOrder(string IDS, long CustomerID)
        {
            var getPreorderResponse = new PreOrderService().GetPreOrderByIDs(IDS);//根据ID查询

            IEnumerable<Column> columnReceipt;
            IEnumerable<Column> columnReceiptDetail;
            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_PreOrder").Count() == 0)
            {
                columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrder").ColumnCollection;
            }
            else
            {
                columnReceipt = module.Tables.TableCollection.First(m => m.Name == "WMS_PreOrder").ColumnCollection;
            }
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_PreOrderDetail").Count() == 0)
            {
                columnReceiptDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
            }
            else
            {
                columnReceiptDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
            }
            if (CustomerID == 0)
            {
                columnReceipt = columnReceipt.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
            }
            else
            {
                var notKeyColumns1 = columnReceipt.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID));
                var notKeyColumns2 = columnReceiptDetail.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID));
                columnReceipt = columnReceipt.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true && c.DbColumnName != "ReceiptType" && c.DbColumnName != "Status")).Union(notKeyColumns1.Where(c => c.IsShowInList));
                columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true)).Union(notKeyColumns2.Where(c => c.IsShowInList));
            }
            //导出
            Export(getPreorderResponse.Result.SearchCondition, getPreorderResponse.Result.PreOd, columnReceipt, columnReceiptDetail);
        }

        private void ExportExecl(PreOrderViewModel vm, IEnumerable<PreOrderSearchCondition> SearchCondition, IEnumerable<PreOrderDetail> PreOd)
        {
            IEnumerable<Column> columnReceipt;
            IEnumerable<Column> columnReceiptDetail;
            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_PreOrder").Count() == 0)
            {
                columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrder").ColumnCollection;
            }
            else
            {
                columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_PreOrder").ColumnCollection;
            }
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_PreOrderDetail").Count() == 0)
            {
                columnReceiptDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
            }
            else
            {
                columnReceiptDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
            }

            //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
            //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            //IEnumerable<Table> tables = module.Tables.TableCollection;
            //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_PreOrder").ColumnCollection;
            //IEnumerable<Column> columnReceiptDetail = tables.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
            if (vm.SearchCondition.CustomerID == 0)
            {
                columnReceipt = columnReceipt.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
            }
            else
            {
                var notKeyColumns1 = columnReceipt.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.SearchCondition.CustomerID));
                //  .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID)))
                //.Select(c =>
                //{
                //    if (c.InnerColumns.Count == 0)
                //    {
                //        return c;
                //    }
                //    else
                //    {
                //        if (c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID))
                //        {
                //            return c.InnerColumns.First(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID);
                //        }

                //        return c;
                //    }
                //});
                var notKeyColumns2 = columnReceiptDetail.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.SearchCondition.CustomerID));
                //  .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID)))
                //.Select(c =>
                //{
                //    if (c.InnerColumns.Count == 0)
                //    {
                //        return c;
                //    }
                //    else
                //    {
                //        if (c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID))
                //        {
                //            return c.InnerColumns.First(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID);
                //        }

                //        return c;
                //    }
                //});
                columnReceipt = columnReceipt.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true && c.DbColumnName != "ReceiptType" && c.DbColumnName != "Status")).Union(notKeyColumns1.Where(c => c.IsShowInList));
                columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true)).Union(notKeyColumns2.Where(c => c.IsShowInList));
            }

            Export(SearchCondition, PreOd, columnReceipt, columnReceiptDetail);


        }
        //导出
        private void Export(IEnumerable<PreOrderSearchCondition> receipts, IEnumerable<PreOrderDetail> PreOd, IEnumerable<Column> columnReceipt, IEnumerable<Column> columnReceiptDetail)
        {
            //IEnumerable<Receipt> receipts = response.ReceiptCollection;
            IEnumerable<PreOrderSearchCondition> receiptDetails = receipts;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            DataTable dtReceiptDetail = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                //if (receipt.DisplayName != "客户" && receipt.DisplayName != "预出库单号")
                //{
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
                //}
            }
            foreach (var receiptDetail in columnReceiptDetail)
            {
                if (receiptDetail.DisplayName != "仓库编号")
                {
                    dtReceiptDetail.Columns.Add(receiptDetail.DisplayName, typeof(string));
                }
            }
            receipts.Each((i, s) =>
            {
                DataRow drReceipt = dtReceipt.NewRow();
                foreach (var receipt in columnReceipt)
                {

                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.PreOrderSearchCondition).GetProperty(receipt.DbColumnName).GetValue(s);
                }
                dtReceipt.Rows.Add(drReceipt);
            });

            PreOd.Each((i, s) =>
            {
                DataRow drReceipt = dtReceiptDetail.NewRow();
                foreach (var receiptDetail in columnReceiptDetail)
                {
                    if (receiptDetail.DisplayName != "仓库编号")
                    {
                        drReceipt[receiptDetail.DisplayName] = typeof(Runbow.TWS.Entity.PreOrderDetail).GetProperty(receiptDetail.DbColumnName).GetValue(s);
                    }
                }
                dtReceiptDetail.Rows.Add(drReceipt);
            });


            dtReceipt.TableName = "预出库单主信息";
            dtReceiptDetail.TableName = "预出库单明细信息";
            ds.Tables.Add(dtReceipt);
            ds.Tables.Add(dtReceiptDetail);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "预出库单" + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "预出库单" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        [HttpPost]  //导入模板
        public string ImputEcecl(string CustomerName, long CustomerID, string WarehouseName, long WarehouseID)
        {
            PreOrderViewModel vm = new PreOrderViewModel();

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    if (ds == null)
                    {
                        return new { result = "<h3><font color='#FF0000'>Excel格式有误</font></h3>", IsSuccess = false }.ToJsonString();
                    }
                    string s = "";
                    try
                    {
                        TransData(CustomerID, WarehouseID, ref ds, ref s);
                    }
                    catch (Exception e)
                    {
                        s = e.Message;
                    }
                    if (s != "")
                    {
                        return new { result = "<h3><font color='#FF0000'>" + s + "</font></h3>", IsSuccess = false }.ToJsonString();//<font color='#FF0000'> </font>
                    }
                    if (ds == null)
                    {
                        return new { result = "<h3><font color='#FF0000'>Excel格式有误</font></h3>", IsSuccess = false }.ToJsonString();
                    }
                    DataTable dtpo = new DataTable();
                    DataTable dtpodetail = new DataTable();
                    WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == ApplicationConfigHelper.GetCacheInfo().First(c => c.UserName == base.UserInfo.Name).WarehouseID).ToArray()[0];//Bob
                    dtpo = ds.Tables["预出库单主信息"];
                    dtpodetail = ds.Tables["预出库单明细信息"];
                    IEnumerable<Column> columnpo;
                    IEnumerable<Column> columnpoDetail;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_PreOrder").Count() == 0)
                    {
                        columnpo = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrder").ColumnCollection;
                    }
                    else
                    {
                        columnpo = module.Tables.TableCollection.First(t => t.Name == "WMS_PreOrder").ColumnCollection;
                    }
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_PreOrderDetail").Count() == 0)
                    {
                        columnpoDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
                    }
                    else
                    {
                        columnpoDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
                    }

                    var UnitAndSpecificationsList = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(base.UserInfo.ProjectID, CustomerID, 0);
                    var UnitList = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Unit }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
                    var UnitAndSpecificationsLists = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name });
                    var SpecificationsList = UnitAndSpecificationsList.Select(t => new { Code = t.Specifications, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
                    bool useCustomerOrderNumber = module.UseCustomerOrderNumber;
                    StringBuilder sb = new StringBuilder();
                    IEnumerable<PreOrder> pos = this.InitPoFromDataTable(dtpo, columnpo, useCustomerOrderNumber, sb);
                    for (int i = 0; i < dtpodetail.Rows.Count; i++)
                    {
                        if (!Regex.IsMatch(dtpodetail.Rows[i]["期望数量"].ToString(), @"^[-]?\d+[.]?\d*$"))
                        {
                            return new { result = "<h3><font color='#FF0000'>" + dtpodetail.Rows[i]["期望数量"].ToString() + "不是数字</font></h3>", IsSuccess = false }.ToJsonString();
                        }
                        if (UnitList.Where(c => c.Value == dtpodetail.Rows[i]["单位"].ToString()).Count() == 0)
                        {
                            return new { result = "<h3>单位" + "<font color='#FF0000'>" + dtpodetail.Rows[i]["单位"].ToString() + "</font>" + "不存在,请配置！</h3>", IsSuccess = false }.ToJsonString();
                        }
                        if (SpecificationsList.Where(c => c.Value == dtpodetail.Rows[i]["规格"].ToString()).Count() == 0)
                        {
                            return new { result = "<h3>规格" + "<font color='#FF0000'>" + dtpodetail.Rows[i]["规格"].ToString() + "</font>" + "不存在,请配置！</h3>", IsSuccess = false }.ToJsonString();
                        }
                        if (UnitAndSpecificationsLists.Where(c => c.Value == dtpodetail.Rows[i]["单位"].ToString() && c.Text == dtpodetail.Rows[i]["规格"].ToString()).Count() == 0)
                        {
                            return new { result = "<h3>单位" + "<font color='#FF0000'>" + dtpodetail.Rows[i]["单位"].ToString() + "</font>" + "与规格" + "<font color='#FF0000'>" + dtpodetail.Rows[i]["规格"].ToString() + "</font>" + "不匹配,请修改！</h3>", IsSuccess = false }.ToJsonString();
                        }
                    }

                    IEnumerable<PreOrderDetail> poDetailss = this.InitPoDetailFromDataTable(dtpodetail, columnpoDetail, useCustomerOrderNumber, sb);
                    List<ProductSearch> productListS = new List<ProductSearch>();
                    IEnumerable<ProductSearch> productList;
                    bool IsInt1 = false;
                    List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                    #region
                    IEnumerable<WMS_Config_Type> ctype = null;
                    ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", 0, 0, 0);
                    ctype = ctype.Where(c => c.Code == "AKZO" && c.Name == CustomerID.ToString());
                    #endregion
                    pos.Each((e, h) =>
                    {
                        //if (warehouse != null)
                        //{
                        //    h.str2 = warehouse.Remark;
                        //}

                        h.Creator = base.UserInfo.Name;
                        h.CreateTime = DateTime.Now;
                        h.CustomerID = CustomerID;
                        h.WarehouseId = WarehouseID;
                        h.Status = 1;
                        h.DetailCount = poDetailss.Where(a => a.ExternOrderNumber == h.ExternOrderNumber).Count();
                        h.CustomerName = CustomerName;
                        if (ctype != null && ctype.Count() > 0 && h.OrderType != "调整出库")
                        {
                            IsInt1 = true;
                            h.Int1 = 1;
                            h.Int2 = 1;
                        }
                        WMS_Log_Operation operation = new WMS_Log_Operation();
                        operation.MenuName = "预出库管理";
                        operation.Operation = "预出库-导入";
                        operation.OrderType = "PreOrder";
                        operation.Controller = Request.RawUrl;
                        operation.Creator = base.UserInfo.Name;
                        operation.CreateTime = DateTime.Now;
                        operation.ProjectID = (int)base.UserInfo.ProjectID;
                        operation.ProjectName = base.UserInfo.ProjectName;
                        operation.CustomerID = (int)h.CustomerID;
                        operation.CustomerName = h.CustomerName;
                        operation.WarehouseID = (int)h.WarehouseId;
                        operation.WarehouseName = h.Warehouse;
                        operation.ExternOrderNumber = h.ExternOrderNumber;
                        logs.Add(operation);
                    });
                    poDetailss.Each((e, h) =>
                    {
                        h.Creator = base.UserInfo.Name;
                        h.CreateTime = DateTime.Now;
                        h.CustomerID = CustomerID;
                        h.CustomerName = CustomerName;
                        ProductSearch ps = new ProductSearch();
                        ps.SKU = h.SKU;
                        productListS.Add(ps);
                    });
                    productList = ApplicationConfigHelper.GetSearchProduct(CustomerID, productListS, "SKU");

                    List<string> ExternOrderNumber = new List<string>();
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
                    foreach (var po in pos)
                    {
                        if (po.OrderTime == null)
                        {
                            po.OrderTime = DateTime.Now.Date;
                        }
                        var countAsn = new ASNManagementService().ExternKeyCheck(po.ExternOrderNumber, "2", CustomerID);
                        if (countAsn > 0)
                        {
                            return new { result = "<h3><font color='#FF0000'>外部单号" + po.ExternOrderNumber + "已存在!</font></h3>", IsSuccess = false }.ToJsonString();
                        }
                        int Rows_count = 1;

                        if (ExternOrderNumber.Contains(po.ExternOrderNumber))
                        {
                            return new { result = "<h3><font color='#FF0000'>EXCEL中存在相同的外部单号" + po.ExternOrderNumber + "!</font></h3>", IsSuccess = false }.ToJsonString();
                        }
                        ExternOrderNumber.Add(po.ExternOrderNumber);
                        if (ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(m => m.WarehouseName == po.Warehouse).Count() == 0)
                        {
                            return new { result = "<h3><font color='#FF0000'>第【" + Rows_count + "】行的仓库名称【" + po.Warehouse + "】在系统中不存在！</font></h3>", IsSuccess = false }.ToJsonString();
                        }
                        if (ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.Name == po.CustomerName).Count() == 0)
                        {
                            return new { result = "<h3><font color='#FF0000'>第【" + Rows_count + "】行的客户名称【" + po.CustomerName + "】在系统不存在！</font></h3>", IsSuccess = false }.ToJsonString();
                        }

                        if (wms.Where(m => m.Name == po.OrderType).Count() == 0)
                        {
                            return new { result = "<h3><font color='#FF0000'>第【" + Rows_count + "】行的预出库单类型【" + po.OrderType + "】在系统中不存在！</font></h3>", IsSuccess = false }.ToJsonString();
                        }
                        Rows_count++;
                    }

                    foreach (var podetails in poDetailss)
                    {
                        int Rows_counts = 1;
                        if (productList.Where(c => (c.SKU == podetails.SKU)).Count() == 0)
                        {
                            return new { result = "<h3><font color='#FF0000'>第【" + Rows_counts + "】行的SKU【" + podetails.SKU + "】在系统中不存在！</font></h3>", IsSuccess = false }.ToJsonString();
                        }
                        Rows_counts++;
                        if (ctype != null && ctype.Count() > 0 && IsInt1)
                        {
                            podetails.Int1 = 1;
                            podetails.Int2 = 1;
                        }
                    }
                    #region
                    int index = 0;
                    var poDetailsdate = from q in poDetailss
                                        group q by new { q.ExternOrderNumber, q.CustomerName, q.Warehouse, q.SKU, q.UPC, q.GoodsType, q.BatchNumber, q.BoxNumber, q.Unit, q.Specifications, q.LineNumber, q.Area, q.Location } into r
                                        select new PreOrderDetail
                                        {
                                            ID = r.Sum(a => a.ID),
                                            POID = r.Sum(a => a.POID),
                                            ExternOrderNumber = r.Key.ExternOrderNumber,
                                            CustomerID = r.Max(a => a.CustomerID),
                                            CustomerName = r.Key.CustomerName,
                                            LineNumber = ReturnLineNumber(index++),
                                            WarehouseId = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(a => a.WarehouseName == r.Key.Warehouse).Select(a => a.WarehouseID).FirstOrDefault(),
                                            Warehouse = r.Key.Warehouse,
                                            Area = r.Key.Area,
                                            Location = r.Key.Location,
                                            SKU = r.Key.SKU,
                                            BoxNumber = r.Key.BoxNumber,
                                            GoodsName = r.Max(a => a.GoodsName),
                                            GoodsType = r.Key.GoodsType,
                                            UPC = r.Key.UPC,
                                            InventoryQty = r.Sum(a => a.InventoryQty),
                                            OriginalQty = r.Sum(a => a.OriginalQty),
                                            AllocatedQty = r.Sum(a => a.AllocatedQty),
                                            BatchNumber = r.Key.BatchNumber,
                                            Unit = r.Key.Unit,
                                            Specifications = r.Key.Specifications,
                                            Creator = r.Max(a => a.Creator),
                                            CreateTime = r.Max(a => a.CreateTime),
                                            Updator = r.Max(a => a.Updator),
                                            UpdateTime = r.Max(a => a.UpdateTime),
                                            Remark = r.Max(a => a.Remark),
                                            str1 = r.Max(a => a.str1),
                                            str2 = r.Max(a => a.str2),
                                            str3 = r.Max(a => a.str3),
                                            str4 = r.Max(a => a.str4),
                                            str5 = r.Max(a => a.str5),
                                            str6 = r.Max(a => a.str6),
                                            str7 = r.Max(a => a.str7),
                                            str8 = r.Max(a => a.str8),
                                            str9 = r.Max(a => a.str9),
                                            str10 = r.Max(a => a.str10),
                                            str11 = r.Max(a => a.str11),
                                            str12 = r.Max(a => a.str12),
                                            str13 = r.Max(a => a.str13),
                                            str14 = r.Max(a => a.str14),
                                            str15 = r.Max(a => a.str15),
                                            str16 = r.Max(a => a.str16),
                                            str17 = r.Max(a => a.str17),
                                            str18 = r.Max(a => a.str18),
                                            str19 = r.Max(a => a.str19),
                                            str20 = r.Max(a => a.str20),
                                            DateTime1 = r.Max(a => a.DateTime1),
                                            DateTime2 = r.Max(a => a.DateTime2),
                                            DateTime3 = r.Max(a => a.DateTime3),
                                            DateTime4 = r.Max(a => a.DateTime4),
                                            DateTime5 = r.Max(a => a.DateTime5),
                                            Int1 = r.Max(a => a.Int1),
                                            Int2 = r.Max(a => a.Int2),
                                            Int3 = r.Max(a => a.Int3),
                                            Int4 = r.Max(a => a.Int4),
                                            Int5 = r.Max(a => a.Int5)
                                        };
                    #endregion
                    PreOrderRequest request = new PreOrderRequest();
                    request.PreOrderList = pos;
                    request.PreOd = poDetailsdate;

                    var response = new PreOrderService().AddPreOrderAndPreOrderDetail(request, base.UserInfo.Name);
                    if (response.IsSuccess)
                    {
                        StringBuilder returnSb = new StringBuilder();
                        response.Result.PreOrderList.OrderBy(a => a.Status).Each((b, h) =>
                        {
                            if (h.Status.Value <= 3)
                            {
                                new LogOperationService().AddLogOperation(logs);
                                returnSb.Append("<h3><font color='#33cc70'>" + h.ExternOrderNumber + " 导入成功！</font></h3>");
                            }
                            else
                            {
                                returnSb.Append("<h3><font color='#FF0000'>" + h.ExternOrderNumber + " 导入失败！</font></h3>");
                            }
                        });
                        return new { result = returnSb.ToString(), IsSuccess = false }.ToJsonString();
                    }
                    else
                    {
                        return new { result = "<h3><font color='#FF0000'>批量导入失败！</font></h3>" + response.Result.ToString(), IsSuccess = false }.ToJsonString();
                    }

                }
            }
            return new { result = "<h3>失败</h3>", IsSuccess = false }.ToJsonString();
        }

        private async static Task<string> AddLogOperation(List<WMS_Log_Operation> logs)
        {
            string message = await Task.Run(() =>
            {
                LogOperationService service = new LogOperationService();
                var response = service.AddLogOperation(logs);
                return response.Result;
            }).ConfigureAwait(false);
            return message;
        }

        private string ReturnLineNumber(int row_count)
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
        public JsonResult CheckOutboundOrder(string Id)
        {
            var response = new PreOrderService().CheckOutboundOrder(Id);
            if (response.IsSuccess)
            {
                if (response.Result.OrderInfo.Count() > 0)
                {
                    return Json(new { ErrorCode = "1", OrderInfo = response.Result.OrderInfo });
                }
                else
                {
                    return Json(new { ErrorCode = "0" });
                }


            }
            return Json(new { ErrorCode = "0" });
        }
        private IEnumerable<PreOrderDetail> InitPoDetailFromDataTable(DataTable dt, IEnumerable<Column> columnsConfig, bool useCustomerOrderNumber, StringBuilder sb)
        {
            IList<PreOrderDetail> AsnDetails = new List<PreOrderDetail>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PreOrderDetail asnDetail = new PreOrderDetail();

                string value;

                foreach (var column in columnsConfig.Where(c => c.IsKey))
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (column.Type == "DateTime" || column.Type == "DateTimeWithTime")
                                {
                                    DateTime dttemp;
                                    if (DateTime.TryParse(value, out dttemp))
                                    {
                                        typeof(PreOrderDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, dttemp, null);
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是日期格式，请修改。<br/>");
                                    }
                                }
                                else if (column.Type == "CheckBox" || (column.Type == "DropDownList" && !column.IsKey))
                                {
                                    if (value == "1" || value == "0" || value == "是" || value == "否" || value == "Y" || value == "y" || value == "N" || value == "n")
                                    {
                                        if (value == "1" || value == "是" || value == "Y" || value == "y")
                                        {
                                            typeof(PreOrderDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, "1", null);
                                        }
                                        else
                                        {
                                            typeof(PreOrderDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    var propertyInfoTemp = typeof(PreOrderDetail).GetProperty(column.DbColumnName);
                                    try
                                    {
                                        propertyInfoTemp.SetValue(asnDetail, Convert.ChangeType(value, propertyInfoTemp.PropertyType), null);
                                    }
                                    catch
                                    {
                                        propertyInfoTemp.SetValue(asnDetail, null);
                                    }
                                }

                                break;
                            }

                            break;
                        }
                    }
                }

                foreach (var column in columnsConfig.Where(c => c.IsKey == false && c.IsHide == false && c.ForView == true))
                {
                    //foreach (var column in col.InnerColumns)
                    //{
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            //value = dt.Rows[i][j].ToString().Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (column.Type == "DateTime" || column.Type == "DateTimeWithTime")
                                {
                                    DateTime dttemp;
                                    if (DateTime.TryParse(value, out dttemp))
                                    {
                                        typeof(PreOrderDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, dttemp, null);
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是日期格式，请修改。<br/>");
                                    }
                                }
                                else if (column.Type == "CheckBox" || (column.Type == "DropDownList" && !column.IsKey))
                                {
                                    if (value == "1" || value == "0" || value == "是" || value == "否" || value == "Y" || value == "y" || value == "N" || value == "n")
                                    {
                                        if (value == "1" || value == "是" || value == "Y" || value == "y")
                                        {
                                            typeof(PreOrderDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, "1", null);
                                        }
                                        else
                                        {
                                            typeof(PreOrderDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    var propertyInfoTemp = typeof(PreOrderDetail).GetProperty(column.DbColumnName);
                                    try
                                    {
                                        propertyInfoTemp.SetValue(asnDetail, Convert.ChangeType(value, propertyInfoTemp.PropertyType), null);
                                    }
                                    catch
                                    {
                                        propertyInfoTemp.SetValue(asnDetail, null);
                                    }
                                }
                                break;
                            }
                            break;
                        }
                    }
                    //}
                }
                AsnDetails.Add(asnDetail);
            }

            return AsnDetails;
        }

        private IEnumerable<PreOrder> InitPoFromDataTable(DataTable dt, IEnumerable<Column> columnsConfig, bool useCustomerOrderNumber, StringBuilder sb)
        {
            IList<PreOrder> pos = new List<PreOrder>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PreOrder po = new PreOrder();

                string value;

                foreach (var column in columnsConfig.Where(c => c.IsKey))
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (column.Type == "DateTime" || column.Type == "DateTimeWithTime")
                                {
                                    DateTime dttemp;
                                    if (DateTime.TryParse(value, out dttemp))
                                    {
                                        typeof(PreOrder).GetProperty(column.DbColumnName).SetValue(po, dttemp, null);
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是日期格式，请修改。<br/>");
                                    }
                                }
                                else if (column.Type == "CheckBox" || (column.Type == "DropDownList" && !column.IsKey))
                                {
                                    if (value == "1" || value == "0" || value == "是" || value == "否" || value == "Y" || value == "y" || value == "N" || value == "n")
                                    {
                                        if (value == "1" || value == "是" || value == "Y" || value == "y")
                                        {
                                            typeof(PreOrder).GetProperty(column.DbColumnName).SetValue(po, "1", null);
                                        }
                                        else
                                        {
                                            typeof(PreOrder).GetProperty(column.DbColumnName).SetValue(po, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    var propertyInfoTemp = typeof(PreOrder).GetProperty(column.DbColumnName);
                                    try
                                    {
                                        propertyInfoTemp.SetValue(po, Convert.ChangeType(value, propertyInfoTemp.PropertyType), null);
                                    }
                                    catch
                                    {
                                        propertyInfoTemp.SetValue(po, null);
                                    }
                                }

                                break;
                            }

                            break;
                        }
                    }
                }
                foreach (var column in columnsConfig.Where(c => c.IsKey == false && c.IsHide == false && c.ForView == true))
                {
                    //foreach (var column in col.InnerColumns)
                    //{
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            //value = dt.Rows[i][j].ToString().Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (column.Type == "DateTime" || column.Type == "DateTimeWithTime")
                                {
                                    DateTime dttemp;
                                    if (DateTime.TryParse(value, out dttemp))
                                    {
                                        typeof(PreOrder).GetProperty(column.DbColumnName).SetValue(po, dttemp, null);
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是日期格式，请修改。<br/>");
                                    }
                                }
                                else if (column.Type == "CheckBox" || (column.Type == "DropDownList" && !column.IsKey))
                                {
                                    if (value == "1" || value == "0" || value == "是" || value == "否" || value == "Y" || value == "y" || value == "N" || value == "n")
                                    {
                                        if (value == "1" || value == "是" || value == "Y" || value == "y")
                                        {
                                            typeof(PreOrder).GetProperty(column.DbColumnName).SetValue(po, "1", null);
                                        }
                                        else
                                        {
                                            typeof(PreOrder).GetProperty(column.DbColumnName).SetValue(po, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    var propertyInfoTemp = typeof(PreOrder).GetProperty(column.DbColumnName);
                                    try
                                    {
                                        propertyInfoTemp.SetValue(po, Convert.ChangeType(value, propertyInfoTemp.PropertyType), null);
                                    }
                                    catch
                                    {
                                        propertyInfoTemp.SetValue(po, null);
                                    }
                                }
                                break;
                            }
                            break;
                        }
                    }
                    //}
                }
                pos.Add(po);
            }
            return pos;
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
        private void GenQueryPodViewModel(PreOrderViewModel vm)
        {
            var Configs = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection;
            if (Configs.Where(t => t.Name == "WMS_PreOrder").Count() == 0)
            {
                vm.Config1 = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrder");
            }
            else
            {
                vm.Config1 = Configs.First(t => t.Name == "WMS_PreOrder");
            }
            if (Configs.Where(t => t.Name == "WMS_PreOrderDetail").Count() == 0)
            {
                vm.Config2 = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail");
            }
            else
            {
                vm.Config2 = Configs.First(t => t.Name == "WMS_PreOrderDetail");
            }

            //vm.Config1 = (((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID,vm.SearchCondition.CustomerID)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrder"));
            //vm.Config2 = (((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail"));

            if (base.UserInfo.UserType == 2)
            {
                vm.CustomerNames = vm.CustomerNames = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            }
            else if (base.UserInfo.UserType == 0)
            {
                vm.CustomerNames = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            }
            else
            {
                vm.CustomerNames = Enumerable.Empty<SelectListItem>();
            }
        }
        // 0查看 1新增 2编辑
        [HttpGet]
        public ActionResult PreOrderCreateOrEdit(string ID, bool? hideActionButton, bool? showEditRelated, long? customerID, long? WarehouseID, int ViewType = 0, int backFlag = 0, int Flag = 0)
        {
            PreOrderViewModel vm = new PreOrderViewModel();
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
            vm.OrderTypes = st;
            #region 产品中心赋值
            IEnumerable<WMSConfig> wms_product_center = null;
            try
            {
                wms_product_center = ApplicationConfigHelper.GetWMS_Config("ProductCenter_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }

            if (wms_product_center == null)
            {
                wms_product_center = ApplicationConfigHelper.GetWMS_Config("ProductCenter");
            }
            List<SelectListItem> st_product_center = new List<SelectListItem>();
            foreach (WMSConfig w in wms_product_center)
            {
                st_product_center.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            vm.str8 = st_product_center;
            #endregion
            IEnumerable<SelectListItem> selectList = null;
            try
            {
                selectList = ApplicationConfigHelper.GetWMS_Config("ProductLevel_" + base.UserInfo.ProjectName).Select(c => new SelectListItem() { Value = c.Name, Text = c.Name });
            }
            catch (Exception)
            {
            }

            if (selectList == null || selectList.Count() == 0)
            {
                vm.selectList = ApplicationConfigHelper.GetWMS_Config("ProductLevel").Select(c => new SelectListItem() { Value = c.Name, Text = c.Name });
            }
            else
            {
                vm.selectList = selectList;
            }

            vm.SearchCondition = new PreOrderSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            vm.ViewType = ViewType;
            vm.ReturnViewType = "get";
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            //vm.HideActionButton = hideActionButton ?? false;
            //vm.ShowEditRelated = showEditRelated ?? true;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            vm.Flag = Flag;
            ViewBag.backFlag = backFlag;
            //vm.Customers = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetStorerID(base.UserInfo.ID)
            //               .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name.ToString() });
            if (customerID == null)
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                customerID = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                    .Select(a => a.CustomerID).FirstOrDefault();
                // .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName }); 
                // customerID = Convert.ToInt64(vm.Customers.First().Value); 
            }
            else
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                       .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            //if (customerID == null)
            //{
            //    StringBuilder sb = new StringBuilder();
            //    vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(1, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
            //                     .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //    foreach (var i in vm.Customers)
            //    {
            //        sb.Append("'" + i.Value + "',");
            //    }
            //    if (sb.Length > 1)
            //    {
            //        vm.PreAndDetail.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
            //        customerID = Convert.ToInt64(vm.Customers.First().Value);
            //    }
            //}
            if (ViewType != 1)
            {
                PreOrderSearchCondition SearchCondition = new PreOrderSearchCondition();
                SearchCondition.ID = Convert.ToInt32(ID);
                var getPreOrderByConditionResponse = new PreOrderService().GetPreOrderAndDetail(new PreOrderRequest() { SearchCondition = SearchCondition });
                vm.PreAndDetail = getPreOrderByConditionResponse.Result;
            }

            if (ViewType == 1)
            {
                vm.PreAndDetail = new PreOrderAndPreOrderDetail();
                vm.PreAndDetail.SearchCondition = new PreOrderSearchCondition();
                vm.PreAndDetail.SearchCondition.OrderTime = DateTime.Now;
                vm.PreAndDetail.SearchCondition.CustomerID = customerID;
                vm.SearchCondition.CustomerID = customerID;
            }
            if (customerID != null)
            {
                var UnitAndSpecificationsList = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(base.UserInfo.ProjectID, customerID, 0);

                var UnitList = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Unit }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });

                var UnitAndSpecificationsLists = UnitAndSpecificationsList.Select(t => new { Code = t.Unit, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name });
                var SpecificationsList = UnitAndSpecificationsList.Select(t => new { Code = t.Specifications, Name = t.Specifications }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Code });
                vm.UnitList = UnitList;
                vm.SpecificationsList = SpecificationsList;
                if (base.UserInfo.UserType == 0)
                {
                    vm.PreAndDetail.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
                    vm.SearchCondition.CustomerID = customerID;
                }
                //else if (base.UserInfo.UserType == 1)
                //{

                //}
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        vm.SearchCondition.CustomerID = customerID;
                    }
                    else
                    {
                        var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                        if (customerIDs != null && customerIDs.Count() == 1)
                        {
                            vm.SearchCondition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                        }
                    }
                }
            }
            Session["backFlag"] = ViewType;

            this.GenQueryPodViewModel(vm);

            return View(vm);
        }
        [HttpPost]
        public ActionResult PreOrderCreateOrEdit(string PreOrderJson, string PreOrderNumber, string ExternOrderNumber, string PreOrderDetailJson, long? CustomerID, bool? hideActionButton, bool? showEditRelated, int ViewType = 0)
        {
            PreOrderViewModel vm = new PreOrderViewModel();
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
            vm.OrderTypes = st;
            vm.SearchCondition = new PreOrderSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            vm.ReturnViewType = "post";
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            StringBuilder sb = new StringBuilder();
            DataTable dt = JsonToDataTable(PreOrderDetailJson);
            IEnumerable<Column> columns;
            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_PreOrderDetail").Count() == 0)
            {
                columns = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
            }
            else
            {
                columns = module.Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
            }
            var customerColumn = columns.First(c => c.DbColumnName == "CustomerName").DisplayName;
            columns = columns.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == CustomerID));
            bool useCustomerOrderNumber = module.UseCustomerOrderNumber;
            IEnumerable<PreOrderDetail> PreOrderDetails = this.AddPreOrderDetailFromDataTable(dt, columns, useCustomerOrderNumber, sb);
            PreOrderRequest request = new PreOrderRequest();
            var responseJsonFieldsets = jsonlist<PreOrder>(PreOrderJson);
            bool IsInt1 = false;
            #region
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", 0, 0, 0);
            ctype = ctype.Where(c => c.Code == "AKZO" && c.Name == CustomerID.ToString());
            #endregion
            responseJsonFieldsets.Each((i, o) =>
            {
                o.CustomerName = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(b => b.CustomerID == o.CustomerID)
                           .Select(c => c.CustomerName).FirstOrDefault();
                o.CreateTime = DateTime.Now;
                o.Creator = base.UserInfo.Name;
                o.Status = 1;
                //if ((o.CustomerID == 75 || o.CustomerID == 74 || o.CustomerID == 10089) && o.OrderType != "调整出库")
                if (ctype != null && ctype.Count() > 0 && o.OrderType != "调整出库")
                {
                    IsInt1 = true;
                    o.Int1 = 1;
                    o.Int2 = 1;
                }
                else
                {
                    IsInt1 = false;
                    o.Int1 = 0;
                    o.Int2 = 0;
                }
            });
            PreOrderDetails.Each((i, od) =>
            {

                od.SKU = od.SKU.Trim();
                od.LineNumber = od.LineNumber.Trim();
                od.GoodsName = od.GoodsName;
                od.GoodsType = od.GoodsType;
                od.Creator = base.UserInfo.Name;
                od.CustomerID = CustomerID.ObjectToInt64();
                od.CustomerName = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(b => b.CustomerID == od.CustomerID)
                           .Select(c => c.CustomerName).FirstOrDefault();
                od.PreOrderNumber = PreOrderNumber;
                od.ExternOrderNumber = ExternOrderNumber;
                od.BatchNumber = od.BatchNumber == "请选择" ? null : od.BatchNumber;
                od.BoxNumber = od.BoxNumber == "请选择" ? null : od.BoxNumber;
                //if ((od.CustomerID == 75 || od.CustomerID == 74 || od.CustomerID == 10089) && IsInt1)
                if (ctype != null && ctype.Count() > 0 && IsInt1)
                {
                    od.Int1 = 1;
                    od.Int2 = 1;
                }
                else
                {
                    od.Int1 = 0;
                    od.Int2 = 0;
                }
            });
            request.PreOrderList = responseJsonFieldsets;
            request.PreOd = PreOrderDetails;
            PreOrderService pos = new PreOrderService();
            var response = pos.AddPreOrderAndPreOrderDetail(request, base.UserInfo.Name);
            if (response.IsSuccess)
            {
                vm.PreAndDetail = response.Result;
                if (ViewType == 1)
                {
                    vm.PreAndDetail = response.Result;
                    vm.PreAndDetail.SearchCondition = response.Result.SearchCondition;
                }
                if (vm.PreAndDetail.SearchCondition.CustomerID != null)
                {
                    if (base.UserInfo.UserType == 0)
                    {
                        vm.PreAndDetail.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
                    }
                    else if (base.UserInfo.UserType == 2)
                    {
                        if (vm.PreAndDetail.SearchCondition.CustomerID.HasValue)
                        {
                            vm.PreAndDetail.SearchCondition.CustomerID = vm.PreAndDetail.SearchCondition.CustomerID;
                        }
                        else
                        {
                            var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                            if (customerIDs != null && customerIDs.Count() == 1)
                            {
                                vm.PreAndDetail.SearchCondition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                            }
                        }
                    }
                }
            }
            this.GenQueryPodViewModel(vm);
            return View(vm);
        }

        public JsonResult AddPreOrderCreateOrEdit(string PreOrderJson, string PreOrderNumber, string ExternOrderNumber, string PreOrderDetailJson, long? CustomerID, long? WarehouseID, bool? hideActionButton, bool? showEditRelated, int ViewType = 0)
        {
            string OperationType = "修改";
            if (ViewType == 1)
            {
                OperationType = "新增";
                var countAsn = new ASNManagementService().ExternKeyCheck(ExternOrderNumber, "2", CustomerID.Value);
                if (countAsn > 0)
                {
                    return Json(new { Errorcode = 0, error = "外部单号已存在!" }); ;
                }
            }
            PreOrderViewModel vm = new PreOrderViewModel();
            vm.SearchCondition = new PreOrderSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            StringBuilder sb = new StringBuilder();
            DataTable dt = JsonToDataTable(PreOrderDetailJson);
            IEnumerable<Column> columns;
            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_PreOrderDetail").Count() == 0)
            {
                columns = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
            }
            else
            {
                columns = module.Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
            }
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            var customerColumn = columns.First(c => c.DbColumnName == "CustomerName").DisplayName;
            bool useCustomerOrderNumber = module.UseCustomerOrderNumber;
            IEnumerable<PreOrderDetail> PreOrderDetails = this.AddPreOrderDetailFromDataTable(dt, columns, useCustomerOrderNumber, sb);
            PreOrderRequest request = new PreOrderRequest();
            var responseJsonFieldsets = jsonlist<PreOrder>(PreOrderJson);
            bool IsInt1 = false;
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            #region
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", 0, 0, 0);
            ctype = ctype.Where(c => c.Code == "AKZO" && c.Name == CustomerID.ToString());
            #endregion
            responseJsonFieldsets.Each((i, o) =>
            {
                #region log
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "预出库管理";
                operation.Operation = "预出库-" + OperationType;
                operation.OrderType = "PreOrder";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.CustomerID = int.Parse(o.CustomerName.ToString());
                operation.CustomerName = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                    .Where(b => b.CustomerID == long.Parse(o.CustomerName)).Select(c => c.CustomerName).FirstOrDefault();
                operation.ExternOrderNumber = o.ExternOrderNumber;
                logs.Add(operation);
                #endregion

                o.CustomerID = long.Parse(o.CustomerName);
                o.CustomerName = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                    .Where(b => b.CustomerID == long.Parse(o.CustomerName)).Select(c => c.CustomerName).FirstOrDefault();
                o.CreateTime = DateTime.Now;
                o.WarehouseId = long.Parse(o.Warehouse);
                o.Warehouse = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetCacheInfo()
                    .Where(c => (c.CustomerID == CustomerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)))
                    .Where (c => c.WarehouseID == int.Parse(o.Warehouse))
                    .Select(c => c.WarehouseName).FirstOrDefault();
                o.CreateTime = DateTime.Now;
                o.Creator = base.UserInfo.Name;
                o.Status = 1;
                if (ctype != null && ctype.Count() > 0 && o.OrderType != "调整出库")
                {
                    IsInt1 = true;
                    o.Int1 = 1;
                    o.Int2 = 1;
                }
            });
            PreOrderDetails.Each((i, od) =>
            {
                od.SKU = od.SKU.Trim();
                od.LineNumber = od.LineNumber.Trim();
                od.GoodsName = od.GoodsName;
                od.GoodsType = od.GoodsType;
                od.Creator = base.UserInfo.Name;
                od.CustomerID = long.Parse(CustomerID.ToString());
                od.CustomerName = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                    .Where(b => b.CustomerID == CustomerID).Select(c => c.CustomerName).FirstOrDefault();
                od.WarehouseId = long.Parse(WarehouseID.ToString());
                od.Warehouse = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetCacheInfo()
                    .Where(c => (c.CustomerID == CustomerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)))
                    .Where (c => c.WarehouseID == int.Parse(WarehouseID.ToString()))
                    .Select(c => c.WarehouseName).FirstOrDefault();
                od.PreOrderNumber = PreOrderNumber;
                od.ExternOrderNumber = ExternOrderNumber;
                od.BatchNumber = od.BatchNumber == "请选择" ? null : od.BatchNumber;
                od.BoxNumber = od.BoxNumber == "请选择" ? null : od.BoxNumber;
                od.Unit = od.Unit == "请选择" ? null : od.Unit;
                od.Specifications = od.Specifications == "请选择" ? null : od.Specifications;
                if (ctype != null && ctype.Count() > 0 && IsInt1)
                {
                    od.Int1 = 1;
                    od.Int2 = 1;
                }
            });
            request.PreOrderList = responseJsonFieldsets;
            request.PreOd = PreOrderDetails;
            PreOrderService pos = new PreOrderService();
            Response<PreOrderAndPreOrderDetail> response;
            if (OperationType == "修改")
            {
                response = pos.AddPreOrderAndPreOrderDetail(request, base.UserInfo.Name, OperationType);//编辑
            }
            else
            {
                response = pos.AddPreOrderAndPreOrderDetail(request, base.UserInfo.Name);//编辑
            }
            if (response.IsSuccess)
            {
                if (response.Result.PreO != null)
                {
                    new LogOperationService().AddLogOperation(logs);
                    return Json(new { Errorcode = 1, ID = response.Result.PreO.ID });
                }
                else
                {
                    return Json(new { Errorcode = 0 });
                }
            }
            return Json(new { Errorcode = 0 });
        }

        //public JsonResult AddInventoryOfOutbound(string PreOrderJson, string PreOrderNumber, string ExternOrderNumber, string PreOrderDetailJson, long? CustomerID, bool? hideActionButton, bool? showEditRelated, int ViewType = 0)
        //{
        //    PreOrderViewModel vm = new PreOrderViewModel();
        //    vm.SearchCondition = new PreOrderSearchCondition();
        //    vm.SearchCondition.UserType = base.UserInfo.UserType;
        //    vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
        //    //vm.Customers = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetStorerID(base.UserInfo.ID)
        //    //               .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name.ToString() });

        //    StringBuilder sb = new StringBuilder();
        //    DataTable dt = JsonToDataTable(PreOrderDetailJson);
        //    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID).ProjectCollection.First(p => p.Id == "1");
        //    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
        //    IEnumerable<Column> columns = module.Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
        //    var customerColumn = columns.First(c => c.DbColumnName == "CustomerName").DisplayName;
        //    columns = columns
        //       .Select(c =>
        //       {
        //           if (c.InnerColumns.Count == 0)
        //           {
        //               return c;
        //           }
        //           else
        //           {
        //               if (c.InnerColumns.Any(innerc => innerc.CustomerID == CustomerID.ObjectToInt64()))
        //               {
        //                   return c.InnerColumns.First(innerc => innerc.CustomerID == CustomerID.ObjectToInt64());
        //               }
        //               return c;
        //           }
        //       });
        //    bool useCustomerOrderNumber = module.UseCustomerOrderNumber;
        //    IEnumerable<PreOrderDetail> PreOrderDetails = this.AddPreOrderDetailFromDataTable(dt, columns, useCustomerOrderNumber, sb);
        //    //var getASNByConditionResponse = new PreOrderService().GetPreOrderAndDetail(new PreOrderRequest() { PageIndex = 1 });
        //    //vm.PreAndDetail = getASNByConditionResponse.Result;
        //    PreOrderRequest request = new PreOrderRequest();
        //    //IList<PreOrderDetail> PreOrderDetails = new List<PreOrderDetail>();
        //    var responseJsonFieldsets = jsonlist<PreOrder>(PreOrderJson);
        //    responseJsonFieldsets.Each((i, o) =>
        //    {
        //        o.CustomerName = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(b => b.CustomerID == o.CustomerID)
        //                   .Select(c => c.CustomerName).FirstOrDefault();
        //        o.CreateTime = DateTime.Now;
        //        o.Creator = base.UserInfo.Name;
        //        o.Status = 1;
        //    });
        //    PreOrderDetails.Each((i, od) =>
        //    {

        //        od.SKU = od.SKU.Trim();
        //        od.LineNumber = od.LineNumber.Trim();
        //        od.GoodsName = od.GoodsName;
        //        od.GoodsType = od.GoodsType;
        //        od.Creator = base.UserInfo.Name;
        //        od.CustomerID = CustomerID.ObjectToInt64();
        //        od.CustomerName = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(b => b.CustomerID == od.CustomerID)
        //                   .Select(c => c.CustomerName).FirstOrDefault();
        //        od.PreOrderNumber = PreOrderNumber;
        //        od.ExternOrderNumber = ExternOrderNumber;
        //    });
        //    request.PreOrderList = responseJsonFieldsets;
        //    request.PreOd = PreOrderDetails;

        //    // var response = new PreOrderService().GetPreOrder(new PreOrderRequest
        //    //{
        //    //    SearchCondition = vm.SearchCondition,
        //    PreOrderService pos = new PreOrderService();
        //    var response = pos.AddInventoryOfOutbound(request, base.UserInfo.Name);
        //    if (response.IsSuccess)
        //    {
        //        //if (response.Result.SearchCondition != null)
        //        //{
        //        //    return Json(new { Errorcode = 1, ID = response.Result.SearchCondition.ID });
        //        //}
        //        //else
        //        //{
        //        //    return Json(new { Errorcode = 0 });
        //        }
        //    }
        //    return Json(new { Errorcode = 0 });

        //    //if (response.IsSuccess)
        //    //{
        //    //    //vm.ViewType= 0;
        //    //    vm.PreAndDetail = response.Result;

        //    //    if (ViewType == 1)
        //    //    {
        //    //        vm.PreAndDetail = response.Result;
        //    //        vm.PreAndDetail.SearchCondition = response.Result.SearchCondition;
        //    //    }
        //    //    if (vm.PreAndDetail.SearchCondition.CustomerID != null)
        //    //    {
        //    //        if (base.UserInfo.UserType == 0)
        //    //        {
        //    //            vm.PreAndDetail.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
        //    //        }
        //    //        //else if (base.UserInfo.UserType == 1)
        //    //        //{

        //    //        //}
        //    //        else if (base.UserInfo.UserType == 2)
        //    //        {
        //    //            if (vm.PreAndDetail.SearchCondition.CustomerID.HasValue)
        //    //            {
        //    //                vm.PreAndDetail.SearchCondition.CustomerID = vm.PreAndDetail.SearchCondition.CustomerID;
        //    //            }
        //    //            else
        //    //            {
        //    //                var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(1, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
        //    //                     .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
        //    //                if (customerIDs != null && customerIDs.Count() == 1)
        //    //                {
        //    //                    vm.PreAndDetail.SearchCondition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
        //    //                }
        //    //            }
        //    //        }
        //    //    }
        //    //}

        //    //this.GenQueryPodViewModel(vm);
        //    //return View(vm);
        //}

        [HttpGet]
        public ActionResult AssignedAllocation(string ID, bool? hideActionButton, bool? showEditRelated, long? customerID, string ShowSubmit)
        {
            PreOrderViewModel vm = new PreOrderViewModel();
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
            vm.OrderTypes = st;
            vm.SearchCondition = new PreOrderSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            //vm.ViewType = ViewType;
            //vm.HideActionButton = hideActionButton ?? false;
            //vm.ShowEditRelated = showEditRelated ?? true;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            vm.ShowSubmit = ShowSubmit;
            //vm.Customers = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetStorerID(base.UserInfo.ID)
            //               .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name.ToString() });
            StringBuilder sb = new StringBuilder();
            if (customerID == 0)
            {
                vm.CustomerNames = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
                                     .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                foreach (var i in vm.CustomerNames)
                {
                    sb.Append("'" + i.Value + "',");
                }
                if (sb.Length > 1)
                {
                    vm.PreAndDetail.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            PreOrderSearchCondition SearchCondition = new PreOrderSearchCondition();
            SearchCondition.ID = Convert.ToInt32(ID);
            var getPreOrderByConditionResponse = new PreOrderService().Allocation_GetPreOrderAndDetail(new PreOrderRequest() { SearchCondition = SearchCondition });
            vm.PreAndDetail = getPreOrderByConditionResponse.Result;


            if (base.UserInfo.UserType == 0)
            {
                vm.PreAndDetail.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
            {

            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.PreAndDetail.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
                                     .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.PreAndDetail.SearchCondition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                    }
                }
            }
            this.GenQueryPodViewModel(vm);

            return View(vm);
        }

        [HttpGet]
        public ActionResult ManualAllocation(string ID, bool? hideActionButton, bool? showEditRelated, long? customerID, string ShowSubmit)
        {
            PreOrderViewModel vm = new PreOrderViewModel();
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
            vm.OrderTypes = st;
            vm.SearchCondition = new PreOrderSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            //vm.ViewType = ViewType;
            //vm.HideActionButton = hideActionButton ?? false;
            //vm.ShowEditRelated = showEditRelated ?? true;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            vm.ShowSubmit = ShowSubmit;
            //vm.Customers = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetStorerID(base.UserInfo.ID)
            //               .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name.ToString() });
            StringBuilder sb = new StringBuilder();
            if (customerID == 0)
            {
                vm.CustomerNames = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
                                     .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                foreach (var i in vm.CustomerNames)
                {
                    sb.Append("'" + i.Value + "',");
                }
                if (sb.Length > 1)
                {
                    vm.PreAndDetail.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            PreOrderSearchCondition SearchCondition = new PreOrderSearchCondition();
            SearchCondition.ID = Convert.ToInt32(ID);
            var getPreOrderByConditionResponse = new PreOrderService().Allocation_GetPreOrderAndDetail(new PreOrderRequest() { SearchCondition = SearchCondition });
            vm.PreAndDetail = getPreOrderByConditionResponse.Result;


            if (base.UserInfo.UserType == 0)
            {
                vm.PreAndDetail.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
            {

            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.PreAndDetail.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
                                     .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.PreAndDetail.SearchCondition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                    }
                }
            }
            this.GenQueryPodViewModel(vm);

            return View(vm);
        }
        public JsonResult GetPrdOrder_distributionInventory(long PREID)
        {
            //long CustomerID, string SKU, string UPC, string Type, string BatchNumber, string BoxNumber, string Location, string Warehouse, string Unit, string Specifications            

            //IEnumerable<WMSConfig> wms = null;
            //try
            //{
            //    wms = ApplicationConfigHelper.GetWMS_Config("ManualAllocation_" + base.UserInfo.ProjectName);
            //}
            //catch (Exception)
            //{
            //}

            //if (wms == null)
            //{
            //    wms = ApplicationConfigHelper.GetWMS_Config("ManualAllocation");
            //}
            //var getPreOrderByConditionResponse = new PreOrderService().GetPrdOrder_distributionInventory(PREID, wms.FirstOrDefault().Name);

            IEnumerable<WMS_Config_Type> wms = null;
            wms = ApplicationConfigHelper.GetWMS_ConfigType("GetPreOrderInventory", base.UserInfo.ProjectID, 0, 0);
            var getPreOrderByConditionResponse = new PreOrderService().GetPrdOrder_distributionInventory(PREID, wms.FirstOrDefault().Name);
            ////    new InventorySearchCondition
            //{
            //    CustomerID = CustomerID,
            //    SKU = SKU,
            //    UPC = UPC,
            //    GoodsType = Type,
            //    BatchNumber = BatchNumber,
            //    BoxNumber = BoxNumber,
            //    Warehouse = Warehouse,
            //    Unit = Unit,
            //    Specifications = Specifications
            //}


            if (getPreOrderByConditionResponse.IsSuccess)
            {
                if (getPreOrderByConditionResponse.Result.inventorys.Count() > 0)
                {
                    return Json(new { Errorcode = 1, Inventory = getPreOrderByConditionResponse.Result.inventorys });
                }
                else
                {
                    return Json(new { Errorcode = 0 });
                }
            }
            return Json(new { Errorcode = 0 });
            //InventorySearchCondition
            //  CustomerID:CustomerID,
            //  SKU: SKU,
            //  Type: Type,
            //  BatchNumber:BatchNumber,
            //  BoxNumber: BoxNumber,
            //  Location: Location
            //var getPreOrderByConditionResponse = new PreOrderService().GetPrdOrder_distributionInventory(Id);
            //if (getPreOrderByConditionResponse.IsSuccess)
            //{
            //    if (getPreOrderByConditionResponse.Result.Inventorys.Count() > 0)
            //    {
            //        return Json(new { Errorcode = 1, Inventory = getPreOrderByConditionResponse.Result.Inventorys });
            //    }
            //    else
            //    {
            //        return Json(new { Errorcode = 0 });
            //    }
            //}

        }

        public JsonResult AssignedAllocationJson(long? ID, long? CustomerId, string Criterion, string Jaonstr)
        {
            PreOrderViewModel vm = new PreOrderViewModel();
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
            vm.OrderTypes = st;
            IEnumerable<DistributionInformation> DisInfo = null;
            IEnumerable<PreOrderDetail> Pod = jsonlist<PreOrderDetail>(Jaonstr);
            this.AssignedManualAllocate(Pod, ID.Value, CustomerId, Pod.First().WarehouseId, Criterion, ref DisInfo);
            //var response = new PreOrderService().ManualAllocationJson(new ManualAllocationRequest() { PodRequest = pod, ID = Convert.ToInt64(ID), CustomerId = CustomerId, Creator = base.UserInfo.Name, Criterion = Criterion });
            //return response;
            if (DisInfo != null && DisInfo.Count() > 0)
            {
                #region 操作日志
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "预出库管理";
                operation.Operation = "预出库-指定分配";
                operation.OrderType = "PreOrder";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = ID.ToString();
                logs.Add(operation);
                new LogOperationService().AddLogOperation(logs);
                #endregion
                var data = from q in DisInfo
                           select new
                           {
                               q.POID,
                               q.Customer,
                               q.ExternOrderNumber,
                               q.Note,
                               q.Type,
                               q.SKU,
                               q.QTY,
                               q.Article,
                               q.Size,
                               Message = ApplicationConfigHelper.GetWMS_Config("PreOrderStatus").Where(a => a.Code == q.Note).FirstOrDefault().Name,
                           };
                return Json(new { Errorcode = 1, data = data });
            }
            else
            {
                return Json(new { Errorcode = 0 });
            }
            //return Json(new { Errorcode = 0, data = "操作失败" });
        }
        [HttpPost]
        public JsonResult ManualAllocationJson(long? ID, long? CustomerId, string Criterion, string Jaonstr)
        {
            PreOrderViewModel vm = new PreOrderViewModel();
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
            vm.OrderTypes = st;
            IEnumerable<DistributionInformation> DisInfo = null;
            IEnumerable<PreOrderDetail> Pod = jsonlist<PreOrderDetail>(Jaonstr);
            Pod.GroupBy(q => new { q.CustomerID, q.WarehouseId }).Each((i, g) =>
            this.ManualAllocate(g.Select(a => new PreOrderDetail(a)), ID.Value, CustomerId, g.First().WarehouseId, Criterion, ref DisInfo));
            //var response = new PreOrderService().ManualAllocationJson(new ManualAllocationRequest() { PodRequest = pod, ID = Convert.ToInt64(ID), CustomerId = CustomerId, Creator = base.UserInfo.Name, Criterion = Criterion });
            //return response;
            if (DisInfo.Count() > 0)
            {
                #region 操作日志
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "预出库管理";
                operation.Operation = "预出库-手动分配";
                operation.OrderType = "PreOrder";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = ID.ToString();
                logs.Add(operation);
                new LogOperationService().AddLogOperation(logs);
                #endregion
                var data = from q in DisInfo
                           select new
                           {
                               q.Customer,
                               q.ExternOrderNumber,
                               q.Note,
                               Message = ApplicationConfigHelper.GetWMS_Config("PreOrderStatus").Where(a => a.Code == q.Note).FirstOrDefault().Name,
                           };
                return Json(new { Errorcode = 1, data = data }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Errorcode = 0 }, JsonRequestBehavior.AllowGet);
            }
            //return Json(new { Errorcode = 0, data = "操作失败" });
        }

        [HttpPost]
        public JsonResult ManualAllocationSaveJson(long? ID, long? CustomerId, string Criterion, string Jaonstr)
        {
            PreOrderViewModel vm = new PreOrderViewModel();
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
            vm.OrderTypes = st;
            IEnumerable<PreOrderDetail> Pod = jsonlist<PreOrderDetail>(Jaonstr);
            Pod.Each((i, p) =>
            {
                p.str20 = p.Area + "|" + p.Location;
                p.Int5 = p.IID.ObjectToInt32();
            });

            var response = new PreOrderService().ManualAllocationSaveJson(new ManualAllocationRequest() { PodRequest = Pod, ID = Convert.ToInt64(ID), Creator = base.UserInfo.Name });
            //return response;

            return Json(new { Errorcode = 1, data = "" }, JsonRequestBehavior.AllowGet);



        }

        public static List<T> jsonlist<T>(string str)
        {
            try
            {
                return JsonConvert.DeserializeObject<List<T>>(str);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnsConfig"></param>
        /// <param name="useCustomerOrderNumber"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        private IEnumerable<PreOrderDetail> AddPreOrderDetailFromDataTable(DataTable dt, IEnumerable<Column> columnsConfig, bool useCustomerOrderNumber, StringBuilder sb)
        {
            IList<PreOrderDetail> PreOrderDetails = new List<PreOrderDetail>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PreOrderDetail asnDetail = new PreOrderDetail();

                string value;

                foreach (var column in columnsConfig.Where(c => c.IsKey == true))
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            //value = dt.Rows[i][j].ToString().Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (column.Type == "DateTime" || column.Type == "DateTimeWithTime")
                                {
                                    DateTime dttemp;
                                    if (DateTime.TryParse(value, out dttemp))
                                    {
                                        typeof(PreOrderDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, dttemp, null);
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是日期格式，请修改。<br/>");
                                    }
                                }
                                else if (column.Type == "CheckBox" || (column.Type == "DropDownList" && !column.IsKey))
                                {
                                    if (value == "1" || value == "0" || value == "是" || value == "否" || value == "Y" || value == "y" || value == "N" || value == "n")
                                    {
                                        if (value == "1" || value == "是" || value == "Y" || value == "y")
                                        {
                                            typeof(PreOrderDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, "1", null);
                                        }
                                        else
                                        {
                                            typeof(PreOrderDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    var propertyInfoTemp = typeof(PreOrderDetail).GetProperty(column.DbColumnName);
                                    try
                                    {
                                        propertyInfoTemp.SetValue(asnDetail, Convert.ChangeType(value, propertyInfoTemp.PropertyType), null);
                                    }
                                    catch
                                    {
                                        propertyInfoTemp.SetValue(asnDetail, null);
                                    }
                                }

                                break;
                            }

                            break;
                        }
                    }
                }

                foreach (var column in columnsConfig.Where(c => c.IsKey == false && c.IsHide == false && c.ForView == true))
                {
                    //foreach (var column in col.InnerColumns)
                    //{
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            //value = dt.Rows[i][j].ToString().Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (column.Type == "DateTime" || column.Type == "DateTimeWithTime")
                                {
                                    DateTime dttemp;
                                    if (DateTime.TryParse(value, out dttemp))
                                    {
                                        typeof(PreOrderDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, dttemp, null);
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, <strong>" + value + "</strong>不是日期格式，请修改。<br/>");
                                    }
                                }
                                else if (column.Type == "CheckBox" || (column.Type == "DropDownList" && !column.IsKey))
                                {
                                    if (value == "1" || value == "0" || value == "是" || value == "否" || value == "Y" || value == "y" || value == "N" || value == "n")
                                    {
                                        if (value == "1" || value == "是" || value == "Y" || value == "y")
                                        {
                                            typeof(PreOrderDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, "1", null);
                                        }
                                        else
                                        {
                                            typeof(PreOrderDetail).GetProperty(column.DbColumnName).SetValue(asnDetail, "0", null);
                                        }
                                    }
                                    else
                                    {
                                        sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列，<strong>" + value + "</strong>必须为0，1，是，否，N，Y中一个，请修改。<br />");
                                    }
                                }
                                else
                                {
                                    var propertyInfoTemp = typeof(PreOrderDetail).GetProperty(column.DbColumnName);
                                    try
                                    {
                                        propertyInfoTemp.SetValue(asnDetail, Convert.ChangeType(value, propertyInfoTemp.PropertyType), null);
                                    }
                                    catch
                                    {
                                        propertyInfoTemp.SetValue(asnDetail, null);
                                    }
                                }
                                break;
                            }
                            break;
                        }
                    }
                    //}
                }
                PreOrderDetails.Add(asnDetail);
            }

            return PreOrderDetails;
        }
        private static DataTable JsonToDataTable(string strJson)
        {
            //取出表名  
            //Regex rg = new Regex(@"(?<={)[^:]+(?=:/[)", RegexOptions.IgnoreCase);  
            //string strName = rg.Match(strJson).Value;  
            DataTable tb = null;
            //去除表名  
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));

            //获取数据  
            Regex rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value.Replace("\"", "");
                string[] strRows = strRow.Split(',');

                //创建表  
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = "PreOrderDetail";
                    foreach (string str in strRows)
                    {
                        DataColumn dc = new DataColumn();
                        string[] strCell = str.Split(':');
                        dc.ColumnName = strCell[0].ToString();
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                //增加内容  
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split(':')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("/", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }
            return tb;
        }

        /// <summary>
        /// 省市区的智能提示
        /// </summary>
        /// <param name="find">告诉方法做什么</param>
        /// <param name="Province">省份</param>
        /// <param name="City"></param>
        /// <param name="District"></param>
        /// <returns></returns>
        public JsonResult GetCity(string find, string Province, string City, string District)
        {

            if (find == "Province")
            {
                var pid = ApplicationConfigHelper.GetRegions().Where(q => q.Grade == 2);
                return Json(pid.Where(s => s.Name.IndexOf(Province, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.Name }), JsonRequestBehavior.AllowGet);
            }
            else if (find == "City")
            {
                var pidWhere = ApplicationConfigHelper.GetRegions().Where(q => q.Name == Province);
                long pid = 0;
                if (pidWhere != null)
                {
                    pid = pidWhere.Select(q => q.ID).FirstOrDefault();
                }

                var cid = ApplicationConfigHelper.GetRegions().Where(q => q.SupperID == pid || q.Grade == 3);
                return Json(cid.Where(s => s.Name.IndexOf(City, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.Name }), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var cidWhere = ApplicationConfigHelper.GetRegions().Where(q => q.Name == City && q.Grade == 3);
                long cid = 0;
                if (cidWhere != null)
                {
                    cid = cidWhere.Select(q => q.ID).FirstOrDefault();
                }

                var dis = ApplicationConfigHelper.GetRegions().Where(q => q.SupperID == cid || q.Grade == 4);
                return Json(dis.Where(s => s.Name.IndexOf(District, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.Name }), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCitys(string find, string Province, string City, string District)
        {
            if (find == "Province")
            {
                var pro = ApplicationConfigHelper.GetRegions().Where(q => q.Grade == 2);
                return Json(pro.Where(s => s.Name.IndexOf(Province, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { value = t.ID.ToString(), Text = t.Name }), JsonRequestBehavior.AllowGet);
            }
            else if (find == "City")
            {

                var pro = ApplicationConfigHelper.GetRegions().Where(q => q.Name == Province).Select(q => q.ID).FirstOrDefault();
                var cit = ApplicationConfigHelper.GetRegions().Where(q => q.SupperID == pro && q.Grade == 3);
                return Json(cit.Where(s => s.Name.IndexOf(City, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.Name }), JsonRequestBehavior.AllowGet);
            }
            else
            {

                var cit = ApplicationConfigHelper.GetRegions().Where(q => q.Name == City && q.Grade == 3).FirstOrDefault();
                var dis = ApplicationConfigHelper.GetRegions().Where(q => q.SupperID == cit.ID && q.Grade == 4);
                return Json(dis.Where(s => s.Name.IndexOf(District, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.Name }), JsonRequestBehavior.AllowGet);

            }


        }
        //public string Cancel(string ids) {
        //    PreOrderService service = new PreOrderService();
        //    var sul = service.Cancel(ids);
        //    return sul;
        //}


        /// <summary>
        /// 现场分配
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult WorkersAlloctions(string ids)
        {
            PreOrderService service = new PreOrderService();
            IEnumerable<string> numbers = Enumerable.Empty<string>();
            List<PreOrderIds> list = new List<PreOrderIds>();
            PreOrderIds p = new PreOrderIds();
            if (ids.IndexOf(',') > 0)
            {
                numbers = ids.Split(',').Select(s => { return s.Trim(); });
                foreach (var item in numbers)
                {
                    list.Add(new PreOrderIds { ID = Convert.ToInt64(item) });
                }
            }
            else
            {
                list.Add(new PreOrderIds { ID = Convert.ToInt64(ids) });
            }
            PreOrderSearchCondition sc = new PreOrderSearchCondition();
            sc.Ids = list;
            var response = new PreOrderService().GetPreOrder(new PreOrderRequest
            {
                SearchCondition = sc
            });
            if (response.IsSuccess)
            {
                IEnumerable<DistributionInformation> DisInfo = null;
                if (response.Result.SearchCondition.Count() > 0)
                {
                    response.Result.SearchCondition.GroupBy(q => new { q.CustomerID, q.WarehouseId }).Each((i, g) =>
                        this.WorkersAllocation(g.Select(a => new PreOrderIds { ID = a.ID }), g.First().CustomerID.Value, g.First().WarehouseId.Value, ref DisInfo));
                }
                var data = from q in DisInfo
                           select new
                           {
                               q.Customer,
                               q.ExternOrderNumber,
                               q.Note,
                               Message = ApplicationConfigHelper.GetWMS_Config("PreOrderStatus").Where(a => a.Code == q.Note).FirstOrDefault().Name,
                           };
                return Json(new { Errorcode = 1, data = data });
            }
            else
            {
                return Json(new { Errorcode = 0, data = "操作失败" });
            }
        }

        /// <summary
        /// 自动分配
        /// </summary>
        /// <param name="preorderlist"></param>
        /// <param name="ids"></param>
        /// <param name="Criterion"></param>
        /// <returns></returns>
        public JsonResult AutomaticAllocation(string ids)
        {
            PreOrderService service = new PreOrderService();
            IEnumerable<string> numbers = Enumerable.Empty<string>();
            List<PreOrderIds> list = new List<PreOrderIds>();
            PreOrderIds p = new PreOrderIds();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            List<WMS_Cord_Operation> cords = new List<WMS_Cord_Operation>();
            if (ids.IndexOf(',') > 0)
            {
                numbers = ids.Split(',').Select(s => { return s.Trim(); });
                foreach (var item in numbers)
                {
                    list.Add(new PreOrderIds { ID = Convert.ToInt64(item) });
                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "预出库管理";
                    operation.Operation = "预出库-自动分配";
                    operation.OrderType = "PreOrder";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = base.UserInfo.Name;
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = (int)base.UserInfo.ProjectID;
                    operation.ProjectName = base.UserInfo.ProjectName;
                    operation.OrderID = item;
                    logs.Add(operation);
                    WMS_Cord_Operation cord = new WMS_Cord_Operation()
                    {
                        MenuName = "预出库管理",
                        Operation = "预出库-自动分配",
                        OrderType = "PreOrder",
                        Controller = Request.RawUrl,
                        Creator = base.UserInfo.Name,
                        CreateTime = DateTime.Now,
                        ProjectID = (int)base.UserInfo.ProjectID,
                        ProjectName = base.UserInfo.ProjectName,
                        OrderID = item
                    };
                    cords.Add(cord);
                }
            }
            else
            {
                list.Add(new PreOrderIds { ID = Convert.ToInt64(ids) });
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "预出库管理";
                operation.Operation = "预出库-自动分配";
                operation.OrderType = "PreOrder";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = ids;
                logs.Add(operation);
                WMS_Cord_Operation cord = new WMS_Cord_Operation()
                {
                    MenuName = "预出库管理",
                    Operation = "预出库-自动分配",
                    OrderType = "PreOrder",
                    Controller = Request.RawUrl,
                    Creator = base.UserInfo.Name,
                    CreateTime = DateTime.Now,
                    ProjectID = (int)base.UserInfo.ProjectID,
                    ProjectName = base.UserInfo.ProjectName,
                    OrderID = ids
                };
                cords.Add(cord);
            }
            PreOrderSearchCondition sc = new PreOrderSearchCondition();
            sc.Ids = list;
            var response = new PreOrderService().GetPreOrder(new PreOrderRequest
            {
                SearchCondition = sc
            });

            if (response.IsSuccess && response.Result.SearchCondition != null && response.Result.SearchCondition.Any())
            {
                #region 验证取消单，不能分配
                //查询订单是否是取消单调不同存过
                IEnumerable<WMSConfig> wms = null;
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("OrderCancel_" + base.UserInfo.ProjectName);
                }
                catch
                { }
                if (wms == null)
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("OrderCancel");
                }

                IEnumerable<PreOrder> checkpreoders = new DeliverConfirmService().ValidOrderCancel(response.Result.SearchCondition.ToList(), wms.FirstOrDefault().Name, 4);
                if (checkpreoders != null && checkpreoders.Any())
                {
                    //存在取消单
                    return Json(new { Errorcode = 2, data = checkpreoders.Select(m => m.ExternOrderNumber).ToList() });
                }
                #endregion
                IEnumerable<DistributionInformation> DisInfo = null;
                if (response.Result.SearchCondition.Count() > 0)
                {
                    response.Result.SearchCondition.GroupBy(q => new { q.CustomerID, q.WarehouseId }).Each((i, g) => 
                    this.AutomaticAllocation(g.Select(a => new PreOrderIds { ID = a.ID }), g.First().CustomerID.Value, g.First().WarehouseId.Value, ref DisInfo));
                    new LogOperationService().AddLogOperation(logs);
                    //Cord By Young
                    //var co= CordHelper.AddWMS_Cord_FeedBack(cords);
                    //新增字段 2017-07-14  nike需要分配失败明细加入 article 和 size
                    List<ProductSearch> productListS = new List<ProductSearch>();
                    IEnumerable<ProductSearch> productList;
                    foreach (DistributionInformation item in DisInfo)
                    {
                        ProductSearch ps = new ProductSearch();
                        ps.SKU = item.SKU;
                        productListS.Add(ps);
                    }
                    productList = ApplicationConfigHelper.GetSearchProduct(response.Result.SearchCondition.FirstOrDefault().CustomerID.Value, productListS, "SKU");
                    foreach (DistributionInformation item in DisInfo)
                    {
                        item.Article = productList.Where(m => m.SKU == item.SKU).Select(m => m.Str10).FirstOrDefault();
                        item.Size = productList.Where(m => m.SKU == item.SKU).Select(m => m.Str9).FirstOrDefault();
                    }
                }

                var data = from q in DisInfo
                           select new
                           {
                               q.POID,
                               q.Customer,
                               q.ExternOrderNumber,
                               q.Note,
                               q.Type,
                               q.SKU,
                               q.QTY,
                               q.Article,
                               q.Size,
                               Message = ApplicationConfigHelper.GetWMS_Config("PreOrderStatus").Where(a => a.Code == q.Note).FirstOrDefault().Name,
                           };
                return Json(new { Errorcode = 1, data = data });
            }
            else
            {
                return Json(new { Errorcode = 0, data = "操作失败" });
            }

        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="preorderlist"></param>
        /// <param name="ids"></param>
        /// <param name="Criterion"></param>
        /// <returns></returns>
        [HttpPost]
        public string Cancel(string ids, string CustomerID, string reasonCode = "", string reasonRemark = "")//, string Criterion
        {
            PreOrderService preService = new PreOrderService();//实例化预出库单的业务服务类
            IEnumerable<string> number = ids.Split(',').Select(s => { return s.Trim(); });
            //    Enumerable.Empty<string>();
            //if (ids.IndexOf(',') > 0)
            //{
            //    number = ids.Split(',').Select(s => { return s.Trim(); }); //截取逗号并移除空格
            //}
            //number
            List<PreOrderBackStatus> list = new List<PreOrderBackStatus>();

            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            foreach (var item in number)
            {
                PreOrderBackStatus pre = new PreOrderBackStatus();
                pre.ID = Convert.ToInt64(item);
                pre.Updator = base.UserInfo.Name;
                pre.UpdateTime = DateTime.Now;
                list.Add(pre);

                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "预出库管理";
                operation.Operation = "预出库-取消";
                operation.OrderType = "PreOrder";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = pre.ID.ToString();
                logs.Add(operation);
            }
            var success = preService.Cancel(list, CustomerID, reasonCode, reasonRemark);//调用业务服务类的取消方法//Criterion
            new LogOperationService().AddLogOperation(logs);
            return success;//返回这个对象

        }

        public JsonResult OrderFinish(string ids)
        {
            PreOrderService preService = new PreOrderService();//实例化预出库单的业务服务类
            IEnumerable<string> number = ids.Split(',').Select(s => { return s.Trim(); });
            //number
            List<PreOrderBackStatus> list = new List<PreOrderBackStatus>();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            foreach (var item in number)
            {
                PreOrderBackStatus pre = new PreOrderBackStatus();
                pre.ID = Convert.ToInt64(item);
                pre.Updator = base.UserInfo.Name;
                pre.UpdateTime = DateTime.Now;
                list.Add(pre);

                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "预出库管理";
                operation.Operation = "预出库-完成";
                operation.OrderType = "PreOrder";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = pre.ID.ToString();
                logs.Add(operation);
            }
            var success = preService.OrderFinish(list);//调用业务服务类的取消方法//Criterion
            if (success.Trim() == "成功")
            {
                new LogOperationService().AddLogOperation(logs);
                return Json(new { Code = 1 });//返回这个对象
            }
            return Json(new { Code = 0 });//返回这个对象
        }

        //private object Allocate(IEnumerable<long> enumerable, System.Nullable<long> systemNullable, long p)
        //{
        //    throw new NotImplementedException();
        //} 

        /// <summary>
        /// 现场分配
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseID"></param>
        /// <param name="DisInfo"></param>      
        private void WorkersAllocation(IEnumerable<PreOrderIds> IDs, long? CustomerID, long WareHouseID, ref IEnumerable<DistributionInformation> DisInfo)
        {
            Object[] parameters = new Object[7];
            parameters[0] = "现场分配";
            parameters[1] = "全部分配";
            parameters[2] = 0;
            parameters[3] = "0";
            parameters[4] = base.UserInfo.Name;
            parameters[5] = IDs;
            //IEnumerable<PreOrderDetail> Request=null;
            parameters[6] = null;
            string allocateInstanceName = allocateInstanceNameStr(CustomerID, WareHouseID);
            //未找到类型“Runbow.TWS.Web.AllocateInstances.DefaultAllocateForSystem”上的构造函数。

            IAllocate allocateInstance = Activator.CreateInstance(Type.GetType(allocateInstanceName), parameters) as IAllocate;
            DisInfo = allocateInstance.Allocate();
            //settledInstance.SettledPodForReceive(podIDs, base.UserInfo.Name, base.UserInfo.ProjectID, SettledType, customerOrShipperID, relatedCustomerID, message);
        }

        /// <summary>
        /// 自动分配
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseID"></param>
        /// <param name="DisInfo"></param>      
        private void AutomaticAllocation(IEnumerable<PreOrderIds> IDs, long? CustomerID, long WareHouseID, ref IEnumerable<DistributionInformation> DisInfo)
        {
            Object[] parameters = new Object[7];
            parameters[0] = "自动分配";
            parameters[1] = "全部分配";
            parameters[2] = 0;
            parameters[3] = CustomerID.Value.ToString();
            parameters[4] = base.UserInfo.Name;
            parameters[5] = IDs;
            //IEnumerable<PreOrderDetail> Request=null;
            parameters[6] = null;
            string allocateInstanceName = allocateInstanceNameStr(CustomerID, WareHouseID);
            //未找到类型“Runbow.TWS.Web.AllocateInstances.DefaultAllocateForSystem”上的构造函数。

            IAllocate allocateInstance = Activator.CreateInstance(Type.GetType(allocateInstanceName), parameters) as IAllocate;
            DisInfo = allocateInstance.Allocate();
            //settledInstance.SettledPodForReceive(podIDs, base.UserInfo.Name, base.UserInfo.ProjectID, SettledType, customerOrShipperID, relatedCustomerID, message);

        }
        /// <summary>
        /// 手动分配
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseID"></param>
        /// <param name="DisInfo"></param>
        private void ManualAllocate(IEnumerable<PreOrderDetail> Request, long ID, long? CustomerID, long WareHouseID, string Criterion, ref IEnumerable<DistributionInformation> DisInfo)
        {
            Object[] parameters = new Object[7];
            parameters[0] = "手动分配";
            parameters[1] = Criterion;
            parameters[2] = ID;
            parameters[3] = CustomerID.ToString();
            parameters[4] = base.UserInfo.Name;
            parameters[5] = null;
            parameters[6] = Request;
            string allocateInstanceName = allocateInstanceNameStr(CustomerID, WareHouseID);
            //未找到类型“Runbow.TWS.Web.AllocateInstances.DefaultAllocateForSystem”上的构造函数。
            IAllocate allocateInstance = Activator.CreateInstance(Type.GetType(allocateInstanceName), parameters) as IAllocate;
            DisInfo = allocateInstance.Allocate();
            //settledInstance.SettledPodForReceive(podIDs, base.UserInfo.Name, base.UserInfo.ProjectID, SettledType, customerOrShipperID, relatedCustomerID, message);
        }

        /// <summary>
        /// 指定分配
        /// </summary>
        /// <param name="IDs"></param>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseID"></param>
        /// <param name="DisInfo"></param>
        private void AssignedManualAllocate(IEnumerable<PreOrderDetail> Request, long ID, long? CustomerID, long WareHouseID, string Criterion, ref IEnumerable<DistributionInformation> DisInfo)
        {
            Object[] parameters = new Object[7];
            parameters[0] = "指定分配";
            parameters[1] = Criterion;
            parameters[2] = ID;
            parameters[3] = CustomerID.ToString();
            parameters[4] = base.UserInfo.Name;
            parameters[5] = null;
            parameters[6] = Request;
            string allocateInstanceName = assignedallocateInstanceNameStr(CustomerID, WareHouseID);
            //未找到类型“Runbow.TWS.Web.AllocateInstances.DefaultAllocateForSystem”上的构造函数。
            IAllocate allocateInstance = Activator.CreateInstance(Type.GetType(allocateInstanceName), parameters) as IAllocate;
            DisInfo = allocateInstance.Allocate();
            //settledInstance.SettledPodForReceive(podIDs, base.UserInfo.Name, base.UserInfo.ProjectID, SettledType, customerOrShipperID, relatedCustomerID, message);
        }
        public void TransData(long? CustomerID, long WareHouseID, ref DataSet transData, ref string message)
        {
            if (ApplicationConfigHelper.GetCacheInfo().Where(p => p.UserName == base.UserInfo.Name).ToList().Count() <= 0)
            {
                message = "用户" + base.UserInfo.DisplayName + "没有分配仓库!";
                return;
            }
            Object[] parameters = new Object[5];
            parameters[0] = "PreOrder";
            parameters[1] = CustomerID;
            parameters[2] = base.UserInfo.ProjectID;
            if (WareHouseID == 0)
            {
                parameters[3] = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && c.CustomerID == CustomerID).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct().FirstOrDefault().WarehouseID;
            }
            else
            {
                parameters[3] = WareHouseID;
            }
            parameters[4] = transData;

            string transDataInstanceName = transDataInstanceNameStr(CustomerID, long.Parse(parameters[3].ToString()));
            ITransData transDataInstance = Activator.CreateInstance(Type.GetType(transDataInstanceName), parameters) as ITransData;
            transData = transDataInstance.TransData(ref message);
        }
        /// <summary>
        /// 指定分配参数
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        private string assignedallocateInstanceNameStr(long? CustomerID, long WareHouseID)
        {
            if (ApplicationConfigHelper.GetApplicationConfig().ProjectCollection.Where(p => p.Id == base.UserInfo.ProjectID.ToString()).Count() > 0)
            {
                var allocateConfigCollection = ApplicationConfigHelper.GetApplicationConfig().ProjectCollection
                           .First(p => p.Id == base.UserInfo.ProjectID.ToString())
                           .AllocateConfigs.AllocateConfigCollection;

                string allocateInstanceName = "Runbow.TWS.Web.AllocateInstances.DefaultAllocateForSystem";

                if (allocateConfigCollection != null && allocateConfigCollection.Any())
                {
                    var customerAllocateConfig = allocateConfigCollection.FirstOrDefault(t => t.CustomerID == CustomerID);

                    if (customerAllocateConfig != null)
                    {
                        allocateInstanceName = string.IsNullOrEmpty(customerAllocateConfig.DefaultAllocateInstance) ? allocateInstanceName : customerAllocateConfig.DefaultAllocateInstance;

                        if (customerAllocateConfig.WarehouseConfigCollection != null && customerAllocateConfig.WarehouseConfigCollection.Any())
                        {
                            var finalAllocateConfig = customerAllocateConfig.WarehouseConfigCollection.FirstOrDefault(t => t.WarehouseID == WareHouseID);

                            if (finalAllocateConfig != null)
                            {
                                allocateInstanceName = string.IsNullOrEmpty(finalAllocateConfig.AllocateInstance) ? allocateInstanceName : finalAllocateConfig.AllocateInstance;
                            }
                        }
                    }
                }
                return allocateInstanceName;
            }
            else
            {
                return "Runbow.TWS.Web.AllocateInstances.DefaultAllocateForSystem";
            }
        }
        /// <summary>
        /// 手动分配参数
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        private string allocateInstanceNameStr(long? CustomerID, long WareHouseID)
        {
            if (ApplicationConfigHelper.GetApplicationConfig().ProjectCollection.Where(p => p.Id == base.UserInfo.ProjectID.ToString()).Count() > 0)
            {
                var allocateConfigCollection = ApplicationConfigHelper.GetApplicationConfig().ProjectCollection
                           .First(p => p.Id == base.UserInfo.ProjectID.ToString())
                           .AllocateConfigs.AllocateConfigCollection;

                string allocateInstanceName = "Runbow.TWS.Web.AllocateInstances.DefaultAllocateForSystem";

                if (allocateConfigCollection != null && allocateConfigCollection.Any())
                {
                    var customerAllocateConfig = allocateConfigCollection.FirstOrDefault(t => t.CustomerID == CustomerID);

                    if (customerAllocateConfig != null)
                    {
                        allocateInstanceName = string.IsNullOrEmpty(customerAllocateConfig.DefaultAllocateInstance) ? allocateInstanceName : customerAllocateConfig.DefaultAllocateInstance;

                        if (customerAllocateConfig.WarehouseConfigCollection != null && customerAllocateConfig.WarehouseConfigCollection.Any())
                        {
                            var finalAllocateConfig = customerAllocateConfig.WarehouseConfigCollection.FirstOrDefault(t => t.WarehouseID == WareHouseID);

                            if (finalAllocateConfig != null)
                            {
                                allocateInstanceName = string.IsNullOrEmpty(finalAllocateConfig.AllocateInstance) ? allocateInstanceName : finalAllocateConfig.AllocateInstance;
                            }
                        }
                    }
                }
                return allocateInstanceName;
            }
            else
            {
                return "Runbow.TWS.Web.AllocateInstances.DefaultAllocateForSystem";
            }
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
        //private IsettledForPodNew CreateSettledInstance_New()
        //{
        //    IsettledForPodNew settled;
        //    int useNew = 0;
        //    string implementClassName = this.GetSettledInstanceName( ref useNew);
        //    if (!string.IsNullOrEmpty(implementClassName) && useNew == 1)
        //    {
        //        settled = Activator.CreateInstance(Type.GetType(implementClassName)) as IsettledForPodNew;

        //        if (settled == null)
        //        {
        //            settled = null;
        //        }
        //    }
        //    else
        //    {
        //        settled = null;
        //    }

        //    return settled;
        //}

        //private string GetSettledInstanceName(ref int UserNew)
        //{
        //    var allocateConfigCollection = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID).ProjectCollection
        //       .First(p => p.Id == base.UserInfo.ProjectID.ToString())
        //       .AllocateConfigs.AllocateConfigCollection;
        //     var settledConfig = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID).ProjectCollection
        //    //    .First(p => p.Id == base.UserInfo.ProjectID.ToString())
        //    //    .SettledPodConfigs.SettledPodConfigCollection
        //    //    .First(s => s.TargetID == customerOrShipperID && s.Target == target);

        //    UserNew = settledConfig.UseNew;

        //    return settledConfig.InstanceName;
        //}
        [HttpPost]
        public ActionResult GetBatchlist(string sku, long? CustomerID, string Warehouse, string BatchNumber, string GoodsType, string BoxNumber, string Unit, string Specifications, string UPC)
        {
            try
            {
                Warehouse=ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && c.WarehouseID == long.Parse(Warehouse)).FirstOrDefault().WarehouseName;
                BatchNumber = BatchNumber == "请选择" ? null : BatchNumber;
                BoxNumber = BoxNumber == "请选择" ? null : BoxNumber;
                Unit = Unit == "请选择" ? null : Unit;
                Specifications = Specifications == "请选择" ? null : Specifications;
                UPC = UPC == "请选择" ? null : UPC;
                var Product = new PreOrderService().GetBatchBySKU(sku, CustomerID, Warehouse, BatchNumber, GoodsType, BoxNumber, Unit, Specifications, UPC);
                if (Product != null)
                {
                    return Json(Product.Select(t => new { Sku = t.SKU, BatchNumber = t.BatchNumber, InventoryQty = t.InventoryQty, BoxNumber = t.BoxNumber, Unit = t.Unit, Specifications = t.Specifications }).Distinct(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                //throw;
            }
          
            return Json(new { Sku = string.Empty, BatchNumber = string.Empty }, JsonRequestBehavior.AllowGet);
        }

        public void ReportExportExecl(long? ID, string warename)
        {
            long CustomerID = ID ?? 0;
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            string WarehouseName = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetCacheInfo()
                    .Where(c => (c.CustomerID == CustomerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)))
                    .Where(c => c.WarehouseID == int.Parse(warename))
                    .Select(c => c.WarehouseName).FirstOrDefault();
            IEnumerable<Column> columnReceipt;
            IEnumerable<Column> columnReceiptDetail;
            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
            Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_PreOrder").Count() == 0)
            {
                columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrder").ColumnCollection;
            }
            else
            {
                columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_PreOrder").ColumnCollection;
            }
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_PreOrderDetail").Count() == 0)
            {
                columnReceiptDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
            }
            else
            {
                columnReceiptDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_PreOrderDetail").ColumnCollection;
            }
            columnReceipt = columnReceipt.Where(c => (c.IsImportColumn == true));
            columnReceiptDetail = columnReceiptDetail.Where(c => (c.IsImportColumn == true));
            ExportDemo(WarehouseName, columnReceipt, columnReceiptDetail);
        }

        private void ExportDemo(string warehousename, IEnumerable<Column> columnReceipt, IEnumerable<Column> columnReceiptDetail)
        {
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            DataTable dtReceiptDetail = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                if (receipt.DisplayName != "客户" && receipt.DisplayName != "预出库单号" && receipt.DisplayName != "明细数量")
                {
                    dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
                }
            }
            foreach (var receiptDetail in columnReceiptDetail)
            {
                if (receiptDetail.DisplayName != "客户" && receiptDetail.DisplayName != "预出库单号" && receiptDetail.DisplayName != "仓库编号" 
                    && receiptDetail.DisplayName != "产品名称" && receiptDetail.DisplayName != "行号" && receiptDetail.DisplayName != "可用数量" 
                    && receiptDetail.DisplayName != "已分配数量")
                {
                    dtReceiptDetail.Columns.Add(receiptDetail.DisplayName, typeof(string));
                }
            }
            DataRow dr1 = dtReceipt.NewRow();
            dr1["外部单号"] = "test" + DateTime.Now.ToString("yyyyMMdd") + "001";
            dr1["仓库名称"] = warehousename;
            dr1["订单日期"] = DateTime.Now.ToString("yyyy-MM-dd");
            dr1["预出库单类型"] = "领用发货";
            dtReceipt.Rows.Add(dr1);

            DataRow dr11 = dtReceiptDetail.NewRow();
            dr11["外部单号"] = "test" + DateTime.Now.ToString("yyyyMMdd") + "001";
            dr11["仓库"] = warehousename;
            dr11["SKU"] = "10000000005031";
            dr11["产品等级"] = "A品";
            dr11["期望数量"] = 10;
            dtReceiptDetail.Rows.Add(dr11);

            dtReceipt.TableName = "预出库单主信息";
            dtReceiptDetail.TableName = "预出库单明细信息";
            ds.Tables.Add(dtReceipt);
            ds.Tables.Add(dtReceiptDetail);
            EPPlusOperation.ExportDataSetByEPPlus(ds, "预出库单导入模板" + DateTime.Now.ToString("yyyy-MM-dd"));
        }

        [HttpPost]  //退货仓 导入模板
        public string ImputEcecl_TH(string CustomerName, long CustomerID, string WarehouseName, long WarehouseID)
        {
            PreOrderViewModel vm = new PreOrderViewModel();
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    if (ds == null)
                    {
                        return new { result = "<h3><font color='#FF0000'>Excel格式有误</font></h3>", IsSuccess = false }.ToJsonString();
                    }
                    try
                    {
                        DataTable dtpo = new DataTable();
                        DataTable dtpoloadkey = new DataTable();
                        dtpo = ds.Tables[0];        //获取第一张表
                        //dtpoloadkey = ds.Tables[1];  //获取第二张表
                        List<PreOrder> preOrders = new List<PreOrder>();
                        List<PreOrderDetail> preOrderDetails = new List<PreOrderDetail>();

                        #region 验证空值
                        //List<ProductSearch> searprolistfs = new List<ProductSearch>();
                        //string aa = "";
                        //string bb = "";
                        //string cc = "";
                        //for (int i = 0; i < dtpo.Rows.Count; i++)
                        //{
                        //    ProductSearch psfs = new ProductSearch();
                        //    aa = dtpo.Rows[i]["sku"].ToString();
                        //    bb = aa.Substring(6, 3);
                        //    cc = aa.Substring(9);
                        //    aa = aa.Substring(0, 6);
                        //    psfs.Str10 = aa + "-" + bb;
                        //    psfs.Str9 = cc;
                        //    psfs.Str8 = "01";
                        //    searprolistfs.Add(psfs);
                        //}
                        #endregion

                        #region 获得SKU集合
                        //IEnumerable<ProductSearch> productListnso;
                        //IEnumerable<ProductSearch> productListfs;

                        //productListnso = ApplicationConfigHelper.GetSearchProduct(CustomerID, searprolistfs, "nso");

                        //productListfs = ApplicationConfigHelper.GetSearchProduct(CustomerID, searprolistfs, "Acticle");

                        #endregion
                        for (int i = 0; i < dtpo.Rows.Count; i++)
                        {
                            string address1 = dtpo.Rows[i]["地址1"].ToString();
                            string address2 = dtpo.Rows[i]["地址1"].ToString();
                            string address3 = dtpo.Rows[i]["地址1"].ToString();
                            string address4 = dtpo.Rows[i]["地址1"].ToString();
                            //主表数据
                            preOrders.Add(new PreOrder()
                            {
                                //ExternOrderNumber = "",
                                //CustomerID = CustomerID,
                                //CustomerName = CustomerName,
                                //Warehouse = WarehouseName,
                                //OrderTime = DateTime.Parse(dtpo.Rows[i]["计划发货时间"].ToString()),cid
                                //City = dtpo.Rows[i]["城市"].ToString(),
                                //Address = address1 + "&" + address2 + "&" + address3 + "&" + address4,
                                //str3= dtpo.Rows[i]["Division Code"].ToString(),
                                //str4= dtpo.Rows[i]["NFS店铺编码"].ToString(),
                                //str5= dtpo.Rows[i]["公司名"].ToString(),
                                //str7= dtpo.Rows[i]["计划发货时间"].ToString(),
                                //str8 = dtpo.Rows[i]["VAS Code"].ToString()




                            });

                            //明细表数据
                            preOrderDetails.Add(new PreOrderDetail()
                            {

                            });


                        }
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }




                    //var response = new PreOrderService().AddPreOrderAndPreOrderDetail_TH(CustomerID.ToString(), request, base.UserInfo.Name);
                    //if (response.IsSuccess)
                    //{
                    //    StringBuilder returnSb = new StringBuilder();
                    //    response.Result.PreOrderList.OrderBy(a => a.Status).Each((b, h) =>
                    //    {
                    //        if (h.Status.Value <= 3)
                    //        {
                    //            new LogOperationService().AddLogOperation(logs);
                    //            returnSb.Append("<h3><font color='#33cc70'>" + h.ExternOrderNumber + " 导入成功！</font></h3>");
                    //        }
                    //        else
                    //        {
                    //            returnSb.Append("<h3><font color='#FF0000'>" + h.ExternOrderNumber + " 导入失败！</font></h3>");
                    //        }
                    //    });
                    //    return new { result = returnSb.ToString(), IsSuccess = false }.ToJsonString();
                    //}
                    //else
                    //{
                    //    return new { result = "<h3><font color='#FF0000'>批量导入失败！</font></h3>" + response.Result.ToString(), IsSuccess = false }.ToJsonString();
                    //}

                }
            }
            return new { result = "<h3>失败</h3>", IsSuccess = false }.ToJsonString();
        }

        /// <summary>
        /// 批量更新LoadKey
        /// </summary>
        /// <returns></returns>
        public ActionResult BatchUpdateLoadKey(PreOrderViewModel vm, int? PageIndex, long? customerID)
        {
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

            vm.SearchCondition = new PreOrderSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            //vm.HideActionButton = hideActionButton ??  false;
            //vm.ShowEditRelated = showEditRelated ?? true;
            // ViewBag.ShowSubmit = ShowSubmit;
            if (customerID == null)
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            else
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            vm.CustomerNames = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            if (vm.Warehouses != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var i in vm.Warehouses)
                {
                    sb.Append("'" + i.Value + "',");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            if (customerID == null)
            {
                StringBuilder sb = new StringBuilder();
                vm.CustomerNames = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                foreach (var i in vm.CustomerNames)
                {
                    sb.Append("" + i.Value + ",");
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

            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
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
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                    }
                }
            }
            var response = new PreOrderService().GetPreOrder(new PreOrderRequest
            {
                SearchCondition = vm.SearchCondition,
                PageIndex = 0,
                PageSize = UtilConstants.PAGESIZE

            });
            if (response.IsSuccess)
            {
                vm.SearchConditionResponse = response.Result.SearchCondition;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }

            this.GenQueryPodViewModel(vm);
            if (vm.Warehouses.Count() == 1)
            {
                vm.SearchCondition.Warehouse = vm.Warehouses.Select(c => c.Text).FirstOrDefault().ToString();
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }

        /// <summary>
        /// 批量更新LoadKey
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchUpdateLoadKey(PreOrderViewModel vm, int? PageIndex, string Action, long? customerID)
        {
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

            vm.SearchCondition = new PreOrderSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            //vm.HideActionButton = hideActionButton ??  false;
            //vm.ShowEditRelated = showEditRelated ?? true;
            // ViewBag.ShowSubmit = ShowSubmit;
            if (customerID == null)
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            else
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                      .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            vm.CustomerNames = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            if (vm.Warehouses != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var i in vm.Warehouses)
                {
                    sb.Append("'" + i.Value + "',");
                }
                if (sb.Length > 1)
                {
                    vm.SearchCondition.Warehouse = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            if (customerID == null)
            {
                StringBuilder sb = new StringBuilder();
                vm.CustomerNames = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                foreach (var i in vm.CustomerNames)
                {
                    sb.Append("" + i.Value + ",");
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

            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
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
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                                 .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = Convert.ToInt64(customerIDs.First().Value);
                    }
                }
            }
            var response = new PreOrderService().GetPreOrder(new PreOrderRequest
            {
                SearchCondition = vm.SearchCondition,
                PageIndex = 0,
                PageSize = UtilConstants.PAGESIZE

            });
            if (Action == "下载批量更新LoadKey模板")
            {
                IEnumerable<Column> columnLoadKey;
                var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_BatchUpdateLoadKey").Count() == 0)
                {
                    columnLoadKey = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_BatchUpdateLoadKey").ColumnCollection.Where(c => c.IsImportColumn == true);
                }
                else
                {
                    columnLoadKey = module.Tables.TableCollection.First(t => t.Name == "WMS_BatchUpdateLoadKey").ColumnCollection.Where(c => c.IsImportColumn == true);
                }
                AltLoadKey(null, columnLoadKey, Action);

            }
            else if (Action == "下载批量更新订单品级模板")
            {
                IEnumerable<Column> columnGoodsType;
                var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_BatchUpdateGoodsType").Count() == 0)
                {
                    columnGoodsType = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_BatchUpdateGoodsType").ColumnCollection.Where(c => c.IsImportColumn == true);
                }
                else
                {
                    columnGoodsType = module.Tables.TableCollection.First(t => t.Name == "WMS_BatchUpdateGoodsType").ColumnCollection.Where(c => c.IsImportColumn == true);
                }
                AltLoadKey(null, columnGoodsType, Action);

            }
            if (response.IsSuccess)
            {
                vm.SearchConditionResponse = response.Result.SearchCondition;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }

            this.GenQueryPodViewModel(vm);
            if (vm.Warehouses.Count() == 1)
            {
                vm.SearchCondition.Warehouse = vm.Warehouses.Select(c => c.Text).FirstOrDefault().ToString();
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);

        }

        /// <summary>
        /// 下载模板
        /// </summary>
        /// <param name="response"></param>
        /// <param name="columnLoadKey"></param>
        private void AltLoadKey(GetReceiptDetailByConditionResponse response, IEnumerable<Column> ColumnCount, string Action)
        {
            if (Action == "下载批量更新LoadKey模板")
            {
                DataSet ds = new DataSet();
                DataTable dtLoadKey = new DataTable();
                foreach (var receipt in ColumnCount)
                {
                    dtLoadKey.Columns.Add(receipt.DisplayName, typeof(string));
                }
                DataRow dr1 = dtLoadKey.NewRow();

                dr1["系统单号"] = "PRE1032020030000074";
                dr1["外部单号"] = "20200310F-3408Y-1400004";
                dr1["PLNO"] = "340120200308001";
                dr1["LoadKey"] = "202003101537001";
                dtLoadKey.Rows.Add(dr1);
                dtLoadKey.TableName = "批量更新LoadKey模板";
                ds.Tables.Add(dtLoadKey);
                //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "批量更新LoadKey模板");
                EPPlusOperation.ExportDataSetByEPPlus(ds, "批量更新LoadKey模板");
            }
            else if (Action == "下载批量更新订单品级模板")
            {
                DataSet ds = new DataSet();
                DataTable dtGoodsType = new DataTable();
                foreach (var receipt in ColumnCount)
                {
                    dtGoodsType.Columns.Add(receipt.DisplayName, typeof(string));
                }
                DataRow drs = dtGoodsType.NewRow();

                drs["入库单号"] = "RE1032020030000001";
                drs["箱号"] = "2020031400006";
                drs["SKU"] = "00091204257864";
                drs["货品等级"] = "A品";
                drs["期望数量"] = "10";
                dtGoodsType.Rows.Add(drs);
                dtGoodsType.TableName = "批量更新订单品级模板";
                ds.Tables.Add(dtGoodsType);
                //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "批量更新订单品级模板");
                EPPlusOperation.ExportDataSetByEPPlus(ds, "批量更新订单品级模板");
            }
        }

        /// <summary>
        /// 批量更新LoadKey
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public string BatchIimportUpdateLoadKey(long CustomerID, string Action)
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
                        if (Action == "批量更新LoadKey")
                        {
                            List<PreOrder> preOrders = new List<PreOrder>();
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                preOrders.Add(new PreOrder()
                                {
                                    PreOrderNumber = ds.Tables[0].Rows[i]["系统单号"].ToString(),
                                    ExternOrderNumber = ds.Tables[0].Rows[i]["外部单号"].ToString(),
                                    str9 = ds.Tables[0].Rows[i]["PLNO"].ToString(),
                                    str10 = ds.Tables[0].Rows[i]["LoadKey"].ToString()
                                });
                            }
                            //获取更新总数
                            int LoadKeyCount = ds.Tables[0].Rows.Count;
                            PreOrderRequest request = new PreOrderRequest();
                            request.PreOrderList = preOrders;
                            //判断数据是否存在
                            foreach (var item in preOrders)
                            {
                                var CountEON = new ASNManagementService().ExternKeyCheck_TH("", item.ExternOrderNumber, "", "1", CustomerID);
                                if (CountEON == 0)
                                {
                                    return new { result = "<h3><font color='#FF0000'>外部单号：" + item.ExternOrderNumber + "不存在!</font></h3>", IsSuccess = false }.ToJsonString();
                                }
                                var CountPON = new ASNManagementService().ExternKeyCheck_TH(item.PreOrderNumber, "", "", "2", CustomerID);
                                if (CountPON == 0)
                                {
                                    return new { result = "<h3><font color='#FF0000'>系统单号：" + item.PreOrderNumber + "不存在!</font></h3>", IsSuccess = false }.ToJsonString();
                                }
                                var CountPLNO = new ASNManagementService().ExternKeyCheck_TH("", "", item.str9, "3", CustomerID);
                                if (CountPLNO == 0)
                                {
                                    return new { result = "<h3><font color='#FF0000'>PLNO：" + item.str9 + "不存在!</font></h3>", IsSuccess = false }.ToJsonString();
                                }
                                var Countss = new ASNManagementService().ExternKeyCheck_TH(item.PreOrderNumber, item.ExternOrderNumber, item.str9, "4", CustomerID);
                                if (Countss == 0)
                                {
                                    return new { result = "<h3><font color='#FF0000'>外部单号：" + item.ExternOrderNumber + " 系统单号：" + item.PreOrderNumber + " PLNO：" + item.str9 + "关联数据不存在!</font></h3>", IsSuccess = false }.ToJsonString();
                                }

                            }
                            var response = new PreOrderService().BatchIimportUpdateLoadKey(CustomerID.ToString(), request);
                            if (response.IsSuccess)
                            {
                                return new { result = "<h3><font color='#00dd00'>批量更新LoadKey成功！</font></h3>", IsSuccess = true }.ToJsonString();
                            }
                            else
                            {
                                return new { result = "<h3><font color='#FF0000'>批量更新LoadKey失败！</font></h3>" + response.Result.ToString(), IsSuccess = false }.ToJsonString();
                            }
                        }
                        else if (Action == "批量更新订单品级")
                        {
                            List<ASNDetail> receiptDetails = new List<ASNDetail>();
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                receiptDetails.Add(new ASNDetail()
                                {
                                    ASNNumber = ds.Tables[0].Rows[i]["入库单号"].ToString(),
                                    str2 = ds.Tables[0].Rows[i]["箱号"].ToString(),
                                    SKU = ds.Tables[0].Rows[i]["SKU"].ToString(),
                                    QtyExpected = double.Parse(ds.Tables[0].Rows[i]["期望数量"].ToString()),
                                    GoodsType = ds.Tables[0].Rows[i]["货品等级"].ToString()
                                });
                            }
                            AddASNAndASNDetailRequest request = new AddASNAndASNDetailRequest();
                            request.asnDetail = receiptDetails;
                            //判断数据是否存在
                            foreach (var item in receiptDetails)
                            {
                                //入库单号
                                var CountReceiptNumber = new ASNManagementService().ExternKeyCheck_TH("", item.ASNNumber, "", "5", CustomerID);
                                if (CountReceiptNumber == 0)
                                {
                                    return new { result = "<h3><font color='#FF0000'>入库单号：" + item.ASNNumber + "不存在!</font></h3>", IsSuccess = false }.ToJsonString();
                                }
                                //SKU
                                var CountSKU = new ASNManagementService().ExternKeyCheck_TH(item.SKU, "", "", "6", CustomerID);
                                if (CountSKU == 0)
                                {
                                    return new { result = "<h3><font color='#FF0000'>SKU：" + item.SKU + "不存在!</font></h3>", IsSuccess = false }.ToJsonString();
                                }
                                //产品等级
                                var CountGoodsType = new ASNManagementService().ExternKeyCheck_TH(item.GoodsType, "", "", "7", CustomerID);
                                if (CountGoodsType == 0)
                                {
                                    return new { result = "<h3><font color='#FF0000'>货品等级：" + item.GoodsType + "不存在!</font></h3>", IsSuccess = false }.ToJsonString();
                                }
                                //入库单号与sku是否存在
                                var CountSKUorReceiptNumber = new ASNManagementService().ExternKeyCheck_TH(item.ASNNumber, item.SKU, "", "8", CustomerID);
                                if (CountSKUorReceiptNumber == 0)
                                {
                                    return new { result = "<h3><font color='#FF0000'>入库单号：" + item.ASNNumber + "中，SKU" + item.SKU + "不存在!</font></h3>", IsSuccess = false }.ToJsonString();
                                }
                                ///判断入库单状态是否可以进行操作，当状态为未上架 5，上架中 3 ，已上架 9 可以进行操作
                                var CountStatus = new ASNManagementService().ExternKeyCheck_TH(item.ASNNumber, "", "", "9", CustomerID);
                                if (CountStatus == 0)
                                {
                                    return new { result = "<h3><font color='#FF0000'>入库单： " + item.ASNNumber + " 当前状态不可进行操作！</font></h3>", IsSuccess = false }.ToJsonString();
                                }
                            }
                            var response = new PreOrderService().BatchIimportUpdateGoodsType(CustomerID.ToString(), request);
                            if (response.IsSuccess)
                            {
                                return new { result = "<h3><font color='#00dd00'>批量更新订单品级成功！</font></h3>", IsSuccess = true }.ToJsonString();
                            }
                            else if (response.SuccessMessage.ToString().Contains("失败，进行变更的数量与订单数量总和不符"))
                            {
                                return new { result = "<h3><font color='#FF0000'>批量更新订单品级失败！进行变更的SKU数量与订单SKU数量总和不符！</font></h3>", IsSuccess = false }.ToJsonString();
                            }
                            else
                            {
                                return new { result = "<h3><font color='#FF0000'>批量更新订单品级失败！</font></h3>" + response.Result.ToString(), IsSuccess = false }.ToJsonString();
                            }

                        }

                    }
                }
            }
            return new { result = "<h3>失败</h3>", IsSuccess = false }.ToJsonString();
        }


        /// <summary>
        /// 出库单消单页面，用于查询cord取消单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CancelOrderIndex(long? customerID)
        {
            return View();
        }

        /// <summary>
        /// 取消单界面查询条件加载
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCancelOrderWhere(long? customerID)
        {
            try
            {
                CancelOrderModel vm = new CancelOrderModel();
                vm.CancelOrderSearch = new CancelOrderSearchCondition();
                vm.CancelOrderSearch.StartCreateTime = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");//一个月
                vm.CancelOrderSearch.EndCreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
                var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
                var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                //ViewBag.CustomerList = CustomerList;
                vm.CustomerList = CustomerList;
                if (base.UserInfo.UserType == 0)
                {
                    vm.CancelOrderSearch.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        vm.CancelOrderSearch.CustomerID = customerID;
                    }
                    else
                    {
                        var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                        if (customerIDs != null && customerIDs.Count() == 1)
                        {
                            vm.CancelOrderSearch.CustomerID = customerIDs.First();
                        }
                        else
                        {
                            vm.CancelOrderSearch.CustomerID = 0;
                        }
                    }
                }
                IEnumerable<SelectListItem> WarehouseList = null;
                if (vm.CancelOrderSearch.CustomerID == 0)
                {
                    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                                        .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
                }
                else
                {
                    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.CancelOrderSearch.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseName, WarehouseName = t.WarehouseName }).Distinct()
                                         .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
                }
                vm.WarehouseList = WarehouseList;
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
        /// /查询取消订单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCancelOrderList(RequestModel request)
        {
            ResponseModel res = new ResponseModel();
            string msg = string.Empty;
            res.code = 401;
            CancelOrderSearchCondition searchCondition = new CancelOrderSearchCondition();
            try
            {
                searchCondition = JsonConvert.DeserializeObject<CancelOrderSearchCondition>(request.requestData);
            }
            catch (Exception ex)
            {
                res.msg = "查询条件有误";
                res.code = 402;
                return Json(new { res });
            }
            try
            {
                //查询数据
                if (searchCondition.CustomerID != null && searchCondition.CustomerID > 0)
                {
                    //得到要调用的存储过程
                    IEnumerable<WMSConfig> wms = null;
                    try
                    {
                        wms = ApplicationConfigHelper.GetWMS_Config("GetCancelOrder_" + base.UserInfo.ProjectName);
                    }
                    catch
                    {

                    }
                    if (wms == null)
                    {
                        wms = ApplicationConfigHelper.GetWMS_Config("GetCancelOrder");
                    }

                    searchCondition.PageIndex = request.page > 0 ? request.page - 1 : 0;
                    searchCondition.PageSize = request.limit > 0 ? request.limit : 20;
                    IEnumerable<CancelOrderInfo> list;
                    int rowcounts = 0;
                    list = new PreOrderService().GetCancelOrderList(searchCondition, wms.FirstOrDefault().Name, out msg, out rowcounts);
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
            return Json(new { res });
        }

    }
}

