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
    public class SettledForAkzotbInRunbowProject_New : BaseSettled
    {
        public SettledForAkzotbInRunbowProject_New(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
            : base(SettledType, customerOrShipperID, IsGroupedPods, PodIDs, ProjectID, Creator,RelatedCustomerID)
        {

        }
        //淘宝akzo
        public override void CustomerDefinedSettledPod()
        {
            foreach (var gPod in SettledPodResponse.GroupedPods)
            {
                var innerPodIDs = gPod.PodIDs.Split('|').Select(i => i.ObjectToInt64());
                var price = quotedPriceCollection.FirstOrDefault(q =>
                                    q.ProjectID == projectID && q.Target == 0 && q.TargetID == gPod.TargetID && q.StartCityName.Trim() == gPod.StartCityName.Trim() && q.EndCityName.Trim() == gPod.EndCityName.Trim()
                                    && q.PodTypeName.Trim() == gPod.PODTypeName.Trim() && q.ShipperTypeName.Trim() == gPod.ShipperTypeName.Trim() && q.TplOrTtlName.Trim() == gPod.TtlOrTplName.Trim()
                                    && q.EffectiveStartTime <= gPod.ActualDeliveryDate.ObjectToDateTime() && (q.EffectiveEndTime >= gPod.ActualDeliveryDate.ObjectToDateTime() || q.EffectiveEndTime == null));

                if (price == null)
                {
                    this.GenErrorMessage_ByGroupedPod(gPod, "无系统报价，出货运单也无报价,请先配置报价.");
                    continue;
                }
                //向结算成功列表中插入数据   运费
                this.settledPodCollection.Each((i, settledPod) =>
                {
                    var originalPod = from q in innerPodIDs where q == settledPod.PodID select q;
                    if (originalPod.Count() > 0)
                    {
                        if (settledPod.Weight == null)
                        {
                            this.GenErrorMessage_ByGroupedPod(gPod, "请先设置货物的重量");
                        }
                        else
                        {
                            if (originalPod.First().ToString() == settledPod.PodID.ToString())
                            {
                                //如果小于50kg 运费=基价   否则运费=基价+续价*（重量-50）
                                if (settledPod.Weight <= 50)
                                {
                                    settledPod.ShipAmt = price.Price;
                                }
                                else
                                {
                                    settledPod.ShipAmt = price.Price + price.Point * (decimal)(settledPod.Weight - 50);
                                }
                                settledPod.Str4 = price.Price.ToString();
                                settledPod.BAFAmt = 0;
                            }
                        }
                    }
                });
            }
        }
    }
}