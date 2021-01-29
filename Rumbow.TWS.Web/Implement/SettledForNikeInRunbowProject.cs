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
    public class SettledForNikeInRunbowProject : ISettledForPod
    {
        public void SettledPodForReceive(IEnumerable<long> PodIDs, string Creator, long ProjectID, int Target, long CustomerOrShipperID, long RelatedCustomerID, StringBuilder Message)
        {
            if (Message.Length > 0)
            {
                return;
            }

            PodService service = new PodService();
            //var forSettledPodsResponse = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = PodIDs });

            //if (!forSettledPodsResponse.IsSuccess)
            //{
            //    throw forSettledPodsResponse.Exception;
            //}

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

            var quotedPrices = ApplicationConfigHelper.GetProjectQuotedPrice(ProjectID,Target,CustomerOrShipperID,RelatedCustomerID);
            string settledNumber = Guid.NewGuid().ToString();
            IList<SettledPod> settledPods = new List<SettledPod>();
            #region 空运结算
            //空运运单
            IList<Pod> airFreights = new List<Pod>();
            foreach (var gPod in groupedPodsResponse.Result.GroupedPods)
            {
                double airFreightPrice = 0;
                decimal airFreighShipAmt = 0;
                double airFreightBoxNumber = 0;
                var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                var tempOriginalPods = podsResponse.Result.Where(p => innerPodIDs.Contains(p.ID));
                foreach (var tempOriginalPod in tempOriginalPods)
                {
                    if (string.Equals(tempOriginalPod.ShipperTypeName, "空运", StringComparison.OrdinalIgnoreCase))
                    {
                        airFreights.Add(tempOriginalPod);
                    } 
                }
                if (airFreights.Count> 0)
                {
                    
                    foreach (var airFreight in airFreights)
                    {
                        airFreightBoxNumber = (1000 / 6.0) * (double)(airFreight.BoxNumber / 11);
                        if (airFreightBoxNumber > 500)
                        {
                            airFreightPrice = 11.44;
                        }
                        if (airFreightBoxNumber > 100)
                        {
                            airFreightPrice = 13.52;
                        }
                        else
                        {
                            airFreightPrice = 16.64;
                        }  
                        if (airFreight.BoxNumber == null)
                        {
                            this.GenErrorMessage(airFreight, Message, "请先设置货物的箱数");
                            continue;
                        }
                        airFreighShipAmt = (decimal)airFreightBoxNumber * (decimal)airFreightPrice;
                        SettledPod settledPod = new SettledPod()
                        {
                            ProjectID = airFreight.ProjectID,
                            CustomerOrderNumber = airFreight.CustomerOrderNumber,
                            SystemNumber = airFreight.SystemNumber,
                            PodID = airFreight.ID,
                            SettledNumber = settledNumber,
                            SettledType = 0,
                            CustomerOrShipperID = airFreight.CustomerID.Value,
                            CustomerOrShipperName = airFreight.CustomerName,
                            StartCityID = airFreight.StartCityID.Value,
                            StartCityName = airFreight.StartCityName,
                            EndCityID = airFreight.EndCityID.Value,
                            EndCityName = airFreight.EndCityName,
                            ShipperTypeID = airFreight.ShipperTypeID.Value,
                            ShipperTypeName = airFreight.ShipperTypeName,
                            PODTypeID = airFreight.PODTypeID.Value,
                            PODTypeName = airFreight.PODTypeName,
                            TtlOrTplID = airFreight.TtlOrTplID.Value,
                            TtlOrTplName = airFreight.TtlOrTplName,
                            ActualDeliveryDate = airFreight.ActualDeliveryDate.Value,
                            BoxNumber = airFreight.BoxNumber,
                            Weight = airFreight.Weight,
                            Volume = airFreight.Volume,
                            GoodsNumber = airFreight.GoodsNumber,
                            ShipAmt = airFreighShipAmt,
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
                            Str4 = airFreightPrice.ToString(),
                            Str5 = string.Empty,
                            Remark = string.Empty,
                            DateTime1 = null,
                            DateTime2 = null,
                            CreateTime = DateTime.Now,
                            Creator = Creator,
                            InvoiceID = 0,
                            RelatedCustomerID = airFreight.CustomerID,
                            IsAudit = true
                        };

                        settledPods.Add(settledPod);
                    }
                }
                airFreights.Clear();
            }
                #endregion
                #region 公路运算
                //起运城市是太仓CLC、太仓CRW、上海的按同城结算（大仓和工厂直发）
               var podsResponses = podsResponse.Result.Where(s => s.PODTypeName != "退货运单" && s.PODTypeName != "门店调拨" && s.ShipperTypeName != "空运").GroupBy(x => new { x.ActualDeliveryDate, x.EndCityName, })
                .Select(g => new
                {
                    Peo = g.Key,
                    count = g.Count(),
                    pods = g.Select(k => { return k; })
                });
               //存放发货地点为太仓CLC、太仓CRW、上海的运单（大仓发货、工厂直发）
                IList<Pod> Temporary = new List<Pod>();
              //存放发货地点为广州的运单（工厂直发）
                IList<Pod> guangzhouTemporary = new List<Pod>();
                foreach (var podsRp in podsResponses)
                {
                    double boxnumner = 0;
                    double guangzhouzboxnumner = 0;
                    double cube = 0;
                    double guangzhouzcube = 0;
                    foreach (var pod in podsRp.pods)
                    {
                        if (pod.BoxNumber == null)
                        {
                            this.GenErrorMessage(pod, Message, "请先设置货物的箱数");
                            continue;
                        }
                        if (string.Equals(pod.StartCityName.Trim(), "广州", StringComparison.OrdinalIgnoreCase))
                        {
                            guangzhouzboxnumner += (double)pod.BoxNumber;
                            guangzhouTemporary.Add(pod);
                        }
                        else
                        {
                            boxnumner += (double)pod.BoxNumber;
                            Temporary.Add(pod);
                        } 
                    }
                    cube = boxnumner /11;
                    guangzhouzcube = guangzhouzboxnumner / 11;
                    if (Temporary.Count>0)
                    {
                        Settlement(settledNumber, ProjectID, Message, cube, boxnumner, Temporary, settledPods, quotedPrices);
                        Temporary.Clear();
                    }
                    if (guangzhouTemporary.Count>0)
                    {
                        Settlement(settledNumber, ProjectID, Message, guangzhouzcube, guangzhouzboxnumner, guangzhouTemporary, settledPods, quotedPrices);
                        guangzhouTemporary.Clear();
                    }
                  
                }
                //退货运单，目的城市是太仓CLC、太仓CRW、上海的按同城结算
                var ReturnspodsResponses = podsResponse.Result.Where(s => s.PODTypeName == "退货运单" && s.ShipperTypeName != "空运").GroupBy(x => new { x.ActualDeliveryDate, x.StartCityName, })
               .Select(g => new
               {
                   Peo = g.Key,
                   count = g.Count(),
                   pods = g.Select(k => { return k; })
               });
                //存放目的地为太仓CLC、太仓CRW、上海的运单（退货运单）
                IList<Pod> ReturnsTemporary = new List<Pod>(); 
                //存放目的地为广州的运单（退货运单）
                IList<Pod> ReturnsguangzhouTemporary = new List<Pod>();
                foreach (var podsRp in ReturnspodsResponses)
                {
                    double Returnsboxnumber = 0;
                    double Returnsguangzhouzboxnumner = 0;
                    double Returnscube = 0;
                    double Returnsguangzhouzcube = 0;
                    foreach (var pod in podsRp.pods)
                    {
                        if (pod.BoxNumber == null)
                        {
                            this.GenErrorMessage(pod, Message, "请先设置货物的箱数");
                            continue;
                        }
                        if (string.Equals(pod.StartCityName.Trim(), "广州", StringComparison.OrdinalIgnoreCase))
                        {
                            Returnsguangzhouzboxnumner += (double)pod.BoxNumber;
                            ReturnsguangzhouTemporary.Add(pod);
                        }
                        else
                        {   
                            Returnsboxnumber += (double)pod.BoxNumber;
                            ReturnsTemporary.Add(pod);
                        }
                    }
                    Returnscube = Returnsboxnumber / 11;
                    Returnsguangzhouzcube = Returnsboxnumber / 11;
                    if (ReturnsTemporary.Count>0)
                    {
                        Settlement(settledNumber, ProjectID, Message, Returnscube, Returnsboxnumber, ReturnsTemporary, settledPods, quotedPrices);
                        ReturnsTemporary.Clear();
                    }
                    if (ReturnsguangzhouTemporary.Count>0)
                    {
                        Settlement(settledNumber, ProjectID, Message, Returnsguangzhouzcube, Returnsguangzhouzboxnumner, ReturnsguangzhouTemporary, settledPods,quotedPrices);
                        ReturnsguangzhouTemporary.Clear();
                    }     
                }
                #endregion
            //除“华启物流”外都有提货补贴
            var prs= podsResponse.Result.Where(p=>p.ShipperName!="华启物流");
            prs.Each((i, e) =>
            {
                settledPods.Each((m,n)=>{
                    if (n.PodID==e.ID)
                    {
                        n.OtherAmt = (decimal)n.BoxNumber * (decimal)1.56;
                    }
                });
            });
           
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
        private void GenErrorMessage_ByGroupedPod(GroupedPods pod, StringBuilder sb, string errorMessage)
        {
            sb.Append("运单:").Append(pod.PodIDs).Append(",对应客户为:").Append(pod.TargetName).Append(",起运城市为:")
              .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
              .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(errorMessage).Append("*");
        }
        private void GenErrorMessage(Pod pod, StringBuilder sb, string errorMessage)
        {
            sb.Append("运单:").Append(pod.CustomerOrderNumber).Append(",对应客户为:").Append(pod.CustomerName).Append(",起运城市为:")
              .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
              .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(errorMessage).Append("*");
        }
        /// <summary>
        /// 结算方法
        /// </summary>
        /// <param name="settledNumber"></param>
        /// <param name="ProjectID"></param>
        /// <param name="Message"></param>
        /// <param name="cube">合并后的立方</param>
        /// <param name="boxnumner">合并后的箱数</param>
        /// <param name="Temporary">合并后运单的集合</param>
        /// <param name="settledPods">结算结果集合</param>
        /// <param name="quotedPrices">报价</param>
        private void Settlement(string settledNumber, long ProjectID, StringBuilder Message, double cube, double boxnumner, IList<Pod> Temporary, IList<SettledPod> settledPods, IEnumerable<QuotedPrice> quotedPrices)
        {
            decimal shipAmt = 0;
            string n = Temporary[0].EndCityName.Trim().Substring(0, 2) == "太仓" ? Temporary[0].EndCityName.Trim().Substring(0, 2) : Temporary[0].EndCityName.Trim();
            QuotedPrice price = quotedPrices.FirstOrDefault(q =>
              q.ProjectID == ProjectID && q.Target == 0 && q.TargetID == Temporary[0].CustomerID && q.StartCityName.Trim() == (Temporary[0].StartCityName.Trim().Substring(0, 2) == "太仓" ? Temporary[0].StartCityName.Trim().Substring(0, 2) : Temporary[0].StartCityName.Trim())
              && q.EndCityName.Trim() == (Temporary[0].EndCityName.Trim().Substring(0, 2) == "太仓" ? Temporary[0].EndCityName.Trim().Substring(0, 2) : Temporary[0].EndCityName.Trim())
              && q.PodTypeName.Trim() == Temporary[0].PODTypeName.Trim() && q.ShipperTypeName.Trim() == Temporary[0].ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == Temporary[0].TtlOrTplName.Trim() && q.StartVal < cube && q.EndVal >= cube
              && q.EffectiveStartTime <= Temporary[0].ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= Temporary[0].ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
            if (price == null)
            {
                this.GenErrorMessage(Temporary[0], Message, " 无系统对应报价,请先配置报价.");
                
            }
            if (Message.Length > 0)
            {
                return;
            }
            shipAmt = price.Price * (decimal)cube;
            foreach (var pod in Temporary)
            {
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