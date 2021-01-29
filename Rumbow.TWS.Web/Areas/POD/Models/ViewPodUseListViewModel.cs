using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class ViewPodUseListViewModel
    {
        public Table Config { get; set; }

        public IEnumerable<Pod> PodCollection { get; set; }

        public bool IsInnerUser { get; set; }

        public long ProjectRoleID { get; set; }
    }
}