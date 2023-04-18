using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.SellerComplianceReport
{
    public class SellerComplianceDto
    {
        public int PropertyID { get; set; }
        public int SellerID { get; set; }
        public string CustomerName { get; set; }
        public string SellerName { get; set; }
        public string Premises { get; set; }
        public int? UnitNo { get; set; }
        public int? LotNo { get; set; }
        public DateTime? TdsCertificateDate { get; set; }
        public string TdsCertificateNo { get; set; }
        public decimal? Amount { get; set; }
        public string Form16BFileName { get; set; }

    }
}
