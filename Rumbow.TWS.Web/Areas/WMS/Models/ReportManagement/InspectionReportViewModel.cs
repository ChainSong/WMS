using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity.WMS.Report;
using System.Web.Mvc;

namespace Runbow.TWS.Web.Areas.WMS.Models.ReportManagement
{
    public class InspectionReportViewModel
    {
        public InspectionReportSearchCondition SearchCondition { get; set; }
        public IEnumerable<InspectionReport> InspectionReportCollection { get; set; }

        public IEnumerable<SelectListItem> CustomerList { get; set; }
        public IEnumerable<SelectListItem> WarehouseList { get; set; }

        public int PageIndex { get; set; }

        public int PageCount { get; set; }
    }
}