namespace Runbow.TWS.MessageContracts
{
    public class GetUsersByConditionRequest
    {
        public string Name { get; set; }

        public string DisplyName { get; set; }

        public bool State { get; set; }

        public long  ProjectID { get; set; }

        public int UserType { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}