using System;

namespace ReProServices.Application.ClientPayments.Queries
{
    public class ClientPaymentFilter
    {
        public int NatureOfPaymentID { get; set; }
        public  string CustomerName { get; set; }
        public int PropertyID { get; set; } = 0;
        public  string Premises { get; set; }
        public  int UnitNo { get; set; }
        public string SellerName { get; set; }
        public int  SellerID { get; set; }
        public DateTime? FromRevisedDate { get; set; }
        public DateTime? ToRevisedDate { get; set; }
        public int LotNo { get; set; }
        public int? RemittanceStatusID { get; set; }
    }
}
