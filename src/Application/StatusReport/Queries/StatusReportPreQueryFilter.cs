using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReProServices.Application.StatusReport.Queries
{
   public static class StatusReportPreQueryFilter
    {
        public static IQueryable<StatusReportDto> PreFilterReportBy(this IQueryable<StatusReportDto> report,
            StatusReportFilter filter)
        {
            IQueryable<StatusReportDto> reportList = report;
            if (filter.PropertyID > 0)
            {
                reportList = reportList.Where(x => x.PropertyID == filter.PropertyID);
            }
            if (filter.UnitNo > 0)
            {
                reportList = reportList.Where(x => x.UnitNo == filter.UnitNo);
            }
            if (filter.LotNo > 0) {
                reportList = reportList.Where(x => x.LotNo == filter.LotNo);
            }
            return reportList;
        }
    }
}
