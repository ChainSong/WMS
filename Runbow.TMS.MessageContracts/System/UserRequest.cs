namespace Runbow.TWS.MessageContracts
{
    public class UserRequest
    {
        public long ID { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }

        public bool State { get; set; }

        public char Sex { get; set; }

        public string Tel { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public int UserType { get; set; }

        public long CustomerOrShipperID { get; set; }

        public string RuleArea { get; set; }

        public long ProjectId { get; set; }

        public long ProjectRoleId { get; set; }

        public string UserName { get; set; }

    }
}