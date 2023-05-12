using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Domain.Entities
{
    public class DebitAdvice
    {
        public int DebitAdviceID { get; set; }
        public int ClientPaymentTransactionID { get; set; }
        public string CinNo { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? BlobId { get; set; }

    }
}
