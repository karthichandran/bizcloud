using System;

namespace ReProServices.Domain.Entities
{

    public class TaxesAndFees
    {
        public bool IsTdsDeductedBySeller { get; set; }
        public Decimal AmountValue { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime DateOfPayment { get; set; }
        public DateTime DateOfDeduction { get; set; }
        public DateTime DueDateOfPaymentOfTds { get; set; }
        public int NoOfMonthsOfDeductionDelay { get; set; }
        public int NoOfDaysOfDelay { get; set; }
        public decimal InterestOfLateDeduction { get; set; }
        public decimal LateFeePerDay { get; set; }
        public decimal LateFeeAmount { get; set; }
        public decimal GstPercentage { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal TdsAmount { get; set; }
        public decimal TdsInterestAmount { get; set; }
        public decimal TdsPercentage { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Share { get; set; }
        public decimal AmountShare { get; set; }
    }
}
