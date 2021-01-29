using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Entity.WMS.Order;

namespace Runbow.TWS.Web.Areas.WMS.Models.OrderManagement
{
    public class WaveModel
    {
        public WaveSearchCondition SearchCondition { get; set; }
        public Table Config1 { get; set; }
        public Table Config2 { get; set; }
        public bool ShowCustomerOrShipperDrop { get; set; }
        public bool IsInnerUser { get; set; }
        public long ProjectRoleID { get; set; }
        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<WMS_Wave> WaveCollection { get; set; }
        public IEnumerable<WMS_WaveDetail> WaveDetailCollection { get; set; }
        public IEnumerable<SelectListItem> Customers
        {
            get;
            set;
        }
    }
}