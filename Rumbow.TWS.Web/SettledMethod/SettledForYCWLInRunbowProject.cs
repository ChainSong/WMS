using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Web.Implement;
using Runbow.TWS.Common;

namespace Runbow.TWS.Web.SettledMethod
{
    public class SettledForYCWLInRunbowProject : BaseSettled
    {
        public SettledForYCWLInRunbowProject(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator, RelatedCustomerID)
        {

        }

        public override void CustomerDefinedSettledPod()
        {
            foreach (var gPod in this.SettledPodResponse.GroupedPods)
            {
                var weight = gPod.Weight;

                if (weight <= 200)
                {
                    weight = 199;
                }

                var innerPodIDs = gPod.PodIDs.Split('|');

                var price = quotedPriceCollection.FirstOrDefault(q => q.ProjectID == 1 && q.Target == 1 && q.TargetID == gPod.TargetID && q.TargetName == gPod.TargetName
                                                                       && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                                                                       && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim()
                                                                       && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < weight && q.EndVal >= weight
                                                                       && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime > gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                //最低收费
                var minPrice = quotedPriceCollection.FirstOrDefault(q => q.ProjectID == 1 && q.Target == 1 && q.TargetID == gPod.TargetID && q.TargetName == gPod.TargetName
                                                                       && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                                                                       && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim()
                                                                       && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal == 0 && q.EndVal == 200
                                                                       && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime > gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                if (price == null || minPrice == null)
                {
                    this.GenErrorMessage_ByGroupedPod(gPod, "无系统报价,请先配置报价.");
                    continue;
                }

                //合并总重小于200，单个送货地址
                if (gPod.Weight <= 200)
                {
                    List<string> list = new List<string>();
                    foreach (var id in innerPodIDs)
                    {
                        var item = this.podCollection.First(p => p.ID.ToString() == id);
                        if (!list.Contains(item.Str36))
                        {
                            this.settledPodCollection.Each((i, settledPod) =>
                                {
                                    if (item.ID == settledPod.PodID)
                                    {
                                        if (settledPod.Weight == null)
                                        {
                                            this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                                        }
                                        else
                                        {
                                            settledPod.ShipAmt = price.Price;
                                            settledPod.BAFAmt = 0;
                                            settledPod.Str4 = price.Price.ToString();
                                        }
                                    }
                                });
                        }
                        else
                        {
                            this.settledPodCollection.Each((i, settledPod) =>
                            {
                                if (item.ID == settledPod.PodID)
                                {
                                    if (settledPod.Weight == null)
                                    {
                                        this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                                    }
                                    else
                                    {
                                        settledPod.ShipAmt = 0;
                                        settledPod.BAFAmt = 0;
                                        settledPod.Str4 = price.Price.ToString();
                                    }
                                }
                            });
                        }
                        list.Add(item.Str36);
                    }
                }
                //合并运单总重大于200，单个送货地址，每条运单运费不足最低费用，按最低费用计算
                else
                {
                    this.settledPodCollection.Each((i, settledPod) =>
                        {
                            var originalPodIDs = from q in innerPodIDs where q == settledPod.PodID.ToString() select q;

                            if (originalPodIDs.Count() > 0)
                            {
                                if (settledPod.Weight == null)
                                {
                                    this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                                }
                                else
                                {
                                    if (originalPodIDs.First().ToString() == settledPod.PodID.ToString())
                                    {
                                        settledPod.ShipAmt = (decimal)settledPod.Weight * price.Price > minPrice.Price ? (decimal)settledPod.Weight * price.Price : minPrice.Price;
                                        settledPod.BAFAmt = 0;
                                        settledPod.Str4 = price.Price.ToString();
                                    }
                                }
                            }

                        });
                }
            }
        }
    }
}