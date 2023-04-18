using System;
using System.Linq;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;

namespace ReProServices.Application.CustomerBillings.Commands
{
    public class CreateCustomerBillingCommand : IRequest<CustomerBillingDto>
    {
        public CustomerBillingDto CustomerBillingDto { get; set; }

        public class CreateCustomerBillingCommandHandler : IRequestHandler<CreateCustomerBillingCommand, CustomerBillingDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public CreateCustomerBillingCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<CustomerBillingDto> Handle(CreateCustomerBillingCommand request, CancellationToken cancellationToken)
            {
                var custBill = request.CustomerBillingDto;
                var isDuplicateBill = _context.CustomerBilling
                    .Where(x => x.OwnershipID == custBill.OwnershipID).ToList()
                    .Any();

                if (isDuplicateBill)
                {
                    throw new ApplicationException("Bill Against this Unit Already exists");
                }

                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var amount = new CustomerBillingFunctions().CalculateAmount(custBill);
                var entity = new Domain.Entities.CustomerBilling
                {
                    Amount = amount,
                    CoOwner = custBill.CoOwner,
                    GstAmount = (custBill.GstRate/100) * amount,
                    GstRate = custBill.GstRate,
                    OwnershipID = custBill.OwnershipID,
                    PayableBy = custBill.PayableBy,
                    TotalPayable = ((custBill.GstRate / 100) * amount) + amount,
                    CustomerID = custBill.CustomerID,
                    PaymentMethodID = custBill.PaymentMethodID,
                    CostPerInstallment = custBill.CostPerInstallment,
                    NoOfInstallments = custBill.NoOfInstallments,
                    BillDate = custBill.BillDate,
                    CustomerBillCreateDate = DateTime.Today,
                    Created = DateTime.Now,
                    CreatedBy = userInfo.UserID.ToString()
                };

                await _context.CustomerBilling.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                custBill.TotalPayable = entity.TotalPayable;
                custBill.GstAmount = entity.GstAmount;
                custBill.CustomerBillID = entity.CustomerBillID;
                return custBill;
            }
        }
    }
}
