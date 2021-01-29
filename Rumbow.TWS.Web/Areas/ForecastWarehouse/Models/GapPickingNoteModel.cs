using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity.ForecastWarehouse;

namespace Runbow.TWS.Web.Areas.ForecastWarehouse.Models
{
    public class GapPickingNoteModel
    {
        public UserCode Code { get; set; }

        public GapPickingNote Gappackingnote { get; set; }

        #region label
        [Display(Name = "门店代码(Store Code)")]
        public string StoreCode { get; set; }

        [Display(Name = "门店名称(Store Name)")]
        public string StoreName { get; set; }

        [Display(Name = "所在城市(City)")]
        public string City { get; set; }

        [Display(Name = "“转货”或“退货”")]
        public string TransferorReturn { get; set; }

        [Display(Name = "服务明细(见说明)")]
        public string ServiceDetail { get; set; }

        [Display(Name = "箱数")]
        public string CartonQuantity { get; set; }

        [Display(Name = "目的代码")]
        public string DestinationCode { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

        [Display(Name = "品牌")]
        public string Brand { get; set; }

        [Display(Name = "订单创建日期")]
        public DateTime CreatTime { get; set; }

        [Display(Name = "预计提货日期")]
        public DateTime ExpectedDeliveryDate { get; set; }

        [Display(Name = "预计到货日期")]
        public DateTime ExpectedArrivalDate { get;set; }
        #endregion

        public int ViewType { get; set; }

        public IEnumerable<SelectListItem> Transferorreturns
        {
            get
            {
                return new SelectListItem[] {  
                    new SelectListItem(){ Value = "转货", Text="转货(Transfer)"},
                    new SelectListItem(){ Value = "退货", Text="退货(Return)"}
                };
            }
        }
    }
}