using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.POD.Nike;
using Runbow.TWS.MessageContracts.POD.Nike;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class NikePODForBSModel
    {
        public IEnumerable<NikeforBSPOD> PodCollection { get; set; }

        public NikePODForBSRequest Request { get; set; }

        public int PageCount { get; set; }

        public int PageIndex { get; set; }

        public PodSearchCondition SearchCondition { get; set; }

        public NikePodForBSCondition Condition { get; set; }

        public IEnumerable<SelectListItem> TtlOrTpl
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "请选择",Selected=true }, 
                    new SelectListItem() { Value = "LTL", Text = "LTL" }, 
                    new SelectListItem() { Value = "FTL", Text = "FTL" }, 
                };

            }
        }
        public IEnumerable<SelectListItem> SelectPODType
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "已分配提货车辆", Text = "已分配提货车辆" }, 
                    new SelectListItem() { Value = "未分配提货车辆", Text = "未分配提货车辆" }, 
                    new SelectListItem() { Value = "已分配干线车辆", Text = "已分配干线车辆" },
                    new SelectListItem() { Value = "未分配干线车辆", Text = "未分配干线车辆" },
                    new SelectListItem() { Value = "已分配配送车辆", Text = "已分配配送车辆"} ,
                    new SelectListItem() { Value = "未分配配送车辆", Text = "未分配配送车辆" }
                };
            }
        }
        public IEnumerable<SelectListItem> PODType
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "提货车辆", Text = "提货车辆" }, 
                    new SelectListItem() { Value = "干线车辆", Text = "干线车辆" },
                    new SelectListItem() { Value = "配送车辆", Text = "配送车辆"} 
                };
            }
        }
        public IEnumerable<SelectListItem> IsConversion
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "1", Text = "Y" }, 
                    new SelectListItem() { Value = "0", Text = "N",Selected=true }, 
                };

            }
        }

        public IEnumerable<SelectListItem> Carriers
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "0", Text = "沿用原承运商",Selected=true }, 
                    new SelectListItem() { Value = "1", Text = "更改承运商" }, 
                    new SelectListItem() { Value = "2", Text = "承运商后期分配" }, 
                };

            }
        }

        public IEnumerable<SelectListItem> PodState
        {
            get
            {
                return new List<SelectListItem>() 
                { 
                    new SelectListItem() { Value = "", Text = "请选择",Selected=true }, 
                    new SelectListItem() { Value = "待审核", Text = "待审核" }, 
                    new SelectListItem() { Value = "待调度", Text = "待调度" }, 
                      new SelectListItem() { Value = "待发运", Text = "待发运" }, 
                       new SelectListItem() { Value = "待签收", Text = "待签收" },
                        new SelectListItem() { Value = "待结案", Text = "待结案" },
                         new SelectListItem() { Value = "待结算", Text = "待结算" },
                          new SelectListItem() { Value = "待开票", Text = "待开票" },
                           new SelectListItem() { Value = "待付款", Text = "待付款" },
                            new SelectListItem() { Value = "完成", Text = "完成" },
                };

            }
        }
    }
}