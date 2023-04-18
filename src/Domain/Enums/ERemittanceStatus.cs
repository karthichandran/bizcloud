namespace ReProServices.Domain.Enums
{
    public enum ERemittanceStatus
    {
        Ignore = 0,
        Pending  = 1,
        TdsPaid = 2,
        Form16BRequested = 3,
        Form16BDownloaded = 4,
        Form16BSentToCustomer = 5,
        ExcludeOnlyTDSpayments = 6
    }
}
