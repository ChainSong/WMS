using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.POD.Models;
using Runbow.TWS.Web.Common;
using UtilConstants = Runbow.TWS.Common.Constants;
using WebConstants = Runbow.TWS.Web.Common.Constants;
namespace Runbow.TWS.Web.Areas.POD.Controllers
{
    public class PrintPODController : BaseController
    {
        //
        // GET: /POD/PrintPOD/
        [HttpGet]
        public ActionResult PrintPod()
        {

            QueryPodViewModel vm = new QueryPodViewModel();
            vm.SearchCondition = new PodSearchCondition();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            vm.SearchCondition.RuleArea = base.UserInfo.RuleArea;
            vm.SearchCondition.UserID = base.UserInfo.ID.ToString();
            if (base.UserInfo.UserType == 1)
            {
                vm.SearchCondition.ShipperID = base.UserInfo.CustomerOrShipperID;
            } 
            var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
            //vm.IsInnerUser = vm.ShowCustomerOrShipperDrop = base.UserInfo.UserType == 2;
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            vm.SearchCondition.CustomerIDs = customerIDs;
            GenQueryPodViewModel(vm);
            var result = new PodService().QueryPod(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
            vm.PodCollection = result.PodCollections;
            vm.PageIndex = result.PageIndex;
            vm.PageCount = result.PageCount;
            return View(vm);
        }
        [HttpPost]
        public ActionResult PrintPod(QueryPodViewModel vm, int? PageIndex, string Action)
        {
            GenQueryPodViewModel(vm);
          
            
            if (base.UserInfo.UserType == 1)
            {
                vm.SearchCondition.ShipperID = base.UserInfo.CustomerOrShipperID;
            }
            var customerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
            vm.SearchCondition.RuleArea = base.UserInfo.RuleArea;
            vm.SearchCondition.UserID = base.UserInfo.ID.ToString();
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            vm.SearchCondition.CustomerIDs = customerIDs;
            var result = new PodService().QueryPod(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
            vm.PodCollection = result.PodCollections;
            vm.PageIndex = result.PageIndex;
            vm.PageCount = result.PageCount;
            return View(vm);
        }
        private void GenQueryPodViewModel(QueryPodViewModel vm)
        {
            vm.Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "Pod");
            vm.ShipperTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.PodStates = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.PODSTATE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                          .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            
            vm.Shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID)
                             .Select(c => new SelectListItem() { Value = c.ShipperID.ToString(), Text = c.Name });
            vm.PodStates = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.PODSTATE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.TtlOrTpls = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.TTLORTPL)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
        }
        public ActionResult PrintDemo(string SystemNumber)
        {
            QueryPodViewModel vm = new QueryPodViewModel();

            PodSearchCondition SearchCondition = new PodSearchCondition();
            SearchCondition.SystemNumber = SystemNumber;
            vm.SearchCondition = SearchCondition;

            var results = new PodService().QueryBSPod(new QueryPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = 0, SearchCondition = vm.SearchCondition, ProjectID = base.UserInfo.ProjectID }).Result;
            vm.PodCollection = results.PodCollections;
            vm.PageIndex = results.PageIndex;
            vm.PageCount = results.PageCount;
            //IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            ////这里使用自定义日期格式，如果不使用的话，默认是ISO8601格式  
            //timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";

            //string jsonStr = JsonConvert.SerializeObject(results.PodCollections, Formatting.Indented, timeConverter);
            return View(vm);
            //return jsonStr;
        }
    }
}
