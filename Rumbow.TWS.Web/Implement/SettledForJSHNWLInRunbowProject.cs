using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Runbow.TWS.Biz;
using Runbow.TWS.Common;
using Runbow.TWS.Entity;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Web.Common;
using Runbow.TWS.Web.Interface;

namespace Runbow.TWS.Web.Implement
{
    public class SettledForJSHNWLInRunbowProject : ISettledForPod
    {
        public void SettledPodForReceive(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, System.Text.StringBuilder Message)
        {
            throw new NotImplementedException();
        }

        public void SettledPodForPay(IEnumerable<long> PodIDs, string creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, System.Text.StringBuilder Message)
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


            string settledNumber = Guid.NewGuid().ToString();
            IList<SettledPod> settledPods = new List<SettledPod>();
            #region 海润光伏供应链
            if (podsResponse.Result.First().CustomerID == 28)
            {
                podsResponse.Result.Each((i, p) =>
                {
                    decimal shipAmt = 0;
                    if (!string.IsNullOrEmpty(p.Str7))
                    {
                        try
                        {
                            shipAmt = p.Str7.ObjectToDecimal();
                        }
                        catch
                        {
                            this.GenErrorMessage_ByPod(p, Message, "支出金额格式有误。");
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(p.Str6))
                        {
                            try
                            {
                                shipAmt = p.Str6.ObjectToDecimal() * (0.86).ObjectToDecimal();
                            }
                            catch
                            {
                                this.GenErrorMessage_ByPod(p, Message, "收入金额格式有误。");
                            }
                        }
                        else
                        {
                            this.GenErrorMessage_ByPod(p, Message, "收入金额支出金额都为空。");
                        }
                    }

                    SettledPod settledPod = new SettledPod()
                    {
                        ProjectID = ProjectID,
                        CustomerOrderNumber = p.CustomerOrderNumber,
                        SystemNumber = p.SystemNumber,
                        PodID = p.ID,
                        SettledNumber = settledNumber,
                        SettledType = 1,
                        CustomerOrShipperID = p.ShipperID.Value,
                        CustomerOrShipperName = p.ShipperTypeName,
                        StartCityID = p.StartCityID.Value,
                        StartCityName = p.StartCityName,
                        EndCityID = p.EndCityID.Value,
                        EndCityName = p.EndCityName,
                        ShipperTypeID = p.ShipperTypeID.Value,
                        ShipperTypeName = p.ShipperTypeName,
                        PODTypeID = p.PODTypeID.Value,
                        PODTypeName = p.PODTypeName,
                        TtlOrTplID = p.TtlOrTplID.Value,
                        TtlOrTplName = p.TtlOrTplName,
                        ActualDeliveryDate = p.ActualDeliveryDate.Value,
                        BoxNumber = p.BoxNumber,
                        Weight = p.Weight,
                        Volume = p.Volume,
                        GoodsNumber = p.GoodsNumber,
                        ShipAmt = shipAmt,
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
                        Creator = creator,
                        InvoiceID = 0,
                        RelatedCustomerID = p.CustomerID,
                        IsAudit = true
                    };

                    settledPods.Add(settledPod);

                });
            }
            #endregion

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

        private void GenErrorMessage_ByGroupedPod(GroupedPods pod, StringBuilder sb, string errorMessage)
        {
            sb.Append("运单:").Append(pod.PodIDs).Append(",对应客户为:").Append(pod.TargetName).Append(",起运城市为:")
              .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
              .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(errorMessage).Append("*");
        }

        private void GenErrorMessage_ByPod(Pod pod, StringBuilder sb, string errorMessage)
        {
            sb.Append("运单:").Append(pod.CustomerOrderNumber).Append(",对应客户为:").Append(pod.CustomerName).Append(",起运城市为:")
              .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
              .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(errorMessage).Append("*");
        }
        private void Settlement(string settledNumber, long ProjectID, StringBuilder Message, double cube, double boxnumner, IEnumerable<Pod> pods, IList<SettledPod> settledPods, IEnumerable<QuotedPrice> quotedPrices)
        {
            decimal shipAmt = 0;
            QuotedPrice price = null;
            //调拨运单
            if (cube == 0)
            {
                price = quotedPrices.FirstOrDefault(q =>
                q.ProjectID == ProjectID && q.Target == 1 && q.StartCityName.Trim() == pods.First().StartCityName.Trim() && q.EndCityName.Trim() == pods.First().EndCityName.Trim()
                && q.PodTypeName.Trim() == pods.First().PODTypeName.Trim() && q.ShipperTypeName.Trim() == pods.First().ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == pods.First().TtlOrTplName.Trim() && q.StartVal <= boxnumner && q.EndVal >= boxnumner
                && q.EffectiveStartTime <= pods.First().ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= pods.First().ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                if (price == null)
                {
                    this.GenErrorMessage_ByPod(pods.First(), Message, " 无系统对应报价,请先配置报价.");
                }
                if (Message.Length > 0)
                {
                    return;
                }
                shipAmt = price.Price * (decimal)boxnumner;
            }
            else
            {
              
                price = quotedPrices.FirstOrDefault(q =>
                 q.ProjectID == ProjectID && q.Target == 1 && q.TargetID == pods.First().ShipperID &&q.StartCityName == pods.First().StartCityName.Trim() && q.EndCityName.Trim()== pods.First().EndCityName.Trim()
                && q.PodTypeName.Trim() == pods.First().PODTypeName.Trim() && q.ShipperTypeName.Trim() == pods.First().ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == pods.First().TtlOrTplName.Trim() && q.StartVal < cube && q.EndVal >= cube
                && q.EffectiveStartTime <= pods.First().ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= pods.First().ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                if (price == null)
                {
                    this.GenErrorMessage_ByPod(pods.First(), Message, " 无系统对应报价,请先配置报价.");

                }
                if (Message.Length > 0)
                {
                    return;
                }
                shipAmt = price.Price * (decimal)cube;
            }

            foreach (var pod in pods)
            {

                SettledPod settledPod = new SettledPod()
                {
                    ProjectID = pod.ProjectID,
                    CustomerOrderNumber = pod.CustomerOrderNumber,
                    SystemNumber = pod.SystemNumber,
                    PodID = pod.ID,
                    SettledNumber = settledNumber,
                    SettledType = 1,
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
                    Weight = pod.Weight,
                    Volume = pod.Volume,
                    GoodsNumber = pod.GoodsNumber,
                    ShipAmt = shipAmt * (decimal)(pod.BoxNumber / boxnumner),
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
                    Str4 = price.Price.ToString(),
                    Str5 = string.Empty,
                    Remark = string.Empty,
                    DateTime1 = null,
                    DateTime2 = null,
                    CreateTime = DateTime.Now,
                    Creator = pod.Creator,
                    InvoiceID = 0,
                    RelatedCustomerID = pod.CustomerID,
                    IsAudit = true
                };
                settledPods.Add(settledPod);
            }
        }
    }
}