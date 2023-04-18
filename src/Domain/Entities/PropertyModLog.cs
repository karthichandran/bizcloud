using System;
using System.ComponentModel.DataAnnotations;

namespace ReProServices.Domain.Entities
{
    public class PropertyModLog
    {
        [Key]
        public int PropertyModLogId { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ModDate { get; set; }
        public int ModById { get; set; }
        public int? PropertyId { get; set; }

        //[ForeignKey(nameof(PropertyIdFk))]
        //[InverseProperty(nameof(Property.PropertyModLog))]
        public virtual Property PropertyIdFkNavigation { get; set; }
    }
}
