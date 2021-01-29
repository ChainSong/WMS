using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Common;
using Runbow.TWS.Web.Implement;

namespace Runbow.TWS.Web.SettledMethod
{
    public class SettledForAkzoInRunbowProject : BaseSettled
    {
        public SettledForAkzoInRunbowProject(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator, RelatedCustomerID)
        {

        }

        public override void CustomerDefinedSettledPod()
        {
            foreach (var gpod in this.SettledPodResponse.GroupedPods)
            {
                var weight = gpod.Weight;
                if (weight < 300)
                {
                    weight = 300;
                }
                string[] innerPodID = gpod.PodIDs.Split('|');

                var price = quotedPriceCollection.FirstOrDefault(q =>
                                                        q.ProjectID == 1 && q.Target == 0 && q.TargetID == gpod.TargetID && q.StartCityName.Trim() == gpod.StartCityName.Trim()
                                                        && q.EndCityName.Trim() == gpod.EndCityName.Trim() && q.PodTypeName.Trim() == gpod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gpod.ShipperTypeName.Trim()
                                                        && q.TplOrTtlName.Trim() == gpod.TtlOrTplName.Trim() && q.StartVal <= weight && q.EndVal > weight && q.EffectiveStartTime <= gpod.ActualDeliveryDate.ObjectToDateTime()
                                                        && (q.EffectiveEndTime > gpod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                if (price == null)
                {
                    this.GenErrorMessage_ByGroupedPod(gpod, "无系统报价,请先配置报价.");
                    continue;
                }

                this.settledPodCollection.Each((i, settledPod) =>
                    {
                        var originalPodID = from q in innerPodID where q == settledPod.PodID.ToString() select q;

                        if (originalPodID.Count() > 0)
                        {
                            if (settledPod.Weight == null)
                            {
                                this.GenErrorMessage_ByGroupedPod(gpod, "请先设置货物的重量");
                            }
                            else
                            {
                                //出货运单
                                if (originalPodID.First().ToString() == settledPod.PodID.ToString() && settledPod.PODTypeName == "出货运单")
                                {
                                    //合并总重小于300，按300算，运费为每单重量占比*价格*300
                                    //if (gpod.Weight < 300)
                                    //{
                                    //    settledPod.ShipAmt = (decimal)(settledPod.Weight / gpod.Weight) * price.Price * 300;
                                    //}
                                    //else
                                    //{
                                    //    settledPod.ShipAmt = (decimal)settledPod.Weight * price.Price;
                                    //}
                                    settledPod.ShipAmt = gpod.Weight < 300 ? (decimal)(settledPod.Weight / gpod.Weight) * price.Price * 300 : (decimal)settledPod.Weight * price.Price;
                                    settledPod.BAFAmt = 0;
                                    settledPod.Str4 = price.Price.ToString();
                                }
                                //转仓运单
                                if (originalPodID.First().ToString() == settledPod.PodID.ToString() && settledPod.PODTypeName == "移库调拨")
                                {
                                    settledPod.ShipAmt = (decimal)settledPod.Weight * price.Price;
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