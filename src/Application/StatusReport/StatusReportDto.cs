using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.StatusReport
{
   public class StatusReportDto
    {
        public int PropertyID { get; set; }
        public string Premises { get; set; }
        public string CustomerName { get; set; }

        public int? UnitNo { get; set; }
        public int? LotNo { get; set; }
        public DateTime? PaymentReceiptDate { get; set; }
        public decimal? RemittanceOfTdsAmount { get; set; }
        public DateTime? Form16BRequested { get; set; }
        public DateTime? Form16BDownloaded { get; set; }
        public DateTime? MailDate { get; set; }
    }
}
