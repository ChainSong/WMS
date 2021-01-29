using Runbow.TWS.RFScan.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.RFScan.Controllers
{
    public class MoveController : Controller
    {
        //
        // GET: /Move/

        public ActionResult Index(string CustomerID, string WareHouseName, string WareHouseID)
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

        public JsonResult MoveLocation(string CustomerID, string WareHouseName, string WareHouseID)
        {
            //if (Session[Constants.USER_INFO_KEY] == null || CustomerID == null || WareHouseName == null || WareHouseID == null)
            //{
            //    return RedirectToAction("", "login");
            //}
            //Session["CustomerID"] = CustomerID;
            //Session["WareHouseName"] = WareHouseName;
            //Session["WareHouseIDs"] = WareHouseID;
            //ViewBag.WareHouseID = WareHouseID;
            //ViewBag.UserName = Session["Name"].ToString();
            return Json(new { Code = 0 });
        }
    }
}
