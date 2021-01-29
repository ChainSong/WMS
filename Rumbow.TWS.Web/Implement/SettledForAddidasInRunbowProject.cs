using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Runbow.TWS.Biz;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Web.Interface;
using Runbow.TWS.Common;
using System.Text.RegularExpressions;

namespace Runbow.TWS.Web.Implement
{
    //Demo
    public class SettledForAdidasInRunbowProject_New : BaseSettled
    {
        public SettledForAdidasInRunbowProject_New(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator, RelatedCustomerID)
        {

        }

        public override void CustomerDefinedSettledPod()
        {
            //Demo 对settledPodCollection中的运单更改他的结算费用信息
            this.settledPodCollection.Each((i, settledPod) =>
            {
                settledPod.ShipAmt = i;
                settledPod.BAFAmt = i;
            });

        }
    }

    public class SettledForAddidasInRunbowProject : ISettledForPod
    {
        private double specialDivisor = 11.4;
        private double commonDivisor = 11;
        private decimal point100 = 100;
        private decimal point80 = 80;

        private decimal point_from_suzhou = 100;
        private decimal point_from_shanghai = 200;
        public void SettledPodForReceive(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
        {
            if (Message.Length > 0)
            {
                return;
            }

            PodService service = new PodService();
            //forSettledPodsResponse 待结算运单列表
            var forSettledPodsResponse = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = PodIDs });

            if (!forSettledPodsResponse.IsSuccess)
            {
                throw forSettledPodsResponse.Exception;
            }
            //同天同城合并，取得合并运单列表 groupedPodsResponse
            var groupedPodsResponse = service.SettledPodSearch(new SettledPodRequest() { IDs = PodIDs, SettledType = 0 });
            if (!groupedPodsResponse.IsSuccess)
            {
                throw groupedPodsResponse.Exception;
            }

