using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Order;
using Runbow.TWS.MessageContracts.WMS.BarCode;
using Runbow.TWS.Web.Areas.WMS.Models.BarCode;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Common;
using UtilConstants = Runbow.TWS.Common.Constants;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class BarCodeController : BaseController
    {
        //
        // GET: /WMS/BarCode/
        [HttpGet]
        public ActionResult Index(int? PageIndex, long? customerID)
        {
            BarCodeModel vm = new BarCodeModel();
            vm.SearchCondition = new BarCodeSearchCondition();
            #region 订单类型
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("BarCodeType_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("BarCodeType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.BarCodeType = st;
            #endregion
            #region 客户
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
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.SearchCondition.CustomerID = customerIDs.First();
                    }
                }
            }
            #endregion
            #region 仓库
            IEnumerable<SelectListItem> WarehouseList = null;
            var WarehouseListAll = ApplicationConfigHelper.GetCacheInfo();
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = WarehouseListAll.Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
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
                WarehouseList = WarehouseListAll.Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            ViewBag.WarehouseList = WarehouseList;
            #endregion
            if (CustomerList.Count() == 1)
            {
                vm.SearchCondition.CustomerID = CustomerList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            }
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            }
            
            
            vm.SearchCondition.StartCreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
            vm.SearchCondition.EndCreateTime = DateTime.Now;
            var response = new BarCodeService().QueryBarCodeList(new GetBarCodeByConditionRequest()
            {
                SearchCondition = null,
                PageIndex = PageIndex ?? 0,
                PageSize = UtilConstants.PAGESIZE,
            });
            if (response.IsSuccess)
            {
                vm.BarCodeCollection = response.Result.BarCodeCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            return View(vm);
        }
        [HttpPost]
        public ActionResult Index(BarCodeModel vm, int? PageIndex, string Action)
        {
            #region 订单类型
            IEnumerable<WMSConfig> wms = null;
            try
            {
                wms = ApplicationConfigHelper.GetWMS_Config("BarCodeType_" + base.UserInfo.ProjectName);
            }
            catch (Exception)
            {
            }

            if (wms == null)
            {
                wms = ApplicationConfigHelper.GetWMS_Config("BarCodeType");
            }
            List<SelectListItem> st = new List<SelectListItem>();
            foreach (WMSConfig w in wms)
            {
                st.Add(new SelectListItem() { Value = w.Name, Text = w.Name });
            }
            vm.BarCodeType = st;
            #endregion
            #region 客户
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            #endregion
            #region 仓库
            IEnumerable<SelectListItem> WarehouseList = null;
            var WarehouseListAll = ApplicationConfigHelper.GetCacheInfo();
            if (vm.SearchCondition.CustomerID == null)
            {
                WarehouseList = WarehouseListAll.Where(c => c.UserID == base.UserInfo.ID && CustomerListID.Contains(c.CustomerID.Value)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
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
                WarehouseList = WarehouseListAll.Where(c => (c.CustomerID == vm.SearchCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseID.ToString(), Text = c.WarehouseName });
            }

            ViewBag.WarehouseList = WarehouseList;
            #endregion
            if (CustomerList.Count() == 1)
            {
                vm.SearchCondition.CustomerID = CustomerList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            }
            if (WarehouseList.Count() == 1)
            {
                vm.SearchCondition.WarehouseID = WarehouseList.Select(c => c.Value).FirstOrDefault().ObjectToInt64();
            }
            var response = new BarCodeService().QueryBarCodeList(new GetBarCodeByConditionRequest()
            {
                SearchCondition = vm.SearchCondition,
                PageIndex = PageIndex ?? 0,
                PageSize = UtilConstants.PAGESIZE,
            });
            if (response.IsSuccess)
            {
                vm.BarCodeCollection = response.Result.BarCodeCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            return View(vm);
        }
    }
}
