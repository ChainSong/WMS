using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Common;
using Runbow.TWS.Web.Implement;

namespace Runbow.TWS.Web.SettledMethod
{
    public class SettledForNikePayInRunbowProject : BaseSettled
    {
        public SettledForNikePayInRunbowProject(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator, RelatedCustomerID)
        {

        }


        public override void CustomerDefinedSettledPod()
        {
            foreach (var gPod in SettledPodResponse.GroupedPods)
            {
                #region 出货运单
                if (gPod.PODTypeName.Trim() != "门店调拨" && gPod.PODTypeName.Trim() != "退货运单" && gPod.ShipperTypeName.Trim() != "空运")
                {
                    double volume;
                    //山晓，浩发，建民  体积=箱数/11
                    if (gPod.TargetName.Trim() == "山晓物流" || gPod.TargetName.Trim() == "浩发物流" || gPod.TargetName.Trim() == "建民物流")
                    {
                        volume = gPod.BoxNumber / 11;
                    }
                    //其他 体积= 箱数/12
                    else
                    {
                        volume = gPod.BoxNumber / 12;
                    }
                    var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());

                    var price = this.quotedPriceCollection.FirstOrDefault(q => q.ProjectID == 1 && q.Target == 1 && q.TargetID == gPod.TargetID && q.TargetName.Trim() == gPod.TargetName.Trim()
                                                                           && q.StartVal < volume && q.EndVal >= volume && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                                                                           && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim()
                                                                           && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null)
                                                                           && q.RelatedCustomerID == this.relatedCustomerID);

                    if (price == null)
                    {
                        this.GenErrorMessage_ByGroupedPod(gPod, "无系统报价，请先配置报价");
                        continue;
                    }

                    this.settledPodCollection.Each((i, settledPod) =>
                        {
                            var originalPodIDs = from q in innerPodIDs where q == settledPod.PodID select q;
                            if (originalPodIDs.Count() > 0)
                            {
                                if (settledPod.BoxNumber == null)
                                {
                                    this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                                }
                                else
                                {
                                    if (originalPodIDs.First().ToString() == settledPod.PodID.ToString())
                                    {
                                        settledPod.ShipAmt = price.Price * (decimal)volume;
                                        settledPod.OtherAmt = settledPod.CustomerOrShipperName != "鸿全物流" ? (decimal)volume * 10 : (decimal)volume * 15;
                                        settledPod.Str4 = price.Price.ToString();
                                    }
                                }
                            }

                        });
                }
                #endregion

                #region 退货运单
                //退货和出货方法一样，支出没有补贴
                if (gPod.PODTypeName.Trim() == "退货运单" && gPod.ShipperTypeName.Trim() != "空运")
                {
                    double volume;
                    //山晓，浩发，建民  体积=箱数/11
                    if (gPod.TargetName.Trim() == "山晓物流" || gPod.TargetName.Trim() == "浩发物流" || gPod.TargetName.Trim() == "建民物流")
                    {
                        volume = gPod.BoxNumber / 11;
                    }
                    //其他 体积= 箱数/12
                    else
                    {
                        volume = gPod.BoxNumber / 12;
                    }

                    var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());

                    var price = this.quotedPriceCollection.FirstOrDefault(q => q.ProjectID == 1 && q.Target == 1 && q.TargetID == gPod.TargetID && q.TargetName.Trim() == gPod.TargetName.Trim()
                                                                            && q.StartVal < volume && q.EndVal >= volume && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                                                                            && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim()
                                                                            && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null)
                                                                            && q.RelatedCustomerID == this.relatedCustomerID);

                    if (price == null)
                    {
                        this.GenErrorMessage_ByGroupedPod(gPod, "无系统报价，请先配置报价");
                        continue;
                    }

                    this.settledPodCollection.Each((i, settledPod) =>
                    {
                        var originalPodIDs = from q in innerPodIDs where q == settledPod.PodID select q;
                        if (originalPodIDs.Count() > 0)
                        {
                            if (settledPod.BoxNumber == null)
                            {
                                this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                            }
                            else
                            {
                                if (originalPodIDs.First().ToString() == settledPod.PodID.ToString())
                                {
                                    settledPod.ShipAmt = price.Price * (decimal)volume;
                                    settledPod.Str4 = price.Price.ToString();
                                }
                            }
                        }

                    });
                }
                #endregion

                #region 门店调拨
                //方法与出货方法一样，单价根据装货地址和卸货地址取得单价，当没有单价时，取收入部分的单价*0.95作为支出单价
                if (gPod.PODTypeName.Trim() == "门店调拨" && gPod.ShipperTypeName.Trim() != "空运")
                {
                    double volume;
                    if (gPod.TargetName.Trim() == "山晓物流" || gPod.TargetName.Trim() == "浩发物流" || gPod.TargetName.Trim() == "建民物流")
                    {
                        volume = gPod.BoxNumber / 11;
                    }
                    //其他 体积= 箱数/12
                    else
                    {
                        volume = gPod.BoxNumber / 12;
                    }

                    var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());

                    decimal unitPrice = 0;
                    var price = this.quotedPriceCollection.FirstOrDefault(q => q.ProjectID == 1 && q.Target == 1 && q.TargetID == gPod.TargetID && q.TargetName.Trim() == gPod.TargetName.Trim()
                                                                            && q.StartVal < volume && q.EndVal >= volume && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                                                                            && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim()
                                                                            && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null)
                                                                            && q.RelatedCustomerID == this.relatedCustomerID);
                    //调拨无单价
                    if (price == null)
                    {
                        //收入部分 Target = 0
                        unitPrice = this.quotedPriceCollection.FirstOrDefault(q => q.ProjectID == 1 && q.Target == 0 && q.TargetID == gPod.TargetID && q.TargetName.Trim() == gPod.TargetName.Trim()
                                                                            && q.StartVal < volume && q.EndVal >= volume && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                                                                            && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim()
                                                                            && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null)
                                                                            && q.RelatedCustomerID == this.relatedCustomerID).Price * (decimal)0.95;
                    }
                    else
                    {   
                        unitPrice = price.Price;
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
                                        settledPod.ShipAmt = unitPrice * (decimal)settledPod.BoxNumber;
                                        settledPod.Str4 = unitPrice.ToString();
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