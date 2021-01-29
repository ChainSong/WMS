using Runbow.TWS.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class AbnormalPODSearchModel
    {
        public DataTable AbnormalTable { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }

        public bool IsExport { get; set; }
        public bool IsExportTrack { get; set; }
        public Table Config { get; set; }
        public IEnumerable<SelectListItem> Customers { get; set; }
        public PodSearchCondition SearchCondition { get; set; }
        [Display(Name = "开始发货日期")]
        public DateTime? BeginActualDeliveryDate { get; set; }

        [Display(Name = "结束发货日期")]
        public DateTime? EndActualDeliveryDate { get; set; }
    }
}