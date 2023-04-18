using System;

namespace ReProServices.Domain.Entities
{
    public class StatementOfAccount
    {
        public Guid OwnershipID { get; set; }
        public int? UnitNo { get; set; }
        public DateTime? PayableDateOfPayment { get; set; }
        public string PayableReceiptNo { get; set; }
        public decimal? PayableAmountPaid { get; set; }
        public decimal? PayableGrossAmount { get; set; }
        public decimal? PayableGst { get; set; }
        public decimal? PayableTds { get; set; }
        public decimal? PayableLateFee { get; set; }
        public decimal? PayableInterest { get; set; }
        public decimal? PayableServiceFee { get; set; }

        public DateTime? ReceivedDate { get; set; }
        public decimal? ReceivedTotalAmount { get; set; }
        public decimal? ReceivedTds { get; set; }
        public decimal? ReceivedInterest { get; set; }
        public decimal? ReceivedLateFee { get; set; }
        public decimal? ReceivedServiceCharge { get; set; }
        public DateTime? RemittedDate { get; set; }
        public decimal? RemittedTds { get; set; }
        public decimal? RemittedInterest { get; set; }
        public decimal? RemittedLateFee { get; set; }
    }
}

 