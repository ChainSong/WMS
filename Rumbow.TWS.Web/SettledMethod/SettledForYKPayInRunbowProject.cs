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
    public class SettledForYKPayInRunbowProject : BaseSettled
    {
        public SettledForYKPayInRunbowProject(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator, RelatedCustomerID)
        {

        }

        public override void CustomerDefinedSettledPod()
        {
            foreach (var gPod in this.SettledPodResponse.GroupedPods)
            {
                #region OLD
                //var weight = gPod.Weight;
                //string[] innerPodIDs = gPod.PodIDs.Split('|');

                //if (weight <= 200)
                //{
                //    weight = 199;
                //}

                //var price = this.quotedPriceCollection.FirstOrDefault(q => q.ProjectID == 1 && q.Target == 1 && q.TargetID == gPod.TargetID && q.TargetName == gPod.TargetName
                //                                                       && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                //                                                       && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim()
                //                                                       && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < weight && q.EndVal >= weight
                //                                                       && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime > gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                //if (price == null)
                //{ 
                //    this.GenErrorMessage_ByGroupedPod(gPod,"无系统报价,请先配置报价.");
                //    continue;
                //}

                ////合并运单重量小于200，只记一单费用，其他都为0
                //if (gPod.Weight <= 200)
                //{
                //    for (int i = 0; i <innerPodIDs.Length; i++)
                //    {
                //        this.settledPodCollection.Each((s, settledPod) =>
                //            {
                //                if (i < 1)
                //                {
                //                    if (innerPodIDs[i].ToString() == settledPod.PodID.ToString())
                //                    {
                //                        if (settledPod.Weight == null)
                //                        {
                //                            this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                //                        }
                //                        else
                //                        {
                //                            settledPod.ShipAmt = price.Price;
                //                            settledPod.BAFAmt = 0;
                //                            settledPod.Str4 = price.Price.ToString();
                //                        }
                //                    }
                //                }
                //                else
                //                {
                //                    if (innerPodIDs[i].ToString() == settledPod.PodID.ToString())
                //                    {
                //                        if (settledPod.Weight == null)
                //                        {
                //                            this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                //                        }
                //                        else
                //                        {
                //                            settledPod.ShipAmt = 0;
                //                            settledPod.BAFAmt = 0;
                //                            settledPod.Str4 = price.Price.ToString();
                //                        }
                //                    }
                //                }

                //            });
                //    }
                //}
                //else
                //{
                //    this.settledPodCollection.Each((s, settltedPod) =>
                //        {
                //            var originalPodID = from q in innerPodIDs where q == settltedPod.PodID.ToString() select q;
                //            if (originalPodID.Count() > 0)
                //            {
                //                if (settltedPod.Weight == null)
                //                {
                //                    this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                //                }
                //                else
                //                {
                //                    if (originalPodID.First().ToString() == settltedPod.PodID.ToString())
                //                    {
                //                        settltedPod.ShipAmt = price.Price * (decimal)settltedPod.Weight;
                //                        settltedPod.BAFAmt = 0;
                //                        settltedPod.Str4 = price.Price.ToString();
                //                    }
                //                }
                //            }
                //        });
                //}

                #endregion

                #region 改
                #region
                //var podIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());

                ////同天同城同点
                //var groupPodResponse = new PodService().SettledPodByAddress(new SettledPodRequest() { IDs = podIDs, SettledType = this.settledType });

                //if (!groupPodResponse.IsSuccess)
                //{
                //    throw groupPodResponse.Exception;
                //}
                //var settledPodResponse = groupPodResponse.Result;

                //foreach (var group in settledPodResponse.GroupedPods)
                //{
                #endregion
                var weight = gPod.Weight;
                if (weight <= 200)
                {
                    weight = 199;
                }
                var ids = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());

                var price = this.quotedPriceCollection.FirstOrDefault(q =>
                                                                    q.ProjectID == 1 && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == this.relatedCustomerID && q.StartCityName.Trim() == gPod.StartCityName.Trim()
                                                                    && q.EndCityName.Trim() == gPod.EndCityName.Trim() && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim()
                                                                    && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < weight && q.EndVal >= weight && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime()
                                                                    && (q.EffectiveEndTime > gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                if (price == null)
                {
                    this.GenErrorMessage_ByGroupedPod(gPod, "无系统报价，请先配置报价");
                    continue;
                }

                //合并重量小于200，多订单只有一个价
                if (gPod.Weight <= 200)
                {
                    List<string> str = new List<string>();
                    #region
                    //foreach (var item in ids)
                    //{
                    //    var address = this.podCollection.First(p => p.ID == item);

                    //    this.settledPodCollection.Each((i, settledPod) =>
                    //        {
                    //            if (item.ToString() == settledPod.PodID.ToString())
                    //            {
                    //                if (settledPod.Weight == null)
                    //                {
                    //                    this.GenErrorMessage_ByGroupedPod(group, "请先设置货物的重量");
                    //                }
                    //                else
                    //                {
                    //                    settledPod.ShipAmt = !str.Contains(address.Str36) ? price.Price : 0;
                    //                    settledPod.BAFAmt = 0;
                    //                    settledPod.Str4 = price.Price.ToString();
                    //                }
                    //            }
                    //        });
                    //    str.Add(address.Str36);
                    //}
                    #endregion
                    this.settledPodCollection.Each((i, settledPod) =>
                        {
                            var originalPodIDs = from q in ids where q == settledPod.PodID select q;
                            if (originalPodIDs.Count() > 0)
                            {
                                var address = this.podCollection.First(q => q.ID.ToString() == originalPodIDs.First().ToString());
                                if (address.Str36 != null)
                                {
                                    if (settledPod.Weight == null)
                                    {
                                        this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                                    }
                                    else
                                    {
                                        if (originalPodIDs.First().ToString() == settledPod.PodID.ToString())
                                        {
                                            settledPod.ShipAmt = !str.Contains(address.Str36) ? price.Price : 0;
                                            settledPod.BAFAmt = 0;
                                            settledPod.Str4 = price.Price.ToString();
                                        }
                                    }
                                    str.Add(address.Str36);
                                }
                                else
                                {
                                    this.GenErrorMessage_ByGroupedPod(gPod, "运单无地址，请先配置地址");
                                }
                            }
                        });
                }
                else
                {
                    this.settledPodCollection.Each((s, settledPod) =>
                    {
                        var originalPodID = from q in ids where q == settledPod.PodID select q;
                        if (originalPodID.Count() > 0)
                        {
                            var address = this.podCollection.First(q => q.ID.ToString() == originalPodID.First().ToString());
                            if (address.Str36 != null)
                            {
                                if (settledPod.Weight == null)
                                {
                                    this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                                }
                                else
                                {
                                    if (originalPodID.First().ToString() == settledPod.PodID.ToString())
                                    {
                                        settledPod.ShipAmt = price.Price * (decimal)settledPod.Weight;
                                        settledPod.BAFAmt = 0;
                                        settledPod.Str4 = price.Price.ToString();
                                    }
                                }
                            }
                            else
                            {
                                this.GenErrorMessage_ByGroupedPod(gPod, "运单无地址，请先配置地址");
                            }
                        }
                    });
                }
                //}
                #endregion
            }
        }
    }
}