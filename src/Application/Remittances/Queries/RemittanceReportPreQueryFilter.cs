using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ReProServices.Application.TdsRemittance;

namespace ReProServices.Application.Remittances.Queries
{
   public static class RemittanceReportPreQueryFilter
    {
        public static IQueryable<RemittanceReportDto> PreFilterReportBy(this IQueryable<RemittanceReportDto> report,
           RemittanceReportFilter filter)
        {
            IQueryable<RemittanceReportDto> reportList = report;
            
            //if (filter.PropertyID > 0)
            //{
            //    reportList = reportList.Where(x => x.PropertyID == filter.PropertyID);
            //}
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
