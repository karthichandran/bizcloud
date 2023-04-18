using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Domain.Entities
{
    public class ViewPayableClientPayments
    {
        public Guid OwnershipID { get; set; }
        public DateTime? PayableDateOfPayment { get; set; }
        public string PayableReceiptNo { get; set; }
        public decimal? PayableAmountPaid { get; set; }
        public decimal? PayableGrossAmount { get; set; }
        public decimal? PayableGst { get; set; }
        public decimal? PayableTds { get; set; }
        public decimal? PayableInterest { get; set; }        
        public decimal? PayableLateFee { get; set; }

        public int UnitNo { get; set; }
    }
}
