using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
    public class SellerModLog
    {
        [Key]
        [Column("SellerModLogID")]
        public int SellerModLogId { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ModDate { get; set; }
        public int ModById { get; set; }
       
        public int? SellerId { get; set; }

    }
}
