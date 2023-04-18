using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReProServices.Application.PasswordSettingReport.Queries
{
    public static class TracesPasswordReportPreQuery
    {
        public static IQueryable<TracesPasswordDto> PreFilterReportBy(this IQueryable<TracesPasswordDto> report,
          TracesPasswordReportFilter filter)
        {
            IQueryable<TracesPasswordDto> reportList = report;
            if (filter.LotNo > 0)
            {
                reportList = reportList.Where(x => x.LotNumber == filter.LotNo);
            }

            if (filter.UnitNo > 0)
            {
                reportList = reportList.Where(x => x.UnitNo == filter.UnitNo);
            }
           
            if (filter.PropertyId > 0)
            {
                reportList = reportList.Where(x => x.PropertyId == filter.PropertyId);
            }
            return reportList;
        }
    }
}
