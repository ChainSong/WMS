using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.Phone.Models
{
    public class WeiChartUserConfigMode
    {
        public IEnumerable<WeiQueryPod> WeiQueryPod { get; set; }

        public string Type { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string Id { get; set; }
    }
}