using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ShipperEntity = Runbow.TWS.Entity.Shipper;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class ShipperListViewModel
    {
        [Display(Name = "代码")]
        public string Code { get; set; }

        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "英文名称")]
        public string EnglishName { get; set; }

        [Display(Name = "是否可用")]
        public bool State { get; set; }

        public IEnumerable<ShipperEntity> Shippers { get; set; }
            public int PageIndex { get; set; }

            public int PageCount { get; set; }
        public IEnumerable<SelectListItem> States
        {
            get
            {
                return new SelectListItem[] {
                    new SelectListItem(){ Value = "True", Text="可用"},
                    new SelectListItem(){Value = "False", Text="禁用"}
                };
            }
        }

        
    }
}