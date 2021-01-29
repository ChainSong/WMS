using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.MessageContracts;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.ForecastWarehouse.Models
{
    public class ForecastOrdersModel
    {
            //id, waveId, WaveReleaseTime, DeliverTime, PickTime, State
        public class ViewModel
        {
           
            public ForecastOrders ForecastOrders { get; set; }
        }
        public IEnumerable<ForecastOrders> IEnumerableForecastOrders { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public string Message { get; set; }
        public List<ForecastOrders> ListForecastOrders { get; set; }
        public ForecastOrders ForecastOrders { get; set; }
        public string ShipToSity { get; set; }
        [Display(Name = "ID")]
        public string WID { get; set; }

        [Display(Name = "项目名称")]
        public string ProjectName { get; set; }
   

        [Display(Name = "波次号")]
        public int waveId { get; set; }

     
        [Display(Name = "WaveReleaseTime")]
        public string WaveReleaseTime { get; set; }

        public string WaveReleaseTime2 { get; set; }
        public string WaveReleaseTime3 { get; set; }

        public string WaveReleaseTime4 { get; set; }
        [Display(Name = "发货时间")]
        public string DeliverTime { get; set; }

        public string DeliverTime2 { get; set; }

        [Display(Name = "提货时间")]
        public DateTime PickTime { get; set; }

        public string zhi2 { get; set; }

  
        public DateTime PickTime2 { get; set; }


        [Display(Name = "类型")]
        public string States { get; set; }
        public IEnumerable<SelectListItem> State
        {
            get
            {
                return new SelectListItem[] {  
                    new SelectListItem(){Value = "待发货/正在发货中", Text="待发货/正在发货中"},
                    new SelectListItem(){ Value = "全部", Text="全部"},
                    new SelectListItem(){Value = "已作废", Text="已作废"},
                   new SelectListItem(){Value = "已发货", Text="发货完成"},
                   new SelectListItem(){Value = "正在发货中", Text="正在发货中"},
                   new SelectListItem(){Value = "待发货", Text="待发货"}
                };
            }
        }
        public IEnumerable<SelectListItem> State2
        {
            get
            {
                return new SelectListItem[] {
                    new SelectListItem(){ Value = "请选择", Text="请选择"},
                    new SelectListItem(){Value = "已作废", Text="已作废"},
                   new SelectListItem(){Value = "已发货", Text="已发货"},
                   new SelectListItem(){Value = "正在发货中", Text="正在发货中"},
                   new SelectListItem(){Value = "待发货", Text="待发货"}
                };
            }
        }
        public IEnumerable<SelectListItem> zhi
        {
            get
            {
                return new SelectListItem[] {
                    new SelectListItem(){ Value = "请选择", Text="请选择"},
                    new SelectListItem(){Value = "desc", Text="降序"},
                   new SelectListItem(){Value = "asc", Text="升序"},
                  
                };
            }
        }
    }
}