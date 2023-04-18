using System.Linq;

namespace ReProServices.Application.StatusReport.Queries
{
    public static class StatusReportPostQueryFilter
    {
        public static IQueryable<StatusReportDto> PostFilterReportBy(this IQueryable<StatusReportDto> report,
           StatusReportFilter filter)
        {
            IQueryable<StatusReportDto> statusReportList = report;
            if (!string.IsNullOrEmpty(filter.CustomerName))
            {
                statusReportList = statusReportList
                                   .Where(x => x.CustomerName.ToLower().Contains(filter.CustomerName.ToLower()));
            }

            return statusReportList;
        }
    }
}
