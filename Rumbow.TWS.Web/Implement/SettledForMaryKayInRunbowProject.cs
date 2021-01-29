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
    public class SettledForMaryKayInRunbowProject : ISettledForPod
    {
        public void SettledPodForReceive(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
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
                if (!pod.Weight.HasValue || pod.Weight.Value == 0)
                {
                    this.GenErrorMessage(pod, Message, " 无重量,请先编辑运单重量.");
                    continue;
                }

                if (string.IsNullOrEmpty(pod.Str2))
                {
                    this.GenErrorMessage(pod, Message, " 无OPP编号,请先编辑运单OPP编号.");
                    continue;
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
                    Weight = pod.Weight,
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
                    this.GenErrorMessage(pod, Message, " 无对应报价,请先配置.");
                    continue;
                }

                var price = prices.FirstOrDefault();
                if (price == null)
                {
                    throw new Exception("系统出错");
                }

                string priceStr = price.Remark;

                if (string.IsNullOrEmpty(priceStr))
                {
                    this.GenErrorMessage(pod, Message, " 报价字符串为空,不符规定,请重新编辑.");
                    continue;
                }

                IEnumerable<string> priceTotal = priceStr.Split(';');
                if (priceTotal == null || !priceTotal.Any())
                {
                    this.GenErrorMessage(pod, Message, " 报价字符串有问题,不符规定,请重新编辑.");
                    continue;
                }

                int i = 0;
                int j = 0;
                foreach (string str in priceTotal)
                {
                    i ++;
                    var temp = str.Split(':');
                    if (temp == null || !temp.Any())
                    {
                        this.GenErrorMessage(pod, Message, " 报价字符串(" + str + ")有问题,不符规定,请重新编辑.");
                        continue;
                    }

                    string oppId = temp[0];
                    if (!string.Equals(oppId, pod.Str2,StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var priceArray = temp[1].Split(',');
                    if (priceArray == null || priceArray.Length != 4)
                    {
                        this.GenErrorMessage(pod, Message, " 报价字符串(" + str + ")有问题,不符规定,请重新编辑.");
                        continue;
                    }

                    decimal firstPrice = 0;
                    decimal lastPrice = 0;
                    double firstWeight = 0;
                    int type = 0;

                    try
                    {
                        firstPrice = priceArray[0].ObjectToDecimal();
                        lastPrice = priceArray[1].ObjectToDecimal();
                        firstWeight = double.Parse(priceArray[2]);
                        type = priceArray[3].ObjectToInt32();
                    }
                    catch
                    {
                        this.GenErrorMessage(pod, Message, " 报价字符串(" + str + ")有问题,不符规定,请重新编辑.");
                        continue;
                    }

                    if (firstPrice == 0 || lastPrice == 0 || firstWeight == 0)
                    {
                        this.GenErrorMessage(pod, Message, " 报价字符串(" + str + ")有问题,不符规定,请重新编辑.");
                        continue;
                    }

                    if (pod.Weight.Value <= firstWeight)
                    {
                        settledPod.ShipAmt = firstPrice;
                    }
                    else
                    {
                        var tempWeight = pod.Weight.Value - firstWeight;
                        var tempWeightInt = Convert.ToInt32(tempWeight);
                        if (type == 0 && tempWeight > tempWeightInt)
                        {
                            tempWeight = tempWeightInt + 1;
                        }

                        settledPod.ShipAmt = firstPrice + (decimal)tempWeight * lastPrice;
                    }

                    settledPods.Add(settledPod);
                    j = 1;
                }

                if (i == priceTotal.Count() && j == 0)
                {
                    this.GenErrorMessage(pod, Message, " 报价字符串不存在与运单匹配的OPP编号,请重新编辑.");
                    continue;
                }
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