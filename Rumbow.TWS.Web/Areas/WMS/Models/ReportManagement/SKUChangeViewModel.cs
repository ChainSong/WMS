using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Report;

namespace Runbow.TWS.Web.Areas.WMS.Models.ReportManagement
{
    public class SKUChangeViewModel
    {
        public Table Config { get; set; }

        public bool IsInnerUser { get; set; }
        public long ProjectRoleID { get; set; }
        public ReportSKUChangeSearchCondition SearchCondition { get; set; }//条件
        public IEnumerable<ReportSKUChange> ReportSKUChangeCollection { get; set; }//查询实体

        public int PageIndex { get; set; }
        public int PageCount { get; set; }

    }
}