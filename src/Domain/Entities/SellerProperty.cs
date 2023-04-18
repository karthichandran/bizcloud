using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
    public  class SellerProperty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SellerPropertyID { get; set; }
        public decimal SellerShare { get; set; }
        public int PropertyID { get; set; }
        public int SellerID { get; set; }
        public bool? isSharedSeller { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Updated { get; set; }
        public string UpdatedBy { get; set; }
        public virtual Seller Seller { get; set; }
        public virtual Property Property { get; set; }
    }
}
