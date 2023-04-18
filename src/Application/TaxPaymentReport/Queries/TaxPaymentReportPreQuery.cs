using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReProServices.Application.TaxPaymentReport.Queries
{
    public static class TaxPaymentReportPreQuery
    {
        public static IQueryable<TaxPaymentDto> PreFilterReportBy(this IQueryable<TaxPaymentDto> report,
          TaxPaymentReportFilter filter)
        {
            IQueryable<TaxPaymentDto> reportList = report;
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
