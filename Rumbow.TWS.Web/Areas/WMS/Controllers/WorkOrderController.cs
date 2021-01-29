using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Web.Areas.WMS.Models.WorkOrderManagement;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Controllers
{
    public class WorkOrderController : BaseController
    {
        //
        // GET: /WMS/WorkOrder/

        public ActionResult Index()
        {
            IndexViewModel vm = new IndexViewModel();
            vm.WorkOrderCondition = new Entity.WMS.WorkOrder.WorkOrderSearchCondition();
            vm.WorkOrderCondition.StartCreateTime = DateTime.Parse(DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd"));
            vm.WorkOrderCondition.EndCreateTime = DateTime.Now;
            return View(vm);
        }

        public ActionResult WorkOrderCreateOrEdit()
        {
            return View();
        }
    }
}
