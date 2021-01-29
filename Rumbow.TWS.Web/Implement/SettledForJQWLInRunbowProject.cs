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
    public class SettledForJQWLInRunbowProject : ISettledForPod
    {
        public void SettledPodForReceive(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
        {
            throw new NotImplementedException();
        }

        public void SettledPodForPay(IEnumerable<long> PodIDs, string creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
        {
            if (Message.Length > 0)
            {
                return;
            }
            PodService service = new PodService();
            IList<SettledPod> settledPods = new List<SettledPod>();
            //podsResponse 待结算运单列表
            var podsResponse = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = PodIDs });
            if (!podsResponse.IsSuccess)
            {
                throw podsResponse.Exception;
            }
            //取得报价
            var quotedPrice = ApplicationConfigHelper.GetProjectQuotedPrice(ProjectID,Target,CustomerOrShipperID,RelatedCustomerID);
            //生成结算序列号
            string settledNumber = Guid.NewGuid().ToString();

            long relatedCustomerID = 0;

            #region Adidas

            if (podsResponse.Result.First().CustomerID == 1)
            {
                relatedCustomerID = 1;
                var groupedPodsResponse = service.SettledPodSearch(new SettledPodRequest() { IDs = PodIDs, SettledType = 1 });
                if (!groupedPodsResponse.IsSuccess)
                {
                    throw groupedPodsResponse.Exception;
                }
                foreach (var gPod in groupedPodsResponse.Result.GroupedPods)
                {
                    double cube = gPod.BoxNumber / 11;
                    var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                    QuotedPrice price = quotedPrice.FirstOrDefault(q =>
                    q.ProjectID == ProjectID && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == relatedCustomerID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                    && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < cube && q.EndVal >= cube
                    && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                    if (price == null)
                    {
                        this.GenErrorMessage_ByGroupedPod(gPod, Message, "无系统对应报价, 请先配置报价.");
                        continue;
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

                    if (Message.Length > 0)
                    {
                        continue;
                    }

                    //同天同城，出货运单，第一个客户不算点费，之后30元一个
                    if (string.Equals(gPod.PODTypeName.Trim(), "出货运单", StringComparison.OrdinalIgnoreCase))
                    {
                        var temp = (from p in podsResponse.Result.Where(k => innerPodIDs.Contains(k.ID)) group p by p.Str7 into g select new { g.Key, Pods = g });
                        if (temp != null)
                        {
                            temp.Each((i, k) =>
                            {
                                if (i > 0)
                                {
                                    var innPodIDs = k.Pods.Select(p => p.ID);
                                    settledPods.First(s => s.PodID == innPodIDs.First()).PointAmt = 30;
                                    settledPods.First(s => s.PodID == innPodIDs.First()).Remark = "佳勤出货，同天同城同客户30元点费";
                                }
                            });
                        }
                    }

                    //同天同城，退货运单，客户类型为经销商，无论去几个地方30元，自营店无
                    if (string.Equals(gPod.PODTypeName.Trim(), "退货运单", StringComparison.OrdinalIgnoreCase))
                    {
                        var temp = (from p in podsResponse.Result.Where(k => innerPodIDs.Contains(k.ID)) group p by p.Str10 into g select new { g.Key, Pods = g });
                        if (temp != null)
                        {
                            foreach (var k in temp)
                            {
                                if (k.Key == "经销商")
                                {
                                    var innPodIDs = k.Pods.Select(p => p.ID);
                                    settledPods.First(s => innPodIDs.Contains(s.PodID)).PointAmt = 30;
                                    settledPods.First(s => innPodIDs.Contains(s.PodID)).Remark = "佳勤退货，同天同城经销商30元点费";
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            #endregion Adidas
            #region akzo
            if (podsResponse.Result.First().CustomerID == 7)
            {
                relatedCustomerID = 7;
                #region   目的地是四川但不是成都的   为转仓运单
                var regins = ApplicationConfigHelper.GetRegions().Where(r => r.SupperID == 29 && r.Name != "成都");
                IList<string> cityNames = new List<string>();
                regins.Each((i, r) =>
                {
                    cityNames.Add(r.Name);
                    ApplicationConfigHelper.GetRegions().Where(g => g.SupperID == r.ID || g.SupperID == 256).Each((j, k) => cityNames.Add(k.Name));
                });
                IList<Pod> Normal = new List<Pod>();
                //存放转仓运单的
                IList<Pod> Special = new List<Pod>();
                podsResponse.Result.Each((i, p) =>
                {
                    if (cityNames.Contains(p.EndCityName))
                    {
                        Special.Add(p);
                    }
                    else
                    {
                        Normal.Add(p);
                    }
                });
                IEnumerable<long> podIDC = Normal.Select(p => p.ID);
                var groupedSprcial = Special.GroupBy(s => s.ActualDeliveryDate).Select(g => new { Date = g.Key.Value, Pods = g.Select(k => { return k; }) });
                IList<Pod> DatePods = new List<Pod>();
                double SpecialWeight = 0;
                decimal SpecialPrice = 0;
                decimal SpecialShipAmt = 0;
                foreach (var item in groupedSprcial)
                {
                    DateTime dt = item.Date;
                    foreach (var pod in Special)
                    {
                        if (pod.ActualDeliveryDate == dt)
                        {
                            SpecialWeight += (double)pod.Weight;
                            DatePods.Add(pod);
                        }
                    }
                    if (SpecialWeight >= 30000)
                    {
                        SpecialPrice = (decimal)668 / 1000;
                    }
                    else if (SpecialWeight >= 20000)
                    {
                        SpecialPrice = (decimal)670 / 1000;
                    }
                    else if (SpecialWeight >= 10000)
                    {
                        SpecialPrice = (decimal)675 / 1000;
                    }
                    else if (SpecialWeight >= 3000)
                    {
                        SpecialPrice = (decimal)685 / 1000;
                    }
                    else
                    {
                        SpecialPrice = (decimal)690 / 1000;
                    }
                    SpecialShipAmt = (decimal)SpecialWeight * SpecialPrice;
                    if (SpecialWeight > 0)
                    {
                        foreach (var special in DatePods)
                        {
                            if (special.Weight == null)
                            {
                                this.GenErrorMessage_ByPod(special, Message, "请先设置货物的重量");
                                continue;
                            }
                            SettledPod settledPod = new SettledPod()
                            {
                                ProjectID = special.ProjectID,
                                CustomerOrderNumber = special.CustomerOrderNumber,
                                SystemNumber = special.SystemNumber,
                                PodID = special.ID,
                                SettledNumber = special.CustomerOrderNumber,
                                SettledType = 1,
                                CustomerOrShipperID = special.ShipperID.Value,
                                CustomerOrShipperName = special.ShipperName,
                                StartCityID = special.StartCityID.Value,
                                StartCityName = special.StartCityName,
                                EndCityID = special.EndCityID.Value,
                                EndCityName = special.EndCityName,
                                ShipperTypeID = special.ShipperTypeID.Value,
                                ShipperTypeName = special.ShipperTypeName,
                                PODTypeID = special.PODTypeID.Value,
                                PODTypeName = special.PODTypeName,
                                TtlOrTplID = special.TtlOrTplID.Value,
                                TtlOrTplName = special.TtlOrTplName,
                                ActualDeliveryDate = special.ActualDeliveryDate.Value,
                                BoxNumber = special.BoxNumber,
                                Weight = special.Weight,
                                Volume = special.Volume,
                                GoodsNumber = special.GoodsNumber,
                                ShipAmt = SpecialShipAmt * (decimal)(special.Weight / SpecialWeight),
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
                                Str4 = SpecialPrice.ToString(),
                                Str5 = string.Empty,
                                Remark = string.Empty,
                                DateTime1 = null,
                                DateTime2 = null,
                                CreateTime = DateTime.Now,
                                Creator = creator,
                                InvoiceID = 0,
                                RelatedCustomerID = special.CustomerID,
                                IsAudit = true
                            };

                            settledPods.Add(settledPod);
                        }
                    }
                    SpecialWeight = 0;
                    SpecialPrice = 0;
                    SpecialShipAmt = 0;
                    DatePods.Clear();
                }
                var groupedPodsResponse = service.SettledPodSearch(new SettledPodRequest() { IDs = podIDC, SettledType = 1, IsID = 1 });
                if (!groupedPodsResponse.IsSuccess)
                {
                    throw groupedPodsResponse.Exception;
                }
                #endregion

                #region  目的地是成都 、重庆、西藏下面的市   就要按照同天同城同客户获取报价

                var podsResponses = from q in podsResponse.Result
                                    group q by new { q.Str2, q.StartCityName, q.ActualDeliveryDate, q.EndCityName }

                                        into g
                                        let ids = g.Select(b => b.ID.ToString()).ToArray()
                                        select new
                                        {
                                            ActualDeliveryDate = g.Max(a => a.ActualDeliveryDate),
                                            ShipperTypeName = g.Max(a => a.ShipperTypeName),
                                            BoxNumber = g.Sum(a => a.BoxNumber),
                                            Volume = g.Sum(a => a.Volume),
                                            Weight = g.Sum(a => a.Weight),
                                            TargetID = g.Max(a => a.ShipperID),
                                            ShipperName = g.Max(a => a.ShipperName),
                                            ShipperTypeID = g.Max(a => a.ShipperTypeID),
                                            PODTypeID = g.Max(a => a.PODTypeID),
                                            PODTypeName = g.Max(a => a.PODTypeName),
                                            TtlOrTplID = g.Max(a => a.TtlOrTplID),
                                            TtlOrTplName = g.Max(a => a.TtlOrTplName),
                                            StartCityID = g.Max(a => a.StartCityID),
                                            StartCityName = g.Max(a => a.StartCityName),
                                            GoodsNumber = g.Max(a => a.GoodsNumber),
                                            EndCityName = g.Max(a => a.EndCityName),
                                            PodIDs = string.Join("|", ids),
                                            //sd=g.Max(a=>a.),
                                        };
                foreach (var gPod in podsResponses)
                {
                    List<string> endcityname = new List<string> { "成都","重庆","长寿","达州","涪陵","广安","华蓥","江津","万源",
                                                                  "万州","永川","昌都","噶尔","拉萨","林芝","那曲"," 乃东","日喀则"
                                                                 };
                    
                    if (endcityname.Contains(gPod.EndCityName))
                    {
                        double weight = (double)gPod.Weight;
                        double weights = 0;
                        if (weight < 50)
                        {
                            weights = 50;
                        }
                        else
                        {
                            weights = weight;
                        }

                        var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                        QuotedPrice price = new QuotedPrice();
                        QuotedPrice tempPrice;
                        //tempPrice = quotedPrice.FirstOrDefault(q =>
                        //q.ProjectID == ProjectID && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == relatedCustomerID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                        //&& q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal <= weight && q.EndVal > weight
                        //&& q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));

                        tempPrice = quotedPrice.FirstOrDefault(q =>
                       q.ProjectID == ProjectID && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == relatedCustomerID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                       && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal <= weights && q.EndVal > weight
                       && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));



                        GroupedPods gPods = new GroupedPods();
                        gPods.PodIDs = gPod.PodIDs;
                        gPods.TargetName = gPod.ShipperName;
                        gPods.StartCityName = gPod.StartCityName;
                        gPods.EndCityName = gPod.EndCityName;
                        gPods.ShipperTypeName = gPod.ShipperTypeName;
                        gPods.PODTypeName = gPod.PODTypeName;
                        gPods.TtlOrTplName = gPod.TtlOrTplName;
                        if (tempPrice == null)
                        {
                            this.GenErrorMessage_ByGroupedPod(gPods, Message, "无系统对应报价,请先配置报价.");
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
                        decimal shipAmt = price.Price;

                        foreach (var id in innerPodIDs)
                        {
                            var originalPod = podsResponse.Result.FirstOrDefault(p => p.ID == id);
                            if (originalPod.Weight == null)
                            {
                                this.GenErrorMessage_ByGroupedPod(gPods, Message, "请先设置货物的重量");
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
                                ShipAmt = weight < 50 ? shipAmt * (decimal)(originalPod.Weight / weight) * 50 : shipAmt * (decimal)originalPod.Weight,
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
    }
}