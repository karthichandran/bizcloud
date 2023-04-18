using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReProServices.Domain.Entities
{
	public class ViewClientPaymentImports
	{
		public string PropertyCode { get; set; }
		public Guid OwnershipID { get; set; }
		public DateTime? DateOfAgreement { get; set; }
		public DateTime DateOfPayment { get; set; }
		public DateTime RevisedDateOfPayment { get; set; }
		public DateTime DateOfDeduction { get; set; }
		public string ReceiptNo { get; set; }
		public int PropertyID { get; set; }
		public int CustomerPropertyID { get; set; }
		public int CustomerID { get; set; }
		public int LotNo { get; set; }
		public int UnitNo { get; set; }
		public int PaymentMethodID { get; set; }
		public decimal AmountPaid { get; set; }
		public decimal CustomerShare { get; set; }
		public decimal SellerShare { get; set; }
		public Guid InstallmentID { get; set; }
		public int NatureOfPaymentID { get; set; }
		public bool TdsCollectedBySeller { get; set; }
        public int GstRateID { get; set; }
        public int TdsRateID { get; set; }
		public string NatureOfPayment { get; set; }
		public string CustomerPAN { get; set; }
		public string NotToBeConsideredReason { get; set; }
		public string CustomerAlias { get; set; }
		public string CustomerName { get; set; }
		public int SellerID { get; set; } 
		public int SellerPropertyID { get; set; }
		public int StatusTypeID { get; set; }
		public int PropertyType { get; set; }
		public string SellerName { get; set; }
		public string PropertyPremises { get; set; }
		public string Remarks { get; set; }
		public decimal ReceiptTotal { get; set; }
		public decimal? TotalUnitCost { get; set; }
		public decimal LateFeePerDay { get; set; }
		public bool CoOwner { get; set; }
	}
}
