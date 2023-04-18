using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities

{    public class CustomerProperty
    {
        [Key]
        [Column("CustomerPropertyID")]
        public int CustomerPropertyId { get; set; }
        public decimal? CustomerShare { get; set; }
        public int CustomerId { get; set; }
        public int PropertyId { get; set; }
        public  DateTime DateOfSubmission { get; set; }
        public int? UnitNo { get; set; }
        public string Remarks { get; set; }
        public bool IsShared { get; set; }
        public int? StatusTypeId { get; set; }
        public int? PaymentMethodId { get; set; }
        public int GstRateID { get; set; }
        public int TdsRateID { get; set; }
        public decimal? TotalUnitCost { get; set; }
        public bool? TdsCollectedBySeller { get; set; }
        public string CustomerAlias { get; set; }
        public Guid? OwnershipID { get; set; }
        public bool IsPrimaryOwner { get; set; }
        public bool? IsArchived { get; set; }
        public DateTime? DateOfAgreement { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Property Property { get; set; }
        public decimal? StampDuty { get; set; }
        //public DateTime? Created { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime? Updated { get; set; }
        //public string UpdatedBy { get; set; }

    }
}
