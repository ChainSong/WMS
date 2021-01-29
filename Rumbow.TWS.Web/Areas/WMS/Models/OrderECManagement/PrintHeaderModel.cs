using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Print;

namespace Runbow.TWS.Web.Areas.WMS.Models.OrderECManagement
{
    public class PrintHeaderModel
    {
        public PrintHeaderSearchCondition SearchCondition { get; set; }
        public Table Config1 { get; set; }
        public Table Config2 { get; set; }
        public bool ShowCustomerOrShipperDrop { get; set; }
        public bool IsInnerUser { get; set; }
        public long ProjectRoleID { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int ViewType { get; set; }
        public IEnumerable<PrintHeader> PrintHeaderCollection { get; set; }
        public IEnumerable<PrintDetail> PrintDetailCollection { get; set; }
        public IEnumerable<SelectListItem> Customers
        {
            get;
            set;
        }
    }
}