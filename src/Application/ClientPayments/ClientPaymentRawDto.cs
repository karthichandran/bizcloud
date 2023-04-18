using System;
using System.ComponentModel.DataAnnotations;

namespace ReProServices.Application.ClientPayments
{
    public class ClientPaymentRawDto
    {
        [Key]
        public int ClientPaymentID { get; set; }
        public Guid OwnershipID { get; set; }
        public bool CoOwner { get; set; }
        public  int PropertyID { get; set; }
        public  string PropertyPremises { get; set; }
        public  int UnitNo { get; set; }
        public int CustomerPropertyId { get; set; }
        public string Remarks { get; set; }
        public int TdsTaxCode { get; set; }
        public decimal TdsRate { get; set; }
        public decimal? TotalUnitCost { get; set; }
        public bool TdsCollectedBySeller { get; set; }
        public DateTime? DateOfAgreement { get; set; }
        public int PropertyType { get; set; }
        public decimal LateFeePerDay { get; set; }
        public Guid InstallmentID { get; set; }
        public int PaymentMethodID { get; set; }
        public DateTime DateOfPayment { get; set; }
        public DateTime? RevisedDateOfPayment { get; set; }
        public DateTime DateOfDeduction { get; set; }
        public string ReceiptNo { get; set; }
        public int LotNo { get; set; }
        public decimal AmountPaid { get; set; }
        public int SellerPropertyId { get; set; }
        public decimal SellerShare { get; set; }
        public int SellerID { get; set; }
        public string SellerName { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal CustomerShare { get; set; }
        public string PAN { get; set; }
        public int GstTaxCode { get; set; }
        public decimal GstRate { get; set; }
        public decimal GstAmount { get; set; }
        public decimal Tds { get; set; }
        public decimal TdsInterest { get; set; }
        public decimal LateFee { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal OwnershipShare { get; set; }
        public int StatusTypeID { get; set; }
        public int NatureOfPaymentID { get; set; }
        public int ClientPaymentTransactionID { get; set; }
        public decimal GrossShareAmount { get; set; }
        public string NatureofPaymentText { get; set; }

        public decimal ShareAmount { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int RemittanceStatusID { get; set; }
    }
}
