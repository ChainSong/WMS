using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Runbow.TWS.Biz;
using Runbow.TWS.MessageContracts.Reports;
using Runbow.TWS.Web.Areas.Reports.Models;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.Reports.Controllers
{
    public class PodBaiduReportController : Controller
    {
        //
        // GET: /Reports/PodBaiduReport/
        [HttpGet]
        public ActionResult ShowCaseHotMap()
        {
            ShowCaseHotMapModel sm = new ShowCaseHotMapModel();
            ShowCaseHotMapRequest sr = new ShowCaseHotMapRequest();
            sr.StartActualDeliveryDate = DateTime.Now.Year.ToString()+"-01-01";
            sr.EndActualDeliveryDate = DateTime.Now.ToString("yyyy-MM-dd");
            sm.Request = sr;
            return View(sm);
        }
        [HttpPost]
        public JsonResult ShowCaseHotMap(int Customer, string City, string StartActualDeliveryDate, string EndActualDeliveryDate, int Type)
        {

            var result = new PodReportService().ShowCaseHotMap(new ShowCaseHotMapRequest()
            {
                Customer = Customer,
                City = City,
                StartActualDeliveryDate = StartActualDeliveryDate,
                EndActualDeliveryDate = EndActualDeliveryDate,
                HotMapType = Type
            }).Result;

            if (string.IsNullOrEmpty(City))
            {
                if (result.showCaseHotMap.Count() > 0)
                {
                    var data = result.showCaseHotMap.Where(a => a.EndCitylat != null).Select(a => new { lng = Convert.ToDecimal(a.EndCitylng), lat = Convert.ToDecimal(a.EndCitylat), count = a.Count });
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string str = js.Serialize(data);
                    return Json(new { data = str, str = "" });
                }

            }
            else
            {
                if (result.showCaseHotMap.Count() > 0)
                {
                    var EndCityData = result.showCaseHotMap.Where(a => a.EndCitylat != null).Select(a => new { lng = Convert.ToDecimal(a.EndCitylng), lat = Convert.ToDecimal(a.EndCitylat) }).First();
                    var data = result.showCaseHotMap.Select(a => new { lat = Convert.ToDecimal(a.ShippingAddresslat), lng = Convert.ToDecimal(a.ShippingAddresslng), count = a.Count });
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    string str = js.Serialize(data);
                    string lat = js.Serialize(EndCityData.lat);
                    string lng = js.Serialize(EndCityData.lng);
                    return Json(new { lat = lat, lng = lng, data = str, str = "" });
                }
            }
            return Json(new { str = "无数据" });
        }

    }
}
