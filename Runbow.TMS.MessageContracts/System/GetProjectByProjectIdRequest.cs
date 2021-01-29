namespace Runbow.TWS.MessageContracts
{
    public class GetProjectByProjectIdRequest
    {
        public long ProjectID { get; set; }
        public bool IsSuccess { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
    }

    public class SaveProject
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}