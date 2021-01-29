using Runbow.TWS.Biz.RFWeb;
using Runbow.TWS.Logger.LogHelper;
using Runbow.TWS.RFScan.Common; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.RFScan.Controllers
{
    public class BestLocationController : Controller
    {
        //
        // GET: /BestLocation/

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
        /// <summary>
        /// 获取推荐箱
        /// </summary>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public JsonResult GetBestLocation(string BoxNumber)
        {
            BestLocationManagementService Service = new BestLocationManagementService();
            SysLogWriter.Error("获取推荐箱");
            try
            {
                var response = Service.GetBestLocation(Session["CustomerID"].ToString(), Session["WareHouseName"].ToString(), BoxNumber);
                if (!string.IsNullOrEmpty(response))
                {
                    return Json(new { Code = 1, Data = response });
                }
                return Json(new { Code = 0 });
            }
            catch (Exception ex)
            {
                SysLogWriter.Error("获取推荐箱失败：" + ex.Message);
                return Json(new { Code = 0 });
            }
        }


        /// <summary>
        /// 获取推荐箱
        /// </summary>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public JsonResult RefreshLocation(string BoxNumber,string Location)
        {
            BestLocationManagementService Service = new BestLocationManagementService();
            SysLogWriter.Error("获取推荐箱");
            try
            {
                var response = Service.RefreshLocation(Session["CustomerID"].ToString(), Session["WareHouseName"].ToString(), BoxNumber,Location);
                if (!string.IsNullOrEmpty(response))
                {
                    return Json(new { Code = 0, Data = response });
                }
                return Json(new { Code = 0 });
            }
            catch (Exception ex)
            {
                SysLogWriter.Error("获取推荐箱失败：" + ex.Message);
                return Json(new { Code = 0 });
            }
        }
    }
}
