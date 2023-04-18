using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.CustomerBillings.Queries
{
    public class GetCustomerBillByBillingIdQuery : IRequest<CustomerBillingDto>
    {
        public int CustomerBillID { get; set; }
        public class GetCustomerBillByBillingIdQueryHandler : IRequestHandler<GetCustomerBillByBillingIdQuery, CustomerBillingDto>
        {
            private readonly IApplicationDbContext _context;

            public GetCustomerBillByBillingIdQueryHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<CustomerBillingDto> Handle(GetCustomerBillByBillingIdQuery request, CancellationToken cancellationToken)
            {
                var vm = await (from cb in _context.CustomerBilling
                                join cp in _context.ViewCustomerPropertyBasic on cb.OwnershipID equals cp.OwnershipID
                                where cb.CustomerBillID == request.CustomerBillID
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
                               .FirstOrDefaultAsync(cancellationToken);
                return vm;
            }

        }
    }
}
