using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
	[Table("ViewClientPaymentReport")]
	public class ViewClientPaymentReport
    {
		public int LotNo { get; set; }
		public Guid OwnershipID { get; set; }
		public string CustomerName { get; set; }
		public string PropertyPremises { get; set; }
		public int UnitNo { get; set; }
		public DateTime? DateOfBooking { get; set; }
		public decimal? TotalUnitCost { get; set; }
		public DateTime DateOfPayment { get; set; }
		public DateTime RevisedDateOfPayment { get; set; }
		public DateTime DateOfDeduction { get; set; }
		public string ReceiptNo { get; set; }
		public decimal ShareAmountPaid { get; set; }
		public string SellerName { get; set; }
		public Guid InstallmentID { get; set; }
		public string NatureOfPaymentText { get; set; }
		public int NatureOfPaymentID { get; set; }
		public decimal GstRate { get; set; }
		public decimal Gst { get; set; } = 0;
		public decimal TdsRate { get; set; }
		public decimal Tds { get; set; } = 0;
		public decimal TdsInterest { get; set; } = 0;
		public decimal LateFee { get; set; } = 0;
		public decimal GrossShareAmount { get; set; }
		public int ClientPaymentTransactionID { get; set; }
		public int PropertyID { get; set; }
		public int SellerID { get; set; }

		public int RemittanceStatusID { get; set; }
		public string RemittanceStatus { get; set; }

		public DateTime? ChallanDate { get; set; }
	}
}
