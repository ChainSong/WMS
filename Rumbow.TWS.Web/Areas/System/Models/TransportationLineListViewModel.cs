using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class TransportationLineListViewModel
    {
        [Display(Name = "段位名称")]
        public string Name { get; set; }

        [Display(Name = "是否可用")]
        public bool State { get; set; }

        [Display(Name = "起始城市")]
        public long? StartCityID { get; set; }

        public string StartCityName { get; set; }

        [Display(Name = "到达城市")]
        public long? EndCityID { get; set; }

        public string EndCityName { get; set; }

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

        public IEnumerable<TransportationLine> TransportationLineCollection { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }
    }
}