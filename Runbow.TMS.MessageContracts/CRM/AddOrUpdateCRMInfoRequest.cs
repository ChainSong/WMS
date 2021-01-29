using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class AddOrUpdateCRMInfoRequest
    {
        public CRMInfo CRMInfo { get; set; }
        public CRMTrackInfo CRMTrackInfo { get; set; }
    }
   
}