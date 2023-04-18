using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
    [Table("ViewCustomerPropertyExpanded")]
    public class ViewCustomerPropertyExpanded
    {
        [Key]
        public int CustomerPropertyID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPAN { get; set; }
        public string PropertyPremises { get; set; }
        public int PropertyID { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public int UnitNo { get; set; }
        public string Remarks { get; set; }
        public int StatusTypeID { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal? TotalUnitCost { get; set; }
        public DateTime? DateOfAgreement { get; set; }
        public Guid OwnershipID { get; set; }
        public decimal? StampDuty { get; set; }


        public int PaymentMethodID { get; set; }
        public int PropertyStateID { get; set; }
        public string PaymentMethod { get; set; }
        public string PropertyPinCode { get; set; }
        public string PropertyAddressLine2 { get; set; }
        public string PropertyAddressLine1 { get; set; }
        public string PropertyCity { get; set; }
        public int PropertyType { get; set; }
        public bool TdsCollectedBySeller { get; set; }
        public int UnitTdsRateID { get; set; }
        public decimal CustomerShare { get; set; }
        public bool IsUnitShared { get; set; }
        public int UnitGstRateID { get; set; }
        public string CustomerAddressPremises { get; set; }
        public string CustomerAddressLine1 { get; set; }
        public string CustomerAddressLine2 { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerPinCode { get; set; }
        public string CustomerMobileNo { get; set; }
        public string CustomerEmailID { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CustomerStateID { get; set; }
        public string IsTracesRegistered { get; set; }
        public string TracesPassword { get; set; }
        public bool AllowForm16B { get; set; }
        public string PropertyState { get; set; }
        public string PropertyForm26BState { get; set; }
        public string CustomerState { get; set; }
        public string CustomerFrom26BState { get; set; }
        public decimal TdsInterestRate { get; set; }
        public decimal LateFeePerDay { get; set; }
        public bool? OnlyTDS { get; set; }
    }
}
