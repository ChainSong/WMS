using System.Collections.Generic;
using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PodExceptionManageViewModel : PodReleatedInforBaseViewModel
    {
        public IEnumerable<PodException> PodExceptions { get; set; }

        public PodException PodException { get; set; }
    }
}