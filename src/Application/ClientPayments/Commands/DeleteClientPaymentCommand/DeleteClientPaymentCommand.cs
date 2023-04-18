using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.ClientPayments.Commands.DeleteClientPaymentCommand
{
    public class DeleteClientPaymentCommand:IRequest<Unit>
    {
        public Guid InstallmentID { get; set; }

        public class DeleteClientPaymentCommandHandler : IRequestHandler<DeleteClientPaymentCommand, Unit> {
            private readonly IApplicationDbContext _context;
            public DeleteClientPaymentCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<Unit> Handle(DeleteClientPaymentCommand request, CancellationToken cancellationToken) {

                var transactions = _context.ClientPaymentTransaction.Where(x => x.InstallmentID == request.InstallmentID).ToList();
                foreach (var trans in transactions)
                {
                    _context.ClientPaymentTransaction.Remove(trans);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                var payment = _context.ClientPayment.First(x => x.InstallmentID == request.InstallmentID);
                _context.ClientPayment.Remove(payment);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }

    }
}
