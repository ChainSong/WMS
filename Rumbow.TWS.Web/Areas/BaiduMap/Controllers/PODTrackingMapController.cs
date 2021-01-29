using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Biz.BaiduMap;
using Runbow.TWS.MessageContracts.BaiduMap;
using Runbow.TWS.Web.Areas.BaiduMap.Models;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Common;
namespace Runbow.TWS.Web.Areas.BaiduMap.Controllers
{
    public class PODTrackingMapController : Controller
    {
        //
        // GET: /Map/PODTrackingMap/

        public ActionResult Index()
        {

            return View();
        }
        public JsonResult PODGeographicalPosition(string Customerordernumber, string EndCustomer, string Destination, DateTime? start_DeliveryDate, DateTime? end_DeliveryDate, DateTime? start_PlanArrive, DateTime? end_PlanArrive)
        {
            PODTrackingMap mv = new PODTrackingMap();

            //var CustomerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
            PODTrackingMapService Service = new PODTrackingMapService();
            var response = Service.GPSALLGlobalTracking(new PODTrackingMapRequest
            {
                Customerordernumber = Customerordernumber.Trim(),
                EndCustomer = EndCustomer.Trim(),
                Destination = Destination.Trim(),
                start_DeliveryDate = start_DeliveryDate,
                end_DeliveryDate = end_DeliveryDate,
                start_PlanArrive = start_PlanArrive,
                end_PlanArrive = end_PlanArrive,
                Customer = 8
            });
            if (response.IsSuccess)
            {
                if (response.Result.Response.Count() == 0)
                {
                    return Json(new { code = 0 }, JsonRequestBehavior.AllowGet);
                }
                #region
                //var CarInfo = from q in response.Result.Response
                //              select new
                //              {
                //                  q.Latitude,
                //                  q.Longitude,
                //                  q.CarNo,
                //                  q.info
                //              };
                //var PODinfo = from q in response.Result.PODTrackingMap
                //           select new
                //           {
                //               q.CustomerOrderNumber,
                //               q.CarNo
                //           };

                //var Data = from q in groupedPodsResponse.Result.GroupedPods
                //           group q by new { q.ActualDeliveryDate, q.PODTypeName } into g
                //           let ids = g.Select(b => b.PodIDs.ToString()).ToArray()
                //           select new
                //           {
                //               g.Key,
                //               ActualDeliveryDate = g.Max(a => a.ActualDeliveryDate),
                //               ShipperTypeName = g.Max(a => a.ShipperTypeName),
                //               BoxNumber = g.Sum(a => a.BoxNumber),
                //               Volume = g.Sum(a => a.Volume),
                //               PodIDs = string.Join("|", ids),

                //           };
                #endregion
                response.Result.Response.Each((a, b) =>
                {
                    #region
                    //var podInfo = from q in response.Result.PODTrackingMap
                    //              where q.CarNo == b.CarNo
                    //              group q by new { q.CarNo, q.CustomerOrderNumber } into g
                    //              //let ids = g.Select(c => c.CustomerOrderNumber).ToString()
                    //              select new
                    //              {
                    //                  g.Key.CustomerOrderNumber
                    //              };
                    #endregion
                    StringBuilder sb = new StringBuilder();
                    if (string.IsNullOrEmpty(b.Hub))
                    {
                        sb.Append("<div>目的城市：" + b.EndCityName + "</br>发车时间： " + b.ActualDepartureTime + "</br>车牌号：" + b.CarNo + "</br>当前位置：" + b.GeographicalPosition + "</div></br>");  //</br>订单数量：" + b.Num + "</br>
                    }
                    else
                    {
                        response.Result.Response.Where(c => !string.IsNullOrEmpty(c.Hub) && c.Hub == b.Hub).GroupBy(d => new { d.Hub }).Select(e => new { e.Key.Hub, Num = e.Count() }).Each((q, w) =>
                        {
                            sb.Append("<div>运单数量：" + b.Num + "</br>位置： " + w.Hub);
                        });
                    }
                    b.info = sb.ToString();
                    #region 
                    //"<div>运单数量：" + b.Num + "</br>车牌号：" + b.CarNo + "</br>司机：" + b.DriverName + "</br>手机：" + b.DriverPhone + " </br>位置：" + b.GeographicalPosition + "</div></br>");
                    //href='/BaiduMap/PODTrackingMap/PODGeographicalPosition?type=Car'
                    //sb.Append("<div>运单数量：" + b.Num + "条</br>车牌号：<a data-car='" + b.CarNo + "'data-type='Car' onclick='PartialView(this)' href='javascript:void(0);'>" + b.CarNo + "</a></br>司机：" + b.DriverName + "</br>手机：" + b.DriverPhone + "</div></br>位置：" + b.GeographicalPosition + "</div></br>");
                    //<a id="listSystemNumber" href="/POD/POD/ViewPodAll/842185?showEditRelated=True" data-id="842185">Runbow20160430093027020059</a>
                    #endregion
                });
                #region
                //response.Result.Response.Where(c => !string.IsNullOrEmpty(c.Hub)).GroupBy(d => new { d.Hub }).Select(e => new { e.Key.Hub, Latitude = e.Max(a => a.Latitude), Longitude = e.Max(a => a.Longitude), Num = e.Count(), info="" }).Each((a, b) =>
                //{
                //    StringBuilder sb = new StringBuilder();
                //    sb.Append("<div>运单数量：" + b.Num+"</br>位置： " + b.Hub);
                //    b.info = sb.ToString();
                //});
                #endregion
                var data = from q in response.Result.Response
                           //where string.IsNullOrEmpty(q.Hub)
                           //group q by new {q.PODType, q.Hub } into r
                           select new
                           {
                               lat = q.Latitude,
                               lng = q.Longitude,
                               CarNo = q.CarNo,
                               Info = q.info,
                               PODType = q.PODType  ,
                               Code = q.Code
                           };
                #region
                //var dataU = from q in response.Result.Response
                //            where !string.IsNullOrEmpty(q.Hub)
                //            group q by new { q.Hub } into r
                //            select new
                //            {
                //                Num = r.Count(),
                //                Hub = r.Key.Hub,
                //                lat = r.Max(a => a.Latitude),
                //                lng = r.Max(a => a.Longitude),
                //            };
                #endregion
                var CardataTotal = from q in response.Result.Response
                                   //where string.IsNullOrEmpty(q.Hub)
                                   group q by new { q.PODType  } into r//, q.Hub 
                                   select new
                                   {
                                       Num = r.Count(),
                                       PODType = r.Key.PODType
                                   };
                #region
                //var adad = from q in response.Result.Response
                //           //where string.IsNullOrEmpty(q.Hub)
                //           group q by new { q.Hub } into r//, q.Hub 
                //           select new
                //           {
                //               Num = r.Count()
                //           }.Num; 
                //var WDdataTotal = from q in response.Result.Response
                //                  where !string.IsNullOrEmpty(q.Hub)
                //                  group q by new { q.Hub } into r
                //                  select new
                //                  {
                //                      Num = r.Count(),
                //                      lat=r.Max(a=>a.Latitude),
                //                      lng = r.Max(a => a.Longitude),
                //                      Hub=r.Key.Hub
                //                  };
                #endregion
                return Json(new { code = 1, info = data, dataTotal = CardataTotal }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { code = 0 }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PODtrajectory(string type, string CarNo)
        {
            //PODTrackingMap mv = new PODTrackingMap();
            ViewBag.Type = type;
            ViewBag.CarNo = CarNo;
            return View();
        }
        public JsonResult CarInfoPOD(string CarNo, string Type)
        {//PODGeographicalPosition
            PODTrackingMapService Service = new PODTrackingMapService();
            if (string.IsNullOrEmpty(CarNo))
            {
                return Json(new { Code = 0 }, JsonRequestBehavior.AllowGet);
            }

            var response = Service.GetCarInfoPOD(new PODTrackingMapRequest
            {
                CarNo = CarNo,
                Type = Type
                //Customer = 8
            });
            if (response.IsSuccess)
            {
                return Json(new { Code = 1, PODTrackingMap = response.Result.PODTrackingMap }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Code = 0 }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PartialPODView(string Type, string ID)
        {
            PODTrackingMap mv = new PODTrackingMap();

            //var CustomerIDs = ApplicationConfigHelper.GetProjectUserCustomers(1, base.UserInfo.ID).Select(c => c.CustomerID);
            PODTrackingMapService Service = new PODTrackingMapService();
            var response = Service.PartialPODView(new PODTrackingMapRequest
            {
                Type = Type,
                ID = ID
            });
            if (response.IsSuccess)
            {
                var data = response.Result.Response.Select(q => new
                {
                    q.CustomerName,
                    q.DriverName,
                    q.DriverPhone,
                    q.GeographicalPosition,
                    q.CustomerOrderNumber,
                    q.CarNo,
                    q.Latitude,
                    q.Longitude,
                    q.PODType,
                    Times = q.Times.ToString()
                });
                return Json(new { Code = 1, PODTrackingMap = data }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Code = 1 }, JsonRequestBehavior.AllowGet);
        }

    }
}
