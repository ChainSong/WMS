using Runbow.TWS.Entity;

namespace Runbow.TWS.MessageContracts
{
    public class GetCRMTrackInfoRequest
    {
        public CRMTrackInfo CRMTrackInfo { get; set; }
        public long? ID { get; set; }
    }
}