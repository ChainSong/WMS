namespace Runbow.TWS.MessageContracts
{
    public class SetTransportationLineStateRequest
    {
        public long ID { get; set; }

        public bool State { get; set; }
    }
}