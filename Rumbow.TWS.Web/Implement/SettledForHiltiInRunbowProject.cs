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

namespace Runbow.TWS.Web.Implement
{
    public class SettledForHiltiInRunbowProject : ISettledForPod
    {
        private decimal _receiveForCheck = 50;
        private decimal _receiveForCache = 120;
        private double _settledByItem = 50;

        public void SettledPodForReceive(IEnumerable<long> PodIDs, string Creator,long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID,  StringBuilder Message)
        {
            if (Message.Length > 0)
            {
                return;
            }

            PodService service = new PodService();
            var podsResponse = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = PodIDs });
            if (!podsResponse.IsSuccess)
            {
                throw podsResponse.Exception;
            }

            var quotedPrice = ApplicationConfigHelper.GetProjectQuotedPrice(ProjectID,Target,CustomerOrShipperID,RelatedCustomerID);

            string settledNumber = Guid.NewGuid().ToString();

            IList<SettledPod> settledPods = new List<SettledPod>();

            foreach (var pod in podsResponse.Result)
            {
                if(string.IsNullOrEmpty(pod.Str9))
                {
                    this.GenErrorMessage(pod, Message, "毛重没有录入,请重新编辑运单");
                    continue;
                }

                double weight = 0;

                try
                {
                    weight = pod.Str9.ObjectToDouble();
                }
                catch
                {
                    this.GenErrorMessage(pod, Message, "毛重数据不是数字类型,请重新编辑运单");
                    continue;
                }

                int carType = 0;
                if (pod.TtlOrTplID == 27)
                {
                    if(string.IsNullOrEmpty(pod.Str29))
                    {
                        this.GenErrorMessage(pod, Message, "无整车吨位信息,请重新编辑运单");
                        continue;
                    }

                    try
                    {
                        carType = pod.Str29.ObjectToInt32();
                    }
                    catch
                    {
                        this.GenErrorMessage(pod, Message, "整车吨位信息有误（2|5|10|15|20),,请重新编辑运单");
                        continue;
                    }
                }

                SettledPod settledPod = new SettledPod()
                {
                    ProjectID = pod.ProjectID,
                    CustomerOrderNumber = pod.CustomerOrderNumber,
                    SystemNumber = pod.SystemNumber,
                    PodID = pod.ID,
                    SettledNumber = settledNumber,
                    SettledType = 0,
                    CustomerOrShipperID = pod.CustomerID.Value,
                    CustomerOrShipperName = pod.CustomerName,
                    StartCityID = pod.StartCityID.Value,
                    StartCityName = pod.StartCityName,
                    EndCityID = pod.EndCityID.Value,
                    EndCityName = pod.EndCityName,
                    ShipperTypeID = pod.ShipperTypeID.Value,
                    ShipperTypeName = pod.ShipperTypeName,
                    PODTypeID = pod.PODTypeID.Value,
                    PODTypeName = pod.PODTypeName,
                    TtlOrTplID = pod.TtlOrTplID.Value,
                    TtlOrTplName = pod.TtlOrTplName,
                    ActualDeliveryDate = pod.ActualDeliveryDate.Value,
                    BoxNumber = pod.BoxNumber,
                    Weight = weight,
                    Volume = pod.Volume,
                    GoodsNumber = pod.GoodsNumber,
                    BAFAmt = 0,
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
                    Str4 = string.Empty,
                    Str5 = string.Empty,
                    Remark = string.Empty,
                    DateTime1 = null,
                    DateTime2 = null,
                    CreateTime = DateTime.Now,
                    Creator = Creator,
                    InvoiceID = 0,
                    RelatedCustomerID = pod.CustomerID,
                    IsAudit = true
                };

                IEnumerable<QuotedPrice> prices = quotedPrice.Where(q =>
                   q.ProjectID == ProjectID && q.Target == 0 && q.TargetID == pod.CustomerID && q.StartCityName == pod.StartCityName && q.EndCityName == pod.EndCityName
                   && q.PodTypeName == pod.PODTypeName && q.ShipperTypeName == pod.ShipperTypeName && q.TplOrTtlName == pod.TtlOrTplName
                   && q.EffectiveStartTime <= pod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= pod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));

