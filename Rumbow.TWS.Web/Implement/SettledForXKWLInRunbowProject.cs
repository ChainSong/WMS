﻿using System;
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
    public class SettledForXKWLInRunbowProject : ISettledForPod
    {
        private double _settledByItem = 50;
        private decimal _rate = 0.003M;

        public void SettledPodForReceive(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
        {
            throw new NotImplementedException();
        }

        public void SettledPodForPay(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
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
                //For Hilti
                if (pod.CustomerID == 2)
                {
                    if (string.IsNullOrEmpty(pod.Str24))
                    {
                        this.GenErrorMessage(pod, Message, " 无净重,请先编辑运单毛重");
                        continue;
                    }

                    double weight = 0;
                    try
                    {
                        weight = pod.Str24.ObjectToDouble();
                    }
                    catch
                    {
                        this.GenErrorMessage(pod, Message, "净重数据不是数字类型,请重新编辑运单");
                        continue;
                    }

                    IEnumerable<QuotedPrice> prices = quotedPrice.Where(q =>
                    q.ProjectID == ProjectID && q.Target == 0 && q.TargetID == pod.CustomerID && q.StartCityName == pod.StartCityName && q.EndCityName == pod.EndCityName
                    && q.PodTypeName == pod.PODTypeName && q.ShipperTypeName == pod.ShipperTypeName && q.TplOrTtlName == pod.TtlOrTplName
                    && q.EffectiveStartTime <= pod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= pod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));

                    if (prices == null || !prices.Any())
                    {
                        this.GenErrorMessage(pod, Message, " 无对应报价,请先配置");
                        continue;
                    }

                    SettledPod settledPod = new SettledPod()
                    {
                        ProjectID = pod.ProjectID,
                        CustomerOrderNumber = pod.CustomerOrderNumber,
                        SystemNumber = pod.SystemNumber,
                        PodID = pod.ID,
                        SettledNumber = settledNumber,
                        SettledType = 1,
                        CustomerOrShipperID = pod.ShipperID.Value,
                        CustomerOrShipperName = pod.ShipperName,
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

                    QuotedPrice price;
                    QuotedPrice minPrice = prices.FirstOrDefault(p => p.StartVal == 0 && p.EndVal == 50);

                    if (minPrice == null)
                    {
                        this.GenErrorMessage(pod, Message, " 系统报价有问题！");
                        continue;
                    }

                    price = prices.FirstOrDefault(p => p.StartVal < weight && p.EndVal >= weight);

                    if (price == null)
                    {
                        this.GenErrorMessage(pod, Message, " 根据运单净重,未找到对应报价");
                        continue;
                    }

                    if (weight <= this._settledByItem)
                    {
                        settledPod.ShipAmt = minPrice.Price;
                    }
                    else
                    {
                        settledPod.ShipAmt = price.Price * (decimal)weight;
                        if (settledPod.ShipAmt < minPrice.Price)
                        {
                            settledPod.ShipAmt = minPrice.Price;
                        }
                    }

                    if (pod.Str22 == "1")
                    {
                        if (string.Equals(pod.Str30.Trim(), "现金"))
                        {
                            if (string.IsNullOrEmpty(pod.Str14))
                            {
                                this.GenErrorMessage(pod, Message, " 代收款方式为现金,但未提供代收款金额,无法结算代收现金手续费,请编辑运单代收款金额");
                                continue;
                            }

                            decimal tempAmt = 0;
                            try
                            {
                                tempAmt = pod.Str14.ObjectToDecimal();
                            }
                            catch
                            {
                                this.GenErrorMessage(pod, Message, " 代收款方式为现金,但代收款金额输入有误,无法结算代收现金手续费,请编辑运单代收款金额");
                                continue;
                            }
                            settledPod.ShipAmt += tempAmt * this._rate;
                        }
                    }

                    settledPods.Add(settledPod);
                }
            }

            if (Message.Length > 0)
            {
                return;
            }
            else
            {
                settledPods.Each((i, p) => { p.TotalAmt = p.ShipAmt + p.PointAmt + p.BAFAmt + p.OtherAmt + p.Amt1 + p.Amt2 + p.Amt3 + p.Amt4 + p.Amt5; });
                new SettledService().SettlePods(new SettlePodsRequest() { SettledPods = settledPods, SettledType = 1 });
            }
        }

        private void GenErrorMessage(Pod pod, StringBuilder sb, string errorMessage)
        {
            sb.Append("运单:").Append(pod.CustomerOrderNumber).Append(",对应客户为:").Append(pod.CustomerName).Append(",起运城市为:")
              .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
              .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(errorMessage).Append("*");
        }
    }
}