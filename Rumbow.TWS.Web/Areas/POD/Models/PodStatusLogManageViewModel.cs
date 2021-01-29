using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PodStatusLogManageViewModel : PodReleatedInforBaseViewModel
    {
        public IEnumerable<PodStatusLog> PodStatusLogs { get; set; }

        public PodStatusLog PodStatusLog { get; set; }
    }
}