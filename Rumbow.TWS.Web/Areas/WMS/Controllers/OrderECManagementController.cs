using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.WMS.Models.OrderManagement;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Common;
using UtilConstants = Runbow.TWS.Common.Constants;
using Runbow.TWS.Biz;
using Runbow.TWS.Web.Areas.WMS.Models.PreOrders;
using Newtonsoft.Json;
using System.Data;
using Runbow.TWS.MessageContracts.WMS.PreOrders;
using Runbow.TWS.Biz.WMS;
using System.Web.Script.Serialization;
using System.Text;
using Runbow.TWS.Entity.WMS.PreOrders;
using Runbow.TWS.MessageContracts.WMS.Order;
using SysIO = System.IO;
using MyFile = System.IO.File;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using Runbow.TWS.Dao.RabbitMQ;
using Runbow.TWS.Entity.RabbitMQ;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Runbow.TWS.Web.Interface;
using Runbow.TWS.Entity.WMS.Order;
using Runbow.TWS.Entity.WMS;
using Runbow.TWS.Common.Util;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.Web.Areas.WMS.Models.OrderECManagement;
using Runbow.TWS.Entity.WMS.Print;
using Runbow.TWS.MessageContracts.WMS.Print;
using Runbow.TWS.MessageContracts.WMS.DeliverConfirm;
using Runbow.TWS.Entity.WMS.DeliveryConfirm;
using Runbow.TWS.Entity.WMS.Log;
//using NPOI.SS.Formula.Functions;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class OrderECManagementController : BaseController
    {
        public ActionResult Index(long? customerID)
        {
            #region 注释
            //OrderViewModel vm = new OrderViewModel();
            //IEnumerable<WMSConfig> wms = null;
            //try
            //{
            //    wms = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName);
            //}
            //catch (Exception)
            //{
            //}

            //if (wms == null)
            //{
            //    wms = ApplicationConfigHelper.GetWMS_Config("OrderType");
            //}
            //List<SelectListItem> st = new List<SelectListItem>();
            //foreach (WMSConfig w in wms)
            //{
            //    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            //}
            //if (base.UserInfo.UserType == 2)
            //{
            //    vm.Customers = vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
            //                     .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //}
            //else if (base.UserInfo.UserType == 0)
            //{
            //    vm.Customers = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            //}
            //else
            //{
            //    vm.Customers = Enumerable.Empty<SelectListItem>();
            //}
            //vm.OrderType = st;
            //vm.SearchCondition = new OrderSearchCondition();
            //vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            //var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            //var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            //var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ////var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
            ////                         .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            //ViewBag.CustomerList = CustomerList;

            //var strlist = vm.Str19.Select(c => new SelectListItem() { Value = c.Value.ToString(), Text = c.Text });
            ////var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
            ////                         .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            //ViewBag.Str19List = strlist;
            //ViewBag.ProjectName = base.UserInfo.ProjectName;
            //vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-10);
            //vm.SearchCondition.EndCreateTime = DateTime.Now;

            //if (base.UserInfo.UserType == 0)
            //{
            //    vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            //}
            //else if (base.UserInfo.UserType == 2)
            //{
            //    if (customerID.HasValue)
            //    {
            //        vm.SearchCondition.CustomerID = customerID;
            //    }
            //    else
            //    {
            //        var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
            //        if (customerIDs != null && customerIDs.Count() == 1)
            //        {
            //            vm.SearchCondition.CustomerID = customerIDs.First();
            //        }
            //    }
            //}
            //IEnumerable<SelectListItem> WarehouseList = null;
            //if (vm.SearchCondition.CustomerID == null)
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                                        .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            //    StringBuilder sb = new StringBuilder();

            //    foreach (var i in CustomerListID)
            //    {
            //        sb.Append("" + i + ",");
            //    }
            //    if (sb.Length > 1)
            //    {
            //        vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
            //    }
            //    else
            //    {
            //        vm.SearchCondition.CustomerIDs = "0";
            //    }
            //}
            //else
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                         .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            //}

            //ViewBag.WarehouseList = WarehouseList;
            //if (CustomerList.Count() == 1)
            //{
            //    vm.SearchCondition.CustomerID = CustomerList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();
            //}
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Text).FirstOrDefault();

            //}
            #endregion

            OrderViewModel vm = new OrderViewModel();
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
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.Customers = CustomerList;
            vm.SearchCondition = new OrderSearchCondition();
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;

            var strlist = vm.Str19.Select(c => new SelectListItem() { Value = c.Value.ToString(), Text = c.Text });
            ViewBag.Str19List = strlist;
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            //vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-10);
            //vm.SearchCondition.EndCreateTime = DateTime.Now;
            vm.SearchCondition.StartCreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd HH:mm:ss"));
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


            var WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == vm.SearchCondition.Warehouse).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });

            ViewBag.WorkStation = WorkStation;
            var getOrderByConditionRequest = new GetOrderByConditionRequest();
            getOrderByConditionRequest.SearchCondition = vm.SearchCondition;
            getOrderByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getOrderByConditionRequest.PageIndex = 0;
            var getOrderByConditionResponse = new OrderManagementService().GetOrderHeaderByCondition(getOrderByConditionRequest);

            if (getOrderByConditionResponse.IsSuccess)
            {
                vm.OrderCollection = getOrderByConditionResponse.Result.OrderCollection;
                vm.PageIndex = getOrderByConditionResponse.Result.PageIndex;
                vm.PageCount = getOrderByConditionResponse.Result.PageCount;
            }
            GenQueryOrderAndOrderDetailViewModel(vm);
            return View(vm);
        }
        /// <summary>
        /// 订单状态统计
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public ActionResult StatusStatis(long CustomerID)
        {
            OrderViewModel vm = new OrderViewModel();

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

            //获取预出库状态
            List<SelectListItem> prest = new List<SelectListItem>();
            IEnumerable<WMSConfig> prewms = ApplicationConfigHelper.GetWMS_Config("PreOrderStatus");
            foreach (var item in prewms)
            {
                prest.Add(new SelectListItem() { Text = item.Name, Value = item.Code });
            }
            ViewBag.Prest = prest;

            vm.SearchCondition = new OrderSearchCondition();

            //获取customerList
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Where(m => m.CustomerID == CustomerID).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            if (CustomerList.Count() == 1)
            {
                vm.SearchCondition.CustomerID = CustomerList.Select(m => m.Value).FirstOrDefault().ObjectToInt32();
            }
            //获取客户对应的仓库
            var warehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID))
                               .Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                               .Select(c => new SelectListItem() { Value = c.WarehouseName, Text = c.WarehouseName });
            ViewBag.WarehouseList = warehouseList;

            if (warehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = warehouseList.Select(c => c.Text).FirstOrDefault();
            }

            ViewBag.ProjectName = base.UserInfo.ProjectName;
            vm.SearchCondition.StartCreateTime = DateTime.Now;
            vm.SearchCondition.EndCreateTime = DateTime.Now;

            var getOrderByConditionRequest = new GetOrderByConditionRequest();
            getOrderByConditionRequest.SearchCondition = vm.SearchCondition;
            getOrderByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getOrderByConditionRequest.PageIndex = 0;

            var getOrderByConditionResponse = new OrderManagementService().GetOrderStatusByCondition(getOrderByConditionRequest);

            if (getOrderByConditionResponse.IsSuccess)
            {
                vm.OrderCollection = getOrderByConditionResponse.Result.OrderCollection;
                vm.PageIndex = getOrderByConditionResponse.Result.PageIndex;
                vm.PageCount = getOrderByConditionResponse.Result.PageCount;
            }
            //GenQueryOrderAndOrderDetailViewModel(vm);
            return View(vm);
        }
        /// <summary>
        /// 订单状态统计
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult StatusStatis(OrderViewModel vm, string Action)
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
            vm.OrderType = st;

            //获取预出库状态
            List<SelectListItem> prest = new List<SelectListItem>();
            IEnumerable<WMSConfig> prewms = ApplicationConfigHelper.GetWMS_Config("PreOrderStatus");
            foreach (var item in prewms)
            {
                prest.Add(new SelectListItem() { Text = item.Name, Value = item.Code });
            }
            ViewBag.Prest = prest;

            //获取customerList
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Where(m => m.CustomerID == vm.SearchCondition.CustomerID).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;


            //获取客户对应的仓库
            var warehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID))
                               .Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                               .Select(c => new SelectListItem() { Value = c.WarehouseName, Text = c.WarehouseName });
            ViewBag.WarehouseList = warehouseList;


            ViewBag.ProjectName = base.UserInfo.ProjectName;

            var getOrderByConditionRequest = new GetOrderByConditionRequest();
            getOrderByConditionRequest.SearchCondition = vm.SearchCondition;

            var getOrderByConditionResponse = new OrderManagementService().GetOrderStatusByCondition(getOrderByConditionRequest);

            if (getOrderByConditionResponse.IsSuccess)
            {
                vm.OrderCollection = getOrderByConditionResponse.Result.OrderCollection;
                vm.PageIndex = getOrderByConditionResponse.Result.PageIndex;
                vm.PageCount = getOrderByConditionResponse.Result.PageCount;
            }
            //GenQueryOrderAndOrderDetailViewModel(vm);
            return View(vm);
        }
        /// <summary>
        /// 根据订单状态查询明细
        /// </summary>
        /// <returns></returns>
        public JsonResult SearchOrderTotal(long CustomerID, string Warehouose, int Status, DateTime? StartTime, DateTime? EndTime, int type)
        {
            IEnumerable<OrderInfo> info = null;//最后统计的订单信息

            OrderSearchCondition SearchCondition = new OrderSearchCondition();
            SearchCondition.CustomerID = CustomerID;
            SearchCondition.Warehouse = Warehouose;
            SearchCondition.Status = Status;
            SearchCondition.StartCreateTime = StartTime;
            SearchCondition.EndCreateTime = EndTime;
            var response = new OrderManagementService().SearchOrderTotal(SearchCondition, type);

            if (response.IsSuccess && response.Result.OrderCollection.Count() > 0)
            {
                info = response.Result.OrderCollection;
                var data = from q in info
                           select new
                           {
                               q.OrderNumber,
                               q.ExternOrderNumber,
                               q.str1,
                               q.OrderType,
                               q.Int1,
                               q.Warehouse
                           };
                return Json(new { Errorcode = 1, data = data });
            }
            else
            {
                return Json(new { Errorcode = 0, data = "未查询到数据" });
            }
        }
        public ActionResult TESTTabpage()
        {
            return View();
        }
        public string UpdatePrintStatus(string IDs)
        {
            return new OrderManagementService().UpdatePrintStatus(IDs);
        }
        public ActionResult WaveIndex(long? customerID)
        {
            OrderViewModel vm = new OrderViewModel();
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
            vm.SearchCondition = new OrderSearchCondition();
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
            //                         .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;
            ViewBag.UserName = base.UserInfo.Name.ToUpper();
            vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-6);
            vm.SearchCondition.EndCreateTime = DateTime.Now;
            vm.SearchCondition.Status = 1;
            vm.SearchCondition.IsHaveWave = "2";
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
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
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

            ViewBag.WarehouseList = WarehouseList;
            if (CustomerList.Count() == 1)
            {
                vm.SearchCondition.CustomerID = CustomerList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();
            }
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Text).FirstOrDefault();
                //ViewBag.WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == vm.SearchCondition.Warehouse).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = "INSTRUCTION_" + c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });
            }
            var WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == vm.SearchCondition.Warehouse).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });

            ViewBag.WorkStation = WorkStation;
            var getOrderByConditionRequest = new GetOrderByConditionRequest();
            getOrderByConditionRequest.SearchCondition = vm.SearchCondition;
            getOrderByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getOrderByConditionRequest.PageIndex = 0;
            var getOrderByConditionResponse = new OrderManagementService().GetOrderByCondition_Wave(getOrderByConditionRequest);

            if (getOrderByConditionResponse.IsSuccess)
            {
                vm.OrderCollection = getOrderByConditionResponse.Result.OrderCollection;
                vm.PageIndex = getOrderByConditionResponse.Result.PageIndex;
                vm.PageCount = getOrderByConditionResponse.Result.PageCount;
            }
            GenQueryOrderAndOrderDetailViewModel(vm);
            return View(vm);
        }
        [HttpPost]
        public string AllocatedWave(string IDS, string WaveNumber)
        {
            //string WaveNumber="";
            //WaveNumber="WAVE"+base.UserInfo.Name.ToUpper()+DateTime.Now.ToString("yyyyMMddHHmmss");
            OrderManagementService order = new OrderManagementService();
            var s = order.AllocatedWave(IDS, WaveNumber);
            return s.Result;

        }
        [HttpPost]
        public ActionResult Index(OrderViewModel vm, int? PageIndex, string Action)
        {
            //IEnumerable<WMSConfig> wms = null;
            //try
            //{
            //    wms = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName);
            //}
            //catch (Exception)
            //{
            //}

            //if (wms == null)
            //{
            //    wms = ApplicationConfigHelper.GetWMS_Config("OrderType");
            //}
            //List<SelectListItem> st = new List<SelectListItem>();
            //foreach (WMSConfig w in wms)
            //{
            //    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            //}
            //vm.OrderType = st;
            //var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            //var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            //var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            //ViewBag.CustomerList = CustomerList;
            //var strlist = vm.Str19.Select(c => new SelectListItem() { Value = c.Value.ToString(), Text = c.Text });
            ////var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
            ////                         .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            //ViewBag.Str19List = strlist;
            //ViewBag.ProjectName = base.UserInfo.ProjectName;
            //IEnumerable<SelectListItem> WarehouseList = null;
            //if (vm.SearchCondition.CustomerID == null)
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                                        .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            //    StringBuilder sb = new StringBuilder();

            //    foreach (var i in CustomerListID)
            //    {
            //        sb.Append("" + i + ",");
            //    }
            //    if (sb.Length > 1)
            //    {
            //        vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
            //    }
            //    else
            //    {
            //        vm.SearchCondition.CustomerIDs = "0";
            //    }
            //}
            //else
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                         .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            //}


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
            var strlist = vm.Str19.Select(c => new SelectListItem() { Value = c.Value.ToString(), Text = c.Text });
            ViewBag.Str19List = strlist;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
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
                vm.SearchCondition.CustomerID = 0;

            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            ViewBag.CustomerList = CustomerList;
            ViewBag.WarehouseList = WarehouseList;
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
            ViewBag.ProjectName = base.UserInfo.ProjectName;

            var WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == vm.SearchCondition.Warehouse).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });

            ViewBag.WorkStation = WorkStation;
            ViewBag.WarehouseList = WarehouseList;
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getOrderByConditionRequest = new GetOrderByConditionRequest();
            getOrderByConditionRequest.SearchCondition = vm.SearchCondition;
            getOrderByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getOrderByConditionRequest.PageIndex = PageIndex ?? 0;
            var getOrderByConditionResponse = new OrderManagementService().GetOrderHeaderByCondition(getOrderByConditionRequest);
            //var WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == vm.SearchCondition.Warehouse).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });

            ViewBag.WorkStation = WorkStation;
            if (getOrderByConditionResponse.IsSuccess)
            {
                vm.OrderCollection = getOrderByConditionResponse.Result.OrderCollection;
                vm.PageIndex = getOrderByConditionResponse.Result.PageIndex;
                vm.PageCount = getOrderByConditionResponse.Result.PageCount;
                if (Action == "导出")
                {
                    //一次导出1000条数据
                    getOrderByConditionRequest.PageSize = 1000;
                    getOrderByConditionResponse = new OrderManagementService().GetOrderImportByCondition(getOrderByConditionRequest);
                    IEnumerable<Column> columnOrder;
                    IEnumerable<Column> columnOrderDetail;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Order").Count() == 0)
                    {
                        columnOrder = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Order").ColumnCollection;
                    }
                    else
                    {
                        columnOrder = module.Tables.TableCollection.First(t => t.Name == "WMS_Order").ColumnCollection;
                    }
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_OrderDetail").Count() == 0)
                    {
                        columnOrderDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_OrderDetail").ColumnCollection;
                    }
                    else
                    {
                        columnOrderDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_OrderDetail").ColumnCollection;
                    }



                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnOrder = tables.First(t => t.Name == "WMS_Order").ColumnCollection;
                    //IEnumerable<Column> columnOrderDetail = tables.First(t => t.Name == "WMS_OrderDetail").ColumnCollection;
                    if (vm.SearchCondition.CustomerID == 0)
                    {
                        columnOrder = columnOrder.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                        columnOrderDetail = columnOrderDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                    }
                    else
                    {
                        var notKeyColumns1 = columnOrder.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.SearchCondition.CustomerID));
                        //             .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID)))
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
                        var notKeyColumns2 = columnOrderDetail.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.SearchCondition.CustomerID));
                        //             .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID)))
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
                        columnOrder = columnOrder.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true)).Union(notKeyColumns1.Where(c => c.IsShowInList));
                        columnOrderDetail = columnOrderDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true)).Union(notKeyColumns2.Where(c => c.IsShowInList));
                    }
                    Export(getOrderByConditionResponse.Result, columnOrder, columnOrderDetail);

                }
                if (Action == "合并")
                {
                    IList<OrderBackStatus> Orders = new List<OrderBackStatus>();
                    foreach (var str in getOrderByConditionResponse.Result.OrderCollection)
                    {
                        Orders.Add(new OrderBackStatus
                        {
                            ID = str.ID.ObjectToInt64(),
                            UpdateTime = DateTime.Now,
                            Updator = base.UserInfo.Name
                        });
                    }
                    var GetOutResult = new OrderManagementService().UnionOrder(Orders);
                    var getOrderByConditionResponsed = new OrderManagementService().GetOrderByCondition(getOrderByConditionRequest);
                    vm.OrderCollection = getOrderByConditionResponsed.Result.OrderCollection;
                    vm.PageIndex = getOrderByConditionResponsed.Result.PageIndex;
                    vm.PageCount = getOrderByConditionResponsed.Result.PageCount;
                }
            }
            GenQueryOrderAndOrderDetailViewModel(vm);
            if (base.UserInfo.UserType == 2)
            {
                vm.Customers = vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
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
            return View(vm);
        }
        public void ExportOrder(string ids, long CustomerID)
        {
            var getOrderByConditionResponse = new OrderManagementService().GetOrderByIDs(ids);
            IEnumerable<Column> columnOrder;
            IEnumerable<Column> columnOrderDetail;
            var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First();
            Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Order").Count() == 0)
            {
                columnOrder = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Order").ColumnCollection;
            }
            else
            {
                columnOrder = module.Tables.TableCollection.First(t => t.Name == "WMS_Order").ColumnCollection;
            }
            if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_OrderDetail").Count() == 0)
            {
                columnOrderDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_OrderDetail").ColumnCollection;
            }
            else
            {
                columnOrderDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_OrderDetail").ColumnCollection;
            }
            if (CustomerID == 0)
            {
                columnOrder = columnOrder.Where(c => (c.IsImportColumn == true));
                columnOrderDetail = columnOrderDetail.Where(c => (c.IsImportColumn == true));
            }
            else
            {
                columnOrder = columnOrder.Where(c => (c.IsImportColumn == true));
                columnOrderDetail = columnOrderDetail.Where(c => (c.IsImportColumn == true));
            }
            Export(getOrderByConditionResponse.Result, columnOrder, columnOrderDetail);
        }
        [HttpPost]
        public ActionResult WaveIndex(OrderViewModel vm, int? PageIndex, string Action)
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
            vm.OrderType = st;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            ViewBag.CustomerList = CustomerList;
            ViewBag.UserName = base.UserInfo.Name.ToUpper();
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
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
            ViewBag.WarehouseList = WarehouseList;
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;

            var getOrderByConditionRequest = new GetOrderByConditionRequest();
            getOrderByConditionRequest.SearchCondition = vm.SearchCondition;
            if (vm.SearchCondition.NowRowCount == null)
            {
                getOrderByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            }
            else
            {
                getOrderByConditionRequest.PageSize = vm.SearchCondition.NowRowCount.ObjectToInt32();
            }
            getOrderByConditionRequest.PageIndex = PageIndex ?? 0;
            var getOrderByConditionResponse = new OrderManagementService().GetOrderByCondition_Wave(getOrderByConditionRequest);

            if (getOrderByConditionResponse.IsSuccess)
            {
                vm.OrderCollection = getOrderByConditionResponse.Result.OrderCollection;
                vm.PageIndex = getOrderByConditionResponse.Result.PageIndex;
                vm.PageCount = getOrderByConditionResponse.Result.PageCount;
            }
            GenQueryOrderAndOrderDetailViewModel(vm);
            return View(vm);
        }
        public ActionResult OrderDetailView(long ID = 0, long CustomerID = 0)
        {
            OrderViewModel vm = new OrderViewModel();
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
            vm.SearchCondition = new OrderSearchCondition();
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getOrderAndOrderDetailByIDResponse = new OrderManagementService().GetOrderAndOrderDetailByCondition(ID);
            if (getOrderAndOrderDetailByIDResponse.IsSuccess)
            {
                vm.order = getOrderAndOrderDetailByIDResponse.Result.order;
                vm.OrderDetailCollection = getOrderAndOrderDetailByIDResponse.Result.OrderDetailCollection;
                vm.PageIndex = getOrderAndOrderDetailByIDResponse.Result.PageIndex;
                vm.PageCount = getOrderAndOrderDetailByIDResponse.Result.PageCount;
                vm.SearchCondition.CustomerID = CustomerID;
            }
            GenQueryOrderAndOrderDetailViewModel(vm);
            return View(vm);
        }
        private void GenQueryOrderAndOrderDetailViewModel(OrderViewModel vm)
        {
            var Configs = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection;
            if (Configs.Where(t => t.Name == "WMS_Order").Count() == 0)
            {
                vm.Config1 = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Order");
            }
            else
            {
                vm.Config1 = Configs.First(t => t.Name == "WMS_Order");
            }
            if (Configs.Where(t => t.Name == "WMS_OrderDetail").Count() == 0)
            {
                vm.Config2 = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_OrderDetail");
            }
            else
            {
                vm.Config2 = Configs.First(t => t.Name == "WMS_OrderDetail");
            }

            //var s = ((Projects)ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID,vm.SearchCondition.CustomerID)).ProjectCollection.First();
            //vm.Config1 = s.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Order");
            //vm.Config2 = s.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_OrderDetail");

        }
        public JsonResult GetProvince(string id, string name, string type)
        {
            var Province = ApplicationConfigHelper.GetProvinceList();
            if (type == "keydown")
            {
                return Json(Province.Where(s => s.Province.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.Province }), JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(Province.Where(s => s.Province == name).Select(t => new { Value = t.ID.ToString(), Text = t.Province }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCity(string province, string name, string type)
        {
            ApplicationConfigHelper.RefreshGetCityList(province);
            var City = ApplicationConfigHelper.GetCityList(province);
            if (type == "keydown")
            {
                return Json(City.Where(s => s.City.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.City }), JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(City.Where(s => s.City == name).Select(t => new { Value = t.ID.ToString(), Text = t.City }), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDistrict(string city, string name, string type)
        {
            ApplicationConfigHelper.RefreshGetDistrictList(city);
            var Districts = ApplicationConfigHelper.GetDistrictList(city);
            if (type == "keydown")
            {
                return Json(Districts.Where(s => s.District.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.District }), JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(Districts.Where(s => s.District == name).Select(t => new { Value = t.ID.ToString(), Text = t.District }), JsonRequestBehavior.AllowGet);
            }
        }
        public string PickOrConfirm(string ID, string type)
        {
            IList<OrderBackStatus> Orders = new List<OrderBackStatus>();
            if (ID.Contains(","))
            {
                string[] strs = ID.Split(',');
                foreach (var str in strs)
                {
                    Orders.Add(new OrderBackStatus
                    {
                        ID = str.ObjectToInt64(),
                        UpdateTime = DateTime.Now,
                        Updator = base.UserInfo.Name
                    });
                }
            }
            else
            {
                Orders.Add(new OrderBackStatus
                {
                    ID = ID.ObjectToInt64(),
                    UpdateTime = DateTime.Now,
                    Updator = base.UserInfo.Name
                });
            }
            var GetPickResult = new OrderManagementService().Pick(Orders, type, base.UserInfo.Name);
            new PickingService().CreatePickingAndDetail(base.UserInfo.Name, ID);
            return GetPickResult.Result;
        }
        public JsonResult AddInstructions(string ids, string WorkStation, string WarehouseQueue, int Priority = 0)
        {

            var response = new OrderManagementService().AddInstructions(ids, WorkStation.Trim(), "1", Priority, UserInfo.Name);
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
            return Json(new { Code = 0 });
        }
        public ActionResult Package(long ID)
        {
            OrderViewModel vm = new OrderViewModel();
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
            vm.order = new OrderInfo();
            //var GetPickResult = new OrderManagementService().Pick(ID, type, base.UserInfo.Name);
            var box = ApplicationConfigHelper.GetApplicationBox(base.UserInfo.ProjectName).Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name }); ;
            ViewBag.BoxList = box;
            var getOrderByConditionResponse = new OrderManagementService().GetPackageByCondition(ID);
            if (getOrderByConditionResponse.IsSuccess)
            {
                vm.OrderCollection = getOrderByConditionResponse.Result.OrderCollection;
                vm.PackageCollection = getOrderByConditionResponse.Result.packages;
                vm.PackageDetailCollection = getOrderByConditionResponse.Result.packageDetails;
                vm.OrderDetailCollection = getOrderByConditionResponse.Result.OrderDetailCollection;
            }
            return View(vm);
        }
        public ActionResult NikePackage(long ID)
        {
            OrderViewModel vm = new OrderViewModel();
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
            vm.order = new OrderInfo();
            //var GetPickResult = new OrderManagementService().Pick(ID, type, base.UserInfo.Name);
            var box = ApplicationConfigHelper.GetApplicationBox(base.UserInfo.ProjectName).Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name }); ;
            ViewBag.BoxList = box;
            var getOrderByConditionResponse = new OrderManagementService().GetPackageByCondition(ID);
            if (getOrderByConditionResponse.IsSuccess)
            {
                vm.OrderCollection = getOrderByConditionResponse.Result.OrderCollection;
                vm.PackageCollection = getOrderByConditionResponse.Result.packages;
                vm.PackageDetailCollection = getOrderByConditionResponse.Result.packageDetails;
                vm.OrderDetailCollection = getOrderByConditionResponse.Result.OrderDetailCollection;
                if (vm.PackageCollection != null && vm.PackageCollection.Any())
                {
                }
                else//没有箱子的时候
                {

                    ViewBag.FirstBox = GetMaxBoxnumber(vm.OrderCollection.Select(m => m.ID).FirstOrDefault().ToString());

                }
                //List<OrderDetailInfo> OdInfoList = new List<OrderDetailInfo>();
                //var OrderInfoList = vm.OrderDetailCollection.GroupBy;
                List<OrderDetailInfo> OdInfoList = new List<OrderDetailInfo>();
                OrderDetailInfo[] OrderInfoList = (OrderDetailInfo[])Clone<OrderDetailInfo>(vm.OrderDetailCollection).ToArray();
                //OrderDetailInfo[] OrderInfoList = (OrderDetailInfo[])vm.OrderDetailCollection.ToArray().Clone();
                for (int i = 0; i < OrderInfoList.Length; i++)
                {
                    bool Isexist = false;
                    for (int j = 0; j < OdInfoList.Count; j++)
                    {
                        if (OdInfoList[j].SKU == OrderInfoList[i].SKU)
                        {
                            Isexist = true;
                            OdInfoList[j].Qty += OrderInfoList[i].Qty;
                        }
                    }
                    if (!Isexist)
                    {
                        OdInfoList.Add(OrderInfoList[i]);
                    }

                }
                ViewBag.OrderDetailInfoList = OdInfoList.ToJsonString();
            }
            return View(vm);
        }
        /// <summary>
        /// YXDR包装
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult YXDRPackage(long ID)
        {
            OrderViewModel vm = new OrderViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName);//出库单类型
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
            vm.order = new OrderInfo();
            var box = ApplicationConfigHelper.GetApplicationBox(base.UserInfo.ProjectName).Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name }); ;
            ViewBag.BoxList = box;
            var getOrderByConditionResponse = new OrderManagementService().GetPackageByCondition(ID);
            if (getOrderByConditionResponse.IsSuccess)
            {
                vm.OrderCollection = getOrderByConditionResponse.Result.OrderCollection;
                vm.PackageCollection = getOrderByConditionResponse.Result.packages;
                vm.PackageDetailCollection = getOrderByConditionResponse.Result.packageDetails;
                vm.OrderDetailCollection = getOrderByConditionResponse.Result.OrderDetailCollection;
                if (vm.PackageCollection != null && vm.PackageCollection.Any())
                {
                }
                else//没有箱子的时候
                {
                    ViewBag.FirstBox = GetMaxBoxnumber(vm.OrderCollection.Select(m => m.ID).FirstOrDefault().ToString());
                }
                List<OrderDetailInfo> OdInfoList = new List<OrderDetailInfo>();
                OrderDetailInfo[] OrderInfoList = (OrderDetailInfo[])Clone<OrderDetailInfo>(vm.OrderDetailCollection).ToArray();
                for (int i = 0; i < OrderInfoList.Length; i++)
                {
                    bool Isexist = false;
                    for (int j = 0; j < OdInfoList.Count; j++)
                    {
                        if (OdInfoList[j].SKU == OrderInfoList[i].SKU)
                        {
                            Isexist = true;
                            OdInfoList[j].Qty += OrderInfoList[i].Qty;
                        }
                    }
                    if (!Isexist)
                    {
                        OdInfoList.Add(OrderInfoList[i]);
                    }

                }
                ViewBag.OrderDetailInfoList = OdInfoList.ToJsonString();
            }

            return View(vm);
        }
        [HttpPost]
        public string YXDRPackage(long ID, string JsonPackage, int flag)
        {
            var responseJsonFieldsets = jsonlist<PackageDetailInfo>(JsonPackage);
            DateTime d = DateTime.Now;
            AddPackageAndDetailRequest request = new AddPackageAndDetailRequest();
            IList<PackageInfo> packages = new List<PackageInfo>();
            IList<PackageDetailInfo> packageDetails = new List<PackageDetailInfo>();
            responseJsonFieldsets.Each((i, package) =>
            {
                packages.Add(new PackageInfo()
                {
                    Creator = base.UserInfo.Name,
                    CreateTime = d,
                    Updator = base.UserInfo.Name,
                    UpdateTime = d,
                    PackageType = package.str2,
                    PackageNumber = package.PackageNumber,
                    Length = package.Length,
                    Width = package.Width,
                    Height = package.Height,
                    NetWeight = package.NetWeight,
                    GrossWeight = package.GrossWeight,
                    PackageTime = d,
                    OID = ID
                });

                packageDetails.Add(new PackageDetailInfo()
                {
                    Creator = base.UserInfo.Name,
                    CreateTime = d,
                    Updator = base.UserInfo.Name,
                    UpdateTime = d,
                    PackageNumber = package.PackageNumber,
                    SKU = package.SKU,
                    UPC = package.UPC,
                    GoodsName = package.GoodsName,
                    GoodsType = package.GoodsType,
                    Qty = package.Qty
                });

            });
            request.packages = packages;
            request.packageDetails = packageDetails;
            var response = new OrderManagementService().AddPackageAndDetail(ID, request, flag);
            return response.Result;
        }
        public ActionResult BarCodePackage(long ID)
        {
            OrderViewModel vm = new OrderViewModel();
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
            vm.order = new OrderInfo();
            //var GetPickResult = new OrderManagementService().Pick(ID, type, base.UserInfo.Name);
            var box = ApplicationConfigHelper.GetApplicationBox(base.UserInfo.ProjectName).Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name }); ;
            ViewBag.BoxList = box;
            var getOrderByConditionResponse = new OrderManagementService().GetPackageByCondition(ID);
            if (getOrderByConditionResponse.IsSuccess)
            {
                vm.OrderCollection = getOrderByConditionResponse.Result.OrderCollection;
                vm.PackageCollection = getOrderByConditionResponse.Result.packages;
                vm.PackageDetailCollection = getOrderByConditionResponse.Result.packageDetails;
                vm.OrderDetailCollection = getOrderByConditionResponse.Result.OrderDetailCollection;
                if (vm.PackageCollection != null && vm.PackageCollection.Any())
                {
                }
                else//没有箱子的时候
                {

                    ViewBag.FirstBox = GetMaxBoxnumber(vm.OrderCollection.Select(m => m.ID).FirstOrDefault().ToString());

                }
                //List<OrderDetailInfo> OdInfoList = new List<OrderDetailInfo>();
                //var OrderInfoList = vm.OrderDetailCollection.GroupBy;
                List<OrderDetailInfo> OdInfoList = new List<OrderDetailInfo>();
                OrderDetailInfo[] OrderInfoList = (OrderDetailInfo[])Clone<OrderDetailInfo>(vm.OrderDetailCollection).ToArray();
                //OrderDetailInfo[] OrderInfoList = (OrderDetailInfo[])vm.OrderDetailCollection.ToArray().Clone();
                for (int i = 0; i < OrderInfoList.Length; i++)
                {
                    bool Isexist = false;
                    for (int j = 0; j < OdInfoList.Count; j++)
                    {
                        if (OdInfoList[j].SKU == OrderInfoList[i].SKU)
                        {
                            Isexist = true;
                            OdInfoList[j].Qty += OrderInfoList[i].Qty;
                        }
                    }
                    if (!Isexist)
                    {
                        OdInfoList.Add(OrderInfoList[i]);
                    }

                }
                ViewBag.OrderDetailInfoList = OdInfoList.ToJsonString();
            }
            IEnumerable<BarCodeInfo> list_barcode = new BarCodeService().GetBarCodeByOID(ID);
            vm.BarCodeCollection = list_barcode;
            return View(vm);
        }
        [HttpPost]
        public string BarCodePackage(long ID, string JsonPackage, int flag)
        {
            var responseJsonFieldsets = jsonlist<PackageDetailInfo>(JsonPackage);
            DateTime d = DateTime.Now;
            AddPackageAndDetailRequest request = new AddPackageAndDetailRequest();
            IList<PackageInfo> packages = new List<PackageInfo>();
            IList<PackageDetailInfo> packageDetails = new List<PackageDetailInfo>();

            IList<BarCodeInfo> listBarCode = new List<BarCodeInfo>();
            BarCodeInfo info;
            responseJsonFieldsets.Each((i, package) =>
            {
                packages.Add(new PackageInfo()
                {
                    Creator = base.UserInfo.Name,
                    CreateTime = d,
                    Updator = base.UserInfo.Name,
                    UpdateTime = d,
                    PackageType = package.str2,
                    PackageNumber = package.PackageNumber,
                    Length = package.Length,
                    Width = package.Width,
                    Height = package.Height,
                    NetWeight = package.NetWeight,
                    GrossWeight = package.GrossWeight,
                    PackageTime = d,
                    OID = ID
                });

                packageDetails.Add(new PackageDetailInfo()
                {
                    Creator = base.UserInfo.Name,
                    CreateTime = d,
                    Updator = base.UserInfo.Name,
                    UpdateTime = d,
                    PackageNumber = package.PackageNumber,
                    SKU = package.SKU,
                    UPC = package.UPC,
                    GoodsName = package.GoodsName,
                    GoodsType = package.GoodsType,
                    Qty = package.Qty
                });

                package.BarCodes.Split(',').Each((j, item) =>
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        info = new BarCodeInfo();
                        info.SKU = package.SKU;
                        info.BarCode = item;
                        info.Type = "出库单";
                        info.OrderID = Convert.ToInt64(ID);
                        info.OrderNumber = package.OrderNumber;
                        info.Status = 0;
                        info.CustomerID = package.CustomerID;
                        info.CustomerName = package.CustomerName;
                        info.PackageNumber = package.PackageNumber;
                        listBarCode.Add(info);
                    }

                });

            });
            request.packages = packages;
            request.packageDetails = packageDetails;
            //先删除箱号对应的barcode信息
            var response = new OrderManagementService().AddPackageAndDetail(ID, request, flag);
            if (response.IsSuccess == true)
            {
                var message = new BarCodeService().GenerateBarCodeOrder(listBarCode);
            }
            return response.Result;
        }
        public string GetSKUAndBarCodeByBarCode(string BarCode)
        {
            BarCodeService service = new BarCodeService();
            DataTable dt = service.GetSKUAndBarCodeByBarCode(BarCode);
            return DataTableConvertJson.DataTable2Json(dt);
        }
        public static List<T> Clone<T>(object List)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, List);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as List<T>;
            }
        }
        [HttpPost]
        public string Package(long ID, string JsonPackage, int flag)
        {
            var responseJsonFieldsets = jsonlist<PackageDetailInfo>(JsonPackage);
            DateTime d = DateTime.Now;
            AddPackageAndDetailRequest request = new AddPackageAndDetailRequest();
            IList<PackageInfo> packages = new List<PackageInfo>();
            IList<PackageDetailInfo> packageDetails = new List<PackageDetailInfo>();
            responseJsonFieldsets.Each((i, package) =>
                    {
                        packages.Add(new PackageInfo()
                        {
                            Creator = base.UserInfo.Name,
                            CreateTime = d,
                            Updator = base.UserInfo.Name,
                            UpdateTime = d,
                            PackageType = package.str2,
                            PackageNumber = package.PackageNumber,
                            Length = package.Length,
                            Width = package.Width,
                            Height = package.Height,
                            NetWeight = package.NetWeight,
                            GrossWeight = package.GrossWeight,
                            PackageTime = d,
                            OID = ID
                        });

                        packageDetails.Add(new PackageDetailInfo()
                        {
                            Creator = base.UserInfo.Name,
                            CreateTime = d,
                            Updator = base.UserInfo.Name,
                            UpdateTime = d,
                            PackageNumber = package.PackageNumber,
                            SKU = package.SKU,
                            UPC = package.UPC,
                            GoodsName = package.GoodsName,
                            GoodsType = package.GoodsType,
                            Qty = package.Qty
                        });

                    });
            request.packages = packages;
            request.packageDetails = packageDetails;
            var response = new OrderManagementService().AddPackageAndDetail(ID, request, flag);
            return response.Result;
        }
        [HttpPost]
        public string NikePackage(long ID, string JsonPackage, int flag)
        {
            var responseJsonFieldsets = jsonlist<PackageDetailInfo>(JsonPackage);
            DateTime d = DateTime.Now;
            AddPackageAndDetailRequest request = new AddPackageAndDetailRequest();
            IList<PackageInfo> packages = new List<PackageInfo>();
            IList<PackageDetailInfo> packageDetails = new List<PackageDetailInfo>();
            responseJsonFieldsets.Each((i, package) =>
            {
                packages.Add(new PackageInfo()
                {
                    Creator = base.UserInfo.Name,
                    CreateTime = d,
                    Updator = base.UserInfo.Name,
                    UpdateTime = d,
                    PackageType = package.str2,
                    PackageNumber = package.PackageNumber,
                    Length = package.Length,
                    Width = package.Width,
                    Height = package.Height,
                    NetWeight = package.NetWeight,
                    GrossWeight = package.GrossWeight,
                    PackageTime = d,
                    OID = ID
                });

                packageDetails.Add(new PackageDetailInfo()
                {
                    Creator = base.UserInfo.Name,
                    CreateTime = d,
                    Updator = base.UserInfo.Name,
                    UpdateTime = d,
                    PackageNumber = package.PackageNumber,
                    SKU = package.SKU,
                    UPC = package.UPC,
                    GoodsName = package.GoodsName,
                    GoodsType = package.GoodsType,
                    Qty = package.Qty
                });

            });
            request.packages = packages;
            request.packageDetails = packageDetails;
            var response = new OrderManagementService().AddPackageAndDetail(ID, request, flag);
            return response.Result;
        }
        [HttpPost]
        public string OrderBackStatus(string ID, int ToStatus, int type, string username, string password)
        {
            GetOrderByConditionRequest request = new GetOrderByConditionRequest();
            IList<OrderBackStatus> Orders = new List<OrderBackStatus>();
            if (ID.Contains(","))
            {
                string[] strs = ID.Split(',');
                foreach (var str in strs)
                {
                    Orders.Add(new OrderBackStatus
                    {
                        ID = str.ObjectToInt64(),
                        UpdateTime = DateTime.Now,
                        Updator = base.UserInfo.Name
                    });

                }
            }
            else
            {
                Orders.Add(new OrderBackStatus
                {
                    ID = ID.ObjectToInt64(),
                    UpdateTime = DateTime.Now,
                    Updator = base.UserInfo.Name
                });
            }
            request.Orders = Orders;

            var GetBackResult = new OrderManagementService().OrderBackStatus(request, ToStatus, type);
            return GetBackResult.Result;
        }
        [HttpGet]
        public ActionResult InventoryOfOutbound(string Ids, long? CustomerId, string Warehouse)
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
            vm.ViewType = 1;
            vm.SearchCondition = new PreOrderSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            InventoryOfOutboundRequest Request = new InventoryOfOutboundRequest();
            //vm.ViewType = ViewType;
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            //vm.HideActionButton = hideActionButton ?? false;
            //vm.ShowEditRelated = showEditRelated ?? true;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;

            //vm.Customers = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetStorerID(base.UserInfo.ID)
            //               .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name.ToString() });
            if (CustomerId == null)
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value))
                                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
                CustomerId = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                    .Select(a => a.CustomerID).FirstOrDefault();
                // .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName }); 
                // customerID = Convert.ToInt64(vm.Customers.First().Value); 
            }
            else
            {
                vm.Warehouses = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == CustomerId && c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)))
                                       .Select(c => new SelectListItem() { Value = c.WarehouseName, Text = c.WarehouseName });
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
            //if (ViewType != 1)
            //{
            PreOrderSearchCondition SearchCondition = new PreOrderSearchCondition();
            SearchCondition.CustomerID = CustomerId;
            SearchCondition.Warehouse = Warehouse;
            SearchCondition.OrderTime = DateTime.Now;
            var getPreOrderByConditionResponse = new PreOrderService().GetInventoryOfOutbound(new InventoryOfOutboundRequest()
            {
                Ids = Ids,
                CustomerId = CustomerId,
                Warehouse = Warehouse
                //CustomerId=CustomerId,
                //Area = Area,
                //Location = Location,
                //SKU = SKU,
                //GoodsType = GoodsType,
                //CustomerIds = CustomerIds
            });

            vm.PreAndDetail = getPreOrderByConditionResponse.Result;
            vm.PreAndDetail.PreOd.Each((a, h) =>
            {
                h.LineNumber = "0000" + (a + 1);
            });


            if (CustomerId != null)
            {
                if (base.UserInfo.UserType == 0)
                {
                    vm.PreAndDetail.SearchCondition = new PreOrderSearchCondition();
                    vm.PreAndDetail.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
                    vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                //else if (base.UserInfo.UserType == 1)
                //{

                //}
                else if (base.UserInfo.UserType == 2)
                {
                    if (CustomerId.HasValue)
                    {
                        vm.PreAndDetail.SearchCondition = SearchCondition;

                        //vm.PreAndDetail.SearchCondition = new PreOrderSearchCondition();
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
            this.GenQueryPodViewModel(vm);

            return View(vm);
        }
        public JsonResult InventoryOfOutboundJson(string CustomerId, string PreOrderJson, string Jaonstr)
        {
            PreOrderViewModel vm = new PreOrderViewModel();

            IEnumerable<PreOrderDetail> PreOrderDetails = jsonlist<PreOrderDetail>(Jaonstr);
            PreOrderRequest request = new PreOrderRequest();
            var responseJsonFieldsets = jsonlist<PreOrder>(PreOrderJson);
            responseJsonFieldsets.Each((i, o) =>
            {
                o.CustomerName = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(b => b.CustomerID == o.CustomerID)
                           .Select(c => c.CustomerName).FirstOrDefault();
                o.CreateTime = DateTime.Now;
                o.Creator = base.UserInfo.Name;
                //o.Status = 1;
            });
            PreOrderDetails.Each((i, od) =>
            {

                //od.SKU = od.SKU.Trim();
                //od.LineNumber = od.LineNumber.Trim();
                //od.GoodsName = od.GoodsName;
                //od.GoodsType = od.GoodsType;
                od.ExternOrderNumber = responseJsonFieldsets.First().ExternOrderNumber;
                od.Creator = base.UserInfo.Name;
                od.CustomerID = CustomerId.ObjectToInt64();
                od.CustomerName = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(b => b.CustomerID == od.CustomerID)
                           .Select(c => c.CustomerName).FirstOrDefault();
                //od.PreOrderNumber = PreOrderNumber;
                //od.ExternOrderNumber = ExternOrderNumber;
            });
            request.PreOrderList = responseJsonFieldsets;
            request.PreOd = PreOrderDetails;
            var Result = new PreOrderService().AddInventoryOfOutbound(request, base.UserInfo.Name);
            if (Result.IsSuccess)
            {
                return Json(new { Code = 1, Result = Result.Result });
            }
            //var response = new PreOrderService().ManualAllocationJson(new ManualAllocationRequest() { PodRequest = pod, CustomerId = CustomerId, Creator = base.UserInfo.Name });
            //vm.PreAndDetail = getPreOrderByConditionResponse.Result;
            return Json(new { Code = 0 });
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
        [HttpGet]
        public ActionResult PreOrderQuery(int? CustomerID)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            if (CustomerID == 0 || CustomerID == null)
            {
                ViewBag.CustomerList = CustomerList;
            }
            else
            {
                ViewBag.CustomerList = CustomerList.Where(c => c.Value == CustomerID.ToString());
            }
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
            vm.SearchCondition.CustomerID = CustomerID;
            var getOrderByConditionRequest = new PreOrderRequest();
            getOrderByConditionRequest.SearchCondition = vm.SearchCondition;
            getOrderByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getOrderByConditionRequest.PageIndex = 0;
            var response = new PreOrderService().GetPreOrder(getOrderByConditionRequest);
            vm.SearchConditionResponse = response.Result.SearchCondition;
            vm.PageIndex = response.Result.PageIndex;
            vm.PageCount = response.Result.PageCount;
            return View(vm);
        }
        /// <summary>
        /// 快递转换导入
        /// </summary>
        /// <param name="CustomerName"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [HttpPost]
        public string ImputEcecl(string CustomerName, long CustomerID)
        {
            PreOrderViewModel vm = new PreOrderViewModel();
            List<string> OrderKey = new List<string>();
            List<OrderNumbers> orders = new List<OrderNumbers>();//用于验证单号是否存在
            List<string> ExpressList = new List<string>();
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    #region 验证格式
                    if (ds == null)
                    {
                        return new { result = "<h3><font color='#FF0000'>Excel格式有误</font></h3>", IsSuccess = false }.ToJsonString();
                    }
                    if (!ds.Tables[0].Columns.Contains("系统单号") || !ds.Tables[0].Columns.Contains("快递公司"))
                    {
                        return new { result = "<h3><font color='#FF0000'>模版必须包含 ‘系统单号’ 和 ‘快递公司’这两列 </font></h3>", IsSuccess = false }.ToJsonString();
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            OrderNumbers orderNo = new OrderNumbers();
                            orderNo.OrderNumber = ds.Tables[0].Rows[i]["系统单号"].ToString();
                            orderNo.SerialNumber = "";
                            orders.Add(orderNo);
                        }

                        //OrderKey = (from a in ds.Tables[0].AsEnumerable() where a.Field<string>("系统单号") != "" && a.Field<string>("系统单号") != null select a.Field<string>("系统单号")).Distinct().ToList<string>();
                        ExpressList = (from a in ds.Tables[0].AsEnumerable() where a.Field<string>("快递公司") != "" && a.Field<string>("快递公司") != null select a.Field<string>("快递公司")).Distinct().ToList<string>();
                    }
                    else
                    {
                        return new { result = "<h3><font color='#FF0000'>模版中无数据</font></h3>", IsSuccess = false }.ToJsonString();
                    }
                    #endregion

                    var response = new ASNManagementService().OrderNumbersKeyCheck(orders);
                    if (response.IsSuccess)
                    {
                        #region 验证数据
                        foreach (var item in orders)
                        {
                            if (response.Result.OrderCollection.Select(m => m.OrderNumber == item.OrderNumber).Count() <= 0)
                            {
                                return new { result = "<h3><font color='#FF0000'>订单号:" + item.OrderNumber + " 在系统中不存在</font></h3>", IsSuccess = false }.ToJsonString();
                            }
                            //if (!response.Result.OrderCollection.Select(m => m.OrderNumber ==item.OrderNumber).FirstOrDefault())
                            //{
                            //    return new { result = "<h3><font color='#FF0000'>订单号:" + item.OrderNumber + " 在系统中不存在</font></h3>", IsSuccess = false }.ToJsonString();
                            //}
                        }
                        IEnumerable<WMSConfig> wmsexpress = ApplicationConfigHelper.GetWMS_Config("ExpressCompany");
                        foreach (var item in ExpressList)
                        {
                            if (wmsexpress.Select(m => m.Name == item).Count() <= 0)
                            {
                                return new { result = "<h3><font color='#FF0000'>快递公司:" + item + " 在系统中不存在</font></h3>", IsSuccess = false }.ToJsonString();
                            }
                        }
                        #endregion
                        List<OrderInfo> listOrder = new List<OrderInfo>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (ds.Tables[0].Rows[i]["系统单号"].ToString() != "")
                            {
                                OrderInfo oi = new OrderInfo();
                                oi.OrderNumber = ds.Tables[0].Rows[i]["系统单号"].ToString();
                                oi.ExpressCompany = ds.Tables[0].Rows[i]["快递公司"].ToString();
                                listOrder.Add(oi);
                            }
                        }
                        var respones = new OrderManagementService().ChangeExpressByOrderNumber(listOrder);
                        if (respones == "")
                        {
                            return new { result = "<h3><font color='#33cc70'>更新成功</font></h3>", IsSuccess = true }.ToJsonString();
                        }
                        else
                        {
                            return new { result = "<h3><font color='#FF0000'>更新失败，异常信息：" + respones + "</font></h3>", IsSuccess = false }.ToJsonString();
                        }
                    }
                }
            }
            return new { result = "<h3>失败</h3>", IsSuccess = false }.ToJsonString();
        }
        /// <summary>
        /// 导入更新订单快递单号
        /// </summary>
        /// <param name="CustomerName"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateOrderExpressInfo(string CustomerName, long CustomerID)
        {
            OrderECManagementService service = new OrderECManagementService();
            List<string> expressCompanys = new List<string>();//用于验证快递公司是否已配置
            List<OrderNumbers> orders = new List<OrderNumbers>();//用于验证外部单号是否存在
            List<OrderInfo> orderInfos = new List<OrderInfo>();//用于更新订单快递信息
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    #region 验证数据
                    if (ds == null || ds.Tables.Count == 0)
                    {
                        return new { result = "<h3><font color='#FF0000'>Excel格式有误</font></h3>", IsSuccess = false }.ToJsonString();
                    }
                    if (!ds.Tables[0].Columns.Contains("外部单号") || !ds.Tables[0].Columns.Contains("快递公司") || !ds.Tables[0].Columns.Contains("快递单号"))
                    {
                        return new { result = "<h3><font color='#FF0000'>模版必须包含 ‘外部单号’‘快递公司’‘快递单号’这三列 </font></h3>", IsSuccess = false }.ToJsonString();
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            OrderNumbers order = new OrderNumbers();
                            order.ExternOrderNumber = dr["外部单号"].ToString();
                            orders.Add(order);
                        }
                        expressCompanys = ds.Tables[0].AsEnumerable().Where(a => a.Field<string>("快递公司") != "" && a.Field<string>("快递公司") != null).Select(a => a.Field<string>("快递公司")).Distinct().ToList(); //(from a in ds.Tables[0].AsEnumerable() where a.Field<string>("快递公司") != "" && a.Field<string>("快递公司") != null select a.Field<string>("快递公司")).Distinct().ToList<string>();
                    }
                    else
                    {
                        return new { result = "<h3><font color='#FF0000'>模版中无数据</font></h3>", IsSuccess = false }.ToJsonString();
                    }
                    #region 验证快递公司是否配置
                    IEnumerable<WMSConfig> wmsexpress = ApplicationConfigHelper.GetWMS_Config("ExpressCompany");
                    foreach (var item in expressCompanys)
                    {
                        if (wmsexpress.Select(m => m.Name == item).Count() <= 0)
                        {
                            return new { result = "<h3><font color='#FF0000'>快递公司:" + item + " 在系统中不存在</font></h3>", IsSuccess = false }.ToJsonString();
                        }
                    }
                    #endregion


                    var response = service.ExternOrderNumberCheck(orders, CustomerID);
                    if (response.IsSuccess)
                    {
                        #region 验证外部单号是否存在于系统中
                        foreach (var item in orders)
                        {
                            if (response.Result.OrderCollection.Select(m => m.ExternOrderNumber == item.ExternOrderNumber).Count() <= 0)
                            {
                                return new { result = "<h3><font color='#FF0000'>外部单号:" + item.ExternOrderNumber + " 在系统中不存在</font></h3>", IsSuccess = false }.ToJsonString();
                            }
                        }
                        #endregion

                        #region 更新出库单快递信息
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (!string.IsNullOrEmpty(dr["外部单号"].ToString()))
                            {
                                OrderInfo oi = new OrderInfo();
                                oi.ExternOrderNumber = dr["外部单号"].ToString();
                                oi.ExpressCompany = dr["快递公司"].ToString();
                                oi.ExpressNumber = dr["快递单号"].ToString();
                                orderInfos.Add(oi);
                            }
                        }
                        var result = service.UpdateOrderExpressInfo(orderInfos, CustomerID);
                        if (result.Result == "")
                        {
                            return new { result = "<h3><font color='#33cc70'>更新成功</font></h3>", IsSuccess = true }.ToJsonString();
                        }
                        else
                        {
                            return new { result = "<h3><font color='#FF0000'>更新失败，异常信息：" + result.Result + "</font></h3>", IsSuccess = false }.ToJsonString();
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            return new { result = "<h3>导入失败</h3>", IsSuccess = false }.ToJsonString();
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
        public void TransData(long? CustomerID, long WareHouseID, ref DataSet transData, ref string message)
        {
            if (ApplicationConfigHelper.GetCacheInfo().Where(p => p.UserName == base.UserInfo.Name).ToList().Count() <= 0)
            {
                message = "用户" + base.UserInfo.DisplayName + "没有分配仓库!";
                return;
            }
            Object[] parameters = new Object[5];
            parameters[0] = "OrderManagement";
            parameters[1] = CustomerID;
            parameters[2] = base.UserInfo.ProjectID;
            parameters[3] = ApplicationConfigHelper.GetCacheInfo().First(p => p.UserName == base.UserInfo.Name).WarehouseID;//Bob
            parameters[4] = transData;

            string transDataInstanceName = transDataInstanceNameStr(CustomerID, WareHouseID);
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
        public JsonResult BoxChange(string code)
        {

            var boxList = from o in ApplicationConfigHelper.GetApplicationBox(base.UserInfo.ProjectName)
                          where o.Code == code
                          select new { o.Str1, o.Str2, o.Str3 };
            return Json(boxList, JsonRequestBehavior.AllowGet);
        }
        public string Handover(string ID)
        {
            IList<OrderBackStatus> Orders = new List<OrderBackStatus>();
            if (ID.Contains(","))
            {
                string[] strs = ID.Split(',');
                foreach (var str in strs)
                {
                    Orders.Add(new OrderBackStatus
                    {
                        ID = str.ObjectToInt64(),
                        UpdateTime = DateTime.Now,
                        Updator = base.UserInfo.Name
                    });
                }
            }
            else
            {
                Orders.Add(new OrderBackStatus
                {
                    ID = ID.ObjectToInt64(),
                    UpdateTime = DateTime.Now,
                    Updator = base.UserInfo.Name
                });
            }
            var GetHandoverResult = new OrderManagementService().Handover(Orders, base.UserInfo.Name);
            return GetHandoverResult.Result;
        }
        public string Outs(string ID)
        {
            IList<OrderBackStatus> Orders = new List<OrderBackStatus>();
            if (ID.Contains(","))
            {
                string[] strs = ID.Split(',');
                foreach (var str in strs)
                {
                    Orders.Add(new OrderBackStatus
                    {
                        ID = str.ObjectToInt64(),
                        UpdateTime = DateTime.Now,
                        Updator = base.UserInfo.Name
                    });
                }
            }
            else
            {
                Orders.Add(new OrderBackStatus
                {
                    ID = ID.ObjectToInt64(),
                    UpdateTime = DateTime.Now,
                    Updator = base.UserInfo.Name
                });
            }
            var GetOutResult = new OrderManagementService().Outs(Orders);
            return GetOutResult.Result;
        }
        public string ChangeExpress(string ID, string ExpressCompany)
        {
            var GetOutResult = new OrderManagementService().ChangeExpress(ID, ExpressCompany);
            return GetOutResult.Result;
        }
        public string UnionOrder(string ID)
        {
            IList<OrderBackStatus> Orders = new List<OrderBackStatus>();
            if (ID.Contains(","))
            {
                string[] strs = ID.Split(',');
                foreach (var str in strs)
                {
                    Orders.Add(new OrderBackStatus
                    {
                        ID = str.ObjectToInt64(),
                        UpdateTime = DateTime.Now,
                        Updator = base.UserInfo.Name
                    });
                }
            }
            else
            {
                return "请至少选择两单进行合并";

            }
            var GetOutResult = new OrderManagementService().UnionOrder(Orders);
            return GetOutResult.Result;
        }
        public static List<T> jsonlist<T>(string str)
        {
            return JsonConvert.DeserializeObject<List<T>>(str);
        }
        public ActionResult GetSKUlist(string sku, long ID)
        {
            var Product = new OrderManagementService().GetSkuListByCondition(ID).Result.OrderDetailCollection;
            //var ss=(IEnumerable<OrderDetailInfo>)Product;
            IEnumerable<WMSConfig> wms = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Product_Get_Type");
            return Json(Product.Where(s => s.SKU.IndexOf(sku, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.GoodsName, Text = t.SKU, GoodsType = t.GoodsType, Qty = t.Qty }), JsonRequestBehavior.AllowGet);
        }
        private void Export(GetOrderByConditionResponse response, IEnumerable<Column> columnOrder, IEnumerable<Column> columnOrderDetail)
        {
            IEnumerable<OrderInfo> orders = response.OrderCollection;
            IEnumerable<OrderDetailInfo> orderDetails = response.OrderDetailCollection;
            OrderViewModel orderViews = new OrderViewModel();
            DataSet ds = new DataSet();
            DataTable dtOrder = new DataTable();
            DataTable dtOrderDetail = new DataTable();
            foreach (var order in columnOrder)
            {
                dtOrder.Columns.Add(order.DisplayName, typeof(string));
            }
            foreach (var orderDetail in columnOrderDetail)
            {
                dtOrderDetail.Columns.Add(orderDetail.DisplayName, typeof(string));
            }
            orders.Each((i, s) =>
            {
                DataRow drOrder = dtOrder.NewRow();
                foreach (var order in columnOrder)
                {
                    if (order.DisplayName == "出库单状态")
                    {
                        drOrder[order.DisplayName] = orderViews.OrderStatus.Where(m => m.Value == typeof(Runbow.TWS.Entity.OrderInfo).GetProperty(order.DbColumnName).GetValue(s).ToString()).Select(c => c.Text).FirstOrDefault();
                    }
                    else
                    {
                        drOrder[order.DisplayName] = typeof(Runbow.TWS.Entity.OrderInfo).GetProperty(order.DbColumnName).GetValue(s);
                    }
                }
                dtOrder.Rows.Add(drOrder);
            });
            orderDetails.Each((i, s) =>
            {
                DataRow drOrder = dtOrderDetail.NewRow();
                foreach (var orderDetail in columnOrderDetail)
                {
                    drOrder[orderDetail.DisplayName] = typeof(Runbow.TWS.Entity.OrderDetailInfo).GetProperty(orderDetail.DbColumnName).GetValue(s);
                }
                dtOrderDetail.Rows.Add(drOrder);
            });
            dtOrder.TableName = "出库单主信息";
            dtOrderDetail.TableName = "出库单明细信息";
            ds.Tables.Add(dtOrder);
            ds.Tables.Add(dtOrderDetail);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "出库单" + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportDataSetByEPPlus(ds, "出库单" + DateTime.Now.ToString("yyyy-MM-dd"));
        }
        [HttpPost]
        public ActionResult PreOrderQuery(PreOrderViewModel vm, int? PageIndex)
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
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            var getOrderByConditionRequest = new PreOrderRequest();
            getOrderByConditionRequest.SearchCondition = vm.SearchCondition;
            getOrderByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getOrderByConditionRequest.PageIndex = PageIndex ?? 0;
            var response = new PreOrderService().GetPreOrder(getOrderByConditionRequest);
            vm.SearchConditionResponse = response.Result.SearchCondition;
            vm.PageIndex = response.Result.PageIndex;
            vm.PageCount = response.Result.PageCount;
            return View(vm);
        }
        [HttpGet]
        public ActionResult PrintOrder(string id, int Flag)
        {

            OrderViewModel vm = new OrderViewModel();
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
            ViewBag.Id = id;

            var response = new OrderManagementService().GetPrintOrder(id, Flag);

            vm.OrderCollection = response.Result.OrderCollection;
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));

            //vm.OrderCollection.Each((a, b) =>
            //{
            //    string strGUID = "Order" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.OrderNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});
            return View(vm);

        }
        [HttpGet]
        public ActionResult PrintOrderNike(string id, int Flag)
        {

            OrderViewModel vm = new OrderViewModel();
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
            ViewBag.Id = id;

            var response = new OrderManagementService().GetPrintOrderNike(id, Flag);

            vm.OrderCollection = response.Result.OrderCollection;
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));

            //vm.OrderCollection.Each((a, b) =>
            //{
            //    string strGUID = "Order" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.OrderNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});
            return View(vm);

        }
        [HttpGet]
        public ActionResult PrintOrderAkzo(string id, int Flag)
        {

            OrderViewModel vm = new OrderViewModel();
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
            ViewBag.Id = id;

            var response = new OrderManagementService().GetPrintOrderAkzo(id, Flag);

            vm.OrderCollection = response.Result.OrderCollection;
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));

            //vm.OrderCollection.Each((a, b) =>
            //{
            //    string strGUID = "Order" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.OrderNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});
            return View(vm);

        }
        [HttpGet]
        public ActionResult PrintOrder_EWE(string id, int Flag)
        {
            long UPCSum = 0;
            OrderViewModel vm = new OrderViewModel();
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
            ViewBag.Id = id;

            var response = new OrderManagementService().GetPrintOrder(id, Flag);

            vm.OrderCollection = response.Result.OrderCollection;
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;
            foreach (var s in vm.OrderDetailCollection)
            {
                UPCSum += s.Qty.ObjectToInt64();
            }
            vm.UPCSum = UPCSum;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));

            //vm.OrderCollection.Each((a, b) =>
            //{
            //    string strGUID = "Order" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.OrderNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});
            return View(vm);

        }
        /// <summary>
        /// YXDR拣货单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PrintOrderYXDR(string id, int Flag)
        {
            OrderViewModel vm = new OrderViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName);
            }
            catch
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderType");
            }
            List<SelectListItem> slt = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                slt.Add(new SelectListItem { Value = w.Name, Text = w.Name });
            }
            vm.OrderType = slt;
            ViewBag.ID = id;

            var response = new OrderManagementService().GetPrintOrderYXDR(id, Flag);
            #region 打印需要Article和size，在这里获取
            //List<ProductSearch> productListS = new List<ProductSearch>();
            //for (int i = 0; i < response.Result.OrderDetailCollection.Count(); i++)
            //{
            //    ProductSearch ps = new ProductSearch();
            //    ps.SKU = response.Result.OrderDetailCollection.ToArray()[i].SKU;
            //    productListS.Add(ps);
            //}
            //IEnumerable<ProductSearch> products = ApplicationConfigHelper.GetSearchProductYXDR(base.UserInfo.CustomerID, productListS, "SKU");
            //response.Result.OrderDetailCollection.Each((a, b) =>
            //{
            //    b.Article = products.Where(m => m.SKU == b.SKU).ToArray().Length > 0 ? products.Where(m => m.SKU == b.SKU).ToArray()[0].Str10 : "";
            //    b.Size = products.Where(m => m.SKU == b.SKU).ToArray().Length > 0 ? products.Where(m => m.SKU == b.SKU).ToArray()[0].Str9 : "";
            //    b.GoodsName = products.Where(m => m.SKU == b.SKU).ToArray().Length > 0 ? products.Where(m => m.SKU == b.SKU).ToArray()[0].GoodsName : "";
            //});
            #endregion

            vm.OrderCollection = response.Result.OrderCollection;
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;
            var IDlist = "";
            foreach (var item in vm.OrderCollection)
            {
                IDlist += item.ID.ToString() + ',';
            }
            ViewBag.IDs = IDlist.Substring(0, IDlist.Length - 1);
            return View(vm);
        }
        public ActionResult PrintAllPod(string ids)
        {
            PrintPodModel print = new PrintPodModel();
            var response = new OrderManagementService().GetPrintAllPodCondition(ids);

            print.EnumerableCustomerInfo = response.EnumerableBoxListinfo;
            print.PrintPodInfos = response.PrintPodDetails;
            return View(print);
        }
        public ActionResult PrintOrder_Wave(string ID)
        {
            OrderViewModel vm = new OrderViewModel();
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
            var response = new OrderManagementService().GetPrintOrder_Wave(ID);
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;
            vm.OrderCollection = response.Result.OrderCollection;
            vm.UPCSum = response.Result.UPCSum;
            vm.OrderSum = response.Result.OrderSum;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));
            //vm.OrderCollection.Each((a, b) =>
            //{
            //    string strGUID = "Order" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.OrderNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});
            return View(vm);

        }
        [HttpGet]
        public ActionResult PrintPreOrder(string id, int Flag)
        {

            PreOrderViewModel vm = new PreOrderViewModel();
            ViewBag.Id = id;

            var response = new OrderManagementService().GetPrintOrder(id, Flag);

            vm.PreO = response.Result.PreOrderCollection;
            vm.PreOd = response.Result.PreOrderDetailCollection;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));

            //vm.PreO.Each((a, b) =>
            //{
            //    string strGUID = "PreOrder" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.PreOrderNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});
            return View(vm);

        }
        [HttpGet]
        public ActionResult PrintOrderYFBLD(string id, int Flag)
        {

            OrderViewModel vm = new OrderViewModel();
            ViewBag.Id = id;

            var response = new OrderManagementService().GetPrintOrderYFBLD(id, Flag);

            vm.OrderCollection = response.Result.OrderCollection;
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));

            //vm.PreO.Each((a, b) =>
            //{
            //    string strGUID = "PreOrder" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.PreOrderNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});
            return View(vm);

        }
        [HttpGet]
        public ActionResult PrintPreOrderYFBLD(string id, int Flag)
        {

            PreOrderViewModel vm = new PreOrderViewModel();
            ViewBag.Id = id;

            var response = new OrderManagementService().GetPrintOrderYFBLD(id, Flag);

            vm.PreO = response.Result.PreOrderCollection;
            vm.PreOd = response.Result.PreOrderDetailCollection;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));

            //vm.PreO.Each((a, b) =>
            //{
            //    string strGUID = "PreOrder" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.PreOrderNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});
            return View(vm);

        }
        //[HttpPost]
        //public JsonResult PrintOrder(string id)
        //{
        //    OrderViewModel vm = new OrderViewModel();
        //    var response = new OrderManagementService().GetPrintOrder(id);

        //    vm.OrderCollection = response.Result.OrderCollection;
        //    vm.OrderDetailCollection = response.Result.OrderDetailCollection;

        //    JavaScriptSerializer js = new JavaScriptSerializer();
        //    string data = js.Serialize(response);
        //    string data1 = js.Serialize(response.Result.OrderCollection);
        //    string data2 = js.Serialize(response.Result.OrderDetailCollection);

        //    return Json(new { data =data, data1 = data1, data2 = data2 });

        //}
        /// <summary>
        /// listTo sql in 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string Sqlwherein(List<string> list)
        {
            string WhereIn = string.Empty;
            if (list.Count > 0)
            {
                list.ForEach(a =>
                {
                    WhereIn = WhereIn + "'" + a.ToString() + "',";
                });
            }
            else { WhereIn = ","; }
            return WhereIn.TrimEnd(',');
        }
        public string GetMaxBoxnumber(string OrderID)
        {
            string boxnumber = "";
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("GetBoxnumber_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("GetBoxnumber");
            }
            boxnumber = new OrderManagementService().GetMaxBoxnumber(OrderID, wms.FirstOrDefault().Name);
            return boxnumber;
        }
        public string DeletePackInfo(string PackageKey)
        {
            string boxnumber = new OrderManagementService().DeletePackInfo(PackageKey);
            if (boxnumber != "-1")
            {
                new BarCodeService().DeleteBarCode(PackageKey);
            }
            return boxnumber;
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
                    if (va.Contains("Order"))
                    {
                        SysIO.File.Delete(va);
                    }
                }
            }
        }
        public ActionResult ImportReturnNo(long? customerID)
        {
            OrderViewModel vm = new OrderViewModel();
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
            if (base.UserInfo.UserType == 2)
            {
                vm.Customers = vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
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
            vm.SearchCondition = new OrderSearchCondition();
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
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
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

            ViewBag.WarehouseList = WarehouseList;
            if (CustomerList.Count() == 1)
            {
                vm.SearchCondition.CustomerID = CustomerList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();
            }
            return View(vm);
        }
        [HttpPost]
        public string ImportReturnNoExcel(string CustomerName, string CustomerID)
        {
            StringBuilder message = new StringBuilder("");
            int customer_id = 0;
            Int32.TryParse(CustomerID, out customer_id);
            List<string> OrderKey = new List<string>();
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
                    if (ds == null)
                    {
                        return new { result = "<h3><font color='#FF0000'>Excel格式有误</font></h3>", IsSuccess = false }.ToJsonString();
                    }
                    List<OrderNumbers> list = new List<OrderNumbers>();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //OrderKey = (from a in ds.Tables[0].AsEnumerable() where a.Field<string>("出库单号") != "" && a.Field<string>("出库单号") != null select a.Field<string>("出库单号")).Distinct().ToList<string>();
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            OrderNumbers obj = new OrderNumbers();
                            obj.OrderNumber = item[0].ToString();
                            obj.SerialNumber = item[1].ToString();
                            list.Add(obj);
                        }
                    }
                    else
                    {
                        return new { result = "<h3><font color='#FF0000'>模版中无数据</font></h3>", IsSuccess = false }.ToJsonString();
                    }
                    //foreach (var item in OrderKey)
                    //{
                    //    OrderNumbers orderNumber = new OrderNumbers();
                    //    orderNumber.OrderNumber=item;
                    //    orderNumber.SerialNumber = "";
                    //    list.Add(orderNumber);
                    //}
                    var response = new OrderManagementService().OrderKeyCheck(list.AsQueryable(), customer_id);

                    if (response.IsSuccess)
                    {
                        List<OrderNumbers> listUpdate = new List<OrderNumbers>();
                        //循环判断订单号存不存在
                        foreach (var item in list)
                        {
                            if (response.Result.OrderCollection.Select(c => c.OrderNumber == item.OrderNumber).Count() <= 0)
                            {
                                message.Append("<h3><font color='#FF0000'>订单号" + item + "不存在</font></h3><br />");
                            }
                            else
                            {
                                //加入到更新集合中
                                listUpdate.Add(item);
                            }
                        }
                        //做更新操作
                        if (listUpdate != null && listUpdate.Count > 0)
                        {
                            var response2 = new OrderManagementService().UpdateSerialNumberByOrderNumber(list.AsQueryable(), customer_id);
                            if (response2.IsSuccess)
                            {
                                message.Append("<h3>导入成功！</h3><br />");
                                return new { result = message.ToString(), IsSuccess = true }.ToJsonString();
                            }
                        }
                    }
                }
            }
            return new { result = "<h3>失败</h3>", IsSuccess = false }.ToJsonString();
        }
        public ActionResult BatchPrintOrderYFBLD(string id)
        {
            OrderViewModel vm = new OrderViewModel();
            ViewBag.Id = id;

            var response = new OrderManagementService().GetBatchPrintOrderYFBLD(id);

            vm.OrderCollection = response.Result.OrderCollection;
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;
            return View(vm);
        }
        public ActionResult Wave()
        {
            WaveModel vm = new WaveModel();
            #region 客户

            #endregion
            vm.SearchCondition = new WaveSearchCondition();
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
            //                         .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;

            ViewBag.CustomerList = CustomerList;
            ViewBag.UserName = base.UserInfo.Name.ToUpper();
            vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-6);
            vm.SearchCondition.EndCreateTime = DateTime.Now;

            vm.SearchCondition.StartCompleteTime = DateTime.Now.AddDays(-6);
            vm.SearchCondition.EndCompleteTime = DateTime.Now;

            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                if (customerIDs != null && customerIDs.Count() == 1)
                {
                    vm.SearchCondition.CustomerID = customerIDs.First();
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
            if (CustomerList.Count() == 1)
            {
                vm.SearchCondition.CustomerID = CustomerList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();
            }
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();
                //ViewBag.WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == vm.SearchCondition.Warehouse).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = "INSTRUCTION_" + c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });
            }

            var getWaveByConditionRequest = new GetWaveByConditionRequest();
            getWaveByConditionRequest.SearchCondition = vm.SearchCondition;
            getWaveByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getWaveByConditionRequest.PageIndex = 0;
            var getOrderByConditionResponse = new WaveService().GetWaveHeaderByCondition(getWaveByConditionRequest);

            if (getOrderByConditionResponse.IsSuccess)
            {
                vm.WaveCollection = getOrderByConditionResponse.Result.WaveCollection;
                vm.PageIndex = getOrderByConditionResponse.Result.PageIndex;
                vm.PageCount = getOrderByConditionResponse.Result.PageCount;
            }

            return View(vm);
        }
        [HttpPost]
        public ActionResult Wave(WaveModel vm, int? PageIndex, string Action)
        {
            //vm.SearchCondition = new WaveSearchCondition();
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
            //                         .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            ViewBag.CustomerList = CustomerList;

            ViewBag.CustomerList = CustomerList;
            ViewBag.UserName = base.UserInfo.Name.ToUpper();
            //vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-6);
            //vm.SearchCondition.EndCreateTime = DateTime.Now;

            //vm.SearchCondition.StartCompleteTime = DateTime.Now.AddDays(-6);
            //vm.SearchCondition.EndCompleteTime = DateTime.Now;

            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
                if (customerIDs != null && customerIDs.Count() == 1)
                {
                    vm.SearchCondition.CustomerID = customerIDs.First();
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
            if (CustomerList.Count() == 1)
            {
                vm.SearchCondition.CustomerID = CustomerList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();
            }
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();
                //ViewBag.WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == vm.SearchCondition.Warehouse).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = "INSTRUCTION_" + c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });
            }

            var getWaveByConditionRequest = new GetWaveByConditionRequest();
            getWaveByConditionRequest.SearchCondition = vm.SearchCondition;
            getWaveByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getWaveByConditionRequest.PageIndex = PageIndex ?? 0;
            var getOrderByConditionResponse = new WaveService().GetWaveHeaderByCondition(getWaveByConditionRequest);

            if (getOrderByConditionResponse.IsSuccess)
            {
                vm.WaveCollection = getOrderByConditionResponse.Result.WaveCollection;
                vm.PageIndex = getOrderByConditionResponse.Result.PageIndex;
                vm.PageCount = getOrderByConditionResponse.Result.PageCount;
            }
            return View(vm);
        }
        public string CreateWave(string IsSinglePriece, string IsExpressCompany, string CustomerID, string CustomerName, string WarehouseID, string WarehouseName, string WaveCount)
        {
            WaveService service = new WaveService();
            string creator = "";
            if (Session["Name"] != null)
                creator = Session["Name"].ToString();
            string message = service.CreateWave(IsSinglePriece, IsExpressCompany, CustomerID, CustomerName, WarehouseID, WarehouseName, WaveCount, creator);
            return message;
        }
        public ActionResult WaveDetailView(int ID)
        {
            WaveModel wm = new WaveModel();
            var response = new WaveService().GetWaveHeaderAndDetail(ID);
            wm.WaveCollection = response.Result.WaveCollection;
            wm.WaveDetailCollection = response.Result.WaveDetailCollection;
            return View(wm);
        }
        public ActionResult PrintHeader(long? customerID)
        {
            #region 注释
            //PrintHeaderModel vm = new PrintHeaderModel();

            //vm.SearchCondition = new PrintHeaderSearchCondition();
            //vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            //var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            //var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            //var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ////var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
            ////                         .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            //ViewBag.CustomerList = CustomerList;

            //ViewBag.CustomerList = CustomerList;
            //ViewBag.UserName = base.UserInfo.Name.ToUpper();
            //vm.SearchCondition.StartUpdateTime = DateTime.Now.AddDays(-6);
            //vm.SearchCondition.EndUpdateTime = DateTime.Now;

            //if (base.UserInfo.UserType == 0)
            //{
            //    vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            //}
            //else if (base.UserInfo.UserType == 2)
            //{
            //    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
            //    if (customerIDs != null && customerIDs.Count() == 1)
            //    {
            //        vm.SearchCondition.CustomerID = customerIDs.First();
            //    }
            //}
            //IEnumerable<SelectListItem> WarehouseList = null;
            //if (vm.SearchCondition.CustomerID == null)
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                                        .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //    StringBuilder sb = new StringBuilder();

            //    foreach (var i in CustomerListID)
            //    {
            //        sb.Append("" + i + ",");
            //    }
            //    if (sb.Length > 1)
            //    {
            //        vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
            //    }
            //    else
            //    {
            //        vm.SearchCondition.CustomerIDs = "0";
            //    }
            //}
            //else
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                         .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //}

            //ViewBag.WarehouseList = WarehouseList;
            //if (CustomerList.Count() == 1)
            //{
            //    vm.SearchCondition.CustomerID = CustomerList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();
            //}
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();
            //    //ViewBag.WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == vm.SearchCondition.Warehouse).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = "INSTRUCTION_" + c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });
            //}
            #endregion

            //新逻辑
            PrintHeaderModel vm = new PrintHeaderModel();
            vm.SearchCondition = new PrintHeaderSearchCondition();
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;

            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.Customers = CustomerList;
            vm.SearchCondition.StartUpdateTime = DateTime.Now.AddDays(-6);
            vm.SearchCondition.EndUpdateTime = DateTime.Now;
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
                    vm.SearchCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            ViewBag.WarehouseList = WarehouseList;

            var request = new GetPrintByConditionRequest();
            request.SearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = 0;
            var response = new PrintHeaderService().GetPrintHeaderByCondition(request);



            if (response.IsSuccess)
            {
                vm.PrintHeaderCollection = response.Result.PrintHeaderCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }
        [HttpPost]
        public ActionResult PrintHeader(PrintHeaderModel vm)
        {
            #region 注释
            //vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            //var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            //var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            //var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ////var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
            ////                         .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            //ViewBag.CustomerList = CustomerList;

            //ViewBag.CustomerList = CustomerList;
            //ViewBag.UserName = base.UserInfo.Name.ToUpper();
            //vm.SearchCondition.StartUpdateTime = DateTime.Now.AddDays(-6);
            //vm.SearchCondition.EndUpdateTime = DateTime.Now;

            //if (base.UserInfo.UserType == 0)
            //{
            //    vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            //}
            //else if (base.UserInfo.UserType == 2)
            //{
            //    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
            //    if (customerIDs != null && customerIDs.Count() == 1)
            //    {
            //        vm.SearchCondition.CustomerID = customerIDs.First();
            //    }
            //}
            //IEnumerable<SelectListItem> WarehouseList = null;
            //if (vm.SearchCondition.CustomerID == null)
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                                        .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //    StringBuilder sb = new StringBuilder();

            //    foreach (var i in CustomerListID)
            //    {
            //        sb.Append("" + i + ",");
            //    }
            //    if (sb.Length > 1)
            //    {
            //        vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
            //    }
            //    else
            //    {
            //        vm.SearchCondition.CustomerIDs = "0";
            //    }
            //}
            //else
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                         .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //}

            //ViewBag.WarehouseList = WarehouseList;
            //if (CustomerList.Count() == 1)
            //{
            //    vm.SearchCondition.CustomerID = CustomerList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();
            //}
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();
            //    //ViewBag.WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == vm.SearchCondition.Warehouse).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = "INSTRUCTION_" + c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });
            //}
            #endregion



            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            ViewBag.UserName = base.UserInfo.Name.ToUpper();
            vm.SearchCondition.StartUpdateTime = DateTime.Now.AddDays(-6);
            vm.SearchCondition.EndUpdateTime = DateTime.Now;

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
                vm.SearchCondition.CustomerID = 0;

            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            ViewBag.CustomerList = CustomerList;
            ViewBag.WarehouseList = WarehouseList;
            if (vm.SearchCondition.WarehouseName != null)
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
                    vm.SearchCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }


            var request = new GetPrintByConditionRequest();
            request.SearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = 0;
            var response = new PrintHeaderService().GetPrintHeaderByCondition(request);

            if (response.IsSuccess)
            {
                vm.PrintHeaderCollection = response.Result.PrintHeaderCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }
        public ActionResult PrintDetail(long? ID, int? Isopen)
        {
            if (Isopen != null)
            {
                ViewBag.Isopen = Isopen;
            }
            else
            {
                ViewBag.Isopen = 0;
            }
            PrintHeaderModel vm = new PrintHeaderModel();
            if (ID != null)
            {
                var response = new PrintHeaderService().GetPrintHeaderAndDetailByID(Convert.ToInt32(ID));
                vm.PrintHeaderCollection = response.Result.PrintHeaderCollection;
                vm.PrintDetailCollection = response.Result.PrintDetailCollection;
            }
            return View(vm);
        }
        public ActionResult OrderSelect(long? customerID, long PrintID)
        {
            #region 注释
            //OrderViewModel vm = new OrderViewModel();
            //IEnumerable<WMSConfig> wms = null;
            //try
            //{
            //    wms = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName);
            //}
            //catch (Exception)
            //{
            //}

            //if (wms == null)
            //{
            //    wms = ApplicationConfigHelper.GetWMS_Config("OrderType");
            //}
            //List<SelectListItem> st = new List<SelectListItem>();
            //foreach (WMSConfig w in wms)
            //{
            //    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            //}
            //if (base.UserInfo.UserType == 2)
            //{
            //    vm.Customers = vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
            //                     .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //}
            //else if (base.UserInfo.UserType == 0)
            //{
            //    vm.Customers = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            //}
            //else
            //{
            //    vm.Customers = Enumerable.Empty<SelectListItem>();
            //}
            //vm.OrderType = st;
            //vm.SearchCondition = new OrderSearchCondition();
            //vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            //var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            //var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            //var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ////var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
            ////                         .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            //ViewBag.CustomerList = CustomerList;

            //var strlist = vm.Str19.Select(c => new SelectListItem() { Value = c.Value.ToString(), Text = c.Text });
            ////var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
            ////                         .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            //ViewBag.Str19List = strlist;
            //ViewBag.ProjectName = base.UserInfo.ProjectName;
            //vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-10);
            //vm.SearchCondition.EndCreateTime = DateTime.Now;

            //if (base.UserInfo.UserType == 0)
            //{
            //    vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            //}
            //else if (base.UserInfo.UserType == 2)
            //{
            //    if (customerID.HasValue)
            //    {
            //        vm.SearchCondition.CustomerID = customerID;
            //    }
            //    else
            //    {
            //        var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
            //        if (customerIDs != null && customerIDs.Count() == 1)
            //        {
            //            vm.SearchCondition.CustomerID = customerIDs.First();
            //        }
            //    }
            //}
            //IEnumerable<SelectListItem> WarehouseList = null;
            //if (vm.SearchCondition.CustomerID == null)
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                                        .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            //    StringBuilder sb = new StringBuilder();

            //    foreach (var i in CustomerListID)
            //    {
            //        sb.Append("" + i + ",");
            //    }
            //    if (sb.Length > 1)
            //    {
            //        vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
            //    }
            //    else
            //    {
            //        vm.SearchCondition.CustomerIDs = "0";
            //    }
            //}
            //else
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                         .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            //}

            //ViewBag.WarehouseList = WarehouseList;
            //if (CustomerList.Count() == 1)
            //{
            //    vm.SearchCondition.CustomerID = CustomerList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();
            //}
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.SearchCondition.Warehouse = WarehouseList.Select(c => c.Text).FirstOrDefault();

            //}
            #endregion

            OrderViewModel vm = new OrderViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }
            vm.PrintID = PrintID;
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
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.Customers = CustomerList;
            vm.SearchCondition = new OrderSearchCondition();
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;

            var strlist = vm.Str19.Select(c => new SelectListItem() { Value = c.Value.ToString(), Text = c.Text });
            ViewBag.Str19List = strlist;
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            //vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-10);
            //vm.SearchCondition.EndCreateTime = DateTime.Now;
            vm.SearchCondition.StartCreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd HH:mm:ss"));
            vm.SearchCondition.EndCreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ObjectToNullableDateTime();
            vm.SearchCondition.OrderType = vm.OrderType.FirstOrDefault().Value.ToString();
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


            var WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == vm.SearchCondition.Warehouse).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });

            ViewBag.WorkStation = WorkStation;
            var getOrderByConditionRequest = new GetOrderByConditionRequest();
            getOrderByConditionRequest.SearchCondition = vm.SearchCondition;
            getOrderByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getOrderByConditionRequest.PageIndex = 0;
            getOrderByConditionRequest.SearchCondition.Status = 1;
            var getOrderByConditionResponse = new OrderManagementService().GetOrderHeaderByCondition(getOrderByConditionRequest);

            if (getOrderByConditionResponse.IsSuccess)
            {
                vm.OrderCollection = getOrderByConditionResponse.Result.OrderCollection;
                vm.PageIndex = getOrderByConditionResponse.Result.PageIndex;
                vm.PageCount = getOrderByConditionResponse.Result.PageCount;
            }
            GenQueryOrderAndOrderDetailViewModel(vm);
            return View(vm);
        }
        [HttpPost]
        public ActionResult OrderSelect(OrderViewModel vm, int? PageIndex, string Action)
        {
            #region 注释
            //IEnumerable<WMSConfig> wms = null;
            //try
            //{
            //    wms = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName);
            //}
            //catch (Exception)
            //{
            //}

            //if (wms == null)
            //{
            //    wms = ApplicationConfigHelper.GetWMS_Config("OrderType");
            //}
            //List<SelectListItem> st = new List<SelectListItem>();
            //foreach (WMSConfig w in wms)
            //{
            //    st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            //}
            //vm.OrderType = st;
            //var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            //var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            //var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            //ViewBag.CustomerList = CustomerList;
            //var strlist = vm.Str19.Select(c => new SelectListItem() { Value = c.Value.ToString(), Text = c.Text });
            ////var CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID)
            ////                         .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            //ViewBag.Str19List = strlist;
            //ViewBag.ProjectName = base.UserInfo.ProjectName;
            //IEnumerable<SelectListItem> WarehouseList = null;
            //if (vm.SearchCondition.CustomerID == null)
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                                        .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            //    StringBuilder sb = new StringBuilder();

            //    foreach (var i in CustomerListID)
            //    {
            //        sb.Append("" + i + ",");
            //    }
            //    if (sb.Length > 1)
            //    {
            //        vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
            //    }
            //    else
            //    {
            //        vm.SearchCondition.CustomerIDs = "0";
            //    }
            //}
            //else
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                         .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            //}
            #endregion

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
            var strlist = vm.Str19.Select(c => new SelectListItem() { Value = c.Value.ToString(), Text = c.Text });
            ViewBag.Str19List = strlist;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
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
                vm.SearchCondition.CustomerID = 0;

            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            ViewBag.CustomerList = CustomerList;
            ViewBag.WarehouseList = WarehouseList;
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
            ViewBag.ProjectName = base.UserInfo.ProjectName;

            var WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == vm.SearchCondition.Warehouse).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });

            ViewBag.WorkStation = WorkStation;
            ViewBag.WarehouseList = WarehouseList;
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getOrderByConditionRequest = new GetOrderByConditionRequest();
            getOrderByConditionRequest.SearchCondition = vm.SearchCondition;
            getOrderByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getOrderByConditionRequest.PageIndex = PageIndex ?? 0;
            getOrderByConditionRequest.SearchCondition.Status = 1;
            var getOrderByConditionResponse = new OrderManagementService().GetOrderHeaderByCondition(getOrderByConditionRequest);
            //var WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == vm.SearchCondition.Warehouse).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });

            ViewBag.WorkStation = WorkStation;
            if (getOrderByConditionResponse.IsSuccess)
            {
                vm.OrderCollection = getOrderByConditionResponse.Result.OrderCollection;
                vm.PageIndex = getOrderByConditionResponse.Result.PageIndex;
                vm.PageCount = getOrderByConditionResponse.Result.PageCount;
                if (Action == "导出")
                {
                    //一次导出1000条数据
                    getOrderByConditionRequest.PageSize = 1000;
                    getOrderByConditionResponse = new OrderManagementService().GetOrderImportByCondition(getOrderByConditionRequest);
                    IEnumerable<Column> columnOrder;
                    IEnumerable<Column> columnOrderDetail;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Order").Count() == 0)
                    {
                        columnOrder = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Order").ColumnCollection;
                    }
                    else
                    {
                        columnOrder = module.Tables.TableCollection.First(t => t.Name == "WMS_Order").ColumnCollection;
                    }
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_OrderDetail").Count() == 0)
                    {
                        columnOrderDetail = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_OrderDetail").ColumnCollection;
                    }
                    else
                    {
                        columnOrderDetail = module.Tables.TableCollection.First(t => t.Name == "WMS_OrderDetail").ColumnCollection;
                    }



                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnOrder = tables.First(t => t.Name == "WMS_Order").ColumnCollection;
                    //IEnumerable<Column> columnOrderDetail = tables.First(t => t.Name == "WMS_OrderDetail").ColumnCollection;
                    if (vm.SearchCondition.CustomerID == 0)
                    {
                        columnOrder = columnOrder.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                        columnOrderDetail = columnOrderDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                    }
                    else
                    {
                        var notKeyColumns1 = columnOrder.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.SearchCondition.CustomerID));
                        //             .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID)))
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
                        var notKeyColumns2 = columnOrderDetail.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.SearchCondition.CustomerID));
                        //             .Where(c => (c.IsKey == false && c.IsHide == false) || (c.IsKey == false && c.IsHide == true && c.InnerColumns.Any(innerc => innerc.CustomerID == vm.SearchCondition.CustomerID)))
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
                        columnOrder = columnOrder.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true)).Union(notKeyColumns1.Where(c => c.IsShowInList));
                        columnOrderDetail = columnOrderDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true)).Union(notKeyColumns2.Where(c => c.IsShowInList));
                    }
                    Export(getOrderByConditionResponse.Result, columnOrder, columnOrderDetail);

                }
                if (Action == "合并")
                {
                    IList<OrderBackStatus> Orders = new List<OrderBackStatus>();
                    foreach (var str in getOrderByConditionResponse.Result.OrderCollection)
                    {
                        Orders.Add(new OrderBackStatus
                        {
                            ID = str.ID.ObjectToInt64(),
                            UpdateTime = DateTime.Now,
                            Updator = base.UserInfo.Name
                        });
                    }
                    var GetOutResult = new OrderManagementService().UnionOrder(Orders);
                    getOrderByConditionRequest.SearchCondition.Status = 1;
                    var getOrderByConditionResponsed = new OrderManagementService().GetOrderByCondition(getOrderByConditionRequest);
                    vm.OrderCollection = getOrderByConditionResponsed.Result.OrderCollection;
                    vm.PageIndex = getOrderByConditionResponsed.Result.PageIndex;
                    vm.PageCount = getOrderByConditionResponsed.Result.PageCount;
                }
            }
            GenQueryOrderAndOrderDetailViewModel(vm);
            if (base.UserInfo.UserType == 2)
            {
                vm.Customers = vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
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
            return View(vm);
        }
        public JsonResult CreateOrUpdatePrintHeaderAndDetail(int CustomerID, string CustomerName, string WarehouseID, string WarehouseName, string ids, int PrintID, string PrintKey)
        {
            List<PreOrderIds> list = new List<PreOrderIds>();
            foreach (var item in ids.Split(','))
            {
                PreOrderIds id = new PreOrderIds();
                id.ID = Convert.ToInt32(item);
                list.Add(id);
            }
            var response = new PrintHeaderService().CreateOrUpdatePrintHeaderAndDetail(CustomerID, CustomerName, 0, WarehouseName, base.UserInfo.Name, list, PrintID, PrintKey);
            return Json(response);
        }
        /// <summary>
        /// 关联快递单号
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="OrderKey"></param>
        /// <param name="ExpressKey"></param>
        /// <returns></returns>
        public JsonResult RelateExpressKey(int ID, string OrderKey, string ExpressKey)
        {
            //var response = new PrintHeaderService().RelateExpressKey(ID, OrderKey, ExpressKey, base.UserInfo.Name);
            string result = new PrintHeaderService().RelateExpressKey(ID, OrderKey, ExpressKey, base.UserInfo.Name);
            if (result == "")
            {
                return Json(new { IsSuccess = true, msg = "" });
            }
            else
            {
                return Json(new { IsSuccess = false, msg = result });
            }

        }
        //public ActionResult PrintHeaderAndDetail(string ids)
        //{
        //    return View();
        //}
        /// <summary>
        /// YXDR拣货单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PrintHeaderAndDetailYXDR(string id, int Flag)
        {
            OrderViewModel vm = new OrderViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName);
            }
            catch
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderType");
            }
            List<SelectListItem> slt = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                slt.Add(new SelectListItem { Value = w.Name, Text = w.Name });
            }
            vm.OrderType = slt;
            ViewBag.ID = id;

            var response = new OrderManagementService().GetPrintOrderYXDR(id, Flag);
            #region 打印需要Article和size，在这里获取
            //List<ProductSearch> productListS = new List<ProductSearch>();
            //for (int i = 0; i < response.Result.OrderDetailCollection.Count(); i++)
            //{
            //    ProductSearch ps = new ProductSearch();
            //    ps.SKU = response.Result.OrderDetailCollection.ToArray()[i].SKU;
            //    productListS.Add(ps);
            //}
            //IEnumerable<ProductSearch> products = ApplicationConfigHelper.GetSearchProductYXDR(base.UserInfo.CustomerID, productListS, "SKU");
            //response.Result.OrderDetailCollection.Each((a, b) =>
            //{
            //    b.Article = products.Where(m => m.SKU == b.SKU).ToArray().Length > 0 ? products.Where(m => m.SKU == b.SKU).ToArray()[0].Str10 : "";
            //    b.Size = products.Where(m => m.SKU == b.SKU).ToArray().Length > 0 ? products.Where(m => m.SKU == b.SKU).ToArray()[0].Str9 : "";
            //    b.GoodsName = products.Where(m => m.SKU == b.SKU).ToArray().Length > 0 ? products.Where(m => m.SKU == b.SKU).ToArray()[0].GoodsName : "";
            //});
            #endregion

            vm.OrderCollection = response.Result.OrderCollection;
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;
            var IDlist = "";
            foreach (var item in vm.OrderCollection)
            {
                IDlist += item.ID.ToString() + ',';
            }
            ViewBag.IDs = IDlist.Substring(0, IDlist.Length - 1);
            return View(vm);
        }
        /// <summary>
        /// 电商打印关联默认拣货单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PrintHeaderAndDetail(string id, int Flag)
        {
            OrderViewModel vm = new OrderViewModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderType_" + base.UserInfo.ProjectName);
            }
            catch
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderType");
            }
            List<SelectListItem> slt = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                slt.Add(new SelectListItem { Value = w.Name, Text = w.Name });
            }
            vm.OrderType = slt;
            ViewBag.ID = id;

            var response = new OrderManagementService().GetPrintOrderYXDR(id, Flag);


            vm.OrderCollection = response.Result.OrderCollection;
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;
            var IDlist = "";
            foreach (var item in vm.OrderCollection)
            {
                IDlist += item.ID.ToString() + ',';
            }
            ViewBag.IDs = IDlist.Substring(0, IDlist.Length - 1);
            return View(vm);
        }
        /// <summary>
        /// 快递包装
        /// </summary>
        /// <returns></returns>
        public ActionResult Expresspackage(ExpressPackgeModel vm, string ViewType = "0")
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            IEnumerable<SelectListItem> WarehouseList = null;
            WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == base.UserInfo.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            ViewBag.WarehouseList = WarehouseList;

            //vm.SupplieTypeList = getSupplieTypeList();
            vm.SupplieTypeList = ApplicationConfigHelper.GetApplicationBox(base.UserInfo.ProjectName + "B2C").Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name });
            return View(vm);
        }
        /// <summary>
        /// 检测快递单号
        /// </summary>
        /// <param name="ExpressNumber"></param>
        /// <returns></returns>
        //public string CheckExpress(string ExpressNumber)
        //{
        //    ExpressPackageResponse EPR = new ExpressPackageResponse();
        //    ExpressPackgeModel EP = new ExpressPackgeModel();

        //    OrderECManagementService o = new OrderECManagementService();
        //    EPR = o.CheckExpress(ExpressNumber, CustomerID, WarehouseName);
        //    EP.PackageCollection = EPR.PackageCollection;
        //    EP.OrderDetailCollection = EPR.OrderDetailCollection;

        //    string s = "";
        //    DataSet dt = new DataSet();
        //    dt = o.CheckExpress(ExpressNumber, CustomerID, WarehouseName, s);

        //    //return DataSetToJson(dt,"");
        //    return DataTableToJsonWithJsonNet(dt.Tables[0]);
        //}
        /// <summary>
        /// 检测快递单号和订单号
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="Type">区分快递单号还是订单号</param>
        /// <returns></returns>
        public string CheckExpressOrder(string Number, string Type, long CustomerID, long WarehouseID)
        {
            //ExpressPackageResponse EPR = new ExpressPackageResponse();
            //ExpressPackgeModel EP = new ExpressPackgeModel();

            string WarehouseName = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(a => a.WarehouseID == WarehouseID).Select(b => b.WarehouseName).FirstOrDefault();
            OrderECManagementService o = new OrderECManagementService();
            //EPR = o.CheckExpress(Number, CustomerID, WarehouseName);
            //EP.PackageCollection = EPR.PackageCollection;
            //EP.OrderDetailCollection = EPR.OrderDetailCollection;

            DataSet dt = new DataSet();
            dt = o.CheckExpress(Number, CustomerID, WarehouseName, Type);

            //return DataSetToJson(dt,"");
            if (Type == "Order")
            {
                return DataTableToJsonWithJsonNet(dt.Tables[1]);
            }
            return DataTableToJsonWithJsonNet(dt.Tables[0]);
        }
        /// <summary>
        /// datatable转JSON
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public string DataTableToJsonWithJsonNet(DataTable table)
        {
            string jsonString = string.Empty;
            jsonString = JsonConvert.SerializeObject(table);
            return jsonString;
        }
        /// <summary>
        /// dataSet转JSON
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="tblname"></param>
        /// <returns></returns>
        public static string DataSetToJson(DataSet ds, string tblname)
        {
            string json = string.Empty;
            try
            {
                if (ds.Tables.Count == 0)
                    throw new Exception("DataSet中Tables为0");
                json = "{";
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    json += "'tbl" + i + "'" + ":[";
                    for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                    {
                        json += "{";
                        for (int k = 0; k < ds.Tables[i].Columns.Count; k++)
                        {
                            json += "'" + ds.Tables[i].Columns[k].ColumnName + "'" + ":'" + ds.Tables[i].Rows[j][k].ToString() + "'";
                            if (k != ds.Tables[i].Columns.Count - 1)
                                json += ",";
                        }
                        json += "}";
                        if (j != ds.Tables[i].Rows.Count - 1)
                            json += ",";
                    }
                    json += "]";
                    if (i != ds.Tables.Count - 1)
                        json += ",";
                }
                json += "}";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return json;
        }
        /// <summary>
        /// 保存快递包装数据
        /// </summary>
        /// <param name="uu"></param>
        /// <returns></returns>
        public string SaveExpressPackage(long CustomerID, string WarehouseName, long OrderID, string ExpressNumber, string PackageType, string PackageCode)
        {

            DataSet dt = new DataSet();
            OrderECManagementService o = new OrderECManagementService();

            dt = o.SaveExpressPackage(ExpressNumber, PackageType, CustomerID, WarehouseName, PackageCode);
            #region 操作日志
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            WMS_Log_Operation operation = new WMS_Log_Operation();
            operation.MenuName = "快递包装管理";
            operation.Operation = "快递包装-保存";
            operation.OrderType = "ExpressPackage";
            operation.Controller = Request.RawUrl;
            operation.Creator = base.UserInfo.Name;
            operation.CreateTime = DateTime.Now;
            operation.ProjectID = (int)base.UserInfo.ProjectID;
            operation.ProjectName = base.UserInfo.ProjectName;
            operation.OrderID = OrderID.ToString();
            logs.Add(operation);
            new LogOperationService().AddLogOperation(logs);
            #endregion
            return DataTableToJsonWithJsonNet(dt.Tables[0]);

        }
        /// <summary>
        /// 获取配置的耗材类型
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> getSupplieTypeList()
        {
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("BoxSize_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("BoxSize");
            }
            List<SelectListItem> SupplieTypeList = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                SupplieTypeList.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            return SupplieTypeList;
        }
        /// <summary>
        /// 获取配置的耗材类型转JSON
        /// </summary>
        /// <returns>package代表1，2，3，箱型code</returns>
        public string getSupplieTypeListJSON(string PackageType)
        {
            string js = string.Empty;
            //IEnumerable<WMSConfig> wms = null;
            //try
            //{
            //    wms = ApplicationConfigHelper.GetWMS_Config("BoxSize_" + base.UserInfo.ProjectName);
            //}
            //catch (Exception)
            //{

            //}
            //if (wms == null)
            //{
            //    wms = ApplicationConfigHelper.GetWMS_Config("BoxSize");
            //}            

            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //if (wms.Where(m => m.Name == PackageType).Count() == 0)
            //{
            //    js = "";
            //}
            //else
            //{
            //    js = jsonSerializer.Serialize(wms.Where(m => m.Name == PackageType));
            //}

            //新的箱型获取方法
            IEnumerable<BoxSize> boxs = ApplicationConfigHelper.GetApplicationBox(base.UserInfo.ProjectName + "B2C");

            //IEnumerable<SelectListItem> box = ApplicationConfigHelper.GetApplicationBox(base.UserInfo.ProjectName).Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name });
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            if (boxs.Where(m => m.Str1 == PackageType).Count() <= 0)
            {
                js = "";
            }
            else
            {
                js = jsonSerializer.Serialize(boxs.Where(m => m.Str1 == PackageType));
            }

            return js;
        }
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
        public ActionResult Picking()
        {
            return View();
        }
        /// <summary>
        /// 交接出库
        /// </summary>
        /// <returns></returns>
        public ActionResult DeliveryConfirm(long? customerID)
        {
            DeliveryConfirmModel vm = new DeliveryConfirmModel();
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            vm.Customers = CustomerList;
            vm.SearchCondition = new DeliverHeaderSearchCondition();
            ViewBag.UserName = base.UserInfo.Name.ToUpper();
            vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-6);
            vm.SearchCondition.EndCreateTime = DateTime.Now;
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
                    vm.SearchCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }
            ViewBag.WarehouseList = WarehouseList;

            #region 写法淘汰
            //DeliveryConfirmModel vm = new DeliveryConfirmModel();
            //vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            //var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            //var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            //var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            //ViewBag.CustomerList = CustomerList;//客户

            //vm.SearchCondition = new DeliverHeaderSearchCondition();
            //ViewBag.UserName = base.UserInfo.Name.ToUpper();
            //vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-6);
            //vm.SearchCondition.EndCreateTime = DateTime.Now;

            //if (base.UserInfo.UserType == 0)
            //{
            //    vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            //}
            //else if (base.UserInfo.UserType == 2)
            //{
            //    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
            //    if (customerIDs != null && customerIDs.Count() == 1)
            //    {
            //        vm.SearchCondition.CustomerID = customerIDs.First();
            //    }
            //}
            //IEnumerable<SelectListItem> WarehouseList = null;
            //if (vm.SearchCondition.CustomerID == null)
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                                        .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //    StringBuilder sb = new StringBuilder();

            //    foreach (var i in CustomerListID)
            //    {
            //        sb.Append("" + i + ",");
            //    }
            //    if (sb.Length > 1)
            //    {
            //        vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
            //    }
            //    else
            //    {
            //        vm.SearchCondition.CustomerIDs = "0";
            //    }
            //}
            //else
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                         .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //}

            //ViewBag.WarehouseList = WarehouseList;//仓库列表
            //if (CustomerList.Count() == 1)
            //{
            //    vm.SearchCondition.CustomerID = CustomerList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();
            //}
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();

            //}
            #endregion

            var request = new GetDeliverByConditionRequest();
            request.SearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = 0;

            var response = new DeliverConfirmService().GetDeliverHeaderByCondition(request);

            if (response.IsSuccess)
            {
                vm.DeliverHeaderConnection = response.Result.DeliverHeaderConnection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }
        /// <summary>
        /// 交接出库
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeliveryConfirm(DeliveryConfirmModel vm, int? PageIndex, string Action)
        {
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            ViewBag.UserName = base.UserInfo.Name.ToUpper();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
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
                vm.SearchCondition.CustomerID = 0;

            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }
            ViewBag.CustomerList = CustomerList;
            ViewBag.WarehouseList = WarehouseList;
            if (vm.SearchCondition.WarehouseName != null)
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
                    vm.SearchCondition.WarehouseName = sb.Remove(sb.Length - 1, 1).ToString();
                }

            }
            #region 淘汰！
            //vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            //var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            //var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            //var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            //ViewBag.CustomerList = CustomerList;//客户


            //ViewBag.UserName = base.UserInfo.Name.ToUpper();
            //vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-6);
            //vm.SearchCondition.EndCreateTime = DateTime.Now;

            //if (base.UserInfo.UserType == 0)
            //{
            //    vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            //}
            //else if (base.UserInfo.UserType == 2)
            //{
            //    var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
            //    if (customerIDs != null && customerIDs.Count() == 1)
            //    {
            //        vm.SearchCondition.CustomerID = customerIDs.First();
            //    }
            //}
            //IEnumerable<SelectListItem> WarehouseList = null;
            //if (vm.SearchCondition.CustomerID == null)
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                                        .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //    StringBuilder sb = new StringBuilder();

            //    foreach (var i in CustomerListID)
            //    {
            //        sb.Append("" + i + ",");
            //    }
            //    if (sb.Length > 1)
            //    {
            //        vm.SearchCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
            //    }
            //    else
            //    {
            //        vm.SearchCondition.CustomerIDs = "0";
            //    }
            //}
            //else
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                         .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //}

            //ViewBag.WarehouseList = WarehouseList;//仓库列表
            //if (CustomerList.Count() == 1)
            //{
            //    vm.SearchCondition.CustomerID = CustomerList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();
            //}
            //if (WarehouseList.Count() == 1)
            //{
            //    vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt32();

            //}
            #endregion


            var request = new GetDeliverByConditionRequest();
            request.SearchCondition = vm.SearchCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;

            var response = new DeliverConfirmService().GetDeliverHeaderByCondition(request);
            if (response.IsSuccess)
            {
                vm.DeliverHeaderConnection = response.Result.DeliverHeaderConnection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            ViewBag.ProjectName = base.UserInfo.ProjectName;
            return View(vm);
        }
        /// <summary>
        /// 交接单明细
        /// </summary>
        /// <param name="ID">主表ID</param>
        /// <returns></returns>
        public ActionResult DeliverDetail(long? ID, string type, long customerID, string warehouse)
        {
            DeliveryConfirmModel vm = new DeliveryConfirmModel();

            //type=0新增，type=1查看明细
            if (type == "1")
            {
                var response = new DeliverConfirmService().GetDeliverHeaderAndDetailByID(Convert.ToInt32(ID));
                if (response.IsSuccess)
                {
                    vm.DeliverHeaderConnection = response.Result.DeliverHeaderConnection;//交接单表头
                    //vm.DeliverDetailConnection = response.Result.DeliverDetailConnection;//交接单明细
                    vm.DeliverExpressNoConnection = response.Result.DeliverExpressNoConnection;//快递单列表
                }
            }
            ViewBag.Type = type;
            ViewBag.ProjectName = base.UserInfo.ProjectName;

            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerName = CustomerListAll.Where(t => t.CustomerID == customerID).FirstOrDefault().CustomerName;
            ViewBag.CustomerID = customerID;
            ViewBag.CustomerName = CustomerName;
            ViewBag.Warehouse = warehouse;//仓库

            return View(vm);
        }
        /// <summary>
        /// 交接称重验证快递单号
        /// </summary>
        /// <param name="ExpressNumber"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public string VilidateDeliverExpress(string ExpressNumber, long customerID, string warehouse)
        {

            return new DeliverConfirmService().VilidateDeliverExpress(ExpressNumber, customerID, warehouse);
        }
        /// <summary>
        /// 交接称重获取待上传信息
        /// </summary>
        /// <param name="ExpressNumber"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public JsonResult DeliverUploadData(string ExpressNumber, long customerID, string warehouse)
        {
            IEnumerable<DeliverDetail> deliverdetail = null;

            var response = new DeliverConfirmService().GetDeliverUploadData(ExpressNumber, customerID, warehouse);
            if (response.IsSuccess && response.Result.DeliverDetailConnection.Count() > 0)
            {
                deliverdetail = response.Result.DeliverDetailConnection;
                var data = from q in deliverdetail
                           select new
                           {
                               q.OrderNumber,
                               q.str3,
                               q.CustomerID,
                               q.str1,
                               q.str2
                           };
                return Json(new { error = 1, data = data });
            }

            return Json(new { error = 0, data = "未查询到待上传信息！" });
        }
        /// <summary>
        /// 验证订单是否是取消单
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="customerID"></param>
        /// <param name="type">1.代表传入的是系统ordernumber，2.代表快递单号，3.代表外部单号</param>
        /// <returns></returns>
        public string ValidOrderCancel(string OrderNumber, long customerID, string warehouse, int type)
        {
            //查询订单是否是取消单调不同存过
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
            return new DeliverConfirmService().ValidOrderCancel(OrderNumber, customerID, wms.FirstOrDefault().Name, warehouse, type);
        }
        /// <summary>
        /// 验证交接单里面的明细是否有前面称重的时候没拦住的取消单
        /// </summary>
        /// <param name="DeliverID"></param>
        /// <returns>code=0代表没问题，1代表存在取消单，2，代表报错了</returns>
        public JsonResult ValidDeliverOrderCancel(long DeliverID)
        {
            try
            {
                IEnumerable<OrderInfo> orders = new DeliverConfirmService().ValidDeliverOrderCancel(DeliverID);
                if (orders != null && orders.Any())
                {
                    return Json(new { code = 1, data = orders.Select(m => m.ExternOrderNumber).ToList() });
                }
                else
                {
                    return Json(new { code = 0, msg = "" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { code = 2, msg = ex.Message.ToString() });
            }
        }
        /// <summary>
        /// 新增交接称重
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="jsonString">明细</param>
        /// <param name="ExpressCompany">快递公司</param>
        /// <param name="DeliverKey">交接单号</param>
        /// <returns></returns>
        [HttpPost]
        public string DeliverHeaderAndDetailAdd(long customerID, string jsonString, string ExpressCompany, string DeliverKey, int flag, string warehouse)
        {
            var responseJsonFieldsets = jsonlist<DeliverDetail>(jsonString).OrderBy(m => m.DeliverDetailKey);
            AddDeliverAndDetailRequest request = new AddDeliverAndDetailRequest();//添加交接主表和明细
            List<DeliverHeader> delivers = new List<DeliverHeader>();
            List<DeliverDetail> deliverdetails = new List<DeliverDetail>();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();

            //根据传进来的ID获取客户名称,这样准确
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerName = CustomerListAll.Where(t => t.CustomerID == customerID).FirstOrDefault().CustomerName;

            //获取客户对应的仓库
            var warehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID))
                               .Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                               .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //唯一仓库ID
            var WarehouseID = warehouseList.Where(m => m.Text == warehouse).FirstOrDefault().Value;





            //主表赋值
            delivers.Add(new DeliverHeader()
            {
                DeliverKey = DeliverKey,
                Status = "0",
                CustomerID = customerID,
                CustomerName = CustomerName,
                WarehouseID = WarehouseID.ObjectToInt64(),
                WarehouseName = warehouse,
                ExpressCompany = ExpressCompany,//快递公司
                Creator = base.UserInfo.Name,//创建人
                Updator = base.UserInfo.Name

            });
            //子表赋值
            responseJsonFieldsets.Each((a, detail) =>
            {
                deliverdetails.Add(new DeliverDetail()
                {
                    DeliverKey = detail.DeliverKey,//交接单号
                    DeliverID = detail.DeliverID,//交接单ID
                    DeliverDetailKey = detail.DeliverDetailKey,//行号                       
                    OrderNumber = detail.OrderNumber,
                    ExpressNumber = detail.ExpressNumber,//快递单号
                    BoxWeight = detail.BoxWeight,
                    PackBoxKey = detail.PackBoxKey,
                    Status = "0",
                    CustomerID = customerID,
                    CustomerName = CustomerName,
                    WarehouseID = WarehouseID.ObjectToInt64(),//仓库ID
                    WarehouseName = warehouse,//仓库名称
                    Creator = base.UserInfo.Name,
                    Updator = base.UserInfo.Name
                });

                #region 屏蔽
                ////新增时0，保存时1
                //if (flag == 0)
                //{
                //    //新增第一条（一条头信息，一条明细信息）
                //    if (DeliverKey == "自动生成编号")//新增第一条的时候
                //    {
                //        WMS_Log_Operation operation = new WMS_Log_Operation();
                //        operation.MenuName = "电商出库单管理";
                //        operation.Operation = "交接单-新增主表";
                //        operation.OrderType = "Delivery";
                //        operation.Controller = Request.RawUrl;
                //        operation.Creator = base.UserInfo.Name;//用户名
                //        operation.ProjectID = (int)base.UserInfo.ProjectID;
                //        operation.ProjectName = base.UserInfo.ProjectName;
                //        operation.CustomerID = (int)customerID;
                //        operation.CustomerName = CustomerName;
                //        operation.WarehouseID = Convert.ToInt32(WarehouseID.ToString());
                //        operation.WarehouseName = warehouse;
                //        operation.OrderID = "";//新增时主表没有ID
                //        operation.OrderNumber = DeliverKey;
                //    }

                //}



                //if (flag == 0)
                //{
                //    WMS_Log_Operation operation = new WMS_Log_Operation();
                //    operation.MenuName = "电商出库单管理";
                //    operation.Operation = "交接单-新增明细";
                //    operation.OrderType = "Deliver";
                //    operation.Controller = Request.RawUrl;//URL                   
                //    operation.Creator = base.UserInfo.Name;
                //    operation.CreateTime = DateTime.Now;
                //    operation.ProjectID = (int)base.UserInfo.ProjectID;
                //    operation.ProjectName = base.UserInfo.ProjectName;
                //    operation.CustomerID = (int)customerID;
                //    operation.OrderID = "";//新增的时候看不到订单ID
                //    operation.OrderNumber = detail.OrderNumber;//订单号

                //    operation.Str1 = detail.ExpressNumber;//快递单号
                //    operation.Str2 = detail.BoxWeight;//重量

                //    logs.Add(operation);
                //}
                //else
                //{
                //    WMS_Log_Operation operation = new WMS_Log_Operation();
                //    operation.MenuName = "电商出库单管理";
                //    operation.Operation = "交接单-保存";
                //    operation.OrderType = "Deliver";
                //    operation.Controller = Request.RawUrl;//URL                   
                //    operation.Creator = base.UserInfo.Name;
                //    operation.CreateTime = DateTime.Now;
                //    operation.ProjectID = (int)base.UserInfo.ProjectID;
                //    operation.ProjectName = base.UserInfo.ProjectName;
                //    operation.CustomerID = (int)customerID;
                //    operation.OrderID = "";
                //    operation.OrderNumber = detail.OrderNumber;//订单号
                //    operation.Str1 = detail.ExpressNumber;//快递单号
                //    operation.Str2 = detail.BoxWeight;//重量
                //    logs.Add(operation);
                //}
                #endregion

            });

            request.DeliverHeaderConnection = delivers;
            request.DeliverDetailConnection = deliverdetails;
            var response = new DeliverConfirmService().AddDeliverAndDetail(request, flag);//提交

            //new LogOperationService().AddLogOperation(logs);//日志

            return new { result = response.Result, IsSuccess = response.IsSuccess }.ToJsonString();

        }
        /// <summary>
        /// 删除交接单明细
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="DeliverKey"></param>
        /// <param name="ExpressNumber"></param>
        /// <returns></returns>
        public string DeliverDetailDelete(long customerID, string DeliverKey, string ExpressNumber, string warehouse)
        {

            return new DeliverConfirmService().DeliverDetailDelete(customerID, DeliverKey, ExpressNumber, warehouse);

        }
        /// <summary>
        /// YXDR交接清单打印
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="deliverID"></param>
        /// <returns></returns>
        public ActionResult PrintDeliveryYXDR(long deliverID)
        {
            DeliveryConfirmModel vm = new DeliveryConfirmModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PrintDelivery_" + base.UserInfo.ProjectName);
            }
            catch
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PrintDelivery");
            }

            var response = new DeliverConfirmService().GetPrintDelivery(deliverID, wms.FirstOrDefault().Name);
            if (response.IsSuccess)
            {
                vm.DeliverHeaderConnection = response.Result.DeliverHeaderConnection;
                vm.DeliverDetailConnection = response.Result.DeliverDetailConnection;
            }
            ViewBag.DeliverID = deliverID;
            return View(vm);
        }
        /// <summary>
        /// 交接单在提交出库时验证交接明细
        /// </summary>
        /// <param name="DeliverID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public string DeliverCompleteInfoValidate(long DeliverID, long customerID)
        {
            return new DeliverConfirmService().DeliverCompleteInfoValidate(DeliverID, customerID);

        }
        /// <summary>
        /// 交接单提交出库
        /// </summary>
        /// <param name="DeliverID">交接主表ID</param>
        /// <returns></returns>
        public string DeliverOut(long DeliverID)
        {
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            WMS_Log_Operation operation = new WMS_Log_Operation();
            operation.MenuName = "电商出库单管理";
            operation.Operation = "交接单-提交出库";
            operation.OrderType = "DeliverOut";
            operation.Controller = Request.RawUrl;//URL                   
            operation.Creator = base.UserInfo.Name;
            operation.CreateTime = DateTime.Now;
            operation.ProjectID = (int)base.UserInfo.ProjectID;
            operation.ProjectName = base.UserInfo.ProjectName;
            operation.OrderID = DeliverID.ToString();
            operation.OrderNumber = "";//订单号           

            var response = new DeliverConfirmService().DeliverOut(DeliverID);
            operation.Remark = string.IsNullOrEmpty(response.Result) ? "成功" : "失败";
            logs.Add(operation);
            new LogOperationService().AddLogOperation(logs);//提交出库日志
            return response.Result;
        }
        /// <summary>
        /// 交接清单打印
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="deliverID"></param>
        /// <returns></returns>
        public ActionResult PrintDelivery(long deliverID)
        {
            DeliveryConfirmModel vm = new DeliveryConfirmModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PrintDelivery_" + base.UserInfo.ProjectName);
            }
            catch
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PrintDelivery");
            }

            var response = new DeliverConfirmService().GetPrintDelivery(deliverID, wms.FirstOrDefault().Name);
            if (response.IsSuccess)
            {
                vm.DeliverHeaderConnection = response.Result.DeliverHeaderConnection;
                vm.DeliverDetailConnection = response.Result.DeliverDetailConnection;
            }
            ViewBag.DeliverID = deliverID;
            return View(vm);
        }
        public string UpdatePrintHeaderStatus(string IDs)
        {
            return new PrintHeaderService().UpdatePrintStatus(IDs);
        }
        /// <summary>
        /// 爱库存打印箱清单
        /// </summary>
        /// <param name="id">订单ID</param>
        /// <param name="type">打印类型(1=批量,0=单箱 2多单)</param>
        /// <param name="OrderID">订单号</param>
        /// <returns></returns>
        public ActionResult PrintBoxListSAKC(string id, string type, string OrderID)
        {
            ViewBag.OrderID = OrderID;
            ViewBag.Time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            PrintBoxModel print = new PrintBoxModel();

            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PackageBoxList_" + base.UserInfo.ProjectName);
            }
            catch
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PackageBoxList");
            }
            if (type == "1") //批量打印//str17存放的是箱唛号外部单号后七位+箱号后三位
            {
                print.EnumerableCustomerInfo = new OrderManagementService().GetPrintBoxListCondition(id.ToString(), type, wms.FirstOrDefault().Name).EnumerableBoxListinfo;
            }
            else if (type == "0")
            {
                print.EnumerableCustomerInfo = new OrderManagementService().GetPrintBoxListCondition(id.ToString(), type, wms.FirstOrDefault().Name).EnumerableBoxListinfo;
            }
            else if (type == "2")
            {
                print.EnumerableCustomerInfo = new OrderManagementService().GetPrintBoxListCondition(id.ToString(), type, wms.FirstOrDefault().Name).EnumerableBoxListinfo;
            }

            IEnumerable<string> orders = print.EnumerableCustomerInfo.Select(m => m.OrderNumber).Distinct();

            var groupOrder = print.EnumerableCustomerInfo.GroupBy(m => new { m.OrderNumber }).Select(m => new { OrderNumber = m.Key.OrderNumber, PageCount = m.Count(p => p.OrderNumber == m.Key.OrderNumber) }).ToList();

            //PrintBoxModel print = new PrintBoxModel();
            //print.EnumerableCustomerInfo = new OrderManagementService().GetPrintBoxListCondition(id.ToString(), type).EnumerableBoxListinfo;
            return View(print);
        }
        /// <summary>
        /// （爱库存）批量打印快递面单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PrintExpressAKC(string id, int type)
        {
            PrintExpressModel model = new PrintExpressModel();
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PrintExpress_" + base.UserInfo.ProjectName);
            }
            catch
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("PackageBoxList");
            }
            model.OrderInfos = new OrderManagementService().GetPrintExpressListCondition(id, wms.FirstOrDefault().Name.ToString(), type);

            return View(model);
        }
        /// <summary>
        /// 验证是否超过波次大小
        /// </summary>
        /// <param name="HeaderID"></param>
        /// <param name="IDs">勾选的订单ID</param>
        /// <returns></returns>
        [HttpPost]
        public string VerifyWaveSize(long HeaderID, string IDs)
        {
            try
            {
                //得到波次大小
                IEnumerable<WMSConfig> wms = null;
                try
                {
                    wms = ApplicationConfigHelper.GetWMS_Config("WaveSize_" + base.UserInfo.ProjectName);
                }
                catch (Exception)
                {
                }
                //此项目没有配波次大小所以不需要验证
                if (wms == null)
                {
                    return new { code = 0, msg = "" }.ToJsonString();
                }
                //将传进来的订单查询出来
                IEnumerable<OrderInfo> orderInfos = new PrintHeaderService().GetOrderInfoByPrintID(HeaderID, IDs);
                //未查到
                if (orderInfos == null && !orderInfos.Any())
                {
                    return new { code = 402, msg = "新增失败，请重新勾选订单进行打印关联！" }.ToJsonString();
                }
                List<string> typelist = orderInfos.Select(m => m.OrderType).Distinct().ToList();
                if (typelist.Count() == 1)
                {
                    //这个订单类型需要验证波次大小
                    WMSConfig config = wms.Where(m => m.Name == typelist.FirstOrDefault().ToString()).FirstOrDefault();
                    if (config != null)
                    {
                        int waveSize = config.Code.ObjectToInt32();
                        if (waveSize >= orderInfos.Count())
                        {
                            return new { code = 0, msg = "" }.ToJsonString();
                        }
                        else
                        {
                            return new { code = 402, msg = "新增失败！当前订单类型为" + config.Name + ",勾选的订单数量不允许超过波次大小！" }.ToJsonString();
                        }
                    }
                    else
                    {
                        //不需要验证类型
                        return new { code = 0, msg = "" }.ToJsonString();
                    }
                }
                else
                {
                    return new { code = 402, msg = "新增失败，请选择相同类型的订单进行关联！" }.ToJsonString();
                }

            }
            catch (Exception ex)
            {
                return new { code = 402, msg = ex.Message.ToString() }.ToJsonString();
            }

        }
        /// <summary>
        /// 打印波次拣货单
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintWaveOrderAKC(string ids)
        {
            OrderViewModel vm = new OrderViewModel();
            var response = new OrderManagementService().GetPrintWaveOrderAKC(ids);
            vm.OrderCollection = response.Result.OrderCollection;
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;

            return View(vm);
        }
        /// <summary>
        /// 交接单在提交出库时验证对应快递单是否为可出库状态
        /// </summary>
        /// <param name="DeliverID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckExpressStatus(long DeliverID, long customerID)
        {
            List<WMS_CheckExpress> response;
            try
            {
                response = new DeliverConfirmService().CheckExpressStatus(DeliverID, customerID);

                if (response.Count >0)
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

    }
}