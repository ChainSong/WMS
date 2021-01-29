using Runbow.TWS.Entity.WMS.PreOrders;
using Runbow.TWS.Web.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Common;
using Runbow.TWS.MessageContracts.WMS.PreOrders;
using Runbow.TWS.Entity;
namespace Runbow.TWS.Web.AllocateInstances
{
    public class BaseAllocate : IAllocate
    {

        /// <summary>
        /// 分配方式 （自动OR手动）
        /// </summary>
        public string AllocateMode { get; set; }
        /// <summary>
        /// 分配类型 （全部OR部分）
        /// </summary>
        public string AllocateType { get; set; }

        /// <summary>
        /// 预出库单子
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 待分配运单ID
        /// </summary>
        public IEnumerable<PreOrderIds> List { get; set; }

        /// <summary>
        /// 预出库单
        /// </summary>
        public IEnumerable<PreOrderDetail> Pod { get; set; }
        //<summary>
        //分配存储过程
        //</summary>
        public string SqlProc { get; set; }

        /// <summary>
        /// 分配的结果
        /// </summary>
        public IEnumerable<DistributionInformation> DisInfo = null;

        protected IList<DistributionInformation> Info = new List<DistributionInformation>();

        public BaseAllocate(string allocateMode, string allocateType,long id,string customerId,
         string userName, IEnumerable<PreOrderIds> list, IEnumerable<PreOrderDetail> pod)
        {
            this.AllocateMode = allocateMode;
            this.AllocateType = allocateType;
            this.ID = id;
            this.CustomerId = customerId;
            this.UserName = userName;
            this.List = list;
            this.Pod = pod;
            //this.SqlProc = "Proc_WMS_ManualAllocation";
            //PreOrderService service = new PreOrderService();
            //var response = service.AutomaticAllocation(list, UserName, AllocateType, SqlProc);
            //if (!response.IsSuccess)
            //{
            //    throw response.Exception;
            //}

            // this.DisInfo = response.Result.DisInfo;
        }
        public virtual void CustomerDefinedSettledAllocate()
        {

        }
        /// <summary>
        /// 结算调用方法
        /// </summary>
        /// <returns>如果Message有值，则计算不成功，外部显示Message的值。如果没有值，则结算成功</returns>
        public IEnumerable<DistributionInformation> Allocate()
        {
            //根据用户自定义结算方法取得结算成功运单列表
            this.CustomerDefinedSettledAllocate();
            //this.Info.Each((i, p) => { 

            //});
            if (Info.Count > 0)
            {
                return DisInfo.Union(Info);
            }
            return DisInfo;
            //如果没有错误信息并且结算成功运单列表有值，则插入运单结算表
            //if (this.message.Length == 0 && this.settledPodCollection != null && this.settledPodCollection.Any())
            //{
            //    this.settledPodCollection.Each((i, p) => { p.TotalAmt = p.ShipAmt + p.PointAmt + p.BAFAmt + p.OtherAmt + p.Amt1 + p.Amt2 + p.Amt3 + p.Amt4 + p.Amt5; });
            //    new SettledService().SettlePods(new SettlePodsRequest() { SettledPods = this.settledPodCollection, SettledType = this.settledType });
            //}

            //return this.message;
        }

        /// <summary>
        /// 生成ErrorMessage，针对Grouped运单
        /// </summary>
        /// <param name="pod"></param>
        /// <param name="errorMessage"></param>
        //public virtual void GenErrorMessage_ByGroupedPod(GroupedPods pod, string errorMessage)
        //{
        //    this.message.Append("运单:").Append(pod.PodIDs).Append(",对应客户为:").Append(pod.TargetName).Append(",起运城市为:")
        //      .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
        //       .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(errorMessage).Append("*");
        //}

        /// <summary>
        /// 生成ErrorMessage，针对一条运单
        /// </summary>
        /// <param name="pod"></param>
        /// <param name="sb"></param>
        /// <param name="errorMessage"></param>
        //public virtual void GenErrorMessage_ByPod(Pod pod, StringBuilder sb, string errorMessage)
        //{
        //    this.message.Append("运单:").Append(pod.CustomerOrderNumber).Append(",对应客户为:").Append(pod.CustomerName).Append(",起运城市为:")
        //      .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
        //      .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(errorMessage).Append("*");
        //}

        /// <summary>
        /// 生成ErrorMessage，针对一条运单
        /// </summary>
        /// <param name="pod"></param>
        /// <param name="errorMessage"></param>
        //public virtual void GenErrorMessage_ByPod(Pod pod, string errorMessage)
        //{
        //    this.message.Append("运单:").Append(pod.CustomerOrderNumber).Append(",对应客户为:").Append(pod.CustomerName).Append(",起运城市为:")
        //      .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
        //      .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(errorMessage).Append("*");
        //}
    }
}