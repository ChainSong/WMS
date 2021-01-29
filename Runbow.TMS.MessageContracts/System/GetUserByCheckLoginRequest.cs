namespace Runbow.TWS.MessageContracts
{
    public class GetUserByCheckLoginRequest
    {
        public string Name { get; set; }

        public string DisPlayName { get; set; }

        public string Password { get; set; }

        public long ProjectID { get; set; }

        public bool State { get; set; }
    }
}