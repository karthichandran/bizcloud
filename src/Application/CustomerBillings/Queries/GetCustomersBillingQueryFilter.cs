using System;
using System.Linq;
using NodaTime;
namespace ReProServices.Application.CustomerBillings.Queries
{
    public static class GetCustomersBillingQueryFilter
    {
        public static IQueryable<CustomerBillingDto> FilterCustomerBillsBy(this IQueryable<CustomerBillingDto> customerBillsDtos,
           CustomerBillingFilter filter)
        {
            IQueryable<CustomerBillingDto> customerBills = customerBillsDtos;

            if (!String.IsNullOrEmpty(filter.CustomerName))
            {
                customerBills = customerBills.Where(x => x.CustomerName.ToLower().Contains(filter.CustomerName.ToLower()));
            }

            if (!String.IsNullOrEmpty(filter.PAN))
            {
                customerBills = customerBills.Where(x => x.PAN == filter.PAN);
            }
                    
            if (filter.UnitNo > 0)
            {
                customerBills = customerBills.Where(cp => cp.UnitNo == filter.UnitNo);
            }

            if (filter.PropertyID > 0)
            {
                customerBills = customerBills.Where(cp => cp.PropertyID == filter.PropertyID);
            }

            if (!String.IsNullOrEmpty( filter.Premises ))
            {
                customerBills = customerBills.Where(p => p.PropertyPremises.ToLower().Contains(filter.Premises.ToLower()));
            }

            if (filter.FromDate.HasValue)
            {
                customerBills = customerBills.Where(fd => LocalDate.FromDateTime(fd.BillDate) >= LocalDate.FromDateTime(filter.FromDate.Value));
            }

            if (filter.ToDate.HasValue)
            {
                customerBills = customerBills.Where(fd => LocalDate.FromDateTime(fd.BillDate) <= LocalDate.FromDateTime(filter.ToDate.Value));
            }

            return customerBills;
        }
    }
} 
