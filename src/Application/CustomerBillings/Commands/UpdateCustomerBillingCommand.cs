using System.Linq;
using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System;

namespace ReProServices.Application.CustomerBillings.Commands
{
    public class UpdateCustomerBillingCommand : IRequest<CustomerBillingDto>
    {
        public CustomerBillingDto CustomerBillingDto { get; set; }

        public class UpdateCustomerBillingCommandHandler : IRequestHandler<UpdateCustomerBillingCommand, CustomerBillingDto>
        {
            private readonly IApplicationDbContext _context;
            private readonly ICurrentUserService _currentUserService;
            public UpdateCustomerBillingCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
            {
                _context = context;
                _currentUserService = currentUserService;
            }

            public async Task<CustomerBillingDto> Handle(UpdateCustomerBillingCommand request, CancellationToken cancellationToken)
            {
                var userInfo = _context.Users.FirstOrDefault(x => x.UserName == _currentUserService.UserName && x.IsActive);
                var custBill = request.CustomerBillingDto;
                var amount = new CustomerBillingFunctions().CalculateAmount(custBill);
                var entity = _context.CustomerBilling
                    .First(x => x.CustomerBillID == custBill.CustomerBillID);
                
                entity.Amount = amount;
                entity.CoOwner = custBill.CoOwner;
                entity.GstAmount = (custBill.GstRate / 100) * amount;
                entity.GstRate = custBill.GstRate;
                entity.PayableBy = custBill.PayableBy;
                entity.TotalPayable = ((custBill.GstRate / 100) * amount) + amount;
                entity.PaymentMethodID = custBill.PaymentMethodID;
                entity.CostPerInstallment = custBill.CostPerInstallment;
                entity.NoOfInstallments = custBill.NoOfInstallments;
                entity.BillDate = custBill.BillDate;
                entity.Updated = DateTime.Now;
                entity.UpdatedBy = userInfo.UserID.ToString();

                _ = _context.CustomerBilling.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
                custBill.Amount = entity.Amount;
                custBill.TotalPayable = entity.TotalPayable;
                return custBill;
            }
        }
    }
}
