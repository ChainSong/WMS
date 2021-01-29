namespace Runbow.TWS.MessageContracts
{
    public class GetTransportationLinesByConditionRequest
    {
        public string Name { get; set; }

        public long? StartCityID { get; set; }

        public long? EndCityID { get; set; }

        public bool State { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}