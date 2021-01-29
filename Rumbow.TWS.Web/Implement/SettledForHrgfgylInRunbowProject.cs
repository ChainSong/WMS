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
    public class SettledForHrgfgylInRunbowProject : ISettledForPod
    {
        public void SettledPodForReceive(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
        {
            if (Message.Length > 0)
            {
                return;
            }

            PodService service = new PodService();
            var forSettledPodsResponse = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = PodIDs });

            if (!forSettledPodsResponse.IsSuccess)
            {
                throw forSettledPodsResponse.Exception;
            }

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

            IList<SettledPod> settledPods = new List<SettledPod>();
            string settledNumber = Guid.NewGuid().ToString();
            podsResponse.Result.Each((i, p) => {
                decimal shipAmt = 0;
                if (!string.IsNullOrEmpty(p.Str6))
                {
                    try
                    {
                        shipAmt = p.Str6.ObjectToDecimal();
                    }
                    catch
                    {
                        Message.Append("运单:").Append(p.CustomerOrderNumber).Append(",对应客户为:").Append(p.CustomerName).Append(",起运城市为:")
                            .Append(p.StartCityName).Append(",目的城市为:").Append(p.EndCityName).Append(",运输类型为:").Append(p.ShipperTypeName)
                            .Append(",运单类型为:").Append(p.PODTypeName).Append(",整车/零担为:").Append(p.TtlOrTplName).Append(" 应收金额不正确.").Append("*");
                    }
                }
                else
                {
                    Message.Append("运单:").Append(p.CustomerOrderNumber).Append(",对应客户为:").Append(p.CustomerName).Append(",起运城市为:")
                            .Append(p.StartCityName).Append(",目的城市为:").Append(p.EndCityName).Append(",运输类型为:").Append(p.ShipperTypeName)
                            .Append(",运单类型为:").Append(p.PODTypeName).Append(",整车/零担为:").Append(p.TtlOrTplName).Append(" 应收金额为空.").Append("*");
                }

                SettledPod settledPod = new SettledPod()
                {
                    ProjectID = ProjectID,
                    CustomerOrderNumber = p.CustomerOrderNumber,
                    SystemNumber = p.SystemNumber,
                    PodID = p.ID,
                    SettledNumber = settledNumber,
                    SettledType = 0,
                    CustomerOrShipperID = p.CustomerID.Value,
                    CustomerOrShipperName = p.CustomerName,
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
                    Creator = Creator,
                    InvoiceID = 0,
                    RelatedCustomerID = p.CustomerID,
                    IsAudit = true
                };

                settledPods.Add(settledPod);
 
            });

            if (Message.Length > 0)
            {
                return;
            }
            else
            {
                settledPods.Each((i, p) => { p.TotalAmt = p.ShipAmt + p.PointAmt + p.BAFAmt + p.OtherAmt + p.Amt1 + p.Amt2 + p.Amt3 + p.Amt4 + p.Amt5; });
                new SettledService().SettlePods(new SettlePodsRequest(){SettledPods = settledPods, SettledType = 0});
            }
        }

        public void SettledPodForPay(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
        {
            throw new NotImplementedException();
        }
    }
}