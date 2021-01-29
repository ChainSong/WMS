using Runbow.TWS.Biz.POD;
using Runbow.TWS.MessageContracts.POD.AKZO;
using Runbow.TWS.Web.Areas.POD.Models;
using Runbow.TWS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebConstants = Runbow.TWS.Web.Common.Constants;
using UtilConstants = Runbow.TWS.Common.Constants;
using MyFile = System.IO.File;
using Runbow.TWS.Common;
using System.IO;
using System.Data;
using Runbow.TWS.Entity;
using Runbow.TWS.Biz;
using Runbow.TWS.MessageContracts;
using System.Text;
using Runbow.TWS.Entity.POD.Distribution;
using Runbow.TWS.MessageContracts.POD.Distribution;


namespace Runbow.TWS.Web.Areas.POD.Controllers
{
    public class DistributionController :  BaseController
    {
        private long _customerID = 27;

        //isForQuery=true 需要分页,明细不需要出现checkbox  isForQuery=false  用作分配车辆和结算运单，不需要分页，明细需要出现checkbox供用户选择运单
        [HttpGet]
        public ActionResult QueryOrOperatePod(bool? isForQuery)
        {
            
            QueryOrOperatePodViewModel vm = new QueryOrOperatePodViewModel();
            vm.SearchCondition = new PodDistribution();
            vm.PageIndex = 0;
            vm.PageCount = 0;
            vm.IsForQuery = isForQuery ?? true;
            vm.SearchCondition.ActualDeliveryDate = DateTime.Now.AddDays(-7);
            vm.SearchCondition.EndActualDeliveryDate = DateTime.Now;

            return View(vm);
        }

        [HttpPost]
        public ActionResult QueryOrOperatePod(QueryOrOperatePodViewModel vm, int? PageIndex, string SettledPodIDs)
        {
            vm.SearchCondition.FeeStr5 = ToStrCarModels(vm.SearchCondition.IntCarModels);
            vm.SearchCondition.IsPaging = vm.IsForQuery;
            if (vm.IsDaoChu)
            {
                vm.SearchCondition.podID = vm.SelectedIDs;
                vm.SearchCondition.IsExport = true;
                var podAll = new DistributionService().DbToExcels(new DistributionPodRequest()
                    {
                        SearchCondition = vm.SearchCondition,
                    }).Result.PodExcel;
                return this.ExportDfcjPodsToExcel(podAll);
            }
            var result = new DistributionService().QueryOrOperatePod(new DistributionPodRequest() { PageSize = UtilConstants.PAGESIZE, PageIndex = PageIndex ?? 0, SearchCondition = vm.SearchCondition, ProjectID = 1 }).Result;
            vm.PodCollection = result.PodCollections;
            vm.PageIndex = result.PageIndex;
            vm.PageCount = result.PageCount;
            if (!string.IsNullOrEmpty(vm.SearchCondition.CustomerOrderNumber))
            {
                IEnumerable<string> customerOrderNumbers = Enumerable.Empty<string>();
                if (vm.SearchCondition.CustomerOrderNumber.IndexOf("\n") > 0)
                {
                    customerOrderNumbers = vm.SearchCondition.CustomerOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }

                if (vm.SearchCondition.CustomerOrderNumber.IndexOf(',') > 0)
                {
                    customerOrderNumbers = vm.SearchCondition.CustomerOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }

                if (customerOrderNumbers == null || !customerOrderNumbers.Any())
                {
                    customerOrderNumbers = new string[] { vm.SearchCondition.CustomerOrderNumber };
                }

                var notContainsCustomerOrderNumbers = customerOrderNumbers.Where(c => !string.IsNullOrEmpty(c) &&
                        !vm.PodCollection.Any(p => string.Equals(p.CustomerOrderNumber, c.Trim(), StringComparison.OrdinalIgnoreCase))
                    );

                if (notContainsCustomerOrderNumbers != null && notContainsCustomerOrderNumbers.Any())
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("您输入的客户运单号:");
                    notContainsCustomerOrderNumbers.Each((i, c) => { sb.Append(c).Append(","); });
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("在本页结果中没有出现,系统中不存在此客户运单号或者此运单号在其他页或者请变更其他查询条件.");
                    vm.ReturnClientMessage = sb.ToString();
                }
            }

