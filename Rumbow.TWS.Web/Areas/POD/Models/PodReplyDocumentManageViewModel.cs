using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PodReplyDocumentManageViewModel : PodReleatedInforBaseViewModel
    {
        public PodReplyDocument PodReplyDocument { get; set; }

        public bool IsCoverOld { get; set; }
    }
}