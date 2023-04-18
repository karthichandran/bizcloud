using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.TaxPaymentReport
{
   public class TaxPaymentDto
    {
        public int LotNumber { get; set; }
        public int UnitNo { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string NameInChallan { get; set; }
        public DateTime ChallanPaymentDate { get; set; }
        public string ChallanSerialNo { get; set; }
        public string ChallanIncomeTaxAmount{ get; set; }
        public string ChallanInterestAmount { get; set; }
        public string ChallanFeeAmount { get; set; }
        public string ChallanTotalAmount { get; set; }
        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
    }
}