            return View(vm);
        }
        public ActionResult AuditPod(string PodIDs, string startBatchNumber, string aarriers, string carNumber, double? sumWeight, decimal? deliveryFeeFactory,
                             decimal? deliveryFeeJiatuo, decimal? unloadingCosts, decimal? startFee, decimal? fuelCosts, decimal? packagesFare, int? scores,
                             decimal? pointCharges, decimal? permitFees, decimal? otherCosts, decimal? total, decimal? amountTo, int? carModels, string remarks,string type)
        {
            var PodsID = PodIDs.Split(',').Select(id => id.ObjectToInt64());
            double Weight = 0;
            PodService service = new PodService();
            var podsResponse = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = PodsID });
            if (type=="1")
            {
                if (carModels==3)
                {
                    foreach (var podweight in podsResponse.Result)
                    {
                        Weight += podweight.Weight == null ? 0 : (double)podweight.Weight;
                    }
                    return this.Json(Weight);
                }
                else
                {
                    var podsResponses = podsResponse.Result.GroupBy(p => p.Str6);
                    return this.Json(podsResponses.Count()); 
                }
                
            }
            string carModel = string.Empty;
            decimal amountTos=0;
            decimal totals=0;
            int podCount = PodsID.Count();
            double realityWeight = (double)(sumWeight == null ? 0 : sumWeight);
            IList<SettlePodDistribution> settlePodDistributions = new List<SettlePodDistribution>();
            if (carModels==1||carModels==2)
            {
                if (carModels==1)
                {
                    carModel = "面包车";
                }
                else
                {
                    carModel = "4.2";
                }
                if (packagesFare!=null)
                {
                    amountTos = (decimal)(packagesFare);
                }
                else
                {
                    amountTos = (decimal)((deliveryFeeFactory == null ? 0 : deliveryFeeFactory) + (deliveryFeeJiatuo == null ? 0 : deliveryFeeJiatuo) + (unloadingCosts == null ? 0 : unloadingCosts) +
                   (startFee == null ? 0 : startFee) + (fuelCosts == null ? 0 : fuelCosts) + (packagesFare == null ? 0 : packagesFare) + (pointCharges == null ? 0 : pointCharges)
                   + (permitFees == null ? 0 : permitFees) + (otherCosts == null ? 0 : otherCosts));  
                }
                totals = amountTos / podCount;
                foreach (var pod in podsResponse.Result)
                {
                    SettlePodDistribution settlePodDistribution = new SettlePodDistribution()
                    {
                        FeePodID = pod.ID,
                        FeeSystemNumber = pod.SystemNumber,
                        FeeCustomOrerderNunber = pod.CustomerOrderNumber,
                        FeeCreator = pod.Creator,
                        FeeCreatorTime = DateTime.Now,
                        FeeStr1 = startBatchNumber,
                        FeeStr2 = aarriers,
                        FeeStr3 = carNumber,
                        FeeStr4 = pod.Weight.ToString(),
                        FeeStr5 = carModel,
                        FeeStr10 = remarks,
                        FeeDecimal1 = deliveryFeeFactory/podCount,
                        FeeDecimal2 = deliveryFeeJiatuo/podCount,
                        FeeDecimal3 = unloadingCosts/podCount,
                        FeeDecimal4 = fuelCosts/podCount,
                        FeeDecimal5 = packagesFare/podCount,
                        FeeDecimal6 = startFee/podCount,
                        FeeDecimal7 = pointCharges/podCount,
                        FeeDecimal8 = permitFees/podCount,
                        FeeDecimal9 = otherCosts/podCount,
                        FeeDecimal10 = totals,
                        FeeDecimal11 = amountTos,
                        FeeInt1 = scores

                    };
                    settlePodDistributions.Add(settlePodDistribution);
                }
            }
            else if(carModels==3)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var pod in podsResponse.Result)
                {
                    if (pod.Weight==null||pod.Weight==0)
                    {
                        sb.Append(pod.CustomerOrderNumber+",");
                    }
                }
                if (sb.Length>0)
                {
                    sb.Append("请设置这些运单的重量");
                    return this.Json(sb.ToString());
                }
                //if (string.IsNullOrEmpty(sumWeight))
                //{
                //    foreach (var podWeight in podsResponse.Result)
                //    {
                        
                //        Weight+= (double)podWeight.Weight;
                //    }
                //    sumWeight = Weight.ToString();
                //}
                carModel = "7.6";
                if (packagesFare!=null)
                {
                    amountTos = (decimal)packagesFare;
                }
                else
                {
                    if (sumWeight<10000)
                    {
                        sumWeight = 10000;
                    }
                    amountTos = (decimal)((deliveryFeeFactory == null ? 0 : deliveryFeeFactory) + (deliveryFeeJiatuo == null ? 0 : deliveryFeeJiatuo) + (unloadingCosts == null ? 0 : unloadingCosts) +
                   (startFee == null ? 0 : startFee) + (fuelCosts == null ? 0 : fuelCosts) + (packagesFare == null ? 0 : packagesFare) + (decimal)(sumWeight* 70 / 1000)
                   + (permitFees == null ? 0 : permitFees) + (otherCosts == null ? 0 : otherCosts));
                }
                foreach (var pod in podsResponse.Result)
                {
                    SettlePodDistribution settlePodDistribution = new SettlePodDistribution()
                    {
                        FeePodID = pod.ID,
                        FeeSystemNumber = pod.SystemNumber,
                        FeeCustomOrerderNunber = pod.CustomerOrderNumber,
                        FeeCreator = pod.Creator,
                        FeeCreatorTime = DateTime.Now,
                        FeeStr1 = startBatchNumber,
                        FeeStr2 = aarriers,
                        FeeStr3 = carNumber,
                        FeeStr4 = pod.Weight.ToString(),
                        FeeStr5 = carModel,
                        FeeStr10 = remarks,
                        FeeDecimal1 = deliveryFeeFactory * (decimal)(pod.Weight / realityWeight),
                        FeeDecimal2 = deliveryFeeJiatuo * (decimal)(pod.Weight / realityWeight),
                        FeeDecimal3 = unloadingCosts * (decimal)(pod.Weight / realityWeight),
                        FeeDecimal4 = fuelCosts * (decimal)(pod.Weight / realityWeight),
                        FeeDecimal5 = packagesFare == null ? (decimal)((pod.Weight / realityWeight) * (sumWeight * 0.07)) : packagesFare * (decimal)(pod.Weight / realityWeight),
                        FeeDecimal6 = startFee * (decimal)(pod.Weight / realityWeight),
                        FeeDecimal7 = pointCharges,
                        FeeDecimal8 = permitFees * (decimal)(pod.Weight / realityWeight),
                        FeeDecimal9 = otherCosts * (decimal)(pod.Weight / realityWeight),
                        FeeDecimal10 = amountTos * (decimal)(pod.Weight / realityWeight),
                        FeeDecimal11 = amountTos,
                        FeeInt1 = scores

                    };
                    settlePodDistributions.Add(settlePodDistribution);
                }
            }
           Response<bool> boll= new DistributionService().SettlePodsDistribution(new SettlePodsDistributionRequest() { SettledPodsDistribution = settlePodDistributions });
           return this.Json(boll.IsSuccess);
        }
      private string ToStrCarModels(int IntCarModel)
       {
           string carModel = string.Empty;
            if (IntCarModel==1)
            {
              carModel= "面包车";
            }
            else if (IntCarModel == 2)
            {
                carModel= "4.2";
            }
            else if (IntCarModel == 3)
            {
                carModel= "7.6";
            }
            return carModel;
       }

      public ActionResult deletePodFee(string podID)
      {
          Response<bool> boll = new DistributionService().selectPodFee(podID);
          return this.Json(boll.IsSuccess);
      }
      public ActionResult selectPodFee()
      {
          string startBatchNumber;
          string startBatchNumber1;
          string startBatchNumber2;
          int count = 0;
          startBatchNumber = new DistributionService().selectPodFee();
          if (string.IsNullOrEmpty(startBatchNumber))
          {
              startBatchNumber = DateTime.Now.YearMonthDay().ToString();
              startBatchNumber= startBatchNumber.Replace("-", ""); 
              startBatchNumber = startBatchNumber + "001"; 
          }
          else
          {
              startBatchNumber1 = startBatchNumber.Substring(startBatchNumber.Length - 3);
              startBatchNumber2=startBatchNumber.Substring(0,8);
              count = startBatchNumber1.ObjectToInt32();
              count = count + 1;
              if (count/10==0)
              {
                  startBatchNumber = startBatchNumber2 + "00" + count.ToString();
              }
              else if (count/100==0)
              {
                  startBatchNumber = startBatchNumber2 + "0" + count.ToString();
              }
              else
              {
                  startBatchNumber = startBatchNumber2 + count.ToString();
              }
          }
          return this.Json(startBatchNumber);

      }
      private ActionResult ExportDfcjPodsToExcel(IEnumerable<DbToExcel> Pods)
      {
          DataTable dtPod = new DataTable();
          dtPod.Columns.Add("电脑单号", typeof(string));
          dtPod.Columns.Add("发货清单号", typeof(string));
          dtPod.Columns.Add("客户账号", typeof(string));
          dtPod.Columns.Add("客户订单号", typeof(string));
          dtPod.Columns.Add("客户名称", typeof(string));
          dtPod.Columns.Add("目的地地址", typeof(string));
          dtPod.Columns.Add("联系人", typeof(string));
          dtPod.Columns.Add("联系人电话", typeof(string));
          dtPod.Columns.Add("手机号", typeof(string));
          dtPod.Columns.Add("货品名称", typeof(string));
          dtPod.Columns.Add("桶数", typeof(string));
          dtPod.Columns.Add("升数", typeof(string));
          dtPod.Columns.Add("比重", typeof(string));
          dtPod.Columns.Add("重量", typeof(string));
          dtPod.Columns.Add("省份", typeof(string));
          dtPod.Columns.Add("城市", typeof(string));
          dtPod.Columns.Add("出货日期", typeof(string));
          dtPod.Columns.Add("排车单号", typeof(string));
          dtPod.Columns.Add("车型", typeof(string));
          dtPod.Columns.Add("车牌号", typeof(string));
          dtPod.Columns.Add("总重量", typeof(string));
          dtPod.Columns.Add("提货费（从工厂）", typeof(string));
          dtPod.Columns.Add("提货费（从嘉托）", typeof(string));
          dtPod.Columns.Add("卸货费", typeof(string));
          dtPod.Columns.Add("包车费", typeof(string));
          dtPod.Columns.Add("起步费", typeof(string));
          dtPod.Columns.Add("点数", typeof(string));
          dtPod.Columns.Add("点费", typeof(string));
          dtPod.Columns.Add("油费", typeof(string));
          dtPod.Columns.Add("通行证费", typeof(string));
          dtPod.Columns.Add("其他费用", typeof(string));
          dtPod.Columns.Add("单票费用", typeof(string));
          dtPod.Columns.Add("合计(实际成本)", typeof(string));
          var podsResponses = Pods.GroupBy(x => new { x.FeeStr3, x.FeeStr1, })
                .Select(g => new
                {
                    count = g.Count(),
                    pods = g.Select(k => { return k; })
                });

          foreach (var podGroup in podsResponses)
          {
              decimal sumFeeDecimal1 = 0;
              decimal sumFeeDecimal2 = 0;
              decimal sumFeeDecimal3 = 0;
              decimal sumFeeDecimal5 = 0;
              decimal feeDecimal5 = 0;
              decimal sumFeeDecimal6 = 0;
              decimal sumFeeDecimal7 = 0;
              decimal sumFeeDecimal8 = 0;
              decimal sumFeeDecimal9 = 0;
              double  sumFeeStr4 = 0;
              decimal sumFeeDecimal4 = 0;
              foreach (var pod in podGroup.pods)
              {
                  sumFeeDecimal1 += pod.FeeDecimal1;
                  sumFeeDecimal2 += pod.FeeDecimal2;
                  sumFeeDecimal3 += pod.FeeDecimal3;
                  sumFeeDecimal4 += pod.FeeDecimal4;
                  feeDecimal5 += pod.FeeDecimal5;
                  sumFeeDecimal6 += pod.FeeDecimal6;
                  sumFeeDecimal7 += pod.FeeDecimal7;
                  sumFeeDecimal8 += pod.FeeDecimal8;
                  sumFeeDecimal9 += pod.FeeDecimal9;
                  sumFeeStr4 += (double)pod.FeeStr4.ToDouble();

              }
              sumFeeDecimal5=podGroup.pods.First().FeeDecimal11;
              double FeeInt1 = podGroup.pods.First().FeeInt1;
              string Feestr5 = podGroup.pods.First().FeeStr5;
              decimal feeDecimal11 = podGroup.pods.First().FeeDecimal11;
              podGroup.pods.OrderBy(p => p.DateTime1).Each((i, p) =>
              {
                  if (i>0)
                  {
                       sumFeeDecimal1 = 0;
                       sumFeeDecimal2 = 0;
                       sumFeeDecimal3 = 0;
                       sumFeeDecimal5 = 0;
                       sumFeeDecimal6 = 0;
                       sumFeeDecimal7 = 0;
                       sumFeeDecimal8 = 0;
                       sumFeeDecimal9 = 0;
                       sumFeeStr4 = 0;
                       FeeInt1 = 0;
                       Feestr5 = "";
                       feeDecimal11 = 0;
                       feeDecimal5 = 0;
                       sumFeeDecimal4 = 0;
                  }
                  DataRow dr = dtPod.NewRow();
                  dr["电脑单号"] = p.Str1;
                  dr["发货清单号"] = p.CustomerOrderNumber;
                  dr["客户账号"] = p.Str3;
                  dr["客户订单号"] = p.Str4;
                  dr["客户名称"] = p.Str5;
                  dr["目的地地址"] = p.Str6;
                  dr["联系人"] = p.Str7;
                  dr["联系人电话"] = p.Str8;
                  dr["手机号"] = p.Str9;
                  dr["货品名称"] = p.Str10;
                  dr["桶数"] = p.Str11;
                  dr["升数"] = p.Str12;
                  dr["比重"] = p.Str13;
                  dr["重量"] = p.Weight;
                  dr["省份"] = "上海";
                  dr["城市"] = p.EndCityName;
                  dr["出货日期"] = p.ActualDeliveryDate.Value.ToString("yyyy-MM-dd HH:mm");
                  dr["排车单号"] = p.FeeStr1;
                  dr["车型"] = Feestr5;
                  dr["车牌号"] = p.FeeStr3;
                  dr["总重量"] = sumFeeStr4 == 0 ? "" : sumFeeStr4.ToString();
                  dr["提货费（从工厂）"] = sumFeeDecimal1 == 0 ? "" : sumFeeDecimal1.ToString();
                  dr["提货费（从嘉托）"] = sumFeeDecimal2 == 0 ? "" : sumFeeDecimal2.ToString();
                  dr["卸货费"] = sumFeeDecimal3 == 0 ? "" : sumFeeDecimal3.ToString();
                  //dr["包车费"] = (p.FeeDecimal5 == 0 ? "" : sumFeeDecimal5.ToString()).ToDouble() == 0 ? "" : (p.FeeDecimal5 == 0 ? "" : sumFeeDecimal5.ToString());
                  //dr["包车费"] = (p.FeeDecimal5 == 0 ? "" : (sumFeeDecimal7 == 0 ? sumFeeDecimal5 : feeDecimal5).ToString());
                  dr["包车费"] = (p.FeeDecimal5 == 0 ? "" : feeDecimal5.ToString()).ToDouble() == 0 ? "" : feeDecimal5.ToString();
                  dr["起步费"] = sumFeeDecimal6 == 0 ? "" : sumFeeDecimal6.ToString();
                  dr["点数"] = FeeInt1 == 0 || FeeInt1==1 ? "" : FeeInt1.ToString();
                  dr["点费"] = sumFeeDecimal7 == 0 ? "" : sumFeeDecimal7.ToString();
                  dr["油费"] = sumFeeDecimal4 == 0 ? "" : sumFeeDecimal4.ToString();
                  dr["通行证费"] = sumFeeDecimal8 == 0 ? "" : sumFeeDecimal8.ToString();
                  dr["其他费用"] = sumFeeDecimal9 == 0 ? "" : sumFeeDecimal9.ToString();
                  dr["单票费用"] = p.FeeDecimal10;
                  dr["合计(实际成本)"] = feeDecimal11 == 0 ? "" : feeDecimal11.ToString();
                  dtPod.Rows.Add(dr);
              });

          }

          return this.ExportDataTableToExcel(dtPod, "ExportPods.xls");
      }
      private ActionResult ExportDataTableToExcel(DataTable dt, string FileName)
      {
          var sbHtml = new StringBuilder();
          sbHtml.Append("<style>td{mso-number-format:\"\\@\";}</style>");
          sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
          sbHtml.Append("<tr>");
          for (int i = 0; i < dt.Columns.Count; i++)
          {
              sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", dt.Columns[i].ColumnName);
          }

          sbHtml.Append("</tr>");
          for (int i = 0; i < dt.Rows.Count; i++)
          {
              sbHtml.Append("<tr>");
              for (int j = 0; j < dt.Columns.Count; j++)
              {
                  sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>{0}</td>", dt.Rows[i][j].ToString());
              }
              sbHtml.Append("</tr>");
          }

          sbHtml.Append("</table>");
          Response.Charset = "UTF-8";
          Response.HeaderEncoding = Encoding.UTF8;
          Response.AppendHeader("content-disposition", "attachment;filename=" + FileName);
          Response.ContentEncoding = Encoding.UTF8;
          Response.ContentType = "application/ms-excel";
          Response.Write("<meta http-equiv='content-type' content='application/ms-excel; charset=UTF-8'/>" + sbHtml.ToString());
          Response.Flush();
          Response.End();
          return new EmptyResult();
      }

    }
}
