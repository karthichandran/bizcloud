using System.Linq;
using ReProServices.Domain.Entities;

namespace ReProServices.Application.ClientPayments.Queries.ClientPaymentList
{
    public static class GetClientPaymentListPreFilter
    {
        public static IQueryable<ViewCustomerPropertyBasic> PreFilterPaymentsListBy(this IQueryable<ViewCustomerPropertyBasic> customerPayments,
            ClientPaymentFilter filter)
        {
            IQueryable<ViewCustomerPropertyBasic> clientPayments = customerPayments;

            

            if (filter.UnitNo > 0)
            {
                clientPayments = clientPayments.Where(cp => cp.UnitNo == filter.UnitNo);
            }

            if (filter.PropertyID > 0)
            {
                clientPayments = clientPayments.Where(cp => cp.PropertyID == filter.PropertyID);
            }
            
            return clientPayments;
        }
    }
} 
