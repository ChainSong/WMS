namespace Runbow.TWS.MessageContracts
{
    public class SetUserProjectRoleRequest
    {
        public long UserID { get; set; }

        public long ProjectRoleID { get; set; }

        public long ProjectID { get; set; }
    }
}