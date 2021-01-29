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
    public class SettledForRBInRunbowProject : ISettledForPod
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
            #region Adidas
            if (podsResponse.Result.First().CustomerID == 1)
            {
                relatedCustomerID = 1;
                foreach (var gPod in groupedPodsResponse.Result.GroupedPods)
                {
                    double cube = gPod.BoxNumber / 12;
                    var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                    QuotedPrice price = new QuotedPrice();
                    QuotedPrice tempPrice;
                    tempPrice = quotedPrice.FirstOrDefault(q =>
                    q.ProjectID == ProjectID && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == relatedCustomerID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                    && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < cube && q.EndVal >= cube
                    && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                    if (tempPrice == null)
                    {
                        if (string.Equals(gPod.PODTypeName.Trim(), "退货运单", StringComparison.OrdinalIgnoreCase))
                        {
                            tempPrice = quotedPrice.FirstOrDefault(q =>
                            q.ProjectID == ProjectID && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == relatedCustomerID && q.StartCityName.Trim() == gPod.EndCityName.Trim() && q.EndCityName.Trim() == gPod.StartCityName.Trim()
                            && q.PodTypeName.Trim() == "出货运单" && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < cube && q.EndVal >= cube
                            && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));

                            if (tempPrice == null)
                            {
                                this.GenErrorMessage_ByGroupedPod(gPod, Message, "无系统报价，出货运单也无报价,请先配置报价.");
                                continue;
                            }

                            price.Price = tempPrice.Price;
                            price.Price *= (decimal)0.9;
                        }
                        else
                        {
                            this.GenErrorMessage_ByGroupedPod(gPod, Message, "无系统对应报价,请先配置报价.");
                            continue;
                        }


                    }
                    else
                    {
                        price.Price = tempPrice.Price;
                    }

                    if (Message.Length > 0)
                    {
                        continue;
                    }

                    decimal shipAmt = price.Price * (decimal)cube;

                    foreach (var id in innerPodIDs)
                    {
                        var originalPod = podsResponse.Result.FirstOrDefault(p => p.ID == id);
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
                            ShipAmt = shipAmt * (decimal)(originalPod.BoxNumber / gPod.BoxNumber),
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
                }
            }
            #endregion
            #region Akzo
            if (podsResponse.Result.First().CustomerID == 7)
            {
                relatedCustomerID = 7;
                //var groupedModels = podsResponse.Result.GroupBy(s => new { s.ActualDeliveryDate,s.Str17}).Select(g => new { models = g.Key, Pods = g.Select(k => { return k; }) });
                var groupedModels = podsResponse.Result.GroupBy(s => new { s.ActualDeliveryDate, s.Str17 }).Select(g => new { models = g.Key, count = g.Count() });
                IList<Pod> settelModelspods = new List<Pod>();
                foreach (var groupedModel in groupedModels)
                {
                    DateTime time = (DateTime)groupedModel.models.ActualDeliveryDate;
                    string CarModels = groupedModel.models.Str17;
                    int placeCount = 0;
                    decimal shipAmt = 0;
                    double Models = 0;
                    double weight = 0;
                    string price = "";
                    foreach (var item in podsResponse.Result)
                    {
                        if (item.ActualDeliveryDate == time && item.Str17 == CarModels)
                        {
                            var originalPod = podsResponse.Result.FirstOrDefault(p => p.ID == item.ID);
                            settelModelspods.Add(originalPod);
                        }
                    }
                    foreach (var settelModelspod in settelModelspods)
                    {
                        weight += (double)settelModelspod.Weight;
                    }
                    if (settelModelspods[0].Str17 == "小面包车" || settelModelspods[0].Str17 == "金杯车")
                    {
                        placeCount = settelModelspods.GroupBy(s => new { s.Str3 }).Select(g => new { str3 = g.Key }).Count();
                        shipAmt = 260 + (placeCount - 1) * 30;
                        price = "按车结算";
                    }
                    else
                    {
                        Models = string.IsNullOrEmpty(settelModelspods[0].Str17) ? 0 : Convert.ToDouble(settelModelspods[0].Str17);
                        if (Models == 0)
                        {
                            this.GenErrorMessage_ByPod(podsResponse.Result.FirstOrDefault(p => p.ID == settelModelspods[0].ID), Message, "请设置车型");
                            continue;
                        }
                        if (Models <= 5.5)
                        {
                            placeCount = settelModelspods.GroupBy(s => new { s.Str3 }).Select(g => new { str3 = g.Key, count = g.Count() }).Count();
                            shipAmt = 260 + (placeCount - 1) * 50;
                            price = "按车结算";
                        }
                        if (Models >= 6.2)
                        {
                            if (weight < 10000)
                            {
                                weight = 10000;
                            }
                            shipAmt = (decimal)weight / 1000 * 70;
                            price = (0.07).ToString();

                        }
                    }

                    foreach (var settelModelspod in settelModelspods)
                    {
                        SettledPod settledPod = new SettledPod()
                        {
                            ProjectID = settelModelspod.ProjectID,
                            CustomerOrderNumber = settelModelspod.CustomerOrderNumber,
                            SystemNumber = settelModelspod.SystemNumber,
                            PodID = settelModelspod.ID,
                            SettledNumber = settledNumber,
                            SettledType = 1,
                            CustomerOrShipperID = settelModelspod.ShipperID.Value,
                            CustomerOrShipperName = settelModelspod.ShipperName,
                            StartCityID = settelModelspod.StartCityID.Value,
                            StartCityName = settelModelspod.StartCityName,
                            EndCityID = settelModelspod.EndCityID.Value,
                            EndCityName = settelModelspod.EndCityName,
                            ShipperTypeID = settelModelspod.ShipperTypeID.Value,
                            ShipperTypeName = settelModelspod.ShipperTypeName,
                            PODTypeID = settelModelspod.PODTypeID.Value,
                            PODTypeName = settelModelspod.PODTypeName,
                            TtlOrTplID = settelModelspod.TtlOrTplID.Value,
                            TtlOrTplName = settelModelspod.TtlOrTplName,
                            ActualDeliveryDate = settelModelspod.ActualDeliveryDate.Value,
                            BoxNumber = settelModelspod.BoxNumber,
                            Weight = settelModelspod.Weight,
                            Volume = settelModelspod.Volume,
                            GoodsNumber = settelModelspod.GoodsNumber,
                            ShipAmt = shipAmt * (decimal)(settelModelspod.Weight / weight),
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
                            Str4 = price,
                            Str5 = string.Empty,
                            Remark = time.ToString() + "|" + CarModels,
                            DateTime1 = null,
                            DateTime2 = null,
                            CreateTime = DateTime.Now,
                            Creator = creator,
                            InvoiceID = 0,
                            RelatedCustomerID = settelModelspod.CustomerID,
                            IsAudit = true
                        };
                        settledPods.Add(settledPod);
                    }
                    settelModelspods.Clear();
                }
            }
            #endregion
            #region Nike
            if (podsResponse.Result.First().CustomerID == 8)
            {
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
                 q.ProjectID == ProjectID && q.Target == 1 && q.TargetID == pods.First().ShipperID && (q.StartCityName.Trim().Substring(0, 2) == "太仓" ? "太仓" : q.StartCityName.Trim()) == pods.First().StartCityName.Trim() &&
                 (q.EndCityName.Trim().Substring(0, 2) == "太仓" ? "太仓" : q.EndCityName.Trim()) == pods.First().EndCityName.Trim()
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
                if (string.IsNullOrEmpty(pod.BoxNumber.ToString()))
                {
                    this.GenErrorMessage_ByPod(pod, Message, " 请设置该运单的箱数");
                    continue;
                }
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