namespace Runbow.TWS.MessageContracts
{
    public class SetSegmentStateRequest
    {
        public long ID { get; set; }

        public bool State { get; set; }
    }
}