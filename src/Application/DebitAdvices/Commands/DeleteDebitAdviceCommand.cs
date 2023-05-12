using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ReProServices.Application.Common.Exceptions;
using ReProServices.Application.Common.Interfaces;

namespace ReProServices.Application.DebitAdvices.Commands
{
    public class DeleteDebitAdviceCommand : IRequest<bool>
    {
        public int ClientPaymentTransactionId { get; set; }
        public class DeleteDebitAdviceCommandHandler : IRequestHandler<DeleteDebitAdviceCommand, bool>
        {
            private readonly IApplicationDbContext _context;
            public DeleteDebitAdviceCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(DeleteDebitAdviceCommand request, CancellationToken cancellationToken)
            {

                var entity = _context.DebitAdvices.Where(x => x.ClientPaymentTransactionID == request.ClientPaymentTransactionId).FirstOrDefault();

                if (entity == null)
                    return false;

                    _ = _context.DebitAdvices.Remove(entity);
                    await _context.SaveChangesAsync(cancellationToken);


                return true;
            }
        }
    }
}
