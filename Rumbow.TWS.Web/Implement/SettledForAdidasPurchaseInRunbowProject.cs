using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Common;
using Runbow.TWS.Web.Interface;
using System.Text.RegularExpressions;

namespace Runbow.TWS.Web.Implement
{
    public class SettledForAdidasPurchaseInRunbowProject : ISettledForPod
    {
        public void SettledPodForReceive(IEnumerable<long> PodIDs, string Creator, long ProjectID,int Target,long CustomerOrShipperID,long RelatedCustomerID, StringBuilder Message)
        {
            if (Message.Length > 0)
            {
                return;
            }

            PodService service = new PodService();
            var forSettledPodsResponse = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = PodIDs });

            if (!forSettledPodsResponse.IsSuccess)
            {
                throw forSettledPodsResponse.Exception;
            }

            var groupedPodsResponse = service.SettledPodSearch(new SettledPodRequest() { IDs = PodIDs, SettledType = 0 });
            if (!groupedPodsResponse.IsSuccess)
            {
                throw groupedPodsResponse.Exception;
            }

            var podsResponse = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = PodIDs });
            if (!podsResponse.IsSuccess)
            {
                throw podsResponse.Exception;
            }
            var quotedPrice = ApplicationConfigHelper.GetProjectQuotedPrice(ProjectID,Target,CustomerOrShipperID,RelatedCustomerID);
            var BAF = ApplicationConfigHelper.GetBAFPrice(ProjectID);
            string settledNumber = Guid.NewGuid().ToString();

            var SXshipper = from q in podsResponse.Result
                            where q.ShipperName == "山晓物流" || q.ShipperName == "陆久物流"
                            group q by new { q.ActualDeliveryDate, q.ShipperID,q.StartCityName, q.EndCityName }//q.ShipperID 
                                into r
                                select new
                                {
                                    ActualDeliveryDate = r.Key.ActualDeliveryDate.DateTimeToString(),
                                    StartCityName = r.Key.StartCityName,
                                    EndCityName = r.Key.EndCityName,
                                    ShipperID = r.Key.ShipperID,
                                    Volume = r.Sum(a => a.Volume)
                                };
            var newpodsResponse = from q in podsResponse.Result
                                  where q.ShipperName != "山晓物流" && q.ShipperName != "陆久物流"
                                  group q by new { q.ActualDeliveryDate, q.EndCityName }//q.ShipperID 
                                      into r
                                      select new
                                      {
                                          ActualDeliveryDate = r.Key.ActualDeliveryDate.DateTimeToString(),
                                          StartCityName ="广州",
                                          EndCityName = r.Key.EndCityName,
                                          // ShipperID = r.Key.ShipperID,
                                          Volume = r.Sum(a => a.Volume)
                                      };
            IList<SettledPod> settledPods = new List<SettledPod>();
            foreach (var gPod in groupedPodsResponse.Result.GroupedPods)
            {

             

                //燃油附加费
                decimal BAFPrice = 0;
                //立方数
                double cube = gPod.Volume;
                //if( gPod.EndCityName=="苏州"){
                //    switch (gPod.StartCityName)
                //    {
                //        case "大连":
                //            break;
                //        case "青岛":
                //            break;
                //        case "丹东":
                //            break;
                //        case "番禺":
                //            break;
                //        case "清远":
                //            break;
                //        case "云浮":
                //            break;
                //        default:
                //           gPod.StartCityName = "广州";
                //           break;
                //    }
                //}else 
                //  if(gPod.EndCityName == "天津") {
                //switch (gPod.StartCityName)
                //{
                //   case "大连":
                //       break;
                //   case "青岛":
                //       break;
                //   case "丹东":
                //       break;
                //   case "番禺":
                //       break;
                //   default:
                //     gPod.StartCityName = "广州";
                //     break;
                //} 
                //}
                if (gPod.StartCityName == "大连" || gPod.StartCityName == "丹东")
                {
                    foreach (var item in SXshipper)
                    {
                        if (item.ActualDeliveryDate.ToString() == gPod.ActualDeliveryDate && gPod.StartCityName == item.StartCityName && item.EndCityName == gPod.EndCityName)
                        {
                            cube = Convert.ToDouble(item.Volume);
                        }
                    }
                }
                else
                {
                    foreach (var item in newpodsResponse)
                    {
                        if (item.ActualDeliveryDate.ToString() == gPod.ActualDeliveryDate && item.EndCityName == gPod.EndCityName)
                        {
                            cube = Convert.ToDouble(item.Volume);
                        }
                    }
                }
                var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                //得到长途运输费的报价

                QuotedPrice price = quotedPrice.FirstOrDefault(q =>//
                    q.ProjectID == ProjectID && q.Target == 0 && q.TargetID == 13 && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                    && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < cube && q.EndVal >= cube
                    && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));

                QueryBAFPrice BAFPrices = BAF.FirstOrDefault(q =>
                   q.ProjectID == ProjectID && q.BAFStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && q.BAFEndTime > gPod.ActualDeliveryDate.ObjectToDateTime());


                if (BAFPrices != null)
                {
                    BAFPrice = BAFPrices.BAFPrice;
                }
                if (price == null)
                {
                    var forSettledPodsTemp = forSettledPodsResponse.Result.Where(p => gPod.PodIDs.Split('|').Contains(p.ID.ToString()));
                    if (forSettledPodsTemp != null)
                    {
                        forSettledPodsTemp.Each((k, g) =>
                        {
                            Message.Append("运单:").Append(g.CustomerOrderNumber).Append(",对应客户为:").Append(g.CustomerName).Append(",起运城市为:")
                            .Append(g.StartCityName).Append(",目的城市为:").Append(g.EndCityName).Append(",运输类型为:").Append(g.ShipperTypeName)
                            .Append(",运单类型为:").Append(g.PODTypeName).Append(",整车/零担为:").Append(g.TtlOrTplName).Append(" 无系统对应报价,请先配置报价.").Append("*");
                        });
                    }
                    continue;
                }
                if (Message.Length > 0)
                {
                    continue;
                }
                decimal shipAmt = 0;

                foreach (var id in innerPodIDs)
                {
                    //提货费
                    // decimal del = 0;
                    var originalPod = podsResponse.Result.FirstOrDefault(p => p.ID == id);
                    //是否是长途运输
                    //   if (originalPod.Str5 == "Y")
                    //  {
                    //   QuotedPrice delivery = quotedPrice.FirstOrDefault(q =>
                    //  q.ProjectID == ProjectID && q.Target == 0 && q.TargetID == gPod.TargetID && q.StartCityName.Trim() == originalPod.Str3 && q.EndCityName.Trim() == originalPod.Str3
                    //  && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < cube && q.EndVal >= cube
                    //  && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                    //  if (delivery != null)
                    // {
                    // del = delivery.Price;
                    // }
                    // }
                    shipAmt = price.Price >= 100 ? price.Price * (decimal)originalPod.Volume : price.Price * (decimal)originalPod.BoxNumber;

                    SettledPod settledPod = new SettledPod()
                    {
                        ProjectID = originalPod.ProjectID,
                        CustomerOrderNumber = originalPod.CustomerOrderNumber,
                        SystemNumber = originalPod.SystemNumber,
                        PodID = id,
                        SettledNumber = settledNumber,
                        SettledType = 0,
                        CustomerOrShipperID = originalPod.CustomerID.Value,
                        CustomerOrShipperName = originalPod.CustomerName,
                        StartCityID = originalPod.StartCityID.Value,
                        StartCityName = originalPod.StartCityName,
                        EndCityID = originalPod.EndCityID.Value,
                        EndCityName = originalPod.EndCityName,
                        ShipperTypeID = originalPod.ShipperTypeID.Value,
                        ShipperTypeName = originalPod.ShipperTypeName,
                        PODTypeID = originalPod.PODTypeID.Value,
                        PODTypeName = originalPod.PODTypeName,
                        TtlOrTplID = originalPod.TtlOrTplID.Value,
                        TtlOrTplName = originalPod.TtlOrTplName,
                        ActualDeliveryDate = originalPod.ActualDeliveryDate.Value,
                        BoxNumber = originalPod.BoxNumber,
                        Weight = originalPod.Weight,
                        Volume = originalPod.Volume,
                        GoodsNumber = originalPod.GoodsNumber,
                        ShipAmt = shipAmt,
                        //燃油附加费=长途运费*费率(浮动率)
                        BAFAmt = shipAmt * BAFPrice,// BAFPrice * shipAmt * (decimal)(originalPod.Str40.ObjectToDouble() / cube),
                        PointAmt = 0,
                        //其他费=卸货费(箱数*0.45)+提货费(是工厂直发才有提货费)
                        OtherAmt = 0,//(decimal)originalPod.BoxNumber * (decimal)0.45 + del * (decimal)originalPod.Volume,
                        Amt1 = 0,
                        Amt2 = 0,
                        Amt3 = 0,
                        Amt4 = 0,
                        Amt5 = 0,
                        Str1 = string.Empty,
                        Str2 = string.Empty,
                        Str3 = string.Empty,
                        Str4 = price.Price.ToString(),
                        Str5 = string.Empty,
                        Remark = string.Empty,
                        DateTime1 = null,
                        DateTime2 = null,
                        CreateTime = DateTime.Now,
                        Creator = Creator,
                        InvoiceID = 0,
                        RelatedCustomerID = originalPod.CustomerID,
                        IsAudit = true
                    };
                    settledPods.Add(settledPod);
                }
                if (Message.Length > 0)
                {
                    continue;
                }
            }
            if (Message.Length > 0)
            {
                return;
            }
            else
            {
                settledPods.Each((i, p) => { p.TotalAmt = p.ShipAmt + p.PointAmt - p.BAFAmt + p.OtherAmt + p.Amt1 + p.Amt2 + p.Amt3 + p.Amt4 + p.Amt5; });
                new SettledService().SettlePods(new SettlePodsRequest() { SettledPods = settledPods, SettledType = 0 });
            }
        }
        public void SettledPodForPay(IEnumerable<long> PodIDs, string Creator, long ProjectID, StringBuilder Message)
        {
            throw new NotImplementedException();
        }


        public void SettledPodForPay(IEnumerable<long> PodIDs, string creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
        {
            throw new NotImplementedException();
        }
    }
}