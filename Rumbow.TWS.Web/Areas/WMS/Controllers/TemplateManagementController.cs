using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts.WMS.Template;
using Runbow.TWS.Web.Areas.WMS.Models.TemplateManagement;
using Runbow.TWS.Web.Common;
using UtilConstants = Runbow.TWS.Common.Constants;
namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class TemplateManagementController : BaseController
    {
        //
        // GET: /WMS/TemplateManagement/
        [HttpGet]
        public ActionResult Index(int? PageIndex, long ProjectID = 0, long CustomerID = 0)
        {
            IndexViewModel vm = new IndexViewModel();
            vm.searchCondition = new SearchCondition();
            vm.searchCondition.ProjectID = ProjectID;
            vm.searchCondition.CustomerID = CustomerID;
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var ProjectList = CustomerListAll.Select(t => new { Value = t.ProjectID, Text = t.ProjectName }).Distinct().Select(c => new SelectListItem() { Value = c.Value.ToString(), Text = c.Text });
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            ViewBag.ProjectList = ProjectList;
            GetTemplateByConditionRequest getTemplateByConditionRequest = new GetTemplateByConditionRequest();
            getTemplateByConditionRequest.SearchCondition = vm.searchCondition;
            getTemplateByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getTemplateByConditionRequest.PageIndex = PageIndex ?? 0;
            var getTemplateByConditionResponse = new TemplateManagementService().GetTemplateByCondition(getTemplateByConditionRequest);
            if (getTemplateByConditionResponse.IsSuccess)
            {
                vm.TemplateCollection = getTemplateByConditionResponse.Result.TemplateCollection;
                vm.PageIndex = getTemplateByConditionResponse.Result.PageIndex;
                vm.PageCount = getTemplateByConditionResponse.Result.PageCount;
            }
            return View(vm);
        }
        [HttpPost]
        public ActionResult Index(IndexViewModel vm, int? PageIndex)
        {
            vm.searchCondition = new SearchCondition();
            var CustomerListAll = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Where(t => t.StoreType == 2 || t.StoreType == 3);
            var ProjectList = CustomerListAll.Select(t => new { Value = t.ProjectID, Text = t.ProjectName }).Distinct().Select(c => new SelectListItem() { Value = c.Value.ToString(), Text = c.Text });
            var CustomerList = CustomerListAll.Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            ViewBag.CustomerList = CustomerList;
            ViewBag.ProjectList = ProjectList;
            GetTemplateByConditionRequest getTemplateByConditionRequest = new GetTemplateByConditionRequest();
            getTemplateByConditionRequest.SearchCondition = vm.searchCondition;
            getTemplateByConditionRequest.PageSize = UtilConstants.PAGESIZE;
            getTemplateByConditionRequest.PageIndex = PageIndex ?? 0;
            var getTemplateByConditionResponse = new TemplateManagementService().GetTemplateByCondition(getTemplateByConditionRequest);
            if (getTemplateByConditionResponse.IsSuccess)
            {
                vm.TemplateCollection = getTemplateByConditionResponse.Result.TemplateCollection;
                vm.PageIndex = getTemplateByConditionResponse.Result.PageIndex;
                vm.PageCount = getTemplateByConditionResponse.Result.PageCount;
            }
            return View(vm);
        }
        [HttpGet]
        public ActionResult TemplateCreate(string TableName, string TableNameCH, string ProjectName, string CustomerName, long ProjectID = 0, long CustomerID = 0)
        {
            IndexViewModel vm = new IndexViewModel();
            vm.searchCondition = new SearchCondition();
            vm.searchCondition.ProjectID = ProjectID;
            vm.searchCondition.CustomerID = CustomerID;
            vm.searchCondition.ProjectName = ProjectName;
            vm.searchCondition.CustomerName = CustomerName;
            vm.searchCondition.TableName = TableName;
            vm.searchCondition.TableNameCH = TableNameCH;
            GetTemplateByConditionRequest getTemplateByConditionRequest = new GetTemplateByConditionRequest();
            getTemplateByConditionRequest.SearchCondition = vm.searchCondition;
            var getTemplateDetailByConditionResponse = new TemplateManagementService().GetTemplateDetailByCondition(getTemplateByConditionRequest);
            if (getTemplateDetailByConditionResponse.IsSuccess)
            {
                vm.TemplateCollection = getTemplateDetailByConditionResponse.Result.TemplateCollection;
            }
            return View(vm);
        }
        [HttpPost]
        public string TemplateCreate(string jsonString, long ProjectID, long CustomerID, string TableName)
        {
            string s = "";
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<TableColumn> objs = Serializer.Deserialize<List<TableColumn>>(jsonString);
            GetTemplateByConditionRequest getTemplateByConditionRequest = new GetTemplateByConditionRequest();
            getTemplateByConditionRequest.tableColumns = objs;
            SearchCondition serch = new SearchCondition();
            serch.ProjectID = ProjectID;
            serch.CustomerID = CustomerID;
            serch.TableName = TableName;
            getTemplateByConditionRequest.SearchCondition = serch;
            var Responses = new TemplateManagementService().EditTemplateDetail(getTemplateByConditionRequest);
            if (Responses.IsSuccess)
            {
                s = Responses.Result;
                ApplicationConfigHelper.RefreshGetApplicationConfigNew(ProjectID, CustomerID);
            }
            return s;
        }

    }
}
