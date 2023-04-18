using System;

namespace ReProServices.Application.ClientPayments
{
	public class ClientPaymentTransactionDto
	{
		public int ClientPaymentTransactionID { get; set; }
		public int ClientPaymentID { get; set; }
		public int CustomerID { get; set; }
		public decimal CustomerShare { get; set; }
		public int SellerID { get; set; }
		public decimal SellerShare { get; set; }
		
		public decimal Gst { get; set; }
        public decimal TdsRate { get; set; }
		public decimal Tds { get; set; }
		public decimal TdsInterest { get; set; }
		public decimal LateFee { get; set; }
		public decimal GrossShareAmount { get; set; }
		public DateTime Created { get; set; }
		public string CreatedBy { get; set; }
		public Guid InstallmentID { get; set; }

		public virtual string CustomerName { get; set; }
		public virtual string SellerName { get; set; }
		public int SellerPropertyId { get; set; }
		public decimal OwnershipShare { get; set; }

        public decimal ShareAmount { get; set; }
  
		public int RemittanceStatusID { get; set; }

		public Guid OwnershipID { get; set; }

	}
}
