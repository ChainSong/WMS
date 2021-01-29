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

namespace Runbow.TWS.Web.Implement
{
    public class SettledForHFWLInRunbowProject:ISettledForPod
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
            var podsResponse = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = PodIDs });
            if (!podsResponse.IsSuccess)
            {
                throw podsResponse.Exception;
            }

            var quotedPrice = ApplicationConfigHelper.GetProjectQuotedPrice(ProjectID,Target,CustomerOrShipperID,RelatedCustomerID);

            string settledNumber = Guid.NewGuid().ToString();
            IList<SettledPod> settledPods = new List<SettledPod>();
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
                    cube = boxnumner / 12;
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
                    cube = boxnumner / 12;
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

                //只有大仓出货按照12箱/方*10计算支出补贴
                var prs = podsResponse.Result.Where(p => p.PODTypeName == "大仓出货");
                prs.Each((i, e) =>
                {
                    settledPods.Each((m, n) =>
                    {
                        if (n.PodID == e.ID)
                        {
                            n.OtherAmt = (decimal)(n.BoxNumber / 12) * 10;
                        }
                    });
                });
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