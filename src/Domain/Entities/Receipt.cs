using System;
namespace ReProServices.Domain.Entities
{
	public class Receipt
	{
		public int ReceiptID { get; set; }
		public int ReceiptType { get; set; }
		public int ClientPaymentTransactionID { get; set; }
		public int CustomerBillingID { get; set; }
		public int LotNo { get; set; }
		public decimal ServiceFee { get; set; }
		public decimal GstPayable { get; set; }
		public decimal Tds { get; set; }
		public decimal TdsInterest { get; set; }
		public decimal LateFee { get; set; }
		public DateTime DateOfReceipt { get; set; }
		public int? ModeOfReceiptID { get; set; }
		public string ReferenceNo { get; set; }
		public decimal TdsReceived { get; set; }
		public decimal TotalServiceFeeReceived { get; set; }
		public decimal TdsInterestReceived { get; set; }
		public decimal LateFeeReceived { get; set; }
		public bool? EmailSent { get; set; }
		public DateTime? EmailSentDate { get; set; }
		public Guid OwnershipID { get; set; }
		public int CustomerID { get; set; }
		public DateTime? Created { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? Updated { get; set; }
		public string UpdatedBy { get; set; }

	}
}
