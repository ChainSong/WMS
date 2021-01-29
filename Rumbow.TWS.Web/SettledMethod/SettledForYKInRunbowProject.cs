using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Web.Implement;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.Biz;
using Runbow.TWS.MessageContracts;

namespace Runbow.TWS.Web.SettledMethod
{
    public class SettledForYKInRunbowProject : BaseSettled
    {
        public SettledForYKInRunbowProject(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator, RelatedCustomerID)
        {

        }

        public override void CustomerDefinedSettledPod()
        {
            foreach (var gPod in base.SettledPodResponse.GroupedPods)
            {
                #region OLD
                //double weight = gPod.Weight;

                //if (weight <= 200)
                //{
                //    weight = 199;
                //}
                //string[] innerPodIDs = gPod.PodIDs.Split('|');

                //var price = quotedPriceCollection.FirstOrDefault(q =>
                //                        q.ProjectID == projectID && q.Target == 0 && q.TargetID == gPod.TargetID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                //                        && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < weight && q.EndVal >= weight
                //                        && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                //if (price == null)
                //{
                //    this.GenErrorMessage_ByGroupedPod(gPod, "无系统报价,请先配置报价.");
                //    continue;
                //}
                ////同天同地点
                ////合并运单，单条重量小于等于200，总价为0-200区间报价；多条合并运单，总重量小于等于200，只取一条，总价为0-200区间报价，其余多条总价都为0；
                //if (gPod.Weight <= 200)
                //{
                //    #region OLD
                //    //int length = innerPodIDs.Length;

                //    //if (length > 1)
                //    //{
                //    //    #region
                //    //    for (int i = 0; i < length; i++)
                //    //    {
                //    //        if (i < 1)
                //    //        {
                //    //            this.settledPodCollection.Each((k, settledPod) =>
                //    //                {
                //    //                    var ids = Convert.ToInt64(innerPodIDs[i].ToString());
                //    //                    if (ids == settledPod.PodID)
                //    //                    {
                //    //                        if (settledPod.Weight == null)
                //    //                        {
                //    //                            this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                //    //                        }
                //    //                        else
                //    //                        {
                //    //                            settledPod.ShipAmt = price.Price;
                //    //                            settledPod.BAFAmt = 0;
                //    //                            settledPod.Str4 = price.Price.ToString();
                //    //                        }
                //    //                    }
                //    //                });
                //    //        }
                //    //        else
                //    //        {
                //    //            this.settledPodCollection.Each((k, settledPod) =>
                //    //            {
                //    //                var ids = Convert.ToInt64(innerPodIDs[i].ToString());
                //    //                if (ids == settledPod.PodID)
                //    //                {
                //    //                    if (settledPod.Weight == null)
                //    //                    {
                //    //                        this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                //    //                    }
                //    //                    else
                //    //                    {
                //    //                        settledPod.ShipAmt = 0;
                //    //                        settledPod.BAFAmt = 0;
                //    //                        settledPod.Str4 = price.Price.ToString();
                //    //                    }
                //    //                }
                //    //            });
                //    //        }
                //    //    }
                //    //    #endregion
                //    //}
                //    //else
                //    //{
                //    //    this.settledPodCollection.Each((k, settledPod) =>
                //    //    {
                //    //        var originalID = from q in innerPodIDs where Convert.ToInt64(q) == settledPod.PodID select q;
                //    //        if (originalID.Count() > 0)
                //    //        {
                //    //            if (settledPod.Weight == null)
                //    //            {
                //    //                this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                //    //            }
                //    //            else
                //    //            {
                //    //                if (originalID.First().ToString() == settledPod.PodID.ToString())
                //    //                {
                //    //                    settledPod.ShipAmt = price.Price;
                //    //                    settledPod.BAFAmt = 0;
                //    //                    settledPod.Str4 = price.Price.ToString();
                //    //                }
                //    //            }
                //    //        }

                //    //    });
                //    //}
                //    #endregion

                //    for (int i = 0; i < innerPodIDs.Length; i++)
                //    {
                //        this.settledPodCollection.Each((s, settledPod) =>
                //            {
                //                if (innerPodIDs[i].ToString() == settledPod.PodID.ToString())
                //                {
                //                    if (settledPod.Weight == null)
                //                    {
                //                        this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                //                    }
                //                    else
                //                    {
                //                        if (i < 1)
                //                        {
                //                            settledPod.ShipAmt = price.Price;
                //                            settledPod.BAFAmt = 0;
                //                            settledPod.Str4 = price.Price.ToString();
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
                //    //合并运单重量大于200，总价为每条运单乘以报价
                //    this.settledPodCollection.Each((i, settledPod) =>
                //        {
                //            var originalPod = from q in innerPodIDs where q == settledPod.PodID.ToString() select q;
                //            if (originalPod.Count() > 0)
                //            {
                //                if (settledPod.Weight == null)
                //                {
                //                    this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                //                }
                //                else
                //                {
                //                    if (originalPod.First().ToString() == settledPod.PodID.ToString())
                //                    {
                //                        settledPod.ShipAmt = (decimal)settledPod.Weight * price.Price;
                //                        settledPod.BAFAmt = 0;
                //                        settledPod.Str4 = price.Price.ToString();
                //                    }
                //                }
                //            }
                //        });
                //}
                #endregion
                #region 改
                #region
                //var ids = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());

                //同天同点合并
                //var groupPodResponse = new PodService().SettledPodByAddress(new SettledPodRequest() { SettledType = this.settledType, IDs = ids });

                //if (!groupPodResponse.IsSuccess)
                //{
                //    throw groupPodResponse.Exception;
                //}
                //var settledPodResponse = groupPodResponse.Result;

                //foreach (var item in settledPodResponse.GroupedPods)
                //{
                #endregion
                double weight = gPod.Weight;
                if (weight <= 200)
                {
                    weight = 199;
                }
                var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());

                var price = quotedPriceCollection.FirstOrDefault(q =>
                                                                    q.ProjectID == 1 && q.Target == 0 && q.TargetID == gPod.TargetID && q.StartCityName.Trim() == gPod.StartCityName.Trim()
                                                                    && q.EndCityName.Trim() == gPod.EndCityName.Trim() && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim()
                                                                    && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < weight && weight <= q.EndVal && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime()
                                                                    && (q.EffectiveEndTime > gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                if (price == null)
                {
                    this.GenErrorMessage_ByGroupedPod(gPod, "无系统报价，请先配置报价");
                    continue;
                }

                //合并重量小于200，多条运单只有一个运费
                if (gPod.Weight <= 200)
                {
                    List<string> str = new List<string>();
                    this.settledPodCollection.Each((i, settledPod) =>
                        {
                            var originalPodIDs = from q in innerPodIDs where q == settledPod.PodID select q;
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
                                    this.GenErrorMessage_ByGroupedPod(gPod, "运单无地址，无法结算");
                                }
                            }

                        });
                }
                else
                {
                    this.settledPodCollection.Each((i, settledPod) =>
                        {
                            var originalPodID = from q in innerPodIDs where q == settledPod.PodID select q;
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
                                            settledPod.ShipAmt = (decimal)settledPod.Weight * price.Price;
                                            settledPod.BAFAmt = 0;
                                            settledPod.Str4 = price.Price.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    this.GenErrorMessage_ByGroupedPod(gPod, "运单无地址，无法结算");
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