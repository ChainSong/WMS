namespace Runbow.TWS.MessageContracts
{
    public class GetMenuByProjectRoleIDRequest
    {
        public long ProjectRoleID { get; set; }

        public bool GetAll { get; set; }
    }
}