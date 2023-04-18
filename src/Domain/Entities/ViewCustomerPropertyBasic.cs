using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
    [Table("ViewCustomerPropertyBasic")]
   public class ViewCustomerPropertyBasic
    {
        [Key]
        public int CustomerPropertyID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string PAN { get; set; }
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
        public string UnitStatus { get; set; }
        public decimal? StampDuty { get; set; }
        public string TracesPassword { get; set; }
        public string CustomerAlias { get; set; }
        public string IncomeTaxPassword { get; set; }
    }
}
