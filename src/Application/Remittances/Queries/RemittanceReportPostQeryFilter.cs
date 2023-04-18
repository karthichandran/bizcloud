using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ReProServices.Application.TdsRemittance;

namespace ReProServices.Application.Remittances.Queries
{
    public static class RemittanceReportPostQeryFilter
    {
        public static IQueryable<RemittanceReportDto> PostFilterReportBy(this IQueryable<RemittanceReportDto> report,
          RemittanceReportFilter filter)
        {
            IQueryable<RemittanceReportDto> remittanceList = report;
            if (!string.IsNullOrEmpty(filter.CustomerName))
            {
                remittanceList = remittanceList.Where(x => x.CustomerName.ToLower().Contains(filter.CustomerName.ToLower()));
            }

            return remittanceList;
        }
    }
}
