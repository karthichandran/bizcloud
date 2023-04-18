using MediatR;
using AutoMapper;
using ReProServices.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using NodaTime;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.CustomerBillings.Queries
{
    public class GetCustomerBillingListQuery : IRequest<IList<CustomerBillingDto>>
    {
        public CustomerBillingFilter Filter { get; set; } = new CustomerBillingFilter();
        public class GetCustomerBillingByOwnershipIdQueryHandler :
                              IRequestHandler<GetCustomerBillingListQuery, IList<CustomerBillingDto>>
        {

            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetCustomerBillingByOwnershipIdQueryHandler(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IList<CustomerBillingDto>> Handle(GetCustomerBillingListQuery request, CancellationToken cancellationToken)
            {
                var vm = await (from cb in _context.CustomerBilling
                                join cp in _context.ViewCustomerPropertyBasic on cb.OwnershipID equals cp.OwnershipID
                                where cp.OwnershipID == cb.OwnershipID
                                &&  cp.CustomerID == cb.CustomerID
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
                                    PAN = cp.PAN,
                                    CostPerInstallment = cb.CostPerInstallment,
                                    NoOfInstallments = cb.NoOfInstallments,
                                    PaymentMethodID = cb.PaymentMethodID,
                                    BillDate = cb.BillDate,
                                    PayableByText = cb.PayableBy == (int)EPayableBy.Seller ? "Seller" : "Customer",
                                    PaymentMethodText = cb.PaymentMethodID == (int)EPaymentMethod.Lumpsum ? "Lump-Sum" : "Installment"
                                })
                               .ToListAsync(cancellationToken);

                vm = vm.AsQueryable().FilterCustomerBillsBy(request.Filter).ToList();

               return vm;
            }

        }
    }
    
    
}
