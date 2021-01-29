using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PodDetailManageViewModel : PodReleatedInforBaseViewModel
    {
        public IEnumerable<PodDetail> PodDetails { get; set; }

        public PodDetail PodDetail { get; set; }
    }
}