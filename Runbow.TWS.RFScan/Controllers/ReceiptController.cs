
using CSRedis;
using TBIZ = Runbow.TWS.Biz;
using Runbow.TWS.Biz.RFWeb;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.MessageContracts.WMS.Shelves;
using Runbow.TWS.RFScan.Common;
using Runbow.TWS.RFScan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RCommon = Runbow.TWS.Common;
using Runbow.TWS.Entity.WMS.Product;
using Runbow.TWS.MessageContracts;
using System.Threading.Tasks;
using System.Threading;
using Runbow.TWS.Entity.WMS.Receipt;
using Runbow.TWS.Logger.LogHelper;

namespace Runbow.TWS.RFScan.Controllers
{
    public class ReceiptController : Controller
    {
        private static readonly object Lock = new object();
        public ActionResult ReceiptReceivingMain(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetReceiptList(CustomerID, WareHouseID, "");
            vm.ReceiptCollection = response;
            return View(vm);
        }
        /// <summary>
        /// 分款选单页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        public ActionResult ASNListBack_Article(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetASNList(CustomerID, WareHouseID, "");
            vm.AsnCollection = response;
            return View(vm);
        }

        /// <summary>
        /// 分款选单页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ASNListBack_Article(ReceiptViewModel vm, string Action)
        {
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetASNList(Session["CustomerID"].ToString(), Session["WareHouseIDs"].ToString(), vm.SearchCondition.ExternReceiptNumber);
            vm.AsnCollection = response;
            return View(vm);
        }

        /// <summary>
        /// 分SKU选单页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>

        public ActionResult ASNListBack_SKU(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetASNList(CustomerID, WareHouseID, "");
            vm.AsnCollection = response;
            return View(vm);
        }

        /// <summary>
        /// ABC选单页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>

        public ActionResult ASNListBack_ABC(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetASNList(CustomerID, WareHouseID, "");
            vm.AsnCollection = response;
            return View(vm);
        }

        /// <summary>
        /// RF托盘绑定
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        public ActionResult ReceiptBindTray(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            return View(vm);
        }

        /// <summary>
        /// RF绑定托盘移动
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        public ActionResult ReceiptBindTrayMove(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            return View(vm);
        }


        /// <summary>
        /// 托盘验证,1:托盘号是否存在;2:托盘号是否已被绑定
        /// </summary>
        /// <param name="TrayNum"></param>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckTrayNumBindTray(string TrayNum, string CustomerID, string WareHouseName, string WareHouseID, int Status = 0)
        {
            ReceiptManagementService service = new ReceiptManagementService();
            try
            {
                if (service.IsExistTrayNumBindTray(TrayNum, WareHouseID))//库位表是否存在托盘号
                {
                    if (Status == 1)//托盘移动
                    {
                        if (service.IsExistTrayNumBindTray(TrayNum, WareHouseID, 1))
                        {
                            var trayList = RCommon.RedisOperation.GetList<List<ASNScanTray>>("Tray:" + TrayNum + ":Status:" + 1);
                            //if (RCommon.RedisOperation.Exists("Tray:" + TrayNum + ":Status:" + 1))
                            //{

                            //}
                        }
                        else if (service.IsExistTrayNumBindTray(TrayNum, WareHouseID, 4))
                        {

                        }
                        else
                        {
                            return Json(new { data = 3, msg = "托盘未绑定！" });
                        }
                    }
                    return Json(new { data = 1, msg = "" });
                }
                else
                    return Json(new { data = 2, msg = "托盘号不存在！" });
            }
            catch (Exception ex)
            {
                return Json(new { data = 0, msg = ex.Message });
            }
        }

