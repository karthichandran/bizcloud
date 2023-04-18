using System;

namespace ReProServices.Domain.Entities
{ 
    public class CustomerPropertyModLog
    {
        public int CustomerPropertyModLogId { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ModDate { get; set; }
        public int ModById { get; set; }
        public int? CustomerPropertyId { get; set; }
    }
}
