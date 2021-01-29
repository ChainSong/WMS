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
    public class SettledForSFInRunbowProject : ISettledForPod
    {
        #region ISettledForPod Members

        public void SettledPodForReceive(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, System.Text.StringBuilder Message)
        {
            throw new NotImplementedException();
        }

        public void SettledPodForPay(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, System.Text.StringBuilder Message)
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

            foreach (var pod in podsResponse.Result)
            {
                //For Oriflame
                if (pod.CustomerID == 5)
                {
                    if (!pod.Weight.HasValue || pod.Weight.Value == 0)
                    {
                        Message.Append("运单:").Append(pod.CustomerOrderNumber).Append(",对应承运商为:").Append(pod.ShipperName).Append(",起运城市为:")
                       .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
                       .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(" 无重量,请先编辑运单重量。").Append("*");
                        continue;
                    }

                    IEnumerable<QuotedPrice> prices = quotedPrice.Where(q =>
                    q.ProjectID == ProjectID && q.Target == 0 && q.TargetID == pod.CustomerID && q.StartCityName == pod.StartCityName && q.EndCityName == pod.EndCityName
                    && q.PodTypeName == pod.PODTypeName && q.ShipperTypeName == pod.ShipperTypeName && q.TplOrTtlName == pod.TtlOrTplName
                    && q.EffectiveStartTime <= pod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= pod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));

                    if (prices == null || !prices.Any())
                    {
                        Message.Append("运单:").Append(pod.CustomerOrderNumber).Append(",对应承运商为:").Append(pod.ShipperName).Append(",起运城市为:")
                       .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
                       .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(" 无对应报价,请先配置。").Append("*");
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
                        CustomerOrShipperID = pod.ShipperID.Value,
                        CustomerOrShipperName = pod.ShipperName,
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
                        Str4 = string.Empty,
                        Str5 = string.Empty,
                        Remark = string.Empty,
                        DateTime1 = null,
                        DateTime2 = null,
                        CreateTime = DateTime.Now,
                        Creator = Creator,
                        InvoiceID = 0,
                        RelatedCustomerID = pod.CustomerID,
                        IsAudit = true
                    };

                    prices = prices.OrderBy(p => p.StartVal);
                    decimal firstPrice = prices.FirstOrDefault().Price;
                    decimal lastPrice = prices.FirstOrDefault().Price;
                    double firstweight = 1;

                    if (pod.Weight.Value <= firstweight)
                    {
                        settledPod.ShipAmt = firstPrice;
                    }
                    else
                    {
                        settledPod.ShipAmt = firstPrice + (decimal)(pod.Weight.Value - firstweight) * lastPrice;
                    }

                    settledPods.Add(settledPod);
                }
            }

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

        #endregion
    }
}