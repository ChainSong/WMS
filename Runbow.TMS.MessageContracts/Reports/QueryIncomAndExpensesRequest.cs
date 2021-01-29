namespace Runbow.TWS.MessageContracts
{
    public class QueryIncomAndExpensesRequest
    {
        public long CustomerID { get; set; }

        public string Year { get; set; }
    }
}