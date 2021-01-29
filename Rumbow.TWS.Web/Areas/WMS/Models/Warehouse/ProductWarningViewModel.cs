using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Runbow.TWS.Entity;
using Runbow.TWS.Web.Common;

namespace Runbow.TWS.Web.Areas.WMS.Models.Warehouse
{
    public class ProductWarningViewModel
    {
        public ProductWarningSearchCondition SearchCondition { get; set; }
        public IEnumerable<ProductWarningInfo> ProductWarningCollection { get; set; }
        public IEnumerable<ProductStorerInfo> ProductCollection { get; set; }
        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public int ViewType { get; set; }
            
    }
}