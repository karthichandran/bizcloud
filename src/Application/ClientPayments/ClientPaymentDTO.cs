using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReProServices.Application.ClientPayments
{
    public class ClientPaymentDto
    {
        public ClientPaymentDto()
        {
            InstallmentList = new List<ClientPaymentTransactionDto>();
        }
		[Key]
		public int ClientPaymentID { get; set; }
		//public int CustomerPropertyID { get; set; }
		public Guid OwnershipID { get; set; }
		public DateTime? DateOfPayment { get; set; }
		public DateTime? RevisedDateOfPayment { get; set; }
		public DateTime? DateOfDeduction { get; set; }
		public string ReceiptNo { get; set; }
		public int LotNo { get; set; }
		public decimal AmountPaid { get; set; }
		
		public decimal GrossAmount { get; set; }
		public DateTime Created { get; set; }
		public string CreatedBy { get; set; }
		public Guid InstallmentID { get; set; }
		public int NatureOfPaymentID { get; set; }
		//additional parameters
		public string PAN { get; set; }
		public bool? TdsCollectedBySeller { get; set; }
		
		public int StatusTypeID { get; set; }
		public int UnitNo { get; set; }
		public string PropertyPremises { get; set; }
		public bool CoOwner { get; set; }
		public decimal? LateFeePerDay { get; set; }
		public int GstTaxCode { get; set; }
		public int TdsTaxCode { get; set; }
		public string Remarks { get; set; }
		public DateTime? DateOfAgreement { get; set; }
		public int PropertyType { get; set; }
		public int PropertyID { get; set; }
		public decimal TdsRate { get; set; }
		public decimal? TotalUnitCost { get; set; }
		public decimal TdsAmount { get; set; }
		public decimal GstRate { get; set; }
		public decimal GstAmount { get; set; }
		public decimal LateFee { get; set; }
		public decimal TdsInterestAmount { get; set; }
		public string NatureOfPayment { get; set; }
		public decimal ShareAmount { get; set; }
		public decimal RoundoffAdjustment { get; set; }
		
		public IList<ClientPaymentTransactionDto> InstallmentList { get; set; }
    }
}
