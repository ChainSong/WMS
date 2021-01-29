using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runbow.TWS.Biz;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Web.Interface;


namespace Runbow.TWS.Web.Implement
{
    public class SettledForshanxiaoWLInRunbowProject : ISettledForPod
    {
        public void SettledPodForReceive(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, System.Text.StringBuilder Message)
        {
            throw new NotImplementedException();
        }


        public void SettledPodForPay(IEnumerable<long> PodIDs, string creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, System.Text.StringBuilder Message)
        {
            if (Message.Length > 0)
            {
                return;
            }

            PodService service = new PodService();
            var groupedPodsResponse = service.SettledPodSearch(new SettledPodRequest() { IDs = PodIDs, SettledType = 1 });
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

            string settledNumber = Guid.NewGuid().ToString();
            IList<SettledPod> settledPods = new List<SettledPod>();
            long relatedCustomerID = 0;
            #region akzo
            if (podsResponse.Result.First().CustomerID == 7)
            {
                relatedCustomerID = 7;
                #region 同天同城
                foreach (var gPod in groupedPodsResponse.Result.GroupedPods)
                {

                    double weight = gPod.Weight;
                    if (weight < 300)
                    {
                        weight = 300;
                    }
                    var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                    QuotedPrice price = new QuotedPrice();
                    QuotedPrice tempPrice;
                    tempPrice = quotedPrice.FirstOrDefault(q =>
                    q.ProjectID == ProjectID && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == relatedCustomerID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                    && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal <= weight && q.EndVal > weight
                    && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                    if (tempPrice == null)
                    {
                        this.GenErrorMessage_ByGroupedPod(gPod, Message, "无系统对应报价,请先配置报价.");
                        continue;
                    }
                    else
                    {
                        price.Price = tempPrice.Price;
                    }

                    if (Message.Length > 0)
                    {
                        continue;
                    }
                    decimal shipAmt = price.Price * (decimal)weight;

                    foreach (var id in innerPodIDs)
                    {
                        var originalPod = podsResponse.Result.FirstOrDefault(p => p.ID == id);
                        if (originalPod.Weight == null)
                        {
                            this.GenErrorMessage_ByGroupedPod(gPod, Message, "请先设置货物的重量");
                            continue;
                        }
                        SettledPod settledPod = new SettledPod()
                        {
                            ProjectID = originalPod.ProjectID,
                            CustomerOrderNumber = originalPod.CustomerOrderNumber,
                            SystemNumber = originalPod.SystemNumber,
                            PodID = id,
                            SettledNumber = settledNumber,
                            SettledType = 1,
                            CustomerOrShipperID = originalPod.ShipperID.Value,
                            CustomerOrShipperName = originalPod.ShipperName,
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
                            ShipAmt = shipAmt * (decimal)(originalPod.Weight / gPod.Weight),
                            BAFAmt = 0,
                            PointAmt = 0,
                            OtherAmt = 0,
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
                            Creator = creator,
                            InvoiceID = 0,
                            RelatedCustomerID = originalPod.CustomerID,
                            IsAudit = true
                        };
                        settledPods.Add(settledPod);
                    }

                    #region 判断同天同城同客户的运费是否小于80
                    var groupedCustomerPodsResponse = service.SettledPodSearch(new SettledPodRequest() { IDs = innerPodIDs, SettledType = 1, IsID = 1 });
                    if (!groupedCustomerPodsResponse.IsSuccess)
                    {
                        throw groupedCustomerPodsResponse.Exception;
                    }
                    foreach (var gcPod in groupedCustomerPodsResponse.Result.GroupedPods)
                    {
                        var innerCustomerPodIDs = gcPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                        decimal sumshipAmt = 0;
                        IList<SettledPod> settledCustomerPodPods = new List<SettledPod>();
                        foreach (var id in innerCustomerPodIDs)
                        {
                            var originalCustomerPod = podsResponse.Result.FirstOrDefault(p => p.ID == id);
                            foreach (var settledPod in settledPods)
                            {
                                if (originalCustomerPod.CustomerOrderNumber == settledPod.CustomerOrderNumber)
                                {
                                    sumshipAmt += (decimal)settledPod.ShipAmt;
                                    settledCustomerPodPods.Add(settledPod);
                                }
                            }
                        }
                        if (sumshipAmt < 80)
                        {
                            foreach (var settledCustomerPodPod in settledCustomerPodPods)
                            {
                                settledCustomerPodPod.OtherAmt += (80 - sumshipAmt) * (decimal)(settledCustomerPodPod.Weight / gcPod.Weight);
                            }

                        }
                    }
                    #endregion
                }
                #endregion

            }
            #endregion
            #region Nike
            if (podsResponse.Result.First().CustomerID == 8)
            {
                //起运城市是太仓CLC、太仓CRW、上海的按同城结算（大仓和工厂直发[上海]）
                var podsResponses = podsResponse.Result.Where(s => s.PODTypeName == "大仓出货" || s.PODTypeName == "工厂直发").GroupBy(x => new { x.ActualDeliveryDate, x.EndCityName, })
                 .Select(g => new
                 {
                     Peo = g.Key,
                     count = g.Count(),
                     pods = g.Select(k => { return k; })
                 });
                foreach (var pods in podsResponses)
                {
                    double boxnumner = 0;
                    double cube = 0;
                    foreach (var pod in pods.pods)
                    {
                        if (string.IsNullOrEmpty(pod.BoxNumber.ToString()))
                        {
                            this.GenErrorMessage_ByPod(pod, Message, " 请设置该运单的箱数");
                            continue;
                        }
                        boxnumner += (double)pod.BoxNumber;
                    }
                    cube = boxnumner / 11;
                    Settlement(settledNumber, ProjectID, Message, cube, boxnumner, pods.pods, settledPods, quotedPrice);
                }
                //到货城市是太仓CLC、太仓CRW、上海的按同城结算（退货运单）
                var podsReturnResponses = podsResponse.Result.Where(s => s.PODTypeName == "退货运单").GroupBy(x => new { x.ActualDeliveryDate, x.StartCityName, })
                   .Select(g => new
                   {
                       Peo = g.Key,
                       count = g.Count(),
                       pods = g.Select(k => { return k; })
                   });
                foreach (var pods in podsReturnResponses)
                {
                    double boxnumner = 0;
                    double cube = 0;
                    foreach (var pod in pods.pods)
                    {
                        if (string.IsNullOrEmpty(pod.BoxNumber.ToString()))
                        {
                            this.GenErrorMessage_ByPod(pod, Message, " 请设置该运单的箱数");
                            continue;
                        }
                        boxnumner += (double)pod.BoxNumber;
                    }
                    cube = boxnumner / 11;
                    Settlement(settledNumber, ProjectID, Message, cube, boxnumner, pods.pods, settledPods, quotedPrice);
                }
                //门店调拨（按正常的同天同城结算,但报价是按箱并不与承运商做关联结算，）
                var podsTransfersResponses = podsResponse.Result.Where(s => s.PODTypeName == "门店调拨").GroupBy(x => new { x.ActualDeliveryDate, x.StartCityName, x.EndCityName })
                      .Select(g => new
                      {
                          Peo = g.Key,
                          count = g.Count(),
                          pods = g.Select(k => { return k; })
                      });
                foreach (var pods in podsTransfersResponses)
                {
                    double boxnumner = 0;
                    foreach (var pod in pods.pods)
                    {
                        if (string.IsNullOrEmpty(pod.BoxNumber.ToString()))
                        {
                            this.GenErrorMessage_ByPod(pod, Message, " 请设置该运单的箱数");
                            continue;
                        }
                        boxnumner += (double)pod.BoxNumber;
                    }

                    Settlement(settledNumber, ProjectID, Message, 0, boxnumner, pods.pods, settledPods, quotedPrice);
                }

                //只有大仓出货按照11箱/方*10计算支出补贴
                var prs = podsResponse.Result.Where(p => p.PODTypeName == "大仓出货");
                prs.Each((i, e) =>
                {
                    settledPods.Each((m, n) =>
                    {
                        if (n.PodID == e.ID)
                        {
                            n.OtherAmt = (decimal)(n.BoxNumber / 11) * 10;
                        }
                    });
                });
            }
            #endregion
            #region AdidasPurchase
            if (podsResponse.Result.First().CustomerID == 13)
            {
                var newpodsResponse = from q in podsResponse.Result
                                      //where q.StartCityName != "天津" && q.StartCityName != "苏州"
                                      group q by new { q.ActualDeliveryDate, q.EndCityName, q.ShipperID }
                                          into r
                                          select new
                                          {
                                              ActualDeliveryDate = r.Key.ActualDeliveryDate.DateTimeToString(),
                                              EndCityName = r.Key.EndCityName,
                                              ShipperID = r.Key.ShipperID,
                                              Volume = r.Sum(a => a.Volume),
                                              BoxNumber = r.Sum(a => a.BoxNumber)
                                          };

               
                relatedCustomerID = 13;
                foreach (var gPod in groupedPodsResponse.Result.GroupedPods)
                {
                    double cube = gPod.Volume;
                    var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                    //gPod.StartCityName = "广东";
                    foreach (var item in newpodsResponse)
                    {
                        if (item.ActualDeliveryDate.ToString() == gPod.ActualDeliveryDate && item.EndCityName == gPod.EndCityName)
                        {
                            cube = Convert.ToDouble(item.Volume);
                        }
                    }
                    QuotedPrice price;
                    price = quotedPrice.FirstOrDefault(q =>
                    q.ProjectID == ProjectID && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == relatedCustomerID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                    && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < cube && q.EndVal >= cube
                    && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                    if (price == null)
                    {
                        this.GenErrorMessage_ByGroupedPod(gPod, Message, "无系统对应报价,请先配置报价.");
                    }

                    if (Message.Length > 0)
                    {
                        continue;
                    }
                    decimal shipAmt = price.Price;
                    decimal delivery = 0;
                    foreach (var id in innerPodIDs)
                    {

                        //提货费 同天同城收一票
                        //if (cube < 40)
                        //{
                        //    var a = innerPodIDs.First();
                        //    if (innerPodIDs.First().ToString() == id.ToString())
                        //    {
                        //        switch (gPod.EndCityName)
                        //        {
                        //            case "广州":
                        //                delivery = 15;
                        //                break;
                        //            case "清远":
                        //                delivery = 25;
                        //                break;
                        //            case "云浮":
                        //                delivery = 25;
                        //                break;
                        //            case "东莞":
                        //                delivery = 25;
                        //                break;
                        //            case "中山":
                        //                delivery = 25;
                        //                break;
                        //            case "佛山":
                        //                delivery = 25;
                        //                break;
                        //            case "惠州":
                        //                delivery = 25;
                        //                break;
                        //            case "河源":
                        //                delivery = 25;
                        //                break;
                        //            default:
                        //                delivery = 0;
                        //                break;
                        //        }
                        //    }
                        //}
                        var originalPod = podsResponse.Result.FirstOrDefault(p => p.ID == id);
                        //卸货费
                        //decimal unload = 0;
                        //if (gPod.EndCityName == "苏州")
                        //{
                        //    unload = (decimal)4.2;
                        //}
                        //else if (gPod.EndCityName == "广州")
                        //{
                        //    unload = cube< 150 ? (decimal)5.5 : (decimal)5;
                        //}
                        SettledPod settledPod = new SettledPod()
                        {
                            ProjectID = originalPod.ProjectID,
                            CustomerOrderNumber = originalPod.CustomerOrderNumber,
                            SystemNumber = originalPod.SystemNumber,
                            PodID = id,
                            SettledNumber = settledNumber,
                            SettledType = 1,
                            CustomerOrShipperID = originalPod.ShipperID.Value,
                            CustomerOrShipperName = originalPod.ShipperName,
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
                            ShipAmt = shipAmt * (decimal)originalPod.Volume,
                            BAFAmt = 0,
                            PointAmt = 0,
                            OtherAmt = 0,//unload * (decimal)originalPod.Volume + delivery * (decimal)originalPod.Volume,
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
                            Remark = "其他费用=提货费+卸货费",
                            DateTime1 = null,
                            DateTime2 = null,
                            CreateTime = DateTime.Now,
                            Creator = creator,
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
                    //var temp = (from p in podsResponse.Result.Where(k => innerPodIDs.Contains(k.ID)) group p by p.ID into g select new { g.Key, Pods = g });
                    //if (temp != null)
                    //{
                    //    temp.Each((i, k) =>
                    //    {
                    //        var innPodIDs = k.Pods.Select(p => p.ID);

                    //        if (1 == 1)
                    //        {
                    //            settledPods.First(s => s.PodID == innPodIDs.First()).OtherAmt = 4.2*k.Pods.Select(a=>a.Volume);
                    //            settledPods.First(s => s.PodID == innPodIDs.First()).Remark += "成耀出货，同天同城同客户点费135";
                    //        }
                    //    });
                    //}
                }

            }
            #endregion

            if (Message.Length > 0)
            {
                return;
            }

            //OtherAmt  其他费用=保底费
            else
            {
                settledPods.Each((i, p) => { p.TotalAmt = p.ShipAmt + p.PointAmt + p.BAFAmt + p.OtherAmt + p.Amt1 + p.Amt2 + p.Amt3 + p.Amt4 + p.Amt5; });
                new SettledService().SettlePods(new SettlePodsRequest() { SettledPods = settledPods, SettledType = 1 });
            }
        }
        private void GenErrorMessage_ByGroupedPod(GroupedPods pod, StringBuilder sb, string errorMessage)
        {
            sb.Append("运单:").Append(pod.PodIDs).Append(",对应客户为:").Append(pod.TargetName).Append(",起运城市为:")
              .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
              .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(errorMessage).Append("*");
        }
        private void GenErrorMessage_ByPod(Pod pod, StringBuilder sb, string errorMessage)
        {
            sb.Append("运单:").Append(pod.CustomerOrderNumber).Append(",对应客户为:").Append(pod.CustomerName).Append(",起运城市为:")
              .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
              .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(errorMessage).Append("*");
        }
        private void Settlement(string settledNumber, long ProjectID, StringBuilder Message, double cube, double boxnumner, IEnumerable<Pod> pods, IList<SettledPod> settledPods, IEnumerable<QuotedPrice> quotedPrices)
        {
            decimal shipAmt = 0;
            QuotedPrice price = null;
            //调拨运单
            if (cube == 0)
            {
                price = quotedPrices.FirstOrDefault(q =>
                q.ProjectID == ProjectID && q.Target == 1 && q.StartCityName.Trim() == pods.First().StartCityName.Trim() && q.EndCityName.Trim() == pods.First().EndCityName.Trim()
                && q.PodTypeName.Trim() == pods.First().PODTypeName.Trim() && q.ShipperTypeName.Trim() == pods.First().ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == pods.First().TtlOrTplName.Trim() && q.StartVal <= boxnumner && q.EndVal >= boxnumner
                && q.EffectiveStartTime <= pods.First().ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= pods.First().ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                if (price == null)
                {
                    this.GenErrorMessage_ByPod(pods.First(), Message, " 无系统对应报价,请先配置报价.");
                }
                if (Message.Length > 0)
                {
                    return;
                }
                shipAmt = price.Price * (decimal)boxnumner;
            }
            else
            {
                price = quotedPrices.FirstOrDefault(q =>
                 q.ProjectID == ProjectID && q.Target == 1 && q.TargetID == pods.First().ShipperID &&q.StartCityName.Trim() == pods.First().StartCityName.Trim() &&q.EndCityName.Trim() == pods.First().EndCityName.Trim()
                && q.PodTypeName.Trim() == pods.First().PODTypeName.Trim() && q.ShipperTypeName.Trim() == pods.First().ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == pods.First().TtlOrTplName.Trim() && q.StartVal < cube && q.EndVal >= cube
                && q.EffectiveStartTime <= pods.First().ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= pods.First().ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                if (price == null)
                {
                    this.GenErrorMessage_ByPod(pods.First(), Message, " 无系统对应报价,请先配置报价.");

                }
                if (Message.Length > 0)
                {
                    return;
                }
                shipAmt = price.Price * (decimal)cube;
            }

            foreach (var pod in pods)
            {
                SettledPod settledPod = new SettledPod()
                {
                    ProjectID = pod.ProjectID,
                    CustomerOrderNumber = pod.CustomerOrderNumber,
                    SystemNumber = pod.SystemNumber,
                    PodID = pod.ID,
                    SettledNumber = settledNumber,
                    SettledType = 1,
                    CustomerOrShipperID = pod.CustomerID.Value,
                    CustomerOrShipperName = pod.CustomerName,
                    StartCityID = pod.StartCityID.Value,
                    StartCityName = pod.StartCityName,
                    EndCityID = pod.EndCityID.Value,
                    EndCityName = pod.EndCityName,
                    ShipperTypeID = pod.ShipperTypeID.Value,
                    ShipperTypeName = pod.ShipperTypeName,
                    PODTypeID = pod.PODTypeID.Value,
                    PODTypeName = pod.PODTypeName,
                    TtlOrTplID = pod.TtlOrTplID.Value,
                    TtlOrTplName = pod.TtlOrTplName,
                    ActualDeliveryDate = pod.ActualDeliveryDate.Value,
                    BoxNumber = pod.BoxNumber,
                    Weight = pod.Weight,
                    Volume = pod.Volume,
                    GoodsNumber = pod.GoodsNumber,
                    ShipAmt = shipAmt * (decimal)(pod.BoxNumber / boxnumner),
                    BAFAmt = 0,
                    PointAmt = 0,
                    OtherAmt = 0,
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
                    Creator = pod.Creator,
                    InvoiceID = 0,
                    RelatedCustomerID = pod.CustomerID,
                    IsAudit = true
                };
                settledPods.Add(settledPod);

            }

        }
    }
}