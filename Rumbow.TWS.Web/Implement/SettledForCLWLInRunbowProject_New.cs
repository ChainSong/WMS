using System;
using System.Collections.Generic;
using System.Linq;
using Runbow.TWS.Common;
using System.Web;
using Runbow.TWS.Entity;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Implement
{
    public class SettledForCLWLInRunbowProject_New : BaseSettled
    {
        public SettledForCLWLInRunbowProject_New(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator,RelatedCustomerID)
        {

        }

        public override void CustomerDefinedSettledPod()
        {
            //  var BAF = ApplicationConfigHelper.GetBAFPrice(customerOrShipperID);
            long relatedCustomerID = 0;
            #region AdidasPurchase
            if (base.podCollection.First().CustomerID == 13)
            {
                relatedCustomerID = 13;
                foreach (var gPod in SettledPodResponse.GroupedPods)
                {
                    double cube = gPod.Volume;
                    var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                    if (gPod.StartCityName != "衡阳")
                    {
                        gPod.StartCityName = "广州";
                    }
                    var price = quotedPriceCollection.FirstOrDefault(q =>
                      q.ProjectID == projectID && q.Target == 1 && q.TargetID == gPod.TargetID && q.RelatedCustomerID == relatedCustomerID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                      && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim() && q.StartVal < cube && q.EndVal >= cube
                      && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                    //QueryBAFPrice BAFPrices = BAF.FirstOrDefault(q =>
                    //q.ProjectID == customerOrShipperID && q.BAFStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && q.BAFEndTime > gPod.ActualDeliveryDate.ObjectToDateTime());
                    if (price == null)
                    {
                        this.GenErrorMessage_ByGroupedPod(gPod, "无系统报价，出货运单也无报价,请先配置报价.");
                        continue;
                    }
                    decimal shipAmt = price.Price * (decimal)cube * (decimal)0.9;
                    this.settledPodCollection.Each((i, settledPod) =>
                    {
                        settledPod.ShipAmt = i;
                        settledPod.BAFAmt = i;
                    });
                }
            }
            #endregion
        }
    }
}