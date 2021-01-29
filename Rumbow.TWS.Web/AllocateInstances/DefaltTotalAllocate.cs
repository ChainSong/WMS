using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.PreOrders;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.MessageContracts.WMS.PreOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.AllocateInstances
{
    public class DefaltTotalAllocate : BaseAllocate
    {
        public DefaltTotalAllocate(string AllocateMode, string AllocateType, long ID, string CustomerId,
            string UserName, IEnumerable<PreOrderIds> List, IEnumerable<PreOrderDetail> Pod)
            : base(AllocateMode, AllocateType, ID, CustomerId, UserName, List, Pod)
        {
        }
        //new PreOrderService().ManualAllocationJson(new ManualAllocationRequest() 
        //{ PodRequest = pod, ID = Convert.ToInt64(ID), CustomerId = CustomerId, Creator = base.UserInfo.Name, Criterion = Criterion });
        public override void CustomerDefinedSettledAllocate()
        {
            PreOrderService service = new PreOrderService();
            Response<PreOrderAndPreOrderDetail> response = new Response<PreOrderAndPreOrderDetail>();

            switch (AllocateMode)
            {
                case "自动分配":
                    this.SqlProc = "Proc_WMS_AutomatedOutbound_Total";
                    response = service.AutomaticAllocation(List, UserName, AllocateType, SqlProc);
                    break;
                case "手动分配":
                    this.SqlProc = "Proc_WMS_ManualAllocation_Total";
                    response = service.ManualAllocationJson(new ManualAllocationRequest() { PodRequest = Pod, ID = ID, CustomerId = CustomerId, Creator = UserName, Criterion = AllocateType });
                    break;
                case "现场分配":
                    this.SqlProc = "Proc_WMS_ManualAllocation_Total";
                    response = service.WorkersAlloctions(new ManualAllocationRequest() { PodRequest = Pod, ID = ID, CustomerId = CustomerId, Creator = UserName, Criterion = AllocateType });
                    break;
                default:
                    break;
            }
            if (!response.IsSuccess)
            {
                this.DisInfo = null;
            }
            this.DisInfo = response.Result.DisInfo;

        }
    }
}