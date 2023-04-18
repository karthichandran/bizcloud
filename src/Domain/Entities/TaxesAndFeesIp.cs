
using System;

namespace ReProServices.Domain.Entities
{

    public class TaxesAndFeesIp
    {
        public bool IsTdsDeductedBySeller { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime DateOfPayment { get; set; } //payment date between customer with builder
        public DateTime DateOfDeduction { get; set; } //this parameter represent the actual payment of tds to govt
        public decimal GstPercentage { get; set; } = 5;
    }
}
