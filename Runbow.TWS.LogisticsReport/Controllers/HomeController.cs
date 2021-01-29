using Runbow.TWS.Biz.MonitoringReport;
using Runbow.TWS.LogisticsReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.LogisticsReport.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult MonitoringReport()
        {
            var response = new MonitoringReportService().GetMonitoringReport();
            if (response.IsSuccess)
            {
                var OrderQuantity = from q in response.Result.OrderQuantity 
                                    select new
                                    {
                                        q.yi,
                                        q.wu,
                                        q.dayin,
                                        q.qi,
                                        q.ba,
                                        q.jiu,
                                        q.quxiao,
                                        q.zuotian
                                    };
                var TimelyRate = from q in response.Result.TimelyRate
                                 select new
                                 {
                                     q.num12,
                                     q.num24,
                                     q.num36,
                                     q.num48,
                                     q.num
                                 }; 
                return Json(new
                {
                    ErrorCode = 1,
                    OrderQuantity = OrderQuantity, 
                    TimelyRate = response.Result.TimelyRate,
                    WeeksOrders = response.Result.WeeksOrders, 
                    CarbonEmissions = response.Result.CarbonEmissions ,
                    TotalElectri = response.Result.GetElectric 
                });
            }
            return Json(new { ErrorCode = 0 });
        }
        public ActionResult AddElectricNumerical()
        {
            MonitoringReport mv = new MonitoringReport();
            string Date = DateTime.Now.ToString("yyyy-MM-dd");
            var response = new MonitoringReportService().GetElectricMeter(Date);
            if (response.IsSuccess)
            {
                mv.ElectricMeter = response.Result.GetElectric;
            }
            return View(mv);
        }
           [HttpPost]
        public JsonResult GetElectricNumerical(string Date)
        {
            MonitoringReport mv = new MonitoringReport();
            var response = new MonitoringReportService().GetElectricMeter(Date);
            if (response.IsSuccess)
            {
                //mv.ElectricMeter = response.Result.GetElectric;
                return Json(new { ErrorCode = 1, ElectricMeter = response.Result.GetElectric});
            }
            return Json(new { ErrorCode = 0 });
        }
        [HttpPost]
        public JsonResult AddElectricNumerical(string Date, string Office, string NFS, string Digital, string Inline)
        {
            MonitoringReport mv = new MonitoringReport();
            var response = new MonitoringReportService().AddElectricMeter(Date, Office, NFS, Digital, Inline);
            if (response.IsSuccess)
            {
                mv.ElectricMeter = response.Result.GetElectric;
                return Json(new { ErrorCode = 1 });
            }
            return Json(new { ErrorCode = 0 });
        }
    }
}
