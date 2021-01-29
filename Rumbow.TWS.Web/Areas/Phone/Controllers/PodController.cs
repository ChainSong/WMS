using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Areas.Phone.Models;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.Phone.Controllers
{
    public class PodController : Controller
    {

        public ActionResult QueryPod(string Id, string Type)
        {
            WeiChartUserConfigMode vm = new WeiChartUserConfigMode();
            GetQueryPodService service = new GetQueryPodService();
            vm.PageIndex = 0;
            vm.PageSize = 0;
            var response = service.WeiQueryPodService(new WinQueryPodRequest() { WeiQueryPods = null, Id=Id,  Type = Type, PageSize = 10, });
            if (response.IsSuccess)
            {
                vm.WeiQueryPod = response.Result.WeiQueryPods;
                vm.PageSize = response.Result.PageCount;
                vm.PageIndex = response.Result.PageIndex;
                vm.Id = Id;
                vm.Type = Type;
            }
            return View(vm);
        }
        [HttpPost]

        public string QueryPod(string Id, int index, string Type)
        {
            WeiChartUserConfigMode vm = new WeiChartUserConfigMode();
            GetQueryPodService service = new GetQueryPodService();
            vm.PageIndex = 0;
            vm.PageSize = 0;
            var response = service.WeiQueryPodService(new WinQueryPodRequest() { WeiQueryPods = null, Id = Id, Type = Type, PageSize = 10, PageIndex = index });
            if (response.IsSuccess)
            {
                vm.WeiQueryPod = response.Result.WeiQueryPods;
                vm.PageSize = response.Result.PageCount;
                vm.PageIndex = response.Result.PageIndex;
                vm.Type = Type;
            }
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            string js = jsonSerializer.Serialize(vm);
            return js;
        }
        public string PhoneScore(string Id, int ping, string ValFrom)
        {
            GetQueryPodService service = new GetQueryPodService();
            var response = service.PingFen(Id,ping,ValFrom);
            return "";
        }
    }
}