using Runbow.TWS.Entity.WMS.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Runbow.TWS.Web.Areas.WMS.Models.ReportManagement
{
    public class WorkingViewModel
    {
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public long ProjectRoleID { get; set; }

        public WorkingSearchCondition SearchCondition { get; set; }

        public IEnumerable<WMS_WorkingStatis> WorkingCollection { get; set; }
        public IEnumerable<WMS_WorkingStatisDetail> WorkingDetailCollection { get; set; }

    }
}