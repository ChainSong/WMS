using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity;
using Runbow.TWS.Web.Implement;
using Runbow.TWS.Common;
using System.Text.RegularExpressions;

namespace Runbow.TWS.Web.SettledMethod
{
    public class SettledForAddidasInRunbowProject : BaseSettled
    {
        public SettledForAddidasInRunbowProject(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator, RelatedCustomerID)
        {

        }

        public override void CustomerDefinedSettledPod()
        {
            //double specialDivisor = 11.4;
            //double commonDivisor = 11;
            //decimal point100 = 100;
            //decimal point80 = 80;
            //decimal point_from_suzhou = 100;
            //decimal point_from_shanghai = 200;

            foreach (var gPod in base.SettledPodResponse.GroupedPods)
            {
                decimal BAFPrice = 0;
                double weight = 0;

                var innerPodIDS = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());

                var tempOriginalPods = this.podCollection.Where(p => innerPodIDS.Contains(p.ID));
                tempOriginalPods.Each((i, p) =>
                    {
                        int tempInt = 0;
                        double tempCube = 0;
                        if (string.Equals(p.PODTypeName, "出货运单", StringComparison.OrdinalIgnoreCase))
                        {
                            if (p.CustomerOrderNumber.Substring(0, 2) == "SP" || int.TryParse(p.CustomerOrderNumber.Substring(0, 1), out tempInt))
                            {
                                tempCube = p.BoxNumber.Value / 11.4;
                            }
                            else
                            {
                                tempCube = p.BoxNumber.Value / (double)11;
                            }
                        }
                        {
                            tempCube = p.BoxNumber.Value / (double)11;
                        }
                        weight += tempCube;
                        this.podCollection.First(c => c.ID == p.ID).Str40 = tempCube.ToString();
                    });
                var price = quotedPriceCollection.FirstOrDefault(q => q.ProjectID == 1 && q.Target == 0 && q.TargetID == gPod.TargetID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                                                                    && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim()
                                                                    && q.StartVal < weight && q.EndVal >= weight && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));
                var BAFPrices = BAF.FirstOrDefault(q => q.ProjectID == 1 && q.BAFStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && q.BAFEndTime > gPod.ActualDeliveryDate.ObjectToDateTime());

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
                    this.GenErrorMessage_ByGroupedPod(gPod, "");
                    continue;
                }

                //运费
                decimal shipAmt = price.Price * (decimal)weight;

                this.settledPodCollection.Each((i, settledPod) =>
                    {

                    });

            }
        }
    }
}