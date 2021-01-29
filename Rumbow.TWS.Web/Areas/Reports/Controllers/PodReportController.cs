using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Common;
using Runbow.TWS.Web.Areas.Reports.Models;
using Runbow.TWS.Web.Common;
using WebConstants = Runbow.TWS.Web.Common.Constants;
using Runbow.TWS.MessageContracts.Reports;
using System.Text;
using System.Web.Script.Serialization;

namespace Runbow.TWS.Web.Areas.Reports.Controllers
{
    public class PodReportController : BaseController
    {
        private static int[] MonthDay = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        [HttpGet]
        public ActionResult ShowPodAll(long? customerID)
        {
            ShowPodAllViewModel vm = new ShowPodAllViewModel();
            vm.SearchCondition = new PodSearchCondition();
            vm.PageIndex = 0;
            vm.PageCount = 0;
            vm.ProjectRoleID = base.UserInfo.ProjectRoleID;
            vm.IsInnerUser = base.UserInfo.UserType == 2;
            if (base.UserInfo.UserType == 0)
            {
                vm.SearchCondition.CustomerID = base.UserInfo.CustomerOrShipperID;
            }
            else if (base.UserInfo.UserType == 1)
            {
                vm.SearchCondition.ShipperID = base.UserInfo.CustomerOrShipperID;
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

            this.GenQueryPodViewModel(vm);

            return View(vm);
        }

        [HttpPost]
        public ActionResult ShowPodAll(ShowPodAllViewModel vm, int? PageIndex)
        {
            vm.SearchCondition.UserType = base.UserInfo.UserType;
            if (vm.SearchCondition.UserType == 2)
            {
                vm.SearchCondition.CustomerIDs = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID).Select(c => c.CustomerID);
            }
            else if (vm.SearchCondition.UserType == 0)
            {
                vm.SearchCondition.Types = new List<int>();
                vm.SearchCondition.Types.Add(0);
                vm.SearchCondition.Types.Add(2);
            }
            else
            {
                vm.SearchCondition.Types = new List<int>();
                vm.SearchCondition.Types.Add(1);
                vm.SearchCondition.Types.Add(2);
            }

            var result = new PodReportService().QueryPodAndInvoiceAndReceiveOrPayOrders(new QueryPodAndInvoiceAndReceiveOrPayOrdersRequest()
            {
                PageSize = 10,
                PageIndex = PageIndex ?? 0,
                SearchCondition = vm.SearchCondition,
                ProjectID = base.UserInfo.ProjectID
            }).Result;
            vm.PodInvoiceReceiveOrPayOrders = result.PodInvoiceReceiveOrPayOrders;
            vm.PageIndex = result.PageIndex;
            vm.PageCount = result.PageCount;
            this.GenQueryPodViewModel(vm);
            return View(vm);
        }

        [HttpPost]
        public ActionResult ShowEveryCustomerYearPodCount(string year)
        {
            //var data = new[] { new { name = "Hilti", data = 1000,ID=2 }, new { name = "Adidas", data = 5000 , ID=1}, new { name = "Nike", data = 2000,ID=8 }, new { name = "Total", data = 500,ID=4 },new {name = "Oriflame", data = 0,ID=5 },new {name = "MaryKay", data = 0,ID=6 } };
            var customers = ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID);
            var response = new PodReportService().QueryCustomerYearPodCount(new QueryCustomerYearPodCountRequest() { Year = year });
            if (response.IsSuccess)
            {
                List<object> list = new List<object>();
                foreach (var c in customers)
                {
                    var customerData = response.Result.FirstOrDefault(r => r.CustomerID == c.CustomerID);
                    if (customerData != null)
                    {
                        list.Add(new { name = customerData.CustomerName, data = customerData.PodCount, ID = customerData.CustomerID });
                    }
                    else
                    {
                        list.Add(new { name = c.Name, data = 0, ID = c.CustomerID });
                    }
                }

                return Json(list);
            }

            throw response.Exception;
        }

