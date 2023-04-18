using System;
using System.Collections.Generic;
using System.Text;

namespace ReProServices.Application.Remittances.Queries
{
    public class RemittanceReportFilter
    {
        public int SellerID { get; set; }
        public string CustomerName { get; set; }
        public int PropertyID { get; set; } = 0;
        public int UnitNo { get; set; }
        public int LotNo { get; set; }
    }
}