                if (prices == null || !prices.Any())
                {
                    this.GenErrorMessage(pod, Message, " 无对应报价,请先配置");
                    continue;
                }

                QuotedPrice price ;
                QuotedPrice miniPrice = prices.FirstOrDefault(p => p.StartVal == 20 && p.EndVal == 50);

                if (miniPrice == null)
                {
                    this.GenErrorMessage(pod, Message, " 系统报价有问题！");
                    continue;
                }

                if (pod.TtlOrTplID == 26)//零担
                {
                    price = prices.FirstOrDefault(p => p.StartVal < weight && p.EndVal >= weight);

                    if (price == null)
                    {
                        this.GenErrorMessage(pod, Message, " 根据运单毛重,未找到对应报价");
                        continue;
                    }
                    var region = ApplicationConfigHelper.GetRegions().FirstOrDefault(r => r.ID == pod.EndCityID);
                    if ((pod.StartCityID == 10 && pod.EndCityID == 10) || (pod.StartCityID == 10 && region.SupperID == 10))
                    {
                        settledPod.ShipAmt = price.Price;
                    }
                    else
                    {
                        if (weight <= this._settledByItem)
                        {
                            settledPod.ShipAmt = price.Price;
                        }
                        else
                        {
                            settledPod.ShipAmt = price.Price * (decimal)weight;
                            if (settledPod.ShipAmt < miniPrice.Price)
                            {
                                settledPod.ShipAmt = miniPrice.Price;
                            }
                        }
                    }
                }
                else
                {
                    price = prices.First();
                    if (price == null || string.IsNullOrEmpty(price.Remark))
                    {
                        this.GenErrorMessage(pod, Message, "报价录入有误");
                        continue;
                    }

                    var holeCarPrices = price.Remark.Split(';');
                    decimal holeCarPrice = 0;
                    try
                    {
                        holeCarPrice = holeCarPrices.FirstOrDefault(p => p.Split(':')[0].ObjectToInt32() == carType).Split(':')[1].ObjectToDecimal();
                    }
                    catch
                    {
                        this.GenErrorMessage(pod, Message, "报价录入有误");
                        continue;
                    }

                    if (holeCarPrice == 0)
                    {
                        this.GenErrorMessage(pod, Message, "无对应报价,请录入报价");
                        continue;
                    }

                    settledPod.ShipAmt = holeCarPrice;
                }

                if (pod.Str22 == "1")
                {
                    if (string.Equals(pod.Str30.Trim(), "支票"))
                    {
                        settledPod.ShipAmt += this._receiveForCheck;
                    }
                    else
                    {
                        settledPod.ShipAmt += this._receiveForCache;
                    }
                }

                settledPods.Add(settledPod);
            }

            if (Message.Length > 0)
            {
                return;
            }
            else
            {
                settledPods.Each((i, p) => { p.TotalAmt = p.ShipAmt + p.PointAmt + p.BAFAmt + p.OtherAmt + p.Amt1 + p.Amt2 + p.Amt3 + p.Amt4 + p.Amt5; });
                new SettledService().SettlePods(new SettlePodsRequest() { SettledPods = settledPods, SettledType = 0 });
            }
        }

        public void SettledPodForPay(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
        {
            throw new NotImplementedException();
        }

        private void GenErrorMessage(Pod pod, StringBuilder sb, string errorMessage)
        {
            sb.Append("运单:").Append(pod.CustomerOrderNumber).Append(",对应客户为:").Append(pod.CustomerName).Append(",起运城市为:")
              .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
              .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(errorMessage).Append("*");
        }
    }
}