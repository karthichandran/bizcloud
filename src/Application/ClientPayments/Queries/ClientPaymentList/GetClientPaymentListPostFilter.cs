using System;
using System.Linq;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.ClientPayments.Queries.ClientPaymentList
{
    public static class GetClientPaymentReportPostFilter
    {
        public static IQueryable<ViewCustomerPropertyBasic> PostFilterPaymentsListBy(this IQueryable<ViewCustomerPropertyBasic> customerPayments,
            ClientPaymentFilter filter)
        {
            IQueryable<ViewCustomerPropertyBasic> clientPayments = customerPayments;

            if (!String.IsNullOrEmpty(filter.CustomerName))
            {
                clientPayments = clientPayments.Where(x => x.CustomerName.ToLower().Contains(filter.CustomerName.ToLower()));
            }
            
            if (!String.IsNullOrEmpty(filter.Premises))
            {
                clientPayments = clientPayments.Where(p => p.PropertyPremises.ToLower().Contains(filter.Premises.ToLower()));
            }

           
            return clientPayments;
        }
    }
} 
