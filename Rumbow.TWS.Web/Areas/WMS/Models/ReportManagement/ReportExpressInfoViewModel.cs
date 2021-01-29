using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Report;
using Runbow.TWS.Web.Common;
namespace Runbow.TWS.Web.Areas.WMS.Models.ReportManagement
{
    [Serializable]
    public class ReportExpressInfoViewModel
    {
        public Table Config { get; set; }//表字段
        public long ProjectRoleID { get; set; }
        public ReportExpressInfoSearchCondition SearchCondition { get; set; }//查询条件
        public IEnumerable<ReportExpressInfo> ReportExpressChangeCollection { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
    }
}