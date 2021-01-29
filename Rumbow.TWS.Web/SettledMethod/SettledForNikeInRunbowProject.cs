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
    public class SettledForNikeInRunbowProject : BaseSettled
    {
        public SettledForNikeInRunbowProject(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator, RelatedCustomerID)
        {

        }

        public override void CustomerDefinedSettledPod()
        {
            foreach (var gPod in SettledPodResponse.GroupedPods)
            {
                #region 公路/陆运
                #region 大仓出货和工厂直发
                if (gPod.PODTypeName.Trim() != "退货运单" && gPod.PODTypeName.Trim() != "门店调拨" && gPod.ShipperTypeName.Trim() != "空运")
                {
                    #region 太仓和上海
                    //起运城市为太仓CLC，太仓CRW，上海,按同天同城结算（出货运单和大仓出货和工厂直发）
                    if (gPod.StartCityName.Trim().Substring(0, 2) == "太仓" || gPod.StartCityName.Trim() == "上海")
                    {
                        //体积=箱数/11
                        var volume = gPod.BoxNumber / 11;
                        var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                        var startcityname = gPod.StartCityName.Trim().Substring(0, 2) == "太仓" ? "太仓" : "上海";
                        //&& q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim()                                                     
                        var price = this.quotedPriceCollection.FirstOrDefault(q =>
                                                                              q.ProjectID == 1 && q.Target == 0 && q.TargetID == gPod.TargetID && q.StartCityName.Trim() == startcityname
                                                                              && q.EndCityName.Trim() == gPod.EndCityName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim()
                                                                              && q.PodTypeName.Trim() != "退货运单" && q.PodTypeName.Trim() != "门店调拨"
                                                                              && q.StartVal < volume && q.EndVal >= volume && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime()
                                                                              && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                        if (price == null)
                        {
                            this.GenErrorMessage_ByGroupedPod(gPod, "无系统对应报价，请先配置报价");
                            continue;
                        }

                        this.settledPodCollection.Each((i, settledPod) =>
                            {
                                var originalPodIDs = from q in innerPodIDs where q == settledPod.PodID select q;
                                if (originalPodIDs.Count() > 0)
                                {
                                    if (settledPod.BoxNumber == null)
                                    {
                                        this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的箱数");
                                    }
                                    else
                                    {
                                        if (originalPodIDs.First().ToString() == settledPod.PodID.ToString())
                                        {
                                            //运费 成本单价*箱数/11*97%
                                            settledPod.ShipAmt = price.Price * (decimal)(settledPod.BoxNumber / 11) * (decimal)0.97;
                                            //提货补贴 1.56*箱数
                                            settledPod.OtherAmt = (decimal)(1.56 * settledPod.BoxNumber);
                                            settledPod.Str4 = price.Price.ToString();
                                        }
                                    }
                                }
                            });
                    }
                    #endregion
                    #region 广州
                    //起运城市为广州，按同天同城结算（工厂直发）广州没有提货补贴
                    else
                    {
                        if (gPod.StartCityName.Trim() == "广州")
                        {
                            //体积=箱数/11
                            var volume = gPod.BoxNumber / 11;
                            var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                            //&& q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim()
                            var price = this.quotedPriceCollection.FirstOrDefault(q =>
                                                                                  q.ProjectID == 1 && q.Target == 0 && q.TargetID == gPod.TargetID && q.StartCityName.Trim() == gPod.StartCityName.Trim()
                                                                                  && q.EndCityName.Trim() == gPod.EndCityName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim()
                                                                                  && q.PodTypeName.Trim() != "退货运单" && q.PodTypeName.Trim() != "门店调拨"
                                                                                  && q.StartVal < volume && q.EndVal >= volume && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime()
                                                                                  && (q.EffectiveEndTime > gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                            if (price == null)
                            {
                                this.GenErrorMessage_ByGroupedPod(gPod, "无系统对应报价，请先配置报价");
                                continue;
                            }

                            this.settledPodCollection.Each((i, settledPod) =>
                            {
                                var originalPodIDs = from q in innerPodIDs where q == settledPod.PodID select q;
                                if (originalPodIDs.Count() > 0)
                                {
                                    if (settledPod.BoxNumber == null)
                                    {
                                        this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的箱数");
                                    }
                                    else
                                    {
                                        if (originalPodIDs.First().ToString() == settledPod.PodID.ToString())
                                        {
                                            //运费 成本单价*箱数/11*97%
                                            settledPod.ShipAmt = price.Price * (decimal)(settledPod.BoxNumber / 11) * (decimal)0.97;
                                            settledPod.Str4 = price.Price.ToString();
                                        }
                                    }
                                }
                            });
                        }
                    }
                    #endregion
                }
                #endregion
                #region 退货
                if (gPod.PODTypeName.Trim() == "退货运单" && gPod.ShipperTypeName != "空运")
                {
                    var volume = gPod.BoxNumber / 11;
                    var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());

                    var price = this.quotedPriceCollection.FirstOrDefault(q =>
                                                                            q.ProjectID == 1 && q.Target == 0 && q.TargetID == gPod.TargetID && q.StartCityName.Trim() == gPod.StartCityName.Trim()
                                                                            && q.EndCityName.Trim() == gPod.EndCityName.Trim() && q.PodTypeName.Trim() == gPod.PODTypeName.Trim()
                                                                            && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim()
                                                                            && q.StartVal < volume && q.EndVal >= volume && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime()
                                                                            && (q.EffectiveEndTime > gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                    if (price == null)
                    {
                        this.GenErrorMessage_ByGroupedPod(gPod, "无系统对应报价，请先配置报价");
                        continue;
                    }

                    this.settledPodCollection.Each((i, settledPod) =>
                    {
                        var originalPodIDs = from q in innerPodIDs where q == settledPod.PodID select q;
                        if (originalPodIDs.Count() > 0)
                        {
                            if (settledPod.BoxNumber == null)
                            {
                                this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的箱数");
                            }
                            else
                            {
                                if (originalPodIDs.First().ToString() == settledPod.PodID.ToString())
                                {
                                    //运费 成本单价*箱数/11*97%
                                    settledPod.ShipAmt = price.Price * (decimal)(settledPod.BoxNumber / 11) * (decimal)0.97;
                                    //提货补贴 1.56*箱数
                                    settledPod.OtherAmt = (decimal)(1.56 * settledPod.BoxNumber);
                                    settledPod.Str4 = price.Price.ToString();
                                }
                            }
                        }
                    });
                }
                #endregion
                #region 门店调拨
                //门店调拨，按同天同店到同城算，取成本单价与箱数相乘
                if (gPod.PODTypeName.Trim() == "门店调拨" && gPod.ShipperTypeName != "空运")
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
                        var innerPodIDs = sPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                        var boxnumber = sPod.BoxNumber;

                        var price = this.quotedPriceCollection.FirstOrDefault(q => q.ProjectID == 1 && q.Target == 0 && q.TargetID == sPod.TargetID
                                                                                && q.StartCityName.Trim() == sPod.StartCityName.Trim() && q.EndCityName.Trim() == sPod.EndCityName.Trim()
                                                                                && q.PodTypeName.Trim() == sPod.PODTypeName.Trim() && q.StartVal < boxnumber && boxnumber <= q.EndVal && q.EffectiveStartTime <= sPod.ActualDeliveryDate.ObjectToDateTime()
                                                                                && (q.EffectiveEndTime >= sPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                        if (price == null)
                        {
                            this.GenErrorMessage_ByGroupedPod(sPod, "无系统对应报价，请先配置报价");
                            continue;
                        }

                        this.settledPodCollection.Each((i, settledPod) =>
                            {
                                var originalPodIDs = from q in innerPodIDs where q == settledPod.PodID select q;
                                if (originalPodIDs.Count() > 0)
                                {
                                    if (settledPod.BoxNumber == null)
                                    {
                                        this.GenErrorMessage_ByGroupedPod(sPod, "请先设置货物的箱数");
                                    }
                                    else
                                    {
                                        if (originalPodIDs.First().ToString() == settledPod.PodID.ToString())
                                        {
                                            settledPod.ShipAmt = price.Price * (decimal)settledPod.BoxNumber;
                                            settledPod.Str4 = price.Price.ToString();
                                        }
                                    }
                                }
                            });
                    }
                }
                #endregion

                #endregion

                #region 空运
                //空运 根据重量定位成本单价
                if (gPod.ShipperTypeName.Trim() == "空运")
                {
                    var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());

                    this.settledPodCollection.Each((i, settledPod) =>
                        {
                            var originalPodIDs = from q in innerPodIDs where q == settledPod.PodID select q;
                            if (originalPodIDs.Count() > 0)
                            {
                                if (settledPod.BoxNumber == null)
                                {
                                    this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的箱数");
                                }
                                else
                                {
                                    if (originalPodIDs.First().ToString() == settledPod.PodID.ToString())
                                    {
                                        double airPrice = 0;
                                        //重量= 1000/6*箱数/11
                                        var airWeight = (1000 / 6) * (double)(settledPod.BoxNumber / 11);
                                        if (airWeight <= 100)
                                        {
                                            airPrice = 16.64;
                                        }
                                        if (airWeight > 500)
                                        {
                                            airPrice = 11.44;
                                        }
                                        else
                                        {
                                            airPrice = 13.52;
                                        }
                                        //运费 成本单价*重量*97%
                                        settledPod.ShipAmt = (decimal)(airPrice * airWeight * 0.97);
                                        //提货补贴 1.56*箱数
                                        settledPod.OtherAmt = (decimal)(1.56 * settledPod.BoxNumber);
                                        settledPod.Str4 = airPrice.ToString();
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