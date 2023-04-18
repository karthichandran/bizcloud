namespace ReProServices.Application.Receipts.Queries
{
    public class ReceiptFilter
    {
        public int PropertyID { get; set; }
        public int UnitNo { get; set; }
        public int SellerID { get; set; }
        public string CustomerName { get; set; }
        public bool IsTds { get; set; } = true;
        public bool GetPending { get; set; } = true;

        public int LotNo { get; set; }
    }

}
