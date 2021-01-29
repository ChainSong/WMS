using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Web.Areas.WMS.Models.LogOperationManagement;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Entity.WMS.Log;
using System.Text;
using Runbow.TWS.MessageContracts.WMS.LogOperation;
using UtilConstants = Runbow.TWS.Common.Constants;
using SW = System.Web;
namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class LogOperationManagementController : BaseController
    {
        //
        // GET: /WMS/LogOperationManagement/

        public ActionResult Index(int? PageIndex, long? customerID)
        {
            IndexViewModel vm = new IndexViewModel();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;

            
            vm.LogOperationCondition = new LogOperationSearchCondition();
            vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            if (base.UserInfo.UserType == 0)
            {
                vm.LogOperationCondition.CustomerID = (int)base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 2)
            {
                if (customerID.HasValue)
                {
                    vm.LogOperationCondition.CustomerID = (int)customerID;
                }
                else
                {
                    var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
                    if (customerIDs != null && customerIDs.Count() == 1)
                    {
                        vm.LogOperationCondition.CustomerID = (int)customerIDs.First();
                    }
                }
            }
            vm.LogOperationCondition.StartCreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
            vm.LogOperationCondition.EndCreateTime = DateTime.Now;

            IEnumerable<SelectListItem> WarehouseList = null;
            var WarehouseListAll = ApplicationConfigHelper.GetCacheInfo();
            if (vm.LogOperationCondition.CustomerID == null)
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
                    vm.LogOperationCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.LogOperationCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = WarehouseListAll.Where(c => (c.CustomerID == vm.LogOperationCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName, Text = c.WarehouseName });
            }

            ViewBag.WarehouseList = WarehouseList;


            //new LogOperationService().GetLogOperationByCondition(null);
            GetLogOperationByConditionRequest request = new GetLogOperationByConditionRequest();
            request.SearchCondition = vm.LogOperationCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            var response = new LogOperationService().GetLogOperationByCondition(request);
            if (response.IsSuccess)
            {
                vm.LogOperationCollection = response.Result.LogOperationCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }

            return View(vm);
        }

        public ActionResult RFIndex(int? PageIndex, long? customerID)
        {
            IndexViewModel vm = new IndexViewModel();        
            vm.LogOperationRFCondition = new LogOperationRFSearchCondition();
            vm.LogOperationRFCondition.StartCreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
            vm.LogOperationRFCondition.EndCreateTime = DateTime.Now;
            GetLogOperationByConditionRequest request = new GetLogOperationByConditionRequest();
            request.RFSearchCondition = vm.LogOperationRFCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            var response = new LogOperationService().GetLogOperationRFByCondition(request);
            if (response.IsSuccess)
            {
                vm.LogOperationRFCollection = response.Result.LogOperationRFCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(IndexViewModel vm, int? PageIndex, string Action)
        {
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var CustomerListID = CustomerListAll.Select(t => t.CustomerID);
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            //ApplicationConfigHelper.GetApplicationCustomer().Where(c => c.UserID == UserInfo.ID)
            //.Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            IEnumerable<SelectListItem> WarehouseList = null;
            if (vm.LogOperationCondition.CustomerID == null)
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
                    vm.LogOperationCondition.CustomerIDs = sb.Remove(sb.Length - 1, 1).ToString();
                }
                else
                {
                    vm.LogOperationCondition.CustomerIDs = "0";
                }
            }
            else
            {
                WarehouseList = ApplicationConfigHelper.GetCacheInfo().Where(c => (c.CustomerID == vm.LogOperationCondition.CustomerID && c.UserID == base.UserInfo.ID)).Select(t => new { WarehouseID = t.WarehouseID, WarehouseName = t.WarehouseName }).Distinct()
                                     .Select(c => new SelectListItem() { Value = c.WarehouseName.ToString(), Text = c.WarehouseName });
            }
            ViewBag.WarehouseList = WarehouseList;

            //getASNByConditionRequest.SearchCondition = vm.ASNCondition;
            //getASNByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            //getASNByConditionRequest.PageIndex = PageIndex ?? 0;
            //var getASNByConditionResponse = new ASNManagementService().GetASNDetailByConditionResponse(getASNByConditionRequest);
            GetLogOperationByConditionRequest request = new GetLogOperationByConditionRequest();
            request.SearchCondition = vm.LogOperationCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            var response = new LogOperationService().GetLogOperationByCondition(request);
            if (response.IsSuccess)
            {
                vm.LogOperationCollection = response.Result.LogOperationCollection;
                vm.PageIndex = response.Result.PageIndex;
                vm.PageCount = response.Result.PageCount;
            }
            return View(vm);
        }


        [HttpPost]
        public ActionResult RFIndex(IndexViewModel vm, int? PageIndex, string Action)
        {
            GetLogOperationByConditionRequest request = new GetLogOperationByConditionRequest();
            request.RFSearchCondition = vm.LogOperationRFCondition;
            request.PageSize = UtilConstants.PAGESIZE;
            request.PageIndex = PageIndex ?? 0;
            if (Action == "查询" || Action == "RFIndex")
            {
                var response = new LogOperationService().GetLogOperationRFByCondition(request);
                if (response.IsSuccess)
                {
                    vm.LogOperationRFCollection = response.Result.LogOperationRFCollection;
                    vm.PageIndex = response.Result.PageIndex;
                    vm.PageCount = response.Result.PageCount;
                }
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                var response = new LogOperationService().ExportLogOperationRFByCondition(request);
                List<WMS_Log_OperationRF> LogRFDetail = response.Result.LogOperationRFCollection.ToList();
                sb.Append("<style>td{mso-number-format:\"\\@\";}</style>");
                sb.Append("<table border='1' cellspacing='0' cellpadding='0'>");
                sb.Append("<tr><td>操作类型</td><td>订单号</td><td>单状态</td><td>箱号</td><td>箱状态</td><td>SKU</td><td>数量</td><td>库区</td><td>库位</td><td>创建人</td><td>创建时间</td><td>更新人</td><td>更新时间</td></tr>");
                int i = 0;
                foreach (WMS_Log_OperationRF item in LogRFDetail)
                {
                    sb.Append("<tr><td>" + item.LogType.ToString() + "</td><td>" + item.ReleateNumber.ToString() + "</td>");
                    sb.Append("<td>" + (item.ReleateStatus == 0 ? "未完成" : "已完成") + "</td><td>" + item.PackageNumber.ToString() + "</td><td>" + (item.PackageStatus == 0 ? "未完成" : "已完成") + "</td><td>" + item.SKU.ToString() + "</td>");
                    sb.Append("<td>" + item.Qty.ToString() + "</td><td>" + item.Area.ToString() + "</td><td>" + item.Location.ToString() + "</td><td>" + item.Creator.ToString() + "</td>");
                    sb.Append("<td>" + item.CreateTime.ToString() + "</td><td>" + item.Updator.ToString() + "</td><td>" + item.UpdateTime.ToString() + "</td></tr>");
                }
                sb.Append("</table>");
                SW.HttpResponse Response;
                Response = SW.HttpContext.Current.Response;
                Response.Charset = "UTF-8";
                Response.HeaderEncoding = Encoding.UTF8;
                Response.AppendHeader("content-disposition", "attachment;filename=\"" + SW.HttpUtility.UrlEncode("RF操作日志_" + DateTime.Now.ToShortDateString(), Encoding.UTF8) + ".xls\"");
                Response.ContentEncoding = Encoding.UTF8;
                Response.ContentType = "application/ms-excel";
                Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sb.ToString());
                Response.Flush();
                Response.End();
            }
            return View(vm);
        }
    }
}
