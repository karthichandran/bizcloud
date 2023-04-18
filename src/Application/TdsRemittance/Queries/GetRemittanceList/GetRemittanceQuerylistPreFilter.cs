using ReProServices.Domain.Enums;
using System;
using System.Linq;

namespace ReProServices.Application.TdsRemittance.Queries.GetRemittanceList
{
    public static class GetRemittanceQueryListPreFilter
    {
        public static IQueryable<TdsRemittanceDto> PreFilterRemittanceBy(this IQueryable<TdsRemittanceDto> remittances,
            TdsRemittanceFilter filter)
        {
            IQueryable<TdsRemittanceDto> remittanceList = remittances.AsQueryable();

            if (filter.UnitNo > 0)
            {
                remittanceList = remittanceList.Where(_ => _.UnitNo == filter.UnitNo);
            }

            if (filter.FromUnitNo > 0 && filter.ToUnitNo>0)
            {
                remittanceList = remittanceList.Where(_ => _.UnitNo >= filter.FromUnitNo && _.UnitNo <= filter.ToUnitNo);
            }

            if (filter.LotNo > 0)
            {
                remittanceList = remittanceList.Where(_ => _.LotNo == filter.LotNo);
            }

            if (filter.RemittanceStatusID.HasValue)
            {
                if (filter.RemittanceStatusID.Value == (int)ERemittanceStatus.ExcludeOnlyTDSpayments)
                {
                    remittanceList = remittanceList
                               .Where(_ => _.OnlyTDS == false);
                }
                else
                    remittanceList = remittanceList
                                .Where(_ => _.RemittanceStatusID == filter.RemittanceStatusID.Value);
            }

            if (filter.TdsAmount > 0)
            {
                remittanceList = remittanceList.Where(_ => _.TdsAmount == filter.TdsAmount);
            }

            return remittanceList;
        }
    }
}
