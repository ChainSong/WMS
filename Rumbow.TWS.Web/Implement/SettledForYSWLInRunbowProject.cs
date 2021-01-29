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
    public class SettledForYSWLInRunbowProject : ISettledForPod
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
                    double cube = gPod.BoxNumber / 11;
                    var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                    QuotedPrice price = quotedPrice.FirstOrDefault(q =>
                    q.ProjectID == ProjectID && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == relatedCustomerID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                    && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < cube && q.EndVal >= cube
                    && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                    if (price == null)
                    {
                        this.GenErrorMessage_ByGroupedPod(gPod, Message, "无系统对应报价,请先配置报价.");
                        continue;
                    }

                    if (Message.Length > 0)
                    {
                        continue;
                    }

                    //易胜总运费补贴1%
                    decimal shipAmt = price.Price * (decimal)cube * (decimal)1.01;

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
                            Remark = "易胜运费补贴1%。",
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
                                    settledPods.First(s => s.PodID == innPodIDs.First()).Remark += "易胜出货，同天同城同客户30元点费";
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
                                    settledPods.First(s => innPodIDs.Contains(s.PodID)).Remark += "易胜退货，同天同城经销商30元点费"; ;
                                    break;
                                }
                            }
                        }

                    }
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
    }
}