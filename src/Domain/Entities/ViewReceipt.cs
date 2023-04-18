using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
	[Table("ViewReceipt")]
	public class ViewReceipt
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
		public int UnitNo { get; set; }
		public int PropertyID { get; set; }
		public string CustomerName { get; set; }

	}
}
