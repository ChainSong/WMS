using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Web.Implement;
using Runbow.TWS.Common;
using Runbow.TWS.Biz;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Web.SettledMethod
{
    public class SettledForDFWLInRunbowProject : BaseSettled
    {
        public SettledForDFWLInRunbowProject(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator, RelatedCustomerID)
        {

        }

        public override void CustomerDefinedSettledPod()
        {
            foreach (var gPod in this.SettledPodResponse.GroupedPods)
            {
                #region 上海 承运商为顶丰 同天同城同客户
                if (gPod.StartCityName == "上海")
                {
                    var ids = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());

                    var groupPodResponse = new PodService().SettledPodSearch(new SettledPodRequest() { IDs = ids, SettledType = this.settledType, IsID = 1 });
                    if (!groupPodResponse.IsSuccess)
                    {
                        throw groupPodResponse.Exception;
                    }
                    var settledPodResponse = groupPodResponse.Result;

                    foreach (var sPod in settledPodResponse.GroupedPods)
                    {
                        var innerPod = sPod.PodIDs.Split('|');
                        var weight = sPod.Weight;
                        if (weight < 50)
                        {
                            weight = 50;
                        }
                        var price = quotedPriceCollection.FirstOrDefault(q => q.ProjectID == 1 && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == this.relatedCustomerID && q.StartCityName.Trim() == gPod.StartCityName.Trim()
                                                            && q.EndCityName.Trim() == gPod.EndCityName.Trim() && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim()
                                                            && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal <= weight && q.EndVal > weight && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime()
                                                            && (q.EffectiveEndTime > gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                        if (price == null)
                        {
                            this.GenErrorMessage_ByGroupedPod(gPod, "无系统报价，请先配置报价");
                            continue;
                        }

                        this.settledPodCollection.Each((i, settledPod) =>
                        {
                            var originalPodIDs = from q in innerPod where q == settledPod.PodID.ToString() select q;
                            if (originalPodIDs.Count() > 0)
                            {
                                if (settledPod.Weight == null)
                                {
                                    this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                                }
                                else
                                {
                                    if (originalPodIDs.First().ToString() == settledPod.PodID.ToString() && settledPod.PODTypeName == "出货运单")
                                    {
                                        settledPod.ShipAmt = sPod.Weight > 50 ? (decimal)settledPod.Weight * price.Price : (decimal)(settledPod.Weight / sPod.Weight) * price.Price * 50;
                                        settledPod.BAFAmt = 0;
                                        settledPod.Str4 = price.Price.ToString();
                                    }
                                }
                            }
                        });
                    }
                }
                #endregion
                else
                {
                    #region 扬州顶丰,同天同城
                    var weight = gPod.Weight;
                    if (weight < 50)
                    {
                        weight = 50;
                    }
                    var innerPods = gPod.PodIDs.Split('|');

                    var price = quotedPriceCollection.FirstOrDefault(q =>
                                                                     q.ProjectID == 1 && q.Target == 1 && q.TargetID == gPod.TargetID && q.StartCityName.Trim() == gPod.StartCityName.Trim()
                                                            && q.EndCityName.Trim() == gPod.EndCityName.Trim() && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim()
                                                            && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal <= weight && q.EndVal > weight && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime()
                                                            && (q.EffectiveEndTime > gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                    if (price == null)
                    {
                        this.GenErrorMessage_ByGroupedPod(gPod, "无系统报价,请先配置报价.");
                        continue;
                    }

                    this.settledPodCollection.Each((i, settledPod) =>
                        {
                            var originalPodIDs = from q in innerPods where q == settledPod.PodID.ToString() select q;
                            if (originalPodIDs.Count() > 0)
                            {
                                if (settledPod.Weight == null)
                                {
                                    this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                                }
                                else
                                {
                                    if (originalPodIDs.First().ToString() == settledPod.PodID.ToString() && settledPod.PODTypeName == "出货运单")
                                    {
                                        settledPod.ShipAmt = gPod.Weight > 50 ? (decimal)settledPod.Weight * price.Price : (decimal)(settledPod.Weight / gPod.Weight) * price.Price * 50;
                                        settledPod.BAFAmt = 0;
                                        settledPod.Str4 = price.Price.ToString();
                                    }
                                }
                            }
                        });
                    #endregion
                }
            }
        }
    }
}