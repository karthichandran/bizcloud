using System.ComponentModel.DataAnnotations;
using ReProServices.Application.Customers.Queries;
using DateTime = System.DateTime;
using Guid = System.Guid;


namespace ReProServices.Application.TdsRemittance
{
    public class TdsRemittanceDto
    {
        [Key]
        public int ClientPaymentTransactionID { get; set; }
        public int ClientPaymentID { get; set; }
        public Guid OwnershipID { get; set; }
        public string PropertyPremises { get; set; }
        public int UnitNo { get; set; }
        public bool TdsCollectedBySeller { get; set; }
        public Guid InstallmentID { get; set; }
        public DateTime RevisedDateOfPayment { get; set; }
        public DateTime DateOfDeduction { get; set; }
        public string ReceiptNo { get; set; }
        public int LotNo { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal SellerShare { get; set; }
        public string SellerName { get; set; }
        public string CustomerName { get; set; }
        public decimal CustomerShare { get; set; }
        public decimal GstAmount { get; set; }
        public decimal TdsAmount { get; set; }
        public decimal TdsInterest { get; set; }
        public decimal LateFee { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal OwnershipAmount { get; set; }
        public int StatusTypeID { get; set; }
        public decimal GrossShareAmount { get; set; }
        public int RemittanceStatusID { get; set; }
        public string RemittanceStatus  { get; set; }
        public decimal AmountShare { get; set; }

        public virtual DateTime ChallanDate { get; set; }
        public virtual string ChallanAckNo { get; set; }

        public virtual decimal ChallanAmount { get; set; }
        public virtual string CustomerPAN { get; set; }
        public virtual string SellerPAN { get; set; }
        public virtual string TracesPassword { get; set; }
        public virtual string AssessmentYear { get; set; }

        public DateTime? F16BDateOfReq { get; set; }
        public string F16BRequestNo { get; set; }

        public bool OnlyTDS { get; set; }

        public bool IsDebitAdvice { get; set; }
        public int? RemarkId { get; set; }
        public string RemarkDesc { get; set; }
    }
}