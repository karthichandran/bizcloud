 using ReProServices.Domain.Entities;
using System;
using System.Linq;

namespace ReProServices.Application.Customers.Queries
{
    public static class GetCustomersQueryFilter
    {
        public static IQueryable<ViewCustomerPropertyBasic> FilterCustomersBy(this IQueryable<ViewCustomerPropertyBasic> customers,
           CustomerDetailsFilter filter)
        {
            IQueryable<ViewCustomerPropertyBasic> customerList = customers;

            if (!String.IsNullOrEmpty(filter.CustomerName))
            {
                customerList = customerList.Where(x => x.CustomerName.ToUpper().Contains(filter.CustomerName.ToUpper()));
            }

            if (!String.IsNullOrEmpty(filter.PAN))
            {
                customerList = customerList.Where(x => x.PAN.Contains(filter.PAN));
            }

            if (filter.StatusTypeId > 0)
            {
                customerList = customerList
                    .Where(cp => cp.StatusTypeID == filter.StatusTypeId);
            }

            if (!String.IsNullOrEmpty(filter.Remarks))
            {
                customerList = customerList.Where(cp => cp.Remarks.ToUpper().Contains(filter.Remarks.ToUpper())
                                                        && !(String.IsNullOrEmpty(cp.Remarks)));
            }

            if (filter.UnitNo > 0)
            {
                customerList = customerList.Where(cp => cp.UnitNo == filter.UnitNo);

            }

            if (filter.PropertyId > 0)
            {
                customerList = customerList.Where(cp => cp.PropertyID == filter.PropertyId);
            }

            if (!String.IsNullOrEmpty( filter.Premises ))
            {
                customerList = customerList.Where(p => p.PropertyPremises.ToUpper().Contains(filter.Premises.ToUpper()));
            }
                       
            return customerList;
        }
    }
}
