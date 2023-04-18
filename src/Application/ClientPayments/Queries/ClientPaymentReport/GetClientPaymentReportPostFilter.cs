using System;
using System.Linq;
using NodaTime;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.ClientPayments.Queries.ClientPaymentReport
{
    public static class GetClientPaymentReportPostFilter
    {
        public static IQueryable<Domain.Entities.ClientPaymentReport> PostFilterPaymentsBy(this IQueryable<Domain.Entities.ClientPaymentReport> customerPayments,
            ClientPaymentFilter filter)
        {
            IQueryable<Domain.Entities.ClientPaymentReport> clientPayments = customerPayments;


            if (filter.NatureOfPaymentID > 0)
            {
                

                if (filter.NatureOfPaymentID == 1)
                {
                    clientPayments = clientPayments.Where(_ => _.NatureOfPaymentID == (int)ENatureOfPayment.ToBeConsidered);
                }
                else
                {
                    clientPayments = clientPayments.Where(_ => _.NatureOfPaymentID != (int)ENatureOfPayment.ToBeConsidered);
                }
            }

            if (!String.IsNullOrEmpty(filter.CustomerName))
            {
                clientPayments = clientPayments.Where(x => x.CustomerName.ToLower().Contains(filter.CustomerName.ToLower()));
            }

            if (!String.IsNullOrEmpty(filter.SellerName))
            {
                clientPayments = clientPayments.Where(x => x.SellerName.ToLower().Contains(filter.SellerName.ToLower()));
            }

            if (!String.IsNullOrEmpty(filter.Premises))
            {
                clientPayments = clientPayments.Where(p => p.PropertyPremises.ToLower().Contains(filter.Premises.ToLower()));
            }

            if (filter.FromRevisedDate.HasValue)
            {
                clientPayments = clientPayments.Where(fd => LocalDate.FromDateTime(fd.RevisedDateOfPayment) >= LocalDate.FromDateTime(filter.FromRevisedDate.Value));
            }

            if (filter.ToRevisedDate.HasValue)
            {
                clientPayments = clientPayments.Where(fd => LocalDate.FromDateTime(fd.RevisedDateOfPayment) <= LocalDate.FromDateTime(filter.ToRevisedDate.Value));
            }
            return clientPayments;
        }
    }
} 
