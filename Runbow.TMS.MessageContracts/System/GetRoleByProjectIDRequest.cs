namespace Runbow.TWS.MessageContracts
{
    public class GetRoleByProjectIDRequest
    {
        public long ProjectID { get; set; }

        public bool GetAll { get; set; }
    }
}