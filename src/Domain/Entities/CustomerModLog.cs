using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
    public class CustomerModLog
    {
        [Key]
        [Column("CustomerLogID")]
        public int CustomerLogId { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ModDate { get; set; }
        public int ModById { get; set; }
        public int? CustomerId { get; set; }
       
    }
}
