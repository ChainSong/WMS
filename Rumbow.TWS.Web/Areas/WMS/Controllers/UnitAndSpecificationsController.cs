using Runbow.TWS.Biz.UnitAndSpecifications;
using Runbow.TWS.Entity.WMS.UnitAndSpecifications;
using Runbow.TWS.MessageContracts.UnitAndSpecifications;
using Runbow.TWS.Web.Areas.WMS.Models.UnitAndSpecifications;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class UnitAndSpecificationsController : BaseController
    {
        [HttpGet]
        public ActionResult Index(int CustomerID = 0)
        {

            UnitAndSpecificationsModel vm = new UnitAndSpecificationsModel();
            vm.unitAndSpecificationsInfo = new UnitAndSpecificationsInfo();

            vm.CustomerItems = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3))
                .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            if (CustomerID != 0)
            {
                vm.unitAndSpecificationsInfo.CustomerID = CustomerID;
            }
            StringBuilder sb = new StringBuilder();
            foreach (var i in vm.CustomerItems)
            {
                sb.Append("" + i.Value + ",");
            }
            if (sb.Length > 1)
            {
                vm.unitAndSpecificationsInfo.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
            }
            var response = new UnitAndSpecificationsService().GetUnitAndSpecifications(new UnitAndSpecificationsRequest()
            {
                unitAndSpecificationsInfo = vm.unitAndSpecificationsInfo
            });
            if (response.IsSuccess)
            {
                vm.unitAndSpecificationsInfos = response.Result.unitAndSpecificationsInfos;
            }
            #region 页面customerid读取
            //IEnumerable<WMS_Config_Type> ctype = null;
            //ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = "";
            #endregion
            return View(vm);
        }
        [HttpPost]
        public ActionResult Index(UnitAndSpecificationsModel vm)
        {

            vm.CustomerItems = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3))
                .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            StringBuilder sb = new StringBuilder();
            foreach (var i in vm.CustomerItems)
            {
                sb.Append("" + i.Value + ",");
            }
            if (sb.Length > 1)
            {
                vm.unitAndSpecificationsInfo.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
            }
            var response = new UnitAndSpecificationsService().GetUnitAndSpecifications(new UnitAndSpecificationsRequest()
            {
                unitAndSpecificationsInfo = vm.unitAndSpecificationsInfo
            });
            if (response.IsSuccess)
            {
                vm.unitAndSpecificationsInfos = response.Result.unitAndSpecificationsInfos;
            }
            #region 页面customerid读取
            //IEnumerable<WMS_Config_Type> ctype = null;
            //ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = "";
            #endregion
            return View(vm);
        }
        [HttpGet]
        public ActionResult AddUnitAndSpecifications()
        {

            UnitAndSpecificationsModel vm = new UnitAndSpecificationsModel();
            vm.unitAndSpecificationsInfo = new UnitAndSpecificationsInfo();

            vm.CustomerItems = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3))
                .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });


            return View(vm);
        }
        [HttpPost]
        public ActionResult AddUnitAndSpecifications(UnitAndSpecificationsModel vm)
        {

            vm.CustomerItems = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(c => c.State && (c.StoreType == 2 || c.StoreType == 3))
                .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });

            StringBuilder sb = new StringBuilder();
            foreach (var i in vm.CustomerItems)
            {
                sb.Append("" + i.Value + ",");
            }
            if (sb.Length > 1)
            {
                vm.unitAndSpecificationsInfo.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
            }
            vm.unitAndSpecificationsInfo.ProjectID = base.UserInfo.ProjectID;

            var response = new UnitAndSpecificationsService().AddUnitAndSpecifications(new UnitAndSpecificationsRequest()
            {
                unitAndSpecificationsInfo = vm.unitAndSpecificationsInfo
            });
            if (response.IsSuccess)
            {
                ApplicationConfigHelper.RefreshGetGetWMS_UnitAndSpecifications_Config(vm.unitAndSpecificationsInfo.ProjectID, vm.unitAndSpecificationsInfo.CustomerID, 0);
                return RedirectToAction("Index", "UnitAndSpecifications", new { vm.unitAndSpecificationsInfo.CustomerID });
                //Response.Redirect(" UnitAndSpecifications/Index" , vm.unitAndSpecificationsInfo.CustomerID);
            }
            #region 页面customerid读取
            //IEnumerable<WMS_Config_Type> ctype = null;
            //ctype = ApplicationConfigHelper.GetWMS_ConfigType("CustomerID", base.UserInfo.ProjectID, 0, 0);
            ViewBag.ctype = "";
            #endregion
            return View(vm);
        }
        [HttpPost]
        public JsonResult DeleteUnitAndSpecifications(int id = 0)
        {


            var response = new UnitAndSpecificationsService().DeleteUnitAndSpecifications(id);
            if (response)
            {
                return Json(new { Code = 1 });
            }
            return Json(new { Code = 0 });
        }
    }
}