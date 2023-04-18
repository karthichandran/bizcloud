using System;

namespace ReProServices.Application.CustomerBillings.Queries
{
    public class CustomerBillingFilter
    {
        public int CustomerBillID { get; set; }
        public  string CustomerName { get; set; }
        public int PropertyID { get; set; } = 0;
        public  string Premises { get; set; }
        public  int UnitNo { get; set; }
        public bool PaymentStatus { get; set; } 
        public string PAN { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
