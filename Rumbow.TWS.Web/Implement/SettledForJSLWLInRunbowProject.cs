using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Web.Interface;
using Runbow.TWS.Common;
using System.Text.RegularExpressions;
namespace Runbow.TWS.Web.Implement
{
    public class SettledForJSLWLInRunbowProject : ISettledForPod
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

                var Data = from q in groupedPodsResponse.Result.GroupedPods
                           group q by new { q.ActualDeliveryDate, q.PODTypeName } into g
                           let ids = g.Select(b => b.PodIDs.ToString()).ToArray()
                           select new
                           {
                               g.Key,
                               ActualDeliveryDate = g.Max(a => a.ActualDeliveryDate),
                               ShipperTypeName = g.Max(a => a.ShipperTypeName),
                               BoxNumber = g.Sum(a => a.BoxNumber),
                               Volume = g.Sum(a => a.Volume),
                               PodIDs = string.Join("|", ids),

                           };
                var DataFee = from q in podsResponse.Result
                              where Regex.IsMatch(q.Str7.Substring(0, 1), @"^[\u4e00-\u9fa5]+$")
                              group q by new { q.ActualDeliveryDate, q.PODTypeName, q.EndCityName, q.StartCityName, q.ShipperTypeName } into g
                              select new
                              {
                                  g.Key,
                                  BoxNumber = g.Sum(a => a.BoxNumber)
                              };
                //groupedPodsResponse.Result.GroupedPods.GroupBy(q => q.ActualDeliveryDate).Select(a=>a.Sum(q=>q.BoxNumber));
                foreach (var gPod in groupedPodsResponse.Result.GroupedPods)
                {

                    double cube = gPod.BoxNumber / 12;
                    double BoxNumber = 0;
                    foreach (var item in DataFee)
                    {
                        if (gPod.ActualDeliveryDate == item.Key.ActualDeliveryDate.ToString() && gPod.EndCityName == item.Key.EndCityName && gPod.StartCityName == item.Key.StartCityName && gPod.ShipperTypeName == item.Key.ShipperTypeName)
                        {
                            BoxNumber += (double)item.BoxNumber;
                        }
                    }

                    var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                    QuotedPrice price;
                    price = quotedPrice.FirstOrDefault(q =>
                    q.ProjectID == ProjectID && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == relatedCustomerID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                    && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < cube && q.EndVal >= cube
                    && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));

                    if (price == null)
                    {
                        if (string.Equals(gPod.PODTypeName.Trim(), "退货运单", StringComparison.OrdinalIgnoreCase))
                        {
                            price = quotedPrice.FirstOrDefault(q =>
                        q.ProjectID == ProjectID && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == relatedCustomerID && q.StartCityName.Trim() == gPod.EndCityName.Trim() && q.EndCityName.Trim() == gPod.StartCityName.Trim()
                        && q.PodTypeName.Trim() == "出货运单" && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < cube && q.EndVal >= cube
                        && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));

                            if (price == null)
                            {
                                //this.GenErrorMessage_ByGroupedPod(gPod, Message, "无系统报价，出货运单也无报价,请先配置报价.");
                                continue;
                            }

                            price.Price = price.Price;
                            price.Price *= (decimal)0.9;

                        }
                        else
                        {
                            // this.GenErrorMessage_ByGroupedPod(gPod, Message, "无系统对应报价,请先配置报价.");
                            continue;
                        }
                    }

                    if (Message.Length > 0)
                    {
                        continue;
                    }

                    decimal shipAmt = price.Price;

                    foreach (var id in innerPodIDs)
                    {

                        decimal delivery = 0;
                        //提货费
                        if (cube < 30)
                        {
                            var a = Data.First();
                            if (innerPodIDs.First().ToString() == id.ToString())
                            {

                                delivery = 300;
                            }

                        }

                        var originalPod = podsResponse.Result.FirstOrDefault(p => p.ID == id);
                        //装卸费
                        if (gPod.PODTypeName == "退货运单")
                        {
                            delivery = (delivery + (decimal)(originalPod.BoxNumber / 12) * (decimal)4.2) * (decimal)0.9;
                        }
                        else
                        {
                            delivery += (decimal)(originalPod.BoxNumber / 12) * (decimal)4.2;
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
                            ShipAmt = shipAmt * (decimal)(originalPod.BoxNumber / 12),
                            BAFAmt = 0,
                            PointAmt = 0,

                            OtherAmt = delivery,
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

                    if (string.Equals(gPod.PODTypeName.Trim(), "出货运单", StringComparison.OrdinalIgnoreCase))
                    {
                        if ((BoxNumber / 12) <= 10)
                        {
                            var temp = (from p in podsResponse.Result.Where(k => innerPodIDs.Contains(k.ID) && Regex.IsMatch(k.Str7.Substring(0, 1), @"^[\u4e00-\u9fa5]+$")) group p by p.ID into g select new { g.Key, Pods = g });

                            if (temp != null)
                            {
                                temp.Each((i, k) =>
                                {
                                    var innPodIDs = k.Pods.Select(p => p.ID);

                                    if (1 == 1)
                                    {
                                        settledPods.First(s => s.PodID == innPodIDs.First()).PointAmt = 60;
                                        settledPods.First(s => s.PodID == innPodIDs.First()).Remark += "陆久出货，同天低于10方收取60";
                                    }
                                });
                            }
                        }
                    }
                    //if (string.Equals(gPod.PODTypeName.Trim(), "调拨运单", StringComparison.OrdinalIgnoreCase))
                    //{
                    //    var temp = (from p in podsResponse.Result.Where(k => innerPodIDs.Contains(k.ID) && k.PODTypeName.Trim() == "调拨运单" && Char.IsLetter(k.Str7, 1)) group p by p.ID into g select new { g.Key, Pods = g });
                    //    if (temp != null)
                    //    {
                    //        temp.Each((i, k) =>
                    //        {
                    //            var innPodIDs = k.Pods.Select(p => p.ID);
                    //            if (1 == 1)
                    //            {
                    //                settledPods.First(s => s.PodID == innerPodIDs.First()).PointAmt = 60;
                    //                settledPods.First(s => s.PodID == innerPodIDs.First()).Remark += "陆久调拨,";
                    //            }
                    //        });
                    //    }
                    //}
                    if (string.Equals(gPod.PODTypeName.Trim(), "退货运单", StringComparison.OrdinalIgnoreCase))
                    {
                        if ((BoxNumber / 12) <= 10)
                        {
                            var temp = (from p in podsResponse.Result.Where(k => innerPodIDs.Contains(k.ID) && Regex.IsMatch(k.Str7.Substring(0, 1), @"^[\u4e00-\u9fa5]+$")) group p by p.ID into g select new { g.Key, Pods = g });

                            if (temp != null)
                            {
                                temp.Each((i, k) =>
                                {
                                    var innPodIDs = k.Pods.Select(p => p.ID);

                                    if (1 == 1)
                                    {
                                        settledPods.First(s => s.PodID == innPodIDs.First()).PointAmt = (decimal)(60 * 0.9);
                                        settledPods.First(s => s.PodID == innPodIDs.First()).Remark += "陆久出货，同天低于10方收取60";
                                    }
                                });
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