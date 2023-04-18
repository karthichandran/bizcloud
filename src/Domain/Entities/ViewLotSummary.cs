namespace ReProServices.Domain.Entities
{
    public class ViewLotSummary
    {
        public int LotNo {get;set;}
        public int TotalPayments { get;set;}
        public int PaymentsConsidered { get;set;}
        public int PaymentsNotConsidered { get;set;}
        public int TransactionsCount {get;set;}
        public int TransactionsConsidered {get;set;}
        public int TransactionsNotConsidered {get;set;}
        public int TransWithTdsPending {get;set;}
        public int TransWithTdsPaid {get;set;}
        public int TransWithCoOwner {get;set;}
        public int TransWithNoCoOwner {get;set;}
        public int TransWithF16BGenerated {get;set;}
        public int TransWithF16BGeneratedWithSharedOwnership { get;set;}
        public int TransWithF16BGeneratedWithNoSharedOwnership { get;set;}
    }
}
