using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ReProServices.Application.SellerComplianceReport.Queries
{
    public static class SellerComplianceReportPostQueryFilter
    {
        public static IQueryable<SellerComplianceDto> PostFilterReportBy(this IQueryable<SellerComplianceDto> report,
          SellerComplianceReportFilter filter)
        {
            IQueryable<SellerComplianceDto> sellerCompliancetList = report;
            if (!string.IsNullOrEmpty(filter.CustomerName))
            {
                sellerCompliancetList = sellerCompliancetList.Where(x => x.CustomerName.ToLower().Contains(filter.CustomerName.ToLower()));
            }

            return sellerCompliancetList;
        }
    }
}
