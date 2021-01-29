using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Common;
using Runbow.TWS.Web.Implement;

namespace Runbow.TWS.Web.SettledMethod
{
    public class SettledForAkzoPayInRunbowProject : BaseSettled
    {
        public SettledForAkzoPayInRunbowProject(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator, RelatedCustomerID)
        {

        }


        public override void CustomerDefinedSettledPod()
        {
            foreach (var gPod in this.SettledPodResponse.GroupedPods)
            {
                #region 上海 供应商除去嘉庆和顶丰，方法和应收一样
                if (gPod.StartCityName == "上海" && gPod.PODTypeName == "出货运单")
                {
                    var weight = gPod.Weight;
                    if (weight < 300)
                    {
                        weight = 300;
                    }

                    var innerPodIDs = gPod.PodIDs.Split('|');

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


                    this.settledPodCollection.Each((i, settlePod) =>
                        {
                            var originalPodIDs = from q in innerPodIDs where q == settlePod.PodID.ToString() select q;
                            if (originalPodIDs.Count() > 0)
                            {
                                if (settlePod.Weight == null)
                                {
                                    this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                                }
                                else
                                {
                                    if (originalPodIDs.First().ToString() == settlePod.PodID.ToString())
                                    {
                                        settlePod.ShipAmt = gPod.Weight > 300 ? (decimal)settlePod.Weight * price.Price : (decimal)(settlePod.Weight / gPod.Weight) * price.Price * (decimal)300;
                                        settlePod.BAFAmt = 0;
                                        settlePod.Str4 = price.Price.ToString();
                                    }
                                }
                            }

                        });
                }
                #endregion
                #region 扬州 供应商除去顶丰，方法和应收一样
                if (gPod.StartCityName == "扬州" && gPod.PODTypeName == "出货运单")
                {
                    var weight = gPod.Weight;
                    if (weight < 50)
                    {
                        weight = 50;
                    }
                    var innerPodIDs = gPod.PodIDs.Split('|');

                    var price = quotedPriceCollection.FirstOrDefault(q =>
                                                                     q.ProjectID == 1 && q.Target == 1 && q.TargetID == gPod.TargetID && q.StartCityName.Trim() == gPod.StartCityName.Trim()
                                                            && q.EndCityName.Trim() == gPod.EndCityName.Trim() && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim()
                                                            && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal <= weight && q.EndVal > weight && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime()
                                                            && (q.EffectiveEndTime > gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));

                    if (price == null)
                    {
                        this.GenErrorMessage_ByGroupedPod(gPod, "无系统报价，请先配置报价.");
                        continue;
                    }

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
                                        settledPod.ShipAmt = gPod.Weight > 50 ? (decimal)settledPod.Weight * price.Price : (decimal)(settledPod.Weight / gPod.Weight) * price.Price * 50;
                                        settledPod.BAFAmt = 0;
                                        settledPod.Str4 = price.Price.ToString();
                                    }
                                }
                            }

                        });
                }
                #endregion
                #region 转仓
                else
                {
                    var weight = gPod.Weight;
                    var innerPodIDs = gPod.PodIDs.Split('|');

                    var price = quotedPriceCollection.FirstOrDefault(q =>
                                                                     q.ProjectID == 1 && q.Target == 1 && q.TargetID == gPod.TargetID && q.StartCityName.Trim() == gPod.StartCityName.Trim()
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
                                        settledPod.ShipAmt = (decimal)settledPod.Weight * price.Price;
                                        settledPod.BAFAmt = 0;
                                        settledPod.Str4 = price.Price.ToString();
                                    }
                                }
                            }
                        });
                }
                #endregion
            }
        }
    }
}