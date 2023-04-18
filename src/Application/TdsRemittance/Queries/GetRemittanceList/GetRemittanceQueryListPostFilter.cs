using System;
using System.Linq;

namespace ReProServices.Application.TdsRemittance.Queries.GetRemittanceList
{
    public static class GetRemittanceQueryListPostFilter
    {
        public static IQueryable<TdsRemittanceDto> PostFilterRemittanceBy(this IQueryable<TdsRemittanceDto> remittances,
            TdsRemittanceFilter filter)
        {
            IQueryable<TdsRemittanceDto> remittanceList = remittances.AsQueryable();

            if (!String.IsNullOrEmpty(filter.CustomerName))
            {
                remittanceList = remittanceList.Where(_ => _.CustomerName.ToLower()
                                               .Contains(filter.CustomerName.ToLower()));
            }

            if (!String.IsNullOrEmpty( filter.PropertyPremises ))
            {
                remittanceList = remittanceList.Where(_ => _.PropertyPremises.Trim().ToLower()
                                               .Contains(filter.PropertyPremises.Trim().ToLower()));
            }
                       
            return remittanceList;
        }
    }
}
