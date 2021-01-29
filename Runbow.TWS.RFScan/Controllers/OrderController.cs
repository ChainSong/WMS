using CSRedis;
using Newtonsoft.Json;
using Runbow.TWS.Biz.RFWeb;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.RFScan.Common;
using Runbow.TWS.RFScan.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RCommon = Runbow.TWS.Common;

namespace Runbow.TWS.RFScan.Controllers
{
    public class OrderController : Controller
    {

        public ActionResult GetOrderListMain(string CustomerID, string WareHouseName, string WareHouseID)
        {
            OrderViewModel vm = new OrderViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.WareHouseID = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            var response = new OrderManagementService().GetOrderList(CustomerID, WareHouseName,"");
            vm.OrderCollection = response;
            return View(vm);
        }
        [HttpPost]
        public ActionResult GetOrderListMain(OrderViewModel vm, string Action)
        {

            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new OrderManagementService().GetOrderList(Session["CustomerID"].ToString(), Session["WareHouseName"].ToString(), vm.SearchCondition.ExternOrderNumber);
            vm.OrderCollection = response;
            return View(vm);
        }

        //
        // GET: /Receipt/
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
                    Creator = Session["Name"].ToString(),
                    CreateTime = d,
                    Updator = Session["Name"].ToString(),
                    UpdateTime = d,
                    PackageNumber = package.PackageNumber,
                    PackageType = package.PackageType,
                    ExpressNumber = package.ExpressNumber,
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
                    Creator = Session["Name"].ToString(),
                    CreateTime = d,
                    Updator = Session["Name"].ToString(),
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
        public string UpdatePackageExpress(long ID, string JsonPackage)
        {
            var responseJsonFieldsets = jsonlist<PackageInfo>(JsonPackage);
            AddPackageAndDetailRequest request = new AddPackageAndDetailRequest();
            DateTime d = DateTime.Now;
            IList<PackageInfo> packages = new List<PackageInfo>();
            IList<PackageDetailInfo> packageDetails = new List<PackageDetailInfo>();
            responseJsonFieldsets.Each((i, package) =>
            {
                packages.Add(new PackageInfo()
                {
                    Creator = Session["Name"].ToString(),
                    CreateTime = d,
                    Updator = Session["Name"].ToString(),
                    UpdateTime = d,
                    PackageNumber = package.PackageNumber,

                    ExpressNumber = package.ExpressNumber,

                    OID = ID
                });

            });
            request.packages = packages;

            if (new OrderManagementService().UpdatePackage(ID, request))
            {
                return "";
            }
            else
            {
                return "1";

            }

        }
        public ActionResult Index(string CustomerID, string WareHouseName, string WareHouseID,string OrderNumber)
        {
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.OrderNumber = OrderNumber;
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            if (!RCommon.RedisOperation.Exists(OrderNumber))
            {
                List<OrderDetailForRedisRF> orderDetails = new List<OrderDetailForRedisRF>();
                var response = new OrderManagementService().GetOrderDetailList(OrderNumber, CustomerID, WareHouseName);
                orderDetails = response.ToList();
                RCommon.RedisOperation.SetList(OrderNumber, orderDetails);
            }
            return View();
        }
        public ActionResult WavePick(string CustomerID, string WareHouseName, string WareHouseID)
        {
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            return View();
        }
        public ActionResult Confirm(string CustomerID, string WareHouseName, string WareHouseID)
        {
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            return View();
        }
        public ActionResult Confirm2(string CustomerID, string WareHouseName, string WareHouseID)
        {
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            return View();
        }
        public ActionResult BandConfirm(string CustomerID, string WareHouseName, string WareHouseID)
        {
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            return View();
        }
        /// <summary>
        /// 拣货完成同步redis中数据到数据库，同时删除redis
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns></returns>
        public string SaveRecDataFromRedis(string OrderNumber)
        {
            List<OrderDetailForRedisRF> redisList = new List<OrderDetailForRedisRF>();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            string msg = "";
                try
                {
                    if (RCommon.RedisOperation.Exists(OrderNumber))
                    {
                       redisList = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>(OrderNumber);
                       var response = new OrderManagementService().InsertPick(redisList, Session["Name"].ToString());
                       if (response)
                       {
                        WMS_Log_Operation operation = new WMS_Log_Operation();
                        operation.MenuName = "出库单管理";
                        operation.Operation = "出库单-" + "拣货";
                        operation.OrderType = "Order";
                        operation.Controller = Request.RawUrl;
                        operation.Creator = Session["Name"].ToString();
                        operation.CreateTime = DateTime.Now;
                        operation.ProjectID = Convert.ToInt32(Session["ProjectID"]);
                        operation.ProjectName = Session["ProjectName"].ToString();
                        operation.OrderNumber = OrderNumber;
                        operation.ExternOrderNumber = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>(OrderNumber).Select(c => c.ExternOrderNumber).FirstOrDefault().ToString();
                        operation.OrderID = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>(OrderNumber).Select(c=>c.OID).FirstOrDefault().ToString();
                        operation.WarehouseName = Session["WareHouseName"].ToString();
                        operation.WarehouseID =Convert.ToInt32(Session["WareHouseIDs"]);
                        operation.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                        logs.Add(operation);
                        new LogOperationService().AddLogOperation(logs);
                        RCommon.RedisOperation.Del(OrderNumber);
                        msg= "1";
                       }
                    }
                }
                catch (Exception ex)
                {
                   msg= "0";
                }
            return msg;
        }

