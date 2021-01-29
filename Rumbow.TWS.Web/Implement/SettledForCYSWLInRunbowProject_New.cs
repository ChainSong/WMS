using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Common;

namespace Runbow.TWS.Web.Implement
{
    public class SettledForCYSWLInRunbowProject_New : BaseSettled
    {
        public SettledForCYSWLInRunbowProject_New(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator,RelatedCustomerID)
        {

        }
        public override void CustomerDefinedSettledPod()
        {
            long relatedCustomerID = 0;
            #region 成都akzo
            if (base.podCollection.First().CustomerID == 7)
            {
                relatedCustomerID = 7;
                foreach (var gPod in SettledPodResponse.GroupedPods)
                {
                    double weight = gPod.Weight;
                    if (weight < 50)
                    {
                        weight = 50;
                    }
                    var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());

                    var price = quotedPriceCollection.FirstOrDefault(q =>
                      q.ProjectID == projectID && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == relatedCustomerID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                      && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal <= weight && q.EndVal > weight
                      && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));

                    if (price == null)
                    {
                        this.GenErrorMessage_ByGroupedPod(gPod, "无系统报价，出货运单也无报价,请先配置报价.");
                        continue;
                    }
                    this.settledPodCollection.Each((i, settledPod) =>
                    {
                        var originalPod = from q in innerPodIDs where q == settledPod.PodID select q;
                        if (originalPod.Count() > 0)
                        {
                            if (settledPod.Weight == null)
                            {
                                this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                            }
                            else
                            {
                                //出货运单
                                if (originalPod.First().ToString() == settledPod.PodID.ToString() && settledPod.PODTypeName == "出货运单")
                                {
                                    settledPod.ShipAmt = gPod.Weight < 50 ? (decimal)(settledPod.Weight / gPod.Weight) * price.Price * 50 : (decimal)settledPod.Weight * price.Price;
                                    settledPod.BAFAmt = 0;
                                    settledPod.Str4 = price.Price.ToString();
                                }
                                //转仓单
                                if (originalPod.First().ToString() == settledPod.PodID.ToString() && settledPod.PODTypeName == "移库调拨")
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
            #endregion
        }
    }
}