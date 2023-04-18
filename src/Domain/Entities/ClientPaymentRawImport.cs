using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReProServices.Domain.Entities
{
	public class ClientPaymentRawImport
    {
		[Key]
		public int ClientPaymentRawImportID { get; set; }
		public string PropertyCode { get; set; }
		public int UnitNo { get; set; }
		public DateTime DateOfPayment { get; set; }
		public DateTime RevisedDateOfPayment { get; set; }
		public string ReceiptNo { get; set; }
		public int? LotNo { get; set; }
		public string NatureOfPayment { get; set; }
		public string NotToBeConsideredReason { get; set; }
		public decimal AmountPaid { get; set; }
		public string Name { get; set; }
	}
}
