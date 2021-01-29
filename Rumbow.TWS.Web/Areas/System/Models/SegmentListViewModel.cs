using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.System.Models
{
    public class SegmentListViewModel
    {
        [Display(Name = "段位名称")]
        public string Name { get; set; }

        [Display(Name = "是否可用")]
        public bool State { get; set; }

        public IEnumerable<Segment> Segments { get; set; }

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