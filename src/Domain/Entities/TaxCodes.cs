using System;
using System.ComponentModel.DataAnnotations;

namespace ReProServices.Domain.Entities
{
    public class TaxCodes
    {
        [Key]
        public int TaxID { get; set; }
        public int TaxCodeId { get; set; }
        public string TaxName { get; set; }
        public DateTime EffectiveStartDate { get; set; }
        public DateTime EffectiveEndDate { get; set; }
        public decimal TaxValue { get; set; }
        public int? TaxTypeId { get; set; }
       
        public virtual TaxType TaxTypeTable { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
    }
}
