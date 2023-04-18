using MediatR;
using ReProServices.Application.Common.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReProServices.Domain.Entities;
using ReProServices.Domain.Enums;

namespace ReProServices.Application.CustomerBillings.Commands
{
    public class DeleteCustomerBillingCommand : IRequest<Unit>
    {
        public long CustomerBillingID { get; set; }

        public class DeleteCustomerBillingCommandHandler : IRequestHandler<DeleteCustomerBillingCommand, Unit>
        {
            private readonly IApplicationDbContext _context;
            public DeleteCustomerBillingCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(DeleteCustomerBillingCommand request, CancellationToken cancellationToken)
            {
                var custBill = _context.CustomerBilling
                                       .First(x => x.CustomerBillID == request.CustomerBillingID);

                var customerProperty = _context.CustomerProperty
                    .Where(x => x.OwnershipID == custBill.OwnershipID).ToList();

                foreach (var entity in customerProperty)
                {
                    entity.StatusTypeId = (int)EStatusType.Saved;
                    _ = _context.CustomerProperty.Update(entity).State = EntityState.Modified;
                    _ = await _context.SaveChangesAsync(cancellationToken);
                }

                _= _context.CustomerBilling.Remove(custBill).State = EntityState.Deleted;
                _= await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
