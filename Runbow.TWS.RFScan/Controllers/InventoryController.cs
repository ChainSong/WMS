using Newtonsoft.Json;
using Runbow.TWS.Biz.RFWeb;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.RFScan.Common;
using Runbow.TWS.RFScan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Runbow.TWS.Entity.WMS.Log;
using System.Web.Script.Serialization;
using Runbow.TWS.Entity.WMS.Warehouse;
using Runbow.TWS.Logger.LogHelper;

namespace Runbow.TWS.RFScan.Controllers
{
    public class InventoryController : Controller
    {
        //
        // GET: /Inventory/ 
        public ActionResult Index(string CustomerID, string WareHouseName, string WareHouseID)
        {
            //IEnumerable<WarehouseCheck> checklist= new InventoryManagementService().GetWarehouseCheck(long.Parse(CustomerID), WareHouseName);
            //return Json(checklist.ToList(), JsonRequestBehavior.AllowGet);
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            return View();
        }
        public ActionResult Inventory(string CustomerID, string WareHouseName, string WareHouseID)
        {
            //IEnumerable<WarehouseCheck> checklist= new InventoryManagementService().GetWarehouseCheck(long.Parse(CustomerID), WareHouseName);
            //return Json(checklist.ToList(), JsonRequestBehavior.AllowGet);
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            return View();
        }
        [HttpPost]
        public ActionResult GetCheckList(string CustomerID, string WareHouseName, string WareHouseID)
        {
            IEnumerable<WarehouseCheck> checklist = new InventoryManagementService().GetWarehouseCheck(long.Parse(Session["CustomerID"].ToString()), Session["WareHouseName"].ToString());
            return Json(checklist.ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetInventory(string SKU)
        {
            IEnumerable<Inventorys> checklist = new InventoryManagementService().GetInventoryForRFBySKU(long.Parse(Session["CustomerID"].ToString()), Session["WareHouseName"].ToString(), SKU);
            return Json(checklist.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckDetail(string CheckNumber)
        {
            ViewBag.CheckNumber = CheckNumber;
            return View();
        }
        public static List<T> JSONStringToList<T>(string JsonStr)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<T> objs = Serializer.Deserialize<List<T>>(JsonStr);
            return objs;
        }
        public string SaveCheckList(string CheckNumber, string JsonData)
        {
            IEnumerable<WarehouseCheckDetail> receivelist = JSONStringToList<WarehouseCheckDetail>(JsonData).AsEnumerable();
            List<WarehouseCheckDetail> list = new List<WarehouseCheckDetail>();

            var response = new InventoryManagementService().InsertCheckDetailList(receivelist.ToList(), Session["Name"].ToString(), CheckNumber);

            if (response)
            {
                return "1";
            }
            return "0";
        }
        public JsonResult UintList()
        {
            IEnumerable<WMS_UnitAndSpecifications_Config> unit = ApplicationConfigHelper.GetWMS_UnitAndSpecifications_Config(long.Parse(Session["ProjectID"].ToString()), long.Parse(Session["CustomerID"].ToString()), long.Parse(Session["WareHouseIDs"].ToString()));//货品单位信息
            return Json(unit);
        }
        public JsonResult GoodsTypeList()
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

        public ActionResult MoveInventory(string CustomerID, string WareHouseName, string WareHouseID)
        {
            if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            {
                return RedirectToAction("", "login");
            }
            Session["CustomerID"] = CustomerID;
            Session["WareHouseName"] = WareHouseName;
            Session["WareHouseIDs"] = WareHouseID;
            ViewBag.WareHouseID = WareHouseID;
            ViewBag.UserName = Session["Name"].ToString();
            return View();
        }

        public JsonResult CheckBoxNumber(string BoxNumber)
        {
            try
            {
                SysLogWriter.Error(Session["Name"].ToString() + "开始验证原库位" + BoxNumber);
                var response = new InventoryManagementService().CheckBoxNumber(long.Parse(Session["CustomerID"].ToString()), Session["WareHouseName"].ToString(), BoxNumber);
                if (response > 0)
                {
                    return Json(new { Code = response });
                }
                else
                {
                    return Json(new { Code = 0 });
                }
            }
            catch (Exception ex)
            {
                SysLogWriter.Error("CheckBoxNumber（整箱）" + Session["Name"].ToString() + ex.ToString());
            }
            return Json(new { Code = 0 });
        }
        public JsonResult MoveLocation(string BoxNumber, string NewLocation)
        {
            try
            {
                SysLogWriter.Error(Session["Name"].ToString() + "提交移库" + BoxNumber+"："+ NewLocation);
                var response = new InventoryManagementService().MoveLocation(long.Parse(Session["CustomerID"].ToString()), Session["WareHouseName"].ToString(), BoxNumber, NewLocation, Session["Name"].ToString());
                return Json(new { Code = response });

            }
            catch (Exception ex)
            {
                SysLogWriter.Error("MoveLocation（整箱）" + Session["Name"].ToString() + ex.ToString());
            }
            return Json(new { Code = 0 });
        }

    }
}
