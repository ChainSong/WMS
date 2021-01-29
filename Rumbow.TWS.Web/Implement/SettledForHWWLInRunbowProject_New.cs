using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using System.Web;
using Runbow.TWS.Entity;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Implement
{
    /// <summary>
    /// 成都、淘宝akzo  汇维物流 
    /// </summary>
    public class SettledForHWWLInRunbowProject_New : BaseSettled
    {
        public SettledForHWWLInRunbowProject_New(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
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

                    //目的地四川成都那报价是0.093  否则就要按照条件获取报价


                    var price = quotedPriceCollection.FirstOrDefault(q =>
                      q.ProjectID == projectID && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == relatedCustomerID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                      && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal <= weight && q.EndVal > weight
                      && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));

                    if (price == null && gPod.EndCityName != "成都")
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
                                if (originalPod.First().ToString() == settledPod.PodID.ToString())
                                {
                                    if (settledPod.EndCityName == "成都")
                                    {
                                        settledPod.ShipAmt = (decimal)settledPod.Weight * (decimal)0.093; ;
                                        settledPod.Str4 = Convert.ToString(0.093);
                                    }
                                    else
                                    {
                                        if (gPod.Weight < 50)
                                        {
                                            settledPod.ShipAmt = (decimal)(settledPod.Weight / gPod.Weight) * 50 * price.Price;
                                        }
                                        else
                                        {
                                            settledPod.ShipAmt = (decimal)settledPod.Weight * price.Price;
                                        }
                                        settledPod.Str4 = price.Price.ToString();
                                    }
                                    settledPod.BAFAmt = 0;
                                }
                            }
                        }
                    });
                }
            }
            #endregion

            #region   淘宝akzo
            if (base.podCollection.First().CustomerID == 10)
            {
                relatedCustomerID = 10;
                 
                foreach (var gPod in SettledPodResponse.GroupedPods)
                {
                    var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());

                    var price = quotedPriceCollection.FirstOrDefault(q =>
                      q.ProjectID == projectID && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == relatedCustomerID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                      && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim()
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
                                if (originalPod.First().ToString() == settledPod.PodID.ToString())
                                {
                                    //如果小于50kg 运费=基价   否则运费=基价+续价*（重量-50）
                                    if (settledPod.Weight <= 50)
                                    {
                                        settledPod.ShipAmt = price.Price;
                                    }
                                    else
                                    {
                                        settledPod.ShipAmt = price.Price + price.Point * (decimal)(settledPod.Weight - 50);
                                    }
                                    settledPod.Str4 = price.Price.ToString();
                                    settledPod.BAFAmt = 0;
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