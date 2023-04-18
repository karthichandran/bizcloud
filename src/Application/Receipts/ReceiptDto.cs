using AutoMapper;
using ReProServices.Application.Common.Mappings;
using System;

namespace ReProServices.Application.Receipts
{
	public class ReceiptDto : IMapFrom<Domain.Entities.Receipt>
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
		public virtual string CustomerName { get;  set; }
		public virtual int PropertyID { get; set; }
		public virtual int UnitNo { get; set; }
		public virtual int SellerID { get; set; }

		public string Premises { get; set; }
		public string Email { get; set; }

		public string ModeOfReceiptText { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<Domain.Entities.Receipt, ReceiptDto>();
		}
	}
}
