using System;

namespace ReProServices.Domain.Entities
{
	public class ClientPaymentTransaction
	{
		public int ClientPaymentTransactionID { get; set; }
		public int ClientPaymentID { get; set; }
		public int CustomerID { get; set; }
		public decimal CustomerShare { get; set; }
		public int SellerID { get; set; }
		public decimal SellerShare { get; set; }
       
        public decimal Gst { get; set; } = 0;

		public decimal Tds { get; set; }= 0;
        public decimal TdsInterest { get; set; } = 0;
        public decimal LateFee { get; set; } = 0;
		public decimal GrossShareAmount { get; set; }
		public DateTime Created { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? Updated { get; set; }
		public string UpdatedBy { get; set; }
		public Guid InstallmentID { get; set; }
		public Guid OwnershipID { get; set; }
		public decimal ShareAmount { get; set; }
        public int RemittanceStatusID { get; set; }
	}
}
