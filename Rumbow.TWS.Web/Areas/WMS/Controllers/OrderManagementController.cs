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
using Runbow.TWS.Entity.WMS.Log;
using CSRedis;
using System.Net;
using Runbow.TWS.MessageContracts.WMS.JCApi;
using System.Threading.Tasks;
using Runbow.TWS.Entity.WMS.NikeOSRBJPrint;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class OrderManagementController : BaseController
    {
        public ActionResult Index(long? customerID)
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
            ViewBag.CustomerID = customerID == null ? base.UserInfo.CustomerID : customerID;
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

            #region 屏蔽

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

            var WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == vm.SearchCondition.Warehouse).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });

            ViewBag.WorkStation = WorkStation;
            var getOrderByConditionRequest = new GetOrderByConditionRequest();
            getOrderByConditionRequest.SearchCondition = vm.SearchCondition;
            getOrderByConditionRequest.SearchCondition.Model = "物料";
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
        /// 阿克苏出库单发运管理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OrderShipment(long? customerID)
        {
            return View();
        }

        /// <summary>
        /// 获取下拉框需要绑定的数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetOrderShipmentWhere(long? customerID)
        {
            try
            {
                OrderShipmentModel vm = new OrderShipmentModel();
                #region 获取状态和类型
                IEnumerable<WMSConfig> shipmentstatusconfig = null;
                try
                {
                    shipmentstatusconfig = ApplicationConfigHelper.GetWMS_Config("ShipmentStatus_" + base.UserInfo.ProjectName);
                }
                catch (Exception)
                {
                }
                if (shipmentstatusconfig == null)
                {
                    shipmentstatusconfig = ApplicationConfigHelper.GetWMS_Config("ShipmentStatus");
                }
                List<SelectListItem> shipmentstatus = new List<SelectListItem>();
                foreach (var item in shipmentstatusconfig)
                {
                    shipmentstatus.Add(new SelectListItem()
                    {
                        Value = item.Code,
                        Text = item.Name
                    });
                }
                vm.StatusList = shipmentstatus;
                IEnumerable<WMSConfig> shipmenttypeconfig = null;
                try
                {
                    shipmenttypeconfig = ApplicationConfigHelper.GetWMS_Config("ShipmentType_" + base.UserInfo.ProjectName);
                }
                catch (Exception)
                {
                }
                if (shipmenttypeconfig == null)
                {
                    shipmenttypeconfig = ApplicationConfigHelper.GetWMS_Config("ShipmentType");
                }
                List<SelectListItem> shipmenttype = new List<SelectListItem>();
                foreach (var item in shipmenttypeconfig)
                {
                    shipmenttype.Add(new SelectListItem()
                    {
                        Value = item.Code,
                        Text = item.Name
                    });
                }
                vm.TypeList = shipmenttype;
                #endregion

                vm.SearchCondition = new OrderShipmentSearchCondition();
                //vm.SearchCondition.StartCreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
                //vm.SearchCondition.EndCreateTime = DateTime.Now.ToString("yyyy-MM-dd").ObjectToNullableDateTime();

                vm.SearchCondition.StartCreateTime = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");
                vm.SearchCondition.EndCreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
                var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
                var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
                //ViewBag.CustomerList = CustomerList;
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
                IEnumerable<SelectListItem> WarehouseList = null;
                if (vm.SearchCondition.CustomerID == 0)
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
        /// 获取发运单列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public string GetOrderShipmentList(RequestModel request)
        {
            ResponseModel res = new ResponseModel();
            string msg = string.Empty;
            res.code = 401;
            OrderShipmentSearchCondition searchCondition = new OrderShipmentSearchCondition();

            try
            {
                searchCondition = new JavaScriptSerializer().Deserialize<OrderShipmentSearchCondition>(request.requestData);
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
                    searchCondition.PageSize = request.limit > 0 ? request.limit : 20;
                    OrderShipmentResponse response = new OrderShipmentResponse();
                    int rowcounts = 0;
                    response = new OrderManagementService().GetOrderShipmentList(searchCondition, out msg, out rowcounts);
                    if (response != null && response.shipmentAndDetails.Count() > 0 && msg == "")
                    {
                        res.code = 0;
                        res.count = rowcounts;
                        foreach (var item in response.shipmentAndDetails)
                        {
                            item.OrderList = response.shipmentDetails.Where(m => m.SID == item.ID).ToList();
                        }
                        res.data = response.shipmentAndDetails;
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
            return new JavaScriptSerializer().Serialize(res);
        }

        /// <summary>
        /// 发送S5场景打印由index页面触发
        /// </summary>
        /// <param name="IDs"></param>
        /// <returns></returns>
        [HttpPost]
        public string SendS5ShipmentPrint(string IDs)
        {
            ResponseModel res = new ResponseModel();
            res.code = 401;
            string msg = string.Empty;
            if (string.IsNullOrEmpty(IDs))
            {
                res.msg = "请选择需要发送的订单！";
                res.code = 402;
                return new JavaScriptSerializer().Serialize(res);
            }
            //发送
            try
            {
                //获取订单
                IEnumerable<OrderInfo> orderInfos = new OrderManagementService().GetOrderInfosByIDs(IDs);
                if (orderInfos == null || !orderInfos.Any())
                {
                    res.code = 402;
                    res.msg = "没有找到这些订单，请刷新界面！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                //同一个客户
                if (orderInfos.Select(m => m.CustomerID).Distinct().Count() > 1)
                {
                    res.code = 402;
                    res.msg = "请选择同一个客户的订单！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                //必须是同一家仓库的
                if (orderInfos.Select(m => m.Warehouse).Distinct().Count() > 1)
                {
                    res.code = 402;
                    res.msg = "请选择同一家仓库的订单！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                //判断场景
                if (orderInfos.Where(m => m.str7 != "5").Count() > 0)
                {
                    res.code = 402;
                    res.msg = "只能发送 场景5 的订单，请检查！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                //先判断订单是不是都已经出库了
                if (orderInfos.Where(m => m.Status != 9).Count() > 0)
                {
                    res.code = 402;
                    res.msg = "存在没有出库的订单，请检查！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                //判断是否存在已经发送过的订单
                if (orderInfos.Where(m => m.Int3 == 1).Count() > 0)
                {
                    res.code = 402;
                    res.msg = "存在已经发送过的订单，请检查！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                //判断这些订单是否都已经反馈了
                //2.出库反馈：5中订单类型：NLCC转仓订单、NL转仓订单、调色出库、调整出库、正常出库
                //除了调整出库和调色出库其他都需要反馈两次
                if (orderInfos.Where(m => m.Int1 == null || m.Int1 == 0).Count() > 0)
                {
                    res.code = 402;
                    res.msg = "存在出库单还没反馈的情况，请稍后重试！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                //是否存在没有二次反馈的订单
                //if (orderInfos.Where(m => m.OrderType == "正常出库" && m.OrderType == "NL转仓订单" && m.OrderType == "NLCC转仓订单").ToList().Where(m => m.Int2 == null || m.Int2 == 0).Count() > 0)
                //{
                //    res.code = 402;
                //    res.msg = "存在出库单还没二次反馈的情况，请稍后重试！";
                //    return new JavaScriptSerializer().Serialize(res);
                //}

                //生成运单插入到运单表
                OrderShipmentRequest request = new OrderShipmentRequest();
                List<WMS_OrderShipment> shipment = new List<WMS_OrderShipment>();
                List<WMS_OrderShipmentDetail> shipmentDetails = new List<WMS_OrderShipmentDetail>();
                shipment.Add(new WMS_OrderShipment()
                {
                    ShipmentNumber = "SHIP" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999).ToString(),
                    //ShipmentNumber = Guid.NewGuid().ToString("N").ToUpper(),//唯一guid
                    CustomerID = orderInfos.Select(m => m.CustomerID).FirstOrDefault(),
                    CustomerName = orderInfos.Select(m => m.CustomerName).FirstOrDefault(),
                    WarehouseID = orderInfos.Select(m => m.Int5).FirstOrDefault(),
                    WarehouseName = orderInfos.Select(m => m.Warehouse).FirstOrDefault(),
                    Status = 2,//状态直接就是已发送
                    Type = 2,
                    Creator = base.UserInfo.Name,
                    PrintCreator = base.UserInfo.Name,//因为初始就是2，所以加上打印人

                });
                foreach (var item in orderInfos)
                {
                    shipmentDetails.Add(new WMS_OrderShipmentDetail()
                    {
                        ShipmentNumber = shipment.FirstOrDefault().ShipmentNumber,
                        ExternOrderNumber = item.ExternOrderNumber,
                        CustomerID = item.CustomerID,
                        CustomerName = item.CustomerName,
                        WarehouseID = item.Int5,
                        WarehouseName = item.Warehouse,
                        Creator = base.UserInfo.Name

                    });
                }
                request.shipments = shipment;
                request.shipmentDetails = shipmentDetails;
                var response = new OrderManagementService().AddOrderShipmentAndDetail(request, 2);
                if (response.Result == null || response.Result.shipments == null || !response.Result.shipments.Any() || response.Result.shipmentDetails == null || !response.Result.shipmentDetails.Any())
                {
                    //失败
                    msg = "发送失败,数据库插入失败";
                    res.code = 402;
                }
                else
                {
                    //成功之后把订单表的int3更新成1更新成已发送，这样出库单界面上就能看到已经发送了
                    IEnumerable<long> idlist = orderInfos.Select(m => m.ID).ToList();
                    string idstr = String.Join(",", idlist);
                    new ASNManagementService().UpdateConfirmStatus("8", idstr, "");
                    res.code = 0;
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
        /// 发送S1-4场景的打印，由发运界面触发
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>苦中作乐你懂我意思吧？</returns>
        [HttpPost]
        public string SendS14ShipmentPrint(long ID, int Type)
        {
            ResponseModel res = new ResponseModel();
            res.code = 401;
            string msg = string.Empty;
            if (ID <= 0)
            {
                res.code = 402;
                res.msg = "没有找到需要发送打印的运单，请刷新界面重试！";
                return new JavaScriptSerializer().Serialize(res);
            }
            try
            {
                //先得到需要发送的运单
                OrderShipmentResponse response = new OrderShipmentResponse();
                response = new OrderManagementService().GetOrderShipmentByID(ID, Type);
                if (response == null || response.shipments == null || !response.shipments.Any())
                {
                    res.code = 402;
                    res.msg = "没有找到需要发送打印的运单，请刷新界面重试！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                if (response.shipments.FirstOrDefault().Status != 1)
                {
                    res.code = 402;
                    res.msg = "当前状态不允许发送打印，请刷新界面！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                //判断场景
                if (response.shipments.Where(m => m.Type != 1).Count() > 0)
                {
                    res.code = 402;
                    res.msg = "该运单是S5场景，不允许发送！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                //判断运单下面的订单是否已经出库
                if (response.shipmentDetails.Where(m => m.Int2 != 9).Count() > 0)
                {
                    res.code = 402;
                    res.msg = "该运单存在未完成的订单，请检查！";
                    return new JavaScriptSerializer().Serialize(res);
                }

                IEnumerable<string> idlist = response.shipmentDetails.Select(m => m.str2).ToList();
                string idstr = String.Join(",", idlist);
                //入库单
                if (Type == 1)
                {
                    IEnumerable<Receipt> receipts = new ReceiptManagementService().GetRceiptInfoByIDs(idstr);
                    //判断这些订单是否都已经反馈了
                    if (receipts.Where(m => m.Int1 == null || m.Int1 == 0).Count() > 0)
                    {
                        res.code = 402;
                        res.msg = "存在入库单还没反馈的情况，请稍后重试！";
                        return new JavaScriptSerializer().Serialize(res);
                    }
                    //把运单的状态改成2，让returnorder计划任务自己去跑，
                    bool result = new OrderManagementService().UpdateShipmentstatusByID(response.shipments.FirstOrDefault().ID, base.UserInfo.Name, 1, out msg);
                    if (result && msg == "")
                    {
                        res.code = 0;
                        //打印发送成功之后，顺便也改一下int3
                        new ASNManagementService().UpdateConfirmStatus("11", idstr, "");
                    }
                    else
                    {
                        res.code = 402;
                    }
                }
                else
                {
                    IEnumerable<OrderInfo> orderInfos = new OrderManagementService().GetOrderInfosByIDs(idstr);
                    //判断这些订单是否都已经反馈了               
                    if (orderInfos.Where(m => m.Int1 == null || m.Int1 == 0).Count() > 0)
                    {
                        res.code = 402;
                        res.msg = "存在出库单还没反馈的情况，请稍后重试！";
                        return new JavaScriptSerializer().Serialize(res);
                    }
                    //把运单的状态改成2，让returnorder计划任务自己去跑，
                    bool result = new OrderManagementService().UpdateShipmentstatusByID(response.shipments.FirstOrDefault().ID, base.UserInfo.Name, 1, out msg);
                    if (result && msg == "")
                    {
                        res.code = 0;
                        //打印发送成功之后，顺便也改一下int3
                        new ASNManagementService().UpdateConfirmStatus("8", idstr, "");
                    }
                    else
                    {
                        res.code = 402;
                    }
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
        /// 发送S1-4的货物离场，由运单界面触发
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public string SendS14ShipmentGoodsissue(long ID, int Type)
        {
            ResponseModel res = new ResponseModel();
            res.code = 401;
            string msg = string.Empty;
            if (ID <= 0)
            {
                res.code = 402;
                res.msg = "没有找到需要发送货物离场的运单，请刷新界面重试！";
                return new JavaScriptSerializer().Serialize(res);
            }
            try
            {
                //先得到需要发送的运单
                OrderShipmentResponse response = new OrderShipmentResponse();
                response = new OrderManagementService().GetOrderShipmentByID(ID, Type);
                if (response == null || response.shipments == null || !response.shipments.Any())
                {
                    res.code = 402;
                    res.msg = "没有找到需要发送货物离场的运单,请刷新界面重试！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                //判断场景
                if (response.shipments.Where(m => m.Type != 1).Count() > 0)
                {
                    res.code = 402;
                    res.msg = "该运单是S5场景，不允许发送货物离场！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                if (response.shipments.FirstOrDefault().Status != 2)
                {
                    res.code = 402;
                    res.msg = "当前状态不允许发送货物离场，请刷新界面！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                if (response.shipments.FirstOrDefault().Int1 == null || response.shipments.FirstOrDefault().Int1 == 0)
                {
                    res.code = 402;
                    res.msg = "打印消息还没有发给SAP，请稍等再发送货物离场！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                //判断运单下面的订单是否已经出库
                if (response.shipmentDetails.Where(m => m.Int2 != 9).Count() > 0)
                {
                    res.code = 402;
                    res.msg = "该运单存在未完成的订单，请检查！";
                    return new JavaScriptSerializer().Serialize(res);
                }
                IEnumerable<string> idlist = response.shipmentDetails.Select(m => m.str2).ToList();
                string idstr = String.Join(",", idlist);
                //ZLR入库单处理
                if (Type == 1)
                {
                    IEnumerable<Receipt> receipts = new ReceiptManagementService().GetRceiptInfoByIDs(idstr);
                    //判断这些订单是否都已经反馈了
                    if (receipts.Where(m => m.Int1 == null || m.Int1 == 0).Count() > 0)
                    {
                        res.code = 402;
                        res.msg = "存在入库单还没反馈的情况，请稍后重试！";
                        return new JavaScriptSerializer().Serialize(res);
                    }
                    //把运单的状态改成3，让returnorder计划任务自己去跑，
                    bool result = new OrderManagementService().UpdateShipmentstatusByID(response.shipments.FirstOrDefault().ID, base.UserInfo.Name, 2, out msg);
                    if (result && msg == "")
                    {
                        res.code = 0;
                    }
                    else
                    {
                        res.code = 402;
                    }
                }
                else
                {
                    IEnumerable<OrderInfo> orderInfos = new OrderManagementService().GetOrderInfosByIDs(idstr);
                    //判断这些订单是否都已经反馈了               
                    if (orderInfos.Where(m => m.Int1 == null || m.Int1 == 0).Count() > 0)
                    {
                        res.code = 402;
                        res.msg = "存在出库单还没反馈的情况，请稍后重试！";
                        return new JavaScriptSerializer().Serialize(res);
                    }

                    //把运单的状态改成3，让returnorder计划任务自己去跑，
                    bool result = new OrderManagementService().UpdateShipmentstatusByID(response.shipments.FirstOrDefault().ID, base.UserInfo.Name, 2, out msg);
                    if (result && msg == "")
                    {
                        res.code = 0;
                    }
                    else
                    {
                        res.code = 402;
                    }
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
        /// 出库单推送鲸仓
        /// </summary>
        /// <param name="IDS"></param>
        /// <returns></returns>
        public JsonResult OrderSend(string JCRequestList)
        {
            IList<JCAPiResponse> listResponses = new List<JCAPiResponse>();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            try
            {
                string url = UtilConstants.JCSendAPIAddress + "OrderSend";
                List<JCRequestLists> list = jsonlist<JCRequestLists>(JCRequestList.Substring(18, JCRequestList.Length - 19));
                string res = this.HTTPPost(url, JCRequestList);
                listResponses = jsonlist<JCAPiResponse>(res);
                foreach (var item in listResponses)
                {
                    logs.Add(new WMS_Log_Operation()
                    {
                        MenuName = "出库单管理",
                        Operation = "出库单-推送鲸仓",
                        OrderType = "OrderSend",
                        Controller = Request.RawUrl,
                        Creator = base.UserInfo.Name,
                        CreateTime = DateTime.Now,
                        ProjectID = (int)base.UserInfo.ProjectID,
                        ProjectName = base.UserInfo.ProjectName,
                        OrderNumber = item.relatednumber,
                        Str1 = list.Where(c => c.RelateNumber == item.relatednumber).ToJsonString(),//请求报文
                        Str2 = item.message   //返回结果
                    });
                }
                new LogOperationService().AddLogOperation(logs);
            }
            catch (Exception ex)
            {
                logs.Add(new WMS_Log_Operation()
                {
                    MenuName = "出库单管理",
                    Operation = "出库单-推送鲸仓",
                    OrderType = "OrderSend",
                    Controller = Request.RawUrl,
                    Creator = base.UserInfo.Name,
                    CreateTime = DateTime.Now,
                    ProjectID = (int)base.UserInfo.ProjectID,
                    ProjectName = base.UserInfo.ProjectName,
                    OrderNumber = "",
                    Str1 = JCRequestList,
                    Str2 = ex.Message
                });
                new LogOperationService().AddLogOperation(logs);
            }
            return Json(new { Result = listResponses });
        }

        /// <summary>
        /// 出库单出库确认推送鲸仓
        /// </summary>
        /// <param name="IDS"></param>
        /// <returns></returns>
        public JsonResult OrderOutConfirm(string JCRequestList)
        {
            IList<JCAPiResponse> listResponses = new List<JCAPiResponse>();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            try
            {
                string url = UtilConstants.JCSendAPIAddress + "OrderOutConfirm";
                List<JCRequestLists> list = jsonlist<JCRequestLists>(JCRequestList.Substring(18, JCRequestList.Length - 19));
                string res = this.HTTPPost(url, JCRequestList);
                listResponses = jsonlist<JCAPiResponse>(res);
                foreach (var item in listResponses)
                {
                    logs.Add(new WMS_Log_Operation()
                    {
                        MenuName = "出库单管理",
                        Operation = "出库单-出库确认推送鲸仓",
                        OrderType = "OrderOutConfirm",
                        Controller = Request.RawUrl,
                        Creator = base.UserInfo.Name,
                        CreateTime = DateTime.Now,
                        ProjectID = (int)base.UserInfo.ProjectID,
                        ProjectName = base.UserInfo.ProjectName,
                        OrderNumber = item.relatednumber,
                        Str1 = list.Where(c => c.RelateNumber == item.relatednumber).ToJsonString(),//请求报文
                        Str2 = item.message   //返回结果
                    });
                }
                new LogOperationService().AddLogOperation(logs);
            }
            catch (Exception ex)
            {
                logs.Add(new WMS_Log_Operation()
                {
                    MenuName = "出库单管理",
                    Operation = "出库单-出库确认推送鲸仓",
                    OrderType = "OrderOutConfirm",
                    Controller = Request.RawUrl,
                    Creator = base.UserInfo.Name,
                    CreateTime = DateTime.Now,
                    ProjectID = (int)base.UserInfo.ProjectID,
                    ProjectName = base.UserInfo.ProjectName,
                    OrderNumber = "",
                    Str1 = JCRequestList,
                    Str2 = ex.Message
                });
                new LogOperationService().AddLogOperation(logs);
            }
            return Json(new { Result = listResponses });
        }

        /// <summary>
        /// POST提交
        /// </summary>
        private string HTTPPost(string url, string request)
        {
            string responseStr = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = @"POST";
            req.ContentType = "application/json;charset=UTF-8";

            //req.ContentType = "application/x-www-form-urlencoded";
            //req.ContentType = "text/xml";
            if (!string.IsNullOrEmpty(request))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(request);
                req.ContentLength = bytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }
            WebResponse wr;// = req.GetResponse();



            try
            {
                wr = req.GetResponse();
            }
            catch (WebException ex)
            {
                wr = ex.Response;
                throw ex;
            }

            Stream responseStream = wr.GetResponseStream();
            using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                responseStr = reader.ReadToEnd();
            }

            return responseStr;
        }

        /// <summary>
        /// 下发拣货任务
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string OrderTask(string ID)
        {
            OrderManagementService service = new OrderManagementService();
            List<WMS_Task> tasks = new List<WMS_Task>();
            string response = "";
            try
            {
                response = service.OrderTask(ID, base.UserInfo.Name).Result;
                var responseDetail = service.GetOrderDetailByIDS(ID);

                foreach (var item in responseDetail.Result.OrderDetailCollection.GroupBy(c => c.OrderNumber).Select(a => a.Key))
                {
                    List<OrderDetailForRedisRF> orderDetailInfos = new List<OrderDetailForRedisRF>();
                    foreach (var itemdetail in responseDetail.Result.OrderDetailCollection)
                    {
                        if (item == itemdetail.OrderNumber)
                        {
                            orderDetailInfos.Add(new OrderDetailForRedisRF()
                            {
                                ID = itemdetail.ID,
                                LineNumber = itemdetail.LineNumber,
                                GoodsType = itemdetail.GoodsType,
                                Area = itemdetail.Area,
                                Location = itemdetail.Location,
                                OrderNumber = itemdetail.OrderNumber,
                                SKU = itemdetail.SKU,
                                UPC = itemdetail.UPC,
                                BatchNumber = itemdetail.BatchNumber,
                                Unit = itemdetail.Unit,
                                Specifications = itemdetail.Specifications,
                                Qty = itemdetail.Qty,
                                QtyPicked = 0
                            });
                        }
                    }
                    RedisOperation.Del(item);
                    RedisOperation.SetList(item, orderDetailInfos);
                }
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        ///// <summary>
        ///// 检查拣货差异
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public JsonResult CheckDiff(string id)
        //{
        //    List<OrderDetailForRedisRF> response;
        //    try
        //    {
        //        response = new OrderManagementService().CheckDiff(id);

        //        if (response != null)
        //        {
        //            //return response.ToJsonString();
        //            return Json(new { Code = 1, data = response });
        //        }
        //        else
        //        {
        //            return Json(new { Code = 0 });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { Code = 2 });
        //    }

        //}
        /// <summary>
        /// 检查包装差异
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CheckDiff(string id)
        {
            List<OrderDetailForRedisRF> response;
            try
            {
                response = new OrderManagementService().CheckDiff(id);

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

        /// <summary>
        /// 导出包装差异
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void CheckDiffBatch(string ids)
        {
            List<OrderDetailForRedisRF> response;
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn();
            dc = dt.Columns.Add("单号", typeof(string));
            dc = dt.Columns.Add("产品编码", typeof(string));
            dc = dt.Columns.Add("差异数量", typeof(string));
            IList<OrderBackStatus> Orders = new List<OrderBackStatus>();
            try
            {

                if (ids.Contains(","))
                {
                    string[] strs = ids.Split(',');
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
                        ID = ids.ObjectToInt64(),
                        UpdateTime = DateTime.Now,
                        Updator = base.UserInfo.Name
                    });
                }

                response = new OrderManagementService().CheckDiffBatch(Orders);

                if (response != null)
                {
                    foreach (var item in response.ToList())
                    {
                        DataRow dr = dt.NewRow();
                        dr["单号"] = item.OrderNumber;
                        dr["产品编码"] = item.SKU;
                        dr["差异数量"] = item.QtyPicked - item.Qty;
                        dt.Rows.Add(dr);

                    }
                    ExportDataToExcelHelper.ExportExcel(dt, DateTime.Now.ToString("yyyy-MM-dd") + "_出库差异", "差异信息");
                }

            }
            catch (Exception)
            {

            }

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

        [HttpPost]   //出库单管理
        public ActionResult Index(OrderViewModel vm, int? PageIndex, string Action)
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
            ViewBag.CustomerID = vm.SearchCondition.CustomerID == null ? base.UserInfo.CustomerID : vm.SearchCondition.CustomerID;
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
            var WorkStation = ApplicationConfigHelper.GetCacheInfo().Where(a => a.WarehouseName == vm.SearchCondition.Warehouse).GroupBy(a => new { a.OperationAreaID, a.Operation }).Select(c => new SelectListItem() { Value = c.Key.OperationAreaID.ToString(), Text = c.Key.Operation });

            ViewBag.WorkStation = WorkStation;
            ViewBag.WarehouseList = WarehouseList;
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            var getOrderByConditionRequest = new GetOrderByConditionRequest();
            getOrderByConditionRequest.SearchCondition = vm.SearchCondition;
            getOrderByConditionRequest.SearchCondition.Model = "物料";
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
            if (Action == "下载模板")
            {
                IEnumerable<Column> columnReceipt;

                var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Order_Pack").Count() == 0)
                {
                    columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(null, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Warehouse_Location").ColumnCollection.Where(c => c.IsImportColumn == true);
                }
                else
                {
                    columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_Order_Pack").ColumnCollection.Where(c => c.IsImportColumn == true);
                }
                DataSet ds = new DataSet();
                DataTable dtReceipt = new DataTable();
                foreach (var receipt in columnReceipt)
                {
                    dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
                }
                dtReceipt.TableName = "包装信息导入模板";
                ds.Tables.Add(dtReceipt);
                //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "包装信息导入模板");
                EPPlusOperation.ExportDataSetByEPPlus(ds, "包装信息导入模板");

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



            //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
            //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
            //IEnumerable<Table> tables = module.Tables.TableCollection;
            //IEnumerable<Column> columnOrder = tables.First(t => t.Name == "WMS_Order").ColumnCollection;
            //IEnumerable<Column> columnOrderDetail = tables.First(t => t.Name == "WMS_OrderDetail").ColumnCollection;
            if (CustomerID == 0)
            {
                columnOrder = columnOrder.Where(c => c.ForView == true);
                columnOrderDetail = columnOrderDetail.Where(c => c.ForView == true);
            }
            else
            {
                columnOrder = columnOrder.Where(c => c.ForView == true);
                columnOrderDetail = columnOrderDetail.Where(c => c.ForView == true);
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
            string typename = "";
            if (type == "Pick")
                typename = "拣货";
            else if (type == "Confirm")
                typename = "复检";
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
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

                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "出库单管理";
                    operation.Operation = "出库单-" + typename;
                    operation.OrderType = "Order";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = base.UserInfo.Name;
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = (int)base.UserInfo.ProjectID;
                    operation.ProjectName = base.UserInfo.ProjectName;
                    operation.OrderID = str;
                    logs.Add(operation);
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
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "出库单管理";
                operation.Operation = "出库单-" + typename;
                operation.OrderType = "Order";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = ID;
                logs.Add(operation);
            }
            var GetPickResult = new OrderManagementService().Pick(Orders, type, base.UserInfo.Name);
            if (type == "Pick")
            {
                new PickingService().CreatePickingAndDetail(base.UserInfo.Name, ID);
            }
            new LogOperationService().AddLogOperation(logs);
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
                    var FirstBox = GetMaxBoxnumber(vm.OrderCollection.Select(m => m.ID).FirstOrDefault().ToString());
                    if (string.IsNullOrWhiteSpace(FirstBox))
                    {
                        Response.Write("<script>alert('箱号获取失败，请重试！');window.location.href='/WMS/OrderManagement/Index'</script>");
                        //Page.RegisterStartupScript("msg", "<script>alert('查询语句执行出错！');window.location.href='DisplayData.aspx'</script>");
                        //ClientScript.RegisterStartupScript(this.GetType(), "", " <script lanuage=javascript> alert('');location.href='';</script>");System.Web.HttpContext.Current.Response.Write(String.Format("<script language=\"javascript\">alert(\"{0}\");window.location.replace(\"{1}\")</script>", strMessage, strRedirectUrl));
                        //System.Web.HttpContext.Current.Response.Write(String.Format("<script language=\"javascript\">alert(\"{0}\");window.location.replace(\"{1}\")</script>", strMessage, strRedirectUrl));
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "u1", "alert('内容！')", true);
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "d", "alert('请先登录！');location='../login.aspx';", true);
                    }
                    else
                    {
                        ViewBag.FirstBox = FirstBox;
                    }
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

        public ActionResult NikeTHPackage(long ID)
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
                    var FirstBox = GetMaxBoxnumber(vm.OrderCollection.Select(m => m.ID).FirstOrDefault().ToString());
                    if (string.IsNullOrWhiteSpace(FirstBox))
                    {
                        Response.Write("<script>alert('箱号获取失败，请重试！');window.location.href='/WMS/OrderManagement/Index'</script>");
                        //Page.RegisterStartupScript("msg", "<script>alert('查询语句执行出错！');window.location.href='DisplayData.aspx'</script>");
                        //ClientScript.RegisterStartupScript(this.GetType(), "", " <script lanuage=javascript> alert('');location.href='';</script>");System.Web.HttpContext.Current.Response.Write(String.Format("<script language=\"javascript\">alert(\"{0}\");window.location.replace(\"{1}\")</script>", strMessage, strRedirectUrl));
                        //System.Web.HttpContext.Current.Response.Write(String.Format("<script language=\"javascript\">alert(\"{0}\");window.location.replace(\"{1}\")</script>", strMessage, strRedirectUrl));
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "u1", "alert('内容！')", true);
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "d", "alert('请先登录！');location='../login.aspx';", true);
                    }
                    else
                    {
                        ViewBag.FirstBox = FirstBox;
                    }
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

        public ActionResult JitePackage(long ID)
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
            var box = ApplicationConfigHelper.GetApplicationBox(base.UserInfo.ProjectName).Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name });
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
                    var FirstBox = GetMaxBoxnumber(vm.OrderCollection.Select(m => m.ID).FirstOrDefault().ToString());
                    if (string.IsNullOrWhiteSpace(FirstBox))
                    {
                        Response.Write("<script>alert('箱号获取失败，请重试！');window.location.href='/WMS/OrderManagement/Index'</script>");
                        //Page.RegisterStartupScript("msg", "<script>alert('查询语句执行出错！');window.location.href='DisplayData.aspx'</script>");
                        //ClientScript.RegisterStartupScript(this.GetType(), "", " <script lanuage=javascript> alert('');location.href='';</script>");System.Web.HttpContext.Current.Response.Write(String.Format("<script language=\"javascript\">alert(\"{0}\");window.location.replace(\"{1}\")</script>", strMessage, strRedirectUrl));
                        //System.Web.HttpContext.Current.Response.Write(String.Format("<script language=\"javascript\">alert(\"{0}\");window.location.replace(\"{1}\")</script>", strMessage, strRedirectUrl));
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "u1", "alert('内容！')", true);
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "d", "alert('请先登录！');location='../login.aspx';", true);
                    }
                    else
                    {
                        ViewBag.FirstBox = FirstBox;
                    }
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
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
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

                //WMS_Log_Operation operation = new WMS_Log_Operation();
                //operation.MenuName = "出库单管理";
                //operation.Operation = "出库单-包装";
                //operation.OrderType = "Order";
                //operation.Controller = Request.RawUrl;
                //operation.Creator = base.UserInfo.Name;
                //operation.CreateTime = DateTime.Now;
                //operation.ProjectID = (int)base.UserInfo.ProjectID;
                //operation.ProjectName = base.UserInfo.ProjectName;
                //operation.OrderID = ID.ToString();
                //operation.Str1 = package.PackageNumber;
                //logs.Add(operation);
            });
            request.packages = packages;
            request.packageDetails = packageDetails;
            var response = new OrderManagementService().AddPackageAndDetail(ID, request, flag);
            WMS_Log_Operation operation = new WMS_Log_Operation();
            operation.MenuName = "出库单管理";
            operation.Operation = "出库单-包装";
            operation.OrderType = "Order";
            operation.Controller = Request.RawUrl;
            operation.Creator = base.UserInfo.Name;
            operation.CreateTime = DateTime.Now;
            operation.ProjectID = (int)base.UserInfo.ProjectID;
            operation.ProjectName = base.UserInfo.ProjectName;
            operation.OrderID = ID.ToString();
            //operation.Str1 = package.PackageNumber;//每单包装只记录一次日志
            logs.Add(operation);
            new LogOperationService().AddLogOperation(logs);
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
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
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
            WMS_Log_Operation operation = new WMS_Log_Operation();
            operation.MenuName = "出库单管理";
            operation.Operation = "出库单-包装";
            operation.OrderType = "Order";
            operation.Controller = Request.RawUrl;
            operation.Creator = base.UserInfo.Name;
            operation.CreateTime = DateTime.Now;
            operation.ProjectID = (int)base.UserInfo.ProjectID;
            operation.ProjectName = base.UserInfo.ProjectName;
            operation.OrderID = ID.ToString();
            //operation.Str1 = package.PackageNumber;
            logs.Add(operation);
            new LogOperationService().AddLogOperation(logs);
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
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            List<WMS_Cord_Operation> cords = new List<WMS_Cord_Operation>();
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
            WMS_Log_Operation operation = new WMS_Log_Operation();
            operation.MenuName = "出库单管理";
            operation.Operation = "出库单-包装";
            operation.OrderType = "Order";
            operation.Controller = Request.RawUrl;
            operation.Creator = base.UserInfo.Name;
            operation.CreateTime = DateTime.Now;
            operation.ProjectID = (int)base.UserInfo.ProjectID;
            operation.ProjectName = base.UserInfo.ProjectName;
            operation.OrderID = ID.ToString();
            //operation.Str1 = package.PackageNumber;
            logs.Add(operation);
            WMS_Cord_Operation cord = new WMS_Cord_Operation()
            {
                MenuName = "出库单管理",
                Operation = "出库单-包装",
                OrderType = "Order",
                Controller = Request.RawUrl,
                Creator = base.UserInfo.Name,
                CreateTime = DateTime.Now,
                ProjectID = (int)base.UserInfo.ProjectID,
                ProjectName = base.UserInfo.ProjectName,
                OrderID = ID.ToString()
            };
            cords.Add(cord);
            new LogOperationService().AddLogOperation(logs);
            //Cord By Young
            //Task.Run(() => CordHelper.AddWMS_Cord_FeedBack(cords));
            return response.Result;
        }

        [HttpPost]
        public string NikeTHPackage(long ID, string JsonPackage, int flag)
        {
            var responseJsonFieldsets = jsonlist<PackageDetailInfo>(JsonPackage);
            DateTime d = DateTime.Now;
            AddPackageAndDetailRequest request = new AddPackageAndDetailRequest();
            IList<PackageInfo> packages = new List<PackageInfo>();
            IList<PackageDetailInfo> packageDetails = new List<PackageDetailInfo>();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            List<WMS_Cord_Operation> cords = new List<WMS_Cord_Operation>();
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
            WMS_Log_Operation operation = new WMS_Log_Operation();
            operation.MenuName = "出库单管理";
            operation.Operation = "出库单-包装";
            operation.OrderType = "Order";
            operation.Controller = Request.RawUrl;
            operation.Creator = base.UserInfo.Name;
            operation.CreateTime = DateTime.Now;
            operation.ProjectID = (int)base.UserInfo.ProjectID;
            operation.ProjectName = base.UserInfo.ProjectName;
            operation.OrderID = ID.ToString();
            //operation.Str1 = package.PackageNumber;
            logs.Add(operation);
            WMS_Cord_Operation cord = new WMS_Cord_Operation()
            {
                MenuName = "出库单管理",
                Operation = "出库单-包装",
                OrderType = "Order",
                Controller = Request.RawUrl,
                Creator = base.UserInfo.Name,
                CreateTime = DateTime.Now,
                ProjectID = (int)base.UserInfo.ProjectID,
                ProjectName = base.UserInfo.ProjectName,
                OrderID = ID.ToString()
            };
            cords.Add(cord);
            new LogOperationService().AddLogOperation(logs);
            //Cord By Young
            //Task.Run(() => CordHelper.AddWMS_Cord_FeedBack(cords));
            return response.Result;
        }

        [HttpPost]
        public string OrderBackStatus(string ID, int ToStatus, int type, string username, string password)
        {
            GetOrderByConditionRequest request = new GetOrderByConditionRequest();
            IList<OrderBackStatus> Orders = new List<OrderBackStatus>();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            //前台判断未必准确，再加一步判断当前勾选的订单中是否存在9状态的订单，不允许回退                
            IEnumerable<OrderInfo> orderInfos = new OrderManagementService().GetOrderInfosByIDs(ID);
            if (orderInfos == null || !orderInfos.Any())
            {
                return "未找到需要回退的订单，请检查后重试！";
            }
            if (orderInfos.Where(m => m.Status == 9).Count() > 0)
            {
                return "存在已出库的订单，不允许回退，请检查后重试！";
            }
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
                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "出库单管理";
                    operation.Operation = "出库单-状态回退";
                    operation.OrderType = "Order";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = base.UserInfo.Name;
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = (int)base.UserInfo.ProjectID;
                    operation.ProjectName = base.UserInfo.ProjectName;
                    operation.OrderID = str;
                    operation.Remark = ToStatus.ToString();
                    logs.Add(operation);
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
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "出库单管理";
                operation.Operation = "出库单-状态回退";
                operation.OrderType = "Order";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = ID;
                operation.Remark = ToStatus.ToString();
                logs.Add(operation);
            }
            request.Orders = Orders;

            new LogOperationService().AddLogOperation(logs);//csc对接，故采取先加日志方式，删除增量库存
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

        [HttpPost]
        public string ImputEcecl(string CustomerName, long CustomerID)
        {
            PreOrderViewModel vm = new PreOrderViewModel();
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

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        OrderKey = (from a in ds.Tables[0].AsEnumerable() where a.Field<string>("订单号") != "" && a.Field<string>("订单号") != null select a.Field<string>("订单号")).Distinct().ToList<string>();
                    }
                    else
                    {
                        return new { result = "<h3><font color='#FF0000'>模版中无数据</font></h3>", IsSuccess = false }.ToJsonString();
                    }
                    var response = new ASNManagementService().OrderKeyCheck(OrderKey[0]);
                    if (response.IsSuccess)
                    {
                        if (response.Result.OrderCollection.Count() <= 0)
                        {
                            return new { result = "<h3><font color='#FF0000'>订单号不存在:" + OrderKey.ToString() + "</font></h3>", IsSuccess = false }.ToJsonString();
                        }
                        //验证订单号是否存在 
                        var OrderCollection = response.Result.OrderCollection;
                        // var retrunorderkey = response.Result.OrderCollection.GroupBy(c => c.OrderNumber).Each((i, v) => OrderKey.Remove(v.First().OrderNumber));
                        //验证SKU 差异数量
                        DataTable dtpodetail = ds.Tables[0];
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < dtpodetail.Rows.Count; i++)
                        {
                            string SKU = dtpodetail.Rows[i]["SKU"].ToString().Trim();
                            var OrderQty = OrderCollection.Where(a => a.str1 == SKU).Select(b => b.Int1).FirstOrDefault();
                            //空白行
                            if (SKU != "")
                            {
                                #region 已屏蔽
                                //原逻辑有问题  同一个sku被拆分到两个箱子 会提示都缺少SKU
                                //if (OrderCollection.Where(a => a.str1 == SKU).Count() > 0 && SKU != "")
                                //{
                                //    int DifferenceQty = Convert.ToInt32(OrderQty) - Convert.ToInt32(dtpodetail.Rows[i]["QTY"]);
                                //    if (OrderCollection.Where(a => a.str1 == SKU && a.Int1 == Convert.ToInt32(dtpodetail.Rows[i]["QTY"])).Count() <= 0)
                                //    {
                                //        sb.Append("SKU:" + SKU + ",差异" + DifferenceQty + "</br>");
                                //    }
                                //}
                                //else
                                //{
                                //    sb.Append("订单不存在该SKU:" + SKU + "</br>");
                                //}
                                #endregion
                                //正确逻辑 先得到该sku所有包装数量 再得到该SKU所有订单数量  进行比较
                                //先比较SKU是否在订单
                                if (OrderCollection.Where(a => a.str1 == SKU).Count() < 0)
                                {
                                    sb.Append("订单不存在该产品编码:" + SKU + "</br>");
                                }
                                else
                                {
                                    //包装总数
                                    int boxqty = 0;
                                    foreach (DataRow item in dtpodetail.Rows)
                                    {
                                        try
                                        {
                                            if (item["SKU"].ToString().Trim() == SKU)
                                                boxqty += int.Parse(item["QTY"].ToString());
                                        }
                                        catch
                                        {
                                            sb.Append("产品编码:" + SKU + "的数量格式有误！</br>");
                                        }
                                    }
                                    //订单数量
                                    int orderqty = OrderCollection.Where(s => s.str1 == SKU).Sum(b => b.Int1.Value);
                                    int DifferenceQty = orderqty - boxqty;
                                    if (DifferenceQty != 0)
                                    {
                                        sb.Append("产品编码:" + SKU + ",差异" + DifferenceQty + "</br>");
                                    }
                                }
                            }
                        }

                        if (sb.ToString() != "" && sb != null)
                        {
                            return new { result = "<h3><font color='#FF0000'>存在差异信息，如下：" + sb.ToString() + "</font></h3>", IsSuccess = false }.ToJsonString();
                        }
                        else
                        {
                            WarehouseInfo warehouse = ApplicationConfigHelper.GetWarehouseListByCustomer(CustomerID).Where(p => p.ID == ApplicationConfigHelper.GetCacheInfo().First(c => c.UserName == base.UserInfo.Name).WarehouseID).ToArray()[0];//

                            List<PackageInfo> package = new List<PackageInfo>();

                            IEnumerable<BoxSize> listboxs = ApplicationConfigHelper.GetApplicationBox(this.UserInfo.ProjectName.ToString());
                            foreach (DataRow item in dtpodetail.Rows)
                            {
                                //excel空白行
                                if (item["订单号"].ToString() != "")
                                {
                                    PackageInfo pack = new PackageInfo();
                                    pack.OrderNumber = OrderKey[0];
                                    pack.str1 = item["箱号"].ToString();
                                    pack.str2 = item["产品编码"].ToString();
                                    pack.Int1 = Convert.ToInt32(item["Qty"].ToString());
                                    if (dtpodetail.Columns.Contains("箱型"))
                                    {
                                        if (listboxs.Where(m => m.Name == item["箱型"].ToString()).Count() <= 0)
                                        {
                                            return new { result = "<h3>箱型: '" + item["箱型"].ToString() + "' 在系统中不存在！</h3>", IsSuccess = false }.ToJsonString();
                                        }
                                        else
                                        {
                                            pack.PackageType = listboxs.Where(m => m.Name == item["箱型"].ToString()).FirstOrDefault().Code;
                                            pack.Length = pack.PackageType.Split(',')[0];
                                            pack.Width = pack.PackageType.Split(',')[1];
                                            pack.Height = pack.PackageType.Split(',')[2];
                                        }
                                    }
                                    if (dtpodetail.Columns.Contains("重量"))
                                    {
                                        decimal boxweight;
                                        if (decimal.TryParse(item["重量"].ToString(), out boxweight))
                                        {
                                            pack.NetWeight = item["重量"].ToString();
                                        }
                                        else
                                        {
                                            return new { result = "<h3>重量: '" + item["重量"].ToString() + "' 不是数字，格式有误！</h3>", IsSuccess = false }.ToJsonString();
                                        }
                                    }
                                    package.Add(pack);
                                }

                            }
                            //同一箱不允许出现箱型和重量不一致的数据
                            if (dtpodetail.Columns.Contains("箱型") && dtpodetail.Columns.Contains("重量"))
                            {
                                var ddd = from t in package.AsEnumerable()
                                          group t by new { t1 = t.OrderNumber, t4 = t.str1 } into m
                                          select new
                                          {
                                              订单号 = m.Select(p => p.OrderNumber).First(),

                                              箱号 = m.Select(p => p.str1).First(),
                                              count = m.Count(),
                                          };
                                var drr = ddd.Where(d => d.count > 1);
                                if (drr.Count() > 0)
                                {
                                    foreach (var item in drr)
                                    {
                                        var a = package.Where(p => p.str1 == item.箱号);
                                        var ddd2 = from b in a.AsEnumerable()
                                                   group b by new { b1 = b.PackageType, b2 = b.NetWeight } into c
                                                   select new
                                                   {
                                                       箱型 = c.Select(d => d.PackageType).First(),
                                                       重量 = c.Select(d => d.NetWeight).First(),

                                                   };
                                        if (ddd2.Count() > 1)
                                        {
                                            foreach (var item2 in ddd2)
                                            {
                                                return new { result = "<h3>Excel中箱号为 " + item.箱号 + "的行箱号、箱型、重量不一致，请检查！</h3>", IsSuccess = false }.ToJsonString();
                                            }
                                        }
                                    }
                                }
                            }
                            AddPackageAndDetailRequest request = new AddPackageAndDetailRequest();
                            request.packages = package;

                            //导入时调用不同的存储过程
                            IEnumerable<WMSConfig> wms = null;
                            try
                            {
                                wms = ApplicationConfigHelper.GetWMS_Config("InsertPackageDetail_" + base.UserInfo.ProjectName);
                            }
                            catch
                            {

                            }
                            if (wms == null)
                            {
                                wms = ApplicationConfigHelper.GetWMS_Config("InsertPackageDetail");
                            }

                            var respones = new ASNManagementService().ImportPackageInfo(request, wms.FirstOrDefault().Name);
                            if (respones.IsSuccess)
                            {
                                return new { result = "<h3>导入成功</h3>", IsSuccess = true }.ToJsonString();
                            }
                            else
                            {
                                return new { result = "<h3>导入失败,异常信息:" + respones.SuccessMessage.ToString() + "</h3>", IsSuccess = false }.ToJsonString();
                            }
                        }
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
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            List<WMS_Cord_Operation> cords = new List<WMS_Cord_Operation>();
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
                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "出库单管理";
                    operation.Operation = "出库单-出库";
                    operation.OrderType = "Order";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = base.UserInfo.Name;
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = (int)base.UserInfo.ProjectID;
                    operation.ProjectName = base.UserInfo.ProjectName;
                    operation.OrderID = str;
                    logs.Add(operation);
                    WMS_Cord_Operation cord = new WMS_Cord_Operation()
                    {
                        MenuName = "出库单管理",
                        Operation = "出库单-出库",
                        OrderType = "Order",
                        Controller = Request.RawUrl,
                        Creator = base.UserInfo.Name,
                        CreateTime = DateTime.Now,
                        ProjectID = (int)base.UserInfo.ProjectID,
                        ProjectName = base.UserInfo.ProjectName,
                        OrderID = str
                    };
                    cords.Add(cord);
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
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "出库单管理";
                operation.Operation = "出库单-出库";
                operation.OrderType = "Order";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = ID;
                logs.Add(operation);
                WMS_Cord_Operation cord = new WMS_Cord_Operation()
                {
                    MenuName = "出库单管理",
                    Operation = "出库单-出库",
                    OrderType = "Order",
                    Controller = Request.RawUrl,
                    Creator = base.UserInfo.Name,
                    CreateTime = DateTime.Now,
                    ProjectID = (int)base.UserInfo.ProjectID,
                    ProjectName = base.UserInfo.ProjectName,
                    OrderID = ID
                };
                cords.Add(cord);
            }
            var GetOutResult = new OrderManagementService().Outs(Orders);
            if (GetOutResult.Result.Equals(""))
            {
                new LogOperationService().AddLogOperation(logs);
                //Cord By Young
                //Task.Run(() => CordHelper.AddWMS_Cord_FeedBack(cords));
            }
            return GetOutResult.Result;
        }

        /// <summary>
        /// 差异出库
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string OutsWithDiff(string ID)
        {
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
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
                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "出库单管理";
                    operation.Operation = "出库单-出库";
                    operation.OrderType = "Order";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = base.UserInfo.Name;
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = (int)base.UserInfo.ProjectID;
                    operation.ProjectName = base.UserInfo.ProjectName;
                    operation.OrderID = str;
                    logs.Add(operation);
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
                WMS_Log_Operation operation = new WMS_Log_Operation();
                operation.MenuName = "出库单管理";
                operation.Operation = "出库单-出库";
                operation.OrderType = "Order";
                operation.Controller = Request.RawUrl;
                operation.Creator = base.UserInfo.Name;
                operation.CreateTime = DateTime.Now;
                operation.ProjectID = (int)base.UserInfo.ProjectID;
                operation.ProjectName = base.UserInfo.ProjectName;
                operation.OrderID = ID;
                logs.Add(operation);
            }
            var GetOutResult = new OrderManagementService().OutsWithDiff(Orders);
            if (GetOutResult.Result.Equals(""))
            {
                new LogOperationService().AddLogOperation(logs);
            }
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

        /// <summary>
        /// 拣货单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
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

        [HttpGet]    //拣货单打印
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


            //获取打印的id
            var IDlist = "";
            foreach (var item in vm.OrderCollection)
            {
                IDlist += item.ID.ToString() + ',';
            }
            ViewBag.IDs = IDlist.Substring(0, IDlist.Length - 1);
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);

        }

        //[HttpGet]    //nike/拣货单打印B2c订单
        //public ActionResult PrintOrderNikeB2C(string id)
        //{

        //    OrderViewModel vm = new OrderViewModel();

        //    ViewBag.Id = id;

        //    var response = new OrderManagementService().GetPrintOrderNikeB2C(id);

        //    vm.OrderCollection = response.Result.OrderCollection;
        //    vm.OrderDetailCollection = response.Result.OrderDetailCollection;

        //    //获取打印的id
        //    var IDlist = "";
        //    foreach (var item in vm.OrderCollection)
        //    {
        //        IDlist += item.ID.ToString() + ',';
        //    }
        //    ViewBag.IDs = IDlist.Substring(0, IDlist.Length - 1);
        //    #region 页面customerid读取
        //    IEnumerable<WMS_Config_Type> ctype = null;
        //    ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
        //    ViewBag.ctype = ctype;
        //    #endregion
        //    return View(vm);

        //}
        /// <summary>
        /// nikeB2C线上支付拣货单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PrintOrderNikeB2CSFS(string id)
        {

            OrderViewModel vm = new OrderViewModel();

            ViewBag.Id = id;

            var response = new OrderManagementService().GetPrintOrderNikeB2C(id);

            vm.OrderCollection = response.Result.OrderCollection;
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;

            //获取打印的id
            var IDlist = "";
            foreach (var item in vm.OrderCollection)
            {
                IDlist += item.ID.ToString() + ',';
            }
            ViewBag.IDs = IDlist.Substring(0, IDlist.Length - 1);
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);

        }
        /// <summary>
        /// NIKEB2c门店支付拣货单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PrintOrderNikeB2CSTH(string id)
        {

            OrderViewModel vm = new OrderViewModel();

            ViewBag.Id = id;

            var response = new OrderManagementService().GetPrintOrderNikeB2C(id);

            vm.OrderCollection = response.Result.OrderCollection;
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;

            //获取打印的id
            var IDlist = "";
            foreach (var item in vm.OrderCollection)
            {
                IDlist += item.ID.ToString() + ',';
            }
            ViewBag.IDs = IDlist.Substring(0, IDlist.Length - 1);
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);

        }

        /// <summary>
        /// 打印快递面单 nike
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PrintExpressNike(string id)
        {

            PrintExpressModel model = new PrintExpressModel();
            model.OrderInfos = new OrderManagementService().GetPrintExpressNike(id);
            return View(model);
        }

        /// <summary>
        /// 德邦快递面单打印
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintExpressDeppon(string id)
        {
            StringBuilder s = new StringBuilder();
            PrintExpressModel model = new PrintExpressModel();
            model.EnumerableExpressInfo = new OrderManagementService().GetPrintExpressDeppon(id);
            return View(model);
        }

        /// <summary>
        /// 韵达快递面单打印
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PrintExpressYd(string id)
        {
            PrintExpressModel vm = new PrintExpressModel();
            OrderManagementService service = new OrderManagementService();
            vm.YdExpressInfo = service.GetPrintExpressYd(id);
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

            if (base.UserInfo.CustomerID == 87)
                Flag = 4;
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

            //获取打印的id
            var IDlist = "";
            foreach (var item in vm.OrderCollection)
            {
                IDlist += item.ID.ToString() + ',';
            }
            ViewBag.IDs = IDlist.Substring(0, IDlist.Length - 1);

            return View(vm);

        }

        [HttpGet]
        public ActionResult PrintOrderAkzo_TJ(string id, int Flag)
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

            if (base.UserInfo.CustomerID == 87)
                Flag = 4;
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

            //获取打印的id
            var IDlist = "";
            foreach (var item in vm.OrderCollection)
            {
                IDlist += item.ID.ToString() + ',';
            }
            ViewBag.IDs = IDlist.Substring(0, IDlist.Length - 1);

            return View(vm);

        }

        [HttpGet] //批量汇总打印
        public ActionResult PrintOrderSumAkzo(string id, int Flag)
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

            if (base.UserInfo.CustomerID == 87)
                Flag = 4;
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

            //获取打印的id
            var IDlist = "";
            foreach (var item in vm.OrderCollection)
            {
                IDlist += item.ID.ToString() + ',';
            }
            ViewBag.IDs = IDlist.Substring(0, IDlist.Length - 1);

            return View(vm);
        }

        [HttpGet] //批量汇总打印（退货仓）
        public ActionResult PrintOrderSum_TH(string id, int Flag)
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

            if (base.UserInfo.CustomerID == 87)
                Flag = 4;
            var response = new OrderManagementService().GetPrintOrderAkzo_TH(id, Flag);

            vm.OrderCollection = response.Result.OrderCollection;
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));

            //vm.OrderCollection.Each((a, b) =>
            //{
            //    string strGUID = "Order" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.OrderNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});

            //获取打印的id
            var IDlist = "";
            foreach (var item in vm.OrderCollection)
            {
                IDlist += item.ID.ToString() + ',';
            }
            ViewBag.IDs = IDlist.Substring(0, IDlist.Length - 1);

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

        /// <summary>
        /// HABA拣货单打印
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PrintOrderHABA(string id, int Flag)
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

            var response = new OrderManagementService().GetPrintOrderHABA(id, Flag);


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

        [HttpGet]    //西安吉特拣货单打印
        public ActionResult PrintOrder_JT(string id, int Flag)
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

            var response = new OrderManagementService().GetPrintOrder_JT(id, Flag);

            vm.OrderCollection = response.Result.OrderCollection;
            vm.OrderDetailCollection = response.Result.OrderDetailCollection;
            //deleteTmpFiles(Server.MapPath("~/TotalImage/"));

            //vm.OrderCollection.Each((a, b) =>
            //{
            //    string strGUID = "Order" + Guid.NewGuid().ToString();
            //    b.PictureStr = GetDimensionalCode(b.OrderNumber, strGUID + ".jpg");
            //    b.PageIndex = "page" + (a + 1);
            //});


            //获取打印的id
            var IDlist = "";
            foreach (var item in vm.OrderCollection)
            {
                IDlist += item.ID.ToString() + ',';
            }
            ViewBag.IDs = IDlist.Substring(0, IDlist.Length - 1);
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);

        }

        [HttpGet]    //Mono拣货单打印
        public ActionResult PrintOrder_Mono(string id, int Flag)
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


            //获取打印的id
            var IDlist = "";
            foreach (var item in vm.OrderCollection)
            {
                IDlist += item.ID.ToString() + ',';
            }
            ViewBag.IDs = IDlist.Substring(0, IDlist.Length - 1);
            #region 页面customerid读取
            IEnumerable<WMS_Config_Type> ctype = null;
            ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = ctype;
            #endregion
            return View(vm);

        }

        /// <summary>
        /// 出库单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PrintOutOrder_Bridge(string id, int Flag)
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

        public ActionResult PrintAllPod(string ids)
        {
            PrintPodModel print = new PrintPodModel();
            print.EnumerableCustomerInfo = new OrderManagementService().GetPrintAllPodCondition(ids).EnumerableBoxListinfo;

            return View(print);
        }

        /// <summary>
        /// 批量汇总打印托运单 NIKE用
        /// </summary>
        /// <param name="ids">多个ID</param>
        /// <returns></returns>
        public ActionResult PrintSumAllPod(string ids, long customerID, string warehouseName, string searchTime)
        {
            PrintPodModel print = new PrintPodModel();

            print.EnumerableCustomerInfo = new OrderManagementService().GetPrintSumAllPodCondition(ids, customerID, warehouseName, searchTime).EnumerableBoxListinfo;

            return View(print);
        }

        /// <summary>
        /// 退货仓批量打印托运单(出库单页面)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public ActionResult PrintAllPod_TH(string ids)
        {
            PrintPodModel print = new PrintPodModel();
            print.EnumerableCustomerInfo = new OrderManagementService().GetPrintAllPodCondition_TH(ids).EnumerableBoxListinfo;

            return View(print);
        }

        /// <summary>
        /// 退货仓批量汇总打印托运单
        /// </summary>
        /// <param name="ids">多个ID</param>
        /// <returns></returns>
        public ActionResult PrintSumAllPod_TH(string ids, long customerID, string warehouseName, string searchTime)
        {
            PrintPodModel print = new PrintPodModel();

            print.EnumerableCustomerInfo = new OrderManagementService().GetPrintSumAllPodCondition_TH(ids, customerID, warehouseName, searchTime).EnumerableBoxListinfo;

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

        [HttpGet]
        public ActionResult PrintSalesOrder(string id, int Flag)
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
            string boxnumber = "";//可截获 layer提示 不新建箱
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

        /// <summary>
        /// HABA更新出库单总体积
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public string UpdateOrderVolume(string ID, string Volume, string ShipmentType)
        {
            string msg = "";
            bool IsSuccess = new OrderManagementService().UpdateOrderVolume(ID, Volume, ShipmentType, base.UserInfo.Name, out msg);
            if (IsSuccess && msg == "")
            {
                return new { code = 0, message = msg }.ToJsonString();
            }
            else
            {
                return new { code = 402, message = msg }.ToJsonString();
            }
        }

        /// <summary>
        /// 现场用（出库单管理）
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public ActionResult IndexLocale(long? customerID)
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
        /// 现场用（出库单管理）
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="PageIndex"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IndexLocale(OrderViewModel vm, int? PageIndex, string Action)
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

                    if (vm.SearchCondition.CustomerID == 0)
                    {
                        columnOrder = columnOrder.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                        columnOrderDetail = columnOrderDetail.Where(c => (c.IsKey == true && c.ForView == true && c.IsImportColumn == true));
                    }
                    else
                    {
                        var notKeyColumns1 = columnOrder.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.SearchCondition.CustomerID));
                        var notKeyColumns2 = columnOrderDetail.Where(c => (c.IsKey == false && c.IsHide == false && c.ForView == true && c.CustomerID == vm.SearchCondition.CustomerID));
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
            if (Action == "下载模板")
            {
                IEnumerable<Column> columnReceipt;

                var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Order_Pack").Count() == 0)
                {
                    columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(null, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Warehouse_Location").ColumnCollection.Where(c => c.IsImportColumn == true);
                }
                else
                {
                    columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_Order_Pack").ColumnCollection.Where(c => c.IsImportColumn == true);
                }
                DataSet ds = new DataSet();
                DataTable dtReceipt = new DataTable();
                foreach (var receipt in columnReceipt)
                {
                    dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
                }
                dtReceipt.TableName = "包装信息导入模板";
                ds.Tables.Add(dtReceipt);
                //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "包装信息导入模板");
                EPPlusOperation.ExportDataSetByEPPlus(ds, "包装信息导入模板");

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

        /// <summary>
        /// 退货仓获取相同PLNO下订单号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AcquireIDS(string ids)
        {
            List<OrderDetailInfo> response;
            try
            {
                response = new OrderManagementService().AcquireIDS(ids);

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
            catch (Exception e)
            {

            }
            return Json(new { Code = 2 });
        }

        /// <summary>
        /// 现场用（出库单管理）
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="PageIndex"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PrintExpressYto(string BoxNumber)//= "TRB20092010010360050"
        {

            PrintExpressYtoModel ytoModel = new PrintExpressYtoModel();

            var responser = new OrderManagementService().PrintExpressYto(BoxNumber);
            if (responser.IsSuccess)
            {
                foreach (var item in responser.Result.expressDeliverys)
                {
                    item.shortAddressXia = item.shortAddress.Split('-')[1] + "-" + item.shortAddress.Split('-')[2];
                    item.shortAddress = item.shortAddress.Split('-')[0] + " " + item.shortAddress.Split('-')[1] + "-" + item.shortAddress.Split('-')[2] + " " + item.shortAddress.Split('-')[3];
                }
                ytoModel.expressDeliverys = responser.Result.expressDeliverys;
                ytoModel.packageDetailInfos = responser.Result.packageDetailInfos;
            }
            return View(ytoModel);



        }


    }
}
