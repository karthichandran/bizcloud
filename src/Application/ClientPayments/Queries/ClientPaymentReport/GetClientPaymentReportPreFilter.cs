using System.Linq;

namespace ReProServices.Application.ClientPayments.Queries.ClientPaymentReport
{
    public static class GetClientPaymentReportPreFilter
    {
        public static IQueryable<Domain.Entities.ClientPaymentReport> PreFilterPaymentsBy(this IQueryable<Domain.Entities.ClientPaymentReport> customerPayments,
            ClientPaymentFilter filter)
        {
            IQueryable<Domain.Entities.ClientPaymentReport> clientPayments = customerPayments;

            if (filter.LotNo > 0)
            {
                clientPayments = clientPayments.Where(cp => cp.LotNo == filter.LotNo);
            }

            if (filter.RemittanceStatusID.HasValue)
            {
                clientPayments = clientPayments.Where(cp => cp.RemittanceStatusID == filter.RemittanceStatusID.Value);
            }

            if (filter.UnitNo > 0)
            {
                clientPayments = clientPayments.Where(cp => cp.UnitNo == filter.UnitNo);
            }

            if (filter.SellerID > 0)
            {
                clientPayments = clientPayments.Where(cp => cp.SellerID == filter.SellerID);
            }
            
            if (filter.PropertyID > 0)
            {
                clientPayments = clientPayments.Where(cp => cp.PropertyID == filter.PropertyID);
            }
         
            return clientPayments;
        }
    }
} 
