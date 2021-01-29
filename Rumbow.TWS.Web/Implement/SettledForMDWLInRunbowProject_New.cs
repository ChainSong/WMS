using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Web.Interface;
using Runbow.TWS.Common;
namespace Runbow.TWS.Web.Implement
{
    public class SettledForMDWLInRunbowProject_New: BaseSettled
    {
        public SettledForMDWLInRunbowProject_New(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator,long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator,RelatedCustomerID)
        { }
        //晋太渊物流
        public override void CustomerDefinedSettledPod()
        {
            foreach (var gPod in SettledPodResponse.GroupedPods)
            {
                double weight = gPod.Weight;
                if (weight < 200)
                {
                    weight = 1;
                }
                var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                var price = quotedPriceCollection.FirstOrDefault(q =>
                                    q.ProjectID == projectID && q.Target == 0 && q.TargetID == gPod.TargetID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                                    && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < weight && q.EndVal >= weight
                                    && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));

                if (price == null)
                {
                    this.GenErrorMessage_ByGroupedPod(gPod, "无系统报价，出货运单也无报价,请先配置报价.");
                    continue;
                }
                //向结算成功列表中插入数据   运费
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
                            var isCD = base.podCollection.Where(p => p.ID == settledPod.PodID).Select(a => a.Str18);

                            foreach (var item in isCD)
                            {
                                if (originalPod.First().ToString() == settledPod.PodID.ToString() && settledPod.PODTypeName == "移库调拨")
                                {
                                    settledPod.ShipAmt = (decimal)settledPod.Weight * price.Price;
                                    settledPod.BAFAmt = 0;
                                    settledPod.Str4 = price.Price.ToString();
                                }
                            }
                        }
                    }

                });
            }
        }
    }
}