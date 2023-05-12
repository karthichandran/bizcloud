using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.DebitAdvices
{
   public class DebitAdviceDto
    {
        public int DebitAdviceID { get; set; }
        public int ClientPaymentTransactionID { get; set; }
        public string CinNo { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? BlobId { get; set; }
    }
}
