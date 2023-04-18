using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReProServices.Application.Prospect.Queries
{
    public static class GetProspectQueryFilter
    {
        public static IQueryable<ProspectListVM> PreFilter(this IQueryable<ProspectListVM> prospectListVM,
           ProspectFilter filter)
        {
            IQueryable<ProspectListVM> prospectList = prospectListVM;
           
            if (filter.UnitNo > 0)
            {
                prospectList = prospectList.Where(x => x.unitNo == filter.UnitNo);
            }
            if (!string.IsNullOrEmpty(filter.Customer))
            {
                prospectList = prospectList.Where(x => x.name.ToLower().Contains( filter.Customer.ToLower()));
            }
            return prospectList;
        }
    }
}