        /// <summary>
        /// 库位验证
        /// </summary>
        /// <param name="TrayNum"></param>
        /// <param name="Location"></param>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckLocationBindTray(string TrayNum, string Location, string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptManagementService service = new ReceiptManagementService();
            try
            {
                if (service.IsExistTrayNumBindTray(Location, WareHouseID))//库位表存在当前库位号
                {
                    //var trays = RCommon.RedisOperation.GetList<List<ASNScanTray>>("Tray:" + TrayNum + ":Status");

                    List<ASNScanTray> list = new List<ASNScanTray>();
                    list.Add(new ASNScanTray()
                    {
                        CustomerID = Convert.ToInt64(CustomerID),
                        WarehouseID = Convert.ToInt64(WareHouseID),
                        WarehouseName = WareHouseName,
                        TrayNumber = TrayNum,
                        Location = Location,
                        Status = 0
                    });
                    var result = service.AddASNScanTray(list, Session["Name"].ToString(), 1);

                    return Json(new { data = 1, msg = "" });
                }
                else
                    return Json(new { data = 2, msg = "库位不存在！" });
            }
            catch (Exception ex)
            {
                return Json(new { data = 0, msg = ex.Message });
            }
        }


        /// <summary>
        ///  箱号验证，1:是否存在当前扫描箱号;2:扫描的箱号是否为同一单
        /// </summary>
        /// <param name="TrayNum"></param>
        /// <param name="BoxNum"></param>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckBoxNumBindTray(string TrayNum, string BoxNum, string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptManagementService service = new ReceiptManagementService();
            var asnDetails = service.GetASNDetailScanBindTray(BoxNum, CustomerID).FirstOrDefault();//箱号是否存在
            //箱绑托，1；4
            if (asnDetails != null)//箱号存在
            {
                if (RCommon.RedisOperation.Exists("Tray:" + TrayNum))
                {
                    var list = RCommon.RedisOperation.GetList<List<ASNScanTray>>("Tray:" + TrayNum);
                    var boxList = list.Where(a => a.BoxNumber == BoxNum).ToList();
                    if (boxList != null && boxList.Any())//当前箱已扫描
                    {
                        return Json(new { data = 3, msg = "当前箱号已扫描！" });
                    }
                    else
                    {
                        var asnList = list.Where(a => a.ASNNumber == asnDetails.ASNNumber).ToList();//不存在当前箱号，检查是否同一单
                        if (asnList != null && asnList.Any())
                        {
                            var tray = new ASNScanTray()
                            {
                                ASNNumber = asnDetails.ASNNumber,
                                ExternReceiptNumber = asnDetails.ExternReceiptNumber,
                                CustomerID = asnDetails.CustomerID,
                                CustomerName = asnDetails.CustomerName,
                                WarehouseID = Convert.ToInt64(WareHouseID),
                                WarehouseName = WareHouseName,
                                TrayNumber = TrayNum,
                                BoxNumber = BoxNum,
                                Status = asnDetails.Int1.Value
                            };
                            var tList = new List<ASNScanTray>();
                            tList.Add(tray);
                            var result = service.AddASNScanTray(tList, Session["Name"].ToString());
                            list.Add(tray);
                            RCommon.RedisOperation.SetList("Tray:" + TrayNum, list);
                        }
                        else
                            return Json(new { data = 2, msg = "不是同一RSO单" });
                    }
                }
                else
                {


                    List<ASNScanTray> trays = new List<ASNScanTray>();

                    ASNScanTray tray = new ASNScanTray()
                    {
                        ASNNumber = asnDetails.ASNNumber,
                        ExternReceiptNumber = asnDetails.ExternReceiptNumber,
                        CustomerID = asnDetails.CustomerID,
                        CustomerName = asnDetails.CustomerName,
                        WarehouseID = Convert.ToInt64(WareHouseID),
                        WarehouseName = WareHouseName,
                        TrayNumber = TrayNum,
                        BoxNumber = BoxNum,
                        Status = asnDetails.Int1.Value
                    };
                    trays.Add(tray);
                    var result = service.AddASNScanTray(trays, Session["Name"].ToString());
                    RCommon.RedisOperation.SetList("Tray:" + TrayNum, trays);
                }

                #region MyRegion
                //if (service.IsExistTrayNumBindTray(TrayNum, WareHouseID, asnDetails.Int1.Value))  //Redis存在当前托盘号key RCommon.RedisOperation.Exists("Tray:" + TrayNum)
                //{
                //    //var list = RCommon.RedisOperation.GetList<List<ASNScanTray>>("Tray:" + TrayNum + ":Status:" + asnDetails.Int1.Value);
                //    //var list = RCommon.RedisOperation.GetList<List<ASNScanTray>>("Tray:" + TrayNum);//.Where(a => a.Status == asnDetails.Int1).ToList();
                //    var list = service.GetTrayNumList(TrayNum, asnDetails.Int1.Value).ToList();
                //    var boxList = list.Where(a => a.BoxNumber == BoxNum && a.Status == asnDetails.Int1.Value).ToList();
                //    if (boxList != null && boxList.Any())//当前箱已扫描
                //    {
                //        return Json(new { data = 3, msg = "当前箱号已扫描！" });
                //    }
                //    else
                //    {
                //        var asnList = list.Where(a => a.ASNNumber == asnDetails.ASNNumber);//不存在当前箱号，检查是否同一单
                //        if (asnList != null && asnList.Any())//是同一单，新增
                //        {
                //            var tList = new List<ASNScanTray>();
                //            tList.Add(new ASNScanTray()
                //            {
                //                ASNNumber = asnDetails.ASNNumber,
                //                ExternReceiptNumber = asnDetails.ExternReceiptNumber,
                //                CustomerID = asnDetails.CustomerID,
                //                CustomerName = asnDetails.CustomerName,
                //                WarehouseID = Convert.ToInt64(WareHouseID),
                //                WarehouseName = WareHouseName,
                //                TrayNumber = TrayNum,
                //                BoxNumber = BoxNum,
                //                Status = asnDetails.Int1.Value
                //            });
                //            var result = service.AddASNScanTray(tList, Session["Name"].ToString());
                //            //RCommon.RedisOperation.SetList("Tray:" + TrayNum, list);
                //        }
                //        else
                //        {
                //            return Json(new { data = 2, msg = "不是同一RSO单" });
                //        }
                //    }
                //}
                //else//Redis不存在当前托盘号key，说明当前新输入，将此箱号对应的所有detail添加到Redis中
                //{
                //    List<ASNScanTray> trays = new List<ASNScanTray>();

                //    ASNScanTray tray = new ASNScanTray()
                //    {
                //        ASNNumber = asnDetails.ASNNumber,
                //        ExternReceiptNumber = asnDetails.ExternReceiptNumber,
                //        CustomerID = asnDetails.CustomerID,
                //        CustomerName = asnDetails.CustomerName,
                //        WarehouseID = Convert.ToInt64(WareHouseID),
                //        WarehouseName = WareHouseName,
                //        TrayNumber = TrayNum,
                //        BoxNumber = BoxNum,
                //        Status = asnDetails.Int1.Value
                //    };
                //    trays.Add(tray);
                //    var result = service.AddASNScanTray(trays, Session["Name"].ToString());
                //    //RCommon.RedisOperation.SetList("Tray:" + TrayNum + ":Status:" + asnDetails.Int1.Value, trays);
                //    //RCommon.RedisOperation.SetList("Tray:" + TrayNum, trays);
                //}
                #endregion

                return Json(new { data = 1, msg = "" });
            }
            else
                return Json(new { data = 0, msg = "箱号不存在！" });
        }






        /// <summary>
        /// ABC选单页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ASNListBack_ABC(ReceiptViewModel vm, string Action)
        {
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetASNList(Session["CustomerID"].ToString(), Session["WareHouseIDs"].ToString(), vm.SearchCondition.ExternReceiptNumber);
            vm.AsnCollection = response;
            return View(vm);
        }
        /// <summary>
        /// 装箱的时候清空重扫
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <returns></returns>
        public string ClearScanDetail(string AsnNumber)
        {
            string msg = "";
            try
            {
                if (RCommon.RedisOperation.Exists("CloseBox:" + AsnNumber + ":" + Session["Name"].ToString()))
                {
                    RCommon.RedisOperation.Del("CloseBox:" + AsnNumber + ":" + Session["Name"].ToString());
                }
            }
            catch (Exception ex)
            {
                msg = "-1";
            }
            return msg;

        }
        /// <summary>
        /// 上架重扫本箱
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public string ScanBoxNumberAgain(string ReceiptNumber, string BoxNumber)
        {
            string msg = "";
            try
            {
                if (RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber).Where(a => a.str2 == BoxNumber).FirstOrDefault() == null)
                {
                    return "箱号不正确";
                }

                List<WMS_LogRF_Exception> logRF_Exceptions = new List<WMS_LogRF_Exception>();
                if (RCommon.RedisOperation.Exists("RFLogDeleteBoxNUmber:" + DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    logRF_Exceptions = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLogDeleteBoxNUmber:" + DateTime.Now.ToString("yyyy-MM-dd"));
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "ScanBoxNumberAgain",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLogDeleteBoxNUmber:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                else
                {
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "ScanBoxNumberAgain",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLogDeleteBoxNUmber:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }



                var response = new ReceiptManagementService().DeleteScanBoxNumber(ReceiptNumber, BoxNumber);
                if (response)
                {
                    if (RCommon.RedisOperation.Exists("Back:" + ReceiptNumber + ":" + BoxNumber))
                    {
                        var list = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber + ":" + BoxNumber);
                        string creator = list.FirstOrDefault().Creator;
                        if (!string.IsNullOrEmpty(creator))
                        {
                            if (RCommon.RedisOperation.Exists(creator))
                            {
                                var sessionUserList = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(creator);
                                if (sessionUserList.FirstOrDefault().str2 == BoxNumber)
                                {
                                    RCommon.RedisOperation.Del(creator);
                                }
                            }
                        }

                    }
                    if (RCommon.RedisOperation.Exists("ReceiptReceivingBack:" + ReceiptNumber))
                    {
                        var listres = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceivingBack:" + ReceiptNumber);
                        listres.RemoveAll(c => c.str2 == BoxNumber);
                        RCommon.RedisOperation.SetList("ReceiptReceivingBack:" + ReceiptNumber, listres);
                    }
                    var ReceiptDetails = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber).Where(a => a.str2 == BoxNumber);
                    RCommon.RedisOperation.SetList("Back:" + ReceiptNumber + ":" + BoxNumber, ReceiptDetails);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }


        /// <summary>
        /// ABC计数清空重扫
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <returns></returns>
        public string ClearScanSKUDetail(string AsnNumber, string GoodsType)
        {
            string msg = "";
            try
            {
                if (RCommon.RedisOperation.Exists("ABC:" + AsnNumber + ":" + GoodsType + ":" + Session["Name"].ToString()))
                {
                    RCommon.RedisOperation.Del("ABC:" + AsnNumber + ":" + GoodsType + ":" + Session["Name"].ToString());
                }
            }
            catch (Exception ex)
            {
                msg = "-1";
            }
            return msg;

        }
        public string ClearScanSKU()
        {
            string msg = "";
            try
            {
                if (RCommon.RedisOperation.Exists("Back:SKUFloar:" + Session["Name"].ToString()))
                {
                    RCommon.RedisOperation.Del("Back:SKUFloar:" + Session["Name"].ToString());
                }
            }
            catch (Exception ex)
            {
                msg = "-1";
            }
            return msg;

        }
        /// <summary>
        /// 装箱选单页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>

        public ActionResult ASNListBack_CloseBox(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetASNList(CustomerID, WareHouseID, "");
            vm.AsnCollection = response;
            return View(vm);
        }
        /// <summary>
        /// 装箱选单页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ASNListBack_CloseBox(ReceiptViewModel vm, string Action)
        {
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetASNList(Session["CustomerID"].ToString(), Session["WareHouseIDs"].ToString(), vm.SearchCondition.ExternReceiptNumber);
            vm.AsnCollection = response;
            return View(vm);
        }
        /// <summary>
        /// 分SKU选单页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ASNListBack_SKU(ReceiptViewModel vm, string Action)
        {
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetASNList(Session["CustomerID"].ToString(), Session["WareHouseIDs"].ToString(), vm.SearchCondition.ExternReceiptNumber);
            vm.AsnCollection = response;
            return View(vm);
        }

        /// <summary>
        /// 退货上架页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        public ActionResult ASNListBack_ReceiptReceiving(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetReceiptList(CustomerID, WareHouseID, "");
            vm.ReceiptCollection = response;
            return View(vm);
        }
        /// <summary>
        /// 检查绑定的是否是箱号
        /// </summary>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public string CheckBindBoxNumber(string BoxNumber)
        {
            string msg = "";
            try
            {
                msg = new ReceiptManagementService().CheckBindBoxNumber(Session["CustomerID"].ToString(), BoxNumber);
            }
            catch (Exception ex)
            {
                msg = "-1";
            }
            return msg;
        }

        /// <summary>
        /// 退货上架页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ASNListBack_ReceiptReceiving(ReceiptViewModel vm, string Action)
        {
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetReceiptList(Session["CustomerID"].ToString(), Session["WareHouseIDs"].ToString(), vm.SearchCondition.ExternReceiptNumber);
            vm.ReceiptCollection = response;
            return View(vm);
        }

        /// <summary>
        /// 分SKU扫描箱号
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="SKU"></param>
        /// <param name="GoodsType"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        //public JsonResult ScanBox_SKU(string AsnNumber, string SKU, string GoodsType, string BoxNumber, int No)
        //{
        //    List<SKURF> lists = new List<SKURF>();
        //    long Qtys = 0;
        //    string msg = "";
        //    try
        //    {
        //        if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + SKU + ":" + GoodsType + ":" + BoxNumber))
        //        {
        //            lists = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + SKU + ":" + GoodsType + ":" + BoxNumber);
        //        }
        //        lists.Add(new SKURF()
        //        {
        //            AsnNumber = AsnNumber,
        //            SKU = SKU,
        //            BoxNumber = BoxNumber,
        //            GoodsType = GoodsType,
        //            Qty = 1,
        //            No = No,
        //            Creator = Session["Name"].ToString(),
        //            CreateTime = DateTime.Now
        //        });
        //        RCommon.RedisOperation.SetList("ScanBox:" + AsnNumber + ":" + SKU + ":" + GoodsType + ":" + BoxNumber, lists);
        //        //set之后查一遍防止并发
        //        if (lists.Where(c => c.Creator == Session["Name"].ToString()).Count() == RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + SKU + ":" + GoodsType + ":" + BoxNumber).
        //            Where(a => a.Creator == Session["Name"].ToString()).Count())
        //        {
        //            msg = "1";
        //            Qtys = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + SKU + ":" + GoodsType + ":" + BoxNumber).Sum(c => c.Qty);
        //            RCommon.RedisOperation.Del("CheckScanBoxNumber:" + AsnNumber + ":" + Session["Name"].ToString());
        //            return Json(new { Code = "1", data = Qtys });
        //        }
        //        else
        //        {
        //            ScanBox_SKU(AsnNumber, SKU, GoodsType, BoxNumber, No);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Code = "-1", data = Qtys });
        //    }
        //    return Json(new { Code = "0", data = Qtys });
        //}


        /// <summary>
        /// 分SKU扫描箱号
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="SKU"></param>
        /// <param name="GoodsType"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public JsonResult ScanBox_SKU(string AsnNumber, string SKU, string GoodsType, string BoxNumber, int No)
        {
            List<SKURF> lists = new List<SKURF>();
            long Qtys = 0;
            string msg = "";
            try
            {
                if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + BoxNumber))
                {
                    lists = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + BoxNumber);
                }
                lists.Add(new SKURF()
                {
                    AsnNumber = AsnNumber,
                    SKU = SKU,
                    BoxNumber = BoxNumber,
                    GoodsType = GoodsType,
                    Qty = 1,
                    No = No,
                    Creator = Session["Name"].ToString(),
                    CreateTime = DateTime.Now
                });
                RCommon.RedisOperation.SetList("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + BoxNumber, lists);
                //set之后查一遍防止并发
                if (lists.Where(c => c.Creator == Session["Name"].ToString()).Count() == RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + BoxNumber).
                    Where(a => a.Creator == Session["Name"].ToString()).Count())
                {
                    msg = "1";
                    Qtys = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + BoxNumber).Sum(c => c.Qty);
                    RCommon.RedisOperation.Del("CheckScanBoxNumber:" + AsnNumber + ":" + Session["Name"].ToString());
                    return Json(new { Code = "1", data = Qtys });
                }
                else
                {
                    ScanBox_SKU(AsnNumber, SKU, GoodsType, BoxNumber, No);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = Qtys });
            }
            return Json(new { Code = "0", data = Qtys });
        }

        /// <summary>
        /// 装箱扫描箱号
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="SKU"></param>
        /// <param name="GoodsType"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public JsonResult ScanBox_SKUCloseBox(string AsnNumber, string GoodsType, string BoxNumber)
        {
            List<SKURF> lists = new List<SKURF>();
            List<SKURF> listrf = new List<SKURF>();
            List<SKURF> listboxnumber = new List<SKURF>();
            List<ASNDetail> listasndetail = new List<ASNDetail>();
            ReceiptViewModel vm = new ReceiptViewModel();
            try
            {
                if (RCommon.RedisOperation.Exists("CloseBox:" + AsnNumber + ":" + Session["Name"].ToString()))
                {
                    //string SKU = RCommon.RedisOperation.GetList<List<SKURF>>("CloseBox:" + AsnNumber + ":" + Session["Name"].ToString()).Select(c => c.SKU).FirstOrDefault();
                    foreach (var itemgoodstype in vm.ProductLevel.Where(c => c.Text != GoodsType))
                    {
                        if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + BoxNumber))
                        {
                            return Json(new { Code = "2", data = itemgoodstype.Text });//同一箱品级不一样不允许
                        }
                    }

                    //if (RCommon.RedisOperation.Exists("SKU:" + AsnNumber))
                    //{
                    //    listasndetail = RCommon.RedisOperation.GetList<List<ASNDetail>>("SKU:" + AsnNumber);
                    //    foreach (var itemgoodstype in vm.ProductLevel)
                    //    {
                    //        foreach (var item in listasndetail.Where(c => c.SKU != SKU).GroupBy(a => a.SKU).Select(c => new ASNDetail() { SKU = c.Key }))
                    //        {
                    //            if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + item.SKU + ":" + itemgoodstype.Text + ":" + BoxNumber))
                    //            {
                    //                return Json(new { Code = "3", data = item.SKU });//同一箱只能放一种SKU
                    //            }
                    //        }
                    //    }
                    //}



                    if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + BoxNumber))
                    {
                        lists = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + BoxNumber);
                    }
                    listrf = RCommon.RedisOperation.GetList<List<SKURF>>("CloseBox:" + AsnNumber + ":" + Session["Name"].ToString());
                    foreach (var item in listrf)
                    {
                        item.BoxNumber = BoxNumber;
                    }
                    lists.AddRange(listrf);

                    if (RCommon.RedisOperation.Exists("ScanBoxBoxNumberList:" + AsnNumber))
                    {
                        listboxnumber = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBoxBoxNumberList:" + AsnNumber);
                        if (listboxnumber.Find(c => c.BoxNumber == BoxNumber) == null)
                        {
                            listboxnumber.Add(new SKURF()
                            {
                                AsnNumber = AsnNumber,
                                BoxNumber = BoxNumber,
                                GoodsType = GoodsType,
                                CreateTime = DateTime.Now,
                                Creator = Session["Name"].ToString()
                            });
                            RCommon.RedisOperation.SetList("ScanBoxBoxNumberList:" + AsnNumber, listboxnumber);
                        }
                    }
                    else
                    {
                        listboxnumber.Add(new SKURF()
                        {
                            AsnNumber = AsnNumber,
                            BoxNumber = BoxNumber,
                            GoodsType = GoodsType,
                            CreateTime = DateTime.Now,
                            Creator = Session["Name"].ToString()
                        });
                        RCommon.RedisOperation.SetList("ScanBoxBoxNumberList:" + AsnNumber, listboxnumber);
                    }
                    //set完之后再get防止并发
                    if (listboxnumber.Where(c => c.Creator == Session["Name"].ToString()).Count() != RCommon.RedisOperation.GetList<List<SKURF>>("ScanBoxBoxNumberList:" + AsnNumber).Where(c => c.Creator == Session["Name"].ToString()).Count())
                    {
                        ScanBox_SKUCloseBox(AsnNumber, GoodsType, BoxNumber);
                    }
                    else
                    {
                        RCommon.RedisOperation.SetList("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + BoxNumber, lists);
                        RCommon.RedisOperation.Del("CloseBox:" + AsnNumber + ":" + Session["Name"].ToString());
                    }
                    return Json(new { Code = "1" });
                }
                else
                {
                    return Json(new { Code = "0" });
                }


            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1" });
            }

        }

        public ActionResult AddAdjustMent(string adjustmentnumber, string CustomerID, string WareHouseName, string WareHouseID, string StoreCode)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            ViewBag.AdjustMentNumber = adjustmentnumber;
            ViewBag.StoreCode = StoreCode;
            var response = new ReceiptManagementService().GetAdjustmentDetailList(adjustmentnumber);
            vm.AdjustmentDetailCollection = response;
            return View(vm);
        }
        /// <summary>
        /// RF批量移库新增界面
        /// </summary>
        /// <param name="adjustmentnumber"></param>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        public ActionResult AddAdjustMentBatch(string adjustmentnumber, string CustomerID, string WareHouseName, string WareHouseID, string StoreCode)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            ViewBag.AdjustMentNumber = adjustmentnumber;
            ViewBag.StoreCode = StoreCode;
            var response = new ReceiptManagementService().GetAdjustmentDetailList(adjustmentnumber);
            vm.AdjustmentDetailCollection = response;
            return View(vm);
        }
        /// <summary>
        /// RF盘点明细页面
        /// </summary>
        /// <param name="PDNumber"></param>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        public ActionResult WarehouseCheckDetail(string PDNumber, string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            ViewBag.PDNumber = PDNumber;
            ViewBag.ScanTotalQty = 0;
            if (!RCommon.RedisOperation.Exists("PD:" + PDNumber))
            {
                List<WarehouseCheckDetail> warehouseCheckDetails = new List<WarehouseCheckDetail>();
                var response = new ReceiptManagementService().GetWarehouseCheckDetailList(PDNumber);
                warehouseCheckDetails = response.ToList();
                RCommon.RedisOperation.SetList("PD:" + PDNumber, warehouseCheckDetails);
                ViewBag.TotalQty = Convert.ToInt32(response.Sum(a => a.CheckQty));
            }
            else
            {
                ViewBag.TotalQty = Convert.ToInt32(RCommon.RedisOperation.GetList<List<WarehouseCheckDetail>>("PD:" + PDNumber).Sum(a => a.CheckQty));
            }
            if (RCommon.RedisOperation.Exists("PD:" + PDNumber + ":" + Session["Name"].ToString()))
            {
                ViewBag.ScanTotalQty = RCommon.RedisOperation.GetList<List<WarehouseCheckDetail>>("PD:" + PDNumber + ":" + Session["Name"].ToString()).
                    Sum(a => a.ActualQty);
            }
            return View(vm);
        }
        /// <summary>
        /// RF盘点扫描库位校验
        /// </summary>
        /// <param name="PDNumber"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        public JsonResult CheckLocationRedisCheckWarehouse(string PDNumber, string Location)
        {
            List<WarehouseCheckDetail> warehouseCheckDetails = new List<WarehouseCheckDetail>();
            int LocationQty = 0;
            try
            {
                if (RCommon.RedisOperation.Exists("PD:" + PDNumber))
                {
                    warehouseCheckDetails = RCommon.RedisOperation.GetList<List<WarehouseCheckDetail>>("PD:" + PDNumber);
                    if (warehouseCheckDetails.Where(c => c.Location == Location).Count() > 0)
                    {
                        if (RCommon.RedisOperation.Exists("PD:" + PDNumber + ":" + Session["Name"].ToString()))
                        {
                            LocationQty = Convert.ToInt32(RCommon.RedisOperation.GetList<List<WarehouseCheckDetail>>("PD:" + PDNumber + ":" + Session["Name"].ToString()).
                                Where(c => c.Location == Location).Sum(a => a.ActualQty));
                        }
                        return Json(new { Code = "1", data = LocationQty });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = LocationQty });
            }

            return Json(new { Code = "0", data = LocationQty });

        }
        /// <summary>
        /// RF出库交接
        /// </summary>
        /// <param name="StoreCode"></param>
        /// <param name="OrderTime"></param>
        /// <param name="OrderType"></param>
        /// <returns></returns>
        public JsonResult GetPackageDtailRF(string StoreCode, string OrderTime, string OrderType)
        {
            List<PackageInfo> list = new List<PackageInfo>();
            try
            {
                if (RCommon.RedisOperation.Exists("OUT:" + StoreCode + ":" + OrderTime + ":" + OrderType))
                {
                    list = RCommon.RedisOperation.GetList<List<PackageInfo>>("OUT:" + StoreCode + ":" + OrderTime + ":" + OrderType);
                }
                else
                {
                    list = new ReceiptManagementService().GetPackageDtailRF(StoreCode, OrderTime, OrderType).ToList();
                    RCommon.RedisOperation.SetList("OUT:" + StoreCode + ":" + OrderTime + ":" + OrderType, list);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = list });
            }
            if (list.Count() > 0)
            {
                return Json(new { Code = "1", data = list });
            }
            return Json(new { Code = "0", data = list });
        }
        /// <summary>
        /// RF出库交接重置
        /// </summary>
        /// <param name="StoreCode"></param>
        /// <param name="OrderTime"></param>
        /// <param name="OrderType"></param>
        /// <returns></returns>
        public JsonResult ClearPackageDtailRF(string StoreCode, string OrderTime, string OrderType)
        {
            List<PackageInfo> list = new List<PackageInfo>();
            try
            {
                if (RCommon.RedisOperation.Exists("OUT:" + StoreCode + ":" + OrderTime + ":" + OrderType))
                {
                    RCommon.RedisOperation.Del("OUT:" + StoreCode + ":" + OrderTime + ":" + OrderType);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = list });
            }
            return Json(new { Code = "1", data = list });

        }
        /// <summary>
        /// RF查看出库交接未扫描的箱子
        /// </summary>
        /// <param name="StoreCode"></param>
        /// <param name="OrderTime"></param>
        /// <param name="OrderType"></param>
        /// <returns></returns>
        public JsonResult CheckPackageDetailNoScan(string StoreCode, string OrderTime, string OrderType)
        {
            List<PackageInfo> list = new List<PackageInfo>();
            try
            {
                if (RCommon.RedisOperation.Exists("OUT:" + StoreCode + ":" + OrderTime + ":" + OrderType))
                {
                    list = RCommon.RedisOperation.GetList<List<PackageInfo>>("OUT:" + StoreCode + ":" + OrderTime + ":" + OrderType).Where(c => c.Status != 1).ToList();
                }

            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = list });
            }
            if (list.Count() > 0)
            {
                return Json(new { Code = "1", data = list });
            }
            else
            {
                return Json(new { Code = "0", data = list });
            }


        }

        /// <summary>
        /// RF出库交接扫描箱号
        /// </summary>
        /// <param name="StoreCode"></param>
        /// <param name="OrderTime"></param>
        /// <param name="OrderType"></param>
        /// <param name="PackageNumber"></param>
        /// <returns></returns>
        public JsonResult ScanPackageNumberRedis(string StoreCode, string OrderTime, string OrderType, string PackageNumber)
        {
            List<PackageInfo> list = new List<PackageInfo>();

            try
            {
                if (RCommon.RedisOperation.Exists("OUT:" + StoreCode + ":" + OrderTime + ":" + OrderType))
                {
                    list = RCommon.RedisOperation.GetList<List<PackageInfo>>("OUT:" + StoreCode + ":" + OrderTime + ":" + OrderType);
                    if (list.Where(c => c.PackageNumber == PackageNumber).Count() == 0)
                    {
                        return Json(new { Code = "2", data = list });//箱号不存在
                    }
                    foreach (var item in list)
                    {
                        if (item.PackageNumber == PackageNumber)
                        {
                            if (item.Status == 1)
                            {
                                return Json(new { Code = "3", data = list });//已扫描
                            }
                            else
                            {
                                item.Status = 1;
                                item.UpdateTime = DateTime.Now;
                                item.Updator = Session["Name"].ToString();
                                RCommon.RedisOperation.SetList("OUT:" + StoreCode + ":" + OrderTime + ":" + OrderType, list);
                                return Json(new { Code = "1", data = list });//正常
                            }
                        }
                    }
                }
                else
                {
                    return Json(new { Code = "4", data = list });//没有查询到Epacklist
                }

            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = list });
            }
            return Json(new { Code = "0", data = list });
        }
        /// <summary>
        /// RF盘点扫描SKU校验
        /// </summary>
        /// <param name="PDNumber"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public string CheckSKURedisCheckWarehouse(string PDNumber, string SKU)
        {
            List<WarehouseCheckDetail> warehouseCheckDetails = new List<WarehouseCheckDetail>();
            string msg = "";
            try
            {
                if (RCommon.RedisOperation.Exists("PD:" + PDNumber))
                {
                    warehouseCheckDetails = RCommon.RedisOperation.GetList<List<WarehouseCheckDetail>>("PD:" + PDNumber);
                    if (warehouseCheckDetails.Where(c => c.SKU == SKU).Count() > 0)
                    {
                        msg = "1";
                    }
                }
            }
            catch (Exception ex)
            {
                msg = "-1";
            }
            return msg;


        }

        public ActionResult ReceiptGetAdjustMents(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            IEnumerable<WMSConfig> wmsCompany = new TBIZ.ConfigService().GetWMSCustomerConfigByCustomerID(CustomerID).Result;
            List<SelectListItem> stCompanty = new List<SelectListItem>();
            foreach (WMSConfig w in wmsCompany)
            {
                stCompanty.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            vm.CompanyCodeList = stCompanty;
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetAdjustmentList(CustomerID, "");
            vm.AdjustmentCollection = response;
            return View(vm);
        }
        /// <summary>
        /// RF批量移库主界面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        public ActionResult ReceiptGetAdjustMentBatchs(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            IEnumerable<WMSConfig> wmsCompany = new TBIZ.ConfigService().GetWMSCustomerConfigByCustomerID(CustomerID).Result;
            List<SelectListItem> stCompanty = new List<SelectListItem>();
            foreach (WMSConfig w in wmsCompany)
            {
                stCompanty.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            vm.CompanyCodeList = stCompanty;
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetAdjustmentListBatch(CustomerID, "");
            vm.AdjustmentCollection = response;
            return View(vm);
        }
        /// <summary>
        /// RF盘点主界面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        public ActionResult ReceiptGetPDList(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetPDList("", Session["CustomerID"].ToString());
            vm.WarehouseCheckCollection = response;
            return View(vm);
        }
        /// <summary>
        /// 出库交接
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        public ActionResult ReceiptGetOutList(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            IEnumerable<WMSConfig> wms = null;
            IEnumerable<WMSConfig> wmsCompany = new TBIZ.ConfigService().GetWMSCustomerConfig().Result;
            wms = ApplicationConfigHelper.GetWMS_Config("OrderType");
            List<SelectListItem> st = new List<SelectListItem>();
            List<SelectListItem> stCompanty = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            foreach (WMSConfig w in wmsCompany)
            {
                stCompanty.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            vm.OrderTypeList = st;
            vm.CompanyCodeList = stCompanty;
            vm.orderSearchCondition = new OrderSearchCondition();
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            return View(vm);
        }
        /// <summary>
        /// RF库存查询
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        public ActionResult ReceiptGetInventory(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            return View(vm);
        }
        /// <summary>
        /// RF获取库存信息
        /// </summary>
        /// <param name="ScanNumber"></param>
        /// <returns></returns>
        public JsonResult GetInventoryByScanRF(string ScanNumber)
        {
            List<Inventorys> lists = new List<Inventorys>();
            try
            {
                lists = new ReceiptManagementService().GetInventoryList(ScanNumber, Session["CustomerID"].ToString()).ToList();
                if (lists.Count() > 0)
                {
                    return Json(new { Code = 1, data = lists });
                }
                else
                {
                    return Json(new { Code = 0, data = lists });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, data = lists });
            }
        }
        /// <summary>
        /// RF完成盘点
        /// </summary>
        /// <param name="PDNumber"></param>
        /// <returns></returns>
        public string WarehouseCheckOverRF(string PDNumber)
        {
            string msg = "";
            try
            {
                var response = new ReceiptManagementService().WarehouseCheckOverRF(PDNumber);
                if (response)
                {
                    RCommon.RedisOperation.Del("PD:" + PDNumber);

                }
                else
                {
                    msg = "失败";
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
        /// <summary>
        /// RF盘点提交数据
        /// </summary>
        /// <param name="PDNumber"></param>
        /// <returns></returns>
        public string WarehouseCheckCompleteRF(string PDNumber)
        {
            List<WarehouseCheckDetail> warehouseCheckDetails = new List<WarehouseCheckDetail>();
            string msg = "";
            try
            {
                if (RCommon.RedisOperation.Exists("PD:" + PDNumber + ":" + Session["Name"].ToString()))
                {
                    warehouseCheckDetails = RCommon.RedisOperation.GetList<List<WarehouseCheckDetail>>("PD:" + PDNumber + ":" + Session["Name"].ToString());
                    var response = new ReceiptManagementService().InsertWarehouseCheckScan(warehouseCheckDetails);
                    if (response)
                    {
                        RCommon.RedisOperation.Del("PD:" + PDNumber + ":" + Session["Name"].ToString());
                        msg = "1";
                    }
                }
                else
                {
                    msg = "0";
                }
            }
            catch (Exception ex)
            {
                msg = "-1";

            }

            return msg;

        }

        [HttpPost]
        public ActionResult ReceiptGetAdjustMents(ReceiptViewModel vm, string Action)
        {
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            IEnumerable<WMSConfig> wmsCompany = new TBIZ.ConfigService().GetWMSCustomerConfig().Result;
            List<SelectListItem> stCompanty = new List<SelectListItem>();
            foreach (WMSConfig w in wmsCompany)
            {
                stCompanty.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            vm.CompanyCodeList = stCompanty;
            var response = new ReceiptManagementService().GetAdjustmentList(Session["CustomerID"].ToString(), vm.AdjustMentSearchCondition.AdjustmentNumber);
            vm.AdjustmentCollection = response;
            return View(vm);
        }
        [HttpPost]
        public ActionResult ReceiptGetPDList(ReceiptViewModel vm, string Action)
        {
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetPDList(vm.WarehouseCheckSearchCondition.ExternNumber, Session["CustomerID"].ToString());
            vm.WarehouseCheckCollection = response;
            return View(vm);
        }
        [HttpPost]
        public ActionResult ReceiptReceivingMain(ReceiptViewModel vm, string Action)
        {

            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetReceiptList(Session["CustomerID"].ToString(), Session["WareHouseIDs"].ToString(), vm.SearchCondition.ExternReceiptNumber);
            vm.ReceiptCollection = response;
            return View(vm);
        }

        //
        // GET: /Receipt/
        public ActionResult Index(string CustomerID, string WareHouseName, string WareHouseID, string ReceiptaNumber)
        {
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.ReceiptaNumber = ReceiptaNumber;
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var responsestatus = new ReceiptManagementService().GetReceipt(ReceiptaNumber);
            if (responsestatus.Status != 1 && responsestatus.Status != 3)
            {
                return RedirectToAction("ReceiptReceivingMain", "Receipt", new { CustomerID = CustomerID, WareHouseName = WareHouseName, WareHouseID = WareHouseID });
            }
            if (!RCommon.RedisOperation.Exists(ReceiptaNumber))
            {
                List<ReceiptDetail> receiptDetails = new List<ReceiptDetail>();
                var response = new ReceiptManagementService().GetReceiptDetailList(ReceiptaNumber, CustomerID, WareHouseName);
                receiptDetails = response.ToList();
                RCommon.RedisOperation.SetList(ReceiptaNumber, receiptDetails);
            }

            return View();
        }


        /// <summary>
        /// 分款页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <param name="ReceiptaNumber"></param>
        /// <returns></returns>
        public ActionResult ArticleIndex(string CustomerID, string WareHouseName, string WareHouseID, string AsnNumber)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.AsnNumber = AsnNumber;
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            //var responsestatus = new ReceiptManagementService().GetASN(AsnNumber);
            //if (responsestatus.Status != 1 && responsestatus.Status != 3)
            //{
            //    return RedirectToAction("ASNListBack_Article", "Receipt", new { CustomerID = CustomerID, WareHouseName = WareHouseName, WareHouseID = WareHouseID });
            //}
            if (!RCommon.RedisOperation.Exists("Article:" + AsnNumber))
            {
                List<ASNDetail> ASNDetails = new List<ASNDetail>();
                var response = new ReceiptManagementService().GetAsnDetailListArticle(AsnNumber, CustomerID, WareHouseName);
                ASNDetails = response.ToList();
                RCommon.RedisOperation.SetList("Article:" + AsnNumber, ASNDetails);
            }

            return View(vm);
        }
        /// <summary>
        /// 分SKU页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <param name="ReceiptaNumber"></param>
        /// <returns></returns>
        public ActionResult SKUIndex(string CustomerID, string WareHouseName, string WareHouseID, string AsnNumber)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.AsnNumber = AsnNumber;
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var responsestatus = new ReceiptManagementService().GetASN(AsnNumber);
            //if (responsestatus.Status != 1 && responsestatus.Status != 3)
            //{
            //    return RedirectToAction("ASNListBack_SKU", "Receipt", new { CustomerID = CustomerID, WareHouseName = WareHouseName, WareHouseID = WareHouseID });
            //}
            if (!RCommon.RedisOperation.Exists("SKU:" + AsnNumber))
            {
                List<ASNDetail> ASNDetails = new List<ASNDetail>();
                var response = new ReceiptManagementService().GetAsnDetailList(AsnNumber, CustomerID, WareHouseName);
                ASNDetails = response.ToList();
                RCommon.RedisOperation.SetList("SKU:" + AsnNumber, ASNDetails);
            }

            return View(vm);
        }
        /// <summary>
        /// ABC计数页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <param name="AsnNumber"></param>
        /// <returns></returns>
        public ActionResult ABCIndex(string CustomerID, string WareHouseName, string WareHouseID, string AsnNumber)
        {
            //List<string> keys = RCommon.RedisOperation.Like("pear?").ToList();
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.AsnNumber = AsnNumber;
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var responsestatus = new ReceiptManagementService().GetASN(AsnNumber);
            if (RCommon.RedisOperation.Exists("ABC:" + AsnNumber))
            {
                List<ASNDetail> ASNDetails = new List<ASNDetail>();
                var response = new ReceiptManagementService().GetAsnDetailList(AsnNumber, CustomerID, WareHouseName);
                ASNDetails = response.ToList();
                RCommon.RedisOperation.SetList("ABC:" + AsnNumber, ASNDetails);
            }


            return View(vm);
        }

        /// <summary>
        /// 扫ABC redis+1
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="SKU"></param>
        /// <param name="GoodsType"></param>
        /// <returns></returns>
        public JsonResult ScanSKUABC(string AsnNumber, string SKU, string GoodsType)
        {
            List<SKURF> lists = new List<SKURF>();
            try
            {
                if (RCommon.RedisOperation.Exists("ABC:" + AsnNumber + ":" + GoodsType + ":" + Session["Name"].ToString()))
                {
                    lists = RCommon.RedisOperation.GetList<List<SKURF>>("ABC:" + AsnNumber + ":" + GoodsType + ":" + Session["Name"].ToString());

                }
                lists.Add(new SKURF()
                {
                    AsnNumber = AsnNumber,
                    SKU = SKU,
                    GoodsType = GoodsType,
                    Qty = 1,
                    Creator = Session["Name"].ToString(),
                    CreateTime = DateTime.Now
                });
                RCommon.RedisOperation.SetList("ABC:" + AsnNumber + ":" + GoodsType + ":" + Session["Name"].ToString(), lists);
                return Json(new { Code = "1", data = RCommon.RedisOperation.GetList<List<SKURF>>("ABC:" + AsnNumber + ":" + GoodsType + ":" + Session["Name"].ToString()).Sum(c => c.Qty) });
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1" });
            }
        }
        /// <summary>
        /// 扫ABC 提交
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="SKU"></param>
        /// <param name="GoodsType"></param>
        /// <returns></returns>
        public JsonResult ScanSKUABCSubmit(string AsnNumber, string GoodsType, string BoxNumber)
        {
            List<SKURF> lists = new List<SKURF>();
            List<SKURF> listall = new List<SKURF>();
            List<ASNDetail> listasndetails = new List<ASNDetail>();
            List<ASNDetail> listboxs = new List<ASNDetail>();
            ReceiptViewModel vm = new ReceiptViewModel();
            try
            {
                if (RCommon.RedisOperation.Exists("ABC:" + AsnNumber + ":" + GoodsType + ":" + Session["Name"].ToString()))
                {
                    lists = RCommon.RedisOperation.GetList<List<SKURF>>("ABC:" + AsnNumber + ":" + GoodsType + ":" + Session["Name"].ToString());
                    listall = lists.GroupBy(c => new { AsnNumber = c.AsnNumber, SKU = c.SKU, GoodsType = c.GoodsType }).
                        Select(a => new SKURF() { AsnNumber = a.Key.AsnNumber, SKU = a.Key.SKU, GoodsType = a.Key.GoodsType, Qty = a.Sum(x => x.Qty) }).ToList();
                    foreach (var item in listall)
                    {
                        listasndetails.Add(new ASNDetail()
                        {
                            ASNNumber = item.AsnNumber,
                            SKU = item.SKU,
                            GoodsName = item.SKU,
                            GoodsType = item.GoodsType,
                            QtyReceived = item.Qty,
                            BoxNumber = BoxNumber,
                            Creator = Session["Name"].ToString(),
                            CreateTime = DateTime.Now
                        });
                    }

                }


                if (listasndetails.Count() > 0)
                {
                    if (RCommon.RedisOperation.Exists("ABCBoxNumber:" + AsnNumber))
                    {
                        listboxs = RCommon.RedisOperation.GetList<List<ASNDetail>>("ABCBoxNumber:" + AsnNumber);
                    }
                    listboxs.AddRange(listasndetails);
                    RCommon.RedisOperation.SetList("ABCBoxNumber:" + AsnNumber, listboxs);
                    if (listboxs.Where(c => c.Creator == Session["Name"].ToString()).Count() == RCommon.RedisOperation.GetList<List<ASNDetail>>("ABCBoxNumber:" + AsnNumber).Where(c => c.Creator == Session["Name"].ToString()).Count())
                    {
                        var result = new ReceiptManagementService().InsertAsnDetailScanABC(listasndetails);
                        if (result)
                        {
                            RCommon.RedisOperation.Del("ABC:" + AsnNumber + ":" + GoodsType + ":" + Session["Name"].ToString());
                        }
                    }
                    else
                    {
                        ScanSKUABCSubmit(AsnNumber, GoodsType, BoxNumber);
                    }


                    return Json(new { Code = "1" });
                }
                else
                {
                    return Json(new { Code = "0" });
                }


            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1" });
            }
        }

        /// <summary>
        /// 装箱页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <param name="ReceiptaNumber"></param>
        /// <returns></returns>
        public ActionResult CloseBoxIndex(string CustomerID, string WareHouseName, string WareHouseID, string AsnNumber)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.AsnNumber = AsnNumber;
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            //var responsestatus = new ReceiptManagementService().GetASN(AsnNumber);
            //if (responsestatus.Status != 1 && responsestatus.Status != 3)
            //{
            //    return RedirectToAction("ASNListBack_CloseBox", "Receipt", new { CustomerID = CustomerID, WareHouseName = WareHouseName, WareHouseID = WareHouseID });
            //}
            if (!RCommon.RedisOperation.Exists("CloseBox:" + AsnNumber))
            {
                List<ASNDetail> ASNDetails = new List<ASNDetail>();
                var response = new ReceiptManagementService().GetAsnDetailList(AsnNumber, CustomerID, WareHouseName);
                ASNDetails = response.ToList();
                RCommon.RedisOperation.SetList("CloseBox:" + AsnNumber, ASNDetails);
            }

            return View(vm);
        }
        /// <summary>
        /// 退货上架页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <param name="ReceiptaNumber"></param>
        /// <returns></returns>
        public ActionResult ReceiptReceivingIndex(string CustomerID, string WareHouseName, string WareHouseID, string ReceiptNumber)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.ReceiptaNumber = ReceiptNumber;
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            ViewBag.UserName = Session["Name"].ToString();
            var responsestatus = new ReceiptManagementService().GetReceipt(ReceiptNumber);
            if (responsestatus.Status != 1 && responsestatus.Status != 3)
            {
                return RedirectToAction("ASNListBack_ReceiptReceiving", "Receipt", new { CustomerID = CustomerID, WareHouseName = WareHouseName, WareHouseID = WareHouseID });
            }
            if (!RCommon.RedisOperation.Exists("Back:" + ReceiptNumber))
            {
                List<ReceiptDetail> ReceiptDetails = new List<ReceiptDetail>();
                var response = new ReceiptManagementService().GetReceiptDetailList_CustomerID(ReceiptNumber, CustomerID, WareHouseName);

                ReceiptDetails = response.ToList();
                ReceiptDetails.ForEach(c => { c.Creator = null; c.CreateTime = null; c.Updator = Session["Name"].ToString(); c.UpdateTime = DateTime.Now; });
                RCommon.RedisOperation.SetList("Back:" + ReceiptNumber, ReceiptDetails);
                foreach (var item in ReceiptDetails.GroupBy(c => c.str2).Select(a => new ReceiptDetail() { str2 = a.Key }))
                {
                    if (!RCommon.RedisOperation.Exists("Back:" + ReceiptNumber + ":" + item.str2))
                    {
                        RCommon.RedisOperation.SetList("Back:" + ReceiptNumber + ":" + item.str2, ReceiptDetails.Where(a => a.str2 == item.str2));
                    }
                }

            }

            return View(vm);
        }
        /// <summary>
        /// 查看之前是否有没有扫完的箱子
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public JsonResult CheckBoxNumberDiff(string ReceiptNumber, string BoxNumber)
        {

            List<ReceiptDetail> receiptDetails = new List<ReceiptDetail>();
            ReceiptDetail receiptBoxs = new ReceiptDetail();
            try
            {
                if (RCommon.RedisOperation.Exists(ReceiptNumber))
                {
                    receiptDetails = RCommon.RedisOperation.GetList<List<ReceiptDetail>>(ReceiptNumber);
                    foreach (var item in receiptDetails.Where(c => c.str2 != BoxNumber))
                    {
                        if (RCommon.RedisOperation.Exists(ReceiptNumber + ":" + item.str2))
                        {
                            receiptBoxs = RCommon.RedisOperation.GetList<ReceiptDetail>(ReceiptNumber + ":" + item.str2);
                            if (receiptBoxs.Creator == Session["Name"].ToString())
                            {
                                return Json(new { Code = 1, data = item.str2 });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, data = ex.Message });
            }

            return Json(new { Code = 0, data = "" });
        }
        /// <summary>
        /// 查看之前是否有没有扫完的箱子退货
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public JsonResult CheckBoxNumberDiffBack(string ReceiptNumber, string BoxNumber)
        {

            List<ReceiptDetail> receiptDetails = new List<ReceiptDetail>();
            List<ReceiptDetail> receiptBoxs = new List<ReceiptDetail>();
            try
            {
                if (RCommon.RedisOperation.Exists("Back:" + ReceiptNumber))
                {
                    receiptDetails = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber);
                    foreach (var item in receiptDetails.Where(c => c.str2 != BoxNumber))
                    {
                        if (RCommon.RedisOperation.Exists("Back:" + ReceiptNumber + ":" + item.str2))
                        {
                            receiptBoxs = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber + ":" + item.str2);
                            if (receiptBoxs.Select(c => c.Creator).FirstOrDefault() == Session["Name"].ToString())
                            {
                                return Json(new { Code = 1, data = item.str2 });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, data = ex.Message });
            }

            return Json(new { Code = 0, data = "" });
        }
        public string CheckBoxNumber(string ReceiptNumber, string BoxNumber)
        {
            string msg = "";
            List<ReceiptDetail> receiptDetails = new List<ReceiptDetail>();
            if (RCommon.RedisOperation.Exists(ReceiptNumber))
            {
                receiptDetails = RCommon.RedisOperation.GetList<List<ReceiptDetail>>(ReceiptNumber);
                if (receiptDetails.Where(c => c.str2 == BoxNumber).Count() > 0)
                {
                    msg = receiptDetails.Where(c => c.str2 == BoxNumber).Select(a => a.QtyExpected).FirstOrDefault().ToString();
                }
            }
            return msg;
        }
        /// <summary>
        /// 退货仓检查是否是箱号
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public JsonResult CheckBoxNumberBack(string ReceiptNumber, string BoxNumber)
        {
            string msg = "";
            ReceiptReceiving receiptDetails = new ReceiptReceiving();
            if (RCommon.RedisOperation.Exists("Back:" + ReceiptNumber + ":" + BoxNumber))
            {
                try
                {
                    receiptDetails = new ReceiptManagementService().GetLocationByBoxNumber(Session["CustomerID"].ToString(), BoxNumber, ReceiptNumber);
                    if (receiptDetails == null)
                    {
                        //msg = "YES";
                        return Json(new { Code = "1", data = receiptDetails });
                    }
                    else
                    {
                        //msg = response.Location;
                        return Json(new { Code = "2", data = receiptDetails });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Code = "-1", data = receiptDetails });
                }

            }

            return Json(new { Code = "0", data = receiptDetails });
        }

        /// <summary>
        /// 退货仓检查是否是库存里的箱号
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public JsonResult CheckBoxNumberBackNew(string BoxNumber, string oldBoxNumber, string ReceiptNumber)
        {
            ReceiptReceiving receiptDetails = new ReceiptReceiving();
            try
            {
                receiptDetails = new ReceiptManagementService().CheckBoxNumberBackNew(Session["CustomerID"].ToString(), BoxNumber, oldBoxNumber, ReceiptNumber);
                if (receiptDetails.Location == "")
                {
                    return Json(new { Code = "0" });
                }
                else
                {
                    return Json(new { Code = "1", data = receiptDetails });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1" });
            }

        }
        public ActionResult IndexCD(string CustomerID, string WareHouseName, string WareHouseID)
        {

            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            return View();
        }
        public ActionResult IndexCDByReceiptNumber(string CustomerID, string WareHouseName, string WareHouseID)
        {

            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            return View();
        }
        public JsonResult selectList()
        {
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ProductLevel_" + Session["ProjectName"]);
            }
            catch (Exception)
            {
            }

            if (wms == null || wms.Count() <= 0)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ProductLevel");
            }
            return Json(wms);
        }
        //public ActionResult GetReceiptDetail(string ReceiptNumber)
        //{
        //    int ProjectID = 0;
        //    int.TryParse(Session["ProjectID"].ToString(), out ProjectID);
        //    int CustomerID = 0;
        //    int.TryParse(Session["CustomerID"].ToString(), out CustomerID);
        //    int WarehouseID = 0;
        //    int.TryParse(Session["WarehouseIDs"].ToString(), out WarehouseID);
        //    //获取存储过程名称
        //    var configs = ApplicationConfigHelper.GetWMS_ConfigType("RFReceiptReceivingScan",ProjectID,CustomerID,WarehouseID);

        //    //User user =  (User)Session[Constants.USER_INFO_KEY];
        //    IEnumerable<ReceiptDetail> recModelList;
        //    recModelList = new ReceiptManagementService().GetReceiptDetailListByProc(ReceiptNumber, Session["CustomerID"].ToString(), Session["WareHouseName"].ToString(),configs.First().Name);
        //    return Json(recModelList.ToList(), JsonRequestBehavior.AllowGet);
        //    //recModelList;
        //}
        /// <summary>
        /// 通过redis获取订单明细
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public string GetReceiptDetail(string ReceiptNumber)
        {
            List<ReceiptDetail> lists = new List<ReceiptDetail>();
            try
            {
                //CSRedisClient cs = new CSRedisClient(RedisConstants.RedisPath);
                if (RCommon.RedisOperation.Exists(ReceiptNumber))
                {
                    lists = RCommon.RedisOperation.GetList<List<ReceiptDetail>>(ReceiptNumber);
                }

            }
            catch (Exception ex)
            {

            }
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = Int32.MaxValue;
            return jsSerializer.Serialize(lists);
            //return Json(jsSerializer.Serialize(lists), JsonRequestBehavior.AllowGet);
            //recModelList;
        }
        /// <summary>
        /// 通过redis获取订单明细退货仓
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public string GetReceiptDetailBack(string ReceiptNumber)
        {
            List<ReceiptDetail> lists = new List<ReceiptDetail>();
            try
            {
                //CSRedisClient cs = new CSRedisClient(RedisConstants.RedisPath);
                if (RCommon.RedisOperation.Exists("Back:" + ReceiptNumber))
                {
                    lists = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber);
                }

            }
            catch (Exception ex)
            {

            }
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = Int32.MaxValue;
            return jsSerializer.Serialize(lists);
            //return Json(jsSerializer.Serialize(lists), JsonRequestBehavior.AllowGet);
            //recModelList;
        }

        /// <summary>
        /// 扫描SKU根据款号取放置顺序
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public string GetArticleNo(string AsnNumber, string SKU, string GoodsType)
        {
            List<ASNDetail> lists = new List<ASNDetail>();
            List<ArticleRF> listrf = new List<ArticleRF>();
            string Article = "";
            string ArticleNo = "";
            try
            {
                lists = RCommon.RedisOperation.GetList<List<ASNDetail>>("Article:" + AsnNumber);
                Article = lists.Where(c => c.SKU == SKU).Select(a => a.str10).FirstOrDefault();//先通过SKU拿到Article
                if (RCommon.RedisOperation.Exists("Article:" + AsnNumber + ":" + GoodsType))
                {
                    listrf = RCommon.RedisOperation.GetList<List<ArticleRF>>("Article:" + AsnNumber + ":" + GoodsType);
                    if (listrf.Where(c => c.ArticleNo == Article).Count() > 0)
                    {
                        ArticleNo = listrf.Where(c => c.ArticleNo == Article).Select(a => a.No).FirstOrDefault().ToString();
                    }
                    else
                    {

                        listrf.Add(new ArticleRF()
                        {
                            AsnNumber = AsnNumber,
                            ArticleNo = Article,
                            No = (listrf.Max(a => a.No) + 1),
                            GoodsType = GoodsType,
                            Creator = Session["Name"].ToString(),
                            CreateTime = DateTime.Now
                        });
                        //检查List数量防止并发
                        RCommon.RedisOperation.SetList("Article:" + AsnNumber + ":" + GoodsType, listrf);
                        ArticleNo = listrf.Max(a => a.No).ToString();
                        if (RCommon.RedisOperation.GetList<List<ArticleRF>>("Article:" + AsnNumber + ":" + GoodsType).Where(c => c.Creator == Session["Name"].ToString())
                            .Count() != listrf.Where(c => c.Creator == Session["Name"].ToString()).Count())
                        {
                            GetArticleNo(AsnNumber, SKU, GoodsType);
                        }

                    }
                }
                else
                {
                    listrf.Add(new ArticleRF()
                    {
                        AsnNumber = AsnNumber,
                        ArticleNo = Article,
                        No = 1,
                        GoodsType = GoodsType,
                        Creator = Session["Name"].ToString(),
                        CreateTime = DateTime.Now
                    });
                    //set redis之后再check一遍是否被其他人改了
                    RCommon.RedisOperation.SetList("Article:" + AsnNumber + ":" + GoodsType, listrf);
                    if (RCommon.RedisOperation.GetList<List<ArticleRF>>("Article:" + AsnNumber + ":" + GoodsType).Where(c => c.Creator == Session["Name"].ToString())
                             .Count() != listrf.Where(c => c.Creator == Session["Name"].ToString()).Count())
                    {
                        GetArticleNo(AsnNumber, SKU, GoodsType);
                    }
                    else
                    {

                        ArticleNo = "1";
                    }
                }
            }
            catch (Exception ex)
            {
                ArticleNo = "-1";
            }
            return ArticleNo;
        }
        /// <summary>
        /// 查看已绑箱明细
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="skuboxnumber"></param>
        /// <param name="GoodsType"></param>
        /// <returns></returns>
        public JsonResult CheckScanBoxDetail(string AsnNumber, string skuboxnumber, string GoodsType)
        {
            List<SKURF> lists = new List<SKURF>();
            List<ASNDetail> ASNDetails = new List<ASNDetail>();
            try
            {
                if (RCommon.RedisOperation.Exists("SKU:" + AsnNumber))
                {
                    ASNDetails = RCommon.RedisOperation.GetList<List<ASNDetail>>("SKU:" + AsnNumber);
                    foreach (var item in ASNDetails)
                    {
                        if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + skuboxnumber))
                        {
                            lists = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + skuboxnumber);
                            lists = lists.GroupBy(c => new { SKU = c.SKU, Creator = c.Creator }).Select(a => new SKURF() { SKU = a.Key.SKU, Creator = a.Key.Creator, Qty = a.Sum(x => x.Qty) }).ToList();
                            return Json(new { Code = "1", data = lists });
                        }
                    }
                }

                return Json(new { Code = "0", data = lists });
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = lists });
            }
        }
        /// <summary>
        /// 查看已绑箱明细
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="skuboxnumber"></param>
        /// <param name="GoodsType"></param>
        /// <returns></returns>
        public JsonResult CheckScanBoxDetailCloseBox(string AsnNumber, string skuboxnumber, string GoodsType)
        {
            List<SKURF> lists = new List<SKURF>();
            List<ASNDetail> ASNDetails = new List<ASNDetail>();
            try
            {
                if (RCommon.RedisOperation.Exists("CloseBox:" + AsnNumber))
                {
                    ASNDetails = RCommon.RedisOperation.GetList<List<ASNDetail>>("CloseBox:" + AsnNumber);
                    foreach (var item in ASNDetails)
                    {
                        if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + skuboxnumber))
                        {
                            lists = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + skuboxnumber);
                            lists = lists.GroupBy(c => new { SKU = c.SKU, Creator = c.Creator }).Select(a => new SKURF() { SKU = a.Key.SKU, Creator = a.Key.Creator, Qty = a.Sum(x => x.Qty) }).ToList();
                            return Json(new { Code = "1", data = lists });
                        }
                    }
                }

                return Json(new { Code = "0", data = lists });
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = lists });
            }
        }
        /// <summary>
        /// 查看ABC已绑箱明细
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="skuboxnumber"></param>
        /// <param name="GoodsType"></param>
        /// <returns></returns>
        public JsonResult CheckScanBoxDetailABC(string AsnNumber, string skuboxnumber)
        {
            List<ASNDetail> lists = new List<ASNDetail>();
            List<ASNDetail> ASNDetails = new List<ASNDetail>();
            try
            {
                if (RCommon.RedisOperation.Exists("ABCBoxNumber:" + AsnNumber))
                {
                    ASNDetails = RCommon.RedisOperation.GetList<List<ASNDetail>>("ABCBoxNumber:" + AsnNumber).Where(c => c.BoxNumber == skuboxnumber).ToList();
                    lists = ASNDetails.GroupBy(c => new { SKU = c.SKU }).Select(a => new ASNDetail() { SKU = a.Key.SKU, QtyReceived = a.Sum(x => x.QtyReceived) }).ToList();
                    return Json(new { Code = "1", data = lists });
                }

                return Json(new { Code = "0", data = lists });
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = lists });
            }
        }
        /// <summary>
        /// 回退已绑箱里的SKU 一次一个
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="skuboxnumber"></param>
        /// <param name="GoodsType"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public JsonResult DeleteSKUScanBox(string AsnNumber, string skuboxnumber, string GoodsType, string SKU)
        {
            List<SKURF> lists = new List<SKURF>();
            try
            {
                if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + skuboxnumber))
                {
                    lists = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + skuboxnumber);
                    lists.Remove(lists.Where(c => c.SKU == SKU).FirstOrDefault());
                    RCommon.RedisOperation.SetList("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + skuboxnumber, lists);
                    lists = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + skuboxnumber).GroupBy(c => new { SKU = c.SKU, Creator = c.Creator }).Select(a => new SKURF() { SKU = a.Key.SKU, Creator = a.Key.Creator, Qty = a.Sum(x => x.Qty) }).ToList();
                    return Json(new { Code = "1", data = lists });
                }
                return Json(new { Code = "0", data = lists });
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = lists });
            }
        }
        /// <summary>
        /// 回退已绑箱里的SKU 一次一个
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="skuboxnumber"></param>
        /// <param name="GoodsType"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public JsonResult DeleteSKUScanBoxCloseBox(string AsnNumber, string skuboxnumber, string GoodsType, string SKU)
        {
            List<SKURF> lists = new List<SKURF>();
            try
            {
                if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + skuboxnumber))
                {
                    lists = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + skuboxnumber);
                    lists.Remove(lists.Where(c => c.SKU == SKU).FirstOrDefault());
                    //foreach (var item in lists)
                    //{
                    //    if (item.SKU == SKU)
                    //    {
                    //        item.Qty = item.Qty - 1;
                    //        break;
                    //    }
                    //}
                    RCommon.RedisOperation.SetList("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + skuboxnumber, lists);
                    lists = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + GoodsType + ":" + skuboxnumber).GroupBy(c => new { SKU = c.SKU, Creator = c.Creator }).Select(a => new SKURF() { SKU = a.Key.SKU, Creator = a.Key.Creator, Qty = a.Sum(x => x.Qty) }).ToList();
                    return Json(new { Code = "1", data = lists });
                }
                return Json(new { Code = "0", data = lists });
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = lists });
            }
        }

        /// <summary>
        /// 回退已绑箱 ABC
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="skuboxnumber"></param>
        /// <param name="GoodsType"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public JsonResult DeleteSKUScanBoxABC(string AsnNumber, string skuboxnumber)
        {
            List<ASNDetail> lists = new List<ASNDetail>();
            try
            {
                if (RCommon.RedisOperation.Exists("ABCBoxNumber:" + AsnNumber))
                {

                    lists = RCommon.RedisOperation.GetList<List<ASNDetail>>("ABCBoxNumber:" + AsnNumber);
                    lists.RemoveAll(c => c.BoxNumber == skuboxnumber);
                    RCommon.RedisOperation.SetList("ABCBoxNumber:" + AsnNumber, lists);
                    if (RCommon.RedisOperation.GetList<List<ASNDetail>>("ABCBoxNumber:" + AsnNumber).Where(c => c.BoxNumber == skuboxnumber).Count() == 0)
                    {
                        var response = new ReceiptManagementService().DeleteScanABCByBoxNumber(AsnNumber, skuboxnumber);
                    }
                    else
                    {
                        DeleteSKUScanBoxABC(AsnNumber, skuboxnumber);
                    }

                    return Json(new { Code = "1", data = lists });
                }
                return Json(new { Code = "0", data = lists });
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = lists });
            }
        }
        /// <summary>
        /// 分SKU的时候检查数量是否满足
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public string CheckSKUQtyBySKU(string AsnNumber, string SKU)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            List<ASNDetail> listasndetail = new List<ASNDetail>();
            List<SKURF> listboxnumber = new List<SKURF>();
            List<SKURF> listsku = new List<SKURF>();
            List<SKURF> listskutemp = new List<SKURF>();
            double QtyExpected = 0;
            double QtyScaned = 0;
            string msg = "0";
            try
            {
                if (RCommon.RedisOperation.Exists("CloseBox:" + AsnNumber))
                {
                    listasndetail = RCommon.RedisOperation.GetList<List<ASNDetail>>("CloseBox:" + AsnNumber);
                    QtyExpected = listasndetail.Where(c => c.SKU == SKU).Sum(c => c.QtyExpected);
                    foreach (var item in listasndetail.GroupBy(a => a.SKU).Select(c => new ASNDetail() { SKU = c.Key }))
                    {
                        foreach (var itemgoodstype in vm.ProductLevel)
                        {
                            if (RCommon.RedisOperation.Exists("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text))
                            {
                                listboxnumber = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text);
                                foreach (var itemboxNum in listboxnumber)
                                {
                                    if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber))
                                    {
                                        listskutemp = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber);
                                        listsku.AddRange(listskutemp);
                                    }
                                }
                            }
                        }
                    }
                    if (RCommon.RedisOperation.Exists("CloseBox:" + AsnNumber + ":" + Session["Name"].ToString()))
                    {
                        listskutemp = RCommon.RedisOperation.GetList<List<SKURF>>("CloseBox:" + AsnNumber + ":" + Session["Name"].ToString());
                        listsku.AddRange(listskutemp);
                    }
                    if (RCommon.RedisOperation.Exists("ScanBoxBoxNumberList:" + AsnNumber))
                    {
                        listboxnumber = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBoxBoxNumberList:" + AsnNumber);
                        foreach (var itemgoodstype in vm.ProductLevel)
                        {
                            foreach (var itemboxNum in listboxnumber)
                            {
                                if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber))
                                {
                                    listskutemp = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber);
                                    listsku.AddRange(listskutemp);
                                }
                            }
                        }
                    }

                    QtyScaned = listsku.Where(c => c.SKU == SKU).Sum(c => c.Qty);
                }
            }
            catch (Exception ex)
            {
                msg = "-1";
            }
            if (QtyExpected != 0 && QtyExpected <= QtyScaned)
            {
                msg = "1";
            }
            return msg;
        }


        /// <summary>
        /// 扫描SKU第一次绑定箱号与序号
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="GoodsType"></param>
        /// <param name="SKU"></param>
        /// <param name="No"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public JsonResult SetSKUNo(string AsnNumber, string GoodsType, string SKU, string BoxNumber)
        {
            List<ASNDetail> lists = new List<ASNDetail>();
            List<SKURF> listrf = new List<SKURF>();
            ReceiptViewModel vm = new ReceiptViewModel();
            string NoFlag = "";
            try
            {
                //先判断序号是不是已经被其他SKU绑定了
                if (RCommon.RedisOperation.Exists("SKU:" + AsnNumber))
                {
                    lists = RCommon.RedisOperation.GetList<List<ASNDetail>>("SKU:" + AsnNumber);
                    foreach (var itemgoodstype in vm.ProductLevel)
                    {
                        foreach (var item in lists)
                        {
                            if (RCommon.RedisOperation.Exists("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text))
                            {
                                listrf = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text);
                                if (itemgoodstype.Text == GoodsType)
                                {
                                    if (listrf.Where(c => c.BoxNumber == BoxNumber || c.SKU == SKU).Count() > 0)
                                    {
                                        NoFlag = "0";
                                        listrf = listrf.Where(c => c.BoxNumber == BoxNumber || c.SKU == SKU).ToList();
                                        return Json(new { Code = "0", data = listrf });
                                    }
                                }
                                else
                                {
                                    if (listrf.Where(c => c.BoxNumber == BoxNumber).Count() > 0)
                                    {
                                        NoFlag = "0";
                                        listrf = listrf.Where(c => c.BoxNumber == BoxNumber).ToList();
                                        return Json(new { Code = "0", data = listrf });
                                    }
                                }

                            }
                        }
                    }
                }

                if (RCommon.RedisOperation.Exists("SKU:" + SKU + ":" + AsnNumber + ":" + GoodsType))
                {
                    listrf = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + SKU + ":" + AsnNumber + ":" + GoodsType);
                    return Json(new { Code = "1", data = listrf });
                }
                else
                {
                    listrf.Clear();
                    lock (Lock)
                    {
                        using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:AGoodSType" + AsnNumber + GoodsType, _client = RCommon.RedisOperation.cs })
                        {
                            if (RCommon.RedisOperation.Lock("AGoodSType" + AsnNumber + GoodsType, 5) != null)
                            {
                                if (RCommon.RedisOperation.Exists("SKU:" + AsnNumber + ":" + GoodsType))
                                {
                                    List<SKURF> listag = new List<SKURF>();
                                    listag = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + AsnNumber + ":" + GoodsType);
                                    listag.Add(new SKURF()
                                    {
                                        AsnNumber = AsnNumber,
                                        SKU = SKU,
                                        GoodsType = GoodsType,
                                        Creator = Session["Name"].ToString(),
                                        CreateTime = DateTime.Now,
                                        No = listag.Max(c => c.No) + 1
                                    });
                                    RCommon.RedisOperation.SetList("SKU:" + AsnNumber + ":" + GoodsType, listag);
                                    listrf.Add(new SKURF()
                                    {
                                        AsnNumber = AsnNumber,
                                        SKU = SKU,
                                        BoxNumber = BoxNumber,
                                        No = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + AsnNumber + ":" + GoodsType).Max(c => c.No),
                                        Order = 1,
                                        GoodsType = GoodsType,
                                        CreateTime = DateTime.Now,
                                        Creator = Session["Name"].ToString()
                                    });
                                    RCommon.RedisOperation.SetList("SKU:" + SKU + ":" + AsnNumber + ":" + GoodsType, listrf);
                                    listrf = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + SKU + ":" + AsnNumber + ":" + GoodsType);
                                    return Json(new { Code = "1", data = listrf });
                                }
                                else
                                {
                                    List<SKURF> listag = new List<SKURF>();
                                    listag.Add(new SKURF()
                                    {
                                        AsnNumber = AsnNumber,
                                        SKU = SKU,
                                        GoodsType = GoodsType,
                                        Creator = Session["Name"].ToString(),
                                        CreateTime = DateTime.Now,
                                        No = 1
                                    });
                                    RCommon.RedisOperation.SetList("SKU:" + AsnNumber + ":" + GoodsType, listag);
                                    listrf.Add(new SKURF()
                                    {
                                        AsnNumber = AsnNumber,
                                        SKU = SKU,
                                        BoxNumber = BoxNumber,
                                        No = 1,
                                        Order = 1,
                                        GoodsType = GoodsType,
                                        CreateTime = DateTime.Now,
                                        Creator = Session["Name"].ToString()
                                    });
                                    RCommon.RedisOperation.SetList("SKU:" + SKU + ":" + AsnNumber + ":" + GoodsType, listrf);
                                    listrf = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + SKU + ":" + AsnNumber + ":" + GoodsType);
                                    return Json(new { Code = "1", data = listrf });
                                }
                            }
                            else
                            {
                                SetSKUNo(AsnNumber, GoodsType, SKU, BoxNumber);

                            }
                            return Json(new { Code = "1", data = listrf });

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = ex.Message });
            }



        }
        /// <summary>
        /// 点击完成检查数量是否有差异
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <returns></returns>
        public JsonResult CheckQtyDiff(string AsnNumber)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            List<ASNDetail> listasndetail = new List<ASNDetail>();
            List<SKURF> listboxnumber = new List<SKURF>();
            List<SKURF> listsku = new List<SKURF>();
            List<SKURF> listskutemp = new List<SKURF>();
            double QtyExpected = 0;
            double QtyScaned = 0;
            try
            {
                if (RCommon.RedisOperation.Exists("SKU:" + AsnNumber))
                {
                    listasndetail = RCommon.RedisOperation.GetList<List<ASNDetail>>("SKU:" + AsnNumber);
                    QtyExpected = listasndetail.Sum(c => c.QtyExpected);
                    foreach (var item in listasndetail.GroupBy(a => a.SKU).Select(c => new ASNDetail() { SKU = c.Key }))
                    {
                        foreach (var itemgoodstype in vm.ProductLevel)
                        {
                            if (RCommon.RedisOperation.Exists("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text))
                            {
                                listboxnumber = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text);
                                foreach (var itemboxNum in listboxnumber)
                                {
                                    if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber))
                                    {
                                        listskutemp = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber);
                                        listsku.AddRange(listskutemp);
                                    }
                                }
                            }
                        }
                    }

                    if (RCommon.RedisOperation.Exists("ScanBoxBoxNumberList:" + AsnNumber))
                    {
                        listboxnumber = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBoxBoxNumberList:" + AsnNumber);
                        foreach (var itemgoodstype in vm.ProductLevel)
                        {
                            foreach (var itemboxNum in listboxnumber)
                            {
                                if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber))
                                {
                                    listskutemp = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber);
                                    listsku.AddRange(listskutemp);
                                }
                            }
                        }
                    }

                    QtyScaned = listsku.Sum(c => c.Qty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1" });
            }
            if (QtyExpected != QtyScaned)
            {
                return Json(new { Code = "0", data = QtyScaned - QtyExpected });
            }
            return Json(new { Code = "1" });


        }

        /// <summary>
        /// 装箱点击完成检查数量是否有差异
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <returns></returns>
        public JsonResult CheckQtyDiffCloseBox(string AsnNumber)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            List<ASNDetail> listasndetail = new List<ASNDetail>();
            List<SKURF> listboxnumber = new List<SKURF>();
            List<SKURF> listsku = new List<SKURF>();
            List<SKURF> listskutemp = new List<SKURF>();
            double QtyExpected = 0;
            double QtyScaned = 0;
            try
            {
                if (RCommon.RedisOperation.Exists("CloseBox:" + AsnNumber))
                {
                    listasndetail = RCommon.RedisOperation.GetList<List<ASNDetail>>("CloseBox:" + AsnNumber);
                    QtyExpected = listasndetail.Sum(c => c.QtyExpected);
                    if (RCommon.RedisOperation.Exists("ScanBoxBoxNumberList:" + AsnNumber))
                    {
                        listboxnumber = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBoxBoxNumberList:" + AsnNumber);
                        foreach (var itemgoodstype in vm.ProductLevel)
                        {
                            foreach (var itemboxNum in listboxnumber)
                            {
                                if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber))
                                {
                                    listskutemp = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber);
                                    listsku.AddRange(listskutemp);
                                }
                            }
                        }
                    }


                    foreach (var item in listasndetail.GroupBy(a => a.SKU).Select(c => new ASNDetail() { SKU = c.Key }))
                    {
                        foreach (var itemgoodstype in vm.ProductLevel)
                        {
                            if (RCommon.RedisOperation.Exists("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text))
                            {
                                listboxnumber = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text);
                                foreach (var itemboxNum in listboxnumber)
                                {
                                    if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber))
                                    {
                                        listskutemp = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber);
                                        listsku.AddRange(listskutemp);
                                    }
                                }
                            }
                        }
                    }

                    //foreach (var item in listasndetail.GroupBy(a => a.SKU).Select(c => new ASNDetail() { SKU = c.Key }))
                    //{
                    //    foreach (var itemgoodstype in vm.ProductLevel)
                    //    {
                    //        if (RCommon.RedisOperation.Exists("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text))
                    //        {
                    //            listboxnumber = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text);
                    //            foreach (var itemboxNum in listboxnumber)
                    //            {
                    //                if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + item.SKU + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber))
                    //                {
                    //                    listskutemp = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + item.SKU + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber);
                    //                    listsku.AddRange(listskutemp);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    QtyScaned = listsku.Sum(c => c.Qty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1" });
            }
            if (QtyExpected != QtyScaned)
            {
                return Json(new { Code = "0", data = QtyScaned - QtyExpected });
            }
            return Json(new { Code = "1" });


        }


        /// <summary>
        /// 分SKU提交完成
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <returns></returns>
        public string UpdateAsnScanSKU(string AsnNumber)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            List<ASNDetail> listasndetail = new List<ASNDetail>();
            List<SKURF> listboxnumber = new List<SKURF>();
            List<SKURF> listsku = new List<SKURF>();
            List<SKURF> listskutemp = new List<SKURF>();
            List<ReceiptDetail> ReceiptDetails = new List<ReceiptDetail>();
            AddReceiptAndReceiptDetailRequest request = new AddReceiptAndReceiptDetailRequest();
            string msg = "";
            try
            {
                if (RCommon.RedisOperation.Exists("SKU:" + AsnNumber))
                {
                    listasndetail = RCommon.RedisOperation.GetList<List<ASNDetail>>("SKU:" + AsnNumber);
                    foreach (var item in listasndetail.GroupBy(a => a.SKU).Select(c => new ASNDetail() { SKU = c.Key }))
                    {
                        foreach (var itemgoodstype in vm.ProductLevel)
                        {
                            if (RCommon.RedisOperation.Exists("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text))
                            {
                                listboxnumber = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text);
                                foreach (var itemboxNum in listboxnumber)
                                {
                                    if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber))
                                    {
                                        listskutemp = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber);
                                        listsku.AddRange(listskutemp);
                                    }
                                }
                            }
                        }
                    }

                    if (RCommon.RedisOperation.Exists("ScanBoxBoxNumberList:" + AsnNumber))
                    {
                        listboxnumber = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBoxBoxNumberList:" + AsnNumber);
                        foreach (var itemgoodstype in vm.ProductLevel)
                        {
                            foreach (var itemboxNum in listboxnumber)
                            {
                                if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber))
                                {
                                    listskutemp = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber);
                                    listsku.AddRange(listskutemp);
                                }
                            }
                        }
                    }




                    foreach (var item in listsku)
                    {
                        ReceiptDetails.Add(new ReceiptDetail()
                        {
                            ASNNumber = AsnNumber,
                            SKU = item.SKU,
                            QtyExpected = item.Qty,
                            str2 = item.BoxNumber,
                            Creator = item.Creator,
                            CreateTime = item.CreateTime,
                            GoodsType = item.GoodsType
                        });

                    }
                    request.ReceiptDetails = ReceiptDetails;
                    var response = new ReceiptManagementService().AddReceiptAndReceiptDetail_ScanSKU(request, Convert.ToInt64(Session["CustomerID"]), Session["Name"].ToString());
                    if (response.IsSuccess)
                    {
                        msg = "1";
                    }
                    else
                    {
                        msg = response.Result;
                    }
                }
                else
                {
                    msg = "0";
                }
            }
            catch (Exception ex)
            {
                msg = "-1";
            }
            return msg;
        }

        /// <summary>
        /// 装箱完成
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <returns></returns>
        public string UpdateAsnScanSKUCloseBox(string AsnNumber)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            List<ASNDetail> listasndetail = new List<ASNDetail>();
            List<SKURF> listboxnumber = new List<SKURF>();
            List<SKURF> listsku = new List<SKURF>();
            List<SKURF> listskutemp = new List<SKURF>();
            List<ReceiptDetail> ReceiptDetails = new List<ReceiptDetail>();
            AddReceiptAndReceiptDetailRequest request = new AddReceiptAndReceiptDetailRequest();
            string msg = "";
            try
            {
                if (RCommon.RedisOperation.Exists("CloseBox:" + AsnNumber))
                {
                    listasndetail = RCommon.RedisOperation.GetList<List<ASNDetail>>("CloseBox:" + AsnNumber);
                    if (RCommon.RedisOperation.Exists("ScanBoxBoxNumberList:" + AsnNumber))
                    {
                        listboxnumber = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBoxBoxNumberList:" + AsnNumber);
                        foreach (var itemgoodstype in vm.ProductLevel)
                        {
                            foreach (var itemboxNum in listboxnumber)
                            {
                                if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber))
                                {
                                    listskutemp = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber);
                                    listsku.AddRange(listskutemp);
                                }
                            }
                        }
                    }


                    foreach (var item in listasndetail.GroupBy(a => a.SKU).Select(c => new ASNDetail() { SKU = c.Key }))
                    {
                        foreach (var itemgoodstype in vm.ProductLevel)
                        {
                            if (RCommon.RedisOperation.Exists("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text))
                            {
                                listboxnumber = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text);
                                foreach (var itemboxNum in listboxnumber)
                                {
                                    if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber))
                                    {
                                        listskutemp = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber);
                                        listsku.AddRange(listskutemp);
                                    }
                                }
                            }
                        }
                    }

                    //foreach (var item in listasndetail.GroupBy(a => a.SKU).Select(c => new ASNDetail() { SKU = c.Key }))
                    //{
                    //    foreach (var itemgoodstype in vm.ProductLevel)
                    //    {
                    //        if (RCommon.RedisOperation.Exists("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text))
                    //        {
                    //            listboxnumber = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text);
                    //            foreach (var itemboxNum in listboxnumber)
                    //            {
                    //                if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + item.SKU + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber))
                    //                {
                    //                    listskutemp = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBox:" + AsnNumber + ":" + item.SKU + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber);
                    //                    listsku.AddRange(listskutemp);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    foreach (var item in listsku)
                    {
                        ReceiptDetails.Add(new ReceiptDetail()
                        {
                            ASNNumber = AsnNumber,
                            SKU = item.SKU,
                            QtyExpected = item.Qty,
                            str2 = item.BoxNumber,
                            Creator = item.Creator,
                            CreateTime = item.CreateTime,
                            GoodsType = item.GoodsType,
                            CustomerID = Convert.ToInt64(Session["CustomerID"])
                        });

                    }
                    request.ReceiptDetails = ReceiptDetails;
                    var response = new ReceiptManagementService().AddReceiptAndReceiptDetail_ScanSKU(request, Convert.ToInt64(Session["CustomerID"]), Session["Name"].ToString());
                    if (response.IsSuccess)
                    {
                        msg = "1";
                    }
                    else
                    {
                        msg = response.Result;
                    }
                }
                else
                {
                    msg = "0";
                }
            }
            catch (Exception ex)
            {
                msg = "-1";
            }
            return msg;
        }

        /// <summary>
        /// 新增箱绑定箱号与序号
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="GoodsType"></param>
        /// <param name="SKU"></param>
        /// <param name="No"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public JsonResult NewSetSKUNo(string AsnNumber, string GoodsType, string SKU, int No, string BoxNumber)
        {
            List<ASNDetail> lists = new List<ASNDetail>();
            List<SKURF> listrf = new List<SKURF>();
            ReceiptViewModel vm = new ReceiptViewModel();
            string NoFlag = "";
            try
            {
                //先判断序号是不是已经被其他SKU绑定了
                if (RCommon.RedisOperation.Exists("SKU:" + AsnNumber))
                {
                    lists = RCommon.RedisOperation.GetList<List<ASNDetail>>("SKU:" + AsnNumber);
                    foreach (var itemgoodstype in vm.ProductLevel)
                    {
                        foreach (var item in lists)
                        {
                            if (RCommon.RedisOperation.Exists("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text))
                            {
                                listrf = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text);
                                if (listrf.Where(c => c.BoxNumber == BoxNumber).Count() > 0)
                                {
                                    NoFlag = "0";
                                    listrf = listrf.Where(c => c.BoxNumber == BoxNumber).ToList();
                                    return Json(new { Code = "0", data = listrf });
                                }
                            }
                        }
                    }
                }
                listrf = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + SKU + ":" + AsnNumber + ":" + GoodsType);
                listrf.Add(new SKURF()
                {
                    AsnNumber = AsnNumber,
                    SKU = SKU,
                    BoxNumber = BoxNumber,
                    No = No,
                    Order = listrf.Max(c => c.Order) + 1,
                    GoodsType = GoodsType,
                    CreateTime = DateTime.Now,
                    Creator = Session["Name"].ToString()
                });
                RCommon.RedisOperation.SetList("SKU:" + SKU + ":" + AsnNumber + ":" + GoodsType, listrf);
                if (RCommon.RedisOperation.Exists("CheckScanBoxNumber:" + AsnNumber + ":" + Session["Name"].ToString()))
                {
                    RCommon.RedisOperation.Del("CheckScanBoxNumber:" + AsnNumber + ":" + Session["Name"].ToString());
                }
                return Json(new { Code = "1", data = listrf.Where(c => c.Order == listrf.Max(a => a.Order)) });

            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = ex.Message });
            }
        }
        /// <summary>
        /// 扫描SKU取放置顺序
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public JsonResult GetSKUNo(string AsnNumber, string SKU, string GoodsType)
        {
            List<SKURF> lists = new List<SKURF>();
            List<SKURF> listrf = new List<SKURF>();

            try
            {
                //lists = RCommon.RedisOperation.GetList<List<ASNDetail>>("SKU:" + AsnNumber);
                if (RCommon.RedisOperation.Exists("SKU:" + SKU + ":" + AsnNumber + ":" + GoodsType))
                {
                    listrf = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + SKU + ":" + AsnNumber + ":" + GoodsType);
                    //listrf.Add(new SKURF()
                    //{
                    //    AsnNumber = AsnNumber,
                    //    No = lists.Max(a => a.No),
                    //    BoxNumber = lists.Where(a => a.No == lists.Max(c => c.No)).Select(a => a.BoxNumber).FirstOrDefault(),
                    //    GoodsType = GoodsType,
                    //    Creator = Session["Name"].ToString(),
                    //    CreateTime = DateTime.Now
                    //});


                }
                else
                {
                    return Json(new { Code = "0", data = listrf.Where(c => c.Order == listrf.Max(a => a.Order)) });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = ex.Message });
            }
            return Json(new { Code = "1", data = listrf.Where(c => c.Order == listrf.Max(a => a.Order)) });
        }


        /// <summary>
        /// 装箱扫一个SKU+1
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public JsonResult GetSKUNoCloseBox(string AsnNumber, string SKU, string GoodsType)
        {
            List<SKURF> lists = new List<SKURF>();
            List<SKURF> listrf = new List<SKURF>();

            try
            {
                if (RCommon.RedisOperation.Exists("CloseBox:" + AsnNumber + ":" + Session["Name"].ToString()))
                {
                    listrf = RCommon.RedisOperation.GetList<List<SKURF>>("CloseBox:" + AsnNumber + ":" + Session["Name"].ToString());
                    if (listrf.Where(c => c.GoodsType == GoodsType).Count() == 0)
                    {
                        return Json(new { Code = "0", data = (listrf.Select(c => c.GoodsType).FirstOrDefault()) });
                    }
                    else
                    {
                        listrf.Add(new SKURF()
                        {
                            AsnNumber = AsnNumber,
                            SKU = SKU,
                            GoodsType = GoodsType,
                            Qty = 1,
                            CreateTime = DateTime.Now,
                            Creator = Session["Name"].ToString()
                        });
                        RCommon.RedisOperation.SetList("CloseBox:" + AsnNumber + ":" + Session["Name"].ToString(), listrf);
                        return Json(new { Code = "1", data = listrf.Where(c => c.GoodsType == GoodsType).Sum(a => a.Qty) });
                    }
                }
                else
                {
                    listrf.Add(new SKURF()
                    {
                        AsnNumber = AsnNumber,
                        SKU = SKU,
                        GoodsType = GoodsType,
                        Qty = 1,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()
                    });
                    RCommon.RedisOperation.SetList("CloseBox:" + AsnNumber + ":" + Session["Name"].ToString(), listrf);
                    return Json(new { Code = "1", data = listrf.Where(c => c.GoodsType == GoodsType).Sum(a => a.Qty) });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = ex.Message });
            }
        }

        public string GetAsnDetail(string AsnNumber)
        {

            List<ASNDetail> lists = new List<ASNDetail>();
            try
            {
                lists = new ReceiptManagementService().GetAsnDetailList(AsnNumber, "0", "0").ToList();
            }
            catch (Exception ex)
            {

            }
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = Int32.MaxValue;
            return jsSerializer.Serialize(lists);
        }
        public string GetAsnDetailCloseBox(string AsnNumber)
        {

            List<ASNDetail> lists = new List<ASNDetail>();
            try
            {
                if (RCommon.RedisOperation.Exists("CloseBox:" + AsnNumber))
                {
                    lists = RCommon.RedisOperation.GetList<List<ASNDetail>>("CloseBox:" + AsnNumber);
                }
                //lists = new ReceiptManagementService().GetAsnDetailList(AsnNumber,"0","0").ToList();
            }
            catch (Exception ex)
            {

            }
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = Int32.MaxValue;
            return jsSerializer.Serialize(lists);
        }

        /// <summary>
        /// 按箱删除上架信息
        /// </summary>
        /// <param name="ReceiptNumber">箱号</param>
        /// <returns></returns>
        public string DeleteRecByBoxnumber(string ReceiptNumber, string Boxnumber)
        {
            var response = new ReceiptManagementService().DeleteRecByBoxnumber(ReceiptNumber, Boxnumber, Session["Name"].ToString());
            if (response)
            {
                return "1";
            }
            return "0";
        }

        public ActionResult GetReceiptDetailYXDR(string str2, string ReceiptNo)
        {
            int ProjectID = 0;
            int.TryParse(Session["ProjectID"].ToString(), out ProjectID);
            int CustomerID = 0;
            int.TryParse(Session["CustomerID"].ToString(), out CustomerID);
            int WarehouseID = 0;
            int.TryParse(Session["WarehouseIDs"].ToString(), out WarehouseID);
            //获取存储过程名称
            var configs = ApplicationConfigHelper.GetWMS_ConfigType("RFReceiptReceivingScan", ProjectID, CustomerID, WarehouseID);

            //User user =  (User)Session[Constants.USER_INFO_KEY];
            IEnumerable<ReceiptDetail> recModelList;
            recModelList = new ReceiptManagementService().GetReceiptDetailListByReceiptNumberAndStr2(ReceiptNo, str2, Session["CustomerID"].ToString(), Session["WareHouseName"].ToString());
            return Json(recModelList.ToList(), JsonRequestBehavior.AllowGet);
            //recModelList;
        }

        public ActionResult GetReceiptReceiving(string ReceiptNumber)
        {
            //User user =  (User)Session[Constants.USER_INFO_KEY];
            IEnumerable<ReceiptReceiving> recModelList = null;
            try
            {
                //CSRedisClient cs = new CSRedisClient(Constants.RedisPath);
                //if (cs.Exists("ReceiptReceiving" + ReceiptNumber))
                //{
                //    recModelList = cs.Get<List<ReceiptReceiving>>("ReceiptReceiving" + ReceiptNumber);
                //}
                //else
                //{
                //    recModelList = new ReceiptManagementService().GetReceiptReceivingList(ReceiptNumber, Session["CustomerID"].ToString(), Session["WareHouseIds"].ToString());
                //    cs.Set("ReceiptReceiving" + ReceiptNumber, recModelList);
                //}
            }
            catch (Exception ex)
            { }
            return Json(recModelList.ToList(), JsonRequestBehavior.AllowGet);
            //recModelList;
        }
        public string CheckLocation(string Location, string ReceiptNumber)
        {

            var WarehouseID = Convert.ToInt64(Session["WareHouseIDs"].ToString());
            var WarehouseName = Session["WareHouseName"].ToString();
            var CustomerID = Convert.ToInt64(Session["CustomerID"].ToString());
            IEnumerable<LocationInfo> locations = null;
            string AreaName = "";
            try
            {
                //CSRedisClient cs = new CSRedisClient(Constants.RedisPath);
                //if (!RCommon.RedisOperation.Exists("Location:WarehouseID" + WarehouseID + ":CustomerID" + CustomerID))
                //{
                //    RCommon.RedisOperation.SetList("Location:WarehouseID" + WarehouseID + ":CustomerID" + CustomerID, DateTime.Now.ToString());
                //    locations = new Runbow.TWS.Biz.WarehouseService().GetWarehouseLocationListByWCID(WarehouseID, CustomerID).Result;
                //    foreach (var item in locations)
                //    {
                //        RCommon.RedisOperation.SetList("Location:" + WarehouseID + ":" + item.Location, item);
                //    }

                //}
                //if (RCommon.RedisOperation.Exists("Location:" + WarehouseID + ":" + Location))
                //{
                //    AreaName = RCommon.RedisOperation.GetList<LocationInfo>("Location:" + WarehouseID + ":" + Location).AreaName;
                //}
                //if (RCommon.RedisOperation.Exists(ReceiptNumber))
                //{
                //    AreaName = RCommon.RedisOperation.GetList<ReceiptDetail>(ReceiptNumber).Area;
                //}
                AreaName = new Runbow.TWS.Biz.WarehouseService().GetWarehouseLocationListByLocation(WarehouseID.ToString(), Location).Result.Select(c => c.AreaName).FirstOrDefault();

            }
            catch (Exception ex)
            { }
            return AreaName;

        }

        public string AdjustMentDetailDeleteRF(string ID)
        {
            bool flags = false;
            string msg = "";
            try
            {
                flags = new ReceiptManagementService().AdjustMentDetailDeleteRF(Convert.ToInt64(ID));
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            if (!flags)
            {
                msg = "删除失败";
            }
            return msg;
        }
        public string AdjustMentCompleteRF(string adjustmentnumber)
        {
            string msg = "";
            string response = "";
            try
            {
                if (Session["CustomerID"].ToString() == "101")
                {
                    response = new ReceiptManagementService().AdjustMentCompleteRF(adjustmentnumber);
                }
                if (Session["CustomerID"].ToString() == "103")
                {
                    response = new ReceiptManagementService().AdjustMentCompleteRFReturn(adjustmentnumber);
                }
                if (response.Contains("成功"))
                {
                    msg = "";
                }
                else
                {
                    msg = response;
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;


        }

        /// <summary>
        /// 检查当前箱是否完成
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public string CheckReceiptDetailQtyPickRedisByBoxNumber(string ReceiptNumber, string BoxNumber)
        {
            string response = "";
            int QtyExpected = 0;
            int QtyReceived = 0;
            try
            {

                List<ReceiptDetail> Lists = new List<ReceiptDetail>();
                List<ReceiptReceiving> ListReceivings = new List<ReceiptReceiving>();

                if (RCommon.RedisOperation.Exists(ReceiptNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<ReceiptDetail>>(ReceiptNumber);
                    QtyExpected = Convert.ToInt32(Lists.Where(a => a.str2 == BoxNumber).Sum(b => b.QtyExpected));
                }
                else
                {
                    response = "2";
                    return response;
                }
                if (RCommon.RedisOperation.Exists("ReceiptReceiving:" + ReceiptNumber))
                {
                    ListReceivings = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber);
                    QtyReceived = Convert.ToInt32(ListReceivings.Where(a => a.str2 == BoxNumber).Count() > 0 ? ListReceivings.Where(a => a.str2 == BoxNumber).Sum(b => b.QtyReceived) : 0);

                }
                if (QtyExpected != 0 && QtyReceived != 0 && QtyExpected == QtyReceived)
                {
                    response = "1";
                    RCommon.RedisOperation.Del(ReceiptNumber + ":" + BoxNumber);
                    var res = new LogOperationService().UpdateLogOperationPackageStatusRF(Session["Name"].ToString(), BoxNumber);
                    if (res.IsSuccess)
                    {
                        RCommon.RedisOperation.Del("GetBox:" + BoxNumber);//箱完成 删除占用key
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                List<WMS_LogRF_Exception> logRF_Exceptions = new List<WMS_LogRF_Exception>();
                if (RCommon.RedisOperation.Exists("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    logRF_Exceptions = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"));
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "CheckReceiptDetailQtyPickRedisByBoxNumber",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                else
                {
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "CheckReceiptDetailQtyPickRedisByBoxNumber",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }

                response = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// 检查当前箱是否完成退货仓
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public string CheckReceiptDetailQtyPickRedisByBoxNumberBack(string ReceiptNumber, string BoxNumber)
        {

            string response = "1";
            try
            {

                List<ReceiptDetail> Lists = new List<ReceiptDetail>();
                List<ReceiptReceiving> ListReceivings = new List<ReceiptReceiving>();

                if (RCommon.RedisOperation.Exists("Back:" + ReceiptNumber + ":" + BoxNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber + ":" + BoxNumber);
                    foreach (var item in Lists)
                    {
                        if (item.QtyExpected != item.QtyReceived)
                        {
                            response = "0";
                            break;
                        }
                    }
                    if (response != "0")
                    {
                        RCommon.RedisOperation.Del("Back:" + ReceiptNumber + ":" + BoxNumber);
                        var res = new LogOperationService().UpdateLogOperationPackageStatusRF(Session["Name"].ToString(), BoxNumber);
                    }
                    return response;
                }
                else
                {
                    response = "2";
                    return response;
                }

            }
            catch (Exception ex)
            {
                List<WMS_LogRF_Exception> logRF_Exceptions = new List<WMS_LogRF_Exception>();
                if (RCommon.RedisOperation.Exists("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    logRF_Exceptions = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"));
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "CheckReceiptDetailQtyPickRedisByBoxNumberBack",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                else
                {
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "CheckReceiptDetailQtyPickRedisByBoxNumberBack",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }

                response = "-1";
            }
            return response;
        }

        public string UpdateReceiptDetailQtyRedisByBoxNumber(string ReceiptNumber, string BoxNumber)
        {
            string response = "";
            try
            {

                List<ReceiptDetail> Lists = new List<ReceiptDetail>();
                if (RCommon.RedisOperation.Exists(ReceiptNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<ReceiptDetail>>(ReceiptNumber);
                    using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:RBL" + ReceiptNumber + BoxNumber, _client = RCommon.RedisOperation.cs })
                    {
                        if (RCommon.RedisOperation.Lock("RBL" + ReceiptNumber + BoxNumber, 5) != null)
                        {
                            foreach (var item in Lists.Where(c => c.str2 == BoxNumber))
                            {
                                item.QtyReceived = item.QtyExpected;
                            }
                            //RCommon.RedisOperation.Del(ReceiptNumber);
                            RCommon.RedisOperation.SetList(ReceiptNumber, Lists);
                            response = Lists.Where(c => c.str2 == BoxNumber).Select(a => a.SKU).FirstOrDefault();
                        }
                    }
                }
                return response;

            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
            return response;
        }
        /// <summary>
        /// 分SKU的时候检查是否扫描箱号了
        /// </summary>
        /// <param name="AsnNumber"></param>
        /// <param name="GoodsType"></param>
        /// <param name="SKU"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>

        public JsonResult CheckScanBoxNumber(string AsnNumber, string GoodsType, string SKU)
        {
            List<SKURF> lists = new List<SKURF>();

            try
            {
                if (RCommon.RedisOperation.Exists("CheckScanBoxNumber:" + AsnNumber + ":" + Session["Name"].ToString()))
                {

                    return Json(new { Code = "0", data = RCommon.RedisOperation.GetList<List<SKURF>>("CheckScanBoxNumber:" + AsnNumber + ":" + Session["Name"].ToString()) });
                }
                else
                {
                    if (SKU != "00")
                    {
                        lists.Add(new SKURF()
                        {
                            AsnNumber = AsnNumber,
                            GoodsType = GoodsType,
                            SKU = SKU,
                            Creator = Session["Name"].ToString(),
                            CreateTime = DateTime.Now
                        });
                        RCommon.RedisOperation.SetList("CheckScanBoxNumber:" + AsnNumber + ":" + Session["Name"].ToString(), lists);
                    }
                    return Json(new { Code = "1" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1" });
            }
        }
        /// <summary>
        /// 扫描查询redis中当前SKU数量是否满足
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public string CheckReceiptDetailQtyPickRedis(string ReceiptNumber, string SKU, string BoxNumber)
        {
            string response = "";
            double QtyExpectedL = 0;
            double QtyReceivedL = 0;
            double ScanQtyReceivedL = 0;
            try
            {

                List<ReceiptDetail> Lists = new List<ReceiptDetail>();
                if (RCommon.RedisOperation.Exists(ReceiptNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<ReceiptDetail>>(ReceiptNumber);
                    QtyExpectedL = Lists.Where(c => c.SKU == SKU && c.str2 == BoxNumber).Sum(a => a.QtyExpected);
                    //foreach (var item in Lists.Where(c => c.SKU == SKU && c.str2 == BoxNumber))
                    //{
                    //    if (item.QtyExpected <= item.QtyReceived)
                    //    {
                    //        response = "1";
                    //    }
                    //}


                    if (RCommon.RedisOperation.Exists("ReceiptReceiving:" + ReceiptNumber))
                    {
                        var datass = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber);
                        var datas = datass.Where(c => c.ReceiptNumber == ReceiptNumber && c.str2 == BoxNumber && c.SKU == SKU);
                        QtyReceivedL = datas.Count() > 0 ? datas.Sum(c => c.QtyReceived) : 0;

                    }

                    if (RCommon.RedisOperation.Exists(Session["Name"].ToString()))
                    {
                        var ListScaning = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString());
                        ScanQtyReceivedL = Convert.ToInt64(ListScaning.Where(c => c.ReceiptNumber == ReceiptNumber && c.str2 == BoxNumber && c.SKU == SKU).Sum(a => a.QtyReceived));
                    }
                    if (QtyExpectedL <= (QtyReceivedL + ScanQtyReceivedL))
                    {
                        response = "1";
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
        /// 扫描查询redis中当前SKU数量是否满足退货仓
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public string CheckReceiptDetailQtyPickRedisBack(string ReceiptNumber, string SKU, string BoxNumber)
        {
            string response = "";
            double QtyExpectedL = 0;
            double QtyReceivedL = 0;
            try
            {

                List<ReceiptDetail> Lists = new List<ReceiptDetail>();
                if (RCommon.RedisOperation.Exists("Back:" + ReceiptNumber + ":" + BoxNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber + ":" + BoxNumber);
                    QtyExpectedL = Lists.Where(c => c.SKU == SKU && c.str2 == BoxNumber).Sum(a => a.QtyExpected);
                    QtyReceivedL = Lists.Where(c => c.SKU == SKU && c.str2 == BoxNumber).Sum(a => a.QtyReceived);
                    if (QtyExpectedL <= QtyReceivedL)
                    {
                        response = "1";
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
        //扫描箱号获取箱里面总件数
        public string GetQtyByBoxNumber(string ReceiptNumber, string BoxNumber)
        {
            string response = "";
            try
            {
                if (RCommon.RedisOperation.Exists(ReceiptNumber + ":" + BoxNumber))
                {
                    //判断箱子是否被别人占用
                    if (RCommon.RedisOperation.GetList<ReceiptDetail>(ReceiptNumber + ":" + BoxNumber).Creator != Session["Name"].ToString())
                    {
                        response = "scaning";
                        return response;
                    }

                }
                else
                {
                    //箱子没人占用，先锁定此箱
                    ReceiptDetail rd = new ReceiptDetail();
                    rd.Creator = Session["Name"].ToString();
                    rd.CreateTime = DateTime.Now;
                    RCommon.RedisOperation.SetList(ReceiptNumber + ":" + BoxNumber, rd);
                }

                List<ReceiptDetail> Lists = new List<ReceiptDetail>();
                if (RCommon.RedisOperation.Exists(ReceiptNumber))
                {
                    response = RCommon.RedisOperation.GetList<List<ReceiptDetail>>(ReceiptNumber).Where(c => c.str2 == BoxNumber).
                        Sum(b => b.QtyExpected).ToString();
                }
            }
            catch (Exception ex)
            {
                response = "-1";
            }
            return response;
        }
        //扫描箱号获取箱里面总件数退货仓
        public JsonResult GetQtyByBoxNumberBack(string ReceiptNumber, string BoxNumber)
        {
            List<ReceiptDetail> lists = new List<ReceiptDetail>();
            string response = "";
            try
            {
                if (RCommon.RedisOperation.Exists("Back:" + ReceiptNumber + ":" + BoxNumber))
                {
                    lists = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber + ":" + BoxNumber);
                    //判断箱子是否被别人占用
                    if (lists.Select(c => c.Creator).FirstOrDefault() != Session["Name"].ToString() && !string.IsNullOrEmpty(lists.Select(c => c.Creator).FirstOrDefault()))
                    {
                        //response = "scaning";
                        //return response;
                        return Json(new { Code = "0", data = lists.Select(c => c.Creator).FirstOrDefault() });
                    }
                    else
                    {

                        //箱子没人占用，先锁定此箱
                        lists.ForEach(c => { c.Creator = Session["Name"].ToString(); c.CreateTime = DateTime.Now; c.Updator = Session["Name"].ToString(); c.UpdateTime = DateTime.Now; });
                        RCommon.RedisOperation.SetList("Back:" + ReceiptNumber + ":" + BoxNumber, lists);
                    }
                    //response = lists.Sum(a => a.QtyExpected).ToString();
                    return Json(new { Code = "1", data = lists.Sum(a => a.QtyExpected).ToString() });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1" });
            }
            return Json(new { Code = "1", data = "0" });
        }
        /// <summary>
        /// 异常退出后再次进入
        /// </summary>
        /// <param name="Area"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        public string UpdateReceiptReceivingByLocationAgain(string ReceiptNumber, string Area, string Location, string BoxNumber)
        {

            List<WMS_LogRF_Exception> logRF_Lists = new List<WMS_LogRF_Exception>();
            if (RCommon.RedisOperation.Exists("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd")))
            {
                logRF_Lists = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"));
                logRF_Lists.Add(new WMS_LogRF_Exception()
                {
                    LogType = "UpdateReceiptReceivingByLocationAgain",
                    ReleateNumber = ReceiptNumber,
                    PackageNumber = BoxNumber,
                    Remark = "监控是否进这个方法",
                    CreateTime = DateTime.Now,
                    Creator = Session["Name"].ToString()

                });
                RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Lists);
            }
            else
            {
                logRF_Lists.Add(new WMS_LogRF_Exception()
                {
                    LogType = "UpdateReceiptReceivingByLocationAgain",
                    ReleateNumber = ReceiptNumber,
                    PackageNumber = BoxNumber,
                    Remark = "监控是否进这个方法",
                    CreateTime = DateTime.Now,
                    Creator = Session["Name"].ToString()

                });
                RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Lists);
            }

            string response = "";
            bool msg;
            try
            {
                List<ReceiptReceiving> ListScaning = new List<ReceiptReceiving>();
                List<ReceiptReceiving> ListReceiptReceiving = new List<ReceiptReceiving>();
                List<ReceiptReceiving> ListReceiptReceivingRedis = new List<ReceiptReceiving>();
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                if (RCommon.RedisOperation.Exists(Session["Name"].ToString()))
                {
                    ListScaning = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString());
                    foreach (var item in ListScaning.Where(a => string.IsNullOrEmpty(a.Location) && a.ReceiptNumber == ReceiptNumber))
                    {
                        ListReceiptReceiving.Add(new ReceiptReceiving()
                        {
                            ReceiptNumber = item.ReceiptNumber,
                            ExternReceiptNumber = item.ExternReceiptNumber,
                            CustomerID = item.CustomerID,
                            CustomerName = item.CustomerName,
                            Warehouse = item.Warehouse,
                            Area = Area,
                            Location = Location,
                            str2 = item.str2,
                            SKU = item.SKU,
                            UPC = "",
                            LineNumber = item.LineNumber,
                            SkuLineNumber = item.LineNumber,
                            GoodsName = item.GoodsName,
                            GoodsType = "A品",
                            RID = item.RID,
                            RDID = item.RDID,
                            QtyReceived = item.QtyReceived,
                            Creator = Session["Name"].ToString(),
                            CreateTime = DateTime.Now
                        });
                    }
                    bool SetFlag = false;
                    using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:RR"  + ReceiptNumber , _client = RCommon.RedisOperation.cs })
                        {
                            if (RCommon.RedisOperation.Lock("RR"  + ReceiptNumber , 5) != null)
                            {
                                if (RCommon.RedisOperation.Exists("ReceiptReceiving:" + ListScaning.Select(c => c.ReceiptNumber).FirstOrDefault()))
                                {
                                    ListReceiptReceivingRedis = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ListScaning.Select(c => c.ReceiptNumber).FirstOrDefault());
                                    ListReceiptReceivingRedis.AddRange(ListReceiptReceiving);
                                    SetFlag=RCommon.RedisOperation.SetList("ReceiptReceiving:" + ListScaning.Select(c => c.ReceiptNumber).FirstOrDefault(), ListReceiptReceivingRedis);
                                if (RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber).Where(c => c.str2 == BoxNumber).Count() ==ListReceiptReceivingRedis.Where(c => c.str2 == BoxNumber).Count())
                                  {
                                   
                                    msg = new ReceiptManagementService().InsertReceiptReceiving(ListReceiptReceiving, Session["Name"].ToString());
                                    if (msg)
                                    {
                                        RCommon.RedisOperation.Del(Session["Name"].ToString());
                                       
                                    }
                                }
                                }
                                else
                                {
                                    SetFlag=RCommon.RedisOperation.SetList("ReceiptReceiving:" + ListScaning.Select(c => c.ReceiptNumber).FirstOrDefault(), ListReceiptReceiving);
                                if (RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber).Where(c => c.str2 == BoxNumber).Count() ==ListReceiptReceiving.Count())
                                {
                                    
                                    msg = new ReceiptManagementService().InsertReceiptReceiving(ListReceiptReceiving, Session["Name"].ToString());
                                    if (msg)
                                    {
                                        RCommon.RedisOperation.Del(Session["Name"].ToString());
                                        
                                    }

                                }
                            }

                        }
                    }

                    string BoxCompleteFlag = CheckReceiptDetailQtyPickRedisByBoxNumber(ReceiptNumber, BoxNumber);//检查当前箱是否完成
                    if (BoxCompleteFlag == "1")
                    {
                        return "11";
                    }
                    else
                    {
                        return "1";
                    }

                }
            }
            catch (Exception ex)
            {
                List<WMS_LogRF_Exception> logRF_Exceptions = new List<WMS_LogRF_Exception>();
                if (RCommon.RedisOperation.Exists("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    logRF_Exceptions = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"));
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptReceivingByLocationAgain",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                else
                {
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptReceivingByLocationAgain",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                response = "-1";
            }
            return response;
        }
        /// <summary>
        /// 异常退出后再次进入退货仓
        /// </summary>
        /// <param name="Area"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        public string UpdateReceiptReceivingByLocationAgainBack(string ReceiptNumber, string Area, string Location, string BoxNumber)
        {
            string BoxCompleteFlag = "";
            List<WMS_LogRF_Exception> logRF_Lists = new List<WMS_LogRF_Exception>();
            if (RCommon.RedisOperation.Exists("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd")))
            {
                logRF_Lists = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"));
                logRF_Lists.Add(new WMS_LogRF_Exception()
                {
                    LogType = "UpdateReceiptReceivingByLocationAgain",
                    ReleateNumber = ReceiptNumber,
                    PackageNumber = BoxNumber,
                    Remark = "监控是否进这个方法",
                    CreateTime = DateTime.Now,
                    Creator = Session["Name"].ToString()

                });
                RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Lists);
            }
            else
            {
                logRF_Lists.Add(new WMS_LogRF_Exception()
                {
                    LogType = "UpdateReceiptReceivingByLocationAgain",
                    ReleateNumber = ReceiptNumber,
                    PackageNumber = BoxNumber,
                    Remark = "监控是否进这个方法",
                    CreateTime = DateTime.Now,
                    Creator = Session["Name"].ToString()

                });
                RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Lists);
            }

            string response = "";
            bool msg;
            try
            {
                List<ReceiptReceiving> ListScaning = new List<ReceiptReceiving>();
                List<ReceiptReceiving> ListReceiptReceiving = new List<ReceiptReceiving>();
                List<ReceiptReceiving> ListReceiptReceivingRedis = new List<ReceiptReceiving>();
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                if (RCommon.RedisOperation.Exists(Session["Name"].ToString()))
                {
                    ListScaning = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString());
                    foreach (var item in ListScaning.Where(a => string.IsNullOrEmpty(a.Location) && a.ReceiptNumber == ReceiptNumber))
                    {
                        ListReceiptReceiving.Add(new ReceiptReceiving()
                        {
                            ReceiptNumber = item.ReceiptNumber,
                            ExternReceiptNumber = item.ExternReceiptNumber,
                            CustomerID = item.CustomerID,
                            CustomerName = item.CustomerName,
                            Warehouse = item.Warehouse,
                            Area = Area,
                            Location = Location,
                            str2 = item.str2,
                            SKU = item.SKU,
                            UPC = "",
                            LineNumber = item.LineNumber,
                            SkuLineNumber = item.LineNumber,
                            GoodsName = item.GoodsName,
                            GoodsType = item.GoodsType,
                            RID = item.RID,
                            RDID = item.RDID,
                            QtyReceived = item.QtyReceived,
                            Creator = Session["Name"].ToString(),
                            CreateTime = DateTime.Now
                        });
                    }
                    bool SetFlag = false;
                    using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:RR" + ReceiptNumber, _client = RCommon.RedisOperation.cs })
                    {
                        if (RCommon.RedisOperation.Lock("RR" + ReceiptNumber, 5) != null)
                        {
                            if (RCommon.RedisOperation.Exists("ReceiptReceivingBack:" + ListScaning.Select(c => c.ReceiptNumber).FirstOrDefault()))
                            {
                                ListReceiptReceivingRedis = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceivingBack:" + ListScaning.Select(c => c.ReceiptNumber).FirstOrDefault());
                                ListReceiptReceivingRedis.AddRange(ListReceiptReceiving);
                                SetFlag = RCommon.RedisOperation.SetList("ReceiptReceivingBack:" + ListScaning.Select(c => c.ReceiptNumber).FirstOrDefault(), ListReceiptReceivingRedis);
                                if (RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceivingBack:" + ReceiptNumber).Where(c => c.str2 == BoxNumber).Count() == ListReceiptReceivingRedis.Where(c => c.str2 == BoxNumber).Count())
                                {
                                   
                                    msg = new ReceiptManagementService().InsertReceiptReceiving(ListReceiptReceiving, Session["Name"].ToString());
                                    if (msg)
                                    {
                                        RCommon.RedisOperation.Del(Session["Name"].ToString());
                                        BoxCompleteFlag = CheckReceiptDetailQtyPickRedisByBoxNumberBack(ReceiptNumber, BoxNumber);//检查当前箱是否完成
                                    }
                                  
                                }
                            }
                            else
                            {
                                SetFlag = RCommon.RedisOperation.SetList("ReceiptReceivingBack:" + ListScaning.Select(c => c.ReceiptNumber).FirstOrDefault(), ListReceiptReceiving);
                                if (RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceivingBack:" + ReceiptNumber).Where(c => c.str2 == BoxNumber).Count() == ListReceiptReceiving.Count())
                                {
                                    
                                    msg = new ReceiptManagementService().InsertReceiptReceiving(ListReceiptReceiving, Session["Name"].ToString());
                                    if (msg)
                                    {
                                        RCommon.RedisOperation.Del(Session["Name"].ToString());
                                        BoxCompleteFlag = CheckReceiptDetailQtyPickRedisByBoxNumberBack(ReceiptNumber, BoxNumber);//检查当前箱是否完成
                                    }
                                }
                            }

                        }
                    }


                    if (BoxCompleteFlag == "1")
                    {
                        return "11";
                    }
                    else
                    {
                        return "1";
                    }

                }
            }
            catch (Exception ex)
            {
                List<WMS_LogRF_Exception> logRF_Exceptions = new List<WMS_LogRF_Exception>();
                if (RCommon.RedisOperation.Exists("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    logRF_Exceptions = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"));
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptReceivingByLocationAgain",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                else
                {
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptReceivingByLocationAgain",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                response = "-1";
            }
            return response;
        }


        /// <summary>
        /// 扫描库位，插入数据库，移除当前扫描记录
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        public string UpdateReceiptReceivingByLocation(string ReceiptNumber, string BoxNumber, string Area, string Location)
        {
            string response = "";
            bool msg;
            try
            {
                List<ReceiptReceiving> ListScaning = new List<ReceiptReceiving>();
                List<ReceiptReceiving> ListReceiptReceiving = new List<ReceiptReceiving>();
                List<ReceiptReceiving> ListReceiptReceivingRedis = new List<ReceiptReceiving>();
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                if (RCommon.RedisOperation.Exists(Session["Name"].ToString()))
                {
                    ListScaning = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString());
                    foreach (var item in ListScaning.Where(a => a.ReceiptNumber == ReceiptNumber && a.str2 == BoxNumber && string.IsNullOrEmpty(a.Location)))
                    {
                        ListReceiptReceiving.Add(new ReceiptReceiving()
                        {
                            ReceiptNumber = ReceiptNumber,
                            ExternReceiptNumber = item.ExternReceiptNumber,
                            CustomerID = item.CustomerID,
                            CustomerName = item.CustomerName,
                            Warehouse = item.Warehouse,
                            Area = Area,
                            Location = Location,
                            str2 = item.str2,
                            SKU = item.SKU,
                            UPC = "",
                            LineNumber = item.LineNumber,
                            SkuLineNumber = item.LineNumber,
                            GoodsName = item.GoodsName,
                            GoodsType = "A品",
                            RID = item.RID,
                            RDID = item.RDID,
                            QtyReceived = item.QtyReceived,
                            Creator = Session["Name"].ToString(),
                            CreateTime = DateTime.Now
                        });
                    }
                    bool SetFlag = false;
                    using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:RR" + ReceiptNumber, _client = RCommon.RedisOperation.cs })
                    {
                        if (RCommon.RedisOperation.Lock("RR" + ReceiptNumber, 10) != null)
                        {
                            if (RCommon.RedisOperation.Exists("ReceiptReceiving:" + ReceiptNumber))
                            {
                                ListReceiptReceivingRedis = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber);
                                ListReceiptReceivingRedis.AddRange(ListReceiptReceiving);
                                SetFlag = RCommon.RedisOperation.SetList("ReceiptReceiving:" + ReceiptNumber, ListReceiptReceivingRedis);
                                if (RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber).Where(c => c.str2 == BoxNumber).Count() == ListReceiptReceivingRedis.Where(c => c.str2 == BoxNumber).Count())
                                {
                                    
                                    msg = new ReceiptManagementService().InsertReceiptReceiving(ListReceiptReceiving, Session["Name"].ToString());
                                    if (msg)
                                    {
                                        RCommon.RedisOperation.Del(Session["Name"].ToString());
                                    }
                                    
                                }
                            }

                            else
                            {
                                SetFlag = RCommon.RedisOperation.SetList("ReceiptReceiving:" + ListScaning.Select(c => c.ReceiptNumber).FirstOrDefault(), ListReceiptReceiving);
                                if (RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber).Where(c => c.str2 == BoxNumber).Count() == ListReceiptReceiving.Count())
                                {
                                   
                                    msg = new ReceiptManagementService().InsertReceiptReceiving(ListReceiptReceiving, Session["Name"].ToString());
                                    if (msg)
                                    {
                                        RCommon.RedisOperation.Del(Session["Name"].ToString());
                                    }

                                }
                            }
                        }
                    }
                    string BoxCompleteFlag = CheckReceiptDetailQtyPickRedisByBoxNumber(ReceiptNumber, BoxNumber);//检查当前箱是否完成
                    if (BoxCompleteFlag == "1")
                    {
                        return "11";
                    }
                    else
                    {
                        return "1";
                    }

                }
            }
            catch (Exception ex)
            {
                List<WMS_LogRF_Exception> logRF_Exceptions = new List<WMS_LogRF_Exception>();
                if (RCommon.RedisOperation.Exists("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    logRF_Exceptions = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"));
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptReceivingByLocation",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                else
                {
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptReceivingByLocation",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                response = "-1";
            }
            return response;
        }

        /// <summary>
        /// 扫描库位，插入数据库，移除当前扫描记录退货仓
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        public string UpdateReceiptReceivingByLocationBack(string ReceiptNumber, string BoxNumber, string Area, string Location, string NewBoxNumber)
        {
            string response = "";
            bool msg;
            string BoxCompleteFlag = "";
            try
            {
                List<ReceiptReceiving> ListScaning = new List<ReceiptReceiving>();
                List<ReceiptReceiving> ListReceiptReceiving = new List<ReceiptReceiving>();
                List<ReceiptReceiving> ListReceiptReceivingRedis = new List<ReceiptReceiving>();
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();

                if (RCommon.RedisOperation.Exists(Session["Name"].ToString()))
                {
                    ListScaning = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString());
                    foreach (var item in ListScaning.Where(a => a.ReceiptNumber == ReceiptNumber && a.str2 == BoxNumber && string.IsNullOrEmpty(a.Location)))
                    {
                        ListReceiptReceiving.Add(new ReceiptReceiving()
                        {
                            ReceiptNumber = ReceiptNumber,
                            ExternReceiptNumber = item.ExternReceiptNumber,
                            CustomerID = item.CustomerID,
                            CustomerName = item.CustomerName,
                            Warehouse = item.Warehouse,
                            Area = Area,
                            Location = Location,
                            str4 = string.IsNullOrEmpty(NewBoxNumber) ? null : item.str2,
                            str2 = string.IsNullOrEmpty(NewBoxNumber) ? item.str2 : NewBoxNumber,
                            SKU = item.SKU,
                            UPC = "",
                            LineNumber = item.LineNumber,
                            SkuLineNumber = item.LineNumber,
                            GoodsName = item.GoodsName,
                            GoodsType = item.GoodsType,
                            RID = item.RID,
                            RDID = item.RDID,
                            QtyReceived = item.QtyReceived,
                            Creator = Session["Name"].ToString(),
                            CreateTime = DateTime.Now
                        });
                    }
                    bool SetFlag = false;
                    using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:RR" + ReceiptNumber, _client = RCommon.RedisOperation.cs })
                    {
                        if (RCommon.RedisOperation.Lock("RR" + ReceiptNumber, 5) != null)
                        {
                            if (RCommon.RedisOperation.Exists("ReceiptReceivingBack:" + ReceiptNumber))
                            {
                                ListReceiptReceivingRedis = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceivingBack:" + ReceiptNumber);
                                ListReceiptReceivingRedis.AddRange(ListReceiptReceiving);
                                SetFlag = RCommon.RedisOperation.SetList("ReceiptReceivingBack:" + ReceiptNumber, ListReceiptReceivingRedis);
                                if (RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceivingBack:" + ReceiptNumber).Where(c => c.str2 == (string.IsNullOrEmpty(NewBoxNumber) ? BoxNumber : NewBoxNumber)).Count() == ListReceiptReceivingRedis.Where(c => c.str2 == (string.IsNullOrEmpty(NewBoxNumber) ? BoxNumber : NewBoxNumber)).Count())
                                {
                                   
                                    msg = new ReceiptManagementService().InsertReceiptReceiving(ListReceiptReceiving, Session["Name"].ToString());
                                    if (msg)
                                    {
                                        RCommon.RedisOperation.Del(Session["Name"].ToString());
                                        BoxCompleteFlag = CheckReceiptDetailQtyPickRedisByBoxNumberBack(ReceiptNumber, BoxNumber);//检查当前箱是否完成
                                    }
                                   
                                }
                            }

                            else
                            {
                                SetFlag = RCommon.RedisOperation.SetList("ReceiptReceivingBack:" + ReceiptNumber, ListReceiptReceiving);
                                if (RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceivingBack:" + ReceiptNumber).Where(c => c.str2 == (string.IsNullOrEmpty(NewBoxNumber) ? BoxNumber : NewBoxNumber)).Count() == ListReceiptReceiving.Count())
                                {
                                  
                                    msg = new ReceiptManagementService().InsertReceiptReceiving(ListReceiptReceiving, Session["Name"].ToString());
                                    if (msg)
                                    {
                                        RCommon.RedisOperation.Del(Session["Name"].ToString());
                                        BoxCompleteFlag = CheckReceiptDetailQtyPickRedisByBoxNumberBack(ReceiptNumber, BoxNumber);//检查当前箱是否完成
                                    }
                                   
                                }
                            }
                        }
                    }

                    if (BoxCompleteFlag == "1")
                    {
                        return "11";
                    }
                    else
                    {
                        return "1";
                    }
                }
                //else
                //{
                //    List<ReceiptDetail> receiptdetails = new List<ReceiptDetail>();
                //    if (RCommon.RedisOperation.Exists("Back:" + ReceiptNumber + ":" + BoxNumber))
                //    {
                //        receiptdetails = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber + ":" + BoxNumber);
                //        foreach (var item in receiptdetails)
                //        {
                //            if (item.QtyExpected != item.QtyReceived)
                //            {
                //                ListReceiptReceiving.Add(new ReceiptReceiving()
                //                {
                //                    ReceiptNumber = ReceiptNumber,
                //                    ExternReceiptNumber = item.ExternReceiptNumber,
                //                    CustomerID = item.CustomerID,
                //                    CustomerName = item.CustomerName,
                //                    Warehouse = item.WarehouseName,
                //                    Area = Area,
                //                    Location = Location,
                //                    str4 = string.IsNullOrEmpty(NewBoxNumber) ? null : item.str2,
                //                    str2 = string.IsNullOrEmpty(NewBoxNumber) ? item.str2 : NewBoxNumber,
                //                    SKU = item.SKU,
                //                    UPC = "",
                //                    LineNumber = item.LineNumber,
                //                    SkuLineNumber = item.LineNumber,
                //                    GoodsName = item.GoodsName,
                //                    GoodsType = item.GoodsType,
                //                    RID = item.RID,
                //                    RDID = item.ID,
                //                    QtyReceived =item.QtyExpected- item.QtyReceived,
                //                    Creator = Session["Name"].ToString(),
                //                    CreateTime = DateTime.Now
                //                });
                //                item.QtyReceived = item.QtyExpected;
                //            }
                //        }
                //    }
                //    RCommon.RedisOperation.SetList("Back:" + ReceiptNumber + ":" + BoxNumber, receiptdetails);
                //}


            }
            catch (Exception ex)
            {
                List<WMS_LogRF_Exception> logRF_Exceptions = new List<WMS_LogRF_Exception>();
                if (RCommon.RedisOperation.Exists("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    logRF_Exceptions = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"));
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptReceivingByLocation",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                else
                {
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptReceivingByLocation",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                response = "-1";
            }
            return response;
        }
        /// <summary>
        /// 扫描库存中的箱号，插入数据库，移除当前扫描记录
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        public string UpdateReceiptReceivingByNewBoxNumber(string ReceiptNumber, string BoxNumber, string NewBoxNumber)
        {
            string response = "";
            bool msg;
            try
            {
                List<ReceiptReceiving> ListScaning = new List<ReceiptReceiving>();
                List<ReceiptReceiving> ListReceiptReceiving = new List<ReceiptReceiving>();
                List<ReceiptReceiving> ListReceiptReceivingRedis = new List<ReceiptReceiving>();
                List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
                if (RCommon.RedisOperation.Exists(Session["Name"].ToString()))
                {
                    ListScaning = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString());
                    foreach (var item in ListScaning.Where(a => a.ReceiptNumber == ReceiptNumber && a.str2 == BoxNumber && string.IsNullOrEmpty(a.Location)))
                    {
                        item.str4 = item.str2;
                        item.str2 = NewBoxNumber;

                    }
                    response = "1";
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                List<WMS_LogRF_Exception> logRF_Exceptions = new List<WMS_LogRF_Exception>();
                if (RCommon.RedisOperation.Exists("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    logRF_Exceptions = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"));
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptReceivingByNewBoxNumber",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                else
                {
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptReceivingByNewBoxNumber",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                response = "-1";
            }
            return response;
        }

        /// <summary>
        /// 扫描一个SKU redis+1
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public JsonResult UpdateReceiptDetailQtyRedis(string ReceiptNumber, string SKU, string BoxNumber)
        {
            //string response = "";
            ReceiptReceivingRFScan receiptReceivingRFScan = new ReceiptReceivingRFScan();
            double QtyReceivedL = 0;
            double BoxQtyReceivedL = 0;
            try
            {
                List<ReceiptDetail> Lists = new List<ReceiptDetail>();
                List<ReceiptReceiving> ListScaning = new List<ReceiptReceiving>();
                if (RCommon.RedisOperation.Exists(ReceiptNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<ReceiptDetail>>(ReceiptNumber);
                    if (Lists.Where(c => c.SKU == SKU && c.str2 == BoxNumber).Count() == 0)
                    {
                        return Json(new { Code = "0", data = receiptReceivingRFScan });
                    }
                    if (RCommon.RedisOperation.Exists("ReceiptReceiving:" + ReceiptNumber))
                    {
                        var datass = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber);
                        var datas = datass.Where(c => c.ReceiptNumber == ReceiptNumber && c.str2 == BoxNumber && c.SKU == SKU);
                        var databoxs = datass.Where(c => c.ReceiptNumber == ReceiptNumber && c.str2 == BoxNumber);
                        QtyReceivedL = datas.Count() > 0 ? datas.Sum(c => c.QtyReceived) : 0;
                        BoxQtyReceivedL = databoxs.Count() > 0 ? databoxs.Sum(c => c.QtyReceived) : 0;

                    }
                    using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:BRSL" + BoxNumber + ReceiptNumber + SKU, _client = RCommon.RedisOperation.cs })
                    {
                        if (RCommon.RedisOperation.Lock("BRSL" + BoxNumber + ReceiptNumber + SKU, 5) != null)
                        {
                            foreach (var item in Lists.Where(c => c.SKU == SKU && c.str2 == BoxNumber))
                            {

                                item.QtyReceived = item.QtyReceived + 1;
                                RCommon.RedisOperation.SetList(ReceiptNumber, Lists);
                                if (RCommon.RedisOperation.Exists(Session["Name"].ToString()))
                                {
                                    ListScaning = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString());
                                    int flag = 0;
                                    foreach (var itemscaning in ListScaning)
                                    {
                                        if (itemscaning.ReceiptNumber == ReceiptNumber && itemscaning.str2 == BoxNumber && itemscaning.SKU == SKU)
                                        {
                                            if (item.QtyExpected > (QtyReceivedL + itemscaning.QtyReceived))
                                            {
                                                itemscaning.QtyReceived = itemscaning.QtyReceived + 1;

                                            }
                                            flag = 1;
                                        }
                                    }
                                    if (flag == 0 && item.QtyExpected > QtyReceivedL)
                                    {
                                        ListScaning.Add(new ReceiptReceiving()
                                        {
                                            ReceiptNumber = ReceiptNumber,
                                            ExternReceiptNumber = item.ExternReceiptNumber,
                                            CustomerID = item.CustomerID,
                                            CustomerName = item.CustomerName,
                                            Warehouse = item.WarehouseName,
                                            str2 = item.str2,
                                            SKU = SKU,
                                            LineNumber = item.LineNumber,
                                            SkuLineNumber = item.LineNumber,
                                            GoodsName = item.GoodsName,
                                            GoodsType = "A品",
                                            RID = item.RID,
                                            RDID = item.ID,
                                            QtyReceived = 1,
                                            Creator = Session["Name"].ToString(),
                                            CreateTime = DateTime.Now
                                        });
                                    }
                                    RCommon.RedisOperation.SetList(Session["Name"].ToString(), ListScaning);
                                }
                                else
                                {
                                    if (item.QtyExpected > QtyReceivedL)
                                    {
                                        ListScaning.Add(new ReceiptReceiving()
                                        {
                                            ReceiptNumber = ReceiptNumber,
                                            ExternReceiptNumber = item.ExternReceiptNumber,
                                            CustomerID = item.CustomerID,
                                            CustomerName = item.CustomerName,
                                            Warehouse = item.WarehouseName,
                                            str2 = item.str2,
                                            SKU = SKU,
                                            LineNumber = item.LineNumber,
                                            SkuLineNumber = item.LineNumber,
                                            GoodsName = item.GoodsName,
                                            GoodsType = "A品",
                                            RID = item.RID,
                                            RDID = item.ID,
                                            QtyReceived = 1,
                                            Creator = Session["Name"].ToString(),
                                            CreateTime = DateTime.Now
                                        });
                                        RCommon.RedisOperation.SetList(Session["Name"].ToString(), ListScaning);
                                    }

                                }

                            }
                            //RCommon.RedisOperation.Del(ReceiptNumber);

                        }
                    }
                    var ReceiptNowList = RCommon.RedisOperation.GetList<List<ReceiptDetail>>(ReceiptNumber);
                    var ScanIngList = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString());
                    receiptReceivingRFScan.BoxScan = (BoxQtyReceivedL + Convert.ToInt64(ScanIngList.Where(c => c.str2 == BoxNumber && c.ReceiptNumber == ReceiptNumber).Sum(a => a.QtyReceived))).ToString();
                    receiptReceivingRFScan.SKUTotal = ReceiptNowList.Where(c => c.str2 == BoxNumber && c.ReceiptNumber == ReceiptNumber && c.SKU == SKU).Sum(a => a.QtyExpected).ToString();
                    receiptReceivingRFScan.SKUScan = (QtyReceivedL + Convert.ToInt64(ScanIngList.Where(c => c.str2 == BoxNumber && c.ReceiptNumber == ReceiptNumber && c.SKU == SKU).Sum(a => a.QtyReceived))).ToString();
                    return Json(new { Code = "1", data = receiptReceivingRFScan });
                }


            }
            catch (Exception ex)
            {
                List<WMS_LogRF_Exception> logRF_Exceptions = new List<WMS_LogRF_Exception>();
                if (RCommon.RedisOperation.Exists("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    logRF_Exceptions = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"));
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptDetailQtyRedis",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                else
                {
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptDetailQtyRedis",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                return Json(new { Code = "-1", data = receiptReceivingRFScan });
            }
            return Json(new { Code = "2", data = receiptReceivingRFScan });
        }


        /// <summary>
        /// 扫描一个SKU redis+1退货仓
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public JsonResult UpdateReceiptDetailQtyRedisBack(string ReceiptNumber, string SKU, string BoxNumber)
        {
            //string response = "";
            ReceiptReceivingRFScan receiptReceivingRFScan = new ReceiptReceivingRFScan();
            try
            {
                List<ReceiptDetail> Lists = new List<ReceiptDetail>();
                List<ReceiptReceiving> ListScaning = new List<ReceiptReceiving>();
                if (RCommon.RedisOperation.Exists("Back:" + ReceiptNumber + ":" + BoxNumber))
                {
                    Lists = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber + ":" + BoxNumber);
                    if (Lists.Where(c => c.SKU == SKU && c.str2 == BoxNumber).Count() == 0)
                    {
                        return Json(new { Code = "0", data = receiptReceivingRFScan });
                    }
                    using (var cslock = new CSRedisClientLock() { Name = "CSRedisClientLock:BRSL" + BoxNumber + ReceiptNumber + SKU, _client = RCommon.RedisOperation.cs })
                    {
                        if (RCommon.RedisOperation.Lock("BRSL" + BoxNumber + ReceiptNumber + SKU, 5) != null)
                        {
                            foreach (var item in Lists.Where(c => c.SKU == SKU))
                            {

                                item.QtyReceived = item.QtyReceived + 1;
                                //RCommon.RedisOperation.SetList("Back:" + ReceiptNumber + ":" + BoxNumber, Lists);
                                if (RCommon.RedisOperation.Exists(Session["Name"].ToString()))
                                {
                                    ListScaning = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString());
                                    int flag = 0;
                                    foreach (var itemscaning in ListScaning)
                                    {
                                        if (itemscaning.ReceiptNumber == ReceiptNumber && itemscaning.str2 == BoxNumber && itemscaning.SKU == SKU)
                                        {
                                            if (item.QtyExpected >= item.QtyReceived)
                                            {
                                                itemscaning.QtyReceived = itemscaning.QtyReceived + 1;

                                            }
                                            flag = 1;
                                        }
                                    }
                                    if (flag == 0 && item.QtyExpected >= item.QtyReceived)
                                    {
                                        ListScaning.Add(new ReceiptReceiving()
                                        {
                                            ReceiptNumber = ReceiptNumber,
                                            ExternReceiptNumber = item.ExternReceiptNumber,
                                            CustomerID = item.CustomerID,
                                            CustomerName = item.CustomerName,
                                            Warehouse = item.WarehouseName,
                                            str2 = item.str2,
                                            SKU = SKU,
                                            LineNumber = item.LineNumber,
                                            SkuLineNumber = item.LineNumber,
                                            GoodsName = item.GoodsName,
                                            GoodsType = item.GoodsType,
                                            RID = item.RID,
                                            RDID = item.ID,
                                            QtyReceived = 1,
                                            Creator = Session["Name"].ToString(),
                                            CreateTime = DateTime.Now
                                        });
                                    }
                                    RCommon.RedisOperation.SetList(Session["Name"].ToString(), ListScaning);
                                    RCommon.RedisOperation.SetList("Back:" + ReceiptNumber + ":" + BoxNumber, Lists);
                                }
                                else
                                {
                                    if (item.QtyExpected >= item.QtyReceived)
                                    {
                                        ListScaning.Add(new ReceiptReceiving()
                                        {
                                            ReceiptNumber = ReceiptNumber,
                                            ExternReceiptNumber = item.ExternReceiptNumber,
                                            CustomerID = item.CustomerID,
                                            CustomerName = item.CustomerName,
                                            Warehouse = item.WarehouseName,
                                            str2 = item.str2,
                                            SKU = SKU,
                                            LineNumber = item.LineNumber,
                                            SkuLineNumber = item.LineNumber,
                                            GoodsName = item.GoodsName,
                                            GoodsType = item.GoodsType,
                                            RID = item.RID,
                                            RDID = item.ID,
                                            QtyReceived = 1,
                                            Creator = Session["Name"].ToString(),
                                            CreateTime = DateTime.Now
                                        });
                                        RCommon.RedisOperation.SetList(Session["Name"].ToString(), ListScaning);
                                        RCommon.RedisOperation.SetList("Back:" + ReceiptNumber + ":" + BoxNumber, Lists);
                                    }

                                }

                            }


                        }
                    }
                    var ReceiptNowList = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber + ":" + BoxNumber);
                    var ScanIngList = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString());
                    receiptReceivingRFScan.BoxScan = ReceiptNowList.Sum(a => a.QtyReceived).ToString();
                    receiptReceivingRFScan.SKUTotal = ReceiptNowList.Where(c => c.SKU == SKU).Sum(a => a.QtyExpected).ToString();
                    receiptReceivingRFScan.SKUScan = ReceiptNowList.Where(c => c.SKU == SKU).Sum(a => a.QtyReceived).ToString();
                    return Json(new { Code = "1", data = receiptReceivingRFScan });
                }


            }
            catch (Exception ex)
            {
                List<WMS_LogRF_Exception> logRF_Exceptions = new List<WMS_LogRF_Exception>();
                if (RCommon.RedisOperation.Exists("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    logRF_Exceptions = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"));
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptDetailQtyRedisBack",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                else
                {
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptDetailQtyRedisBack",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                return Json(new { Code = "-1", data = receiptReceivingRFScan });
            }
            return Json(new { Code = "2", data = receiptReceivingRFScan });
        }
        public string CheckLocationForAreaAgain(string Location)
        {
            string ReceiptNumber = "";
            string response = "";
            try
            {
                ReceiptNumber = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString()).Select(c => c.ReceiptNumber).FirstOrDefault();
                response = new ReceiptManagementService().GetAreaForLocationAndStore(Location, ReceiptNumber);
            }
            catch (Exception ex)
            {
                response = "-1";
            }
            return response;
        }
        public string CheckLocationForAreaAgainBack(string Location, string ReceiptNumber, string BoxNumber)
        {
            //string ReceiptNumber = "";
            string response = "";
            try
            {
                //ReceiptNumber = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString()).Select(c => c.ReceiptNumber).FirstOrDefault();
                response = new ReceiptManagementService().GetAreaForLocationAndStoreBack(Location, ReceiptNumber, BoxNumber);
            }
            catch (Exception ex)
            {
                response = "-1";
            }
            return response;
        }
        public string CheckLocationForArea(string Location, string ReceiptNumber)
        {
            return new ReceiptManagementService().GetAreaForLocationAndStore(Location, ReceiptNumber);
        }
        public string CheckLocationForAreaBack(string Location, string ReceiptNumber, string BoxNumber)
        {
            return new ReceiptManagementService().GetAreaForLocationAndStoreBack(Location, ReceiptNumber, BoxNumber);
        }
        /// <summary>
        /// SKU分层
        /// </summary>
        /// <returns></returns>
        public ActionResult ReceiptSKUFloar(string CustomerID)
        {
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            var response = new ReceiptManagementService().GetSKUFloar();
            List<SKUFloar> ReceiptDetails = new List<SKUFloar>();
            ReceiptDetails = response.ToList();
            RCommon.RedisOperation.SetList("Back:SKUFloar", ReceiptDetails);

            return View();
        }
        public JsonResult CheckSKUFloar(string SKU)
        {
            List<SKUFloar> lists = new List<SKUFloar>();
            SKURF listqty = new SKURF();
            try
            {
                if (RCommon.RedisOperation.Exists("Back:SKUFloar"))
                {
                    lists = RCommon.RedisOperation.GetList<List<SKUFloar>>("Back:SKUFloar");
                    if (lists.FirstOrDefault(c => c.SKU == SKU) != null)
                    {
                        if (RCommon.RedisOperation.Exists("Back:SKUFloar:" + Session["Name"].ToString()))
                        {
                            listqty = RCommon.RedisOperation.GetList<SKURF>("Back:SKUFloar:" + Session["Name"].ToString());
                            listqty.Qty = listqty.Qty + 1;
                        }
                        else
                        {
                            listqty.Qty = 1;
                        }
                        RCommon.RedisOperation.SetList("Back:SKUFloar:" + Session["Name"].ToString(), listqty);
                        return Json(new { Code = "1", data = lists.FirstOrDefault(c => c.SKU == SKU).Floar, data2 = listqty.Qty });

                    }
                    else
                    {
                        return Json(new { Code = "0" });
                    }
                }

                return Json(new { Code = "0" });

            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1" });
            }

        }
        /// <summary>
        /// RF移库 查库位
        /// </summary>
        /// <param name="Location"></param>
        /// <returns></returns>
        public string CheckLocationForAreaAdjustMent(string Location, string StoreCode)
        {
            string msg = "";
            try
            {
                msg = new ReceiptManagementService().CheckLocationForAreaAdjustMentByCustomerID(Location, StoreCode, Session["CustomerID"].ToString());
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;
        }
        /// <summary>
        /// 检查移库单是否完成
        /// </summary>
        /// <param name="AdjustMentNumber"></param>
        /// <returns></returns>
        public string CheckAdjustMentStatus(string AdjustMentNumber)
        {
            string msg = "";
            try
            {
                msg = new ReceiptManagementService().CheckAdjustMentStatus(AdjustMentNumber);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return msg;
        }
        /// <summary>
        /// RF移库 扫SKU
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public JsonResult CheckLocationSKUAdjustMent(string Location, string SKU, string StoreCode)
        {
            List<Inventorys> datas = new List<Inventorys>();
            try
            {
                datas = new ReceiptManagementService().CheckLocationSKUAdjustMentByCustomerID(Location, SKU, StoreCode, Session["CustomerID"].ToString());
                if (datas.Count() > 0)
                {
                    return Json(new { Code = "1", data = datas });
                }
                else
                {
                    return Json(new { Code = "0" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1" });
            }
        }


        public string CheckLocationByNewLocationAdjustMent(string Area, string Location, string StoreCode)
        {
            return new ReceiptManagementService().CheckLocationByNewLocationAdjustMentByCustomerID(Area, Location, StoreCode, Session["CustomerID"].ToString());
        }
        /// <summary>
        /// RF 移库扫SKU 获取该SKU分布的库存状况
        /// </summary>
        /// <param name="Area"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public JsonResult GetLocationAndQtyBySKU(string Area, string SKU, string StoreCode)
        {
            List<Inventorys> lists = new List<Inventorys>();
            try
            {
                lists = new ReceiptManagementService().GetLocationAndQtyBySKUByCustomerID(Area, SKU, StoreCode, Session["CustomerID"].ToString()).ToList();
                return Json(new { Code = "1", data = lists });
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = lists });
            }
        }
        /// <summary>
        /// 批量移库扫描库位查询redis已扫记录
        /// </summary>
        /// <param name="Location"></param>
        /// <returns></returns>
        public JsonResult CheckAdjustRedisByLocation(string Location)
        {
            List<Inventorys> lists = new List<Inventorys>();
            try
            {
                if (RCommon.RedisOperation.Exists("ADJ:" + Location + ":" + Session["Name"].ToString()))
                {
                    lists = RCommon.RedisOperation.GetList<List<Inventorys>>("ADJ:" + Location + ":" + Session["Name"].ToString());
                }
                else
                {
                    RCommon.RedisOperation.SetList("ADJ:" + Location + ":" + Session["Name"].ToString(), lists);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = lists });
            }
            if (lists.Count() > 0)
            {
                return Json(new { Code = "1", data = lists });
            }
            return Json(new { Code = "0", data = lists });
        }
        //RF批量移库扫描SKU获取已扫总数
        public JsonResult GetAdjustSKUQtyRedisBySKU(string Location, string SKU)
        {
            List<Inventorys> lists = new List<Inventorys>();
            int qty = 0;
            try
            {
                if (RCommon.RedisOperation.Exists("ADJ:" + Location + ":" + Session["Name"].ToString()))
                {
                    lists = RCommon.RedisOperation.GetList<List<Inventorys>>("ADJ:" + Location + ":" + Session["Name"].ToString());
                }

            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = 0 });
            }
            qty = Convert.ToInt32(lists.Where(c => c.SKU == SKU).Sum(a => a.Qty));
            return Json(new { Code = "1", data = qty });
        }

        /// <summary>
        /// 批量移库扫描SKU redis+1
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public JsonResult UpdateAdjustRedisBySKU(string Location, string FromQty, string SKU)
        {
            List<Inventorys> lists = new List<Inventorys>();
            try
            {
                if (RCommon.RedisOperation.Exists("ADJ:" + Location + ":" + Session["Name"].ToString()))
                {
                    lists = RCommon.RedisOperation.GetList<List<Inventorys>>("ADJ:" + Location + ":" + Session["Name"].ToString());
                }

            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = lists });
            }
            if (lists.Count() > 0)
            {
                int flag = 0;
                foreach (var item in lists)
                {
                    if (item.SKU == SKU)
                    {
                        item.Qty = item.Qty + 1;
                        flag = 1;
                    }
                }
                if (flag == 0)
                {
                    lists.Add(new Inventorys()
                    {
                        SKU = SKU,
                        InventoryQty = Convert.ToInt64(FromQty),
                        Qty = 1
                    });
                }
                RCommon.RedisOperation.SetList("ADJ:" + Location + ":" + Session["Name"].ToString(), lists);
                return Json(new { Code = "1", data = lists });
            }
            else
            {
                lists.Add(new Inventorys()
                {
                    SKU = SKU,
                    InventoryQty = Convert.ToInt64(FromQty),
                    Qty = 1
                });
                RCommon.RedisOperation.SetList("ADJ:" + Location + ":" + Session["Name"].ToString(), lists);
                return Json(new { Code = "1", data = lists });
            }
        }
        /// <summary>
        /// RF盘点扫描SKU redis+1
        /// </summary>
        /// <param name="PDNumber"></param>
        /// <param name="Location"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public JsonResult UpdateQtyRedisWarehouseCheck(string PDNumber, string Location, string SKU)
        {
            WarehouseCheckDetail details = new WarehouseCheckDetail();
            List<WarehouseCheckDetail> listdetails = new List<WarehouseCheckDetail>();
            List<WarehouseCheckDetail> lists = new List<WarehouseCheckDetail>();
            try
            {
                listdetails = RCommon.RedisOperation.GetList<List<WarehouseCheckDetail>>("PD:" + PDNumber);
                details = listdetails.FirstOrDefault();
                if (RCommon.RedisOperation.Exists("PD:" + PDNumber + ":" + Session["Name"].ToString()))
                {
                    lists = RCommon.RedisOperation.GetList<List<WarehouseCheckDetail>>("PD:" + PDNumber + ":" + Session["Name"].ToString());
                }
                if (lists.Count() > 0)
                {
                    int flag = 0;
                    foreach (var item in lists)
                    {
                        if (item.SKU == SKU && item.Location == Location)
                        {
                            item.ActualQty = item.ActualQty + 1;
                            flag = 1;
                        }
                    }
                    if (flag == 0)
                    {
                        lists.Add(new WarehouseCheckDetail()
                        {
                            CID = details.CID,
                            CheckNumber = details.CheckNumber,
                            ExternNumber = details.ExternNumber,
                            CustomerID = details.CustomerID,
                            CustomerName = details.CustomerName,
                            Warehouse = details.Warehouse,
                            Area = listdetails.Where(c => c.Location == Location).Select(a => a.Area).FirstOrDefault(),
                            Location = Location,
                            SKU = SKU,
                            GoodsType = listdetails.Where(c => c.Location == Location && c.SKU == SKU).Select(a => a.GoodsType).FirstOrDefault(),
                            CheckQty = listdetails.Where(c => c.Location == Location && c.SKU == SKU).Count() > 0 ? listdetails.Where(c => c.Location == Location && c.SKU == SKU).Select(a => a.CheckQty).FirstOrDefault() : 0,
                            ActualQty = 1,
                            IS_Difference = "0",
                            IS_Deal = "0",
                            Creator = Session["Name"].ToString(),
                            CreateTime = DateTime.Now
                        });
                    }
                    RCommon.RedisOperation.SetList("PD:" + PDNumber + ":" + Session["Name"].ToString(), lists);
                    return Json(new { Code = "1", data = lists.Sum(a => a.ActualQty) });
                }
                else
                {
                    lists.Add(new WarehouseCheckDetail()
                    {
                        CID = details.CID,
                        CheckNumber = details.CheckNumber,
                        ExternNumber = details.ExternNumber,
                        CustomerID = details.CustomerID,
                        CustomerName = details.CustomerName,
                        Warehouse = details.Warehouse,
                        Area = listdetails.Where(c => c.Location == Location).Select(a => a.Area).FirstOrDefault(),
                        Location = Location,
                        SKU = SKU,
                        GoodsType = listdetails.Where(c => c.Location == Location && c.SKU == SKU).Select(a => a.GoodsType).FirstOrDefault(),
                        CheckQty = listdetails.Where(c => c.Location == Location && c.SKU == SKU).Count() > 0 ? listdetails.Where(c => c.Location == Location && c.SKU == SKU).Select(a => a.CheckQty).FirstOrDefault() : 0,
                        ActualQty = 1,
                        IS_Difference = "0",
                        IS_Deal = "0",
                        Creator = Session["Name"].ToString(),
                        CreateTime = DateTime.Now
                    });
                    RCommon.RedisOperation.SetList("PD:" + PDNumber + ":" + Session["Name"].ToString(), lists);
                    return Json(new { Code = "1", data = lists.Sum(a => a.ActualQty) });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = lists });
            }

        }
        public string CheckUPC(string UPC)
        {

            long WarehouseID = Convert.ToInt64(Session["WareHouseIDs"].ToString());
            //ApplicationConfigHelper.RefreshGetWarehouseLocationList(WarehouseID);
            IEnumerable<ProductStorer> locations = ApplicationConfigHelper.GetALLProductStorerList(Convert.ToInt64(Session["CustomerID"].ToString()), UPC).Where(m => m.UPC == UPC);
            if (locations.Count() > 0)
            {
                return locations.ToList()[0].SKU;
            }
            return "";
        }
        public static List<T> JSONStringToList<T>(string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);
            return objs;
        }
        public string SaveRecData2(string ReceiptNumber, string JsonData)
        {
            IEnumerable<ReceiptReceiving> receivelist = JSONStringToList<ReceiptReceiving>(JsonData).AsEnumerable();
            List<ReceiptReceiving> list = new List<ReceiptReceiving>();
            var distinctList = receivelist.GroupBy(rl => rl.LineNumber);
            foreach (var item in distinctList)
            {
                int i = 0;
                foreach (var item2 in item)
                {
                    i++;
                    item2.SkuLineNumber = i.ToString().PadLeft(5, '0');
                    list.Add(item2);
                }
            }
            var response = new ReceiptManagementService().InsertReceiptReceiving(list, Session["Name"].ToString());

            if (response)
            {
                return "1";
            }
            return "0";
        }
        public string SaveRecData(string ReceiptNumber, string JsonData)
        {
            //SaveData
            //recModelList;
            IEnumerable<ReceiptReceiving> receivelist = JSONStringToList<ReceiptReceiving>(JsonData).AsEnumerable();

            var distinctList = receivelist.GroupBy(rl => rl.LineNumber);
            foreach (var item in distinctList)
            {
                int i = 0;
                //foreach (var item2 in item.)
                //{

                //}
            }
            var response = new ReceiptManagementService().InsertReceiptReceiving(receivelist, Session["Name"].ToString());

            if (response)
            {
                return "1";
            }
            return "0";
        }
        /// <summary>
        /// 扫描插入redis中
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="JsonData"></param>
        /// <returns></returns>
        public string SaveRecDataRedis(string ReceiptNumber, string JsonData, string BoxNumber)
        {
            try
            {

                List<ReceiptReceiving> receivelist = JSONStringToList<ReceiptReceiving>(JsonData);
                receivelist.ForEach(c =>
                {
                    c.Creator = Session["Name"].ToString();
                    c.CreateTime = DateTime.Now;
                });
                if (!RCommon.RedisOperation.Exists("ReceiptReceiving:" + ReceiptNumber))
                {
                    RCommon.RedisOperation.SetList("ReceiptReceiving:" + ReceiptNumber, receivelist);
                }
                else
                {
                    List<ReceiptReceiving> receivelistredis = new List<ReceiptReceiving>();
                    receivelistredis = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber);
                    receivelistredis.AddRange(receivelist);
                    RCommon.RedisOperation.SetList("ReceiptReceiving:" + ReceiptNumber, receivelistredis);

                }
                if (RCommon.RedisOperation.Exists(ReceiptNumber + ":" + BoxNumber + ":" + Session["Name"].ToString()))
                {
                    RCommon.RedisOperation.Del(ReceiptNumber + ":" + BoxNumber + ":" + Session["Name"].ToString());
                }
                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        /// <summary>
        /// 查询是否已完成
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public string CheckComplete(string ReceiptNumber)
        {
            string returns = "1";
            List<ReceiptDetail> receiptdetaillistredis = new List<ReceiptDetail>();
            List<ReceiptReceiving> receiptreceivinglistredis = new List<ReceiptReceiving>();
            try
            {
                if (RCommon.RedisOperation.Exists(ReceiptNumber))
                {
                    receiptdetaillistredis = RCommon.RedisOperation.GetList<List<ReceiptDetail>>(ReceiptNumber);
                    //receiptdetaillistredis.ForEach(c =>
                    //{
                    //    if (c.QtyExpected != c.QtyReceived)
                    //    {
                    //        returns = "2";
                    //    }
                    //});
                }
                else
                {
                    returns = "3";
                    return returns;
                }
                if (RCommon.RedisOperation.Exists("ReceiptReceiving:" + ReceiptNumber))
                {
                    receiptreceivinglistredis = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber);
                    receiptreceivinglistredis = receiptreceivinglistredis.GroupBy(a => new { ReceiptNumber = a.ReceiptNumber, str2 = a.str2, Sku = a.SKU }).
                        Select(b => new ReceiptReceiving { ReceiptNumber = b.Key.ReceiptNumber, str2 = b.Key.str2, SKU = b.Key.Sku, QtyReceived = b.Sum(a => a.QtyReceived) }).ToList();
                }
                foreach (var item in receiptdetaillistredis)
                {
                    if (receiptreceivinglistredis.Where(a => a.ReceiptNumber == item.ReceiptNumber &&
                    a.str2 == item.str2 && a.SKU == item.SKU).Count() == 0)
                    {
                        returns = "2";
                        break;
                    }
                    foreach (var itemdetail in receiptreceivinglistredis.Where(a => a.ReceiptNumber == item.ReceiptNumber &&
                    a.str2 == item.str2 && a.SKU == item.SKU))
                    {
                        if (item.QtyExpected != itemdetail.QtyReceived)
                        {
                            returns = "2";
                            break;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                List<WMS_LogRF_Exception> logRF_Exceptions = new List<WMS_LogRF_Exception>();
                if (RCommon.RedisOperation.Exists("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    logRF_Exceptions = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"));
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "CheckComplete",
                        ReleateNumber = ReceiptNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                else
                {
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "CheckComplete",
                        ReleateNumber = ReceiptNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                returns = "-1";
            }
            return returns;
        }
        /// <summary>
        /// 查询是否已完成退货仓
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public string CheckCompleteBack(string ReceiptNumber)
        {
            string returns = "1";
            List<ReceiptDetail> receiptdetaillistredis = new List<ReceiptDetail>();
            List<ReceiptReceiving> receiptreceivinglistredis = new List<ReceiptReceiving>();
            try
            {
                if (RCommon.RedisOperation.Exists("Back:" + ReceiptNumber))
                {
                    receiptdetaillistredis = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber);
                    foreach (var item in receiptdetaillistredis.GroupBy(c => c.str2).Select(a => new ReceiptDetail() { str2 = a.Key }))
                    {
                        if (RCommon.RedisOperation.Exists("Back:" + ReceiptNumber + ":" + item.str2))
                        {
                            returns = "2";
                            break;
                        }
                    }

                }
                else
                {
                    returns = "3";
                    return returns;
                }


            }
            catch (Exception ex)
            {
                List<WMS_LogRF_Exception> logRF_Exceptions = new List<WMS_LogRF_Exception>();
                if (RCommon.RedisOperation.Exists("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    logRF_Exceptions = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"));
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "CheckCompleteBack",
                        ReleateNumber = ReceiptNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                else
                {
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "CheckCompleteBack",
                        ReleateNumber = ReceiptNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                returns = "-1";
            }
            return returns;
        }
        /// <summary>
        /// RF枪新增移库单
        /// </summary>
        /// <param name="AdjustMentNumber"></param>
        /// <returns></returns>
        public JsonResult AddAdjustMentRF(string AdjustMentNumber, string StoreCode)
        {

            List<Adjustment> adjust = new List<Adjustment>();
            try
            {
                adjust.Add(new Adjustment()
                {
                    AdjustmentNumber = AdjustMentNumber,
                    CustomerID = Convert.ToInt64(Session["CustomerID"]),
                    CustomerName = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.ID == Convert.ToInt64(Session["CustomerID"])).Select(c => c.Name).FirstOrDefault(),
                    Warehouse = Session["WareHouseName"].ToString(),
                    AdjustmentType = "库存移动单",
                    AdjustmentReason = "RF主动移库",
                    AdjustmentTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                    Creator = Session["Name"].ToString(),
                    IsHold = 0,
                    Status = 1,
                    Int1 = 1,
                    Remark = "RF主动移库",
                    str3 = StoreCode
                });
                var response = new ReceiptManagementService().AddAdjustMentRF(adjust);
                if (response)
                {
                    return Json(new { Code = "1" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1" });
            }
            return Json(new { Code = "0" });
        }
        /// <summary>
        /// 为啥不写成一个 因为要修改原来的 影响仓库正在使用
        /// </summary>
        /// <param name="AdjustMentNumber"></param>
        /// <returns></returns>
        public JsonResult AddAdjustMentRFBatch(string AdjustMentNumber, string StoreCode)
        {

            List<Adjustment> adjust = new List<Adjustment>();
            try
            {
                adjust.Add(new Adjustment()
                {
                    AdjustmentNumber = AdjustMentNumber,
                    CustomerID = Convert.ToInt64(Session["CustomerID"]),
                    CustomerName = "NIKE-SH",
                    Warehouse = Session["WareHouseName"].ToString(),
                    AdjustmentType = "库存移动单",
                    AdjustmentReason = "RF主动移库",
                    AdjustmentTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                    Creator = Session["Name"].ToString(),
                    IsHold = 0,
                    Status = 1,
                    Int1 = 2,
                    Remark = "RF主动移库",
                    str3 = StoreCode
                });
                var response = new ReceiptManagementService().AddAdjustMentRF(adjust);
                if (response)
                {
                    return Json(new { Code = "1" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1" });
            }
            return Json(new { Code = "0" });
        }

        public JsonResult AddAdjustMentDetailRF(string AdjustNumber, string Area, string FromLocation, string ToLocation, string SKU, string FromQty, string ToQty, string ToArea, string GoodsType)
        {

            List<AdjustmentDetail> adjustdetails = new List<AdjustmentDetail>();
            try
            {
                adjustdetails.Add(new AdjustmentDetail()
                {
                    AdjustmentNumber = AdjustNumber,
                    CustomerID = Convert.ToInt64(Session["CustomerID"]),
                    CustomerName = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.ID == Convert.ToInt64(Session["CustomerID"])).Select(c => c.Name).FirstOrDefault(),
                    FromLot = null,
                    ToLot = null,
                    SKU = SKU,
                    UPC = null,
                    BatchNumber = null,
                    BoxNumber = null,
                    GoodsName = SKU,
                    FromWarehouse = Session["WareHouseName"].ToString(),
                    ToWarehouse = Session["WareHouseName"].ToString(),
                    FromArea = Area,
                    ToArea = ToArea,
                    FromLocation = FromLocation,
                    ToLocation = ToLocation,
                    FromQty = Convert.ToInt64(FromQty),
                    ToQty = Convert.ToInt64(ToQty),
                    FromGoodsType = GoodsType,
                    ToGoodsType = GoodsType,
                    Unit = null,
                    Specifications = null,
                    IsHold = 0,
                    AdjustmentReason = null,
                    CreateTime = DateTime.Now,
                    Creator = Session["Name"].ToString()
                });
                var response = new ReceiptManagementService().AddAdjustMentDetailRF(adjustdetails);
                if (response == "添加成功")
                {
                    return Json(new { Code = "1" });
                }
                else
                {
                    return Json(new { Code = "0", data = response });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = ex.Message });
            }
        }

        /// <summary>
        /// RF批量移库 扫描目标库位
        /// </summary>
        /// <param name="AdjustNumber"></param>
        /// <param name="Area"></param>
        /// <param name="FromLocation"></param>
        /// <param name="ToLocation"></param>
        /// <returns></returns>
        public JsonResult AddAdjustMentDetailBatchRF(string AdjustNumber, string Area, string FromLocation, string ToLocation, string ToArea)
        {
            List<AdjustmentDetail> adjustdetails = new List<AdjustmentDetail>();
            List<Inventorys> lists = new List<Inventorys>();
            try
            {
                if (RCommon.RedisOperation.Exists("ADJ:" + FromLocation + ":" + Session["Name"].ToString()))
                {
                    lists = RCommon.RedisOperation.GetList<List<Inventorys>>("ADJ:" + FromLocation + ":" + Session["Name"].ToString());
                }
                foreach (var item in lists)
                {
                    adjustdetails.Add(new AdjustmentDetail()
                    {
                        AdjustmentNumber = AdjustNumber,
                        CustomerID = Convert.ToInt64(Session["CustomerID"]),
                        CustomerName = "NIKE-SH",
                        FromLot = null,
                        ToLot = null,
                        SKU = item.SKU,
                        UPC = null,
                        BatchNumber = null,
                        BoxNumber = null,
                        GoodsName = item.SKU,
                        FromWarehouse = Session["WareHouseName"].ToString(),
                        ToWarehouse = Session["WareHouseName"].ToString(),
                        FromArea = Area,
                        ToArea = ToArea,
                        FromLocation = FromLocation,
                        ToLocation = ToLocation,
                        FromQty = item.InventoryQty,
                        ToQty = item.Qty,
                        FromGoodsType = "A品",
                        ToGoodsType = "A品",
                        Unit = null,
                        Specifications = null,
                        IsHold = 0,
                        AdjustmentReason = null,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()
                    });
                }


                var response = new ReceiptManagementService().AddAdjustMentDetailRF(adjustdetails);
                if (response == "添加成功")
                {
                    RCommon.RedisOperation.Del("ADJ:" + FromLocation + ":" + Session["Name"].ToString());
                    return Json(new { Code = "1" });
                }
                else
                {
                    return Json(new { Code = "0", data = response });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = "-1", data = ex.Message });
            }

        }

        /// <summary>
        /// 下次任务检查是否上次有异常数据
        /// </summary>
        /// <returns></returns>
        public JsonResult CheckRedisTask()
        {
            List<ReceiptDetail> receivelist = new List<ReceiptDetail>();
            try
            {
                if (RCommon.RedisOperation.Exists(Session["Name"].ToString()))
                {
                    receivelist = RCommon.RedisOperation.GetList<List<ReceiptDetail>>(Session["Name"].ToString());

                }
                return Json(new { Code = 1, data = receivelist });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 0, data = ex.Message });
            }

        }

        public void GetReceivingFromRedis(string ReceiptNumber)
        {
            List<ReceiptReceiving> receivelistredis = new List<ReceiptReceiving>();
            string sums = "";
            if (RCommon.RedisOperation.Exists("ReceiptReceiving:" + ReceiptNumber))
            {
                receivelistredis = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber);
            }
            sums = receivelistredis.Sum(c => c.QtyReceived).ToString();
        }
        /// <summary>
        /// 查看单差异
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public JsonResult CheckReceiptReceivingByReceiptNumber(string ReceiptNumber)
        {
            List<ReceiptDetail> receiptlistredis = new List<ReceiptDetail>();
            List<ReceiptReceiving> receivelistredis = new List<ReceiptReceiving>();
            List<ReceiptDetail> difflistredis = new List<ReceiptDetail>();
            try
            {
                if (RCommon.RedisOperation.Exists(ReceiptNumber))
                {
                    receiptlistredis = RCommon.RedisOperation.GetList<List<ReceiptDetail>>(ReceiptNumber);
                }
                else
                {
                    return Json(new { Code = 3 });
                }
                if (RCommon.RedisOperation.Exists("ReceiptReceiving:" + ReceiptNumber))
                {
                    receivelistredis = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber);
                }



                if (receivelistredis.Where(c => c.ReceiptNumber == ReceiptNumber).Count() > 0)
                {
                    foreach (var item in receiptlistredis.Where(a => a.ReceiptNumber == ReceiptNumber))
                    {
                        if (receivelistredis.Where(c => c.ReceiptNumber == item.ReceiptNumber && c.str2 == item.str2
                         && c.SKU == item.SKU).Count() == 0)
                        {
                            difflistredis.Add(new ReceiptDetail
                            {
                                BoxNumber = item.str2,
                                SKU = item.SKU,
                                QtyDiff = item.QtyExpected
                            });

                        }
                        else
                        {
                            foreach (var itemdetail in receivelistredis.Where(c => c.ReceiptNumber == item.ReceiptNumber && c.str2 == item.str2
                        && c.SKU == item.SKU).
                            GroupBy(c => new { BoxNumber = c.str2, SKU = c.SKU })
                                    .Select(a => new { BoxNumber = a.Key.BoxNumber, SKU = a.Key.SKU, QtyReceived = a.Sum(c => c.QtyReceived) }))


                                if (item.QtyExpected != itemdetail.QtyReceived)
                                {
                                    difflistredis.Add(new ReceiptDetail
                                    {
                                        BoxNumber = item.str2,
                                        SKU = item.SKU,
                                        QtyDiff = item.QtyExpected - itemdetail.QtyReceived
                                    });
                                    break;
                                }
                        }



                    }
                }
                else
                {
                    foreach (var item in receiptlistredis.Where(a => a.ReceiptNumber == ReceiptNumber))
                    {
                        difflistredis.Add(new ReceiptDetail
                        {
                            BoxNumber = item.str2,
                            SKU = item.SKU,
                            QtyDiff = item.QtyExpected
                        });
                    }
                }
                difflistredis.Where(c => c.QtyDiff > 0);
                if (difflistredis.Count > 0)
                {
                    return Json(new { Code = 1, data = difflistredis });
                }
                else
                {
                    return Json(new { Code = 0 });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Code = 2 });
            }


        }

        /// <summary>
        /// 查看单差异退货
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public JsonResult CheckReceiptReceivingByReceiptNumberBack(string ReceiptNumber)
        {
            List<ReceiptDetail> receiptlistredis = new List<ReceiptDetail>();
            List<ReceiptReceiving> receivelistredis = new List<ReceiptReceiving>();
            List<ReceiptDetail> difflistredis = new List<ReceiptDetail>();
            try
            {
                if (RCommon.RedisOperation.Exists("Back:" + ReceiptNumber))
                {
                    receiptlistredis = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber);
                }
                else
                {
                    return Json(new { Code = 3 });
                }
                if (RCommon.RedisOperation.Exists("ReceiptReceivingBack:" + ReceiptNumber))
                {
                    receivelistredis = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceivingBack:" + ReceiptNumber);
                }



                if (receivelistredis.Where(c => c.ReceiptNumber == ReceiptNumber).Count() > 0)
                {
                    foreach (var item in receiptlistredis.Where(a => a.ReceiptNumber == ReceiptNumber))
                    {
                        if (receivelistredis.Where(c => c.ReceiptNumber == item.ReceiptNumber && c.str2 == item.str2
                         && c.SKU == item.SKU).Count() == 0)
                        {
                            difflistredis.Add(new ReceiptDetail
                            {
                                BoxNumber = item.str2,
                                SKU = item.SKU,
                                QtyDiff = item.QtyExpected
                            });

                        }
                        else
                        {
                            foreach (var itemdetail in receivelistredis.Where(c => c.ReceiptNumber == item.ReceiptNumber && c.str2 == item.str2
                        && c.SKU == item.SKU).
                            GroupBy(c => new { BoxNumber = c.str2, SKU = c.SKU })
                                    .Select(a => new { BoxNumber = a.Key.BoxNumber, SKU = a.Key.SKU, QtyReceived = a.Sum(c => c.QtyReceived) }))


                                if (item.QtyExpected != itemdetail.QtyReceived)
                                {
                                    difflistredis.Add(new ReceiptDetail
                                    {
                                        BoxNumber = item.str2,
                                        SKU = item.SKU,
                                        QtyDiff = item.QtyExpected - itemdetail.QtyReceived
                                    });
                                    break;
                                }
                        }



                    }
                }
                else
                {
                    foreach (var item in receiptlistredis.Where(a => a.ReceiptNumber == ReceiptNumber))
                    {
                        difflistredis.Add(new ReceiptDetail
                        {
                            BoxNumber = item.str2,
                            SKU = item.SKU,
                            QtyDiff = item.QtyExpected
                        });
                    }
                }
                difflistredis.Where(c => c.QtyDiff > 0);
                if (difflistredis.Count > 0)
                {
                    return Json(new { Code = 1, data = difflistredis });
                }
                else
                {
                    return Json(new { Code = 0 });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Code = 2 });
            }


        }
        /// <summary>
        /// 查看自己已上架的
        /// 
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public JsonResult CheckReceiptReceivingZJ(string ReceiptNumber)
        {
            List<ReceiptReceiving> receivelistredis = new List<ReceiptReceiving>();
            try
            {
                if (RCommon.RedisOperation.Exists("ReceiptReceiving:" + ReceiptNumber))
                {
                    receivelistredis = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber).Where(c => c.Creator == Session["Name"].ToString()).
                         GroupBy(c => new { str2 = c.str2, SKU = c.SKU, Location = c.Location })
                                    .Select(a => new ReceiptReceiving { str2 = a.Key.str2, SKU = a.Key.SKU, Location = a.Key.Location, QtyReceived = a.Sum(c => c.QtyReceived) }).ToList();
                    return Json(new { Code = 1, data = receivelistredis });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, data = ex.Message });
            }
            return Json(new { Code = 0, data = "" });
        }
        /// <summary>
        /// 查看自己已上架的退货
        /// 
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public JsonResult CheckReceiptReceivingZJBack(string ReceiptNumber)
        {
            List<ReceiptReceiving> receivelistredis = new List<ReceiptReceiving>();
            try
            {
                if (RCommon.RedisOperation.Exists("ReceiptReceivingBack:" + ReceiptNumber))
                {
                    receivelistredis = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceivingBack:" + ReceiptNumber).Where(c => c.Creator == Session["Name"].ToString()).
                         GroupBy(c => new { str2 = c.str2, SKU = c.SKU, Location = c.Location })
                                    .Select(a => new ReceiptReceiving { str2 = a.Key.str2, SKU = a.Key.SKU, Location = a.Key.Location, QtyReceived = a.Sum(c => c.QtyReceived) }).ToList();
                    return Json(new { Code = 1, data = receivelistredis });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, data = ex.Message });
            }
            return Json(new { Code = 0, data = "" });
        }
        /// <summary>
        /// RF盘点查看库位上已扫描的明细
        /// </summary>
        /// <param name="Location"></param>
        /// <returns></returns>
        public JsonResult CheckLocationScaned(string PDNumber, string Location)
        {
            List<WarehouseCheckDetail> warehouseCheckDetails = new List<WarehouseCheckDetail>();
            try
            {
                if (RCommon.RedisOperation.Exists("PD:" + PDNumber + ":" + Session["Name"].ToString()))
                {
                    warehouseCheckDetails = RCommon.RedisOperation.GetList<List<WarehouseCheckDetail>>("PD:" + PDNumber + ":" + Session["Name"].ToString()).Where(c => c.Location == Location).
                         GroupBy(c => new { SKU = c.SKU })
                                    .Select(a => new WarehouseCheckDetail { SKU = a.Key.SKU, ActualQty = a.Sum(c => c.ActualQty) }).ToList();
                    return Json(new { Code = 1, data = warehouseCheckDetails });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, data = ex.Message });
            }
            return Json(new { Code = 0, data = "" });
        }
        /// <summary>
        /// RF盘点扫错了 清空扫错的SKU重新扫
        /// </summary>
        /// <param name="PDNumber"></param>
        /// <param name="Location"></param>
        /// <param name="SKU"></param>
        /// <returns></returns>
        public JsonResult DeleteSKUAndCheckLocationScaned(string PDNumber, string Location, string SKU)
        {
            List<WarehouseCheckDetail> warehouseCheckDetails = new List<WarehouseCheckDetail>();
            try
            {
                if (RCommon.RedisOperation.Exists("PD:" + PDNumber + ":" + Session["Name"].ToString()))
                {
                    warehouseCheckDetails = RCommon.RedisOperation.GetList<List<WarehouseCheckDetail>>("PD:" + PDNumber + ":" + Session["Name"].ToString());
                    foreach (var item in warehouseCheckDetails)
                    {
                        if (item.Location == Location && item.SKU == SKU)
                        {
                            warehouseCheckDetails.Remove(item);
                            RCommon.RedisOperation.SetList("PD:" + PDNumber + ":" + Session["Name"].ToString(), warehouseCheckDetails);
                            return Json(new { Code = 1, data = warehouseCheckDetails.Where(c => c.Location == Location).ToList() });
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = -1, data = ex.Message });
            }
            return Json(new { Code = 0, data = "" });
        }

        /// <summary>
        /// 查看箱差异
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public JsonResult CheckReceiptReceiving(string ReceiptNumber, string BoxNumber)
        {
            List<ReceiptDetail> receiptlistredis = new List<ReceiptDetail>();
            List<ReceiptReceiving> receivelistredis = new List<ReceiptReceiving>();
            List<ReceiptDetail> difflistredis = new List<ReceiptDetail>();
            try
            {
                if (RCommon.RedisOperation.Exists(ReceiptNumber))
                {
                    receiptlistredis = RCommon.RedisOperation.GetList<List<ReceiptDetail>>(ReceiptNumber);
                }
                if (RCommon.RedisOperation.Exists("ReceiptReceiving:" + ReceiptNumber))
                {
                    receivelistredis = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber);
                }



                if (receivelistredis.Where(c => c.ReceiptNumber == ReceiptNumber && c.str2 == BoxNumber).Count() > 0)
                {
                    foreach (var item in receiptlistredis.Where(a => a.str2 == BoxNumber))
                    {
                        if (receivelistredis.Where(c => c.ReceiptNumber == item.ReceiptNumber && c.str2 == item.str2
                         && c.SKU == item.SKU).Count() == 0)
                        {
                            difflistredis.Add(new ReceiptDetail
                            {
                                SKU = item.SKU,
                                QtyDiff = item.QtyExpected
                            });

                        }
                        else
                        {

                            foreach (var itemdetail in receivelistredis.Where(c => c.ReceiptNumber == item.ReceiptNumber && c.str2 == BoxNumber && c.SKU == item.SKU).
                            GroupBy(c => new { BoxNumber = c.str2, SKU = c.SKU })
                                    .Select(a => new { BoxNumber = a.Key.BoxNumber, SKU = a.Key.SKU, QtyReceived = a.Sum(c => c.QtyReceived) }))
                            {

                                if (item.QtyExpected != itemdetail.QtyReceived)
                                {
                                    difflistredis.Add(new ReceiptDetail
                                    {

                                        SKU = item.SKU,
                                        QtyDiff = item.QtyExpected - itemdetail.QtyReceived
                                    });
                                }
                            }
                        }

                    }
                }
                else
                {
                    foreach (var item in receiptlistredis.Where(a => a.str2 == BoxNumber))
                    {
                        difflistredis.Add(new ReceiptDetail
                        {
                            SKU = item.SKU,
                            QtyDiff = item.QtyExpected
                        });
                    }
                }
                difflistredis.Where(c => c.QtyDiff > 0);
                if (difflistredis.Count > 0)
                {
                    return Json(new { Code = 1, data = difflistredis });
                }
                else
                {
                    return Json(new { Code = 0 });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Code = 2 });
            }


        }

        /// <summary>
        /// 查看箱差异退货
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public JsonResult CheckReceiptReceivingBack(string ReceiptNumber, string BoxNumber)
        {
            List<ReceiptDetail> receiptlistredis = new List<ReceiptDetail>();
            List<ReceiptReceiving> receivelistredis = new List<ReceiptReceiving>();
            List<ReceiptDetail> difflistredis = new List<ReceiptDetail>();
            try
            {
                if (RCommon.RedisOperation.Exists("Back:" + ReceiptNumber + ":" + BoxNumber))
                {
                    receiptlistredis = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber + ":" + BoxNumber);
                    foreach (var item in receiptlistredis)
                    {
                        difflistredis.Add(new ReceiptDetail
                        {
                            SKU = item.SKU,
                            QtyDiff = item.QtyExpected - item.QtyReceived
                        });
                    }
                }

                difflistredis.Where(c => c.QtyDiff > 0);
                if (difflistredis.Count > 0)
                {
                    return Json(new { Code = 1, data = difflistredis });
                }
                else
                {
                    return Json(new { Code = 0 });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Code = 2 });
            }


        }

        /// <summary>
        /// 上架完成更新入库单状态
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public string UpdateReceiptStatus(string ReceiptNumber)
        {
            string msg = "";
            bool response = false;
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            try
            {
                response = new ReceiptManagementService().UpdateReceiptStatus(ReceiptNumber, Session["Name"].ToString());
                if (response)
                {
                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "入库单管理";
                    operation.Operation = "入库单-上架信息导入";
                    operation.OrderType = "ReceiptReceiving";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = Session["Name"].ToString();
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = Convert.ToInt32(Session["ProjectID"]);
                    operation.ProjectName = Session["ProjectName"].ToString();
                    operation.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                    operation.CustomerName = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber).Select(c => c.CustomerName).FirstOrDefault().ToString();
                    operation.WarehouseName = Session["WareHouseName"].ToString();
                    operation.OrderNumber = ReceiptNumber;
                    operation.ExternOrderNumber = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber).Select(c => c.ExternReceiptNumber).FirstOrDefault().ToString();
                    logs.Add(operation);
                    var res = new LogOperationService().AddLogOperation(logs);
                    if (res.IsSuccess)
                    {
                        RCommon.RedisOperation.Del(ReceiptNumber);
                        RCommon.RedisOperation.Del("ReceiptReceiving:" + ReceiptNumber);
                    }
                    msg = "1";
                }
            }
            catch (Exception ex)
            {
                msg = "-1";
            }
            return msg;

        }

        /// <summary>
        /// 上架完成更新入库单状态
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public string UpdateReceiptStatusBack(string ReceiptNumber)
        {
            string msg = "";
            bool response = false;
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            List<SKURF> listboxnumberclosebox = new List<SKURF>();
            try
            {
                response = new ReceiptManagementService().UpdateReceiptStatus(ReceiptNumber, Session["Name"].ToString());
                if (response)
                {
                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "入库单管理";
                    operation.Operation = "入库单-上架信息导入";
                    operation.OrderType = "ReceiptReceiving";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = Session["Name"].ToString();
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = Convert.ToInt32(Session["ProjectID"]);
                    operation.ProjectName = Session["ProjectName"].ToString();
                    operation.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                    operation.CustomerName = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceivingBack:" + ReceiptNumber).Select(c => c.CustomerName).FirstOrDefault().ToString();
                    operation.WarehouseName = Session["WareHouseName"].ToString();
                    operation.OrderNumber = ReceiptNumber;
                    operation.ExternOrderNumber = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceivingBack:" + ReceiptNumber).Select(c => c.ExternReceiptNumber).FirstOrDefault().ToString();
                    logs.Add(operation);
                    var res = new LogOperationService().AddLogOperation(logs);
                    if (res.IsSuccess)
                    {
                        string AsnNumber = RCommon.RedisOperation.GetList<List<ReceiptDetail>>("Back:" + ReceiptNumber).Select(c => c.ASNNumber).FirstOrDefault();
                        List<ASNDetail> listasndetail = new List<ASNDetail>();
                        List<SKURF> listboxnumber = new List<SKURF>();
                        ReceiptViewModel vm = new ReceiptViewModel();
                        //移除分SKU产生的redis
                        if (RCommon.RedisOperation.Exists("SKU:" + AsnNumber))
                        {
                            listasndetail = RCommon.RedisOperation.GetList<List<ASNDetail>>("SKU:" + AsnNumber);
                            foreach (var item in listasndetail.GroupBy(a => a.SKU).Select(c => new ASNDetail() { SKU = c.Key }))
                            {
                                foreach (var itemgoodstype in vm.ProductLevel)
                                {
                                    if (RCommon.RedisOperation.Exists("SKU:" + AsnNumber + ":" + itemgoodstype.Text))
                                    {
                                        RCommon.RedisOperation.Del("SKU:" + AsnNumber + ":" + itemgoodstype.Text);
                                    }

                                    if (RCommon.RedisOperation.Exists("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text))
                                    {
                                        listboxnumber = RCommon.RedisOperation.GetList<List<SKURF>>("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text);
                                        foreach (var itemboxNum in listboxnumber)
                                        {
                                            if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber))
                                            {
                                                RCommon.RedisOperation.Del("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber);
                                            }
                                        }
                                        RCommon.RedisOperation.Del("SKU:" + item.SKU + ":" + AsnNumber + ":" + itemgoodstype.Text);
                                    }
                                }
                            }
                            RCommon.RedisOperation.Del("SKU:" + AsnNumber);
                        }

                        if (RCommon.RedisOperation.Exists("ABC:" + AsnNumber))
                        {
                            RCommon.RedisOperation.Del("ABC:" + AsnNumber);
                        }
                        if (RCommon.RedisOperation.Exists("CloseBox:" + AsnNumber))
                        {
                            RCommon.RedisOperation.Del("CloseBox:" + AsnNumber);
                        }
                        //移除分款产生的redis
                        if (RCommon.RedisOperation.Exists("Article:" + AsnNumber))
                        {
                            listasndetail = RCommon.RedisOperation.GetList<List<ASNDetail>>("SKU:" + AsnNumber);
                            foreach (var itemgoodstype in vm.ProductLevel)
                            {
                                if (RCommon.RedisOperation.Exists("Article:" + AsnNumber + ":" + itemgoodstype.Text))
                                {
                                    RCommon.RedisOperation.Del("Article:" + AsnNumber + ":" + itemgoodstype.Text);
                                }

                            }
                            RCommon.RedisOperation.Del("Article:" + AsnNumber);
                        }

                        //移除装箱的相关redis
                        if (RCommon.RedisOperation.Exists("ScanBoxBoxNumberList:" + AsnNumber))
                        {
                            listboxnumberclosebox = RCommon.RedisOperation.GetList<List<SKURF>>("ScanBoxBoxNumberList:" + AsnNumber);
                            foreach (var itemgoodstype in vm.ProductLevel)
                            {
                                foreach (var itemboxNum in listboxnumberclosebox)
                                {
                                    if (RCommon.RedisOperation.Exists("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber))
                                    {
                                        RCommon.RedisOperation.Del("ScanBox:" + AsnNumber + ":" + itemgoodstype.Text + ":" + itemboxNum.BoxNumber);
                                    }
                                }
                            }
                            RCommon.RedisOperation.Del("ScanBoxBoxNumberList:" + AsnNumber);
                        }



                        RCommon.RedisOperation.Del("Back:" + ReceiptNumber);
                        RCommon.RedisOperation.Del("ReceiptReceivingBack:" + ReceiptNumber);
                    }
                    msg = "1";
                }
            }
            catch (Exception ex)
            {
                msg = "-1";
            }
            return msg;

        }
        /// <summary>
        /// 点击完成同步redis中数据到上架表
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public string InsertIntoReceivingFromRedis(string ReceiptNumber)
        {
            List<ReceiptReceiving> receivelistredis = new List<ReceiptReceiving>();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            try
            {
                if (RCommon.RedisOperation.Exists("ReceiptReceiving:" + ReceiptNumber))
                {
                    receivelistredis = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber);
                }
                else
                {
                    return "2";
                }
                var response = new ReceiptManagementService().InsertReceiptReceiving(receivelistredis, Session["Name"].ToString());

                if (response)
                {
                    WMS_Log_Operation operation = new WMS_Log_Operation();
                    operation.MenuName = "入库单管理";
                    operation.Operation = "入库单-上架信息导入";
                    operation.OrderType = "ReceiptReceiving";
                    operation.Controller = Request.RawUrl;
                    operation.Creator = Session["Name"].ToString();
                    operation.CreateTime = DateTime.Now;
                    operation.ProjectID = Convert.ToInt32(Session["ProjectID"]);
                    operation.ProjectName = Session["ProjectName"].ToString();
                    operation.CustomerID = Convert.ToInt32(Session["CustomerID"]);
                    operation.CustomerName = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber).Select(c => c.CustomerName).FirstOrDefault().ToString();
                    operation.WarehouseName = Session["WareHouseName"].ToString();
                    operation.OrderNumber = ReceiptNumber;
                    operation.ExternOrderNumber = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>("ReceiptReceiving:" + ReceiptNumber).Select(c => c.ExternReceiptNumber).FirstOrDefault().ToString();
                    logs.Add(operation);
                    new LogOperationService().AddLogOperation(logs);
                    RCommon.RedisOperation.Del(ReceiptNumber);
                    RCommon.RedisOperation.Del("ReceiptReceiving:" + ReceiptNumber);
                    return "1";
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
            return "0";
        }

        public string SaveRecDataByStr2(string ReceiptNumber, string JsonData, string str2)
        {
            //SaveData
            //recModelList;
            IEnumerable<ReceiptReceiving> receivelist = JSONStringToList<ReceiptReceiving>(JsonData).AsEnumerable();
            List<ReceiptReceiving> list = new List<ReceiptReceiving>();
            var distinctList = receivelist.GroupBy(rl => rl.LineNumber);
            foreach (var item in distinctList)
            {
                int i = 0;
                foreach (var item2 in item)
                {
                    i++;
                    item2.SkuLineNumber = i.ToString().PadLeft(5, '0');
                    list.Add(item2);
                }
            }
            string msg = "";
            var response = new ReceiptManagementService().InsertReceiptReceivingByStr2(list, Session["Name"].ToString(), str2, out msg);

            if (response && msg == "")
            {
                return "1";
            }
            return msg;
        }

        public JsonResult GetReceiptReceivingByStr2(string str2)
        {
            IEnumerable<ReceiptReceiving> recModelList;
            recModelList = new ReceiptManagementService().GetReceiptReceivingListByStr2(str2, Session["CustomerID"].ToString(), Session["WareHouseIds"].ToString());
            return Json(recModelList.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetReceiptReceivingReceiptNumberAndByStr2(string ReceiptNumber, string str2)
        {
            IEnumerable<ReceiptReceiving> recModelList;
            recModelList = new ReceiptManagementService().GetReceiptReceivingListByReceiptNumberAndStr2(ReceiptNumber, str2, Session["CustomerID"].ToString(), Session["WareHouseIds"].ToString());
            return Json(recModelList.ToList(), JsonRequestBehavior.AllowGet);
        }
        public string IsExist(string receiptnum)
        {
            bool bl = new ReceiptManagementService().IsExist(receiptnum);
            if (bl)
                return "True";
            else
                return "False";
        }
        public string GetTarLocation(string ReceiptNumber, string SKU)
        {
            return new ReceiptManagementService().GetTarLocation(ReceiptNumber, SKU);
        }
        public ActionResult YXDRBoxNumber(string CustomerID, string WareHouseName, string WareHouseID)
        {

            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            return View();
        }
        /// <summary>
        /// RF领用箱
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        public ActionResult ReceiptGetBox(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            var getShelvesByConditionResponse = new ReceiptManagementService().GetRFLogQty(Session["Name"].ToString());
            vm.ReceiptDetailCollection = getShelvesByConditionResponse.ReceiptDetailCollection;
            return View(vm);
        }
        //领用的时候判断是否扫的是箱号
        public JsonResult CheckBoxNumber_RFGetBox(string BoxNumber)
        {
            GetShelvesByConditionResponse getShelvesByConditionResponse = new GetShelvesByConditionResponse();
            ReceiptReceiving receiptReceiving = new ReceiptReceiving();
            List<WMS_Log_OperationRF> log_OperationRF = new List<WMS_Log_OperationRF>();
            try
            {
                getShelvesByConditionResponse = new ReceiptManagementService().GetReceiptByBoxNum(BoxNumber);
                if (getShelvesByConditionResponse.storesByGetReceipt == null)
                {
                    return Json(new { Code = 4, data = getShelvesByConditionResponse.storesByGetReceipt });//箱子不存在
                }
                if (getShelvesByConditionResponse.Shelves.Count() == 0)//箱子没上过架
                {
                    if (RCommon.RedisOperation.Exists("GetBox:" + BoxNumber))
                    {
                        var response = RCommon.RedisOperation.GetList<ReceiptReceiving>("GetBox:" + BoxNumber);
                        if (response.Creator == Session["Name"].ToString())
                        {
                            return Json(new { Code = 2, data = getShelvesByConditionResponse.storesByGetReceipt });//重复扫描
                        }
                        else
                        {
                            return Json(new { Code = 3, data = getShelvesByConditionResponse.storesByGetReceipt });//当前箱已被别人领用
                        }
                    }
                    else
                    {
                        log_OperationRF.Add(new WMS_Log_OperationRF
                        {
                            LogType = "领用",
                            ReleateNumber = getShelvesByConditionResponse.storesByGetReceipt.ReceiptNumber,
                            PackageNumber = BoxNumber,
                            Creator = Session["Name"].ToString(),
                            CreateTime = DateTime.Now,
                            Remark = "RF领用箱"
                        });

                        var msg = new LogOperationService().AddLogOperationRF(log_OperationRF);
                        if (msg.IsSuccess)
                        {
                            receiptReceiving.Creator = Session["Name"].ToString();
                            receiptReceiving.CreateTime = DateTime.Now;
                            RCommon.RedisOperation.SetList("GetBox:" + BoxNumber, receiptReceiving);
                            return Json(new { Code = 1, data = getShelvesByConditionResponse.storesByGetReceipt });
                        }
                    }
                }
                else
                {
                    return Json(new { Code = 5, data = getShelvesByConditionResponse.storesByGetReceipt });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = 0, data = getShelvesByConditionResponse.storesByGetReceipt });
            }
            return Json(new { Code = 6, data = getShelvesByConditionResponse.storesByGetReceipt });

        }
        /// <summary>
        /// 查看自己领用未上架的箱子
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBoxNumStatusList()
        {
            List<WMS_Log_OperationRF> list = new List<WMS_Log_OperationRF>();
            try
            {
                list = new ReceiptManagementService().GetBoxNumStatusList(Session["Name"].ToString()).ToList();
            }
            catch (Exception ex)
            {
                return Json(new { Code = 0, data = list });
            }
            return Json(new { Code = 1, data = list });
        }
        /// <summary>
        /// 扫描箱号的时候验证是否已领用
        /// </summary>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public string CheckGetBox(string BoxNumber)
        {
            string msg = "";
            try
            {
                if (RCommon.RedisOperation.Exists("GetBox:" + BoxNumber))
                {
                    var response = RCommon.RedisOperation.GetList<ReceiptReceiving>("GetBox:" + BoxNumber);
                    if (response.Creator != Session["Name"].ToString())
                    {
                        msg = "0";//别人领用的 别瞎扫
                    }
                    else
                    {
                        msg = "1";//自个领用的 安全
                    }
                }
                else
                {
                    msg = "2";//未领用
                }
            }
            catch (Exception ex)
            {
                msg = "-1";
            }
            return msg;
        }

        /// <summary>
        /// 阿克苏二维码上架页面
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        public ActionResult ReceiptReceivingMainAkzo(string CustomerID, string WareHouseName, string WareHouseID)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            //查询到货单

            var response = new ReceiptManagementService().GetReceiptList(CustomerID, WareHouseID, "");
            vm.ReceiptCollection = response;
            return View(vm);
        }

        /// <summary>
        /// 退货上架页面（整箱）
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        public ActionResult WholeShelves(string CustomerID, string WareHouseName, string WareHouseID)
        {
            SysLogWriter.Error("开始退货上架页面（整箱）");
            ReceiptViewModel vm = new ReceiptViewModel();
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetReceiptList(CustomerID, WareHouseID, "");
            vm.ReceiptCollection = response;
            return View(vm);
        }

        /// <summary>
        /// 退货上架页面（整箱）
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WholeShelves(ReceiptViewModel vm, string Action)
        {
            SysLogWriter.Error("开始退货上架页面（整箱）");
            ViewBag.UserName = Session["Name"].ToString();
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var response = new ReceiptManagementService().GetReceiptList(Session["CustomerID"].ToString(), Session["WareHouseIDs"].ToString(), vm.SearchCondition.ExternReceiptNumber);
            vm.ReceiptCollection = response;
            return View(vm);
        }


        /// <summary>
        /// 退货上架页面（整箱）
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="WareHouseName"></param>
        /// <param name="WareHouseID"></param>
        /// <param name="ReceiptaNumber"></param>
        /// <returns></returns>
        public ActionResult ReceiptReceivingIndex_Whole(string CustomerID, string WareHouseName, string WareHouseID, string ReceiptNumber)
        {
            ReceiptViewModel vm = new ReceiptViewModel();
            SysLogWriter.Error("开始退货上架页面（整箱）");
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.ReceiptaNumber = ReceiptNumber;
            ViewBag.CustomerID = Session["CustomerID"];
            ViewBag.WareHouseName = Session["WareHouseName"];
            ViewBag.WareHouseID = Session["WareHouseIDs"];
            var responsestatus = new ReceiptManagementService().GetReceipt(ReceiptNumber);
            if (responsestatus.Status != 1 && responsestatus.Status != 3)
            {
                return RedirectToAction("ASNListBack_ReceiptReceiving", "Receipt", new { CustomerID = CustomerID, WareHouseName = WareHouseName, WareHouseID = WareHouseID });
            }
            if (!RCommon.RedisOperation.Exists("Back:" + ReceiptNumber))
            {
                List<ReceiptDetail> ReceiptDetails = new List<ReceiptDetail>();
                var response = new ReceiptManagementService().GetReceiptDetailList_CustomerID(ReceiptNumber, CustomerID, WareHouseName);

                ReceiptDetails = response.ToList();
                ReceiptDetails.ForEach(c => { c.Creator = null; c.CreateTime = null; c.Updator = Session["Name"].ToString(); c.UpdateTime = DateTime.Now; });
                RCommon.RedisOperation.SetList("Back:" + ReceiptNumber, ReceiptDetails);
                foreach (var item in ReceiptDetails.GroupBy(c => c.str2).Select(a => new ReceiptDetail() { str2 = a.Key }))
                {
                    if (!RCommon.RedisOperation.Exists("Back:" + ReceiptNumber + ":" + item.str2))
                    {
                        RCommon.RedisOperation.SetList("Back:" + ReceiptNumber + ":" + item.str2, ReceiptDetails.Where(a => a.str2 == item.str2));
                    }
                }

            }

            return View(vm);
        }



        /// <summary>
        /// 扫描库位，插入数据库，移除当前扫描记录退货仓（整箱）
        /// </summary>
        /// <param name="ReceiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <param name="Location"></param>
        /// <returns></returns>
        public string UpdateReceiptReceivingByLocationBack_Whole(string ReceiptNumber, string BoxNumber, string Location)
        {
            string response = "";
            bool msg;
            string BoxCompleteFlag = "";
            try
            {
                SysLogWriter.Error("UpdateReceiptReceivingByLocationBack_Whole 开始扫描库位，插入数据库，移除当前扫描记录退货仓（整箱）" + Session["Name"].ToString());
                if (Session["WareHouseName"].ToString() != null)
                {
                    SysLogWriter.Error("开始扫描库位，插入数据库，移除当前扫描记录退货仓（整箱）" + Session["Name"].ToString());
                    //ListScaning = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString());
                    int tag = 0;

                    tag = new ReceiptManagementService().UpdateReceiptReceivingByLocationBack_Whole(Session["Name"].ToString(), Session["WareHouseName"].ToString(), ReceiptNumber, BoxNumber, Location);
                    if (tag > 0)
                    {
                        RCommon.RedisOperation.Del("Back:" + ReceiptNumber + ":" + BoxNumber);
                        return "11";
                    }
                    else
                    {
                        return "1";
                    }
                }
            }
            catch (Exception ex)
            {
                SysLogWriter.Error("方法UpdateReceiptReceivingByLocationBack_Whole 失败：" + ex.ToString());
                List<WMS_LogRF_Exception> logRF_Exceptions = new List<WMS_LogRF_Exception>();
                if (RCommon.RedisOperation.Exists("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    logRF_Exceptions = RCommon.RedisOperation.GetList<List<WMS_LogRF_Exception>>("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"));
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptReceivingByLocation",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                else
                {
                    logRF_Exceptions.Add(new WMS_LogRF_Exception()
                    {
                        LogType = "UpdateReceiptReceivingByLocation",
                        ReleateNumber = ReceiptNumber,
                        PackageNumber = BoxNumber,
                        Remark = ex.Message,
                        CreateTime = DateTime.Now,
                        Creator = Session["Name"].ToString()

                    });
                    RCommon.RedisOperation.SetList("RFLog:" + DateTime.Now.ToString("yyyy-MM-dd"), logRF_Exceptions);
                }
                response = "-1";
            }
            return response;
        }
        /// <summary>
        /// Check库位（整箱上架）
        /// </summary>
        /// <param name="Location"></param>
        /// <param name="ReceiptNumber"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public int CheckLocationForAreaBack_Whole(string Location, string ReceiptNumber, string BoxNumber)
        {
            return new ReceiptManagementService().GetAreaForLocationAndStoreBack_Whole(Location, ReceiptNumber, BoxNumber);
        }


        public JsonResult StagingLocation(string ReceiptNumber, string BoxNumber)
        {
            try
            {
                if (Session["WareHouseName"].ToString() != null)
                {
                    SysLogWriter.Error("开始扫描库位，插入数据库，移除当前扫描记录退货仓（整箱）" + Session["Name"].ToString());
                    //ListScaning = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString());
                    int tag = 0;
                    //async Task DemoAsync()
                    //{
                    //    await Task.Run(() =>
                    //    {
                    tag = new ReceiptManagementService().StagingLocation(Session["Name"].ToString(), Session["WareHouseName"].ToString(), ReceiptNumber, BoxNumber);
                    //    });

                    //}
                    if (tag > 0)
                    {
                        return Json(new { Code = 1 });
                    }
                    else
                    {
                        return Json(new { Code = 0 });
                    }


                }
            }
            catch (Exception ex)
            {
                SysLogWriter.Error("StagingLocation,暂存失败，操作人：" + Session["Name"].ToString() + "，操作箱号：" + BoxNumber + "错误：" + ex.ToString());
                //throw; 
                return Json(new { Code = -1 });

            }
            return Json(new { Code = 0 });
            //return new ReceiptManagementService().GetAreaForLocationAndStoreBack_Whole(Location, ReceiptNumber, BoxNumber);
        }

        /// <summary>
        /// 重新推荐库位
        /// </summary>
        /// <returns></returns>
        public JsonResult AgainRecommended(string ReceiptNumber, string BoxNumber)
        {
            try
            {
                if (Session["WareHouseName"].ToString() != null)
                {
                    SysLogWriter.Error("重新推荐上架库位(整箱)" + Session["Name"].ToString());
                    //ListScaning = RCommon.RedisOperation.GetList<List<ReceiptReceiving>>(Session["Name"].ToString());
                    //int tag = 0;
                    //async Task DemoAsync()
                    //{
                    //    await Task.Run(() =>
                    //    {
                    var response = new ReceiptManagementService().AgainRecommended(Session["CustomerID"].ToString(), Session["Name"].ToString(), Session["WareHouseName"].ToString(), ReceiptNumber, BoxNumber);
                    //    });
                    if (!string.IsNullOrEmpty(response))
                    {
                        return Json(new { Code = 0, Data = response });
                    }
                    return Json(new { Code = 0 });
                    //}
                    //if (tag > 0)
                    //{
                    //    return Json(new { Code = 1 });
                    //}
                    //else
                    //{
                    //    return Json(new { Code = 0 });
                    //}


                }
            }
            catch (Exception ex)
            {
                SysLogWriter.Error("AgainRecommended,暂存失败，操作人：" + Session["Name"].ToString() + "，操作箱号：" + BoxNumber + "错误：" + ex.ToString());
                //throw; 
                return Json(new { Code = -1 });

            }
            return Json(new { Code = 0 });
        }
    }
}