        public ActionResult GetCustomerMonthlyAndDailyPodCount(string year, long CustomerID)
        {
            var customerName = ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID).FirstOrDefault(c => c.CustomerID == CustomerID).Name;
            var response = new PodReportService().QueryCustomerMonthlyAndDailyPodCount(new QueryCustomerMonthlyAndDailyPodCountRequest() { Year = year, CustomerID = CustomerID });
            if (response.IsSuccess)
            {
                List<object> brandsData = new List<object>();
                List<object> drilldownSeries = new List<object>();

                for (int i = 1; i <= 12; i++)
                {
                    ArrayList dailyPodList = new ArrayList();
                    var key = i.ToString() + "月";
                    var monthlyPods = response.Result.Where(p => p.Month == i);
                    if (monthlyPods != null)
                    {
                        brandsData.Add(new { name = key, y = monthlyPods.Sum(p => p.PodCount), drilldown = key });
                        for (int j = 1; j <= MonthDay[i - 1]; j++)
                        {
                            var dailyPods = response.Result.FirstOrDefault(p => p.Month == i && p.Day == j);
                            if (dailyPods != null)
                            {
                                int[] temp = new int[] { j, dailyPods.PodCount };
                                dailyPodList.Add(temp);
                            }
                            else
                            {
                                int[] temp = new int[] { j, 0 };
                                dailyPodList.Add(temp);
                            }

                        }

                        drilldownSeries.Add(new { name = key, id = key, data = dailyPodList });
                    }
                    else
                    {
                        brandsData.Add(new { name = key, y = 0, drilldown = string.Empty });
                        drilldownSeries.Add(new { name = key, id = key, data = dailyPodList });
                    }
                }

                return Json(new { brandsData = brandsData, drilldownSeries = drilldownSeries });
            }

