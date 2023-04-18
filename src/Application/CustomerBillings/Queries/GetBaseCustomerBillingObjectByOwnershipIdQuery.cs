using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.CustomerBillings.Queries
{
    public class GetBaseCustomerBillingObjectByOwnershipIdQuery : IRequest<CustomerBillingDto>
    {
        public Guid OwnershipId { get; set; }
        public class GetBaseCustomerBillingObjectByOwnershipIdQueryHandler :
                              IRequestHandler<GetBaseCustomerBillingObjectByOwnershipIdQuery, CustomerBillingDto>
        {

            private readonly IApplicationDbContext _context;

            public GetBaseCustomerBillingObjectByOwnershipIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<CustomerBillingDto> Handle(GetBaseCustomerBillingObjectByOwnershipIdQuery request, CancellationToken cancellationToken)
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
                if(vm.Any())
                { return vm.First();}



               var  vmBase = await (from cp in _context.ViewCustomerPropertyBasic
                                where cp.OwnershipID == request.OwnershipId
                                select new CustomerBillingDto
                                {
                                    CustomerID = cp.CustomerID,
                                    CustomerName = cp.CustomerName,
                                    OwnershipID = cp.OwnershipID,
                                    PropertyPremises = cp.PropertyPremises,
                                    PropertyID = cp.PropertyID,
                                    UnitNo = cp.UnitNo,
                                    PaymentMethodID = (int)EPaymentMethod.Lumpsum,
                                    PAN = cp.PAN,
                                    BillDate = cp.DateOfSubmission
                                })
                             .FirstOrDefaultAsync(cancellationToken);

                var ownerCount = _context.CustomerProperty
                    .Where(x => x.OwnershipID == request.OwnershipId).ToList();

                if (ownerCount.Count > 1)
                    vmBase.CoOwner = true;
                return vmBase;
            }

        }
    }
}
