using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReProServices.Domain.Entities
{
	public class ViewClientPayment
	{
		[Key]
		public int ClientPaymentID { get; set; }
		//public int CustomerPropertyID { get; set; }
		public Guid OwnershipID { get; set; }
		public DateTime DateOfPayment { get; set; }
		public DateTime RevisedDateOfPayment { get; set; }
		public DateTime DateOfDeduction { get; set; }
		public string ReceiptNo { get; set; }
		public int LotNo { get; set; }
		public decimal AmountPaid { get; set; }
		public Guid InstallmentID { get; set; }
		public int NatureOfPaymentID { get; set; }
        public decimal GstRate { get; set; }
        public decimal TdsRate { get; set; }
		public string NatureOfPaymentText { get; set; }
	}
}
