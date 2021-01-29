using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Web.Interface;
using Runbow.TWS.Biz.WMS;
using Runbow.TWS.Entity.WMS.PreOrders;
using System.Text;
using Runbow.TWS.MessageContracts.WMS.PreOrders;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.AllocateInstances
{
    public class DefaultNikeAllocate : BaseAllocate
    {
        public DefaultNikeAllocate(string AllocateMode, string AllocateType, long ID, string CustomerId,
            string UserName, IEnumerable<PreOrderIds> List, IEnumerable<PreOrderDetail> Pod)
            : base(AllocateMode, AllocateType, ID, CustomerId, UserName, List, Pod)
        {
        }
        //new PreOrderService().ManualAllocationJson(new ManualAllocationRequest() 
        //{ PodRequest = pod, ID = Convert.ToInt64(ID), CustomerId = CustomerId, Creator = base.UserInfo.Name, Criterion = Criterion });
        public override void CustomerDefinedSettledAllocate()
        {
            if (AllocateMode == "自动分配")
            {
                this.SqlProc = "Proc_WMS_AutomatedOutbound_Nike";
                PreOrderService service = new PreOrderService();
                var response = service.AutomaticAllocation(List, UserName, AllocateType, SqlProc);
                if (!response.IsSuccess)
                {
                    //throw response.Exception;
                    this.DisInfo = null;
                }

                this.DisInfo = response.Result.DisInfo;
            }
            else if (AllocateMode == "指定分配")
            {
                this.SqlProc = "Proc_WMS_AssignedOutbound_Nike";
                PreOrderService service = new PreOrderService();
                var response = service.ManualAllocationJson(new ManualAllocationRequest() { PodRequest = Pod, ID = ID, CustomerId = CustomerId, Creator = UserName, Criterion = AllocateType, SqlProc = SqlProc });
                if (!response.IsSuccess)
                {
                    this.DisInfo = null;
                }
                this.DisInfo = response.Result.DisInfo;
            }
            else
            {
                this.SqlProc = "Proc_WMS_ManualAllocation_Nike";
                PreOrderService service = new PreOrderService();
                var response = service.ManualAllocationJson(new ManualAllocationRequest() { PodRequest = Pod, ID = ID, CustomerId = CustomerId, Creator = UserName, Criterion = AllocateType, SqlProc = SqlProc });
                if (!response.IsSuccess)
                {
                    this.DisInfo = null;
                }
                this.DisInfo = response.Result.DisInfo;
            }
        }
    }
}