            var podsResponse = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = PodIDs });
            if (!podsResponse.IsSuccess)
            {
                throw podsResponse.Exception;
            }
            //取得报价
            var quotedPrice = ApplicationConfigHelper.GetProjectQuotedPrice(ProjectID, Target, CustomerOrShipperID, RelatedCustomerID);
            var BAF = ApplicationConfigHelper.GetBAFPrice(ProjectID);
            string settledNumber = Guid.NewGuid().ToString();
            IList<SettledPod> settledPods = new List<SettledPod>();
            foreach (var gPod in groupedPodsResponse.Result.GroupedPods)
            {

                decimal BAFPrice = 0;
                double cube = 0;
                var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                var tempOriginalPods = podsResponse.Result.Where(p => innerPodIDs.Contains(p.ID));
                tempOriginalPods.Each((i, p) =>
                {
                    int tempInt = 0;
                    double tempCube = 0;
                    if (string.Equals(p.PODTypeName, "出货运单", StringComparison.OrdinalIgnoreCase))
                    {
                        if (p.CustomerOrderNumber.Substring(0, 2) == "SP" || int.TryParse(p.CustomerOrderNumber.Substring(0, 1), out tempInt))
                        {
                            tempCube = p.BoxNumber.Value / specialDivisor;//11.4
                        }
                        else
                        {
                            tempCube = p.BoxNumber.Value / commonDivisor;//11
                        }
                    }
                    else
                    {
                        tempCube = p.BoxNumber.Value / commonDivisor;
                    }
                    cube += tempCube;
                    podsResponse.Result.First(c => c.ID == p.ID).Str40 = tempCube.ToString();
                });
                QuotedPrice price = quotedPrice.FirstOrDefault(q =>
                    q.ProjectID == ProjectID && q.Target == 0 && q.TargetID == gPod.TargetID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                    && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < cube && q.EndVal >= cube
                    && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                QueryBAFPrice BAFPrices = BAF.FirstOrDefault(q =>
                   q.ProjectID == ProjectID && q.BAFStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && q.BAFEndTime > gPod.ActualDeliveryDate.ObjectToDateTime());

                if (BAFPrices == null)
                {
                    BAFPrice = 0;
                }
                else
                {
                    BAFPrice = BAFPrices.BAFPrice;
                }
                if (price == null)
                {
                    var forSettledPodsTemp = forSettledPodsResponse.Result.Where(p => gPod.PodIDs.Split('|').Contains(p.ID.ToString()));
                    if (forSettledPodsTemp != null)
                    {
                        forSettledPodsTemp.Each((k, g) =>
                        {
                            Message.Append("运单:").Append(g.CustomerOrderNumber).Append(",对应客户为:").Append(g.CustomerName).Append(",起运城市为:")
                            .Append(g.StartCityName).Append(",目的城市为:").Append(g.EndCityName).Append(",运输类型为:").Append(g.ShipperTypeName)
                            .Append(",运单类型为:").Append(g.PODTypeName).Append(",整车/零担为:").Append(g.TtlOrTplName).Append(" 无系统对应报价,请先配置报价.").Append("*");
                        });
                    }
                    continue;
                }
                if (Message.Length > 0)
                {
                    continue;
                }
                decimal shipAmt = 0;
                shipAmt = price.Price * (decimal)cube;

                foreach (var id in innerPodIDs)
                {
                    var originalPod = podsResponse.Result.FirstOrDefault(p => p.ID == id);
                    SettledPod settledPod = new SettledPod()
                    {
                        ProjectID = originalPod.ProjectID,
                        CustomerOrderNumber = originalPod.CustomerOrderNumber,
                        SystemNumber = originalPod.SystemNumber,
                        PodID = id,
                        SettledNumber = settledNumber,
                        SettledType = 0,
                        CustomerOrShipperID = originalPod.CustomerID.Value,
                        CustomerOrShipperName = originalPod.CustomerName,
                        StartCityID = originalPod.StartCityID.Value,
                        StartCityName = originalPod.StartCityName,
                        EndCityID = originalPod.EndCityID.Value,
                        EndCityName = originalPod.EndCityName,
                        ShipperTypeID = originalPod.ShipperTypeID.Value,
                        ShipperTypeName = originalPod.ShipperTypeName,
                        PODTypeID = originalPod.PODTypeID.Value,
                        PODTypeName = originalPod.PODTypeName,
                        TtlOrTplID = originalPod.TtlOrTplID.Value,
                        TtlOrTplName = originalPod.TtlOrTplName,
                        ActualDeliveryDate = originalPod.ActualDeliveryDate.Value,
                        BoxNumber = originalPod.BoxNumber,
                        Weight = originalPod.Weight,
                        Volume = originalPod.Volume,
                        GoodsNumber = originalPod.GoodsNumber,
                        ShipAmt = shipAmt * (decimal)(originalPod.Str40.ObjectToDouble() / cube),
                        BAFAmt = BAFPrice * shipAmt * (decimal)(originalPod.Str40.ObjectToDouble() / cube),
                        PointAmt = 0,
                        OtherAmt = 0,
                        Amt1 = 0,
                        Amt2 = 0,
                        Amt3 = 0,
                        Amt4 = 0,
                        Amt5 = 0,
                        Str1 = string.Empty,
                        Str2 = string.Empty,
                        Str3 = string.Empty,
                        Str4 = price.Price.ToString(),
                        Str5 = string.Empty,
                        Remark = string.Empty,
                        DateTime1 = null,
                        DateTime2 = null,
                        CreateTime = DateTime.Now,
                        Creator = Creator,
                        InvoiceID = 0,
                        RelatedCustomerID = originalPod.CustomerID,
                        IsAudit = true
                    };

                    settledPods.Add(settledPod);
                }

                if (Message.Length > 0)
                {
                    continue;
                }

                if (string.Equals(gPod.PODTypeName.Trim(), "调拨运单", StringComparison.OrdinalIgnoreCase))
                {
                    var temp = (from p in podsResponse.Result.Where(k => innerPodIDs.Contains(k.ID)) group p by p.Str6 into g select new { g.Key, Pods = g });

                    if (temp != null)
                    {
                        temp.Each((i, k) =>
                        {
                            var innPodIDs = k.Pods.Select(p => p.ID);
                            if (k.Pods.First().CustomerOrderNumber.Substring(0, 2) == "BR" && (k.Pods.First().StartCityName.Trim() == "苏州" || k.Pods.First().EndCityName.Trim() == "苏州"))
                            {
                                //settledPods.Each((i1, k1) =>
                                //{
                                //    if (innPodIDs.Contains(k1.PodID))
                                //    {
                                settledPods.Each((i1, k1) => { if (innerPodIDs.Contains(k1.PodID)) { k1.PointAmt = 0; } });
                                settledPods.First(c => c.PodID == k.Pods.First().ID).PointAmt = point_from_suzhou;
                                settledPods.First(c => c.PodID == k.Pods.First().ID).Remark += "Adidas调拨运单，BR开头的按照苏州至全国，全国至苏州，按照同天同城同客户合并后每票100计算点费";
                                //k1.PointAmt = point_from_suzhou;//(decimal)(k1.BoxNumber / commonDivisor) * point_from_suzhou
                                //k1.Remark = "Adidas调拨运单，发货城市苏州，按照方*100计算点费";
                                //  }
                                //});
                            }
                            else if (k.Pods.First().CustomerOrderNumber.Substring(0, 2) == "SP" && (k.Pods.First().StartCityName.Trim() == "上海" || k.Pods.First().EndCityName.Trim() == "上海"))
                            {
                                settledPods.Each((i1, k1) => { if (innerPodIDs.Contains(k1.PodID)) { k1.PointAmt = 0; } });
                                settledPods.First(c => c.PodID == k.Pods.First().ID).PointAmt = point_from_shanghai;
                                settledPods.First(c => c.PodID == k.Pods.First().ID).Remark += "Adidas调拨运单，SP开头的按照上海至全国，全国至上海，按照同天同城同客户合并后每票100计算点费";
                            }
                            else if (k.Pods.First().CustomerOrderNumber.Substring(0, 3) == "TRS" || k.Pods.First().CustomerOrderNumber.Substring(0, 3) == "TRN")
                            {
                                settledPods.Each((i1, k1) =>
                                {
                                    if (innPodIDs.Contains(k1.PodID))
                                    {
                                        k1.PointAmt = (decimal)(k1.BoxNumber / commonDivisor) * point100;
                                        k1.Remark = "Adidas调拨运单，TRS开头和TRN开头，按照方*100计算点费";
                                    }
                                });
                            }
                            else if (k.Pods.First().CustomerOrderNumber.Substring(0, 3) == "SPOW")
                            {
                                settledPods.Each((i1, k1) =>
                                {
                                    if (innPodIDs.Contains(k1.PodID))
                                    {
                                        k1.PointAmt = (decimal)(k1.BoxNumber / commonDivisor) * point80;
                                        k1.Remark = "Adidas调拨运单，SPOW开头，按照方*100计算点费";
                                    }
                                });
                            }
                            //else if ((k.Pods.First().StartCityName.Trim() == "东莞" && k.Pods.First().EndCityName == "深圳")
                            //    || (k.Pods.First().StartCityName.Trim() == "深圳" && k.Pods.First().EndCityName == "东莞")
                            //    || (k.Pods.First().StartCityName.Trim() == "东莞" && k.Pods.First().EndCityName == "广州")
                            //    || (k.Pods.First().StartCityName.Trim() == "珠海" && k.Pods.First().EndCityName == "东莞")
                            //    )
                            //{
                            //    settledPods.Each((i1, k1) =>
                            //    {
                            //        if (innPodIDs.Contains(k1.PodID))
                            //        {
                            //            k1.PointAmt = (decimal)(k1.BoxNumber / commonDivisor) * 84;
                            //            k1.Remark = "Adidas调拨运单，发货城市" + k1.StartCityName + ",收货城市" + k1.EndCityName + "，按照方*84计算点费";
                            //        }
                            //    });
                            //}
                            else
                            {
                                settledPods.Each((i1, k1) =>
                                {
                                    if (innPodIDs.Contains(k1.PodID))
                                    {
                                        k1.PointAmt = (decimal)(k1.BoxNumber / commonDivisor) * point80;
                                        k1.Remark = "Adidas调拨运单，如无特殊，按照方*80计算点费";
                                    }
                                });
                            }

                        });
                    }
                }

                //同城同天同一家客户经销商点费为100元、自营店（客户简称为中文）原则上无点费
                if (string.Equals(gPod.PODTypeName.Trim(), "退货运单", StringComparison.OrdinalIgnoreCase))
                {
                    var temp = (from p in podsResponse.Result.Where(k => innerPodIDs.Contains(k.ID) && k.Str10 == "经销商" || (k.Str10 == "自营店" && k.Str7 == "淘宝" && k.Str7 == "北区卫星舱")) group p by p.Str2 into g select new { g.Key, Pods = g });
                    if (temp != null)
                    {
                        temp.Each((i, k) =>
                        {
                            var innPodIDs = k.Pods.Select(p => p.ID);
                            settledPods.Each((i1, k1) => { if (innerPodIDs.Contains(k1.PodID)) { k1.PointAmt = 0; } });
                            settledPods.First(c => c.PodID == k.Pods.First().ID).PointAmt = point100;
                            settledPods.First(c => c.PodID == k.Pods.First().ID).Remark += "Adidas退货，同天同城，点费100元。";
                        });
                    }
                }

                //出货 自营店（客户简称为中文）按方收取点费
                if (string.Equals(gPod.PODTypeName.Trim(), "出货运单", StringComparison.OrdinalIgnoreCase))
                {
                    var temp = podsResponse.Result.Where(p => innerPodIDs.Contains(p.ID) && p.Str10 == "自营店" && Regex.IsMatch(p.Str7.Substring(0, 1), @"^[\u4e00-\u9fa5]+$") && p.Str7 != "淘宝" && p.Str7 != "北区卫星舱" && (p.CustomerOrderNumber.Substring(0, 3) != "RBO" && p.CustomerOrderNumber.Substring(0, 2) != "OW" && p.CustomerOrderNumber.Substring(0, 2) != "ZD" && p.CustomerOrderNumber.Substring(0, 2) != "SC" && p.CustomerOrderNumber.Substring(0, 2) != "OB"));
                    if (temp != null)
                    {
                        temp.Each((i, k) =>
                        {
                            int tInt;
                            double cbm;
                            if (int.TryParse(k.CustomerOrderNumber.Substring(0, 1), out tInt))
                            {
                                cbm = k.BoxNumber.Value / specialDivisor;

                            }
                            else
                            {
                                cbm = k.BoxNumber.Value / commonDivisor;
                            }

                            settledPods.First(c => c.PodID == k.ID).PointAmt = point80 * (decimal)cbm;
                            settledPods.First(c => c.PodID == k.ID).Remark = "Adidas出货，自营店按方*80 收取点费";
                        });
                    }
                }
            }

            if (Message.Length > 0)
            {
                return;
            }
            else
            {
                settledPods.Each((i, p) => { p.TotalAmt = p.ShipAmt + p.PointAmt - p.BAFAmt + p.OtherAmt + p.Amt1 + p.Amt2 + p.Amt3 + p.Amt4 + p.Amt5; });
                new SettledService().SettlePods(new SettlePodsRequest() { SettledPods = settledPods, SettledType = 0 });
            }
        }

        public void SettledPodForPay(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
        {
            throw new NotImplementedException();
        }
    }
}