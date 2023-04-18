using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.Prospect
{
   public class ProspectProcessDto
    {
        public int ProspectPropertyID { get; set; }
        public int? PaymentMethodId { get; set; }
        public int GstRateID { get; set; }
        public int TdsRateID { get; set; }
        public decimal? TotalUnitCost { get; set; }
        public bool? TdsCollectedBySeller { get; set; }
        public DateTime? DateOfAgreement { get; set; }
    }
}
