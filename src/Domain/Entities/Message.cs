using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReProServices.Domain.Entities
{
    public class Message
    {
        [Key]
        [Column("MessageID")]
        public int MessageID { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
        public bool? Verified { get; set; }
        public int? Lane { get; set; }
        public int Otp { get; set; }
    }
}
