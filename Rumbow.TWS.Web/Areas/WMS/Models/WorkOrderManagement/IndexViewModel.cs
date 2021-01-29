using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;
using Runbow.TWS.Entity.WMS.WorkOrder;

namespace Runbow.TWS.Web.Areas.WMS.Models.WorkOrderManagement
{
    public class IndexViewModel
    {
        public WorkOrderSearchCondition WorkOrderCondition { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int ViewType { get; set; }
        public IEnumerable<WorkOrder> WorkOrderCollection { get; set; }

        public IEnumerable<SelectListItem> WorkOrderType
        {
            get;
            set;
        }

        public IEnumerable<SelectListItem> WorkOrderStatus
        {
            get;
            set;
        }

        public IEnumerable<SelectListItem> FileType
        {
            get;
            set;
        }
    }
}