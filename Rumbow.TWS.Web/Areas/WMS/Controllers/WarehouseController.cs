using Runbow.TWS.Web.Areas.WMS.Models.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using SW = System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.MessageContracts;
using UtilConstants = Runbow.TWS.Common.Constants;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using System.Web.Script.Serialization;
using System.Data;
using System.IO;
using MyFile = System.IO.File;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Runbow.TWS.MessageContracts.WMS.Warehouse;
using Runbow.TWS.Entity.WMS.Warehouse;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using Runbow.TWS.Entity.WMS.Log;
using Runbow.TWS.MessageContracts.WMS.JCApi;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Common.Layui;
using Runbow.TWS.Common.Util;
//using MyIndexViewModel = Runbow.TWS.Web.Areas.WMS.Models.Warehouse.IndexViewModel;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class WarehouseController : BaseController
    {
        // GET: /WMS/Warehouse/
        public ActionResult WmsWebApi(string OrderKey)
        {
            string url = "http://192.168.18.249:9010/";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("UserToken", "97674305-71CB-4779-B9C9-F945628FE6C3");
            string result = client.GetStringAsync("api/UpdateOrderStatus/?OrderKey=" + OrderKey).Result;
            ViewBag.Message = result;
            return View();
            //string result = await response.Content.ReadAsStringAsync();
            //if (result == "审核通过")
            //{
            //    ViewBag.Message = "审核通过";
            //    return View();
            //}
            //else
            //{
            //    ViewBag.Message = "审核不通过";
            //    return View();
            //}
        }

        [HttpGet]
        public ActionResult QRCode(long WarehouseID)
        {
            QRCodeViewModel vm = new QRCodeViewModel();
            GetQRCodeByConditonRequest getQRCodeByConditionRequest = new GetQRCodeByConditonRequest();
            QRCodeSearchCondition ws = new QRCodeSearchCondition();
            ws.WarehouseID = WarehouseID;
            ws.ProjectID = base.UserInfo.ProjectID;
            ws.UserID = base.UserInfo.ID;
            vm.SearchCondition = ws;
            ViewBag.WarehouseID = WarehouseID;
            getQRCodeByConditionRequest.SearchCondition = ws;
            var getQRCodeByConditionResponse = new WarehouseService().GetQRCodeByCondition(getQRCodeByConditionRequest);
            if (getQRCodeByConditionResponse.IsSuccess)
            {
                vm.QRCodeCollection = getQRCodeByConditionResponse.Result.QRCodeCollection;
                vm.OperationCollection = getQRCodeByConditionResponse.Result.OperationCollection;
                vm.ChargeCollection = getQRCodeByConditionResponse.Result.ChargeCollection;
                var OperationList = vm.OperationCollection.Select(t => new { Code = t.ID, Name = t.Operation }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name });
                ViewBag.OperationList = OperationList;
                var ChargeList = vm.ChargeCollection.Select(t => new { Code = t.ID, Name = t.ChargingName }).Distinct().Select(c => new SelectListItem() { Value = c.Code.ToString(), Text = c.Name });
                ViewBag.ChargeList = ChargeList;
            }
            return View(vm);
        }

        /// <summary>
        /// 盘点单推送鲸仓
        /// </summary>
        /// <param name="IDS"></param>
        /// <returns></returns>
        public JsonResult CheckSend(string JCRequestList)
        {
            IList<JCAPiResponse> listResponses = new List<JCAPiResponse>();
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            try
            {
                string url = UtilConstants.JCSendAPIAddress + "CheckSend";
                List<JCRequestLists> list = jsonlist<JCRequestLists>(JCRequestList.Substring(18, JCRequestList.Length - 19));
                string res = this.HTTPPost(url, JCRequestList);
                listResponses = jsonlist<JCAPiResponse>(res);
                foreach (var item in listResponses)
                {
                    logs.Add(new WMS_Log_Operation()
                    {
                        MenuName = "盘点单管理",
                        Operation = "盘点单-推送鲸仓",
                        OrderType = "Check",
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
                    MenuName = "盘点单管理",
                    Operation = "盘点单-推送鲸仓",
                    OrderType = "Check",
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

        [HttpPost]
        public string SendMap(string MapTypejson)
        {
            string returns = "";
            string FtpServerIP = UtilConstants.FtpServerIP;
            string FtpRemotePath = UtilConstants.FtpRemotePath;
            string FtpUserID = UtilConstants.FtpUserID;
            string FtpPassword = UtilConstants.FtpPassword;
            FtpWeb f = new FtpWeb(FtpServerIP, FtpRemotePath, FtpUserID, FtpPassword);
            f.ExistsFile(Server.MapPath("~/FTP_WMS/wms_mapdetail.txt"));
            StreamWriter sr = new StreamWriter(Server.MapPath("~/FTP_WMS/wms_mapdetail.txt"), false, Encoding.ASCII);
            try
            {
                sr.Write(MapTypejson);
                sr.Close();
            }
            catch (Exception ex)
            {
                returns = ex.Message;
            }
            returns = f.Upload(Server.MapPath("~/FTP_WMS/wms_mapdetail.txt"));
            return returns;
        }

        public JsonResult GetLocationByGoodShelf(long id, long WarehouseID)
        {
            var LocationList = ApplicationConfigHelper.GetLocationGoodShelfList(WarehouseID);
            var Location = LocationList.Where(c => c.ID == id).Select(item => new { item.GoodsShelvesName, item.Location });
            return Json(Location, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string SaveQRCode(string JsonString, long? WareHouseID, long? Length, long? Width, int Flag, string MapTypejson)
        {
            GetQRCodeByConditonRequest request = new GetQRCodeByConditonRequest();
            request.QRCode = JSONStringToList<QRCodeInfo>(JsonString);
            var s = new WarehouseService().SaveQRCode(request, base.UserInfo.ProjectID, WareHouseID, Length, Width, base.UserInfo.Name, Flag);
            ApplicationConfigHelper.RefreshGetLocationGoodShelfList((long)WareHouseID);
            if (s == "OK")
            {
                SendMap(MapTypejson);
            }
            return s;
        }

        [HttpPost]
        public string EditQRCode(string JsonString, long? WareHouseID, int Flag, string MapTypejson)
        {
            GetQRCodeByConditonRequest request = new GetQRCodeByConditonRequest();
            request.QRCode = JSONStringToList<QRCodeInfo>(JsonString);
            var s = new WarehouseService().EditQRCode(request, base.UserInfo.ProjectID, WareHouseID, Flag);
            if (s == "OK")
            {
                SendMap(MapTypejson);
            }
            return s;
        }

        //1新增,2编辑,3查看
        [HttpGet]
        public ActionResult GoodsShelvesCreate(long? WarehouseID, int ViewType, long ID = 0, long CustomerID = 0)
        {
            GetGoodsShelfByConditonRequest getWarehouseByConditionRequest = new GetGoodsShelfByConditonRequest();
            GoodsShelfsViewModel vm = new GoodsShelfsViewModel();
            GoodsShelfSearchCondition ws = new GoodsShelfSearchCondition();
            vm.ViewType = ViewType;
            vm.SearchCondition = new GoodsShelfSearchCondition();
            vm.SearchCondition.CustomerID = CustomerID;
            ws.ProjectID = base.UserInfo.ProjectID;
            ws.UserID = base.UserInfo.ID;
            ws.ID = ID;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
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

            ViewBag.WarehouseList = WarehouseList;
            if (WarehouseID != null)
            {
                vm.SearchCondition.WareHouseID = WarehouseID.ObjectToInt64();
            }
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.WareHouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            }
            getWarehouseByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getWarehouseByConditionRequest.SearchCondition = ws;
            var responses = new WarehouseService().GetGoodsShelfByID(ID);
            if (responses.IsSuccess)
            {
                vm.SearchCondition = responses.Result.GoodsShelf;
                vm.GoodsShelfCollection = responses.Result.GoodsShelfCollection;
                vm.GoodsShelfRowAndCellCollection = responses.Result.GoodsShelfRowAndCellCollection;
            }
            return View(vm);
        }

        [HttpPost]
        public string GoodsShelvesCreate(string JsonString, string LocationJson, int ViewType, long ID, long CustomerID, long WarehouseID, int Rows, string RowAndCelljsons)
        {
            GetGoodsShelfByConditonRequest request = new GetGoodsShelfByConditonRequest();
            request.GoodsShelf = JSONStringToList<GoodsShelfInfo>(JsonString);
            request.GoodsShelfRowAndCell = JSONStringToList<GoodsShelfInfo>(RowAndCelljsons);
            request.GoodsShelf.Each((i, s) =>
            {
                s.ProjectID = base.UserInfo.ProjectID;
                s.UpdateTime = DateTime.Now;
                s.ID = ID;
                s.Levels = Rows.ToString();
            });
            if (LocationJson != "")
            {
                request.GoodsShelfForLocation = JSONStringToList<GoodsShelfInfo>(LocationJson);
                request.GoodsShelfForLocation.Each((i, s) =>
                {
                    s.ProjectID = base.UserInfo.ProjectID;
                    s.CustomerID = CustomerID;
                    s.WareHouseID = WarehouseID;
                    s.UpdateTime = DateTime.Now;
                    s.ID = ID;
                    s.Location = s.Location == "" ? s.Location : s.Location.Substring(s.Location.IndexOf('|') + 1);
                    s.LevelsNumber = s.LevelsNumber;
                    s.SerialNumber = s.SerialNumber;
                    s.GoodsShelvesName = request.GoodsShelf.Select(c => c.GoodsShelvesName).FirstOrDefault();
                    s.Levels = Rows.ToString();
                });
            }
            string responses;
            if (request.GoodsShelfForLocation != null)
            {
                responses = new WarehouseService().SaveGoodsShelfWithLocation(request, ViewType);
            }
            else
            {
                responses = new WarehouseService().SaveGoodsShelf(request, ViewType);
            }
            //RefreshGoodsShelfInfoInfo
            ApplicationConfigHelper.RefreshGoodsShelfInfoInfo();
            //ApplicationConfigHelper.RefreshGetGoodsShelfList(base.UserInfo.ProjectID, request.GoodsShelf.Select(c => c.CustomerID).FirstOrDefault().ObjectToInt64(), request.GoodsShelf.Select(c => c.WareHouseID).FirstOrDefault().ObjectToInt64());
            return responses;
        }

        [HttpGet]
        public string DeleteGoodsShelf(long ID, long CustomerID, long WarehouseID)
        {
            var responses = new WarehouseService().DeleteGoodsShelf(ID);
            ApplicationConfigHelper.RefreshGetGoodsShelfList(base.UserInfo.ProjectID, CustomerID, WarehouseID);
            return responses;
        }

        [HttpGet]
        public ActionResult GoodsShelves(GoodsShelfsViewModel vm, int? PageIndex, int? Flag, long customerID = 0, long WarehouseID = 0)
        {
            Session["searchFlag"] = Flag;
            GetGoodsShelfByConditonRequest getWarehouseByConditionRequest = new GetGoodsShelfByConditonRequest();
            GoodsShelfSearchCondition ws = new GoodsShelfSearchCondition();
            vm.SearchCondition = new GoodsShelfSearchCondition();
            ws.ProjectID = base.UserInfo.ProjectID;
            ws.UserID = base.UserInfo.ID;
            getWarehouseByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            if (base.UserInfo.UserType == 0)
            {
                ws.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID != 0)
                {
                    ws.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        ws.CustomerID = customerIDs.First();
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
            ViewBag.WarehouseList = WarehouseList;
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.WareHouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
                ws.WareHouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            }
            if (Flag == 1)
            {
                vm.SearchCondition.CustomerID = customerID;
                vm.SearchCondition.WareHouseID = WarehouseID;
                ws.WareHouseID = WarehouseID;
            }
            getWarehouseByConditionRequest.SearchCondition = ws;
            getWarehouseByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getWarehouseByConditionRequest.PageIndex = PageIndex ?? 0;
            var getGoodsShelfByConditionResponse = new WarehouseService().GetGoodsShelfByCondition(getWarehouseByConditionRequest);
            if (getGoodsShelfByConditionResponse.IsSuccess)
            {
                vm.GoodsShelfCollection = getGoodsShelfByConditionResponse.Result.GoodsShelfCollection;
                vm.PageIndex = getGoodsShelfByConditionResponse.Result.PageIndex;
                vm.PageCount = getGoodsShelfByConditionResponse.Result.PageCount;
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult GoodsShelves(GoodsShelfsViewModel vm, int? PageIndex, string Action, long customerID = 0)
        {
            GetGoodsShelfByConditonRequest getWarehouseByConditionRequest = new GetGoodsShelfByConditonRequest();
            vm.SearchCondition.ProjectID = base.UserInfo.ProjectID;
            vm.SearchCondition.UserID = base.UserInfo.ID;
            getWarehouseByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
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
            ViewBag.WarehouseList = WarehouseList;
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.WareHouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            }
            getWarehouseByConditionRequest.SearchCondition = vm.SearchCondition;
            getWarehouseByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getWarehouseByConditionRequest.PageIndex = PageIndex ?? 0;
            var getGoodsShelfByConditionResponse = new WarehouseService().GetGoodsShelfByCondition(getWarehouseByConditionRequest);
            if (getGoodsShelfByConditionResponse.IsSuccess && (Action == "GoodsShelves" || Action == "查询"))
            {
                vm.GoodsShelfCollection = getGoodsShelfByConditionResponse.Result.GoodsShelfCollection;
                vm.PageIndex = getGoodsShelfByConditionResponse.Result.PageIndex;
                vm.PageCount = getGoodsShelfByConditionResponse.Result.PageCount;
            }
            if (Action == "下载货架模板")
            {
                IEnumerable<Column> columnReceipt;
                var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_GoodsShelves").Count() == 0)
                {
                    columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.
                        First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M002").
                        Tables.TableCollection.First(t => t.Name == "WMS_GoodsShelves").ColumnCollection.Where(c => c.DbColumnName != "Location" && c.DbColumnName != "LevelsNumber" && c.DbColumnName != "SerialNumber" && c.DbColumnName != "Grids" && c.DbColumnName != "AreaName");
                }
                else
                {
                    columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_GoodsShelves").ColumnCollection.Where(c => c.DbColumnName != "Location" && c.DbColumnName != "LevelsNumber" && c.DbColumnName != "SerialNumber" && c.DbColumnName != "Grids" && c.DbColumnName != "AreaName");
                }
                //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID,null).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                //IEnumerable<Table> tables = module.Tables.TableCollection;
                //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_GoodsShelves").ColumnCollection;
                DownGoodShelf(columnReceipt);
            }
            if (Action == "下载货架库位关联模板")
            {
                IEnumerable<Column> columnReceipt;
                var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_GoodsShelves").Count() == 0)
                {
                    columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.
                        First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M002").
                        Tables.TableCollection.First(t => t.Name == "WMS_GoodsShelves").ColumnCollection.Where(c => c.DbColumnName == "AreaName" || c.DbColumnName == "Location" || c.DbColumnName == "GoodsShelvesName" || c.DbColumnName == "LevelsNumber" || c.DbColumnName == "SerialNumber");
                }
                else
                {
                    columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_GoodsShelves").ColumnCollection.Where(c => c.DbColumnName == "AreaName" || c.DbColumnName == "Location" || c.DbColumnName == "GoodsShelvesName" || c.DbColumnName == "LevelsNumber" || c.DbColumnName == "SerialNumber");
                }
                //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID,null).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                //IEnumerable<Table> tables = module.Tables.TableCollection;
                //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_GoodsShelves").ColumnCollection;
                DownGoodShelfAndLocation(columnReceipt);
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult ProductWarning(int? PageIndex, long customerID = 0, long WarehouseID = 0)
        {
            ProductWarningViewModel vm = new ProductWarningViewModel();
            GetProductWarningByConditonRequest getProductWarningRequest = new GetProductWarningByConditonRequest();
            ProductWarningSearchCondition ws = new ProductWarningSearchCondition();
            vm.SearchCondition = new ProductWarningSearchCondition();
            getProductWarningRequest.PageSize = UtilConstants.PAGESIZE;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            if (base.UserInfo.UserType == 0)
            {
                ws.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID != 0)
                {
                    ws.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        ws.CustomerID = customerIDs.First();
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
            ViewBag.WarehouseList = WarehouseList;
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            }
            if (WarehouseID != 0)
            {
                ws.WarehouseID = WarehouseID;
            }
            getProductWarningRequest.SearchCondition = ws;
            var getProductWarningByConditionResponse = new WarehouseService().GetProductWarningByCondition(getProductWarningRequest);
            if (getProductWarningByConditionResponse.IsSuccess)
            {
                vm.ProductWarningCollection = getProductWarningByConditionResponse.Result.ProductWarningCollection;
                vm.PageIndex = getProductWarningByConditionResponse.Result.PageIndex;
                vm.PageCount = getProductWarningByConditionResponse.Result.PageCount;
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult ProductWarning(ProductWarningViewModel vm, int? PageIndex, long customerID = 0, long WarehouseID = 0)
        {
            GetProductWarningByConditonRequest getProductWarningRequest = new GetProductWarningByConditonRequest();
            //ProductWarningSearchCondition ws = new ProductWarningSearchCondition();
            //vm.SearchCondition = new ProductWarningSearchCondition();
            getProductWarningRequest.PageSize = UtilConstants.PAGESIZE;
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
                if (customerID != 0)
                {
                    vm.SearchCondition.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
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
            ViewBag.WarehouseList = WarehouseList;
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            }
            getProductWarningRequest.SearchCondition = vm.SearchCondition;
            var getProductWarningByConditionResponse = new WarehouseService().GetProductWarningByCondition(getProductWarningRequest);
            if (getProductWarningByConditionResponse.IsSuccess)
            {
                vm.ProductWarningCollection = getProductWarningByConditionResponse.Result.ProductWarningCollection;
                vm.PageIndex = getProductWarningByConditionResponse.Result.PageIndex;
                vm.PageCount = getProductWarningByConditionResponse.Result.PageCount;
            }
            return View(vm);
        }

        [HttpPost]
        public string ProductWarningAdd(string IDS, string WarehouseID, string CustomerID, string WarehouseName, string MinNumber, string MaxNumber)
        {
            var message = new WarehouseService().ProductWarningAdd(IDS, WarehouseID, CustomerID, WarehouseName, MinNumber, MaxNumber);
            return message;
        }

        [HttpPost]
        public string ProductWarningDelete(string IDS)
        {
            var message = new WarehouseService().ProductWarningDelete(IDS);
            return message;
        }

        [HttpPost]
        public string ProductWarningEdit(string ID, string MinNumber, string MaxNumber)
        {
            var message = new WarehouseService().ProductWarningEdit(ID, MinNumber, MaxNumber);
            return message;
        }
       
        //1新增,2编辑,3查看
        [HttpGet]
        public ActionResult ProductWarningCreate(long? WarehouseID, int ViewType, long ID = 0, long CustomerID = 0)
        {
            GetProductWarningByConditonRequest getProductWarningRequest = new GetProductWarningByConditonRequest();
            ProductWarningViewModel vm = new ProductWarningViewModel();
            ProductWarningSearchCondition ws = new ProductWarningSearchCondition();
            vm.SearchCondition = new ProductWarningSearchCondition();
            getProductWarningRequest.PageSize = UtilConstants.PAGESIZE;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            if (base.UserInfo.UserType == 0)
            {
                ws.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (CustomerID != 0)
                {
                    ws.CustomerID = CustomerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        ws.CustomerID = customerIDs.First();
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
            ViewBag.WarehouseList = WarehouseList;
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            }
            getProductWarningRequest.SearchCondition = ws;
            var responses = new WarehouseService().GetNoWarningSKUByCondition(getProductWarningRequest);
            //GetProductWarningByID(ID);
            if (responses.IsSuccess)
            {
                //vm.SearchCondition = responses.Result;
                vm.ProductCollection = responses.Result.ProductCollection;
                vm.PageIndex = responses.Result.PageIndex;
                vm.PageCount = responses.Result.PageCount;
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult ProductWarningCreate(ProductWarningViewModel vm, int? PageIndex, long? WarehouseID, long CustomerID = 0)
        {
            GetProductWarningByConditonRequest getProductWarningRequest = new GetProductWarningByConditonRequest();
            //ProductWarningSearchCondition ws = new ProductWarningSearchCondition();
            getProductWarningRequest.PageSize = UtilConstants.PAGESIZE;
            getProductWarningRequest.PageIndex = PageIndex ?? 0;
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
                if (CustomerID != 0)
                {
                    vm.SearchCondition.CustomerID = CustomerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
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
            ViewBag.WarehouseList = WarehouseList;
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            }
            getProductWarningRequest.SearchCondition = vm.SearchCondition;
            var responses = new WarehouseService().GetNoWarningSKUByCondition(getProductWarningRequest);
            if (responses.IsSuccess)
            {
                vm.ProductCollection = responses.Result.ProductCollection;
                vm.PageIndex = responses.Result.PageIndex;
                vm.PageCount = responses.Result.PageCount;
            }
            return View(vm);
        }

        public ActionResult Index(IndexViewModel vm, int? PageIndex, long? customerID)
        {
            GetWarehouseByConditonRequest getWarehouseByConditionRequest = new GetWarehouseByConditonRequest();
            WarehouseSearchCondition ws = new WarehouseSearchCondition();
            ws.Address = null;
            ws.AreaID = 0;
            ws.Description = null;
            ws.ID = 0;
            ws.ProvinceCity = null;
            ws.SearchType = "1";
            ws.WarehouseName = null;
            ws.WarehouseStatus = null;
            ws.WarehouseType = null;
            ws.ProjectID = base.UserInfo.ProjectID;
            ws.UserID = base.UserInfo.ID;
            if (base.UserInfo.UserType == 0)
            {
                ws.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    ws.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        ws.CustomerID = customerIDs.First();
                    }
                }
            }
            getWarehouseByConditionRequest.SearchCondition = ws;
            //getWarehouseByConditionRequest.SearchCondition = vm.SearchCondition;
            getWarehouseByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getWarehouseByConditionRequest.PageIndex = PageIndex ?? 0;
            var getWarehouseByConditionResponse = new WarehouseService().GetWarehouseByCondition(getWarehouseByConditionRequest);
            if (getWarehouseByConditionResponse.IsSuccess)
            {
                vm.WarehouseCollection = getWarehouseByConditionResponse.Result.WarehouseCollection;
                vm.PageIndex = getWarehouseByConditionResponse.Result.PageIndex;
                vm.PageCount = getWarehouseByConditionResponse.Result.PageCount;
                //Session["Warehouse_SearchCondition"] = vm.SearchCondition;
                //Session["Warehouse_PageIndex"] = vm.PageIndex;
            }
            var WarehouseListAll = ApplicationConfigHelper.GetCacheInfo();
            var WarehouseList = WarehouseListAll.Where(c => (c.ProjectID == base.UserInfo.ProjectID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            ViewBag.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetWarehouseList().Where(c => c.ProjectID == base.UserInfo.ProjectID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.WarehouseName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.CustomerID == ws.CustomerID)
                                     .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel vm, int? PageIndex, string Action, long? customerID)
        {
            var getWarehouseByConditionRequest = new GetWarehouseByConditonRequest();
            vm.SearchCondition.ProjectID = base.UserInfo.ProjectID;
            vm.SearchCondition.UserID = base.UserInfo.ID;
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
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }
            if (Action == "查询" || Action == "Index")
            {
                getWarehouseByConditionRequest.SearchCondition = vm.SearchCondition;
                getWarehouseByConditionRequest.PageSize = UtilConstants.PAGESIZE;
                getWarehouseByConditionRequest.PageIndex = PageIndex ?? 0;
            }
            if (Action == "下载库位模板")
            {
                IEnumerable<Column> columnReceipt;
                var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Warehouse_Location").Count() == 0)
                {
                    columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_Warehouse_Location").ColumnCollection.Where(c => c.IsImportColumn == true);
                }
                else
                {
                    columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_Warehouse_Location").ColumnCollection.Where(c => c.IsImportColumn == true);
                }
                //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                //IEnumerable<Table> tables = module.Tables.TableCollection;
                //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_Warehouse_Location").ColumnCollection;
                DownLocation(null, columnReceipt);
            }
            if (Action == "下载库位货架关联模板")
            {
                IEnumerable<Column> columnReceipt;
                var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_Warehouse_Location").Count() == 0)
                {
                    columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").
                        Tables.TableCollection.First(t => t.Name == "WMS_Warehouse_Location").ColumnCollection.Where(c => c.DbColumnName == "AreaName" || c.DbColumnName == "Location" || c.DbColumnName == "GoodsShelf" || c.DbColumnName == "LevelsNumber"
                            || c.DbColumnName == "SerialNumber");
                }
                else
                {
                    columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_Warehouse_Location").ColumnCollection.
                        Where(c => c.DbColumnName == "AreaName" || c.DbColumnName == "Location" || c.DbColumnName == "GoodsShelf" || c.DbColumnName == "LevelsNumber"
                            || c.DbColumnName == "SerialNumber");
                }
                DownLocation2(null, columnReceipt);
            }
            var getWarehouseByConditionResponse = new WarehouseService().GetWarehouseByCondition(getWarehouseByConditionRequest);
            if (getWarehouseByConditionResponse.IsSuccess)
            {
                vm.WarehouseCollection = getWarehouseByConditionResponse.Result.WarehouseCollection;
                vm.PageIndex = getWarehouseByConditionResponse.Result.PageIndex;
                vm.PageCount = getWarehouseByConditionResponse.Result.PageCount;
                //Session["Warehouse_SearchCondition"] = vm.SearchCondition;
                //Session["Warehouse_PageIndex"] = vm.PageIndex;
            }
            var WarehouseListAll = ApplicationConfigHelper.GetCacheInfo();
            var WarehouseList = WarehouseListAll.Where(c => (c.ProjectID == base.UserInfo.ProjectID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            ViewBag.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetWarehouseList().Where(c => c.ProjectID == base.UserInfo.ProjectID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.WarehouseName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.CustomerID == vm.SearchCondition.CustomerID)
                                     .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            return View(vm);
        }

        private void DownLocation(GetReceiptDetailByConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            //IEnumerable<ReceiptDetail> receipts = response.ReceiptDetailCollection2;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }
            DataRow dr1 = dtReceipt.NewRow();
            //dr1["货架"] = "货架1";

            dr1["库区"] = "库区1";
            dr1["库位"] = "A-01-01";
            dr1["库位类型"] = "地面";
            //dr1["第几层"] = "1";
            //dr1["第几格"] = "1";
            dr1["Classification"] = "正常";
            dr1["Handling"] = "Handling1";
            dr1["ABCClassification"] = "A类";
            dr1["最大放货量"] = "999999";
            dr1["是否整箱"] = "0";
            dtReceipt.Rows.Add(dr1);
            dtReceipt.TableName = "库位导入模板";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "库位导入模板");
            EPPlusOperation.ExportByEPPlus(ds.Tables[0], "库位导入模板");
        }

        private void DownLocation2(GetReceiptDetailByConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }
            DataRow dr1 = dtReceipt.NewRow();
            dr1["货架"] = "货架1";
            dr1["库区"] = "库区1";
            dr1["库位"] = "A-01-01";
            dr1["第几层"] = "1";
            dr1["所处层序号"] = "1";//第几格 table_columns 存的是 所处层序号
            dtReceipt.Rows.Add(dr1);
            dtReceipt.TableName = "库位货架关联导入模板";
            ds.Tables.Add(dtReceipt);
            ExportDataToExcelHelper.ExportDataSetToExcel(ds, "库位货架关联导入模板");
            EPPlusOperation.ExportDataSetByEPPlus(ds, "库位货架关联导入模板");
        }

        private void DownGoodShelf(IEnumerable<Column> columnReceipt)
        {
            //IEnumerable<ReceiptDetail> receipts = response.ReceiptDetailCollection2;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            DataTable dtRowAndCell = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }
            DataColumn dc1 = new DataColumn();
            dc1.ColumnName = "货架名";
            dc1.DataType = typeof(string);
            DataColumn dc2 = new DataColumn();
            dc2.ColumnName = "第几层";
            dc2.DataType = typeof(string);
            DataColumn dc3 = new DataColumn();
            dc3.ColumnName = "格数";
            dc3.DataType = typeof(string);
            dtRowAndCell.Columns.Add(dc1);
            dtRowAndCell.Columns.Add(dc2);
            dtRowAndCell.Columns.Add(dc3);
            DataRow dr01 = dtRowAndCell.NewRow();
            dr01[0] = "货架1";
            dr01[1] = "1";
            dr01[2] = "3";
            dtRowAndCell.Rows.Add(dr01);
            DataRow dr1 = dtReceipt.NewRow();
            dr1["货架名"] = "货架1";
            dr1["层数"] = "3";
            //dr1["列数"] = "3";
            dr1["长度"] = "3";
            dr1["宽度"] = "3";
            dr1["高度"] = "3";
            dr1["重量"] = "3";
            dr1["备注"] = "备注1";
            dtReceipt.Rows.Add(dr1);
            dtReceipt.TableName = "货架基础信息";
            dtRowAndCell.TableName = "货架层列信息";
            ds.Tables.Add(dtReceipt);
            ds.Tables.Add(dtRowAndCell);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "货架导入模板");
            EPPlusOperation.ExportByEPPlus(ds.Tables[0], "货架导入模板");
        }

        private void DownGoodShelfAndLocation(IEnumerable<Column> columnReceipt)
        {
            //IEnumerable<ReceiptDetail> receipts = response.ReceiptDetailCollection2;
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            DataTable dtRowAndCell = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }
            DataRow dr1 = dtReceipt.NewRow();
            dr1["货架名"] = "货架1";
            dr1["库区"] = "库区1";
            dr1["库位"] = "库位1";
            //dr1["列数"] = "3";
            dr1["第几层"] = "1";
            dr1["第几格"] = "1";
            dtReceipt.Rows.Add(dr1);
            dtReceipt.TableName = "货架库位关联";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "货架库位关联导入模板");
            EPPlusOperation.ExportByEPPlus(ds.Tables[0], "货架库位关联导入模板");
        }
      
        /// <summary>
        /// 新增界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create(long? ID, int? ViewType)
        {
            WarehouseOperationViewModel vm = new WarehouseOperationViewModel();
            vm.Warehouse = new WarehouseInfo();
            vm.Area = new AreaInfo();
            vm.Warehouse.Areas = new List<AreaInfo>();
            GetWarehouseByIDRequest request = new GetWarehouseByIDRequest();
            Response<GetWarehouseByIDResponse> response = new Response<GetWarehouseByIDResponse>();
            request.ID = ID;
            //response = new WarehouseService().GetWarehouseByID(request);
            response = new WarehouseService().GetWarehouseAndAreaByID(request);
            ///根据ID判断编辑修改状态
            if (ID == null)
            {
                vm.ViewType = 1;   //新增操作
            }
            else
            {
                if (ViewType == 0)
                {//若为查看模式，则更新数据
                    vm.ViewType = 0;
                }
                else
                {//否则认为是修改模式
                    vm.ViewType = 2;   //修改操作
                }
            }
            vm.Warehouse = response.Result.Warehouse;
            vm.Warehouse.Areas = response.Result.Areas;
            //vm.Area.Locations = response.Result.Locations;
            return View(vm);
        }
       
        /// <summary>
        /// 新增、修改界面提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(long? ID, WarehouseOperationViewModel vm)
        {
            vm.Area = new AreaInfo();  //初始化库区
            ///新增或修改库区
            AddOrUpdateWarehouseRequest requestOperate = new AddOrUpdateWarehouseRequest();
            vm.Warehouse.ProjectID = base.UserInfo.ProjectID;
            requestOperate.warehouse = vm.Warehouse;
            Response<GetWarehouseByIDResponse> responseOperate = new WarehouseService().AddOrUpdateWarehouse(requestOperate);
            vm.Warehouse = responseOperate.Result.Warehouse;
            vm.Warehouse.Areas = responseOperate.Result.Areas;
            vm.Area.Locations = responseOperate.Result.Locations;
            vm.ViewType = 0;  //新增或修改完成后，查看
            ApplicationConfigHelper.RefreshGetWarehouseList();  //刷新仓库下拉列表
            return View(vm);
        }
      
        /// <summary>
        /// 库区新增、编辑视图
        /// </summary>
        /// <param name="ID">库区ID</param>
        /// <param name="ViewType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AreaCreate(long? ID, long? WarehouseID, int? ViewType)
        {
            WarehouseOperationViewModel avm = new WarehouseOperationViewModel();
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            var WarehouseListAll = ApplicationConfigHelper.GetCacheInfo();
            var WarehouseList = WarehouseListAll.Where(c => (c.ProjectID == base.UserInfo.ProjectID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            avm.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            ///为空则赋值
            if (avm.Area == null)
            {
                avm.Area = new AreaInfo();
            }
            if (avm.Warehouse == null)
            {
                avm.Warehouse = new WarehouseInfo();
            }
            ViewData["ViewDataAreaID"] = ID ?? 0;
            ViewData["ViewDataWarehouseID"] = WarehouseID ?? 0;
            if (ViewType == 1)
            {
                //新增操作，只有仓库信息，测试应该调用仓库ID获取仓库信息
                Response<GetWarehouseByIDResponse> responseAdd = new Response<GetWarehouseByIDResponse>();
                GetWarehouseByIDRequest requestAdd = new GetWarehouseByIDRequest();
                requestAdd.ID = WarehouseID;
                responseAdd = new WarehouseService().GetWarehouseByID(requestAdd);
                avm.Warehouse = responseAdd.Result.Warehouse;
            }
            else
            {
                //编辑操作，已知库区ID，获取库区信息
                GetWarehouseAreaByIDRequest request = new GetWarehouseAreaByIDRequest();
                Response<GetWarehouseAreaByIDResponse> response = new Response<GetWarehouseAreaByIDResponse>();
                request.ID = ID ?? 0;
                response = new WarehouseService().GetWarehouseAreaByID(request);
                avm.Warehouse = response.Result.Warehouse;
                avm.Area = response.Result.Area;
                avm.Area.Locations = response.Result.Locations;
            }
            ViewBag.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetWarehouseList().Where(c => c.ProjectID == base.UserInfo.ProjectID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.WarehouseName });
            avm.ViewType = ViewType;
            return View(avm);
        }
       
        /// <summary>
        /// 库区新增提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AreaCreate(long? ID, WarehouseOperationViewModel vm)
        {
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            var WarehouseListAll = ApplicationConfigHelper.GetCacheInfo();
            var WarehouseList = WarehouseListAll.Where(c => (c.ProjectID == base.UserInfo.ProjectID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            vm.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            ///新增或修改
            //提交库区信息
            AddOrUpdateWarehouseAreaRequest request = new AddOrUpdateWarehouseAreaRequest();
            request.Area = vm.Area;   //库区里面已经包含仓库ID
            if (request.Area.WarehouseID == 0)
            {///包装仓库ID一直能够获取
                request.Area.WarehouseID = vm.Warehouse.ID;
            }
            Response<GetWarehouseAreaByIDResponse> response = new WarehouseService().AddOrUpdateWarehouseArea(request);
            if (response.IsSuccess)
            {///正确
                vm.Warehouse = response.Result.Warehouse;
                vm.Area = response.Result.Area;
                vm.Area.Locations = response.Result.Locations;
                vm.ViewType = 0;  //新增或修改完成后，查看
                ApplicationConfigHelper.RefreshGetWarehouseAreaList(0);  //刷新下拉列表
            }
            else
            {///错误
                Response.Write("<script>alert('" + response.SuccessMessage + "'); window.history.back();</script>");
                Response.End();
            }
            return View(vm);
        }
      
        /// <summary>
        /// 库位新增视图
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ViewType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LocationCreate(long? ID, long? AreaID, int? ViewType, string WLSearchCondition_SearchType, long WarehouseID = 0)
        {
            WarehouseOperationViewModel lvm = new WarehouseOperationViewModel();
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            var WarehouseListAll = ApplicationConfigHelper.GetCacheInfo();
            var WarehouseList = WarehouseListAll.Where(c => (c.ProjectID == base.UserInfo.ProjectID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            var GoodsShelfListAll = ApplicationConfigHelper.GetGoodsShelfInfo();
            var GoodsShelfList = GoodsShelfListAll.Where(c => (c.ProjectID == base.UserInfo.ProjectID && c.WareHouseID == (WarehouseID == 0 ? c.WareHouseID : WarehouseID))).Select(t => new { ID = t.ID, Name = t.GoodsShelvesName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            lvm.WarehouseList = WarehouseList;
            lvm.GoodsShelfList = GoodsShelfList;
            //ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //.Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            ///为空则赋值
            if (lvm.Warehouse == null)
            {
                lvm.Warehouse = new WarehouseInfo();
            }
            if (lvm.Area == null)
            {
                lvm.Area = new AreaInfo();
            }
            ViewData["ViewDataAreaID"] = AreaID ?? 0;
            if (lvm.Location == null)
            {
                lvm.Location = new LocationInfo();
            }
            ViewData["ViewDataLocationID"] = ID ?? 0;

            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ParaLocationType_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ParaLocationType");
            }
            List<SelectListItem> stParaLocationType = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                stParaLocationType.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            lvm.ParaLocationTypeList = stParaLocationType;


            if (AreaID != 0 && AreaID != null)
            {///新增操作时，由于没有库位ID，只能通过库区ID获取对应的所属仓库和库区信息
                Response<GetWarehouseAreaByIDResponse> responseAdd = new Response<GetWarehouseAreaByIDResponse>();
                GetWarehouseAreaByIDRequest requestAdd = new GetWarehouseAreaByIDRequest();
                requestAdd.ID = AreaID ?? 0;
                responseAdd = new WarehouseService().GetWarehouseAreaByID(requestAdd);
                lvm.Warehouse = responseAdd.Result.Warehouse;
                lvm.Area = responseAdd.Result.Area;
                lvm.Area.Locations = responseAdd.Result.Locations;
            }
            if (ID != 0 && ID != null)
            {///默认是编辑操作
                Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
                GetWarehouseLocationByIDRequest request = new GetWarehouseLocationByIDRequest();
                request.ID = ID;
                response = new WarehouseService().GetWarehouseLocationByID(request);
                lvm.Warehouse = response.Result.Warehouse;
                lvm.Area = response.Result.Area;
                lvm.Location = response.Result.Location;
                lvm.GoodsShelf = response.Result.GoodsShelf;
                lvm.GoodsShelf.ID = response.Result.GoodsShelf.ID;
            }
            ViewData["WLSearchCondition_SearchType"] = WLSearchCondition_SearchType;
            lvm.ViewType = ViewType;
            return View(lvm);
        }
       
        /// <summary>
        /// 库位新增提交
        /// </summary>
        /// <param name="lvm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LocationCreate(long? ID, WarehouseOperationViewModel lvm, string WLSearchCondition_SearchType)
        {
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            var WarehouseListAll = ApplicationConfigHelper.GetCacheInfo();
            var WarehouseList = WarehouseListAll.Where(c => (c.ProjectID == base.UserInfo.ProjectID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            var GoodsShelfListAll = ApplicationConfigHelper.GetGoodsShelfInfo();
            var GoodsShelfList = GoodsShelfListAll.Where(c => (c.ProjectID == base.UserInfo.ProjectID && c.WareHouseID == (lvm.GoodsShelf.ID == 0 ? c.WareHouseID : lvm.GoodsShelf.ID))).Select(t => new { ID = t.ID, Name = t.GoodsShelvesName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            lvm.WarehouseList = WarehouseList;
            lvm.GoodsShelfList = GoodsShelfList;
            //ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            if (lvm.Warehouse == null)
            {
                lvm.Warehouse = new WarehouseInfo();
            }
            if (lvm.Area == null)
            {
                lvm.Area = new AreaInfo();
            }
            if (lvm.Location == null)
            {
                lvm.Location = new LocationInfo();
            }
            if (lvm.GoodsShelf == null)
            {
                lvm.GoodsShelf = new GoodsShelfInfo();
            }
            Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
            AddOrUpdateWarehouseLocationRequest request = new AddOrUpdateWarehouseLocationRequest();
            request.Location = lvm.Location;
            request.Location.WarehouseID = lvm.Warehouse.ID;
            request.Location.AreaID = lvm.Area.ID;
            request.Location.GoodsShelfID = lvm.GoodsShelf.ID;
            if (ID != 0)
            {///如果ID不为0，说明为编辑操作
                request.Location.ID = ID ?? 0;
            }
            request.Location.CreateTime = DateTime.Now;
            response = new WarehouseService().AddOrUpdateWarehouseLocation(request);
            if (response.IsSuccess)
            {///正确
                lvm.Warehouse = response.Result.Warehouse;
                lvm.Area = response.Result.Area;
                lvm.Location = response.Result.Location;
                lvm.ViewType = 0;  //新增或修改完成后，查看
                lvm.GoodsShelf = response.Result.GoodsShelf;
                string WarehouseName = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(a => a.WarehouseID == lvm.Warehouse.ID).Select(b => b.WarehouseName).FirstOrDefault();
                ApplicationConfigHelper.RefreshCacheInfo("WarehouseLocationList_" + WarehouseName, WarehouseName);
            }
            else
            {
                Response.Write("<script>alert('" + response.SuccessMessage + "'); window.history.back();</script>");
                Response.End();
            }
            ViewData["WLSearchCondition_SearchType"] = WLSearchCondition_SearchType;

            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ParaLocationType_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ParaLocationType");
            }
            List<SelectListItem> stParaLocationType = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                stParaLocationType.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            lvm.ParaLocationTypeList = stParaLocationType;

            return View(lvm);
        }

        [HttpPost]
        public string WarehouseDelete(string Warehouse_ID)
        {
            Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
            AddOrUpdateWarehouseLocationRequest request = new AddOrUpdateWarehouseLocationRequest();
            response = new WarehouseService().WarehouseDelete(Warehouse_ID);
            ApplicationConfigHelper.RefreshCacheInfo();
            return response.IsSuccess.ToString();
        }

        [HttpPost]
        public string AreaDelete(string Area_ID)
        {
            Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
            AddOrUpdateWarehouseLocationRequest request = new AddOrUpdateWarehouseLocationRequest();
            response = new WarehouseService().DeleteArea(Area_ID);
            return response.IsSuccess.ToString();
        }

        [HttpPost]
        public string MapDelete(long WarehouseID)
        {
            string response = "";
            response = new WarehouseService().DeleteMap(WarehouseID);
            string FtpServerIP = UtilConstants.FtpServerIP;
            string FtpRemotePath = UtilConstants.FtpRemotePath;
            string FtpUserID = UtilConstants.FtpUserID;
            string FtpPassword = UtilConstants.FtpPassword;
            FtpWeb f = new FtpWeb(FtpServerIP, FtpRemotePath, FtpUserID, FtpPassword);
            f.Delete("wms_mapdetail.txt");
            return response;
        }

        [HttpPost]
        public string LocationDelete(string Location_ID)
        {
            Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
            AddOrUpdateWarehouseLocationRequest request = new AddOrUpdateWarehouseLocationRequest();
            response = new WarehouseService().DeleteLocation(Location_ID);
            return response.IsSuccess.ToString();
        }
      
        /// <summary>
        /// 根据仓库ID获取库区信息
        /// </summary>
        /// <param name="WarehouseID"></param>
        /// <returns></returns>
        public JsonResult UpdateWarehouseAreaList(long? WarehouseID)
        {
            ///读取值，并返回
            var AreaList = from o in ApplicationConfigHelper.GetWarehouseAreaList(0) //获取库区
                           where o.WarehouseID == WarehouseID
                           select new
                           {
                               o.ID,
                               o.AreaName
                           };
            return Json(AreaList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateWarehouseGoodsShelfList(long? WarehouseID)
        {
            ///读取值，并返回
            var GoodsShelvesList = from o in ApplicationConfigHelper.GetGoodsShelfInfo() //获取库区
                                   where o.WareHouseID == WarehouseID
                                   select new
                                   {
                                       o.ID,
                                       o.GoodsShelvesName
                                   };
            return Json(GoodsShelvesList, JsonRequestBehavior.AllowGet);
        }
      
        /// <summary>
        /// 查询库位
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="PageIndex"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IndexLocation(IndexWLocationViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);
            vm.WarehouseIDD = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ParaLocationType_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ParaLocationType");
            }
            List<SelectListItem> stParaLocationType = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                stParaLocationType.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            vm.LocationType1 = stParaLocationType;

            var getWLocationByConditionRequest = new GetWLocationByConditonRequest();
            if (Action == "查询" || Action == "IndexLocation")
            {
                getWLocationByConditionRequest.WLSearchCondition = vm.WLSearchCondition;
                getWLocationByConditionRequest.PageSize = UtilConstants.PAGESIZE;
                getWLocationByConditionRequest.PageIndex = PageIndex ?? 0;
            }
            if (vm.WLSearchCondition.WarehouseID == null && vm.WarehouseIDD != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var i in vm.WarehouseIDD)
                {
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    vm.WLSearchCondition.WarehouseID = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }

            if (vm.WLSearchCondition.LocationType != null && vm.LocationType1 != null)
            {
                var ss = vm.WLSearchCondition.LocationType;
                StringBuilder sb = new StringBuilder();
                sb.Append("" + ss + "");
            }

            var getWLocationByConditionResponse = new WarehouseService().GetWLocationByCondition(getWLocationByConditionRequest);
            if (getWLocationByConditionResponse.IsSuccess)
            {
                vm.WLocationCollection = getWLocationByConditionResponse.Result.WLocationCollection;
                vm.PageIndex = getWLocationByConditionResponse.Result.PageIndex;
                vm.PageCount = getWLocationByConditionResponse.Result.PageCount;
                //Session["WLocation_SearchCondition"] = vm.WLSearchCondition;
                //Session["WLocation_PageIndex"] = vm.PageIndex;
            }
            long wid = 0;
            try
            {
                wid = vm.WLSearchCondition.WarehouseID == null ? 0 : Convert.ToInt32(vm.WLSearchCondition.WarehouseID);
            }
            catch (Exception)
            {
            }
            vm.WarehouseAreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == wid).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            return View(vm);
        }

        public ActionResult IndexLocation(IndexWLocationViewModel vm, int? PageIndex, int flag = 0)
        {
            vm.WarehouseAreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == null).Select(a => new { a.ID, a.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            vm.WarehouseIDD = null;
            vm.flag = flag;
            var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);

            vm.WarehouseIDD = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //IEnumerable<SelectListItem> WarehouseList = null;
            //var customerIDs = ApplicationConfigHelper.GetApplicationCustomer().Where(m => m.UserID == base.UserInfo.ID).Select(c => c.ID);
            //if (customerIDs != null && customerIDs.Count() >= 0)
            //{
            //    WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerIDs.First() && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
            //                         .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            //}
            ////vm.WarehouseAreaList = WarehouseList;

            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ParaLocationType_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("ParaLocationType");
            }
            List<SelectListItem> stParaLocationType = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                stParaLocationType.Add(new SelectListItem() { Value = w.Code, Text = w.Name });
            }
            vm.LocationType1 = stParaLocationType;

            GetWLocationByConditonRequest getWLocationByConditionRequest = new GetWLocationByConditonRequest();
            WLocationSearchCondition wl = new WLocationSearchCondition();
            wl.ABCClassification = null;
            wl.AreaID = null;
            wl.Category = null;
            wl.Classification = null;
            wl.Handling = null;
            wl.IsMultiLot = null;
            wl.IsMultiSKU = null;
            wl.Location = null;
            wl.LocationLevel = null;
            wl.LocationStatus = null;
            //wl.LocationType = null;
            if (vm.WarehouseIDD != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var i in vm.WarehouseIDD)
                {
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    wl.WarehouseID = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }

            if (vm.LocationType1 != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var i in vm.LocationType1)
                {
                    sb.Append("" + i.Value + ",");
                }
                if (sb.Length > 1)
                {
                    wl.LocationType = sb.Remove(sb.Length - 1, 1).ToString();
                }
            }

            getWLocationByConditionRequest.WLSearchCondition = wl;
            getWLocationByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getWLocationByConditionRequest.PageIndex = PageIndex ?? 0;
            var getWLocationByConditionResponse = new WarehouseService().GetWLocationByCondition(getWLocationByConditionRequest);
            if (getWLocationByConditionResponse.IsSuccess)
            {
                vm.WLocationCollection = getWLocationByConditionResponse.Result.WLocationCollection;
                vm.PageIndex = getWLocationByConditionResponse.Result.PageIndex;
                vm.PageCount = getWLocationByConditionResponse.Result.PageCount;
            }
            return View(vm);
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
        //库位导入
        [HttpPost]
        public string LocationImport(string ID)
        {
            IndexViewModel vm = new IndexViewModel();
            string js = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    if (ds != null && ds.Tables[0] != null)
                    {
                        //IList<LocationInfo> locations = new List<LocationInfo>();
                        //Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
                        AddOrUpdateWarehouseLocationRequest request = new AddOrUpdateWarehouseLocationRequest();
                        StringBuilder sb = new StringBuilder();
                        var areaList = ApplicationConfigHelper.GetWarehouseAreaListByWID(ID.ObjectToInt64());
                        var goodList = ApplicationConfigHelper.GetGoodsShelfList(base.UserInfo.ProjectID, 0, ID.ObjectToInt64());
                        string areString = "";
                        string GoodsString = "";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (areaList.Where(c => c.AreaName == ds.Tables[0].Rows[i]["库区"].ToString()).Select(m => m.ID).Count() == 0)
                            {
                                areString = areString + ds.Tables[0].Rows[i]["库区"].ToString() + ",";
                            }

                            if (ds.Tables[0].Rows[i]["楼层"].ToString() == null || ds.Tables[0].Rows[i]["楼层"].ToString() == "")
                            {
                                return new { result = "下列库区库位楼层为空：【" + ds.Tables[0].Rows[i]["库区"].ToString() + "】【" + ds.Tables[0].Rows[i]["库位"].ToString() + "】", IsSuccess = false }.ToJsonString();
                            }

                            //if (ds.Tables[0].Rows[i]["库位类型"].ToString() == "货架")
                            //{
                            //    if (goodList.Where(c => c.GoodsShelvesName == ds.Tables[0].Rows[i]["货架"].ToString()).Select(m => m.ID).Count() == 0)
                            //    {
                            //        GoodsString = GoodsString + ds.Tables[0].Rows[i]["货架"].ToString() + ",";
                            //    }
                            //}

                            if (ds.Tables[0].Select("库区='" + ds.Tables[0].Rows[i]["库区"].ToString() + "' and   库位='" + ds.Tables[0].Rows[i]["库位"].ToString() + "'").Length >1)
                            {
                                return new { result = "下列库区库位重复：【" + ds.Tables[0].Rows[i]["库区"].ToString() + "】【" + ds.Tables[0].Rows[i]["库位"].ToString() + "】", IsSuccess = false }.ToJsonString();
                            }
                        }
                        if (areString != "")
                        {
                            return new { result = "下列库区不存在：" + areString, IsSuccess = false }.ToJsonString();
                        }
                        // if (GoodsString != "")
                        //{
                        //    return new { result = "<h3><font color='#FF0000'>导入失败,下列货架不存在："+GoodsString+"</font></h3>", IsSuccess = false }.ToJsonString();
                        //}
                        IEnumerable<LocationInfo> locations = this.InitLocationFromDataTable(ds.Tables[0], sb, ID);
                        var groupedlocations = from g in locations
                                               group g by new {
                                                   g.GoodsShelvesName,
                                                   g.LevelsNumber,
                                                   g.SerialNumber,
                                                   g.ID,
                                                   g.GoodsShelfID,
                                                   g.WarehouseName,
                                                   g.WarehouseID,
                                                   g.AreaID,
                                                   g.Location,
                                                   g.CustomerID,
                                                   g.CustomerName,
                                                   g.AreaName,
                                                   g.LocationType,
                                                   g.LocationStatus,
                                                   g.Classification,
                                                   g.Category,
                                                   g.Handling,
                                                   g.ABCClassification,
                                                   g.IsMultiLot,
                                                   g.IsMultiSKU,
                                                   g.LocationLevel,
                                                   g.GoodsPutOrder,
                                                   g.GoodsPickOrder,
                                                   g.Volume,
                                                   g.Weight,
                                                   g.MaxID,
                                                   g.MaxNumber,
                                                   g.Length,
                                                   g.Width,
                                                   g.Height,
                                                   g.X_Coordinate,
                                                   g.Y_Coordinate,
                                                   g.Z_coordinate,
                                                   g.Remark,
                                                   g.Creator,
                                                   g.CreateTime,
                                                   g.Int1,
                                                   g.Str1,
                                                   g.Str2,
                                                   g.Str3
                                               } into r
                                               select new LocationInfo {
                                                   GoodsShelvesName=r.Key.GoodsShelvesName,
                                                   LevelsNumber=r.Key.LevelsNumber,
                                                   SerialNumber = r.Key.SerialNumber,
                                                   ID = r.Key.ID,
                                                   GoodsShelfID = r.Key.GoodsShelfID,
                                                   WarehouseName = r.Key.WarehouseName,
                                                   WarehouseID = r.Key.WarehouseID,
                                                   AreaID = r.Key.AreaID,
                                                   CustomerID = r.Key.CustomerID,
                                                   CustomerName = r.Key.CustomerName,
                                                   AreaName = r.Key.AreaName,
                                                   Location = r.Key.Location,
                                                   LocationType = r.Key.LocationType,
                                                   LocationStatus = r.Key.LocationStatus,
                                                   Classification = r.Key.Classification,
                                                   Category = r.Key.Category,
                                                   Handling = r.Key.Handling,
                                                   ABCClassification = r.Key.ABCClassification,
                                                   IsMultiLot = r.Key.IsMultiLot,
                                                   IsMultiSKU = r.Key.IsMultiSKU,
                                                   LocationLevel = r.Key.LocationLevel,
                                                   GoodsPutOrder = r.Key.GoodsPutOrder,
                                                   GoodsPickOrder = r.Key.GoodsPickOrder,
                                                   Volume = r.Key.Volume,
                                                   Weight = r.Key.Weight,
                                                   MaxID = r.Key.MaxID,
                                                   MaxNumber = r.Key.MaxNumber,
                                                   Length = r.Key.Length,
                                                   Width = r.Key.Width,
                                                   Height = r.Key.Height,
                                                   X_Coordinate = r.Key.X_Coordinate,
                                                   Y_Coordinate = r.Key.Y_Coordinate,
                                                   Z_coordinate = r.Key.Z_coordinate,
                                                   Remark = r.Key.Remark,
                                                   Creator = r.Key.Creator,
                                                   CreateTime = r.Key.CreateTime,
                                                   Int1 = r.Key.Int1,
                                                   Str1 = r.Key.Str1,
                                                   Str2 = r.Key.Str2,
                                                   Str3 = r.Key.Str3
                                               };
                        var response = new WarehouseService().ImportLocation(new AddLocationRequest() { Location = groupedlocations });
                        if (response.IsSuccess)
                        {
                            string WarehouseName = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(a => a.WarehouseID.ToString() == ID).Select(b => b.WarehouseName).FirstOrDefault();
                            ApplicationConfigHelper.RefreshCacheInfo("WarehouseLocationList_" + WarehouseName, WarehouseName);
                            return new { result = "", IsSuccess = true }.ToJsonString();
                        }
                        //else if (response.SuccessMessage.ToString().Contains("有重复"))
                        //{
                        //    return new { result = "<h3><font color='#FF0000'>导入失败！数据库中有重复库位</font></h3>", IsSuccess = false }.ToJsonString();
                        //}
                        else
                        {
                            return new { result = "导入失败！", IsSuccess = false }.ToJsonString();
                        }
                    }
                }
            }
            return new { result = "库位文件读取失败", IsSuccess = false }.ToJsonString();
        }

        [HttpPost]
        public string LocationAndGoodShelfImport(string ID)
        {
            IndexViewModel vm = new IndexViewModel();
            string js = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    if (ds != null && ds.Tables[0] != null)
                    {
                        //IList<LocationInfo> locations = new List<LocationInfo>();
                        //Response<GetWarehouseLocationByIDResponse> response = new Response<GetWarehouseLocationByIDResponse>();
                        AddOrUpdateWarehouseLocationRequest request = new AddOrUpdateWarehouseLocationRequest();
                        StringBuilder sb = new StringBuilder();
                        var areaList = ApplicationConfigHelper.GetWarehouseAreaList(ID.ObjectToInt64());
                        var goodList = ApplicationConfigHelper.GetGoodsShelfList(base.UserInfo.ProjectID, 0, ID.ObjectToInt64());
                        //var LocationList = ApplicationConfigHelper.GetWarehouseLocationList(ID.ObjectToInt64());
                        string WarehouseName = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(a => a.WarehouseID.ToString() == ID).Select(b => b.WarehouseName).FirstOrDefault();
                        var LocationList = ApplicationConfigHelper.GetWarehouseLocationListByWarehouseName(WarehouseName);
                        string LocationString = "";
                        string areString = "";
                        string GoodsString = "";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (areaList.Where(c => c.AreaName == ds.Tables[0].Rows[i]["库区"].ToString()).Select(m => m.ID).Count() == 0)
                            {
                                areString = areString + ds.Tables[0].Rows[i]["库区"].ToString() + ",";
                            }
                            if (LocationList.Where(c => c.Location == ds.Tables[0].Rows[i]["库位"].ToString()).Select(m => m.ID).Count() == 0)
                            {
                                LocationString = LocationString + ds.Tables[0].Rows[i]["库位"].ToString() + ",";
                            }
                            if (goodList.Where(c => c.GoodsShelvesName == ds.Tables[0].Rows[i]["货架"].ToString()).Select(m => m.ID).Count() == 0)
                            {
                                GoodsString = GoodsString + ds.Tables[0].Rows[i]["货架"].ToString() + ",";
                            }
                        }
                        if (areString != "")
                        {
                            return new { result = "<h3><font color='#FF0000'>导入失败,下列库区不存在：" + areString + "</font></h3>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                        }
                        if (LocationString != "")
                        {
                            return new { result = "<h3><font color='#FF0000'>导入失败,下列库位不存在：" + LocationString + "</font></h3>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                        }
                        if (GoodsString != "")
                        {
                            return new { result = "<h3><font color='#FF0000'>导入失败,下列货架不存在：" + GoodsString + "</font></h3>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                        }
                        IEnumerable<LocationInfo> locations = this.InitLocationFromDataTable2(ds.Tables[0], sb, ID);
                        //var groupedlocations = from g in locations
                        //                    select new LocationInfo() {Location = g.Location, WarehouseID = g.WarehouseID,  AreaID = g.AreaID };
                        var response = new WarehouseService().ImportLocationAndGoodShelf(new AddLocationRequest() { Location = locations });
                        if (response.IsSuccess)
                        {
                            return new { result = "<h3><font color='#00dd00'>导入成功</font></h3>", IsSuccess = true }.ToJsonString();
                        }
                        //else if (response.SuccessMessage.ToString().Contains("有重复"))
                        //{
                        //    return new { result = "<h3><font color='#FF0000'>导入失败！数据库中有重复库位</font></h3>", IsSuccess = false }.ToJsonString();
                        //}
                        else
                        {
                            return new { result = "<h3><font color='#FF0000'>导入失败！</font></h3>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                        }
                    }
                }
            }
            return new { result = "<h3>失败</h3>", IsSuccess = false }.ToJsonString();
        }

        [HttpPost]
        public string GoodsShelfImportClick(long CustomerID, long WarehouseID)
        {
            IndexViewModel vm = new IndexViewModel();
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    if (ds != null && ds.Tables[0] != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        var goodList = ApplicationConfigHelper.GetGoodsShelfList(base.UserInfo.ProjectID, CustomerID, WarehouseID);
                        IList<GoodsShelfInfo> GoodsShelfs = new List<GoodsShelfInfo>();
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            //if (goodList.Where(c => c.GoodsShelvesName == ds.Tables[1].Rows[i]["货架名"].ToString()).Select(m => m.ID).Count() == 0)
                            //{
                            //    GoodsString = GoodsString + ds.Tables[1].Rows[i]["货架名"].ToString() + ",";
                            //}
                            if (!Regex.IsMatch(ds.Tables[1].Rows[i]["层数"].ToString(), @"^[-]?\d+[.]?\d*$"))
                            {
                                return new { result = "<h3><font color='#FF0000'>" + ds.Tables[1].Rows[i]["层数"].ToString() + "不是数字</font></h3>", IsSuccess = false }.ToJsonString();
                            }
                        }
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (!Regex.IsMatch(ds.Tables[0].Rows[i]["第几层"].ToString(), @"^[-]?\d+[.]?\d*$"))
                            {
                                return new { result = "<h3><font color='#FF0000'>" + ds.Tables[0].Rows[i]["第几层"].ToString() + "不是数字</font></h3>", IsSuccess = false }.ToJsonString();
                            }
                            if (!Regex.IsMatch(ds.Tables[0].Rows[i]["格数"].ToString(), @"^[-]?\d+[.]?\d*$"))
                            {
                                return new { result = "<h3><font color='#FF0000'>" + ds.Tables[0].Rows[i]["格数"].ToString() + "不是数字</font></h3>", IsSuccess = false }.ToJsonString();
                            }
                            GoodsShelfInfo gs = new GoodsShelfInfo();
                            gs.GoodsShelvesName = ds.Tables[0].Rows[i]["货架名"].ToString();
                            gs.RowNumber = ds.Tables[0].Rows[i]["第几层"].ObjectToInt32();
                            gs.CellNumber = ds.Tables[0].Rows[i]["格数"].ObjectToInt32();
                            GoodsShelfs.Add(gs);
                        }
                        //if (GoodsString != "")
                        //{
                        //    return new { result = "<h3><font color='#FF0000'>导入失败,sheet2中下列货架不存在：" + GoodsString + "</font></h3>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                        //}
                        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        //{
                        //    if (goodList.Where(c => c.GoodsShelvesName == ds.Tables[0].Rows[i]["货架名"].ToString()).Select(m => m.ID).Count() == 0)
                        //    {
                        //        GoodsString2 = GoodsString + ds.Tables[0].Rows[i]["货架名"].ToString() + ",";
                        //    }
                        //}
                        //if (GoodsString2 != "")
                        //{
                        //    return new { result = "<h3><font color='#FF0000'>导入失败,sheet1中下列货架不存在：" + GoodsString2 + "</font></h3>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                        //}
                        IEnumerable<Column> columnasn;
                        var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                        Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                        if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_GoodsShelves").Count() == 0)
                        {
                            columnasn = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M002").
                                Tables.TableCollection.First(t => t.Name == "WMS_GoodsShelves").ColumnCollection.Where(c => c.DbColumnName != "Location" && c.DbColumnName != "LevelsNumber" && c.DbColumnName != "SerialNumber");
                        }
                        else
                        {
                            columnasn = module.Tables.TableCollection.First(t => t.Name == "WMS_GoodsShelves").ColumnCollection.Where(c => c.DbColumnName != "Location" && c.DbColumnName != "LevelsNumber" && c.DbColumnName != "SerialNumber");
                        }
                        //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, CustomerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                        //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                        //IEnumerable<Column> columnasn = module.Tables.TableCollection.First(t => t.Name == "WMS_GoodsShelves").ColumnCollection;
                        bool useCustomerOrderNumber = module.UseCustomerOrderNumber;
                        IEnumerable<GoodsShelfInfo> gsList = this.InitGoodsShelfFromDataTable(ds.Tables[1], columnasn);
                        //string goodstring = "";
                        //var lists = ApplicationConfigHelper.GetGoodsShelfList(base.UserInfo.ProjectID, CustomerID, WarehouseID);
                        //foreach (var s in gsList)
                        //{
                        //    if (lists.Count() > 0)
                        //    {
                        //        if (lists.Where(c => c.GoodsShelvesName == s.GoodsShelvesName).Select(m => m.GoodsShelvesName).Count()>0)
                        //        {
                        //            if (lists.Where(c => c.GoodsShelvesName == s.GoodsShelvesName).Select(m => m.GoodsShelvesName).FirstOrDefault().ToString() != "")
                        //            {
                        //                goodstring = goodstring + lists.Where(c => c.GoodsShelvesName == s.GoodsShelvesName).Select(m => m.GoodsShelvesName).FirstOrDefault() + ",";
                        //            }
                        //        }
                        //    }
                        //}
                        //if (goodstring != "")
                        //{
                        //    return new { result = "<h3>导入失败,下列货架名已存在："+goodstring.Substring(0,goodstring.Length-1)+"</h3>", IsSuccess = false }.ToJsonString();
                        //}
                        gsList.Each((i, s) =>
                        {
                            s.CustomerID = CustomerID;
                            s.ProjectID = base.UserInfo.ProjectID;
                            s.WareHouseID = WarehouseID;
                        });
                        GetGoodsShelfByConditonRequest request = new GetGoodsShelfByConditonRequest();
                        request.GoodsShelf = gsList;
                        request.GoodsShelfRowAndCell = GoodsShelfs;
                        var response = new WarehouseService().ImportGoodsShelf(request, 1);
                        ApplicationConfigHelper.RefreshGetGoodsShelfList(base.UserInfo.ProjectID, CustomerID, WarehouseID);
                        return new { result = "<h3><font color='#00dd00'>导入成功</font></h3>", IsSuccess = true }.ToJsonString();
                    }
                }
            }
            return new { result = "<h3>失败</h3>", IsSuccess = false }.ToJsonString();
        }

        [HttpPost]
        public string GoodsShelfLocationImportClick(long CustomerID, long WarehouseID)
        {
            IndexViewModel vm = new IndexViewModel();
            string js = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    if (ds != null && ds.Tables[0] != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        IList<GoodsShelfInfo> GoodsShelfs = new List<GoodsShelfInfo>();
                        IEnumerable<Column> columnasn;
                        var areaList = ApplicationConfigHelper.GetWarehouseAreaList(WarehouseID);
                        var goodList = ApplicationConfigHelper.GetGoodsShelfList(base.UserInfo.ProjectID, CustomerID, WarehouseID);
                        //var LocationList = ApplicationConfigHelper.GetWarehouseLocationList(WarehouseID);
                        string WarehouseName = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(a => a.WarehouseID == WarehouseID).Select(b => b.WarehouseName).FirstOrDefault();
                        var LocationList = ApplicationConfigHelper.GetWarehouseLocationListByWarehouseName(WarehouseName);
                        string LocationString = "";
                        string areString = "";
                        string GoodsString = "";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (areaList.Where(c => c.AreaName == ds.Tables[0].Rows[i]["库区"].ToString()).Select(m => m.ID).Count() == 0)
                            {
                                areString = areString + ds.Tables[0].Rows[i]["库区"].ToString() + ",";
                            }
                            if (LocationList.Where(c => c.Location == ds.Tables[0].Rows[i]["库位"].ToString()).Select(m => m.ID).Count() == 0)
                            {
                                LocationString = LocationString + ds.Tables[0].Rows[i]["库位"].ToString() + ",";
                            }
                            if (goodList.Where(c => c.GoodsShelvesName == ds.Tables[0].Rows[i]["货架名"].ToString()).Select(m => m.ID).Count() == 0)
                            {
                                GoodsString = GoodsString + ds.Tables[0].Rows[i]["货架名"].ToString() + ",";
                            }
                            if (!Regex.IsMatch(ds.Tables[0].Rows[i]["第几层"].ToString(), @"^[-]?\d+[.]?\d*$"))
                            {
                                return new { result = "<h3><font color='#FF0000'>" + ds.Tables[0].Rows[i]["第几层"].ToString() + "不是数字</font></h3>", IsSuccess = false }.ToJsonString();
                            }
                            if (!Regex.IsMatch(ds.Tables[0].Rows[i]["第几格"].ToString(), @"^[-]?\d+[.]?\d*$"))
                            {
                                return new { result = "<h3><font color='#FF0000'>" + ds.Tables[0].Rows[i]["第几格"].ToString() + "不是数字</font></h3>", IsSuccess = false }.ToJsonString();
                            }
                        }
                        if (areString != "")
                        {
                            return new { result = "<h3><font color='#FF0000'>导入失败,下列库区不存在：" + areString + "</font></h3>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                        }
                        if (LocationString != "")
                        {
                            return new { result = "<h3><font color='#FF0000'>导入失败,下列库位不存在：" + LocationString + "</font></h3>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                        }
                        if (GoodsString != "")
                        {
                            return new { result = "<h3><font color='#FF0000'>导入失败,下列货架不存在：" + GoodsString + "</font></h3>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                        }
                        var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                        Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                        if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_GoodsShelves").Count() == 0)
                        {
                            columnasn = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M002").
                                Tables.TableCollection.First(t => t.Name == "WMS_GoodsShelves").ColumnCollection.Where(c => c.DbColumnName == "AreaName" || c.DbColumnName == "Location" || c.DbColumnName == "GoodsShelvesName" || c.DbColumnName == "LevelsNumber" || c.DbColumnName == "SerialNumber");
                        }
                        else
                        {
                            columnasn = module.Tables.TableCollection.First(t => t.Name == "WMS_GoodsShelves").ColumnCollection.Where(c => c.DbColumnName == "AreaName" || c.DbColumnName == "Location" || c.DbColumnName == "GoodsShelvesName" || c.DbColumnName == "LevelsNumber" || c.DbColumnName == "SerialNumber");
                        }
                        bool useCustomerOrderNumber = module.UseCustomerOrderNumber;
                        IEnumerable<GoodsShelfInfo> gsList = this.InitGoodsShelfFromDataTable(ds.Tables[0], columnasn);
                        gsList.Each((i, s) =>
                        {
                            s.CustomerID = CustomerID;
                            s.ProjectID = base.UserInfo.ProjectID;
                            s.WareHouseID = WarehouseID;
                        });
                        GetGoodsShelfByConditonRequest request = new GetGoodsShelfByConditonRequest();
                        request.GoodsShelf = gsList;
                        var response = new WarehouseService().ImportGoodsShelfLocation(request, 1);
                        ApplicationConfigHelper.RefreshGetGoodsShelfList(base.UserInfo.ProjectID, CustomerID, WarehouseID);
                        return new { result = "<h3><font color='#00dd00'>导入成功</font></h3>", IsSuccess = true }.ToJsonString();
                    }
                }
            }
            return new { result = "<h3>失败</h3>", IsSuccess = false }.ToJsonString();
        }

        private IEnumerable<LocationInfo> InitLocationFromDataTable(DataTable dt, StringBuilder sb, string ID)
        {
            var areaList = ApplicationConfigHelper.GetWarehouseAreaListByWID(ID.ObjectToInt64());
            //var goodList = ApplicationConfigHelper.GetGoodsShelfList(base.UserInfo.ProjectID, 0, ID.ObjectToInt64());
            //IEnumerable<WMSConfig> LocationTypes = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("ParaLocationType");
            IEnumerable<WMSConfig> Classifications = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("ParaLocationClassify");
            IEnumerable<WMSConfig> Handlings = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("Handling");
            IEnumerable<WMSConfig> ABCClassifications = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("ABCClassification");
            IEnumerable<WMSConfig> LocationTypes = null;
            try
            {
                LocationTypes = ApplicationConfigHelper.GetWMS_Config("ParaLocationType_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }

            if (LocationTypes == null)
            {
                LocationTypes = ApplicationConfigHelper.GetWMS_Config("ParaLocationType");
            }

            IList<LocationInfo> locations = new List<LocationInfo>();
            IList<Column> columnsConfig = new List<Column>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                LocationInfo location = new LocationInfo();
                string columnName;
                string value;
                columnName = "库位";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                        //value = dt.Rows[i][j].ToString().Trim();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + " 不能为空<br/>");
                            break;
                        }
                        location.Location = value;
                        break;
                    }
                }
                location.WarehouseID = Convert.ToInt64(ID);
                //location.LevelsNumber = dt.Columns.Contains("第几层")==false ? 0 : dt.Rows[i]["第几层"].ObjectToInt64();
                //location.SerialNumber = dt.Columns.Contains("第几格") == false ? 0 : dt.Rows[i]["第几格"].ObjectToInt64();
                //location.GoodsShelvesName = dt.Columns.Contains("货架") == false ? "" : dt.Rows[i]["货架"].ToString();
                //location.GoodsShelfID = dt.Columns.Contains("货架") == false ? 0 : goodList.Where(c => c.GoodsShelvesName == dt.Rows[i]["货架"].ToString()).Select(m => m.ID).FirstOrDefault().ObjectToInt64();
                location.AreaID = areaList.Where(c => c.AreaName == dt.Rows[i]["库区"].ToString()).Select(m => m.ID).FirstOrDefault().ObjectToInt64();
                location.LocationStatus = "1";
                location.LocationType = LocationTypes.Where(c => c.Name == dt.Rows[i]["库位类型"].ToString().Trim()).Select(c => c.Code).FirstOrDefault().ObjectToInt32();
                location.LocationLevel = dt.Rows[i]["库位级别"].ToString();
                location.Classification = Classifications.Where(c => c.Name == dt.Rows[i]["Classification"].ToString().Trim()).Select(c => c.Code).FirstOrDefault().ObjectToInt32();
                location.Handling = Handlings.Where(c => c.Name == dt.Rows[i]["Handling"].ToString().Trim()).Select(c => c.Code).FirstOrDefault().ObjectToInt32();
                location.ABCClassification = ABCClassifications.Where(c => c.Name == dt.Rows[i]["ABCClassification"].ToString().Trim()).Select(c => c.Code).FirstOrDefault().ToString();
                location.GoodsPickOrder = dt.Columns.Contains("拣货次序") == false ? "" : dt.Rows[i]["拣货次序"].ToString();
                location.Length = dt.Columns.Contains("长") == false ? "" : dt.Rows[i]["长"].ToString();
                location.Width = dt.Columns.Contains("宽") == false ? "" : dt.Rows[i]["宽"].ToString();
                location.Height = dt.Columns.Contains("高") == false ? "" : dt.Rows[i]["高"].ToString();
                location.MaxNumber = dt.Rows[i]["最大放货量"].ToString();

                location.Volume = dt.Columns.Contains("体积") == false ? "" : dt.Rows[i]["体积"].ToString();
                location.Int1 = dt.Columns.Contains("是否整箱") == false ? 0 : Convert.ToInt32(dt.Rows[i]["是否整箱"].ToString());
                location.Str1 = dt.Columns.Contains("库位容积率") == false ? "" : dt.Rows[i]["库位容积率"].ToString();
                location.Str2 = dt.Columns.Contains("项目所属") == false ? "" : dt.Rows[i]["项目所属"].ToString();
                location.Str3 = dt.Columns.Contains("楼层") == false ? "" : dt.Rows[i]["楼层"].ToString();

                locations.Add(location);
            }
            return locations;
        }

        private IEnumerable<LocationInfo> InitLocationFromDataTable2(DataTable dt, StringBuilder sb, string ID)
        {
            var areaList = ApplicationConfigHelper.GetWarehouseAreaList(ID.ObjectToInt64());
            var goodList = ApplicationConfigHelper.GetGoodsShelfList(base.UserInfo.ProjectID, 0, ID.ObjectToInt64());
            IList<LocationInfo> locations = new List<LocationInfo>();
            IList<Column> columnsConfig = new List<Column>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                LocationInfo location = new LocationInfo();
                string columnName;
                string value;
                columnName = "库位";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (string.Equals(dt.Columns[j].ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                        //value = dt.Rows[i][j].ToString().Trim();
                        if (string.IsNullOrEmpty(value))
                        {
                            sb.Append("第" + (i + 2).ToString() + "行，第" + (j + 1).ToString() + "列, " + columnName + " 不能为空<br/>");
                            break;
                        }
                        location.Location = value;
                        break;
                    }
                }
                location.WarehouseID = Convert.ToInt64(ID);
                location.LevelsNumber = dt.Columns.Contains("第几层") == false ? 0 : dt.Rows[i]["第几层"].ObjectToInt64();
                location.SerialNumber = dt.Columns.Contains("第几格") == false ? 0 : dt.Rows[i]["第几格"].ObjectToInt64();
                location.GoodsShelvesName = dt.Columns.Contains("货架") == false ? "" : dt.Rows[i]["货架"].ToString();
                location.GoodsShelfID = dt.Columns.Contains("货架") == false ? 0 : goodList.Where(c => c.GoodsShelvesName == dt.Rows[i]["货架"].ToString()).Select(m => m.ID).FirstOrDefault().ObjectToInt64();
                location.AreaID = areaList.Where(c => c.AreaName == dt.Rows[i]["库区"].ToString()).Select(m => m.ID).FirstOrDefault().ObjectToInt64();
                locations.Add(location);
            }
            return locations;
        }

        private IEnumerable<GoodsShelfInfo> InitGoodsShelfFromDataTable(DataTable dt, IEnumerable<Column> columnsConfig)
        {
            IList<GoodsShelfInfo> asns = new List<GoodsShelfInfo>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                GoodsShelfInfo asn = new GoodsShelfInfo();
                string value;
                foreach (var column in columnsConfig)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (string.Equals(dt.Columns[j].ColumnName.Trim(), column.DisplayName, StringComparison.OrdinalIgnoreCase))
                        {
                            value = string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()) ? null : dt.Rows[i][j].ToString().Trim();
                            //value = dt.Rows[i][j].ToString().Trim();
                            if (!string.IsNullOrEmpty(value))
                            {
                                var propertyInfoTemp = typeof(GoodsShelfInfo).GetProperty(column.DbColumnName);
                                try
                                {
                                    propertyInfoTemp.SetValue(asn, Convert.ChangeType(value, propertyInfoTemp.PropertyType), null);
                                }
                                catch
                                {
                                    propertyInfoTemp.SetValue(asn, null);
                                }
                                break;
                            }
                            break;
                        }
                    }
                }
                asns.Add(asn);
            }
            return asns;
        }

        private DataSet GetDataFromExcel(HttpPostedFileBase hpf)
        {
            string uploadFolderPath = Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH;
            string targetPath = Path.Combine(Runbow.TWS.Common.Constants.UPLOAD_FOLDER_PATH, Runbow.TWS.Common.Constants.TEMPFOLDER);
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }
            string fileName = "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(hpf.FileName);
            string fullPath = Path.Combine(targetPath, fileName);
            hpf.SaveAs(fullPath);
            hpf.InputStream.Close();
            Runbow.TWS.Common.ExcelHelper excelHelper = new Runbow.TWS.Common.ExcelHelper(fullPath);
            DataSet ds = excelHelper.GetAllDataFromAllSheets();
            excelHelper.Dispose();
            MyFile.Delete(fullPath);
            return ds;
        }
      
        //得到所有客户和仓库  add by hujiaoqiang  20151027
        [HttpGet]
        public ActionResult WarehouseAllocate(long CustomerID)
        {
            var response = new WarehouseService().GetWarehouseAllocate(new WarehouseAllocateRequest() { CustomerID = CustomerID });
            var vm = new Runbow.TWS.Web.Areas.WMS.Models.WarehouseAllocateModel()
            {
                CustomerID = CustomerID,
                Customers = ApplicationConfigHelper.GetApplicationCustomer(),
                Warehouse = ApplicationConfigHelper.GetWarehouseList().Where(c => c.ProjectID == base.UserInfo.ProjectID),
                WarehouseAllocate = response.Result
            };
            return View(vm);
        }
     
        //为当前客户分配仓库权限 add by hujiaoqiang  20151027
        [HttpPost]
        public ActionResult SetWarehouseAllocate(long CustomerID, long WarehouseID)
        {
            var response = new WarehouseService().SetUserWarehouseAllocate(new WarehouseAllocateRequest() { CustomerID = CustomerID, WarehouseID = WarehouseID, Creator = base.UserInfo.Name });
            if (response.IsSuccess)
            {
                ApplicationConfigHelper.RefreshCacheInfo();
                ApplicationConfigHelper.RefreshWarehouseAllocate();
                ApplicationConfigHelper.RefreshGetApplicationCustomer();
                ApplicationConfigHelper.RefreshGetWarehouseList();
                return Json("仓库用户权限设置成功");
            }
            throw new Exception("仓库用户权限设置失败！");
        }
     
        //查找当前客户已有仓库权限  add by hujiaoqiang  20151027
        [HttpPost]
        public JsonResult GetWarehouseAllocate(long CustomerID)
        {
            var response = new WarehouseService().GetWarehouseAllocate(new WarehouseAllocateRequest() { CustomerID = CustomerID });
            if (response.IsSuccess)
            {
                if (response.Result != null)
                {
                    return Json(response.Result);
                }
                else
                {
                    return Json("");
                }
            }
            throw new Exception("获取用户仓库设置失败！");
        }
      
        //add by hujiaoqiang  20151027
        [HttpPost]
        public ActionResult GetAllCustomersbyCustomerID(string name)
        {
            var customers = ApplicationConfigHelper.GetApplicationCustomers().Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3));
            return Json(customers.Where(s => s.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.Name }), JsonRequestBehavior.AllowGet);
        }
     
        //得到所有用户和仓库
        [HttpGet]
        public ActionResult WarehouseAllocates(long UserID)
        {
            var response = new WarehouseService().GetWarehouseAllocates(new WarehouseAllocateRequest() { UserID = UserID });
            var vm = new Runbow.TWS.Web.Areas.WMS.Models.WarehouseAllocateModel()
            {
                UserID = UserID,
                User = ApplicationConfigHelper.GetApplicationUsers(),
                Warehouse = ApplicationConfigHelper.GetWarehouseList().Where(c => c.ProjectID == base.UserInfo.ProjectID),
                WarehouseAllocate = response.Result
            };
            return View(vm);
        }
     
        //添加仓库用户
        public ActionResult SetWarehouseAllocates(long UserID, long WarehouseID)
        {
            var response = new WarehouseService().SetUserWarehouseAllocates(new WarehouseAllocateRequest() { UserID = UserID, WarehouseID = WarehouseID, Creator = base.UserInfo.Name });
            if (response.IsSuccess)
            {
                ApplicationConfigHelper.RefreshCacheInfo();
                ApplicationConfigHelper.RefreshWarehouseAllocate();
                return Json("用户仓库权限分配成功");
            }
            throw new Exception("用户仓库权限分配失败！");
        }

        [HttpPost]
        public JsonResult GetWarehouseAllocates(long UserID)
        {
            var response = new WarehouseService().GetWarehouseAllocates(new WarehouseAllocateRequest() { UserID = UserID });
            if (response.IsSuccess)
            {
                if (response.Result != null)
                {
                    return Json(response.Result);
                }
                else
                {
                    return Json("");
                }
            }
            throw new Exception("获取用户仓库分配失败！");
        }

        [HttpPost]
        public ActionResult GetAllUserByUserID(string name)
        {
            var users = ApplicationConfigHelper.GetApplicationUsers().Where(c => (c.State == true || c.UserType == 2));
            return Json(users.Where(s => s.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0).Select(t => new { Value = t.ID.ToString(), Text = t.Name }), JsonRequestBehavior.AllowGet);
        }
     
        #region  库存盘点 2016-2-25 16:52:45 hzf
        [HttpGet]
        public ActionResult WareHouseCheckDetail(long? customerID, long? warehouseID)
        {
            var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                               .Select(c => c.CustomerID);
            WarehouseCheckModel wa = new WarehouseCheckModel();
            wa.SearchCondition = new WarehouseCheckSearchCondition();
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
                    wa.SearchCondition.CustomerID = customerID;
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
            wa.SearchCondition.StartCheckdate = DateTime.Now.AddDays(-10);
            wa.SearchCondition.EndCheckdate = DateTime.Now;
            var Request = new GetWarehouseCheckByConditonRequest();
            Request.WLSearchCondition = wa.SearchCondition;
            Request.WLSearchCondition.PageSize = UtilConstants.PAGESIZE;
            Request.WLSearchCondition.PageIndex = 0;
            ViewBag.AreaLists = AreaList;
            ViewBag.CustomerID = customerID == null ? base.UserInfo.CustomerID : customerID;
            if (WarehouseList.Count() == 1)
            {
                wa.SearchCondition.Warehouse = WarehouseList.Select(c => c.Text).FirstOrDefault();
            }
            var Requests = new WarehouseService().GetWarehouseCheckList(Request);
            if (Requests.IsSuccess)
            {
                wa.WarehouseCheckCollection = Requests.Result.WarehouseCheckCollection;
                wa.PageIndex = Requests.Result.PageIndex;
                wa.PageCount = Requests.Result.PageCount;
            }
            return View(wa);
        }
       
        /// <summary>
        /// 获取盘点列表信息
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="warehouseID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WareHouseCheckDetail(WarehouseCheckModel wc, int? PageIndex, long? customerID, long? warehouseID, string Action)
        {
            var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3)
                             .Select(c => c.CustomerID);
            if (wc.SearchCondition.CustomerID != null)
                ViewBag.WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == wc.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            //ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == Int64.Parse(wc.SearchCondition.CustomerID.ToString())).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
            if (wc.SearchCondition.Warehouse != null)
            {
                ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == Int64.Parse(wc.SearchCondition.Warehouse.ToString())).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName, Selected = true });
            }
            else
            {
                ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == 0).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            }
            ViewBag.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            //ViewBag.AreaLists = ApplicationConfigHelper.GetWarehouseLocationList().Select(t => new { WarehouseID = t.WarehouseID, AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.AreaName });
            var Request = new GetWarehouseCheckByConditonRequest();
            Request.WLSearchCondition = wc.SearchCondition;
            Request.WLSearchCondition.PageSize = UtilConstants.PAGESIZE;
            Request.WLSearchCondition.PageIndex = PageIndex ?? 0;
            var WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && customerIDs.Contains(c.CustomerID.Value) && c.CustomerID == wc.SearchCondition.CustomerID.Value)
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
            ViewBag.CustomerID = wc.SearchCondition.CustomerID == null ? base.UserInfo.CustomerID : wc.SearchCondition.CustomerID;
            if (WarehouseList.Count() == 1)
            {
                wc.SearchCondition.Warehouse = WarehouseList.Select(c => c.Text).FirstOrDefault();
            }
            var Requests = new WarehouseService().GetWarehouseCheckList(Request);
            if (Requests.IsSuccess)
            {
                wc.WarehouseCheckCollection = Requests.Result.WarehouseCheckCollection;
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
        [HttpGet]
        public ActionResult WareHouseCheckEdit(long? customerID, long? warehouseID, int ViewType = 0, string CheckNumber = "", string flag = null)
        {
            WarehouseCheckModel wa = new WarehouseCheckModel();
            wa.ViewType = ViewType;
            //if (wa.ViewType == 1)
            {
                //根据CheckNumber 获取盘点单信息
                WarehouseCheckModel getwc = new WarehouseCheckModel();
                Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>();
                GetWarehouseCheckByConditonRequest request = new GetWarehouseCheckByConditonRequest();
                WarehouseCheckSearchCondition wc = new WarehouseCheckSearchCondition();
                wc.CheckNumber = CheckNumber;
                wc.ViewType = ViewType;
                request.WLSearchCondition = wc;
                request.WLSearchCondition.CheckNumber = CheckNumber;
                if (flag == "导出差异")
                {
                    IEnumerable<Column> columnReceipt;
                    var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, base.UserInfo.CustomerOrShipperID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                    Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_CheckDetail").Count() == 0)
                    {
                        columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_CheckDetail").ColumnCollection;
                    }
                    else
                    {
                        columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_CheckDetail").ColumnCollection;
                    }
                    //var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, customerID).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString());
                    //Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                    //IEnumerable<Table> tables = module.Tables.TableCollection;
                    //IEnumerable<Column> columnReceipt = tables.First(t => t.Name == "WMS_CheckDetail").ColumnCollection;
                    var responses = new WarehouseService().ExportWarehouseCheckByCheckNumber(request);
                    ExportCheckDetail(responses.Result, columnReceipt);
                    return null;
                }
                else
                {
                    var responses = new WarehouseService().GetWarehouseCheckByCheckNumber(request);
                    wa.SearchCondition = wc;
                    wa.WarehouseCheckDetailCollection = responses.Result.WarehouseCheckDetailCollection;
                    wa.WarehouseCheckCollection = responses.Result.WarehouseCheckCollection;
                }
            }
            return View(wa);
        }

        private void ExportCheckDetail(GetWarehouseCheckByConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            IEnumerable<WarehouseCheckDetail> receipts = response.WarehouseCheckDetailCollection;
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
                    drReceipt[receipt.DisplayName] = typeof(Runbow.TWS.Entity.WarehouseCheckDetail).GetProperty(receipt.DbColumnName).GetValue(s);
                }
                dtReceipt.Rows.Add(drReceipt);
            });
            dtReceipt.TableName = "盘点差异信息";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "盘点差异" + DateTime.Now.ToString("yyyy-MM-dd"));
            EPPlusOperation.ExportByEPPlus(ds.Tables[0], "盘点差异" + DateTime.Now.ToString("yyyy-MM-dd"));
        }
        
        /// <summary>
        /// 1 编辑
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="warehouseID"></param>
        /// <param name="ViewType"></param>
        /// <param name="CheckNumber"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WareHouseCheckEdit(WarehouseCheckModel vm, string[] SKU, string[] ActualQTY, string Action)
        {
            WarehouseCheckModel wa = new WarehouseCheckModel();
            //根据CheckNumber 获取盘点单信息
            WarehouseCheckModel getwc = new WarehouseCheckModel();
            Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>();
            GetWarehouseCheckByConditonRequest request = new GetWarehouseCheckByConditonRequest();
            vm.SearchCondition.ActulQtyargs = ActualQTY;
            request.WLSearchCondition = vm.SearchCondition;
            string Message = new WarehouseService().SaveWarehouseCheckByCheckNumber(request);
            return RedirectToAction("WareHouseCheckDetail");
        }
        
        /// <summary>
        ///   盘点明细页面
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="warehouseID"></param>
        /// <param name="ViewType"></param>
        /// <param name="CheckNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult WareHouseCheck(long? customerID, long? warehouseID, string externNumber = "", int ViewType = 0, string CheckNumber = "")
        {
            WarehouseCheckModel wa = new WarehouseCheckModel();
            wa.ViewType = ViewType;
            if (ViewType == 0)
            {
                wa.SearchCondition = new WarehouseCheckSearchCondition();
                wa.SearchCondition.ExternNumber = externNumber;
                wa.SearchCondition.Checkdate = DateTime.Now;
                wa.SearchCondition.CheckNumber = "PD" + DateTime.Now.ToString("yyyyMMddHHmmss");
                //wa.SearchCondition.ExternNumber = base.UserInfo.Name + DateTime.Now.ToString("MMddHHmmss");
                wa.SearchCondition.str5 = "";
                if (base.UserInfo.UserType == 0)
                {
                    wa.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        wa.SearchCondition.CustomerID = customerID;
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
                //wa.SearchCondition.Warehouse = warehouseID.ToString();
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
                //lrg
                if (warehouseID != null)//传过来的有仓库ID
                {
                    wa.SearchCondition.Warehouse = warehouseID.ToString();
                    //获取库区
                    ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehouseID & c.CustomerID == wa.SearchCondition.CustomerID).Select(t => new { ID = t.ID.ToString(), AreaName = t.AreaName }).Distinct()
                    .Select(c => new SelectListItem() { Value = c.ID, Text = c.AreaName });
                }
                else {
                    int warehou = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == customerID && c.UserID == base.UserInfo.ID)).Select(s => s.WarehouseID).FirstOrDefault();
                    wa.SearchCondition.Warehouse = warehou.ToString();
                    ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehou & c.CustomerID == wa.SearchCondition.CustomerID).
                        Select(t => new { ID = t.ID.ToString(), AreaName = t.AreaName }).Distinct().Select(c => new SelectListItem() { Value = c.ID, Text = c.AreaName });
                }                
               
                ViewBag.Checkdate = DateTime.Now.ToString("yyyy-MM-dd");
                wa.SearchCondition.Checkdate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
               
            }
            #region viewtype=1
            else if (ViewType == 1)
            {
                //根本checknumber获取明细信息
                wa.SearchCondition = new WarehouseCheckSearchCondition();
                wa.SearchCondition.Checkdate = DateTime.Now;
                // wa.SearchCondition.CheckNumber = "PD" + DateTime.Now.ToString("yyyyMMddHHmmss");
                wa.SearchCondition.str5 = "";
                if (base.UserInfo.UserType == 0)
                {
                    wa.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
                }
                else if (base.UserInfo.UserType == 2)
                {
                    if (customerID.HasValue)
                    {
                        wa.SearchCondition.CustomerID = customerID;
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
                wa.SearchCondition.Warehouse = warehouseID.ToString();
                ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID == wa.SearchCondition.CustomerID).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
                //ViewBag.WarehouseList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(c => c.CustomerID ==).Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName, Selected = true });
                ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == warehouseID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
                ViewBag.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            }
            #endregion
            return View(wa);
        }
        
        /// <summary>
        /// 保存查询明细（只带查询条件重新查询数据）
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="PageIndex"></param>
        /// <param name="roles"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult WareHouseCheck(WarehouseCheckModel vm, int? PageIndex, string[] roles, string[] ActualQTY, string Action)
        {
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            WarehouseCheckSearchCondition wc = new WarehouseCheckSearchCondition();
            ViewBag.Checkdate = DateTime.Now.ToString("yyyy-MM-dd");
            vm.SearchCondition.Checkdate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            //vm.SearchCondition.str5 = "";
            #region 执行条件判断
            if (vm.SearchCondition.Type == 5)
            {
                string businessType = "";
                for (int i = 0; i < roles.Length; i++)
                {
                    businessType += roles[i] + ",";
                }
                wc.str5 = businessType.Substring(0, businessType.Length - 1);
            }

            //空库位盘点
            //if (vm.SearchCondition.Type == 8) {
            //    string businessType = "";
            //    for (int i = 0; i < emptylocationType.Length; i++)
            //    {
            //        businessType += emptylocationType[i] + ",";
            //    }
            //    wc.str5 = businessType.Substring(0, businessType.Length - 1);
            //}
            #endregion
            //客户
            ViewBag.CustomerList = ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            //获取仓库
            var warehouselist= ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                 .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            ViewBag.WarehouseList = warehouselist;
            
            
            if (Action == "查询" || Action == "WareHouseCheck")
            {
                string area = vm.SearchCondition.Area;
                string warehouse = vm.SearchCondition.Warehouse;                
                wc.CustomerID = Int64.Parse(vm.SearchCondition.CustomerID.ToString());              
                wc.Warehouse = warehouselist.Where(m => m.Value == vm.SearchCondition.Warehouse).FirstOrDefault().Text;
                //空库位盘点使用到warehouseID和areaID
                wc.WareHouseID =Convert.ToInt64(vm.SearchCondition.Warehouse);
                wc.AreaID = Convert.ToInt64(vm.SearchCondition.Area);

                wc.Area = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == long.Parse(vm.SearchCondition.Warehouse)).
                    Where(c => c.ID.ToString() == vm.SearchCondition.Area).Select(c => c.AreaName).FirstOrDefault();
                wc.Type = int.Parse(vm.SearchCondition.Type.ToString());
                IEnumerable<WMSConfig> Classifications = Runbow.TWS.Web.Common.ApplicationConfigHelper.GetWMS_Config("CheckType");
                wc.Type_description = Classifications.Where(m => m.Code == wc.Type.ToString()).Select(a => a.Name).FirstOrDefault();
                if (vm.SearchCondition.Type == 5)
                {
                    wc.str1 = vm.SearchCondition.str3;
                    wc.str2 = vm.SearchCondition.str4;
                }                
                else
                {
                    wc.str1 = vm.SearchCondition.str1;
                    wc.str2 = vm.SearchCondition.str2;
                }
                wc.PageIndex = PageIndex.Value;
                wc.PageSize = UtilConstants.PAGESIZE;

                vm.ViewType = 0;   //新增操作
                vm.SearchCondition = wc;
                vm.SearchCondition.Checkdate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
               
                var response = new WarehouseService().GetWarehouseCheckNew(new GetWarehouseCheckByConditonRequest() { WLSearchCondition = wc });
                vm.SearchCondition.Warehouse = warehouse;
                vm.SearchCondition.Area = area;
                ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID.ToString() == vm.SearchCondition.Warehouse).Select(t => new { ID = t.ID.ToString(), AreaName = t.AreaName }).Distinct()
                   .Select(c => new SelectListItem() { Value = c.ID, Text = c.AreaName });
                if (response.IsSuccess)
                {
                    if (response.Result != null)
                    {
                        vm.WarehouseCheckDetailCollection = response.Result.WarehouseCheckDetailCollection;
                        vm.PageCount = response.Result.PageCount;
                        if (vm.SearchCondition.Type == 5)
                        {
                            vm.SearchCondition.str3 = vm.SearchCondition.str1;
                            vm.SearchCondition.str4 = vm.SearchCondition.str2;
                        }
                        return View(vm);
                    }
                }               
                return View(vm);
            }
             
            else
            {
                Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>();
                GetWarehouseCheckByConditonRequest request = new GetWarehouseCheckByConditonRequest();
                vm.SearchCondition.Oprer = base.UserInfo.Name;
                vm.SearchCondition.CheckNumber = "PD" + DateTime.Now.ToString("yyyyMMddHHmmss");
                request.WLSearchCondition = vm.SearchCondition;
                request.WLSearchCondition.Roles = roles;
                //request.WLSearchCondition.EmptyLocation = emptylocationType;
                request.WLSearchCondition.WareHouseID = Convert.ToInt64(vm.SearchCondition.Warehouse);
                request.WLSearchCondition.AreaID = Convert.ToInt64(vm.SearchCondition.Area);

                request.WLSearchCondition.Area = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.WarehouseID == long.Parse(vm.SearchCondition.Warehouse)).Where(c => c.ID.ToString() == vm.SearchCondition.Area).Select(c => c.AreaName).FirstOrDefault().ToString();
                //获取仓库名称  lrg
                request.WLSearchCondition.Warehouse= warehouselist.Where(m => m.Value == vm.SearchCondition.Warehouse).FirstOrDefault().Text;              
                request.WLSearchCondition.str5 = wc.str5;
                request.WLSearchCondition.str3 = vm.SearchCondition.str3;
                request.WLSearchCondition.str4 = vm.SearchCondition.str4;
                //request.ActualQTY = ActualQTY;
                if (vm.SearchCondition.Type == 1)
                    vm.SearchCondition.Type_description = "全部盘点";
                if (vm.SearchCondition.Type == 2)
                    vm.SearchCondition.Type_description = "库位盘点";
                if (vm.SearchCondition.Type == 3)
                    vm.SearchCondition.Type_description = "品名盘点";
                if (vm.SearchCondition.Type == 4)
                    vm.SearchCondition.Type_description = "小货量盘点";
                if (vm.SearchCondition.Type == 5)
                    vm.SearchCondition.Type_description = "变动库位盘点";
                if (vm.SearchCondition.Type == 6)
                    vm.SearchCondition.Type_description = "手工盘点";
                if (vm.SearchCondition.Type == 8)                
                    vm.SearchCondition.Type_description = "空库位盘点";
                if (vm.SearchCondition.Type == 9)
                    vm.SearchCondition.Type_description = "门店盘点";
                if (vm.SearchCondition.Type == 10)
                    vm.SearchCondition.Type_description = "门店及品名盘点";
                response = new WarehouseService().GetWarehouseSave(request);
                vm.SearchCondition.str5 = response.Result.Message;
                if (response.Result.Message == "操作成功")
                {
                    logs.Add(new WMS_Log_Operation()
                    {
                        MenuName = "盘点单管理",
                        Operation = "盘点单-新增",
                        OrderType = "Check",
                        Controller = Request.RawUrl,
                        Creator = base.UserInfo.Name,
                        CreateTime = DateTime.Now,
                        ProjectID = (int)base.UserInfo.ProjectID,
                        ProjectName = base.UserInfo.ProjectName,
                        OrderNumber = vm.SearchCondition.CheckNumber //拿到盘点单ID赋值
                    });
                    new LogOperationService().AddLogOperation(logs);
                }
                // return View(vm);
                return RedirectToAction("WareHouseCheckDetail");
            }
           
        }
    
        /// <summary>
        /// 保存查询明细 
        /// </summary>
        /// <param name="CheckNumber"></param>
        /// <param name="ExtrnNumber"></param>
        /// <param name="CheckDate"></param>
        /// <param name="Customer"></param>
        /// <param name="Warehouse"></param>
        /// <param name="Area"></param>
        /// <param name="Type"></param>
        /// <param name="str1">查询条件1</param>
        /// <param name="str2">查询条件2</param>
        /// <param name="Roles">业务类型</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveWarehouseCheckInfo(string CheckNumber, string ExternNumber, string CheckDate, string Customer, string Warehouse, string Area, string Type_Des, string Type, string str1, string str2, string Roles, string ActualQty)
        {
            string ISOK = string.Empty;  //判断是否存在错误信息
            #region 执行条件判断
            if (string.IsNullOrEmpty(ExternNumber))
            {
                ISOK += "外部盘点单号为空 |";
            }
            if (string.IsNullOrEmpty(CheckDate))
            {
                ISOK += "盘点日期为空  |";
            }
            else
            {
                try
                {
                    DateTime.Parse(CheckDate);
                }
                catch { ISOK += "盘点日期格式错误  |"; }
            }
            if (string.IsNullOrEmpty(Warehouse))
            {
                ISOK += "盘点仓库为空  |";
            }
            if (string.IsNullOrEmpty(Area))
            {
                ISOK += "盘点区域为空  |";
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
                WarehouseCheckModel wc = new WarehouseCheckModel();

                //判断查询行数是否变化
                var responsese = new WarehouseService().GetWarehouseCheck(new GetWarehouseCheckByConditonRequest() { WLSearchCondition = wc.SearchCondition });
                if (responsese.Result.PageCount != ActualQty.Split(',').Length)
                {
                    ISOK += "差异行数已更新,请重新查询  |";
                }
            }
            #endregion
            if (ISOK == string.Empty)
            {
                WarehouseCheckModel vm = new WarehouseCheckModel();
                vm.SearchCondition.str1 = str1;
                vm.SearchCondition.str2 = str2;
                vm.SearchCondition.str3 = Roles;
                vm.SearchCondition.str4 = ActualQty;
                vm.SearchCondition.CheckNumber = CheckNumber;
                vm.SearchCondition.ExternNumber = ExternNumber;
                vm.SearchCondition.Checkdate = DateTime.Parse(CheckDate);
                vm.SearchCondition.CustomerID = Int64.Parse(Customer);
                vm.SearchCondition.Warehouse = Warehouse;
                vm.SearchCondition.Area = Area;
                vm.SearchCondition.Type = int.Parse(Type);
                vm.SearchCondition.Type_description = Type_Des;
                vm.SearchCondition.Oprer = base.UserInfo.Name;
                //执行保存操作
                Response<GetWarehouseCheckByConditionResponse> response = new Response<GetWarehouseCheckByConditionResponse>();
                GetWarehouseCheckByConditonRequest request = new GetWarehouseCheckByConditonRequest();
                request.WLSearchCondition = vm.SearchCondition;
                response = new WarehouseService().GetWarehouseSave(request);
            }
            else
            {
                throw new Exception("获取盘点信息失败!");
            }
            return Json("");
        }
      
        //ajax方式查询盘点数据 
        [HttpPost]
        public JsonResult GetWarehouseCheckInfo(string customerID, string warehouseID, string AreaID, string Type, string Types, string Condition1, string Condition2)
        {
            if (customerID != null && warehouseID != null && AreaID != null && Type != null && Types != null)
            {
                WarehouseCheckSearchCondition wc = new WarehouseCheckSearchCondition();
                wc.CustomerID = Int64.Parse(customerID);
                wc.Warehouse = warehouseID;
                wc.Area = AreaID;
                wc.Type = int.Parse(Type);
                wc.Type_description = Types;
                wc.str1 = Condition1;
                wc.str2 = Condition2;
                var response = new WarehouseService().GetWarehouseCheck(new GetWarehouseCheckByConditonRequest() { WLSearchCondition = wc });
                if (response.IsSuccess)
                {
                    if (response.Result != null)
                    {
                        var resultData = Json(response.Result.WarehouseCheckCollection);
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

        [HttpPost]
        public JsonResult GetWareHouseCheckDelete(string CheckNumber)
        {
            WarehouseCheckSearchCondition wc = new WarehouseCheckSearchCondition();
            wc.CheckNumber = CheckNumber;
            var response = new WarehouseService().GetWarehouseCheckDelete(new GetWarehouseCheckByConditonRequest() { WLSearchCondition = wc });
            return Json(response);
        }
      
        //ajax方式完成盘点单 
        [HttpPost]
        [ValidateInput(false)]
        public string GetWareHouseCheckDone(string CheckNumber, string jsonString)
        {
            List<WMS_Log_Operation> logs = new List<WMS_Log_Operation>();
            var responseJsonTable = jsonlist<WarehouseCheckDetail>(jsonString);
            WarehouseCheckSearchCondition wc = new WarehouseCheckSearchCondition();
            wc.CheckNumber = CheckNumber;
            var response = new WarehouseService().GetWarehouseCheckDone(new GetWarehouseCheckByConditonRequest() { WLSearchCondition = wc, Warehousecheckdetails = responseJsonTable });
            //return Json(response);
            if (response == "盘点完成")
            {
                logs.Add(new WMS_Log_Operation()
                {
                    MenuName = "盘点单管理",
                    Operation = "盘点单-完成",
                    OrderType = "Check",
                    Controller = Request.RawUrl,
                    Creator = base.UserInfo.Name,
                    CreateTime = DateTime.Now,
                    ProjectID = (int)base.UserInfo.ProjectID,
                    ProjectName = base.UserInfo.ProjectName,
                    OrderNumber = CheckNumber //拿到盘点单ID赋值
                });
                new LogOperationService().AddLogOperation(logs);
            }
            return response;
        }

        public static List<T> jsonlist<T>(string str)
        {
            return JsonConvert.DeserializeObject<List<T>>(str);
        }

        [HttpGet]
        public ActionResult PrintWareHouseCheck(string checknumber)
        {
            WarehouseCheckModel vm = new WarehouseCheckModel();
            var response = new WarehouseService().GetPrintWareHouseCheckByCheckNumber(checknumber);
            if (response.IsSuccess)
            {
                vm.WarehouseCheck = response.Result.WarehouseCheck;
                vm.WarehouseCheckDetailCollection = response.Result.WarehouseCheckDetailCollection;
            }
            ViewBag.CheckNumber = vm.WarehouseCheck.CheckNumber;
            ViewBag.ExternNumber = vm.WarehouseCheck.ExternNumber;
            ViewBag.CreateTime = vm.WarehouseCheck.CreateTime;
            return View(vm);
        }

        [HttpGet]
        public ActionResult PrintWareHouseCheckNike(string checknumber)
        {
            WarehouseCheckModel vm = new WarehouseCheckModel();
            var response = new WarehouseService().GetPrintWareHouseCheckByCheckNumberNike(checknumber);
            if (response.IsSuccess)
            {
                vm.WarehouseCheck = response.Result.WarehouseCheck;
                vm.WarehouseCheckDetailCollection = response.Result.WarehouseCheckDetailCollection;
            }
            ViewBag.CheckNumber = vm.WarehouseCheck.CheckNumber;
            ViewBag.ExternNumber = vm.WarehouseCheck.ExternNumber;
            ViewBag.CreateTime = vm.WarehouseCheck.CreateTime;
            return View(vm);
        }

        [HttpGet]
        public ActionResult ExportCheckRF(string checknumber)
        {
            WarehouseCheckModel vm = new WarehouseCheckModel();
            StringBuilder sb = new StringBuilder();
            var response = new WarehouseService().GetPrintWareHouseCheckByCheckNumberNikeRF(checknumber);
            if (response.IsSuccess)
            {
                WarehouseCheck WarehouseCheckHeader = response.Result.WarehouseCheck;
                List<WarehouseCheckDetail> WarehouseCheckDetail = response.Result.WarehouseCheckDetailCollection.ToList();
                sb.Append("<style>td{mso-number-format:\"\\@\";}</style>");
                sb.Append("<table border='1' cellspacing='0' cellpadding='0'>");
                sb.Append("<tr><td>盘点单号：</td><td>" + WarehouseCheckHeader.CheckNumber.ToString() + "</td><td>外部盘点单号：</td><td>" + WarehouseCheckHeader.ExternNumber.ToString() + "</td>");
                sb.Append("<td>创建日期：</td><td>" + WarehouseCheckHeader.CreateTime.ToString() + "</td><td></td><td></td><td></td><td></td></tr>");
                sb.Append("<tr><td>行号</td><td>库位</td><td>品名</td><td>库存类型</td><td>库存数量</td><td>实际数量</td><td>差异数量</td></tr>");
                int i = 0;
                foreach (WarehouseCheckDetail item in WarehouseCheckDetail)
                {
                    sb.Append("<tr><td>" + i++ + "</td><td>" + item.Location.ToString() + "</td><td>" + item.SKU.ToString() + "</td>");
                    sb.Append("<td>" + item.GoodsType.ToString() + "</td><td>" + item.CheckQty.ToString() + "</td><td>" + item.ActualQty.ToString() + "</td><td>" + (item.ActualQty-item.CheckQty).ToString() + "</td></tr>");
                }
                sb.Append("</table>");
                SW.HttpResponse Response;
                Response = SW.HttpContext.Current.Response;
                Response.Charset = "UTF-8";
                Response.HeaderEncoding = Encoding.UTF8;
                Response.AppendHeader("content-disposition", "attachment;filename=\"" + SW.HttpUtility.UrlEncode("CheckDetails_" + DateTime.Now.ToShortDateString(), Encoding.UTF8) + ".xls\"");
                Response.ContentEncoding = Encoding.UTF8;
                Response.ContentType = "application/ms-excel";
                Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sb.ToString());
                Response.Flush();
                Response.End();
            }
            return View();
        }

        [HttpGet]
        public ActionResult ExportCheck(string checknumber)
        {
            WarehouseCheckModel vm = new WarehouseCheckModel();
            StringBuilder sb = new StringBuilder();
            var response = new WarehouseService().GetPrintWareHouseCheckByCheckNumberNike(checknumber);
            if (response.IsSuccess)
            {
                WarehouseCheck WarehouseCheckHeader = response.Result.WarehouseCheck;
                List<WarehouseCheckDetail> WarehouseCheckDetail = response.Result.WarehouseCheckDetailCollection.ToList();
                sb.Append("<style>td{mso-number-format:\"\\@\";}</style>");
                sb.Append("<table border='1' cellspacing='0' cellpadding='0'>");
                sb.Append("<tr><td>盘点单号：</td><td>" + WarehouseCheckHeader.CheckNumber.ToString() + "</td><td>外部盘点单号：</td><td>" + WarehouseCheckHeader.ExternNumber.ToString() + "</td>");
                sb.Append("<td>创建日期：</td><td>" + WarehouseCheckHeader.CreateTime.ToString() + "</td><td></td><td></td><td></td><td></td></tr>");
                sb.Append("<tr><td>业务类型</td><td>行号</td><td>库位</td><td>批次</td><td>品名</td><td>Article</td><td>Size</td><td>BU</td><td>库存类型</td><td>库存数量</td><td>实际数量</td></tr>");
                int i = 0;
                foreach (WarehouseCheckDetail item in WarehouseCheckDetail)
                {
                    sb.Append("<tr><td>" + item.BusinessType.ToString() + "</td><td>" + i++ + "</td><td>" + item.Location.ToString() + "</td><td>" + item.BatchNumber + "</td><td>" + item.SKU.ToString() + "</td>");
                    sb.Append("<td>" + item.str10.ToString() + "</td><td>" + item.str9.ToString() + "</td><td>" + item.str8.ToString() + "</td><td>" + item.GoodsType.ToString() + "</td><td>" + item.CheckQty.ToString() + "</td><td>" + item.ActualQty.ToString() + "</td></tr>");
                }
                sb.Append("</table>");
                SW.HttpResponse Response;
                Response = SW.HttpContext.Current.Response;
                Response.Charset = "UTF-8";
                Response.HeaderEncoding = Encoding.UTF8;
                Response.AppendHeader("content-disposition", "attachment;filename=\"" + SW.HttpUtility.UrlEncode("CheckDetails_" + DateTime.Now.ToShortDateString(), Encoding.UTF8) + ".xls\"");
                Response.ContentEncoding = Encoding.UTF8;
                Response.ContentType = "application/ms-excel";
                Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sb.ToString());
                Response.Flush();
                Response.End();
            }
            return View();
        }
        #endregion

        public static List<T> JSONStringToList<T>(string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);
            return objs;
        }

        /// <summary>
        /// 库区变更
        /// </summary>
        /// <param name="vm"></param>
        /// <param name="PageIndex"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public ActionResult AlterationLocation(IndexViewModel vm, int? PageIndex, long? customerID)
        {
            GetWarehouseByConditonRequest getWarehouseByConditionRequest = new GetWarehouseByConditonRequest();
            WarehouseSearchCondition ws = new WarehouseSearchCondition();
            ws.Address = null;
            ws.AreaID = 0;
            ws.Description = null;
            ws.ID = 0;
            ws.ProvinceCity = null;
            ws.SearchType = "1";
            ws.WarehouseName = null;
            ws.WarehouseStatus = null;
            ws.WarehouseType = null;
            ws.ProjectID = base.UserInfo.ProjectID;
            ws.UserID = base.UserInfo.ID;
            if (base.UserInfo.UserType == 0)
            {
                ws.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    ws.CustomerID = customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        ws.CustomerID = customerIDs.First();
                    }
                }
            }
            getWarehouseByConditionRequest.SearchCondition = ws;
            //getWarehouseByConditionRequest.SearchCondition = vm.SearchCondition;
            getWarehouseByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getWarehouseByConditionRequest.PageIndex = PageIndex ?? 0;
            var getWarehouseByConditionResponse = new WarehouseService().GetWarehouseByCondition(getWarehouseByConditionRequest);
            if (getWarehouseByConditionResponse.IsSuccess)
            {
                vm.WarehouseCollection = getWarehouseByConditionResponse.Result.WarehouseCollection;
                vm.PageIndex = getWarehouseByConditionResponse.Result.PageIndex;
                vm.PageCount = getWarehouseByConditionResponse.Result.PageCount;
                //Session["Warehouse_SearchCondition"] = vm.SearchCondition;
                //Session["Warehouse_PageIndex"] = vm.PageIndex;
            }

            //----------------------------------------------
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            //---------------------------------------------------


            var WarehouseListAll = ApplicationConfigHelper.GetCacheInfo();
            var WarehouseList = WarehouseListAll.Where(c => (c.ProjectID == base.UserInfo.ProjectID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            ViewBag.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetWarehouseList().Where(c => c.ProjectID == base.UserInfo.ProjectID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.WarehouseName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.CustomerID == ws.CustomerID)
                                     .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            return View(vm);
        }

        [HttpPost]
        public ActionResult AlterationLocation(IndexViewModel vm, int? PageIndex, string Action, long? customerID)
        {
            var getWarehouseByConditionRequest = new GetWarehouseByConditonRequest();
            vm.SearchCondition.ProjectID = base.UserInfo.ProjectID;
            vm.SearchCondition.UserID = base.UserInfo.ID;
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
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }
            if (Action == "查询" || Action == "Index")
            {
                getWarehouseByConditionRequest.SearchCondition = vm.SearchCondition;
                getWarehouseByConditionRequest.PageSize = UtilConstants.PAGESIZE;
                getWarehouseByConditionRequest.PageIndex = PageIndex ?? 0;
            }
            if (Action == "下载库区变更模板")
            {
                IEnumerable<Column> columnReceipt;
                var project = ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, vm.SearchCondition.CustomerID).ProjectCollection.First();
                Runbow.TWS.Entity.Module module = project.ModuleCollection.First(m => m.Id == "M002");
                if (project.ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.Where(t => t.Name == "WMS_AlterationLocation").Count() == 0)
                {
                    columnReceipt = (ApplicationConfigHelper.GetApplicationConfigNew(base.UserInfo.ProjectID, null)).ProjectCollection.First().ModuleCollection.First(m => m.Id == "M002").Tables.TableCollection.First(t => t.Name == "WMS_AlterationLocation").ColumnCollection.Where(c => c.IsImportColumn == true);
                }
                else
                {
                    columnReceipt = module.Tables.TableCollection.First(t => t.Name == "WMS_AlterationLocation").ColumnCollection.Where(c => c.IsImportColumn == true);
                }
                AltLocation(null, columnReceipt);
            }
            var getWarehouseByConditionResponse = new WarehouseService().GetWarehouseByCondition(getWarehouseByConditionRequest);
            if (getWarehouseByConditionResponse.IsSuccess)
            {
                vm.WarehouseCollection = getWarehouseByConditionResponse.Result.WarehouseCollection;
                vm.PageIndex = getWarehouseByConditionResponse.Result.PageIndex;
                vm.PageCount = getWarehouseByConditionResponse.Result.PageCount;
                //Session["Warehouse_SearchCondition"] = vm.SearchCondition;
                //Session["Warehouse_PageIndex"] = vm.PageIndex;
            }
            //---------------------------------------------------
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            //---------------------------------------------------
            
            var WarehouseListAll = ApplicationConfigHelper.GetCacheInfo();
            var WarehouseList = WarehouseListAll.Where(c => (c.ProjectID == base.UserInfo.ProjectID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct().Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            ViewBag.WarehouseList = WarehouseList;
            //ApplicationConfigHelper.GetWarehouseList().Where(c => c.ProjectID == base.UserInfo.ProjectID).Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.WarehouseName });
            ViewBag.AreaList = ApplicationConfigHelper.GetAllProjectCustomersWarehouse_Area().Where(c => c.CustomerID == vm.SearchCondition.CustomerID)
                                     .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.AreaName });
            return View(vm);
        }

        /// <summary>
        /// 下载模板
        /// </summary>
        /// <param name="response"></param>
        /// <param name="columnReceipt"></param>
        private void AltLocation(GetReceiptDetailByConditionResponse response, IEnumerable<Column> columnReceipt)
        {
            DataSet ds = new DataSet();
            DataTable dtReceipt = new DataTable();
            foreach (var receipt in columnReceipt)
            {
                dtReceipt.Columns.Add(receipt.DisplayName, typeof(string));
            }
            DataRow dr1 = dtReceipt.NewRow();

            dr1["原库区"] = "A01";
            dr1["库位"] = "A11-01-01";
            dr1["新库区"] = "D01";
            dtReceipt.Rows.Add(dr1);
            dtReceipt.TableName = "库区变更模板";
            ds.Tables.Add(dtReceipt);
            //ExportDataToExcelHelper.ExportDataSetToExcel(ds, "库区变更模板");
            EPPlusOperation.ExportByEPPlus(ds.Tables[0], "库区变更模板");

        }

        /// <summary>
        /// 导入原库区，新库区，库位
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpPost]
        public string AlLocationAndGoodShelfImport(string ID)
        {
            IndexViewModel vm = new IndexViewModel();
            string js = null;
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase hpf = Request.Files[0] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //DataSet ds = this.GetDataFromExcel(hpf);
                    DataSet ds = EPPlusOperation.ReadExcelToDataSet(hpf);
                    if (ds != null && ds.Tables[0] != null)
                    {
                        AddOrUpdateWarehouseLocationRequest request = new AddOrUpdateWarehouseLocationRequest();
                        StringBuilder sb = new StringBuilder();
                        var areaList = ApplicationConfigHelper.GetWarehouseAreaList(ID.ObjectToInt64());
                        var ToareaList = ApplicationConfigHelper.GetWarehouseAreaList(ID.ObjectToInt64());
                        string WarehouseName = ApplicationConfigHelper.GetAllProjectCustomersWarehouse().Where(a => a.WarehouseID.ToString() == ID).Select(b => b.WarehouseName).FirstOrDefault();
                        var LocationList = ApplicationConfigHelper.ALGetWarehouseLocationListByWarehouseName(WarehouseName);
                        string fromarea = ds.Tables[0].Rows[0]["原库区"].ToString();
                        string toarea = ds.Tables[0].Rows[0]["新库区"].ToString();
                        //string sqlString = "SELECT * FROM dbo.WMS_Warehouse_Location WHERE Location in ('";
                        string sqlString = "'";
                        string areString = "";
                        string ToareString = "";

                        int LocationCount = 0;

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (areaList.Where(c => c.AreaName == ds.Tables[0].Rows[i]["原库区"].ToString()).Select(m => m.ID).Count() == 0)
                            {
                                areString = areString + ds.Tables[0].Rows[i]["原库区"].ToString() + ",";
                            }
                            if (areaList.Where(c => c.AreaName == ds.Tables[0].Rows[i]["新库区"].ToString()).Select(m => m.ID).Count() == 0)
                            {
                                ToareString = ToareString + ds.Tables[0].Rows[i]["新库区"].ToString() + ",";
                            }

                            sqlString = sqlString + ds.Tables[0].Rows[i]["库位"].ToString() + "','";
                        }
                        //获取库位总数
                        LocationCount = ds.Tables[0].Rows.Count;
                        sqlString = sqlString.Substring(0, sqlString.Length - 2);//去掉后面三个字符
                                                                                 //sqlString = sqlString + "'" + ") AND WarehouseID =" + ID;

                        //对原库区是否存在进行判断
                        if (areString != "")
                        {
                            return new { result = "<h3><font color='#FF0000'>导入失败,下列库区不存在：" + areString + "</font></h3>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                        }
                        //对新库区是否存在进行判断
                        if (ToareString != "")
                        {
                            return new { result = "<h3><font color='#FF0000'>导入失败,下列库区不存在：" + ToareString + "</font></h3>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                        }

                        //获取库区ID
                        string AreaID = LocationList.Where(a => a.AreaName == fromarea).First().AreaID.ToString();
                        string ToAreID = LocationList.Where(a => a.AreaName == toarea).First().AreaID.ToString();
                        
                        //进行导入
                        var response = new WarehouseService().AlImportLocationAndGoodShelf(ID, AreaID, ToAreID, sqlString, LocationCount);

                        //返回信息
                        if (response.IsSuccess)
                        {
                            return new { result = "<h3><font color='#00dd00'>导入成功,更新数量为：" + LocationCount + "</font></h3>", IsSuccess = true }.ToJsonString();
                        }
                        else if (response.SuccessMessage.ToString().Contains("库存数量不为0"))
                        {
                            return new { result = "<h3><font color='#FF0000'>导入失败！库存数量不为0</font></h3>", IsSuccess = false }.ToJsonString();
                        }
                        else if (response.SuccessMessage.ToString().Contains("导入库位数与数据库数量不符"))
                        {
                            return new { result = "<h3><font color='#FF0000'>导入失败！导入库位数与数据库数量不符</font></h3>", IsSuccess = false }.ToJsonString();
                        }
                        else
                        {
                            return new { result = "<h3><font color='#FF0000'>导入失败！</font></h3>" + sb.ToString(), IsSuccess = false }.ToJsonString();
                        }
                    }
                }
            }
            return new { result = "<h3>失败</h3>", IsSuccess = false }.ToJsonString();
        }

        /// <summary>
        /// 导出库位
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetWLocationInfo(CommonRequest request)
        {
            ResponseData res = new ResponseData();
            WLocationSearchCondition cond = new WLocationSearchCondition();
            IEnumerable<LocationInfo> list = null;  //ProductStorer
            res.code = 401;
            try
            {
                if (request.requestData != null)
                    cond = JsonHelper.Deserialize<WLocationSearchCondition>(request.requestData);
            }
            catch (Exception ex)
            {
                res.code = 402;
                res.msg = "JSON字符串转数组对象错误" + ex.Message;
                return Json(res);
            }
            try
            {
                var CustomerListTemp = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3).Select(t => t.CustomerID);

                var WarehouseIDD = ApplicationConfigHelper.GetCacheInfo().Where(c => c.UserID == base.UserInfo.ID && CustomerListTemp.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                                    .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });

                if (string.IsNullOrEmpty(cond.WarehouseID))
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var i in WarehouseIDD)
                    {
                        sb.Append("" + i.Value + ",");
                    }
                    if (sb.Length > 1)
                    {
                        cond.WarehouseID = sb.Remove(sb.Length - 1, 1).ToString();
                    }
                }                                                  
                var response = new WarehouseService().GetWLocationInfo(new GetWLocationByConditonRequest()
                {
                    PageIndex = request.page - 1,
                    PageSize = request.limit,
                    WLSearchCondition = cond
                });
                if (response.IsSuccess)
                {
                    list = response.Result.WLocationCollection;
                    res.count = response.Result.RowCount;
                }
                res.data = list;
                res.code = list != null && list.Any() ? 0 : 0;
                res.msg = list != null && list.Any() ? "200" : "无数据";
            }
            catch (Exception ex)
            {
                res.msg = ex.Message;
            }
            return Json(res);
        }

    }
}