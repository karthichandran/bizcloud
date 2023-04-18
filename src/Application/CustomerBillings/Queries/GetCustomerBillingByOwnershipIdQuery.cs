using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.CustomerBillings.Queries
{
    public class GetCustomerBillingByOwnershipIdQuery : IRequest<List<CustomerBillingDto>>
    {
        public Guid OwnershipId { get; set; }
        public class GetCustomerBillingByOwnershipIdQueryHandler :
                              IRequestHandler<GetCustomerBillingByOwnershipIdQuery, List<CustomerBillingDto>>
        {

            private readonly IApplicationDbContext _context;

            public GetCustomerBillingByOwnershipIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<List<CustomerBillingDto>> Handle(GetCustomerBillingByOwnershipIdQuery request, CancellationToken cancellationToken)
            {
                var vm = await (from cb in _context.CustomerBilling
                                join cp in _context.ViewCustomerPropertyBasic on cb.OwnershipID equals cp.OwnershipID
                                where cb.OwnershipID == request.OwnershipId
                                && cb.CustomerID == cp.CustomerID
                                select new CustomerBillingDto
                                {
                                  CustomerID = cp.CustomerID,
                                  Amount = cb.Amount,
                                  CoOwner = cb.CoOwner,
                                  CustomerBillID = cb.CustomerBillID,
                                  CustomerName = cp.CustomerName,
                                  GstAmount = cb.GstAmount,
                                  GstRate = cb.GstRate,
                                  OwnershipID = cb.OwnershipID,
                                  PayableBy = cb.PayableBy,
                                  PropertyPremises = cp.PropertyPremises,
                                  PropertyID = cp.PropertyID,
                                  TotalPayable = cb.TotalPayable,
                                  UnitNo = cp.UnitNo,
                                  CostPerInstallment = cb.CostPerInstallment,
                                  NoOfInstallments = cb.NoOfInstallments,
                                  PAN = cp.PAN,
                                  PaymentMethodID = cb.PaymentMethodID,
                                  BillDate = cb.BillDate
                                })
                               .ToListAsync(cancellationToken);

                return vm;
            }

        }
    }
}
