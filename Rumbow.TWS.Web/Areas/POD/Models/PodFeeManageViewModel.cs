using Runbow.TWS.Entity;

namespace Runbow.TWS.Web.Areas.POD.Models
{
    public class PodFeeManageViewModel : PodReleatedInforBaseViewModel
    {
        public PodFee PodFee { get; set; }

        public bool IsCoverOld { get; set; }
    }
}