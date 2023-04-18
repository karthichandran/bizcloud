using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Application.SellerProperties
{
    public class SellerPropertyModLog
    {
        [Key]
        [Column("SellerPropertyModLogID")]
        public int SellerPropertyModLogId { get; set; }
        [StringLength(50)]
        public string FieldName { get; set; }
        [StringLength(50)]
        public string OldName { get; set; }
        [StringLength(50)]
        public string NewValue { get; set; }
        [Column(TypeName = "date")]
        public DateTime ModDate { get; set; }
        [Column("ModByID")]
        public int ModById { get; set; }
        [Column("SellerPropertyID_FK")]
        public int? SellerPropertyIdFk { get; set; }

        //[ForeignKey(nameof(SellerPropertyIdFk))]
        //[InverseProperty(nameof(SellerPropertyDto.SellerPropertyModLog))]
        //public virtual SellerPropertyDto SellerPropertyIdFkNavigation { get; set; }
    }
}