        public string SaveRecData(string ReceiptNumber, string Type)
        {
            //SaveData
            //recModelList;
            bool resualt = new OrderManagementService().UpdateOrderStatus(ReceiptNumber, Session["CustomerID"].ToString(), Session["WareHouseName"].ToString(), Session["Name"].ToString(), Type);
            if (resualt)
            {                
                try
                {

                    if (RCommon.RedisOperation.Exists(ReceiptNumber))
                    {
                        RCommon.RedisOperation.Del(ReceiptNumber);
                    }
                    return "1";
                }
                catch (Exception ex)
                {
                    return "0";
                }
            }
            return "0";
        }
        public ActionResult GetBox()
        {

            //var WarehouseID = Convert.ToInt64(Session["WareHouseIDs"].ToString());
            //ApplicationConfigHelper.RefreshGetWarehouseLocationList(WarehouseID);

            IEnumerable<BoxSize> BoxsList = ApplicationConfigHelper.GetApplicationBox(Session["ProjectName"].ToString());
            return Json(BoxsList.ToList(), JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetOrderDetail(string OrderNumber)
        //{
        //    IEnumerable<OrderDetailInfo> orderModelList;
        //    orderModelList = new OrderManagementService().GetOrderDetailList(OrderNumber, Session["CustomerID"].ToString(), Session["WareHouseName"].ToString());
        //    return Json(orderModelList.ToList(), JsonRequestBehavior.AllowGet);
        //    //recModelList;
        //}

           /// <summary>
           /// 扫描出库单号，从redis中拉取明细
           /// </summary>
           /// <param name="OrderNumber"></param>
           /// <returns></returns>
        public ActionResult GetOrderDetail(string OrderNumber)
        {

            List<OrderDetailForRedisRF> lists = new List<OrderDetailForRedisRF>();
            try
            {

                if (RCommon.RedisOperation.Exists(OrderNumber))
                {
                    lists = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>(OrderNumber);
                }

            }
            catch (Exception ex)
            {

            }
            return Json(lists, JsonRequestBehavior.AllowGet);
            //recModelList;
        }

        /// <summary>
        /// 扫描出库单号，从redis中拉取明细过滤掉已拣货的
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns></returns>
        public ActionResult GetOrderDetailDelete(string OrderNumber)
        {

            List<OrderDetailForRedisRF> lists = new List<OrderDetailForRedisRF>();
            try
            {

                if (RCommon.RedisOperation.Exists(OrderNumber))
                {
                    lists = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>(OrderNumber).Where(c=>c.Qty!=c.QtyPicked).ToList();
                }

            }
            catch (Exception ex)
            {

            }
            return Json(lists, JsonRequestBehavior.AllowGet);
            //recModelList;
        }
        /// <summary>
        /// RF复检包装
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderCheckAgain(string CustomerID)
        {
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            return View();
        }
        public string CheckOrderExpress(string OrderNumber, string ExpressNumber)
        {
            string msg = "";
            try
            {
                if (RCommon.RedisOperation.Exists("Check:" + OrderNumber))
                {
                    var lists = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>("Check:" + OrderNumber);
                    if (lists.Where(c => c.ExpressNumber == ExpressNumber).Count() > 0)
                    {
                        msg = "1";
                    }
                    else
                    {
                        msg = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "-1";
            }
            return msg;
        }
        /// <summary>
        /// 扫描箱型
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="boxnums"></param>
        /// <returns></returns>
        public string ScanBoxNum(string OrderNumber, string boxnums)
        {
            OrderViewModel vm = new OrderViewModel();
            string msg = "";
            List<OrderDetailForRedisRF> Lists = new List<OrderDetailForRedisRF>();
            try
            {
                if (vm.BoxSize.Where(c => c.Value == boxnums).Count() == 0)
                {
                    msg = "箱型不存在";
                    return msg;
                }
                if (RCommon.RedisOperation.Exists("Check:" + OrderNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>("Check:" + OrderNumber);
                    Lists.ForEach(a => a.BoxNum = boxnums);
                    RCommon.RedisOperation.SetList("Check:" + OrderNumber, Lists);
                }
                else
                {
                    msg = "订单数据不存在";
                }
               
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
         }
        public JsonResult UpdateOrderDetailQtyRedisSecond(string OrderNumber, string SKU)
        {
            List<OrderDetailForRedisRF> Lists = new List<OrderDetailForRedisRF>();
            try
            {

                if (RCommon.RedisOperation.Exists("Check:" + OrderNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>("Check:" + OrderNumber);
                    using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:OSL" + OrderNumber + SKU, _client = RCommon.RedisOperation.cs })
                    {
                        if (RCommon.RedisOperation.Lock("OSL" + OrderNumber + SKU, 5) != null)
                        {
                            foreach (var item in Lists.Where(c => c.SKU == SKU))
                            {
                                item.QtyPicked = item.QtyPicked + 1;
                                item.Confirmer = Session["Name"].ToString();
                                item.ConfirmeTime = DateTime.Now;
                                RCommon.RedisOperation.SetList("Check:" + OrderNumber, Lists);
                                return Json(new { Code = "1", data = Lists.Where(c => c.SKU == SKU) });
                            }

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = Lists.Where(c => c.SKU == SKU) });
            }
            return Json(new { Code = "0", data = Lists.Where(c => c.SKU == SKU) });
        }
        public string SaveRecDataFromRedisSecond(string OrderNumber)
        {
            List<OrderDetailForRedisRF> redisList = new List<OrderDetailForRedisRF>();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            IList<PackageInfo> packages = new List<PackageInfo>();
            IList<PackageDetailInfo> packageDetails = new List<PackageDetailInfo>();
            AddPackageAndDetailRequest request = new AddPackageAndDetailRequest();
            string msg = "";
            try
            {
                var BoxSizeData = ApplicationConfigHelper.GetWMS_Config("BoxSize").Where(c => c.Str5 == "NIKEB2C");
                if (RCommon.RedisOperation.Exists("Check:" + OrderNumber))
                {
                    redisList = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>("Check:" + OrderNumber);
                    DateTime d = DateTime.Now;
                    packages.Add(new PackageInfo()
                    {
                        Creator = Session["Name"].ToString(),
                        CreateTime = d,
                        Updator = Session["Name"].ToString(),
                        UpdateTime = d,
                        OrderNumber= OrderNumber,
                        PackageType = redisList.Select(c=>c.BoxNum).FirstOrDefault(),
                        PackageNumber = redisList.Select(c => c.ExpressNumber).FirstOrDefault(),
                        Length = BoxSizeData.Where(c=>c.Code== redisList.Select(a => a.BoxNum).FirstOrDefault()).Select(a=>a.Str1).FirstOrDefault(),
                        Width = BoxSizeData.Where(c => c.Code == redisList.Select(a => a.BoxNum).FirstOrDefault()).Select(a => a.Str2).FirstOrDefault(),
                        Height = BoxSizeData.Where(c => c.Code == redisList.Select(a => a.BoxNum).FirstOrDefault()).Select(a => a.Str3).FirstOrDefault(),
                        NetWeight = "0",
                        GrossWeight = "0",
                        PackageTime = d,
                        OID = redisList.Select(c=>c.OID).FirstOrDefault()
                    });
                    foreach (var item in redisList)
                    {
                        packageDetails.Add(new PackageDetailInfo()
                        {
                            Creator = Session["Name"].ToString(),
                            CreateTime = d,
                            Updator = Session["Name"].ToString(),
                            UpdateTime = d,
                            PackageNumber = item.ExpressNumber,
                            SKU = item.SKU,
                            UPC = item.UPC,
                            GoodsName = item.GoodsName,
                            GoodsType = "A品",
                            Qty = item.QtyPicked
                        });

                    }





                    request.packages = packages;
                    request.packageDetails = packageDetails;
                    bool resualt = new OrderManagementService().UpdateOrderStatusByOrderNumber(OrderNumber, Session["Name"].ToString(), request,Session["CustomerID"].ToString());
                    if (resualt)
                    {
                        WMS_Log_Operation operation = new WMS_Log_Operation();
                        operation.MenuName = "出库单管理";
                        operation.Operation = "出库单-" + "复检包装";
                        operation.OrderType = "Order";
                        operation.Controller = Request.RawUrl;
                        operation.Creator = Session["Name"].ToString();
                        operation.CreateTime = DateTime.Now;
                        operation.ProjectID = Convert.ToInt32(Session["ProjectID"]);
                        operation.ProjectName = Session["ProjectName"].ToString();
                        operation.OrderID = redisList.Select(c => c.OID).FirstOrDefault().ToString();
                        logs.Add(operation);
                        new LogOperationService().AddLogOperation(logs);
                        RCommon.RedisOperation.Del("Check:" + OrderNumber);
                        msg = "1";
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "0";
            }
            return msg;
        }
        public string CheckOrderDetailQtyPickRedisSecond(string OrderNumber, string SKU)
        {
            string response = "";
            try
            {

                List<OrderDetailForRedisRF> Lists = new List<OrderDetailForRedisRF>();
                if (RCommon.RedisOperation.Exists("Check:" + OrderNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>("Check:" + OrderNumber);
                    using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:OS" + OrderNumber + SKU, _client = RCommon.RedisOperation.cs })
                    {
                        if (RCommon.RedisOperation.Lock("OS" + OrderNumber + SKU, 5) != null)
                        {
                            foreach (var item in Lists.Where(c => c.SKU == SKU))
                            {
                                if (item.Qty <= item.QtyPicked)
                                {
                                    response = "1";
                                }
                            }

                        }
                    }
                }
                else
                {
                    response = "2";
                }


            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }


        /// <summary>
        /// 验证订单是否是取消单
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="customerID"></param>
        /// <param name="type">1.代表传入的是系统ordernumber，2.代表快递单号</param>
        /// <returns></returns>
        public string ValidOrderCancel(string OrderNumber, long customerID, string warehouse, int type)
        {
            //查询订单是否是取消单调不同存过
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderCancel_" + Session["ProjectName"]);
            }
            catch
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("OrderCancel");
            }
            return new DeliverConfirmService().ValidOrderCancel(OrderNumber, Convert.ToInt64(Session["CustomerID"]), wms.FirstOrDefault().Name, warehouse, type);
        }

        public string UpdateOrderDetailQtyRedisByQtyScan(string OrderNumber, string SKU, string Qty)
        {
            string response = "";
            try
            {

                List<OrderDetailForRedisRF> Lists = new List<OrderDetailForRedisRF>();
                if (RCommon.RedisOperation.Exists(OrderNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>(OrderNumber);
                    using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:OSL" + OrderNumber + SKU, _client = RCommon.RedisOperation.cs })
                    {
                        if (RCommon.RedisOperation.Lock("OSL" + OrderNumber + SKU, 5) != null)
                        {
                            foreach (var item in Lists.Where(c => c.SKU == SKU))
                            {
                                if (item.Qty < Convert.ToDecimal(Qty))
                                {
                                    response = "1";
                                }
                                else
                                {
                                    item.QtyPicked = Convert.ToDecimal(Qty);
                                }
                            }
                            RCommon.RedisOperation.Del(OrderNumber);
                            RCommon.RedisOperation.SetList(OrderNumber, Lists);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }

        public ActionResult GetOrderDetailByOrderNumber(string OrderNumber)
        {

            List<OrderDetailForRedisRF> lists = new List<OrderDetailForRedisRF>();
            try
            {

                if (RCommon.RedisOperation.Exists("Check:"+OrderNumber))
                {
                    lists = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>("Check:" + OrderNumber);
                }
                else
                {
                    var response = new OrderManagementService().GetOrderDetailListByOrderNumber(OrderNumber, Session["CustomerID"].ToString());
                    if (response.Count() > 0)
                    {
                        lists = response.ToList();
                        RCommon.RedisOperation.SetList("Check:" + OrderNumber, lists);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return Json(lists, JsonRequestBehavior.AllowGet);
            //recModelList;
        }
        /// <summary>
        /// 扫描一个更新redis中的实收数量+1
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="SKU"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        public JsonResult UpdateOrderDetailQtyRedis(string OrderNumber, string SKU, string Location)
        {
            List<OrderDetailForRedisRF> Lists = new List<OrderDetailForRedisRF>();
            try
            {

                if (RCommon.RedisOperation.Exists(OrderNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>(OrderNumber);
                    using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:OSL" + OrderNumber + SKU + Location, _client = RCommon.RedisOperation.cs })
                    {
                        if (RCommon.RedisOperation.Lock("OSL" + OrderNumber + SKU + Location, 5) != null)
                        {
                            foreach (var item in Lists.Where(c => c.SKU == SKU && c.Location == Location))
                            {
                                item.QtyPicked = item.QtyPicked + 1;
                                item.Picker = Session["Name"].ToString();
                                item.PickTime = DateTime.Now;
                                RCommon.RedisOperation.SetList(OrderNumber, Lists);
                                return Json(new { Code = "1", data = Lists.Where(c=>c.Location==Location&&c.SKU==SKU) });
                            }
                           
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = Lists.Where(c => c.Location == Location && c.SKU == SKU) });
            }
            return Json(new { Code = "0", data = Lists.Where(c => c.Location == Location && c.SKU == SKU) });
        }
        /// <summary>
        /// 数量回车更新redis中的实收数量
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="SKU"></param>
        /// <param name="Location"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        public string UpdateOrderDetailQtyRedisByQtyScan(string OrderNumber, string SKU, string Location,string Qty)
        {
            string response = "";
            try
            {

                List<OrderDetailForRedisRF> Lists = new List<OrderDetailForRedisRF>();
                if (RCommon.RedisOperation.Exists(OrderNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>(OrderNumber);
                    using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:OSL" + OrderNumber + SKU + Location, _client = RCommon.RedisOperation.cs })
                    {
                        if (RCommon.RedisOperation.Lock("OSL" + OrderNumber + SKU + Location, 5) != null)
                        {
                            foreach (var item in Lists.Where(c => c.SKU == SKU && c.Location == Location))
                            {
                                if (item.Qty < item.QtyPicked + Convert.ToDecimal(Qty))
                                {
                                    response = "1";
                                }
                                else
                                {
                                item.QtyPicked = item.QtyPicked + Convert.ToDecimal(Qty);
                                }
                            }
                            RCommon.RedisOperation.Del(OrderNumber);
                            RCommon.RedisOperation.SetList(OrderNumber, Lists);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }
        /// <summary>
        /// 查询当前SKU是否已经扫描完成
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="SKU"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        public string CheckOrderDetailQtyPickRedis(string OrderNumber, string SKU, string Location)
        {
            string response = "";
            try
            {

                List<OrderDetailForRedisRF> Lists = new List<OrderDetailForRedisRF>();
                if (RCommon.RedisOperation.Exists(OrderNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>(OrderNumber);
                    using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:OSL" + OrderNumber + SKU + Location, _client = RCommon.RedisOperation.cs })
                    {
                        if (RCommon.RedisOperation.Lock("OSL" + OrderNumber + SKU + Location, 5) != null)
                        {
                            foreach (var item in Lists.Where(c => c.SKU == SKU && c.Location == Location))
                            {
                                if (item.Qty <= item.QtyPicked)
                                {
                                    response = "1";
                                }
                            }

                        }
                    }
                }
                else
                {
                    response = "2";
                }


            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }
        /// <summary>
        /// 点击完成同步redis中数据到拣货表
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns></returns>
        public string InsertIntoPickFromRedis(string OrderNumber,string ID)
        {
            List<OrderDetailForRedisRF> picklistredis = new List<OrderDetailForRedisRF>();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            try
            {
                if (RCommon.RedisOperation.Exists(OrderNumber))
                {
                    picklistredis = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>(OrderNumber);
                }
                else
                {
                    return "2";
                }
                var response = new OrderManagementService().InsertPick(picklistredis, Session["Name"].ToString());

                if (response)
                {
                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "出库单管理";
                    operation.Operation = "出库单-" + "拣货";
                    operation.OrderType = "Order";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = Session["Name"].ToString();
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = Convert.ToInt32(Session["ProjectID"]);
                    operation.ProjectName = Session["ProjectName"].ToString();
                    operation.OrderID = ID;
                    logs.Add(operation);
                    new LogOperationService().AddLogOperation(logs);
                    RCommon.RedisOperation.Del(OrderNumber);
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
            return "0";
        }
        /// <summary>
        /// 查询该订单所有SKU是否已扫完
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns></returns>
        public string CheckOrderDetailQtyPickRedisAll(string OrderNumber)

        {
            string response = "";
            try
            {

                List<OrderDetailForRedisRF> Lists = new List<OrderDetailForRedisRF>();
                if (RCommon.RedisOperation.Exists("Check:"+OrderNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<OrderDetailForRedisRF>>("Check:" + OrderNumber);
                    using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:O" + OrderNumber, _client = RCommon.RedisOperation.cs })
                    {
                        if (RCommon.RedisOperation.Lock("O" + OrderNumber, 5) != null)
                        {
                            foreach (var item in Lists)
                            {
                                if (item.Qty != item.QtyPicked)
                                {
                                    response = "1";
                                    return response;
                                }
                            }

                        }
                    }

                }
                else
                {
                    response = "3";
                }


            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }
        public ActionResult GetOrderDetailByWave(string OrderNumber)
        {
            IEnumerable<OrderDetailInfo> orderModelList;
            orderModelList = new OrderManagementService().GetOrderDetailListByWave(OrderNumber, Session["CustomerID"].ToString(), Session["WareHouseName"].ToString());
            return Json(orderModelList.ToList(), JsonRequestBehavior.AllowGet);
            //recModelList;
        }
        public ActionResult GetPackageDetail(string OrderID)
        {
            IEnumerable<PackageDetailInfo> orderModelList;
            orderModelList = new OrderManagementService().GetPackageDetailList(OrderID, Session["CustomerID"].ToString(), Session["WareHouseName"].ToString());
            return Json(orderModelList.ToList(), JsonRequestBehavior.AllowGet);
            //recModelList;
        }
        public ActionResult GetPackage(string OrderNumber)
        {
            IEnumerable<PackageInfo> orderModelList;
            orderModelList = new OrderManagementService().GetPackageList(OrderNumber, Session["CustomerID"].ToString(), Session["WareHouseName"].ToString());
            return Json(orderModelList.ToList(), JsonRequestBehavior.AllowGet);
            //recModelList;
        }
        public ActionResult GetOrderDetail2(string OrderNumber)
        {
            IEnumerable<OrderDetailInfo> orderModelList;
            orderModelList = new OrderManagementService().GetOrderDetailList2(OrderNumber, Session["CustomerID"].ToString(), Session["WareHouseName"].ToString());
            return Json(orderModelList.ToList(), JsonRequestBehavior.AllowGet);
            //recModelList;
        }
        public static List<T> jsonlist<T>(string str)
        {
            return JsonConvert.DeserializeObject<List<T>>(str);
        }

        public string CheckLocation(string Location)
        {

            long WarehouseID = 15;
            //ApplicationConfigHelper.RefreshGetWarehouseLocationList(WarehouseID);
            if (ApplicationConfigHelper.GetWarehouseLocationList(WarehouseID).Where(m => m.Location == Location).Count() > 0)
            {
                return "1";
            }
            return "0";
        }
        public string SaveOrderData(string OrderNumber, string JsonData)
        {
            //SaveData
            //recModelList;
            return "1";
        }
        /// <summary>
        /// 快递包装
        /// </summary>
        /// <returns></returns>
        public ActionResult Expresspackage(long? CustomerID, string WareHouseName, long? WareHouseID)
        {
            ExpressPackageModel vm = new ExpressPackageModel();
            //string ViewType = "0";
            //var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(Convert.ToInt64(Session["ProjectID"].ToString()), Convert.ToInt64(Session["ID"].ToString())).Where(t => t.StoreType == 2 || t.StoreType == 3);
            //var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerID;

            //IEnumerable<SelectListItem> WarehouseList = null;
            //WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == Convert.ToInt64(Session["CustomerID"].ToString()) && c.UserID == int.Parse(Session["ID"].ToString()))).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            ViewBag.WarehouseList = WareHouseID;

            vm.CustomerIDs = CustomerID;
            vm.WarehouseIDs = WareHouseID;
            vm.Warehouses = WareHouseName;
            vm.SupplieTypeList = getSupplieTypeList();
            return View(vm);
        }
        /// <summary>
        /// 检测快递单号
        /// </summary>
        /// <param name="ExpressNumber"></param>
        /// <returns></returns>
        //public string CheckExpress(string ExpressNumber,)
        //{
        //    ExpressPackageResponse EPR = new ExpressPackageResponse();
        //    ExpressPackageModel EP = new ExpressPackageModel();

        //    OrderManagementService o = new OrderManagementService();
        //    string WarehouseName = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(a => a.WarehouseID == base.UserInfo.WarehouseID).Select(b => b.WarehouseName).FirstOrDefault();
        //    EPR = o.CheckExpress(ExpressNumber, base.UserInfo.CustomerID, WarehouseName);
        //    EP.PackageCollection = EPR.PackageCollection;
        //    EP.OrderDetailCollection = EPR.OrderDetailCollection;

        //    string s = "";
        //    DataSet dt = new DataSet();
        //    dt = o.CheckExpress(ExpressNumber, base.UserInfo.CustomerID, WarehouseName, s);

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
            //ExpressPackageModel EP = new ExpressPackageModel();

            string WarehouseName = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(a => a.WarehouseID == WarehouseID).Select(b => b.WarehouseName).FirstOrDefault();
            OrderManagementService o = new OrderManagementService();
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
        public string SaveExpressPackage(long CustomerID, string WarehouseName, long OrderID, string ExpressNumber, string PackageType)
        {
            DataSet dt = new DataSet();
            OrderManagementService o = new OrderManagementService();
            dt = o.SaveExpressPackage(ExpressNumber, PackageType, CustomerID, WarehouseName);
            #region 操作日志
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            WMS_Log_Operation operation = new WMS_Log_Operation();
            operation.MenuName = "快递包装管理";
            operation.Operation = "快递包装-保存";
            operation.OrderType = "ExpressPackage";
            operation.Controller = Request.RawUrl;
            operation.Creator = Session["Name"].ToString();
            operation.CreateTime = DateTime.Now;
            operation.ProjectID =Convert.ToInt32(Session["ProjectID"]);
            operation.ProjectName = Session["ProjectName"].ToString();
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
                wms = ApplicationConfigHelper.GetWMS_Config("BoxSize_" + Session["ProjectName"].ToString());
            }
            catch (Exception)
            {}
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("BoxSize");
            }
            List<SelectListItem> SupplieTypeList = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                SupplieTypeList.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            return SupplieTypeList;
        }
        /// <summary>
        /// 获取配置的耗材类型转JSON
        /// </summary>
        /// <returns></returns>
        public string getSupplieTypeListJSON(string PackageType)
        {
            string js = string.Empty;
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("BoxSize_" + Session["ProjectName"]);
            }
            catch (Exception)
            {

            }
            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("BoxSize");
            }

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            if (wms.Where(m => m.Code == PackageType).Count() == 0)
            {
                if (wms.Where(m => m.Name == PackageType).Count() == 0)
                {
                    js = "";
                }
                else
                {
                    js = jsonSerializer.Serialize(wms.Where(m => m.Name == PackageType));
                }
            }
            else
            {
                js = jsonSerializer.Serialize(wms.Where(m => m.Code == PackageType));
            }
            return js;
        }

        /// <summary>
        /// 软键盘
        /// </summary>
        //public static InputPanel _softKeyBoard = new InputPanel();
        /// <summary>
        /// 显示/隐藏 软键盘
        /// </summary>
        public bool ShowHideSoftKeyBoard(Boolean isShow)
        {
            //_softKeyBoard.Enabled = isShow;
            //return _softKeyBoard.Enabled;
            return isShow;
        }

    }
}
