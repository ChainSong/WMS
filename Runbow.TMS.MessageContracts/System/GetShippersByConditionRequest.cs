namespace Runbow.TWS.MessageContracts
{
    public class GetShippersByConditionRequest
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string EnglishName { get; set; }

        public bool State { get; set; }

        public long ProjectId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}