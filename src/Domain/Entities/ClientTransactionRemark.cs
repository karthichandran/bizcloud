using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReProServices.Domain.Entities
{
   public class ClientTransactionRemark
    {
        [Key]
        public int ClTransRemarkId { get; set; }
        public int ClientPaymentTransactionId { get; set; }
        public int RemittanceRemarkId { get; set; }
        public int TracesRemarkId { get; set; }
    }
}
