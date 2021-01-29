using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.POD.Models;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Web.Implement;
using Runbow.TWS.Web.Interface;
using MyFile = System.IO.File;
using UtilConstants = Runbow.TWS.Common.Constants;
using WebConstants = Runbow.TWS.Web.Common.Constants;
using SystemReflection = System.Reflection;
using Runbow.TWS.Web.ESPService;

namespace Runbow.TWS.Web.Areas.POD.Controllers
{
    public class TianjinController : BaseController
    {
        [HttpGet]
        public ActionResult QueryPod()
        {
            QueryTianjinPodViewModel vm = new QueryTianjinPodViewModel();
            vm.ActualDeliverlyDate = DateTime.Now.AddMonths(-1);
            vm.EndActualDeliverlyDate = DateTime.Now;
            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            return View(vm);
        }

        [HttpPost]
        public ActionResult QueryPod(QueryTianjinPodViewModel vm)
        {
            PodSearchCondition condition = new PodSearchCondition();
            condition.CustomerOrderNumber = vm.CustomerOrderNumbers;
            condition.ActualDeliveryDate = vm.ActualDeliverlyDate;
            condition.EndActualDeliveryDate = vm.EndActualDeliverlyDate;
            condition.CustomerID = vm.CustomerID;
            condition.ShipperID = vm.ShipperID;
            condition.DateTime14 = vm.ExpertArrivalDate;
            condition.EndDateTime14 = vm.EndExpertArrivalDate;

            if (vm.InOrOut == "0")
            {
                condition.EndCityID = 3;
            }
            else
            {
                condition.StartCityID = 3;
            }

            var response = new PodService().QueryPodWithNoPaging(new QueryPodRequest() { SearchCondition = condition, ProjectID = base.UserInfo.ProjectID });
            if (response.IsSuccess)
            {
                vm.PodCollection = response.Result.OrderByDescending(p => p.ActualDeliveryDate);
            }

            vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            
            return View(vm);
        }

    }
}
