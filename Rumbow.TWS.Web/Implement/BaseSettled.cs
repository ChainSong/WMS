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
    public class BaseSettled : IsettledForPodNew
    {
        /// <summary>
        /// 取运单集合和合并的运单集合
        /// </summary>
        private PodService service;

        /// <summary>
        /// 运单集合
        /// </summary>
        protected IEnumerable<Pod> podCollection;

        /// <summary>
        /// 合并运单集合
        /// </summary>
        protected SettledPodResponse SettledPodResponse = null;

        /// <summary>
        /// 结算类型 0 应收  1 应付
        /// </summary>
        protected int settledType = 0;

        /// <summary>
        /// 结算对象ID
        /// </summary>
        protected long customerOrShipperID;

        /// <summary>
        /// 运单是否需要同天同城合并
        /// </summary>
        protected bool IsGroupedPods = true;

        /// <summary>
        /// 待结算运单ID集合
        /// </summary>
        protected IEnumerable<long> podIDCollection;

        /// <summary>
        /// 结算人
        /// </summary>
        protected string creator;

        /// <summary>
        /// 大项目ID
        /// </summary>
        protected long projectID = 1;

        /// <summary>
        /// 关联客户ID
        /// </summary>
        protected long relatedCustomerID;

        /// <summary>
        /// 结算序列号
        /// </summary>
        protected string settledNumber = string.Empty;

        /// <summary>
        /// 报价集合
        /// </summary>
        protected IEnumerable<QuotedPrice> quotedPriceCollection;

        /// <summary>
        /// 油价集合
        /// </summary>
        protected IEnumerable<QueryBAFPrice> BAF { get; set; }

        /// <summary>
        /// 错误提示
        /// </summary>
        protected StringBuilder message = new StringBuilder();

        /// <summary>
        /// 结算成功列表
        /// </summary>
        protected IList<SettledPod> settledPodCollection = new List<SettledPod>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="SettledType">结算类型 0 应收  1 应付</param>
        /// <param name="customerOrShipperID">结算对象ID</param>
        /// <param name="IsGroupedPods">运单是否需要同天同城合并</param>
        /// <param name="PodIDs"> 待结算运单ID集合</param>
        /// <param name="ProjectID">大项目ID</param>
        /// <param name="Creator">结算人</param>
        /// <param name="Message">错误提示</param>
        public BaseSettled(int SettledType, long customerOrShipperID, bool IsGroupedPods, IEnumerable<long> PodIDs, long ProjectID, string Creator, long RelatedCustomerID)
        {
            //赋值
            this.settledType = SettledType;
            this.customerOrShipperID = customerOrShipperID;
            this.IsGroupedPods = IsGroupedPods;
            this.podIDCollection = PodIDs;
            this.projectID = ProjectID;
            this.creator = Creator;
            this.relatedCustomerID = RelatedCustomerID;
            service = new PodService();

            //取得待结算运单列表
            var podsResponse = service.QueryPodByPodIDs(new QueryPodByIDsRequest() { PodIDs = PodIDs });
            if (!podsResponse.IsSuccess)
            {
                throw podsResponse.Exception;
            }
            this.podCollection = podsResponse.Result;

            #region 如果同天同城合并，取得合并运单列表
            if (this.IsGroupedPods)
            {
                Response<SettledPodResponse> groupedPodsResponse = new Response<SettledPodResponse>();

                if (this.relatedCustomerID == 35)
                {
                    //ID:35 艺康同天同点合并运单
                    groupedPodsResponse = service.SettledPodByAddress(new SettledPodRequest() { IDs = PodIDs, SettledType = settledType });
                }
                else
                {
                    //同天同城合并运单
                    groupedPodsResponse = service.SettledPodSearch(new SettledPodRequest() { IDs = PodIDs, SettledType = settledType });
                }
                if (!groupedPodsResponse.IsSuccess)
                {
                    throw groupedPodsResponse.Exception;
                }
                this.SettledPodResponse = groupedPodsResponse.Result;
            }
            #endregion

            //生成结算序列号
            settledNumber = Guid.NewGuid().ToString();

            //取得报价
            quotedPriceCollection = ApplicationConfigHelper.GetProjectQuotedPrice(this.projectID, this.settledType, this.customerOrShipperID, this.relatedCustomerID).Where(q => q.ProjectID == this.projectID && q.Target == this.settledType && q.TargetID == this.customerOrShipperID && q.RelatedCustomerID == this.relatedCustomerID);

            //取得油价
            if (customerOrShipperID == 1 && RelatedCustomerID == 1)
            {
                BAF = ApplicationConfigHelper.GetBAFPrice(ProjectID);
            }

            //将运单中一些必要的字段在基类中赋值，以减少子类的代码
            this.podCollection.Each((i, pod) =>
            {
                SettledPod settledPod = new SettledPod()
                {
                    ProjectID = this.projectID,
                    CustomerOrderNumber = pod.CustomerOrderNumber,
                    SystemNumber = pod.SystemNumber,
                    PodID = pod.ID,
                    SettledNumber = settledNumber,
                    SettledType = this.settledType,
                    CustomerOrShipperID = this.customerOrShipperID,
                    CustomerOrShipperName = this.settledType == 0 ? pod.CustomerName : pod.ShipperName,
                    StartCityID = pod.StartCityID,
                    StartCityName = pod.StartCityName,
                    EndCityID = pod.EndCityID,
                    EndCityName = pod.EndCityName,
                    ShipperTypeID = pod.ShipperTypeID,
                    ShipperTypeName = pod.ShipperTypeName,
                    PODTypeID = pod.PODTypeID,
                    PODTypeName = pod.PODTypeName,
                    TtlOrTplID = pod.TtlOrTplID,
                    TtlOrTplName = pod.TtlOrTplName,
                    ActualDeliveryDate = pod.ActualDeliveryDate,
                    BoxNumber = pod.BoxNumber,
                    Weight = pod.Weight,
                    Volume = pod.Volume,
                    GoodsNumber = pod.GoodsNumber,
                    //费用信息需要在子方法中计算
                    ShipAmt = 0,
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
                    //str4代表报价，需要在子方法中找到
                    Str4 = string.Empty,
                    Str5 = string.Empty,
                    Remark = string.Empty,
                    DateTime1 = null,
                    DateTime2 = null,
                    CreateTime = DateTime.Now,
                    Creator = this.creator,
                    InvoiceID = 0,
                    RelatedCustomerID = pod.CustomerID,
                    IsAudit = true
                };

                this.settledPodCollection.Add(settledPod);
            });

        }

        /// <summary>
        /// 用户自定义结算逻辑
        /// </summary>
        /// <returns></returns>
        public virtual void CustomerDefinedSettledPod()
        {

        }

        /// <summary>
        /// 结算调用方法
        /// </summary>
        /// <returns>如果Message有值，则计算不成功，外部显示Message的值。如果没有值，则结算成功</returns>
        public StringBuilder SettledForPod()
        {
            //根据用户自定义结算方法取得结算成功运单列表
            this.CustomerDefinedSettledPod();

            //如果没有错误信息并且结算成功运单列表有值，则插入运单结算表
            if (this.message.Length == 0 && this.settledPodCollection != null && this.settledPodCollection.Any())
            {
                this.settledPodCollection.Each((i, p) => { p.TotalAmt = p.ShipAmt + p.PointAmt + p.BAFAmt + p.OtherAmt + p.Amt1 + p.Amt2 + p.Amt3 + p.Amt4 + p.Amt5; });
                new SettledService().SettlePods(new SettlePodsRequest() { SettledPods = this.settledPodCollection, SettledType = this.settledType });
            }

            return this.message;
        }

        /// <summary>
        /// 生成ErrorMessage，针对Grouped运单
        /// </summary>
        /// <param name="pod"></param>
        /// <param name="errorMessage"></param>
        public virtual void GenErrorMessage_ByGroupedPod(GroupedPods pod, string errorMessage)
        {
            this.message.Append("运单:").Append(pod.PodIDs).Append(",对应客户为:").Append(pod.TargetName).Append(",起运城市为:")
              .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
               .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(errorMessage).Append("*");
        }

        /// <summary>
        /// 生成ErrorMessage，针对一条运单
        /// </summary>
        /// <param name="pod"></param>
        /// <param name="sb"></param>
        /// <param name="errorMessage"></param>
        public virtual void GenErrorMessage_ByPod(Pod pod, StringBuilder sb, string errorMessage)
        {
            this.message.Append("运单:").Append(pod.CustomerOrderNumber).Append(",对应客户为:").Append(pod.CustomerName).Append(",起运城市为:")
              .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
              .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(errorMessage).Append("*");
        }

        /// <summary>
        /// 生成ErrorMessage，针对一条运单
        /// </summary>
        /// <param name="pod"></param>
        /// <param name="errorMessage"></param>
        public virtual void GenErrorMessage_ByPod(Pod pod, string errorMessage)
        {
            this.message.Append("运单:").Append(pod.CustomerOrderNumber).Append(",对应客户为:").Append(pod.CustomerName).Append(",起运城市为:")
              .Append(pod.StartCityName).Append(",目的城市为:").Append(pod.EndCityName).Append(",运输类型为:").Append(pod.ShipperTypeName)
              .Append(",运单类型为:").Append(pod.PODTypeName).Append(",整车/零担为:").Append(pod.TtlOrTplName).Append(errorMessage).Append("*");
        }

    }
}