            throw response.Exception;
        }

        public ActionResult ShowEveryCustomerYearPodCount()
        {
            return View();
        }

        public ActionResult ShowCustomerPodCountByRegion()
        {
            ViewBag.Customers = ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.Name });

            return View();
        }

        public ActionResult ShowShipperPodCount()
        {
            return View();
        }

        public ActionResult IncomAndExpenses()
        {
            ViewBag.Customers = ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID).Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.Name });

            return View();
        }

        public ActionResult ShipperCost()
        {
            return View();
        }

        private IEnumerable<Region> GetLowerRegions(long regionID)
        {
            IEnumerable<Region> regions = new List<Region>();
            regions = regions.Union(ApplicationConfigHelper.GetRegions().Where(r => r.ID == regionID));
            var lowerRegions = ApplicationConfigHelper.GetRegions().Where(r => r.SupperID == regionID);
            if (lowerRegions != null && lowerRegions.Any())
            {
                lowerRegions.Each((i, r) =>
                {
                    regions = regions.Union(GetLowerRegions(r.ID));
                });
            }
            return regions;
        }

        [HttpPost]
        public ActionResult ShowCustomerPodCountByRegion(long CustomerID, string Year, int Month)
        {
            var response = new PodReportService().QueryCustomerPodCountByRegionAndTime(new QueryCustomerPodCountByRegionAndTimeRequest() { Year = Year, Month = Month, CustomerID = CustomerID });
            if (response.IsSuccess)
            {
                List<object> brandsData = new List<object>();
                List<object> drilldownSeries = new List<object>();
                IEnumerable<Region> provinceRegions = ApplicationConfigHelper.GetRegions().Where(r => r.Grade == 1);
                foreach (var provinceRegion in provinceRegions)
                {
                    IEnumerable<Region> lowerRegions = new List<Region>();
                    lowerRegions = this.GetLowerRegions(provinceRegion.ID);
                    var sum = (from c in response.Result
                               join re in lowerRegions
                               on c.RegionID equals re.ID
                               select c).Sum(k => k.PodCount);
                    brandsData.Add(new { name = provinceRegion.Name, y = sum, drilldown = provinceRegion.Name });
                    var nextLevelRegions = ApplicationConfigHelper.GetRegions().Where(r => r.SupperID == provinceRegion.ID);
                    ArrayList regionPodList = new ArrayList();
                    regionPodList.Add(new object[] { provinceRegion.Name, response.Result.Where(r => r.RegionID == provinceRegion.ID).Sum(r => r.PodCount) });
                    if (nextLevelRegions != null)
                    {

                        foreach (var nextLevelRegion in nextLevelRegions)
                        {
                            IEnumerable<Region> tempRegions = new List<Region>();
                            tempRegions = this.GetLowerRegions(nextLevelRegion.ID);
                            var tempSum = (from c in response.Result
                                           join re in tempRegions
                                           on c.RegionID equals re.ID
                                           select c).Sum(k => k.PodCount);
                            Object[] temp = new Object[] { nextLevelRegion.Name, tempSum };
                            regionPodList.Add(temp);
                        }
                    }
                    drilldownSeries.Add(new { name = provinceRegion.Name, id = provinceRegion.Name, data = regionPodList });
                }

                return Json(new { brandsData = brandsData, drilldownSeries = drilldownSeries });
            }

            throw response.Exception;
        }

        [HttpPost]
        public ActionResult ShowShipperPodCount(long ShipperID, string Year)
        {
            var response = new PodReportService().QueryCustomerPodYearMonthCountByTimeRange(new QueryCustomerPodYearMonthCountByTimeRangeRequest() { Year = Year, ShipperID = ShipperID });
            if (response.IsSuccess)
            {
                List<object> brandsData = new List<object>();
                List<object> drilldownSeries = new List<object>();
                foreach (var customer in ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID))
                {
                    var sum = response.Result.Where(c => c.CustomerID == customer.CustomerID).Sum(c => c.PodCount);
                    brandsData.Add(new { name = customer.Name, y = sum, drilldown = customer.Name });
                    ArrayList podList = new ArrayList();
                    for (int i = 1; i <= 12; i++)
                    {
                        var innerSum = response.Result.Where(c => c.CustomerID == customer.CustomerID && c.Month == i).Sum(c => c.PodCount);
                        podList.Add(new object[] { i.ToString() + "月", innerSum });
                    }
                    drilldownSeries.Add(new { name = customer.Name, id = customer.Name, data = podList });
                }
                return Json(new { brandsData = brandsData, drilldownSeries = drilldownSeries });
            }

            throw response.Exception;
        }

        private void GenQueryPodViewModel(ShowPodAllViewModel vm)
        {
            vm.Config = ((Projects)ApplicationConfigHelper.GetApplicationConfig()).ProjectCollection.First(p => p.Id == base.UserInfo.ProjectID.ToString()).ModuleCollection.First(m => m.Id == "M001").Tables.TableCollection.First(t => t.Name == "Pod");
            vm.ShipperTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.SHIPPERTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.PodStates = ApplicationConfigHelper.GetApplicationConfigs(WebConstants.PODSTATE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            if (base.UserInfo.UserType == 2)
            {
                vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
                                        .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            }
            else if (base.UserInfo.UserType == 0)
            {
                vm.Customers = new List<SelectListItem>() { new SelectListItem() { Value = base.UserInfo.CustomerOrShipperID.ToString(), Text = ApplicationConfigHelper.GetApplicationCustomers().First(c => c.ID == base.UserInfo.CustomerOrShipperID).Name } };
            }
            else
            {
                vm.Customers = Enumerable.Empty<SelectListItem>();
            }

            vm.Shippers = ApplicationConfigHelper.GetProjectShippers(base.UserInfo.ProjectID)
                                .Select(c => new SelectListItem() { Value = c.ShipperID.ToString(), Text = c.Name });
            vm.PODTypes = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.PODTYPE)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
            vm.TtlOrTpls = ApplicationConfigHelper.GetSystemConfigs(base.UserInfo.ProjectID, WebConstants.TTLORTPL)
                                .Select(c => new SelectListItem() { Value = c.ID.ToString(), Text = c.Name });
        }

        [HttpPost]
        public ActionResult IncomAndExpenses(long customerID, string year)
        {
            var response = new PodReportService().QueryIncomAndExpenses(new QueryIncomAndExpensesRequest() { Year = year, CustomerID = customerID });
            if (response.IsSuccess)
            {
                ArrayList incomes = new ArrayList();
                ArrayList expenses = new ArrayList();
                IList<object> columns = new List<object>();
                IList<object> pies = new List<object>();
                for (int i = 1; i <= 12; i++)
                {
                    var income = response.Result.Where(c => c.Type == 0 && c.Month == i).Sum(c => c.Fee);
                    var expensesInner = response.Result.Where(c => c.Type == 1 && c.Month == i).Sum(c => c.Fee);
                    incomes.Add(income);
                    expenses.Add(expensesInner);
                }

                var incomeTotal = response.Result.Where(c => c.Type == 0).Sum(c => c.Fee);
                var expensesTotal = response.Result.Where(c => c.Type == 1).Sum(c => c.Fee);

                return Json(new { incomes = incomes, expenses = expenses, incomeTotal = incomeTotal, expensesTotal = expensesTotal });
            }

            throw response.Exception;
        }

        [HttpPost]
        public ActionResult ShipperCost(long ShipperID, string Year)
        {
            var response = new PodReportService().QueryShipperCost(new QueryShipperCostRequest() { Year = Year, ShipperID = ShipperID });
            if (response.IsSuccess)
            {
                List<object> brandsData = new List<object>();
                List<object> drilldownSeries = new List<object>();
                foreach (var customer in ApplicationConfigHelper.GetProjectCustomers(base.UserInfo.ProjectID))
                {
                    var sum = response.Result.Where(c => c.RelatedCustomerID == customer.CustomerID).Sum(c => c.Fee);
                    brandsData.Add(new { name = customer.Name, y = sum, drilldown = customer.Name });
                    ArrayList podList = new ArrayList();
                    for (int i = 1; i <= 12; i++)
                    {
                        var innerSum = response.Result.Where(c => c.RelatedCustomerID == customer.CustomerID && c.Month == i).Sum(c => c.Fee);
                        podList.Add(new object[] { i.ToString() + "月", innerSum });
                    }
                    drilldownSeries.Add(new { name = customer.Name, id = customer.Name, data = podList });
                }

                return Json(new { brandsData = brandsData, drilldownSeries = drilldownSeries });
            }

            throw response.Exception;
        }
        [HttpGet]
        public ActionResult TransOrderStatus(TransOrderModel vm)
        {
            //vm.Customers = ApplicationConfigHelper.GetProjectUserCustomers(base.UserInfo.ProjectID, base.UserInfo.ID)
            //                           .Select(c => new SelectListItem() { Value = c.CustomerID.ToString(), Text = c.CustomerName });
            //TransOrderModel vm = new TransOrderModel();
            //vm.SearchCondition = new PodSearchCondition();
            //vm.PageIndex = 0;
            //vm.PageCount = 0;
            TransOrderRequest tor = new TransOrderRequest();
            tor.StartTime =DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd");
            tor.EndTime = DateTime.Now.AddDays(+1).ToString("yyyy-MM-dd");
            vm.transOrderRequest = tor;
            //vm.transOrderRequest.StartTime = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");

            //vm.transOrderRequest.EndTime = DateTime.Now.AddDays(0).ToString("yyyy-MM-dd");
            //this.GenQueryPodViewModel(vm);

            return View(vm);
        }
        [HttpPost]
        public ActionResult TransOrderStatus(string id, string Customers, string ShipperName, string startCityTreeName, string endCityTreeName, string startTime, string endTime, string Time)// string Consignee,
        {
            TransOrderModel Tq = new TransOrderModel();
            var response = new PodReportService().QueryTransOrderRange(new TransOrderRequest() { ID = id, Customers = Customers, StartTime = startTime, startCityTreeName=startCityTreeName, endCityTreeName=endCityTreeName, EndTime = endTime, ShipperName = ShipperName, Time = Time });//Consignee = Consignee,
            var responses = new PodReportService().QueryTransOrderRanges(new TransOrderRequest() { ID = id, Customers = Customers, StartTime = startTime, startCityTreeName = startCityTreeName, endCityTreeName = endCityTreeName, EndTime = endTime, ShipperName = ShipperName, Time = Time, PageIndex = 0, PageSize = 100 });//Consignee = Consignee,
            if (response.IsSuccess)
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                //var transOrder = from q in responses.Result.transOrder
                //                 group q by q.CustomerOrderNumber into g
                //                 let ids = g.Select(b => b.Str2.ToString())
                //                 select new
                //                 {
                //                     //g.Key
                //                     ID=g.Max(a=>a.ID),
                //                     ActualDeliveryDate = g.Max(a => a.ActualDeliveryDate),
                //                     BoxNumber = g.Max(a => a.BoxNumber),
                //                     CustomerName = g.Max(a => a.CustomerName),
                //                     CustomerOrderNumber = g.Max(a => a.CustomerOrderNumber),
                //                     ShipperName = g.Max(a => a.ShipperName),
                //                     //Str2 = g.Max(a => a.Str2),
                //                     Str3 = g.Max(a => a.Str3),
                //                     Str2 = string.Join("|", ids),
                //                     GoodsNumber=g.Max(a=>a.GoodsNumber),
                //                     Weight=g.Max(a=>a.Weight),
                //                     Volume=g.Max(a=>a.Volume)

                //                 };
                string js = jsonSerializer.Serialize(responses.Result.transOrder);//transOrder
                StringBuilder sb0 = new StringBuilder();            
                StringBuilder sb1 = new StringBuilder();            
                StringBuilder sb2 = new StringBuilder();            
                StringBuilder sb3 = new StringBuilder();            
                StringBuilder sb4 = new StringBuilder();            
                StringBuilder sbDate = new StringBuilder();
                string[] sbDates = new string[0]; //机型逗号拆分  
                if (string.IsNullOrEmpty(Time) &&  Customers=="0")
                {
                    //var allClassification = from q in response.Result.transOrder
                    //                        group q by q.Str2 into r
                    //                        select new
                    //                        {
                    //                            Str2=r.Key,
                    //                            CustomerOrderNumberNum = r.Count()
                    //                        };
                    int pie1 = response.Result.transOrder.Where(q => q.Str2 == "订单下达").Count();
                    int pie2 = response.Result.transOrder.Where(q => q.Str2 == "干线发车").Count();
                    int pie3 = response.Result.transOrder.Where(q => q.Str2 == "到达终端").Count();
                    int pie4 = response.Result.transOrder.Where(q => q.Str2 == "运单签收").Count();  
                    //foreach (var item in allClassification)
                    //{

                    //    switch (item.Str2)
                    //    {
                    //        case "订单下达":
                    //            pie1 =item.CustomerOrderNumberNum;
                    //            break;
                    //        case "干线发车":
                    //            pie2 = item.CustomerOrderNumberNum;
                    //            break;
                    //        case "到达终端":
                    //            pie3 =item.CustomerOrderNumberNum;
                    //            break;
                    //        case "运单签收":
                    //            pie4 = item.CustomerOrderNumberNum;
                    //            break;       
                    //    }
                    //}


                    var data = from q in response.Result.transOrder
                               group q by new { q.ActualDeliveryDate, q.Str2 } into r
                               select new
                               {
                                   CustomerOrderNumberNum = r.Count(),
                                   //CustomerName = r.Max(a => a.CustomerName),
                                   St2=r.Key.Str2,
                                   ActualDeliveryDate = r.Key.ActualDeliveryDate
                               };
                    //var date = response.Result.transOrder.Select(q => q.ActualDeliveryDate).Distinct();

                    int DateSum = Convert.ToDateTime(endTime).Subtract(Convert.ToDateTime(startTime)).Days;
                  
                    if (DateSum > 0)
                    {
                        sb0.Append("[");
                        sb1.Append("[");
                        sb2.Append("[");
                        sb3.Append("[");
                        sb4.Append("[");

                        List<string> ktls = sbDates.ToList();
                        //foreach (var results in date)
                        //{
                        for (int i = 0; i <= DateSum; i++)
                        {
                            string results = Convert.ToDateTime(startTime).AddDays(i).ToString("yyyy-MM-dd");
                            ktls.Add(results);
                            sb0.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results).Count()).Append(",");
                            sb1.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results && q.Str2 == "订单下达").Count()).Append(",");
                            sb2.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results && q.Str2 == "干线发车").Count()).Append(",");
                            sb3.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results && q.Str2 == "到达终端").Count()).Append(",");
                            sb4.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results && q.Str2 == "运单签收").Count()).Append(",");
                            //sb0.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results).Count()).Append(",");
                            //sb1.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results && q.CustomerName == "Adidas").Count()).Append(",");
                            //sb2.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results && q.CustomerName == "Nike").Count()).Append(",");
                            //sb3.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results && q.CustomerName == "Hilti").Count()).Append(",");
                            //sb4.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results && q.CustomerName == "AKZOTB").Count()).Append(",");
                        }
                        //if (DateSum== 0)
                        //{
                        //    sb0.Append(",");
                        //    sb1.Append(",");
                        //    sb2.Append(",");
                        //    sb3.Append(",");
                        //    sb4.Append(",");
                        //}
                        //sbDate.Remove(sbDate.Length - 1, 1);
                        sb0.Remove(sb0.Length - 1, 1).Append("]");
                        sb1.Remove(sb1.Length - 1, 1).Append("]");
                        sb2.Remove(sb2.Length - 1, 1).Append("]");
                        sb3.Remove(sb3.Length - 1, 1).Append("]");
                        sb4.Remove(sb4.Length - 1, 1).Append("]");
                        sbDates = ktls.ToArray();
                    }

                    return Json(new { Alldata = js, sbDate = sbDates, brandsDataTotal = sb0.ToString(), brandsDataAdidas = sb1.ToString(), brandsDataNIKE = sb2.ToString(), brandsDataHilti = sb3.ToString(), brandsDataAKZO = sb4.ToString(), pie1 = pie1, pie2 = pie2, pie3 = pie3, pie4 = pie4, PageCount=responses.Result.PageCount }, JsonRequestBehavior.AllowGet);
                }
                else if (Time != null && Customers=="0")
                {
                    var data = from q in response.Result.transOrder
                               group q by new { q.CustomerName, q.Str2 } into r
                               select new
                               {
                                   CustomerOrderNumberNum = r.Count(),
                                   CustomerName = r.Max(a => a.CustomerName),
                                   Str2 = r.Key.Str2
                               };
                    var CustomerSum = response.Result.transOrder.Select(q => q.CustomerName).Distinct();
                    if (CustomerSum.Count() > 0)
                    {
                        sb0.Append("[");
                        sb1.Append("[");
                        sb2.Append("[");
                        sb3.Append("[");
                        sb4.Append("[");

                        List<string> ktls = sbDates.ToList();
                        foreach (var results in CustomerSum)
                        {
                            ktls.Add(results);
                            sb0.Append(response.Result.transOrder.Where(q => q.CustomerName == results).Count()).Append(",");
                            sb1.Append(response.Result.transOrder.Where(q => q.Str2 == "订单下达" && q.CustomerName == results).Count()).Append(",");
                            sb2.Append(response.Result.transOrder.Where(q => q.Str2 == "干线发车" && q.CustomerName == results).Count()).Append(",");
                            sb3.Append(response.Result.transOrder.Where(q => q.Str2 == "到达终端" && q.CustomerName == results).Count()).Append(",");
                            sb4.Append(response.Result.transOrder.Where(q => q.Str2 == "运单签收" && q.CustomerName == results).Count()).Append(",");
                            // sbDate.Append("'" +  + "'").Append(",");
                            // sbDates.
                            //sb0.Append(response.Result.transOrder.Where(q =>q.CustomerName == results).Count()).Append(",");
                            //sb1.Append(response.Result.transOrder.Where(q =>q.CustomerName==results &&q.Str2 == "订单到达").Count()).Append(",");
                            //sb2.Append(response.Result.transOrder.Where(q =>q.CustomerName==results &&q.Str2 == "干线运输").Count()).Append(",");
                            //sb3.Append(response.Result.transOrder.Where(q =>q.CustomerName==results &&q.Str2 == "到达终端").Count()).Append(",");
                            //sb4.Append(response.Result.transOrder.Where(q =>q.CustomerName==results &&q.Str2 == "运单签收").Count()).Append(",");
                        }
                        //sbDate.Remove(sbDate.Length - 1, 1);
                        //if (CustomerSum.Count() == 0)
                        //{
                        //    sb0.Append(",");
                        //    sb1.Append(",");
                        //    sb2.Append(",");
                        //    sb3.Append(",");
                        //    sb4.Append(",");
                        //}
                        sb0.Remove(sb0.Length - 1, 1).Append("]");
                        sb1.Remove(sb1.Length - 1, 1).Append("]");
                        sb2.Remove(sb2.Length - 1, 1).Append("]");
                        sb3.Remove(sb3.Length - 1, 1).Append("]");
                        sb4.Remove(sb4.Length - 1, 1).Append("]");
                        sbDates = ktls.ToArray();
                    }
                    return Json(new { Alldata = js, sbDate = sbDates, brandsDataTotal = sb0.ToString(), brandsDataAdidas = sb1.ToString(), brandsDataNIKE = sb2.ToString(), brandsDataHilti = sb3.ToString(), brandsDataAKZO = sb4.ToString(), PageCount = responses.Result.PageCount }, JsonRequestBehavior.AllowGet);
                }
                else if (Customers!="0")
                {
                    var data = from q in response.Result.transOrder
                               group q by new { q.ActualDeliveryDate, q.Str2 } into r
                               select new
                               {
                                   CustomerOrderNumberNum = r.Count(),
                                   Str2 = r.Key.Str2
                               };
                    
                    var CustomerSum = response.Result.transOrder.Select(q => q.ActualDeliveryDate).Distinct();
                    if (CustomerSum.Count() > 0)
                    {
                        sb0.Append("[");
                        sb1.Append("[");
                        sb2.Append("[");
                        sb3.Append("[");
                        sb4.Append("[");
                        List<string> ktls = sbDates.ToList();
                        foreach (var results in CustomerSum)
                        {
                            ktls.Add(results);
                            sb0.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results).Count()).Append(",");
                            sb1.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results && q.Str2 == "订单下达").Count()).Append(",");
                            sb2.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results && q.Str2 == "干线发车").Count()).Append(",");
                            sb3.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results && q.Str2 == "到达终端").Count()).Append(",");
                            sb4.Append(response.Result.transOrder.Where(q => q.ActualDeliveryDate == results && q.Str2 == "运单签收").Count()).Append(",");
                        }
                        //if (CustomerSum.Count() == 0)
                        //{
                        //    sb0.Append(",");
                        //    sb1.Append(",");
                        //    sb2.Append(",");
                        //    sb3.Append(",");
                        //    sb4.Append(",");
                        //}
                        sb0.Remove(sb0.Length - 1, 1).Append("]");
                        sb1.Remove(sb1.Length - 1, 1).Append("]");
                        sb2.Remove(sb2.Length - 1, 1).Append("]");
                        sb3.Remove(sb3.Length - 1, 1).Append("]");
                        sb4.Remove(sb4.Length - 1, 1).Append("]");
                        sbDates = ktls.ToArray();
                    }
                    return Json(new { Alldata = js, sbDate = sbDates, brandsDataTotal = sb0.ToString(), brandsDataAdidas = sb1.ToString(), brandsDataNIKE = sb2.ToString(), brandsDataHilti = sb3.ToString(), brandsDataAKZO = sb4.ToString(), PageCount = responses.Result.PageCount }, JsonRequestBehavior.AllowGet);
                }
            }
            return View("");
        }


        public string ALLTransOrderStatusPaging(string id, string Customers, string ShipperName, string startCityTreeName, string endCityTreeName, string startTime, string endTime,string Time, int? PageIndex)//,
        {
            var responses = new PodReportService().QueryTransOrderRanges(new TransOrderRequest() 
            { 
                ID = id,
                Customers = Customers,
                StartTime = startTime,
                startCityTreeName = startCityTreeName, 
                endCityTreeName = endCityTreeName,
                EndTime = endTime,
                ShipperName = ShipperName,
               Time = Time,
                PageIndex = PageIndex??0,
                PageSize =100
            });
            string js="";
            if (responses.IsSuccess)
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                js = jsonSerializer.Serialize(responses.Result.transOrder);//transOrder
            } 
            return js;
        }
        public ActionResult NotificationCenter()
        {
            return View();
        }
        public ActionResult MonitoringReport()
        {
            return View();
        }
    }
}
