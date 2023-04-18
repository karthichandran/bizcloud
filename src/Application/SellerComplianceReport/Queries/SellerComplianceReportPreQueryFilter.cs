using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ReProServices.Application.SellerComplianceReport.Queries
{
    public static class SellerComplianceReportPreQueryFilter
    {
        public static IQueryable<SellerComplianceDto> PreFilterReportBy(this IQueryable<SellerComplianceDto> report,
           SellerComplianceReportFilter filter)
        {
            IQueryable<SellerComplianceDto> reportList = report;
            if (filter.SellerID > 0)
            {
                reportList = reportList.Where(x => x.SellerID == filter.SellerID);
            }

            if (filter.PropertyID > 0)
            {
                reportList = reportList.Where(x => x.PropertyID == filter.PropertyID);
            }
            if (filter.UnitNo > 0)
            {
                reportList = reportList.Where(x => x.UnitNo == filter.UnitNo);
            }
            if (filter.LotNo > 0)
            {
                reportList = reportList.Where(x => x.LotNo == filter.LotNo);
            }
            return reportList;
        }
    }
}
