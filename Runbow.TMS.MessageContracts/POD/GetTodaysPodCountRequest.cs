namespace Runbow.TWS.MessageContracts
{
    public class GetTodaysPodCountRequest
    {
        public long ProjectID { get; set; }

        public string SystemNumberPrefix { get; set; }
    }
}