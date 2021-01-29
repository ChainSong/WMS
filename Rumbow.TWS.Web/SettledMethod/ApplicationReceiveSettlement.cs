using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Web.Implement;
using Runbow.TWS.Common;
using System.Text;

namespace Runbow.TWS.Web.SettledMethod
{
    public class ApplicationReceiveSettlement : BaseSettled
    {
        public ApplicationReceiveSettlement(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator, RelatedCustomerID)
        {

        }

        public override void CustomerDefinedSettledPod()
        {
            foreach (var gpod in this.SettledPodResponse.GroupedPods)
            {
                var weight = gpod.Weight;

                var innerPodIDs = gpod.PodIDs.Split('|').Select(i => i.ObjectToInt64());

                var price = this.quotedPriceCollection.FirstOrDefault(q => q.ProjectID == 1 && q.Target == 0 && q.TargetID == gpod.TargetID && q.StartCityName.Trim() == gpod.StartCityName.Trim()
                    && q.EndCityName.Trim() == gpod.EndCityName.Trim() && q.PodTypeName.Trim() == gpod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gpod.ShipperTypeName.Trim() && q.TplOrTtlName == gpod.TtlOrTplName.Trim() && q.StartVal <= gpod.Weight && q.EndVal >= gpod.Weight
                    && q.EffectiveStartTime <= gpod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gpod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));

                if (price == null)
                {
                    this.GenErrorMessage_ByGroupedPod(gpod, "无系统报价，请先设置报价");
                    continue;
                }

                this.settledPodCollection.Each((i, settledPod) =>
                    {
                        var originalPodIDs = from q in innerPodIDs where q == settledPod.PodID select q;
                        if (originalPodIDs.Count() > 0)
                        {
                            if (settledPod.Weight == null)
                            {
                                this.GenErrorMessage_ByGroupedPod(gpod, "请先设置货物的重量");
                            }
                            else
                            {
                                if (originalPodIDs.First().ToString() == settledPod.PodID.ToString())
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